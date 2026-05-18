using Microsoft.EntityFrameworkCore;
using Practos3.Data;
using Practos3.Models;

namespace Practos3.Services;

public static class DataSeeder
{
    public static async Task SeedAsync(ShopContext db)
    {
        if (await db.Categories.AnyAsync()) return;

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

        // Ссылки по имени
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
            // --- Религиозные ---
            new() { Name = "«Иисусова молитва» из кипариса", Price = 890,    StockQuantity = 45, Material = "кипарис",       CategoryId = rel.Id,   Description = "Классические православные чётки на 33 зерна из благоухающего кипариса Афонского монастыря. Идеальны для ежедневной молитвенной практики." },
            new() { Name = "«Афонские» из чёрного дерева",  Price = 1200,   StockQuantity = 30, Material = "чёрное дерево",  CategoryId = rel.Id,   Description = "Строгие монашеские чётки из редкого чёрного дерева. Крепкий шнур, узловая вязка." },
            new() { Name = "«Тасбих» из агата",             Price = 2200,   StockQuantity = 20, Material = "агат",           CategoryId = rel.Id,   Description = "Исламские чётки на 99 зёрен из натурального тёмно-серого агата. Ровная полировка и приятный вес." },
            new() { Name = "«Сандаловые православные»",     Price = 1800,   StockQuantity = 25, Material = "сандал",         CategoryId = rel.Id,   Description = "Мягкий ненавязчивый аромат сандала сопровождает молитву. Зёрна диаметром 12 мм, длина 50 см." },
            new() { Name = "«Мала» буддийские из рудракши", Price = 950,    StockQuantity = 40, Material = "рудракша",       CategoryId = rel.Id,   Description = "Традиционные буддийские чётки из семян рудракши — дерева Шивы. 108 зёрен, хлопковая нить." },
            new() { Name = "«Тасбих» из оникса",            Price = 1600,   StockQuantity = 22, Material = "оникс",          CategoryId = rel.Id,   Description = "Глубокий чёрный оникс с тонкими прожилками. Успокаивающая игра пальцев при зикре." },
            new() { Name = "«Святая Русь» из кедра",        Price = 750,    StockQuantity = 60, Material = "кедр",           CategoryId = rel.Id,   Description = "Лёгкие и душистые чётки из сибирского кедра. Хорошо подходят для начинающих." },
            new() { Name = "«Медитация» из белого нефрита", Price = 3200,   StockQuantity = 15, Material = "нефрит",         CategoryId = rel.Id,   Description = "Нефрит символизирует чистоту помыслов. Полированные зёрна с шелковистой поверхностью." },
            new() { Name = "«Молебные» из дуба афонского",  Price = 1400,   StockQuantity = 18, Material = "дуб",            CategoryId = rel.Id,   Description = "Чётки из освящённого дуба, привезённого с горы Афон. Матовая обработка, тёмно-коричневый цвет." },
            new() { Name = "«Намаз» из янтаря",             Price = 2500,   StockQuantity = 12, Material = "янтарь",         CategoryId = rel.Id,   Description = "Исламские чётки из балтийского янтаря медового цвета. Тёплый на ощупь, лёгкий." },

            // --- Деревянные ---
            new() { Name = "«Экодерево» из кедра",          Price = 650,    StockQuantity = 80, Material = "кедр",           CategoryId = wood.Id,  Description = "Простые и приятные чётки из сибирского кедра. Аромат хвои снимает усталость и помогает сосредоточиться." },
            new() { Name = "«Лесная сказка» из сосны",      Price = 480,    StockQuantity = 100, Material = "сосна",          CategoryId = wood.Id,  Description = "Доступные натуральные чётки из карельской сосны. Приятная текстура, светло-золотистый цвет." },
            new() { Name = "«Дубовые» из морёного дуба",    Price = 1100,   StockQuantity = 35, Material = "морёный дуб",    CategoryId = wood.Id,  Description = "Морёный дуб — материал невероятной прочности и красоты. Тёмный шоколадный оттенок, лёгкий глянец." },
            new() { Name = "«Ореховые» из грецкого ореха",  Price = 780,    StockQuantity = 50, Material = "грецкий орех",   CategoryId = wood.Id,  Description = "Тёплые золотисто-коричневые чётки с характерной текстурой ореха. Плотные и долговечные." },
            new() { Name = "«Эбонит» из чёрного дерева",    Price = 2400,   StockQuantity = 20, Material = "чёрное дерево",  CategoryId = wood.Id,  Description = "Эбонитовые чётки из африканского чёрного дерева. Бархатистая поверхность, благородный тёмный цвет." },
            new() { Name = "«Сандал красный»",               Price = 1900,   StockQuantity = 28, Material = "красный сандал", CategoryId = wood.Id,  Description = "Насыщенный тёмно-красный цвет и нежный древесный аромат. Зёрна 10 мм, 45 шт." },
            new() { Name = "«Берёзовые» с узором",           Price = 560,    StockQuantity = 65, Material = "берёза",         CategoryId = wood.Id,  Description = "Лёгкие чётки из карельской берёзы с природным рисунком капа. Каждые — уникальны." },
            new() { Name = "«Можжевельник» ароматные",       Price = 720,    StockQuantity = 55, Material = "можжевельник",   CategoryId = wood.Id,  Description = "Ароматные чётки из можжевельника. Природный антисептик, успокаивающий запах." },
            new() { Name = "«Манго» тропические",            Price = 890,    StockQuantity = 40, Material = "дерево манго",   CategoryId = wood.Id,  Description = "Экзотические чётки из тропического дерева манго. Яркий рисунок волокон, приятный вес." },
            new() { Name = "«Бамбуковые»",                  Price = 420,    StockQuantity = 90, Material = "бамбук",         CategoryId = wood.Id,  Description = "Экологичные чётки из натурального бамбука. Сверхлёгкие, прочные, устойчивые к влаге." },

            // --- Янтарные ---
            new() { Name = "«Медовый янтарь»",              Price = 3500,   StockQuantity = 25, Material = "янтарь медовый",     CategoryId = amb.Id,   Description = "Тёплый янтарь медового цвета с мягким свечением. Балтийское месторождение, ручная обработка." },
            new() { Name = "«Вишнёвый янтарь»",             Price = 4200,   StockQuantity = 18, Material = "янтарь вишнёвый",    CategoryId = amb.Id,   Description = "Редкий тёмно-красный янтарь вишнёвого оттенка. Высокая плотность, насыщенный цвет." },
            new() { Name = "«Молочный янтарь»",             Price = 3800,   StockQuantity = 20, Material = "янтарь молочный",    CategoryId = amb.Id,   Description = "Непрозрачный молочно-белый янтарь — редкий природный феномен. Нежная матовая поверхность." },
            new() { Name = "«Зелёный янтарь»",              Price = 5500,   StockQuantity = 10, Material = "янтарь зелёный",     CategoryId = amb.Id,   Description = "Исключительно редкий зелёный янтарь с природным включением хлорофилла. Коллекционная ценность." },
            new() { Name = "«Прозрачный янтарь»",           Price = 4800,   StockQuantity = 15, Material = "янтарь прозрачный",  CategoryId = amb.Id,   Description = "Кристально прозрачный янтарь без включений. Каждая бусина — словно маленькое солнце." },
            new() { Name = "«Янтарные с инклюзами»",        Price = 12000,  StockQuantity = 5,  Material = "янтарь с инклюзами", CategoryId = amb.Id,   Description = "Уникальные чётки с насекомыми и растениями внутри янтаря. Возраст 40–50 миллионов лет." },
            new() { Name = "«Балтийская волна»",            Price = 6500,   StockQuantity = 8,  Material = "янтарь необработанный", CategoryId = amb.Id, Description = "Чётки из необработанного балтийского янтаря с природной корочкой. Грубая природная красота." },
            new() { Name = "«Янтарь с серебром»",           Price = 8900,   StockQuantity = 12, Material = "янтарь + серебро 925", CategoryId = amb.Id,  Description = "Янтарные бусины в серебряной 925 оплётке ручной работы. Сочетание природы и ювелирного мастерства." },
            new() { Name = "«Янтарные матовые»",            Price = 3200,   StockQuantity = 30, Material = "янтарь матовый",     CategoryId = amb.Id,   Description = "Матовая обработка делает янтарь более приятным на ощупь. Молочно-золотистый оттенок." },
            new() { Name = "«Янтарь прессованный»",         Price = 2200,   StockQuantity = 40, Material = "янтарь прессованный", CategoryId = amb.Id,  Description = "Чётки из прессованной янтарной крошки. Яркий насыщенный цвет по доступной цене." },

            // --- Каменные ---
            new() { Name = "«Аметист» королевский",         Price = 4500,   StockQuantity = 22, Material = "аметист",         CategoryId = stone.Id, Description = "Фиолетовый аметист — камень мудрости и духовного роста. Полированные зёрна 12 мм с глубоким цветом." },
            new() { Name = "«Яшма красная»",                Price = 2800,   StockQuantity = 35, Material = "яшма красная",    CategoryId = stone.Id, Description = "Энергетичная красная яшма с уникальным природным рисунком. Камень силы и уверенности." },
            new() { Name = "«Гематит» стальной",            Price = 1800,   StockQuantity = 50, Material = "гематит",         CategoryId = stone.Id, Description = "Тяжёлые зеркально-блестящие чётки из гематита. Обладают антимагнитными свойствами." },
            new() { Name = "«Малахит» уральский",           Price = 5500,   StockQuantity = 15, Material = "малахит",         CategoryId = stone.Id, Description = "Уральский малахит с неповторимым зеленоватым узором. Камень-оберег от Урала." },
            new() { Name = "«Лазурит» афганский",           Price = 6800,   StockQuantity = 12, Material = "лазурит",         CategoryId = stone.Id, Description = "Глубокий синий лазурит с золотыми вкраплениями пирита. Камень мудрости Древнего Египта." },
            new() { Name = "«Бирюза» натуральная",          Price = 4200,   StockQuantity = 20, Material = "бирюза",          CategoryId = stone.Id, Description = "Натуральная персидская бирюза ярко-голубого цвета. Самый сильный оберег Востока." },
            new() { Name = "«Тигровый глаз» золотой",       Price = 3200,   StockQuantity = 30, Material = "тигровый глаз",   CategoryId = stone.Id, Description = "Переливающийся тигровый глаз с эффектом кошачьего зрачка. Камень удачи в делах." },
            new() { Name = "«Розовый кварц» нежный",        Price = 2600,   StockQuantity = 38, Material = "розовый кварц",   CategoryId = stone.Id, Description = "Нежный розовый кварц — камень любви и гармонии. Полупрозрачные матовые бусины." },
            new() { Name = "«Обсидиан» чёрный вулканический",Price = 2100,  StockQuantity = 45, Material = "обсидиан",        CategoryId = stone.Id, Description = "Вулканическое стекло идеально-чёрного цвета. Мощная защитная энергетика." },
            new() { Name = "«Оникс» чёрный полированный",   Price = 3400,   StockQuantity = 28, Material = "оникс",           CategoryId = stone.Id, Description = "Благородный чёрный оникс с зеркальной полировкой. Камень самоконтроля и выдержки." },
            new() { Name = "«Нефрит зелёный» имперский",    Price = 4800,   StockQuantity = 18, Material = "нефрит",          CategoryId = stone.Id, Description = "Императорский нефрит насыщенного нефритово-зелёного цвета. Символ достатка и здоровья." },
            new() { Name = "«Яшма зелёная» в крапинку",     Price = 2900,   StockQuantity = 32, Material = "яшма зелёная",    CategoryId = stone.Id, Description = "Зелёная яшма с природными вкраплениями. Камень природы и гармонии." },

            // --- Коралловые ---
            new() { Name = "«Красный коралл» средиземноморский",  Price = 7500,   StockQuantity = 12, Material = "коралл красный",  CategoryId = coral.Id, Description = "Подлинный средиземноморский красный коралл. Добывается традиционным способом без вреда экосистеме." },
            new() { Name = "«Розовый коралл» нежный",             Price = 6800,   StockQuantity = 15, Material = "коралл розовый",  CategoryId = coral.Id, Description = "Деликатный розовый коралл с едва уловимым перламутровым блеском. Символ женственности." },
            new() { Name = "«Белый коралл» снежный",              Price = 5500,   StockQuantity = 20, Material = "коралл белый",    CategoryId = coral.Id, Description = "Белоснежный коралл с матовой поверхностью. Создаёт ощущение морской прохлады." },
            new() { Name = "«Коралл оранжевый» закатный",         Price = 6200,   StockQuantity = 10, Material = "коралл оранжевый", CategoryId = coral.Id, Description = "Редкий оранжевый коралл цвета закатного неба. Яркий и согревающий." },
            new() { Name = "«Коралл с позолотой»",                Price = 18500,  StockQuantity = 5,  Material = "коралл + золото 585", CategoryId = coral.Id, Description = "Красный коралл в золотой 585 оплётке ювелирной работы. Роскошный подарок." },
            new() { Name = "«Морской коралл» натуральный",        Price = 4800,   StockQuantity = 25, Material = "коралл морской",  CategoryId = coral.Id, Description = "Натуральный морской коралл с неповторимой ветвистой текстурой. Природная красота без обработки." },
            new() { Name = "«Коралл бамбуковый»",                 Price = 3200,   StockQuantity = 30, Material = "коралл бамбуковый", CategoryId = coral.Id, Description = "Бамбуковый коралл с характерными сегментами. Экологически устойчивый вариант." },
            new() { Name = "«Коралл с серебром» 925",             Price = 9800,   StockQuantity = 8,  Material = "коралл + серебро 925", CategoryId = coral.Id, Description = "Красный коралл в серебряной 925 оправе. Сочетание природного и благородного металла." },

            // --- Хрустальные ---
            new() { Name = "«Горный хрусталь» прозрачный",        Price = 4200,   StockQuantity = 30, Material = "горный хрусталь",     CategoryId = cryst.Id, Description = "Кристально прозрачный горный хрусталь без примесей. Камень ясности и концентрации." },
            new() { Name = "«Дымчатый хрусталь» мистический",     Price = 5500,   StockQuantity = 20, Material = "дымчатый хрусталь",   CategoryId = cryst.Id, Description = "Завораживающий дымчатый кварц с переходом от прозрачного к тёмно-коричневому." },
            new() { Name = "«Хрусталь розовый» нежный",           Price = 4800,   StockQuantity = 25, Material = "розовый хрусталь",    CategoryId = cryst.Id, Description = "Нежный полупрозрачный розовый хрусталь. Мягкое свечение изнутри при ярком свете." },
            new() { Name = "«Аметистовый хрусталь»",              Price = 6200,   StockQuantity = 15, Material = "аметистовый хрусталь", CategoryId = cryst.Id, Description = "Переходящий от бесцветного к фиолетовому аметрин. Редкий природный феномен." },
            new() { Name = "«Хрусталь чёрный» морион",            Price = 5800,   StockQuantity = 18, Material = "чёрный хрусталь",     CategoryId = cryst.Id, Description = "Редкий чёрный хрусталь-морион. Мощный защитный камень с загадочным видом." },
            new() { Name = "«Ледяной хрусталь»",                  Price = 7200,   StockQuantity = 10, Material = "горный хрусталь",     CategoryId = cryst.Id, Description = "Крупные зёрна 16 мм из чистейшего горного хрусталя. Холодный на ощупь даже летом." },
            new() { Name = "«Богемское стекло» радужное",          Price = 3400,   StockQuantity = 40, Material = "богемское стекло",    CategoryId = cryst.Id, Description = "Ручная работа чешских мастеров. Многогранные бусины с радужным переливом при свете." },
            new() { Name = "«Муранское стекло» авторское",         Price = 8500,   StockQuantity = 8,  Material = "муранское стекло",    CategoryId = cryst.Id, Description = "Уникальные чётки из муранского стекла, сделанные мастером острова Мурано. Каждая — единственная." },
            new() { Name = "«Хрусталь зелёный» изумрудный",       Price = 5200,   StockQuantity = 22, Material = "зелёный хрусталь",    CategoryId = cryst.Id, Description = "Насыщенный зелёный хрусталь с тёмными рефлексами. Редкий природный оттенок." },

            // --- Металлические ---
            new() { Name = "«Сталь 316L» матовая",                Price = 2800,   StockQuantity = 45, Material = "нержавеющая сталь",   CategoryId = metal.Id, Description = "Медицинская нержавеющая сталь 316L. Не темнеет, не окисляется, гипоаллергенная." },
            new() { Name = "«Медные» с патиной",                  Price = 1900,   StockQuantity = 55, Material = "медь",                CategoryId = metal.Id, Description = "Медные чётки с натуральной патиной. Со временем приобретают уникальный тёмный облик." },
            new() { Name = "«Латунные» полированные",             Price = 2200,   StockQuantity = 40, Material = "латунь",              CategoryId = metal.Id, Description = "Полированная латунь тёплого золотистого цвета. Приятный вес, благородный блеск." },
            new() { Name = "«Серебро 925» классика",              Price = 8500,   StockQuantity = 18, Material = "серебро 925",         CategoryId = metal.Id, Description = "Классические чётки из серебра 925 пробы с родиевым покрытием. Не темнеют годами." },
            new() { Name = "«Титановые» авиационные",             Price = 6800,   StockQuantity = 20, Material = "титан",               CategoryId = metal.Id, Description = "Чётки из авиационного титана. Невероятно лёгкие и при этом сверхпрочные." },
            new() { Name = "«Вольфрамовые» тяжёлые",              Price = 12000,  StockQuantity = 10, Material = "вольфрам",            CategoryId = metal.Id, Description = "Вольфрам — один из самых плотных металлов. Почувствуй настоящую тяжесть в ладони." },
            new() { Name = "«Медь с гравировкой»",               Price = 4500,   StockQuantity = 15, Material = "медь с гравировкой",  CategoryId = metal.Id, Description = "Каждая бусина гравирована орнаментом вручную. Уникальный аксессуар ручной работы." },
            new() { Name = "«Анодированный алюминий» цветные",    Price = 3200,   StockQuantity = 35, Material = "алюминий анодированный", CategoryId = metal.Id, Description = "Яркие многоцветные бусины из анодированного алюминия. Лёгкие, не темнеют." },

            // --- Антистресс ---
            new() { Name = "«Силиконовые» мягкие",               Price = 350,    StockQuantity = 120, Material = "силикон",            CategoryId = anti.Id,  Description = "Гипоаллергенный мягкий силикон. Идеально подходят для офиса и людей с тревожностью." },
            new() { Name = "«Акриловые» разноцветные",            Price = 420,    StockQuantity = 100, Material = "акрил",              CategoryId = anti.Id,  Description = "Яркие акриловые бусины 20 цветов на выбор. Лёгкие, прочные, весёлые." },
            new() { Name = "«Резиновые» для тренировки",          Price = 280,    StockQuantity = 150, Material = "резина",             CategoryId = anti.Id,  Description = "Специальные резиновые чётки для укрепления мышц кисти и пальцев." },
            new() { Name = "«Деревянные крупные» антистресс",     Price = 580,    StockQuantity = 70,  Material = "берёза",             CategoryId = anti.Id,  Description = "Крупные зёрна 20 мм из берёзы. Тактильно приятные, помогают сосредоточиться." },
            new() { Name = "«Бусины-мячики» foam",                Price = 390,    StockQuantity = 90,  Material = "вспененный ПУ",      CategoryId = anti.Id,  Description = "Мягкие губчатые бусины. Идеальный антистресс: сжимай и отпускай снова и снова." },
            new() { Name = "«Магнитные» неодимовые",              Price = 890,    StockQuantity = 50,  Material = "неодимовые магниты", CategoryId = anti.Id,  Description = "Шарики из неодима. Разбираются и собираются сами по себе — настоящее успокоительное." },
            new() { Name = "«Гладкий камень» нефрит",             Price = 1200,   StockQuantity = 40,  Material = "нефрит",             CategoryId = anti.Id,  Description = "Гладкие нефритовые бусины идеальной формы. Прохладные и приятные на ощупь." },
            new() { Name = "«Мраморные» тяжёлые",                 Price = 950,    StockQuantity = 45,  Material = "мрамор",             CategoryId = anti.Id,  Description = "Тяжёлые мраморные бусины с природными прожилками. Успокаивает своей весомостью." },
            new() { Name = "«Кедровые» ароматные антистресс",     Price = 720,    StockQuantity = 60,  Material = "кедр",               CategoryId = anti.Id,  Description = "Крупные кедровые бусины с ярко выраженным хвойным ароматом. Природная ароматерапия." },
            new() { Name = "«Янтарит» мягкий тёплый",             Price = 680,    StockQuantity = 55,  Material = "янтарит",            CategoryId = anti.Id,  Description = "Чётки из янтарита (прессованного янтаря). Тёплый солнечный цвет поднимает настроение." },

            // --- Коллекционные ---
            new() { Name = "«Императорские» из жадеита",          Price = 35000,  StockQuantity = 2,  Material = "жадеит",              CategoryId = coll.Id,  Description = "Жадеит имперского зелёного цвета — самый редкий нефрит. Символ власти в Китае тысячи лет." },
            new() { Name = "«Афганские» лазурит+золото",          Price = 28000,  StockQuantity = 3,  Material = "лазурит + золото",    CategoryId = coll.Id,  Description = "Авторские чётки с лазуритом и золотыми вставками. Работа кабульского мастера, 1970-е годы." },
            new() { Name = "«Мастер Виктор» из ореха",            Price = 8500,   StockQuantity = 5,  Material = "грецкий орех",        CategoryId = coll.Id,  Description = "Авторские чётки известного петербургского резчика. Каждая бусина вырезана вручную и неповторима." },
            new() { Name = "«Антикварные» янтарные XIX в.",        Price = 45000,  StockQuantity = 1,  Material = "янтарь антикварный",  CategoryId = coll.Id,  Description = "Подлинные чётки XIX века из балтийского янтаря. Сертификат подлинности прилагается." },
            new() { Name = "«Турецкие» из верескового корня",     Price = 15000,  StockQuantity = 4,  Material = "вереск",              CategoryId = coll.Id,  Description = "Традиционный турецкий кёмберли из корня вереска. Невесомые и поразительно красивые." },
            new() { Name = "«Персидские» бирюза с позолотой",     Price = 22000,  StockQuantity = 3,  Material = "бирюза + позолота",   CategoryId = coll.Id,  Description = "Иранские чётки с натуральной бирюзой в позолоченной оправе. Работа тегеранских ювелиров." },
            new() { Name = "«Греческие» из оливкового дерева",    Price = 6800,   StockQuantity = 6,  Material = "оливковое дерево",    CategoryId = coll.Id,  Description = "Комболои — греческие чётки из освящённого оливкового дерева. Народный символ удачи." },
            new() { Name = "«Непальские» рудракша с серебром",    Price = 12000,  StockQuantity = 5,  Material = "рудракша + серебро",  CategoryId = coll.Id,  Description = "Непальские чётки с крупными зёрнами рудракши и серебряными дисками-разделителями." },

            // --- Роскошные ---
            new() { Name = "«Золотые» из золота 585",             Price = 85000,  StockQuantity = 3,  Material = "золото 585",          CategoryId = lux.Id,   Description = "Массивные чётки из жёлтого золота 585 пробы. Вес 120 г. Ювелирная работа высшего класса." },
            new() { Name = "«Серебряные» с эмалью",               Price = 18500,  StockQuantity = 6,  Material = "серебро 925 + эмаль", CategoryId = lux.Id,   Description = "Серебряные чётки с горячей эмалью цвета морской волны. Лимитированная серия — 50 штук." },
            new() { Name = "«Рубиновые» в золоте",                Price = 125000, StockQuantity = 2,  Material = "рубин + золото 585",  CategoryId = lux.Id,   Description = "Бирманские рубины в оправе из красного золота 585. Общий вес камней 18 карат." },
            new() { Name = "«Сапфировые» классика",               Price = 95000,  StockQuantity = 2,  Material = "сапфир + серебро 925", CategoryId = lux.Id,  Description = "Синие сапфиры Цейлона в серебряной родированной оправе. Неизменная элегантность." },
            new() { Name = "«Изумрудные» элитные",                Price = 180000, StockQuantity = 1,  Material = "изумруд + золото 750", CategoryId = lux.Id,  Description = "Колумбийские изумруды в белом золоте 750. Наивысший коллекционный уровень." },
            new() { Name = "«Жемчужные» с золотым замком",        Price = 42000,  StockQuantity = 5,  Material = "жемчуг + золото 585", CategoryId = lux.Id,   Description = "Культивированный японский жемчуг Акоя с замком из жёлтого золота 585." },
            new() { Name = "«Бриллиантовые» белое золото",        Price = 320000, StockQuantity = 1,  Material = "бриллианты + белое золото", CategoryId = lux.Id, Description = "Чётки с 50 бриллиантами общим весом 5 карат в белом золоте 750. Абсолютная роскошь." },
            new() { Name = "«Александрит» редкий хамелеон",       Price = 250000, StockQuantity = 1,  Material = "александрит",         CategoryId = lux.Id,   Description = "Александрит — редчайший камень, меняющий цвет от зелёного днём до малинового вечером." },
        };

        db.Chetkas.AddRange(products);
        await db.SaveChangesAsync();
    }
}
