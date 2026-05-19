# Магазин сувенирных чёток

**Тема:** Разработка веб-приложения для продажи сувенирных товаров со встроенным интеллектуальным поиском  
**Дисциплина:** «Кроссплатформенная среда исполнения программного обеспечения»  
**Кафедра КБ-4** — «Интеллектуальные системы информационной безопасности», РТУ МИРЭА

---

## Репозиторий и образ

| Ресурс | Ссылка |
|--------|--------|
| GitHub | `https://github.com/khachaturov-a/coursework` |
| Docker Hub | `https://hub.docker.com/r/khachaturov/coursework` |

---

## Описание проекта

Полноценный интернет-магазин сувенирных чёток, разработанный на ASP.NET Core 9 + Blazor Server + Entity Framework Core. Приложение включает:

- **Каталог** — 50 товаров в 10 категориях: религиозные, деревянные, янтарные, каменные, коралловые, хрустальные, металлические, антистресс, коллекционные, роскошные
- **Корзину**, **избранное** и **заказы** с хранением по сессии
- **Интеллектуальную систему рекомендаций** на основе истории просмотров, избранного и покупок
- **Фильтрацию и поиск** по названию, категории, материалу, цене
- **REST API** с Swagger-документацией
- **Тёмную / светлую тему**
- **Контейнеризацию** через Docker + docker-compose

---

## Стек технологий

| Компонент | Технология |
|-----------|------------|
| Платформа | .NET 9, ASP.NET Core 9 |
| UI | Blazor Server (Interactive Server) |
| База данных | SQLite + Entity Framework Core 9 (Code First) |
| Миграции | EF Core Migrations |
| API | ASP.NET Core Minimal API |
| Документация API | Swashbuckle / Swagger UI |
| Валидация | FluentValidation 11 |
| Стилизация | Bootstrap 5.3, Bootstrap Icons 1.11, кастомный CSS |
| Контейнеры | Docker (multi-stage build) + docker-compose |

---

## Архитектура проекта

```
bead-shop/
├── Components/                   # Blazor-компоненты
│   ├── App.razor                 # Корневой компонент
│   ├── Routes.razor
│   ├── _Imports.razor
│   ├── Layout/
│   │   ├── MainLayout.razor      # Основной layout (header, footer)
│   │   └── NavMenu.razor         # Навигация с поиском
│   ├── Pages/
│   │   ├── Home.razor            # Главная (hero, категории, рекомендации)
│   │   ├── Catalog.razor         # Каталог с фильтрацией и сортировкой
│   │   ├── ProductDetail.razor   # Карточка товара
│   │   ├── Cart.razor            # Корзина и оформление заказа
│   │   ├── Favorites.razor       # Избранное
│   │   ├── Orders.razor          # История заказов
│   │   └── Quiz.razor            # Тест для подбора чёток
│   └── Shared/
│       ├── ProductCard.razor     # Карточка товара в сетке
│       └── RecommendationPanel.razor
├── Data/
│   └── ShopContext.cs            # DbContext (EF Core, Fluent API)
├── Migrations/                   # EF Core миграции
├── Models/                       # Сущности и DTO
│   ├── Chetkas.cs                # Товар
│   ├── Category.cs               # Категория
│   ├── CartItem.cs               # Элемент корзины
│   ├── FavoriteItem.cs           # Избранное
│   ├── Order.cs / OrderItem.cs   # Заказ / позиция заказа
│   ├── ProductView.cs            # История просмотров
│   └── Dtos.cs                   # DTO для API и рекомендаций
├── Services/
│   ├── BeadService.cs            # CRUD товаров
│   ├── CartService.cs            # Корзина
│   ├── FavoriteService.cs        # Избранное
│   ├── OrderService.cs           # Заказы
│   ├── RecommendationService.cs  # Система рекомендаций
│   ├── SessionService.cs         # Сессии пользователей
│   └── DataSeeder.cs             # Начальное заполнение БД
├── wwwroot/
│   ├── css/app.css               # Кастомные стили (светлая/тёмная тема)
│   ├── js/theme.js               # Переключатель темы
│   └── images/products/          # Фото товаров
├── Program.cs                    # Точка входа, DI, Minimal API, Swagger
├── Dockerfile                    # Multi-stage Docker сборка
├── docker-compose.yml            # Оркестрация контейнера
├── .env.example                  # Шаблон переменных окружения
└── coursework.csproj
```

### Схема слоёв

```
Browser / Blazor Client
        │
        ▼
 Blazor Server (SignalR)
        │
  ┌─────┴──────┐
  │  Services  │  ← DI-сервисы (Cart, Order, Recommendation…)
  └─────┬──────┘
        │
  ┌─────┴──────┐
  │  EF Core   │  ← ShopContext → SQLite
  └────────────┘
        │
  ┌─────┴──────┐
  │ Minimal API│  ← /api/products, /api/categories…
  └────────────┘
```

