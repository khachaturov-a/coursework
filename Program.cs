using Microsoft.EntityFrameworkCore;
using Practos3.Components;
using Practos3.Data;
using Practos3.Models;
using Practos3.Services;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
    ?? throw new InvalidOperationException("Строка подключения 'DefaultConnection' не найдена");

// --- EF Core ---
builder.Services.AddDbContext<ShopContext>(options =>
    options.UseSqlite(connectionString),
    ServiceLifetime.Scoped);

// --- Blazor ---
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

// --- Бизнес-сервисы ---
builder.Services.AddScoped<IBeadService, BeadService>();
builder.Services.AddScoped<ICartService, CartService>();
builder.Services.AddScoped<IFavoriteService, FavoriteService>();
builder.Services.AddScoped<IOrderService, OrderService>();
builder.Services.AddScoped<IRecommendationService, RecommendationService>();
builder.Services.AddScoped<SessionService>();

// --- REST API / Swagger ---
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
    c.SwaggerDoc("v1", new() { Title = "Магазин чёток API", Version = "v1" }));

builder.Services.AddHealthChecks();

var app = builder.Build();

// --- Миграции + сидирование ---
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<ShopContext>();
    db.Database.Migrate();
    await DataSeeder.SeedAsync(db);
}

// --- Middleware ---
app.UseSwagger();
app.UseSwaggerUI();
app.UseStaticFiles();
app.UseAntiforgery();

// --- Blazor ---
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

// --- REST API endpoints ---
app.MapGet("/api/products", async (IBeadService service, IConfiguration config) =>
{
    var maxItems = int.TryParse(config["AppSettings:MaxItems"], out var max) ? max : 50;
    var items = (await service.GetAllAsync(maxItems)).ToList();
    return Results.Ok(new ProductsResponse(items.Count, items));
})
.WithName("GetProducts")
.WithSummary("Получить список всех чёток")
.Produces<ProductsResponse>();

app.MapGet("/api/products/{id:int}", async (int id, ShopContext db) =>
{
    var item = await db.Chetkas
        .Include(c => c.Category)
        .FirstOrDefaultAsync(c => c.Id == id);

    if (item is null)
        return Results.NotFound(new { message = $"Товар с id={id} не найден" });

    return Results.Ok(new ChetkasDto(
        item.Id, item.Name, item.Price, item.StockQuantity,
        item.Material, item.Category?.Name ?? "—", item.Description, item.CategoryId));
})
.WithName("GetProductById")
.WithSummary("Получить чётки по id")
.Produces<ChetkasDto>().Produces(404);

app.MapGet("/api/categories", async (IBeadService service) =>
    Results.Ok(await service.GetCategoriesAsync()))
.WithName("GetCategories")
.WithSummary("Получить список категорий")
.Produces<IEnumerable<CategoryDto>>();

app.MapGet("/api/products/by-category/{categoryId:int}", async (int categoryId, IBeadService service, IConfiguration config) =>
{
    var maxItems = int.TryParse(config["AppSettings:MaxItems"], out var max) ? max : 50;
    return Results.Ok(await service.GetByCategoryAsync(categoryId, maxItems));
})
.WithName("GetProductsByCategory")
.WithSummary("Получить чётки по категории")
.Produces<IEnumerable<ChetkasDto>>();

app.MapPost("/api/categories", async (Category newCategory, IBeadService service, ShopContext db) =>
{
    if (await db.Categories.AnyAsync(c => c.Name == newCategory.Name))
        return Results.BadRequest(new { message = $"Категория '{newCategory.Name}' уже существует" });

    return Results.Created($"/api/categories/{(await service.CreateCategoryAsync(newCategory)).Id}",
        await service.CreateCategoryAsync(newCategory));
})
.WithName("CreateCategory")
.WithSummary("Добавить новую категорию")
.Produces<CategoryDto>(201).Produces(400);

app.MapPost("/api/products", async (Chetkas newItem, IBeadService service, ShopContext db) =>
{
    if (!await db.Categories.AnyAsync(c => c.Id == newItem.CategoryId))
        return Results.BadRequest(new { message = $"Категория id={newItem.CategoryId} не существует" });

    return Results.Created($"/api/products/{(await service.CreateAsync(newItem)).Id}",
        await service.CreateAsync(newItem));
})
.WithName("CreateProduct")
.WithSummary("Добавить новый товар")
.Produces<ChetkasDto>(201).Produces(400);

app.MapGet("/api/config", (IConfiguration config) => Results.Ok(new
{
    appName  = config["AppSettings:AppName"],
    version  = config["AppSettings:Version"],
    maxItems = config["AppSettings:MaxItems"],
    dbSource = config.GetConnectionString("DefaultConnection")
})).WithName("GetConfig").WithSummary("Конфигурация приложения").Produces<object>();

app.MapHealthChecks("/health");

app.Run();
