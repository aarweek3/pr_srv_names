using DAL.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DAL.Configurations
{
    public class NameDetailConfiguration : IEntityTypeConfiguration<NameDetail>
    {
        public void Configure(EntityTypeBuilder<NameDetail> entity)
        {
            entity.ToTable("NameDetails");
            entity.HasKey(nd => nd.Id);

            entity.Property(nd => nd.Id).HasComment("Уникальный идентификатор записи описания");
            entity.Property(nd => nd.NameMainId).IsRequired().HasComment("Внешний ключ на таблицу Names");
            entity.Property(nd => nd.LanguageId).IsRequired().HasComment("Внешний ключ на таблицу Languages");
            entity.Property(nd => nd.Name).HasMaxLength(128).HasComment("Имя на языке");
            entity.Property(nd => nd.FullName).HasMaxLength(128).HasComment("Полное имя");
            entity.Property(nd => nd.Diminutiv).HasMaxLength(128).HasComment("Уменьшительно-ласкательное имя");
            entity.Property(nd => nd.NameRod).HasMaxLength(128).HasComment("Родительный падеж имени");
            entity.Property(nd => nd.NameFemaleCouple).HasMaxLength(256).HasComment("Женское парное имя");
            entity.Property(nd => nd.NameDays).HasMaxLength(256).HasComment("Именины");
            entity.Property(nd => nd.OrthodoxName).HasMaxLength(128).HasComment("Имя в православии");
            entity.Property(nd => nd.OtherFormName).HasMaxLength(256).HasComment("Другие формы имени");
            entity.Property(nd => nd.Synonyms).HasMaxLength(256).HasComment("Синонимы");
            entity.Property(nd => nd.Surname).HasMaxLength(256).HasComment("Отчество");
            entity.Property(nd => nd.LuckyDay).HasMaxLength(64).HasComment("Благоприятный день");
            entity.Property(nd => nd.LuckyDayDescription).HasMaxLength(512).HasComment("Описание благоприятного дня");
            entity.Property(nd => nd.ProfessionAndBusiness).HasMaxLength(1024).HasComment("Профессия и бизнес");
            entity.Property(nd => nd.Health).HasMaxLength(1024).HasComment("Здоровье");
            entity.Property(nd => nd.SexAndLove).HasMaxLength(1024).HasComment("Секс и любовь");
            entity.Property(nd => nd.FamilyAndMarriage).HasMaxLength(1024).HasComment("Семья и брак");
            entity.Property(nd => nd.CharacterTraits).HasMaxLength(256).HasComment("Черты характера");
            entity.Property(nd => nd.Hobbies).HasMaxLength(1024).HasComment("Увлечения и хобби");
            entity.Property(nd => nd.Gender).HasComment("Пол имени");
            entity.Property(nd => nd.Description).HasMaxLength(1024).HasComment("Полное описание значения имени");
            entity.Property(nd => nd.DescriptionShort).HasComment("Краткое описание имени");
            entity.Property(nd => nd.ShortMeaning).HasComment("Краткое значение/перевод имени");
            entity.Property(nd => nd.Origin).HasComment("Происхождение/этимология имени");
            entity.Property(nd => nd.AdultCharacter).HasMaxLength(2056).HasComment("Характер взрослого");
            entity.Property(nd => nd.Country).HasMaxLength(128).HasComment("Страна");
            entity.Property(nd => nd.Etymology).HasMaxLength(1024).HasComment("Этимология имени");
            entity.Property(nd => nd.FormsName).HasMaxLength(512).HasComment("Производные формы");
            entity.Property(nd => nd.HarmonyWith).HasMaxLength(1024).HasComment("Гармоничные имена");
            entity.Property(nd => nd.IncompatibilityWith).HasMaxLength(1024).HasComment("Негармоничные имена");
            entity.Property(nd => nd.LoveAndSexuality).HasMaxLength(1024).HasComment("Любовь и сексуальность");
            entity.Property(nd => nd.MarriageAndFamily).HasMaxLength(1024).HasComment("Брак и семья");
            entity.Property(nd => nd.MarriageBehavior).HasMaxLength(1024).HasComment("Поведение в браке");
            entity.Property(nd => nd.Love).HasMaxLength(512).HasComment("Любовь");
            entity.Property(nd => nd.Sexuality).HasMaxLength(512).HasComment("Сексуальность");
            entity.Property(nd => nd.Marriage).HasMaxLength(512).HasComment("Брак");
            entity.Property(nd => nd.Business).HasMaxLength(512).HasComment("Бизнес");
            entity.Property(nd => nd.Teenager).HasMaxLength(512).HasComment("Подросток");
            entity.Property(nd => nd.ByHigir).HasMaxLength(512).HasComment("По Хигиру");
            entity.Property(nd => nd.ByMendeleev).HasMaxLength(512).HasComment("По Менделееву");
            entity.Property(nd => nd.Personality).HasMaxLength(512).HasComment("Личность");
            entity.Property(nd => nd.MainFeatures).HasMaxLength(512).HasComment("Основные черты");
            entity.Property(nd => nd.Type).HasMaxLength(512).HasComment("Тип");
            entity.Property(nd => nd.Psyche).HasMaxLength(512).HasComment("Психика");
            entity.Property(nd => nd.Will).HasMaxLength(512).HasComment("Воля");
            entity.Property(nd => nd.Excitability).HasMaxLength(512).HasComment("Возбудимость");
            entity.Property(nd => nd.ReactionSpeed).HasMaxLength(512).HasComment("Скорость реакции");
            entity.Property(nd => nd.FieldOfActivity).HasMaxLength(512).HasComment("Поле деятельности");
            entity.Property(nd => nd.Intuition).HasMaxLength(512).HasComment("Интуиция");
            entity.Property(nd => nd.Intellect).HasMaxLength(512).HasComment("Интеллект");
            entity.Property(nd => nd.Sensitivity).HasMaxLength(512).HasComment("Восприимчивость");
            entity.Property(nd => nd.Morality).HasMaxLength(512).HasComment("Нравственность");
            entity.Property(nd => nd.Activity).HasMaxLength(512).HasComment("Активность");
            entity.Property(nd => nd.Sociability).HasMaxLength(512).HasComment("Общительность");
            entity.Property(nd => nd.Energy).HasMaxLength(512).HasComment("Энергетика имени");
            entity.Property(nd => nd.Fate).HasMaxLength(512).HasComment("Судьба имени");
            entity.Property(nd => nd.NameMeaning).HasMaxLength(256).HasComment("Значение имени");
            entity.Property(nd => nd.Abbreviated).HasMaxLength(64).HasComment("Сокращенное имя");
            entity.Property(nd => nd.PositiveCharacteristic).HasMaxLength(512).HasComment("Позитивные характеристики");
            entity.Property(nd => nd.NegativeCharacteristic).HasMaxLength(512).HasComment("Негативные характеристики");
            entity.Property(nd => nd.Meaning).HasMaxLength(1024).HasComment("Значение имени");
            entity.Property(nd => nd.OriginHistory).HasMaxLength(1024).HasComment("История происхождения");
            entity.Property(nd => nd.PhonosemanticAnalysis).HasMaxLength(1024).HasComment("Фоносемантический анализ");
            entity.Property(nd => nd.ProfessionAndCareer).HasMaxLength(1024).HasComment("Профессия и карьера");
            entity.Property(nd => nd.Pronunciation).HasMaxLength(512).HasComment("Произношение");
            entity.Property(nd => nd.CreatedAt).HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasComment("Дата создания записи");
            entity.Property(nd => nd.UpdatedAt).HasComment("Дата последнего обновления");
            entity.Property(nd => nd.IsActive).HasDefaultValue(true).HasComment("Активна ли запись");

            entity.HasIndex(nd => nd.NameMainId).HasDatabaseName("IX_NameDetails_NameMainId");
            entity.HasIndex(nd => nd.LanguageId).HasDatabaseName("IX_NameDetails_LanguageId");
            entity.HasIndex(nd => new { nd.NameMainId, nd.LanguageId }).IsUnique()
                .HasDatabaseName("IX_NameDetails_NameMainId_LanguageId");
            entity.HasIndex(nd => nd.Gender).HasDatabaseName("IX_NameDetails_Gender");
            entity.HasIndex(nd => nd.Origin).HasDatabaseName("IX_NameDetails_Origin");
        }
    }
}