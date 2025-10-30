namespace DAL.Constants
{
    /// <summary>
    /// Константы для длин строковых полей
    /// Используются во всех моделях для единообразия
    /// </summary>
    public static class StringLengths
    {
        // ==========================================
        // Общие константы
        // ==========================================

        /// <summary>
        /// Короткое имя (64 символа)
        /// Используется для: LuckyDay, Abbreviated
        /// </summary>
        public const int ShortName = 64;

        /// <summary>
        /// Стандартное имя (128 символов)
        /// Используется для: Name, FullName, Diminutiv, NameRod, OrthodoxName, Country
        /// </summary>
        public const int Name = 128;

        /// <summary>
        /// Длинное имя (256 символов)
        /// Используется для: NameFemaleCouple, NameDays, OtherFormName, Synonyms, Surname, CharacterTraits, NameMeaning
        /// </summary>
        public const int LongName = 256;


        /// <summary>
        /// Короткое описание (512 символов)
        /// Используется для: LuckyDayDescription, FormsName, различные характеристики
        /// </summary>
        public const int ShortDescription = 512;

        /// <summary>
        /// Стандартное описание (1024 символа)
        /// Используется для: Description, Etymology, различные подробные описания
        /// </summary>
        public const int Description = 1024;

        /// <summary>
        /// Длинное описание (2056 символов)
        /// Используется для: AdultCharacter и других объёмных текстов
        /// </summary>
        public const int LongDescription = 2056;

        /// <summary>
        /// URL адреса (500 символов)
        /// </summary>
        public const int Url = 500;

        /// <summary>
        /// Email адреса (255 символов)
        /// </summary>
        public const int Email = 255;

        // ==========================================
        // Специфичные константы для NameDetail
        // ==========================================

        /// <summary>
        /// Профессия и бизнес (1024 символа)
        /// </summary>
        public const int ProfessionAndBusiness = 1024;

        /// <summary>
        /// Здоровье (1024 символа)
        /// </summary>
        public const int Health = 1024;

        /// <summary>
        /// Секс и любовь (1024 символа)
        /// </summary>
        public const int SexAndLove = 1024;

        /// <summary>
        /// Семья и брак (1024 символа)
        /// </summary>
        public const int FamilyAndMarriage = 1024;

        /// <summary>
        /// Увлечения и хобби (1024 символа)
        /// </summary>
        public const int Hobbies = 1024;

        /// <summary>
        /// Гармония с именами (1024 символа)
        /// </summary>
        public const int HarmonyWith = 1024;

        /// <summary>
        /// Несовместимость с именами (1024 символа)
        /// </summary>
        public const int IncompatibilityWith = 1024;

        /// <summary>
        /// Любовь и сексуальность (1024 символа)
        /// </summary>
        public const int LoveAndSexuality = 1024;

        /// <summary>
        /// Брак и семья (1024 символа)
        /// </summary>
        public const int MarriageAndFamily = 1024;

        /// <summary>
        /// Поведение в браке (1024 символа)
        /// </summary>
        public const int MarriageBehavior = 1024;

        /// <summary>
        /// История происхождения имени (1024 символа)
        /// </summary>
        public const int OriginHistory = 1024;

        /// <summary>
        /// Фоносемантический анализ имени (1024 символа)
        /// </summary>
        public const int PhonosemanticAnalysis = 1024;

        /// <summary>
        /// Профессия и карьера (1024 символа)
        /// </summary>
        public const int ProfessionAndCareer = 1024;

        /// <summary>
        /// Значение имени (1024 символа)
        /// </summary>
        public const int Meaning = 1024;

        // ==========================================
        // Константы для коротких полей (512)
        // ==========================================

        /// <summary>
        /// Любовь (512 символов)
        /// </summary>
        public const int Love = 512;

        /// <summary>
        /// Сексуальность (512 символов)
        /// </summary>
        public const int Sexuality = 512;

        /// <summary>
        /// Брак (512 символов)
        /// </summary>
        public const int Marriage = 512;

        /// <summary>
        /// Бизнес (512 символов)
        /// </summary>
        public const int Business = 512;

        /// <summary>
        /// Подросток (512 символов)
        /// </summary>
        public const int Teenager = 512;

        /// <summary>
        /// По Хигиру (512 символов)
        /// </summary>
        public const int ByHigir = 512;

        /// <summary>
        /// По Менделееву (512 символов)
        /// </summary>
        public const int ByMendeleev = 512;

        /// <summary>
        /// Личность (512 символов)
        /// </summary>
        public const int Personality = 512;

        /// <summary>
        /// Основные черты (512 символов)
        /// </summary>
        public const int MainFeatures = 512;

        /// <summary>
        /// Тип (512 символов)
        /// </summary>
        public const int Type = 512;

        /// <summary>
        /// Психика (512 символов)
        /// </summary>
        public const int Psyche = 512;

        /// <summary>
        /// Воля (512 символов)
        /// </summary>
        public const int Will = 512;

        /// <summary>
        /// Возбудимость (512 символов)
        /// </summary>
        public const int Excitability = 512;

        /// <summary>
        /// Скорость реакции (512 символов)
        /// </summary>
        public const int ReactionSpeed = 512;

        /// <summary>
        /// Поле деятельности (512 символов)
        /// </summary>
        public const int FieldOfActivity = 512;

        /// <summary>
        /// Интуиция (512 символов)
        /// </summary>
        public const int Intuition = 512;

        /// <summary>
        /// Интеллект (512 символов)
        /// </summary>
        public const int Intellect = 512;

        /// <summary>
        /// Восприимчивость (512 символов)
        /// </summary>
        public const int Sensitivity = 512;

        /// <summary>
        /// Нравственность (512 символов)
        /// </summary>
        public const int Morality = 512;

        /// <summary>
        /// Активность (512 символов)
        /// </summary>
        public const int Activity = 512;

        /// <summary>
        /// Общительность (512 символов)
        /// </summary>
        public const int Sociability = 512;

        /// <summary>
        /// Энергетика имени (512 символов)
        /// </summary>
        public const int Energy = 512;

        /// <summary>
        /// Судьба имени в любви и браке (512 символов)
        /// </summary>
        public const int Fate = 512;

        /// <summary>
        /// Позитивная характеристика (512 символов)
        /// </summary>
        public const int PositiveCharacteristic = 512;

        /// <summary>
        /// Негативная характеристика (512 символов)
        /// </summary>
        public const int NegativeCharacteristic = 512;

        /// <summary>
        /// Произношение (512 символов)
        /// </summary>
        public const int Pronunciation = 512;
    }
}