### Модель данных (основные связи)

```
Category ──< Chetkas >─── CartItem
                  │
                  ├───── FavoriteItem
                  ├───── ProductView
                  └───── OrderItem >── Order
```

---

## Установка и запуск

### Предварительные требования

| Инструмент | Версия | Назначение |
|------------|--------|------------|
| [.NET SDK](https://dot.net) | 9.0+ | Локальный запуск |
| [Docker Desktop](https://www.docker.com/products/docker-desktop) | 24+ | Контейнерный запуск |
| [Git](https://git-scm.com) | любая | Клонирование репозитория |

---

### Вариант 1 — Локальный запуск (без Docker)

```bash
# 1. Клонировать репозиторий
git clone https://github.com/khachaturov-a/coursework.git
cd bead-shop

# 2. Восстановить зависимости
dotnet restore

# 3. Применить миграции (создаёт chetki.db)
dotnet ef database update

# 4. Запустить приложение
dotnet run
```

Приложение доступно по адресу: **http://localhost:5000**  
Swagger UI: **http://localhost:5000/swagger**

> База данных заполняется автоматически при первом запуске (50 товаров, 10 категорий).

---

### Вариант 2 — Запуск из Docker Hub (рекомендуется)

Не требует установки .NET SDK. Достаточно Docker Desktop.

```bash
# 1. Скачать готовый образ
docker pull khachaturov/coursework:latest

# 2. Запустить контейнер
docker run -d \
  -p 8089:8080 \
  -v bead-data:/app/data \
  --name bead-shop \
  khachaturov/coursework:latest
```

Приложение доступно по адресу: **http://localhost:8089**

---

### Вариант 3 — Сборка и запуск через docker-compose

```bash
# 1. Клонировать репозиторий
git clone https://github.com/khachaturov-a/coursework.git
cd bead-shop

# 2. Создать файл переменных окружения
cp .env.example .env
# Отредактировать .env: вписать ваш IMAGE_NAME

# 3. Собрать образ локально
docker build -t khachaturov/coursework:latest .

# 4. Запустить через compose
docker-compose up -d

# Посмотреть логи
docker-compose logs -f

# Остановить
docker-compose down
```

Приложение доступно по адресу: **http://localhost:8089**  
Swagger UI: **http://localhost:8089/swagger**

#### Содержимое `.env` (пример)

```env
APP_PORT=8089
IMAGE_NAME=khachaturov/coursework:latest
ASPNETCORE_ENVIRONMENT=Production
```

---

## API Endpoints

Полная документация доступна в Swagger UI по адресу `/swagger`.

| Метод | URL | Описание |
|-------|-----|----------|
| `GET` | `/api/products` | Список всех товаров |
| `GET` | `/api/products/{id}` | Товар по ID |
| `GET` | `/api/categories` | Список категорий |
| `GET` | `/api/products/by-category/{id}` | Товары категории |
| `POST` | `/api/products` | Добавить товар |
| `POST` | `/api/categories` | Добавить категорию |
| `GET` | `/api/config` | Конфигурация приложения |
| `GET` | `/health` | Health check |
| `GET` | `/swagger` | Swagger UI |

---

## Интеллектуальная система рекомендаций

Реализует **контентную фильтрацию** на основе истории взаимодействий пользователя.

**Веса взаимодействий:**

| Событие | Вес |
|---------|-----|
| Покупка товара | **5.0** |
| Добавление в избранное | **3.0** |
| Просмотр (с затуханием) | **1.0 × e^(−0.1 × дней)** |

**Формула оценки товара:**

```
score = catPrefs[category] × 2.0
      + matPrefs[material]  × 1.5
      + (1 − |price − avgPrice| / avgPrice) × 0.5
```

Система строит профиль предпочтений пользователя по категориям, материалам и ценовому диапазону, затем ранжирует доступные товары по степени соответствия.

---

## Docker

### Структура Dockerfile (multi-stage build)

```dockerfile
# Этап 1 — сборка
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
...
RUN dotnet publish -c Release -o /app/publish

# Этап 2 — минимальный runtime (без SDK)
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS final
...
EXPOSE 8080
ENTRYPOINT ["dotnet", "Coursework.dll"]
```

Финальный образ не содержит SDK — только ASP.NET runtime, что минимизирует его размер.

### Volumes

| Volume | Путь в контейнере | Назначение |
|--------|-------------------|------------|
| `bead-data` | `/app/data` | Файл базы данных SQLite (chetki.db) |

### Health check

```yaml
healthcheck:
  test: ["CMD", "curl", "-f", "http://localhost:8080/health"]
  interval: 30s
  timeout: 10s
  retries: 3
```

---

## Лицензия

MIT
