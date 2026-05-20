using Microsoft.EntityFrameworkCore;
using Coursework.Data;
using Coursework.Models;

namespace Coursework.Services;

/// <summary>Начальное заполнение базы данных категориями и товарами.</summary>
public static class DataSeeder
{
    public static async Task SeedAsync(ShopContext db)
    {
        if (await db.Chetkas.CountAsync() == 50) return;

        await db.OrderItems.ExecuteDeleteAsync();
        await db.Orders.ExecuteDeleteAsync();
        await db.CartItems.ExecuteDeleteAsync();
        await db.FavoriteItems.ExecuteDeleteAsync();
        await db.ProductViews.ExecuteDeleteAsync();
        await db.Chetkas.ExecuteDeleteAsync();
        await db.Categories.ExecuteDeleteAsync();

        var categories = new List<Category>
        {
            new() { Name = "Религиозные",   Description = "Православные и медитационные чётки для молитвы и духовных практик" },
            new() { Name = "Деревянные",    Description = "Экологичные чётки из натурального дерева: кипарис, кедр, сандал" },
            new() { Name = "Янтарные",      Description = "Чётки из натурального балтийского янтаря: медовый, вишнёвый, прозрачный" },
            new() { Name = "Каменные",      Description = "Чётки из природных полудрагоценных камней: аметист, яшма, малахит" },
            new() { Name = "Коралловые",    Description = "Чётки из натурального коралла: красного, розового и белого" },
            new() { Name = "Хрустальные",   Description = "Чётки из горного хрусталя и богемского стекла" },
            new() { Name = "Металлические", Description = "Чётки из нержавеющей стали, меди, латуни и серебра" },
            new() { Name = "Антистресс",    Description = "Мягкие силиконовые и деревянные чётки для снятия стресса" },
            new() { Name = "Коллекционные", Description = "Редкие и авторские чётки ручной работы от признанных мастеров" },
            new() { Name = "Роскошные",     Description = "Эксклюзивные чётки из золота, серебра и драгоценных камней" },
        };

        db.Categories.AddRange(categories);
        await db.SaveChangesAsync();

        var rel   = categories[0];
        var wood  = categories[1];
        var amb   = categories[2];
        var stone = categories[3];
        var coral = categories[4];
        var cryst = categories[5];
        var metal = categories[6];
        var anti  = categories[7];
        var coll  = categories[8];
        var lux   = categories[9];

        var products = new List<Chetkas>
        {
            new() { Name = "«Иисусова молитва» из кипариса",    Price = 890,    StockQuantity = 45, Material = "кипарис",      CategoryId = rel.Id,
                    Description = "Классические православные чётки на 33 зерна из благоухающего кипариса Афонского монастыря. Идеальны для ежедневной молитвенной практики." },
            new() { Name = "«Афонские» из чёрного дерева",      Price = 1200,   StockQuantity = 30, Material = "чёрное дерево", CategoryId = rel.Id,
                    Description = "Строгие монашеские чётки из редкого чёрного дерева. Крепкий шнур, узловая вязка." },
            new() { Name = "«Сандаловые православные»",          Price = 1800,   StockQuantity = 25, Material = "сандал",        CategoryId = rel.Id,
                    Description = "Мягкий ненавязчивый аромат сандала сопровождает молитву. Зёрна диаметром 12 мм, длина 50 см." },
            new() { Name = "«Мала» буддийские из рудракши",     Price = 950,    StockQuantity = 40, Material = "рудракша",      CategoryId = rel.Id,
                    Description = "Традиционные буддийские чётки из семян рудракши — дерева Шивы. 108 зёрен, хлопковая нить." },
            new() { Name = "«Медитация» из белого нефрита",     Price = 3200,   StockQuantity = 15, Material = "нефрит",        CategoryId = rel.Id,
                    Description = "Нефрит символизирует чистоту помыслов. Полированные зёрна с шелковистой поверхностью." },

            new() { Name = "«Экодерево» из кедра",              Price = 650,    StockQuantity = 80, Material = "кедр",           CategoryId = wood.Id,
                    Description = "Простые и приятные чётки из сибирского кедра. Аромат хвои снимает усталость и помогает сосредоточиться." },
            new() { Name = "«Лесная сказка» из сосны",          Price = 480,    StockQuantity = 100,Material = "сосна",          CategoryId = wood.Id,
                    Description = "Доступные натуральные чётки из карельской сосны. Приятная текстура, светло-золотистый цвет." },
            new() { Name = "«Дубовые» из морёного дуба",        Price = 1100,   StockQuantity = 35, Material = "морёный дуб",    CategoryId = wood.Id,
                    Description = "Морёный дуб — материал невероятной прочности. Тёмный шоколадный оттенок, лёгкий глянец." },
            new() { Name = "«Сандал красный»",                   Price = 1900,   StockQuantity = 28, Material = "красный сандал", CategoryId = wood.Id,
                    Description = "Насыщенный тёмно-красный цвет и нежный древесный аромат. Зёрна 10 мм, 45 шт." },
            new() { Name = "«Можжевельник» ароматные",           Price = 720,    StockQuantity = 55, Material = "можжевельник",   CategoryId = wood.Id,
                    Description = "Ароматные чётки из можжевельника. Природный антисептик, успокаивающий запах." },

            new() { Name = "«Медовый янтарь»",                  Price = 3500,   StockQuantity = 25, Material = "янтарь медовый",     CategoryId = amb.Id,
                    Description = "Тёплый янтарь медового цвета с мягким свечением. Балтийское месторождение, ручная обработка." },
            new() { Name = "«Вишнёвый янтарь»",                 Price = 4200,   StockQuantity = 18, Material = "янтарь вишнёвый",    CategoryId = amb.Id,
                    Description = "Редкий тёмно-красный янтарь вишнёвого оттенка. Высокая плотность, насыщенный цвет." },
            new() { Name = "«Молочный янтарь»",                 Price = 3800,   StockQuantity = 20, Material = "янтарь молочный",    CategoryId = amb.Id,
                    Description = "Непрозрачный молочно-белый янтарь — редкий природный феномен. Нежная матовая поверхность." },
            new() { Name = "«Янтарь с серебром»",               Price = 8900,   StockQuantity = 12, Material = "янтарь + серебро 925", CategoryId = amb.Id,
                    Description = "Янтарные бусины в серебряной 925 оплётке ручной работы. Сочетание природы и ювелирного мастерства." },
            new() { Name = "«Янтарные матовые»",                Price = 3200,   StockQuantity = 30, Material = "янтарь матовый",     CategoryId = amb.Id,
                    Description = "Матовая обработка делает янтарь приятным на ощупь. Молочно-золотистый оттенок." },

            new() { Name = "«Аметист» королевский",             Price = 4500,   StockQuantity = 22, Material = "аметист",         CategoryId = stone.Id,
                    Description = "Фиолетовый аметист — камень мудрости и духовного роста. Полированные зёрна 12 мм с глубоким цветом." },
            new() { Name = "«Малахит» уральский",               Price = 5500,   StockQuantity = 15, Material = "малахит",         CategoryId = stone.Id,
                    Description = "Уральский малахит с неповторимым зеленоватым узором. Камень-оберег с Урала." },
            new() { Name = "«Лазурит» афганский",               Price = 6800,   StockQuantity = 12, Material = "лазурит",         CategoryId = stone.Id,
                    Description = "Глубокий синий лазурит с золотыми вкраплениями пирита. Камень мудрости Древнего Египта." },
            new() { Name = "«Тигровый глаз» золотой",           Price = 3200,   StockQuantity = 30, Material = "тигровый глаз",   CategoryId = stone.Id,
                    Description = "Переливающийся тигровый глаз с эффектом кошачьего зрачка. Камень удачи в делах." },
            new() { Name = "«Обсидиан» чёрный вулканический",   Price = 2100,   StockQuantity = 45, Material = "обсидиан",        CategoryId = stone.Id,
                    Description = "Вулканическое стекло идеально-чёрного цвета. Мощная защитная энергетика." },

            new() { Name = "«Красный коралл» средиземноморский", Price = 7500,  StockQuantity = 12, Material = "коралл красный",       CategoryId = coral.Id,
                    Description = "Подлинный средиземноморский красный коралл. Добывается традиционным способом без вреда экосистеме." },
            new() { Name = "«Розовый коралл» нежный",            Price = 6800,  StockQuantity = 15, Material = "коралл розовый",       CategoryId = coral.Id,
                    Description = "Деликатный розовый коралл с едва уловимым перламутровым блеском. Символ женственности." },
            new() { Name = "«Белый коралл» снежный",             Price = 5500,  StockQuantity = 20, Material = "коралл белый",         CategoryId = coral.Id,
                    Description = "Белоснежный коралл с матовой поверхностью. Создаёт ощущение морской прохлады." },
            new() { Name = "«Коралл с серебром» 925",            Price = 9800,  StockQuantity = 8,  Material = "коралл + серебро 925", CategoryId = coral.Id,
                    Description = "Красный коралл в серебряной 925 оправе. Сочетание природного и благородного металла." },
            new() { Name = "«Морской коралл» натуральный",       Price = 4800,  StockQuantity = 25, Material = "коралл морской",       CategoryId = coral.Id,
                    Description = "Натуральный морской коралл с неповторимой ветвистой текстурой. Природная красота без обработки." },

            new() { Name = "«Горный хрусталь» прозрачный",      Price = 4200,   StockQuantity = 30, Material = "горный хрусталь",  CategoryId = cryst.Id,
                    Description = "Кристально прозрачный горный хрусталь без примесей. Камень ясности и концентрации." },
            new() { Name = "«Дымчатый хрусталь» мистический",   Price = 5500,   StockQuantity = 20, Material = "дымчатый хрусталь", CategoryId = cryst.Id,
                    Description = "Завораживающий дымчатый кварц с переходом от прозрачного к тёмно-коричневому." },
            new() { Name = "«Хрусталь розовый» нежный",         Price = 4800,   StockQuantity = 25, Material = "розовый хрусталь", CategoryId = cryst.Id,
                    Description = "Нежный полупрозрачный розовый хрусталь. Мягкое свечение изнутри при ярком свете." },
            new() { Name = "«Богемское стекло» радужное",        Price = 3400,   StockQuantity = 40, Material = "богемское стекло", CategoryId = cryst.Id,
                    Description = "Ручная работа чешских мастеров. Многогранные бусины с радужным переливом при свете." },
            new() { Name = "«Муранское стекло» авторское",       Price = 8500,   StockQuantity = 8,  Material = "муранское стекло", CategoryId = cryst.Id,
                    Description = "Уникальные чётки из муранского стекла, сделанные мастером острова Мурано. Каждая — единственная." },

            new() { Name = "«Сталь 316L» матовая",              Price = 2800,   StockQuantity = 45, Material = "нержавеющая сталь",    CategoryId = metal.Id,
                    Description = "Медицинская нержавеющая сталь 316L. Не темнеет, не окисляется, гипоаллергенная." },
            new() { Name = "«Серебро 925» классика",            Price = 8500,   StockQuantity = 18, Material = "серебро 925",           CategoryId = metal.Id,
                    Description = "Классические чётки из серебра 925 пробы с родиевым покрытием. Не темнеют годами." },
            new() { Name = "«Титановые» авиационные",           Price = 6800,   StockQuantity = 20, Material = "титан",                 CategoryId = metal.Id,
                    Description = "Чётки из авиационного титана. Невероятно лёгкие и при этом сверхпрочные." },
            new() { Name = "«Медь с гравировкой»",             Price = 4500,   StockQuantity = 15, Material = "медь с гравировкой",    CategoryId = metal.Id,
                    Description = "Каждая бусина гравирована орнаментом вручную. Уникальный аксессуар ручной работы." },
            new() { Name = "«Латунные» полированные",           Price = 2200,   StockQuantity = 40, Material = "латунь",                CategoryId = metal.Id,
                    Description = "Полированная латунь тёплого золотистого цвета. Приятный вес, благородный блеск." },

            new() { Name = "«Силиконовые» мягкие",              Price = 350,    StockQuantity = 120,Material = "силикон",             CategoryId = anti.Id,
                    Description = "Гипоаллергенный мягкий силикон. Идеально подходят для офиса и людей с тревожностью." },
            new() { Name = "«Акриловые» разноцветные",          Price = 420,    StockQuantity = 100,Material = "акрил",              CategoryId = anti.Id,
                    Description = "Яркие акриловые бусины 20 цветов на выбор. Лёгкие, прочные, весёлые." },
            new() { Name = "«Деревянные крупные» антистресс",   Price = 580,    StockQuantity = 70, Material = "берёза",             CategoryId = anti.Id,
                    Description = "Крупные зёрна 20 мм из берёзы. Тактильно приятные, помогают сосредоточиться." },
            new() { Name = "«Магнитные» неодимовые",            Price = 890,    StockQuantity = 50, Material = "неодимовые магниты", CategoryId = anti.Id,
                    Description = "Шарики из неодима. Разбираются и собираются сами по себе — настоящее успокоительное." },
            new() { Name = "«Кедровые» ароматные антистресс",   Price = 720,    StockQuantity = 60, Material = "кедр",               CategoryId = anti.Id,
                    Description = "Крупные кедровые бусины с ярко выраженным хвойным ароматом. Природная ароматерапия." },

            new() { Name = "«Императорские» из жадеита",        Price = 35000,  StockQuantity = 2,  Material = "жадеит",             CategoryId = coll.Id,
                    Description = "Жадеит имперского зелёного цвета — самый редкий нефрит. Символ власти в Китае тысячи лет." },
            new() { Name = "«Афганские» лазурит+золото",        Price = 28000,  StockQuantity = 3,  Material = "лазурит + золото",   CategoryId = coll.Id,
                    Description = "Авторские чётки с лазуритом и золотыми вставками. Работа кабульского мастера, 1970-е годы." },
            new() { Name = "«Антикварные» янтарные XIX в.",      Price = 45000,  StockQuantity = 1,  Material = "янтарь антикварный", CategoryId = coll.Id,
                    Description = "Подлинные чётки XIX века из балтийского янтаря. Сертификат подлинности прилагается." },
            new() { Name = "«Турецкие» из верескового корня",   Price = 15000,  StockQuantity = 4,  Material = "вереск",             CategoryId = coll.Id,
                    Description = "Традиционный турецкий кёмберли из корня вереска. Невесомые и поразительно красивые." },
            new() { Name = "«Греческие» из оливкового дерева",  Price = 6800,   StockQuantity = 6,  Material = "оливковое дерево",   CategoryId = coll.Id,
                    Description = "Комболои — греческие чётки из освящённого оливкового дерева. Народный символ удачи." },

            new() { Name = "«Золотые» из золота 585",           Price = 85000,  StockQuantity = 3,  Material = "золото 585",              CategoryId = lux.Id,
                    Description = "Массивные чётки из жёлтого золота 585 пробы. Вес 120 г. Ювелирная работа высшего класса." },
            new() { Name = "«Серебряные» с эмалью",             Price = 18500,  StockQuantity = 6,  Material = "серебро 925 + эмаль",     CategoryId = lux.Id,
                    Description = "Серебряные чётки с горячей эмалью цвета морской волны. Лимитированная серия — 50 штук." },
            new() { Name = "«Рубиновые» в золоте",              Price = 125000, StockQuantity = 2,  Material = "рубин + золото 585",       CategoryId = lux.Id,
                    Description = "Бирманские рубины в оправе из красного золота 585. Общий вес камней 18 карат." },
            new() { Name = "«Жемчужные» с золотым замком",      Price = 42000,  StockQuantity = 5,  Material = "жемчуг + золото 585",      CategoryId = lux.Id,
                    Description = "Культивированный японский жемчуг Акоя с замком из жёлтого золота 585." },
            new() { Name = "«Бриллиантовые» белое золото",      Price = 320000, StockQuantity = 1,  Material = "бриллианты + белое золото", CategoryId = lux.Id,
                    Description = "Чётки с 50 бриллиантами общим весом 5 карат в белом золоте 750. Абсолютная роскошь." },
        };

        // Явно назначаем ID 9..58 — имена файлов фото совпадают с этими ID
        for (int i = 0; i < products.Count; i++)
            products[i].Id = 9 + i;

        db.Chetkas.AddRange(products);
        await db.SaveChangesAsync();
    }
}
