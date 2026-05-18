# Магазин сувенирных чёток

**Тема:** Разработка веб-приложения для продажи сувенирных товаров со встроенным интеллектуальным поиском

**Дисциплина:** «Кроссплатформенная среда исполнения программного обеспечения»  
**Кафедра КБ-4** — «Интеллектуальные системы информационной безопасности», РТУ МИРЭА

---

## Описание проекта

Полноценный интернет-магазин сувенирных чёток, разработанный с использованием ASP.NET Core 9, Blazor Server и Entity Framework Core. Приложение включает:

- **Каталог** из 96 товаров в 10 категориях (религиозные, деревянные, янтарные, каменные, коралловые, хрустальные, металлические, антистресс, коллекционные, роскошные)
- **Корзину**, **избранное** и **заказы** с сессионным хранением
- **Интеллектуальную систему рекомендаций** на основе истории покупок, сохранений и просмотров
- **Фильтрацию и поиск** по названию, категории, материалу и цене
- **REST API** с Swagger-документацией
- **Контейнеризацию** через Docker / docker-compose

---

## Стек технологий

| Компонент | Технология |
|-----------|-----------|
| Фреймворк | .NET 9, ASP.NET Core |
| UI | Blazor Server |
| БД | SQLite + Entity Framework Core 9 (Code First) |
| API | Minimal API + Swagger (Swashbuckle) |
| Контейнеры | Docker + docker-compose |
| Стилизация | Bootstrap 5, Bootstrap Icons, кастомный CSS |

---

## Архитектура

```
practos3/
├── Components/          # Blazor-компоненты
│   ├── App.razor
│   ├── Routes.razor
│   ├── Layout/          # MainLayout, NavMenu
│   ├── Pages/           # Home, Catalog, ProductDetail, Cart, Favorites, Orders
│   └── Shared/          # ProductCard, RecommendationPanel
├── Data/
│   └── ShopContext.cs   # DbContext (EF Core)
├── Migrations/          # EF Core миграции
├── Models/              # Chetkas, Category, CartItem, FavoriteItem, Order, OrderItem, ProductView + DTOs
├── Services/            # Бизнес-логика
│   ├── BeadService      # CRUD товаров
│   ├── CartService      # Корзина
│   ├── FavoriteService  # Избранное
│   ├── OrderService     # Заказы
│   ├── RecommendationService  # ИИ-рекомендации
│   ├── SessionService   # Управление сессией
│   └── DataSeeder       # Начальное заполнение БД
├── wwwroot/css/app.css  # Стили
├── Dockerfile
├── docker-compose.yml
└── Program.cs
```

---

## Установка и запуск

### Локальный запуск

```bash
# Клонировать репозиторий
git clone https://github.com/khachaturov/bead-shop
cd bead-shop

# Восстановить зависимости
dotnet restore

# Применить миграции и запустить (сидирование БД происходит автоматически)
dotnet run
```

Приложение доступно по адресу: `http://localhost:5000`  
Swagger UI: `http://localhost:5000/swagger`

### Запуск через Docker

```bash
# Скопировать переменные окружения
cp .env.example .env

# Собрать образ
docker build -t khachaturov/practos3:latest .

# Запустить через compose
docker-compose up -d
```

Приложение доступно по адресу: `http://localhost:8089`

---

## API Endpoints

| Метод | URL | Описание |
|-------|-----|----------|
| GET | `/api/products` | Список всех товаров |
| GET | `/api/products/{id}` | Товар по ID |
| GET | `/api/categories` | Список категорий |
| GET | `/api/products/by-category/{id}` | Товары категории |
| POST | `/api/products` | Добавить товар |
| POST | `/api/categories` | Добавить категорию |
| GET | `/api/config` | Конфигурация приложения |
| GET | `/health` | Health check |
| GET | `/swagger` | Swagger UI |

---

## Интеллектуальная система рекомендаций

Рекомендательная система реализует **контентную фильтрацию** на основе истории взаимодействий пользователя:

**Веса взаимодействий:**
- Покупка товара — **5.0**
- Добавление в избранное — **3.0**
- Просмотр товара — **1.0 × e^(−0.1 × дней)** (экспоненциальное затухание)

**Алгоритм оценки:**
```
score = catPrefs[category] × 2.0
      + matPrefs[material] × 1.5
      + (1 − |price − avgPrice| / avgPrice) × 0.5
```

**Результат:** Система строит профиль предпочтений пользователя по категориям, материалам и ценовому диапазону, затем ранжирует доступные товары по степени соответствия профилю.

---

## Docker Hub

```
docker pull khachaturov/practos3:latest
```

Репозиторий: `https://hub.docker.com/r/khachaturov/practos3`

---

## Репозиторий

`https://github.com/khachaturov/bead-shop`
