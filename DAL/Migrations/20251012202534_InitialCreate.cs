using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace DAL.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Languages",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false, comment: "Уникальный идентификатор языка")
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: false, comment: "Название языка на родном языке"),
                    Description = table.Column<string>(type: "character varying(1024)", maxLength: 1024, nullable: true, comment: "Описание языка"),
                    Code = table.Column<string>(type: "character varying(2)", maxLength: 2, nullable: false, comment: "Код языка по стандарту ISO 639-1"),
                    FlagCode = table.Column<string>(type: "character varying(2)", maxLength: 2, nullable: true, comment: "Код флага для фронтенд-отображения"),
                    DisplayOrder = table.Column<int>(type: "integer", nullable: false, defaultValue: 999, comment: "Порядок отображения языка"),
                    IsDefault = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false, comment: "Является ли язык языком по умолчанию"),
                    TextDirection = table.Column<string>(type: "character varying(3)", maxLength: 3, nullable: false, defaultValue: "ltr", comment: "Направление текста (ltr/rtl)"),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP", comment: "Дата создания записи"),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true, comment: "Дата последнего обновления"),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true, comment: "Активен ли язык")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Languages", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Names",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false, comment: "Уникальный идентификатор имени")
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false, comment: "Само имя на английском языке (уникальное)"),
                    Description = table.Column<string>(type: "character varying(1024)", maxLength: 1024, nullable: true, comment: "Общее описание имени (не локализованное)"),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP", comment: "Дата создания записи"),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true, comment: "Дата последнего обновления"),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true, comment: "Активна ли запись")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Names", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Roles",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    Name = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Roles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Samples",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Samples", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    FirstName = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false, comment: "Имя пользователя"),
                    LastName = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false, comment: "Фамилия пользователя"),
                    Avatar = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true, comment: "Путь к аватару пользователя"),
                    Department = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true, comment: "Отдел пользователя"),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true, comment: "Активен ли пользователь"),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP", comment: "Дата создания аккаунта"),
                    LastLogin = table.Column<DateTime>(type: "timestamp with time zone", nullable: true, comment: "Дата последнего входа"),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true, comment: "Дата последнего обновления"),
                    EmailConfirmedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true, comment: "Дата подтверждения email"),
                    ExternalProvider = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true, comment: "Внешний провайдер OAuth"),
                    ExternalId = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true, comment: "ID пользователя у внешнего провайдера"),
                    IsExternalAccount = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false, comment: "Создан ли аккаунт через внешнего провайдера"),
                    UserName = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    Email = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    NormalizedEmail = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(type: "boolean", nullable: false),
                    PasswordHash = table.Column<string>(type: "text", nullable: true),
                    SecurityStamp = table.Column<string>(type: "text", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "text", nullable: true),
                    PhoneNumber = table.Column<string>(type: "text", nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(type: "boolean", nullable: false),
                    TwoFactorEnabled = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false, comment: "Включена ли двухфакторная аутентификация"),
                    LockoutEnd = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    LockoutEnabled = table.Column<bool>(type: "boolean", nullable: false),
                    AccessFailedCount = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Anecdotes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    Name = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: false),
                    Description = table.Column<string>(type: "character varying(1024)", maxLength: 1024, nullable: true),
                    NameMainId = table.Column<int>(type: "integer", nullable: false),
                    LanguageId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Anecdotes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Anecdotes_Languages_LanguageId",
                        column: x => x.LanguageId,
                        principalTable: "Languages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Anecdotes_Names_NameMainId",
                        column: x => x.NameMainId,
                        principalTable: "Names",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Animals",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    Name = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: false),
                    Description = table.Column<string>(type: "character varying(1024)", maxLength: 1024, nullable: true),
                    NameMainId = table.Column<int>(type: "integer", nullable: false),
                    LanguageId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Animals", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Animals_Languages_LanguageId",
                        column: x => x.LanguageId,
                        principalTable: "Languages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Animals_Names_NameMainId",
                        column: x => x.NameMainId,
                        principalTable: "Names",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Colors",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    Name = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: false),
                    Description = table.Column<string>(type: "character varying(1024)", maxLength: 1024, nullable: true),
                    NameMainId = table.Column<int>(type: "integer", nullable: false),
                    LanguageId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Colors", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Colors_Languages_LanguageId",
                        column: x => x.LanguageId,
                        principalTable: "Languages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Colors_Names_NameMainId",
                        column: x => x.NameMainId,
                        principalTable: "Names",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Comments",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false, comment: "Уникальный идентификатор комментария")
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    AuthorEmail = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true, comment: "Email автора комментария"),
                    Rating = table.Column<int>(type: "integer", nullable: false, defaultValue: 0, comment: "Рейтинг комментария (количество лайков)"),
                    IsApproved = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false, comment: "Одобрен ли комментарий модератором"),
                    ParentCommentId = table.Column<int>(type: "integer", nullable: true, comment: "Внешний ключ на родительский комментарий (для ответов)"),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP", comment: "Дата создания записи"),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true, comment: "Дата последнего обновления"),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true, comment: "Активна ли запись"),
                    Name = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: false, comment: "Название комментария (может быть заголовком или кратким описанием)"),
                    Description = table.Column<string>(type: "character varying(1024)", maxLength: 1024, nullable: true, comment: "Текст комментария (основной контент)"),
                    NameMainId = table.Column<int>(type: "integer", nullable: false, comment: "Внешний ключ на таблицу Names"),
                    LanguageId = table.Column<int>(type: "integer", nullable: false, comment: "Внешний ключ на таблицу Languages")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Comments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Comments_Comments_ParentCommentId",
                        column: x => x.ParentCommentId,
                        principalTable: "Comments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Comments_Languages_LanguageId",
                        column: x => x.LanguageId,
                        principalTable: "Languages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Comments_Names_NameMainId",
                        column: x => x.NameMainId,
                        principalTable: "Names",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Declensions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Nominative = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: true, comment: "Именительный падеж"),
                    Genitive = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: true, comment: "Родительный падеж"),
                    Dative = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: true, comment: "Дательный падеж"),
                    Accusative = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: true, comment: "Винительный падеж"),
                    Instrumental = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: true, comment: "Творительный падеж"),
                    Prepositional = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: true, comment: "Предложный падеж"),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    Name = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: false),
                    Description = table.Column<string>(type: "character varying(1024)", maxLength: 1024, nullable: true),
                    NameMainId = table.Column<int>(type: "integer", nullable: false),
                    LanguageId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Declensions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Declensions_Languages_LanguageId",
                        column: x => x.LanguageId,
                        principalTable: "Languages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Declensions_Names_NameMainId",
                        column: x => x.NameMainId,
                        principalTable: "Names",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Facts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    Name = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: false),
                    Description = table.Column<string>(type: "character varying(1024)", maxLength: 1024, nullable: true),
                    NameMainId = table.Column<int>(type: "integer", nullable: false),
                    LanguageId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Facts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Facts_Languages_LanguageId",
                        column: x => x.LanguageId,
                        principalTable: "Languages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Facts_Names_NameMainId",
                        column: x => x.NameMainId,
                        principalTable: "Names",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ForeignVariants",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    Name = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: false),
                    Description = table.Column<string>(type: "character varying(1024)", maxLength: 1024, nullable: true),
                    NameMainId = table.Column<int>(type: "integer", nullable: false),
                    LanguageId = table.Column<int>(type: "integer", nullable: false),
                    Url = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true, comment: "URL ссылка на внешний ресурс")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ForeignVariants", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ForeignVariants_Languages_LanguageId",
                        column: x => x.LanguageId,
                        principalTable: "Languages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ForeignVariants_Names_NameMainId",
                        column: x => x.NameMainId,
                        principalTable: "Names",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Metals",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    Name = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: false),
                    Description = table.Column<string>(type: "character varying(1024)", maxLength: 1024, nullable: true),
                    NameMainId = table.Column<int>(type: "integer", nullable: false),
                    LanguageId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Metals", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Metals_Languages_LanguageId",
                        column: x => x.LanguageId,
                        principalTable: "Languages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Metals_Names_NameMainId",
                        column: x => x.NameMainId,
                        principalTable: "Names",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "NameDetails",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false, comment: "Уникальный идентификатор записи описания")
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: true, comment: "Имя на языке"),
                    FullName = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: true, comment: "Полное имя"),
                    Diminutiv = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: true, comment: "Уменьшительно-ласкательное имя"),
                    NameRod = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: true, comment: "Родительный падеж имени"),
                    NameFemaleCouple = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true, comment: "Женское парное имя"),
                    NameDays = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true, comment: "Именины"),
                    OrthodoxName = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: true, comment: "Имя в православии"),
                    OtherFormName = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true, comment: "Другие формы имени"),
                    Synonyms = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true, comment: "Синонимы"),
                    Surname = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true, comment: "Отчество"),
                    Abbreviated = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: true, comment: "Сокращенное имя"),
                    Gender = table.Column<int>(type: "integer", nullable: true, comment: "Пол имени"),
                    Description = table.Column<string>(type: "character varying(1024)", maxLength: 1024, nullable: true, comment: "Полное описание значения имени"),
                    DescriptionShort = table.Column<string>(type: "text", nullable: true, comment: "Краткое описание имени"),
                    ShortMeaning = table.Column<string>(type: "text", nullable: true, comment: "Краткое значение/перевод имени"),
                    Origin = table.Column<string>(type: "text", nullable: true, comment: "Происхождение/этимология имени"),
                    Etymology = table.Column<string>(type: "character varying(1024)", maxLength: 1024, nullable: true, comment: "Этимология имени"),
                    Meaning = table.Column<string>(type: "character varying(1024)", maxLength: 1024, nullable: true, comment: "Значение имени"),
                    NameMeaning = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true, comment: "Значение имени"),
                    OriginHistory = table.Column<string>(type: "character varying(1024)", maxLength: 1024, nullable: true, comment: "История происхождения"),
                    Country = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: true, comment: "Страна"),
                    Pronunciation = table.Column<string>(type: "character varying(512)", maxLength: 512, nullable: true, comment: "Произношение"),
                    FormsName = table.Column<string>(type: "character varying(512)", maxLength: 512, nullable: true, comment: "Производные формы"),
                    LuckyDay = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: true, comment: "Благоприятный день"),
                    LuckyDayDescription = table.Column<string>(type: "character varying(512)", maxLength: 512, nullable: true, comment: "Описание благоприятного дня"),
                    CharacterTraits = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true, comment: "Черты характера"),
                    AdultCharacter = table.Column<string>(type: "character varying(2056)", maxLength: 2056, nullable: true, comment: "Характер взрослого"),
                    Teenager = table.Column<string>(type: "character varying(512)", maxLength: 512, nullable: true, comment: "Подросток"),
                    Personality = table.Column<string>(type: "character varying(512)", maxLength: 512, nullable: true, comment: "Личность"),
                    MainFeatures = table.Column<string>(type: "character varying(512)", maxLength: 512, nullable: true, comment: "Основные черты"),
                    PositiveCharacteristic = table.Column<string>(type: "character varying(512)", maxLength: 512, nullable: true, comment: "Позитивные характеристики"),
                    NegativeCharacteristic = table.Column<string>(type: "character varying(512)", maxLength: 512, nullable: true, comment: "Негативные характеристики"),
                    Type = table.Column<string>(type: "character varying(512)", maxLength: 512, nullable: true, comment: "Тип"),
                    Psyche = table.Column<string>(type: "character varying(512)", maxLength: 512, nullable: true, comment: "Психика"),
                    Will = table.Column<string>(type: "character varying(512)", maxLength: 512, nullable: true, comment: "Воля"),
                    Excitability = table.Column<string>(type: "character varying(512)", maxLength: 512, nullable: true, comment: "Возбудимость"),
                    ReactionSpeed = table.Column<string>(type: "character varying(512)", maxLength: 512, nullable: true, comment: "Скорость реакции"),
                    Intuition = table.Column<string>(type: "character varying(512)", maxLength: 512, nullable: true, comment: "Интуиция"),
                    Intellect = table.Column<string>(type: "character varying(512)", maxLength: 512, nullable: true, comment: "Интеллект"),
                    Sensitivity = table.Column<string>(type: "character varying(512)", maxLength: 512, nullable: true, comment: "Восприимчивость"),
                    Morality = table.Column<string>(type: "character varying(512)", maxLength: 512, nullable: true, comment: "Нравственность"),
                    Activity = table.Column<string>(type: "character varying(512)", maxLength: 512, nullable: true, comment: "Активность"),
                    Sociability = table.Column<string>(type: "character varying(512)", maxLength: 512, nullable: true, comment: "Общительность"),
                    Energy = table.Column<string>(type: "character varying(512)", maxLength: 512, nullable: true, comment: "Энергетика имени"),
                    ByHigir = table.Column<string>(type: "character varying(512)", maxLength: 512, nullable: true, comment: "По Хигиру"),
                    ByMendeleev = table.Column<string>(type: "character varying(512)", maxLength: 512, nullable: true, comment: "По Менделееву"),
                    PhonosemanticAnalysis = table.Column<string>(type: "character varying(1024)", maxLength: 1024, nullable: true, comment: "Фоносемантический анализ"),
                    ProfessionAndBusiness = table.Column<string>(type: "character varying(1024)", maxLength: 1024, nullable: true, comment: "Профессия и бизнес"),
                    ProfessionAndCareer = table.Column<string>(type: "character varying(1024)", maxLength: 1024, nullable: true, comment: "Профессия и карьера"),
                    Business = table.Column<string>(type: "character varying(512)", maxLength: 512, nullable: true, comment: "Бизнес"),
                    FieldOfActivity = table.Column<string>(type: "character varying(512)", maxLength: 512, nullable: true, comment: "Поле деятельности"),
                    Health = table.Column<string>(type: "character varying(1024)", maxLength: 1024, nullable: true, comment: "Здоровье"),
                    Hobbies = table.Column<string>(type: "character varying(1024)", maxLength: 1024, nullable: true, comment: "Увлечения и хобби"),
                    SexAndLove = table.Column<string>(type: "character varying(1024)", maxLength: 1024, nullable: true, comment: "Секс и любовь"),
                    LoveAndSexuality = table.Column<string>(type: "character varying(1024)", maxLength: 1024, nullable: true, comment: "Любовь и сексуальность"),
                    Love = table.Column<string>(type: "character varying(512)", maxLength: 512, nullable: true, comment: "Любовь"),
                    Sexuality = table.Column<string>(type: "character varying(512)", maxLength: 512, nullable: true, comment: "Сексуальность"),
                    FamilyAndMarriage = table.Column<string>(type: "character varying(1024)", maxLength: 1024, nullable: true, comment: "Семья и брак"),
                    MarriageAndFamily = table.Column<string>(type: "character varying(1024)", maxLength: 1024, nullable: true, comment: "Брак и семья"),
                    Marriage = table.Column<string>(type: "character varying(512)", maxLength: 512, nullable: true, comment: "Брак"),
                    MarriageBehavior = table.Column<string>(type: "character varying(1024)", maxLength: 1024, nullable: true, comment: "Поведение в браке"),
                    Fate = table.Column<string>(type: "character varying(512)", maxLength: 512, nullable: true, comment: "Судьба имени"),
                    HarmonyWith = table.Column<string>(type: "character varying(1024)", maxLength: 1024, nullable: true, comment: "Гармоничные имена"),
                    IncompatibilityWith = table.Column<string>(type: "character varying(1024)", maxLength: 1024, nullable: true, comment: "Негармоничные имена"),
                    NameMainId = table.Column<int>(type: "integer", nullable: false, comment: "Внешний ключ на таблицу Names"),
                    LanguageId = table.Column<int>(type: "integer", nullable: false, comment: "Внешний ключ на таблицу Languages"),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP", comment: "Дата создания записи"),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true, comment: "Дата последнего обновления"),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true, comment: "Активна ли запись")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NameDetails", x => x.Id);
                    table.ForeignKey(
                        name: "FK_NameDetails_Languages_LanguageId",
                        column: x => x.LanguageId,
                        principalTable: "Languages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_NameDetails_Names_NameMainId",
                        column: x => x.NameMainId,
                        principalTable: "Names",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "NameUrlForParsings",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Status = table.Column<string>(type: "text", nullable: false, comment: "Статус парсинга URL: NotReady/Processing/Ready/Failed"),
                    LastParsedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true, comment: "Дата последнего успешного парсинга"),
                    ParseAttempts = table.Column<int>(type: "integer", nullable: false, defaultValue: 0, comment: "Количество попыток парсинга"),
                    ErrorMessage = table.Column<string>(type: "character varying(1024)", maxLength: 1024, nullable: true, comment: "Сообщение об ошибке парсинга"),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    Name = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: false),
                    Description = table.Column<string>(type: "character varying(1024)", maxLength: 1024, nullable: true),
                    NameMainId = table.Column<int>(type: "integer", nullable: false),
                    LanguageId = table.Column<int>(type: "integer", nullable: false),
                    Url = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true, comment: "URL для парсинга данных об имени")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NameUrlForParsings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_NameUrlForParsings_Languages_LanguageId",
                        column: x => x.LanguageId,
                        principalTable: "Languages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_NameUrlForParsings_Names_NameMainId",
                        column: x => x.NameMainId,
                        principalTable: "Names",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Numbers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    Name = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: false),
                    Description = table.Column<string>(type: "character varying(1024)", maxLength: 1024, nullable: true),
                    NameMainId = table.Column<int>(type: "integer", nullable: false),
                    LanguageId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Numbers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Numbers_Languages_LanguageId",
                        column: x => x.LanguageId,
                        principalTable: "Languages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Numbers_Names_NameMainId",
                        column: x => x.NameMainId,
                        principalTable: "Names",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Patrons",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    Name = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: false),
                    Description = table.Column<string>(type: "character varying(1024)", maxLength: 1024, nullable: true),
                    NameMainId = table.Column<int>(type: "integer", nullable: false),
                    LanguageId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Patrons", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Patrons_Languages_LanguageId",
                        column: x => x.LanguageId,
                        principalTable: "Languages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Patrons_Names_NameMainId",
                        column: x => x.NameMainId,
                        principalTable: "Names",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Planets",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Symbol = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: true, comment: "Астрономический символ планеты"),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    Name = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: false),
                    Description = table.Column<string>(type: "character varying(1024)", maxLength: 1024, nullable: true),
                    NameMainId = table.Column<int>(type: "integer", nullable: false),
                    LanguageId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Planets", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Planets_Languages_LanguageId",
                        column: x => x.LanguageId,
                        principalTable: "Languages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Planets_Names_NameMainId",
                        column: x => x.NameMainId,
                        principalTable: "Names",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Plants",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    Name = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: false),
                    Description = table.Column<string>(type: "character varying(1024)", maxLength: 1024, nullable: true),
                    NameMainId = table.Column<int>(type: "integer", nullable: false),
                    LanguageId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Plants", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Plants_Languages_LanguageId",
                        column: x => x.LanguageId,
                        principalTable: "Languages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Plants_Names_NameMainId",
                        column: x => x.NameMainId,
                        principalTable: "Names",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Professions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    Name = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: false),
                    Description = table.Column<string>(type: "character varying(1024)", maxLength: 1024, nullable: true),
                    NameMainId = table.Column<int>(type: "integer", nullable: false),
                    LanguageId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Professions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Professions_Languages_LanguageId",
                        column: x => x.LanguageId,
                        principalTable: "Languages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Professions_Names_NameMainId",
                        column: x => x.NameMainId,
                        principalTable: "Names",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SeoData",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false, comment: "Уникальный идентификатор SEO-данных")
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    NameMainId = table.Column<int>(type: "integer", nullable: false, comment: "Внешний ключ на таблицу Names"),
                    LanguageId = table.Column<int>(type: "integer", nullable: false, comment: "Внешний ключ на таблицу Languages"),
                    MetaTitle = table.Column<string>(type: "character varying(70)", maxLength: 70, nullable: true, comment: "SEO заголовок (рекомендуется до 70 символов)"),
                    MetaDescription = table.Column<string>(type: "character varying(160)", maxLength: 160, nullable: true, comment: "META описание (рекомендуется до 160 символов)"),
                    MetaKeywords = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true, comment: "META ключевые слова"),
                    UrlSlug = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true, comment: "ЧПУ URL"),
                    CanonicalUrl = table.Column<string>(type: "character varying(300)", maxLength: 300, nullable: true, comment: "Каноническая ссылка"),
                    OgTitle = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true, comment: "Open Graph заголовок"),
                    OgDescription = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true, comment: "Open Graph описание"),
                    OgImage = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true, comment: "Open Graph изображение"),
                    OgType = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true, defaultValue: "article", comment: "Open Graph тип контента"),
                    OgUrl = table.Column<string>(type: "character varying(300)", maxLength: 300, nullable: true, comment: "Open Graph URL"),
                    TwitterCard = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true, defaultValue: "summary_large_image", comment: "Twitter Card тип"),
                    TwitterTitle = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true, comment: "Twitter заголовок"),
                    TwitterDescription = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true, comment: "Twitter описание"),
                    TwitterImage = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true, comment: "Twitter изображение"),
                    ImageUrl = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true, defaultValue: "", comment: "URL изображения"),
                    ImageAltText = table.Column<string>(type: "character varying(125)", maxLength: 125, nullable: true, comment: "Alt текст изображения"),
                    ImageCaption = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true, comment: "Подпись изображения"),
                    SchemaType = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true, comment: "Тип схемы Schema.org"),
                    SchemaJsonLd = table.Column<string>(type: "jsonb", nullable: true, comment: "JSON-LD разметка"),
                    AuthorName = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true, defaultValue: "", comment: "Имя автора"),
                    PublisherName = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true, defaultValue: "", comment: "Имя издателя"),
                    PublishedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true, comment: "Дата публикации"),
                    ModifiedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true, comment: "Дата последнего изменения"),
                    ArticleSection = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true, defaultValue: "", comment: "Раздел статьи"),
                    NoIndex = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false, comment: "Запрет индексации (robots noindex)"),
                    NoFollow = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false, comment: "Запрет следования по ссылкам (robots nofollow)"),
                    Priority = table.Column<int>(type: "integer", nullable: true, defaultValue: 5, comment: "Приоритет страницы для sitemap.xml (0-10)"),
                    Region = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true, comment: "Географический регион"),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP", comment: "Дата создания записи"),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true, comment: "Дата последнего обновления"),
                    CreatedBy = table.Column<string>(type: "character varying(450)", maxLength: 450, nullable: true, comment: "Пользователь, создавший запись"),
                    UpdatedBy = table.Column<string>(type: "character varying(450)", maxLength: 450, nullable: true, comment: "Пользователь, обновивший запись")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SeoData", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SeoData_Languages_LanguageId",
                        column: x => x.LanguageId,
                        principalTable: "Languages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SeoData_Names_NameMainId",
                        column: x => x.NameMainId,
                        principalTable: "Names",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Stones",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    Name = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: false),
                    Description = table.Column<string>(type: "character varying(1024)", maxLength: 1024, nullable: true),
                    NameMainId = table.Column<int>(type: "integer", nullable: false),
                    LanguageId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Stones", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Stones_Languages_LanguageId",
                        column: x => x.LanguageId,
                        principalTable: "Languages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Stones_Names_NameMainId",
                        column: x => x.NameMainId,
                        principalTable: "Names",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Synonyms",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    Name = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: false),
                    Description = table.Column<string>(type: "character varying(1024)", maxLength: 1024, nullable: true),
                    NameMainId = table.Column<int>(type: "integer", nullable: false),
                    LanguageId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Synonyms", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Synonyms_Languages_LanguageId",
                        column: x => x.LanguageId,
                        principalTable: "Languages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Synonyms_Names_NameMainId",
                        column: x => x.NameMainId,
                        principalTable: "Names",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Talents",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    Name = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: false),
                    Description = table.Column<string>(type: "character varying(1024)", maxLength: 1024, nullable: true),
                    NameMainId = table.Column<int>(type: "integer", nullable: false),
                    LanguageId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Talents", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Talents_Languages_LanguageId",
                        column: x => x.LanguageId,
                        principalTable: "Languages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Talents_Names_NameMainId",
                        column: x => x.NameMainId,
                        principalTable: "Names",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Trees",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    Name = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: false),
                    Description = table.Column<string>(type: "character varying(1024)", maxLength: 1024, nullable: true),
                    NameMainId = table.Column<int>(type: "integer", nullable: false),
                    LanguageId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Trees", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Trees_Languages_LanguageId",
                        column: x => x.LanguageId,
                        principalTable: "Languages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Trees_Names_NameMainId",
                        column: x => x.NameMainId,
                        principalTable: "Names",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Zodiacs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ZodiacSign = table.Column<int>(type: "integer", nullable: false, comment: "Номер знака зодиака (1-12)"),
                    StartMonth = table.Column<byte>(type: "smallint", nullable: false, comment: "Месяц начала периода знака (1-12)"),
                    StartDay = table.Column<byte>(type: "smallint", nullable: false, comment: "День начала периода знака (1-31)"),
                    EndMonth = table.Column<byte>(type: "smallint", nullable: false, comment: "Месяц окончания периода знака (1-12)"),
                    EndDay = table.Column<byte>(type: "smallint", nullable: false, comment: "День окончания периода знака (1-31)"),
                    Symbol = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: true, comment: "Астрологический символ знака"),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    Name = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: false),
                    Description = table.Column<string>(type: "character varying(1024)", maxLength: 1024, nullable: true),
                    NameMainId = table.Column<int>(type: "integer", nullable: false),
                    LanguageId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Zodiacs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Zodiacs_Languages_LanguageId",
                        column: x => x.LanguageId,
                        principalTable: "Languages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Zodiacs_Names_NameMainId",
                        column: x => x.NameMainId,
                        principalTable: "Names",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RoleClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    RoleId = table.Column<string>(type: "text", nullable: false),
                    ClaimType = table.Column<string>(type: "text", nullable: true),
                    ClaimValue = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RoleClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RoleClaims_Roles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "Roles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ActivityLogs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false, comment: "Уникальный идентификатор записи лога")
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Action = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false, comment: "Тип действия"),
                    EntityType = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true, comment: "Тип сущности над которой выполнено действие"),
                    EntityId = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true, comment: "ID сущности над которой выполнено действие"),
                    Details = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true, comment: "Дополнительная информация о действии"),
                    Success = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true, comment: "Успешно ли выполнено действие"),
                    Timestamp = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP", comment: "Время выполнения действия"),
                    DeviceType = table.Column<int>(type: "integer", nullable: false, comment: "Тип устройства, с которого выполнено действие"),
                    UserId = table.Column<string>(type: "text", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP", comment: "Дата создания записи"),
                    IpAddress = table.Column<string>(type: "character varying(45)", maxLength: 45, nullable: true, comment: "IP адрес откуда выполнено действие"),
                    UserAgent = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true, comment: "User Agent браузера")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ActivityLogs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ActivityLogs_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    UserId = table.Column<string>(type: "text", nullable: false),
                    ClaimType = table.Column<string>(type: "text", nullable: true),
                    ClaimValue = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserClaims_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserLogins",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(type: "text", nullable: false),
                    ProviderKey = table.Column<string>(type: "text", nullable: false),
                    ProviderDisplayName = table.Column<string>(type: "text", nullable: true),
                    UserId = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserLogins", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_UserLogins_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserRoles",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "text", nullable: false),
                    RoleId = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_UserRoles_Roles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "Roles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserRoles_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserSessions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false, comment: "Уникальный идентификатор сессии")
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    RefreshToken = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false, comment: "Refresh токен"),
                    ExpiresAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, comment: "Дата окончания действия токена"),
                    IsRevoked = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false, comment: "Отозван ли токен"),
                    RevokedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true, comment: "Время аннулирования сессии"),
                    DeviceInfo = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true, comment: "Информация об устройстве (браузер, ОС)"),
                    UserId = table.Column<string>(type: "text", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP", comment: "Дата создания сессии"),
                    IpAddress = table.Column<string>(type: "character varying(45)", maxLength: 45, nullable: true, comment: "IP адрес создания сессии"),
                    UserAgent = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true, comment: "User Agent браузера")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserSessions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserSessions_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserTokens",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "text", nullable: false),
                    LoginProvider = table.Column<string>(type: "text", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Value = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: "FK_UserTokens_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "HoroscopesOfNames",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ZodiacId = table.Column<int>(type: "integer", nullable: false),
                    HoroscopeDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true, comment: "Дата гороскопа"),
                    HoroscopeType = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: true, comment: "Тип гороскопа (ежедневный, еженедельный, годовой)"),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    Name = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: false),
                    Description = table.Column<string>(type: "character varying(1024)", maxLength: 1024, nullable: true),
                    NameMainId = table.Column<int>(type: "integer", nullable: false),
                    LanguageId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HoroscopesOfNames", x => x.Id);
                    table.ForeignKey(
                        name: "FK_HoroscopesOfNames_Languages_LanguageId",
                        column: x => x.LanguageId,
                        principalTable: "Languages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_HoroscopesOfNames_Names_NameMainId",
                        column: x => x.NameMainId,
                        principalTable: "Names",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_HoroscopesOfNames_Zodiacs_ZodiacId",
                        column: x => x.ZodiacId,
                        principalTable: "Zodiacs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ZodiacHoroscopes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ZodiacId = table.Column<int>(type: "integer", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    Name = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: false),
                    Description = table.Column<string>(type: "character varying(1024)", maxLength: 1024, nullable: true),
                    NameMainId = table.Column<int>(type: "integer", nullable: false),
                    LanguageId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ZodiacHoroscopes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ZodiacHoroscopes_Languages_LanguageId",
                        column: x => x.LanguageId,
                        principalTable: "Languages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ZodiacHoroscopes_Names_NameMainId",
                        column: x => x.NameMainId,
                        principalTable: "Names",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ZodiacHoroscopes_Zodiacs_ZodiacId",
                        column: x => x.ZodiacId,
                        principalTable: "Zodiacs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ZodiacTalismans",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ZodiacId = table.Column<int>(type: "integer", nullable: false),
                    TalismanType = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: true, comment: "Тип талисмана (камень, металл, растение)"),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    Name = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: false),
                    Description = table.Column<string>(type: "character varying(1024)", maxLength: 1024, nullable: true),
                    NameMainId = table.Column<int>(type: "integer", nullable: false),
                    LanguageId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ZodiacTalismans", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ZodiacTalismans_Languages_LanguageId",
                        column: x => x.LanguageId,
                        principalTable: "Languages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ZodiacTalismans_Names_NameMainId",
                        column: x => x.NameMainId,
                        principalTable: "Names",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ZodiacTalismans_Zodiacs_ZodiacId",
                        column: x => x.ZodiacId,
                        principalTable: "Zodiacs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                table: "Languages",
                columns: new[] { "Id", "Code", "CreatedAt", "Description", "DisplayOrder", "FlagCode", "IsActive", "IsDefault", "Name", "TextDirection", "UpdatedAt" },
                values: new object[] { 1, "en", new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, 1, "gb", true, true, "English", "ltr", null });

            migrationBuilder.InsertData(
                table: "Languages",
                columns: new[] { "Id", "Code", "CreatedAt", "Description", "DisplayOrder", "FlagCode", "IsActive", "Name", "TextDirection", "UpdatedAt" },
                values: new object[,]
                {
                    { 2, "ru", new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, 2, "ru", true, "Русский", "ltr", null },
                    { 3, "es", new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, 3, "es", true, "Español", "ltr", null },
                    { 4, "fr", new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, 4, "fr", true, "Français", "ltr", null },
                    { 5, "de", new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, 5, "de", true, "Deutsch", "ltr", null },
                    { 6, "it", new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, 6, "it", true, "Italiano", "ltr", null },
                    { 7, "pt", new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, 7, "pt", true, "Português", "ltr", null }
                });

            migrationBuilder.CreateIndex(
                name: "IX_ActivityLogs_Action",
                table: "ActivityLogs",
                column: "Action");

            migrationBuilder.CreateIndex(
                name: "IX_ActivityLogs_Action_Timestamp",
                table: "ActivityLogs",
                columns: new[] { "Action", "Timestamp" });

            migrationBuilder.CreateIndex(
                name: "IX_ActivityLogs_CreatedAt",
                table: "ActivityLogs",
                column: "CreatedAt");

            migrationBuilder.CreateIndex(
                name: "IX_ActivityLogs_DeviceType",
                table: "ActivityLogs",
                column: "DeviceType");

            migrationBuilder.CreateIndex(
                name: "IX_ActivityLogs_EntityType",
                table: "ActivityLogs",
                column: "EntityType");

            migrationBuilder.CreateIndex(
                name: "IX_ActivityLogs_EntityType_EntityId",
                table: "ActivityLogs",
                columns: new[] { "EntityType", "EntityId" });

            migrationBuilder.CreateIndex(
                name: "IX_ActivityLogs_Success",
                table: "ActivityLogs",
                column: "Success");

            migrationBuilder.CreateIndex(
                name: "IX_ActivityLogs_Success_Timestamp",
                table: "ActivityLogs",
                columns: new[] { "Success", "Timestamp" });

            migrationBuilder.CreateIndex(
                name: "IX_ActivityLogs_Timestamp",
                table: "ActivityLogs",
                column: "Timestamp");

            migrationBuilder.CreateIndex(
                name: "IX_ActivityLogs_UserId",
                table: "ActivityLogs",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_ActivityLogs_UserId_Timestamp",
                table: "ActivityLogs",
                columns: new[] { "UserId", "Timestamp" });

            migrationBuilder.CreateIndex(
                name: "IX_Anecdotes_LanguageId",
                table: "Anecdotes",
                column: "LanguageId");

            migrationBuilder.CreateIndex(
                name: "IX_Anecdotes_NameMainId",
                table: "Anecdotes",
                column: "NameMainId");

            migrationBuilder.CreateIndex(
                name: "IX_Anecdotes_NameMainId_LanguageId",
                table: "Anecdotes",
                columns: new[] { "NameMainId", "LanguageId" });

            migrationBuilder.CreateIndex(
                name: "IX_Animals_LanguageId",
                table: "Animals",
                column: "LanguageId");

            migrationBuilder.CreateIndex(
                name: "IX_Animals_NameMainId",
                table: "Animals",
                column: "NameMainId");

            migrationBuilder.CreateIndex(
                name: "IX_Animals_NameMainId_LanguageId",
                table: "Animals",
                columns: new[] { "NameMainId", "LanguageId" });

            migrationBuilder.CreateIndex(
                name: "IX_Colors_LanguageId",
                table: "Colors",
                column: "LanguageId");

            migrationBuilder.CreateIndex(
                name: "IX_Colors_NameMainId",
                table: "Colors",
                column: "NameMainId");

            migrationBuilder.CreateIndex(
                name: "IX_Colors_NameMainId_LanguageId",
                table: "Colors",
                columns: new[] { "NameMainId", "LanguageId" });

            migrationBuilder.CreateIndex(
                name: "IX_Comments_CreatedAt",
                table: "Comments",
                column: "CreatedAt");

            migrationBuilder.CreateIndex(
                name: "IX_Comments_IsApproved",
                table: "Comments",
                column: "IsApproved");

            migrationBuilder.CreateIndex(
                name: "IX_Comments_IsApproved_CreatedAt",
                table: "Comments",
                columns: new[] { "IsApproved", "CreatedAt" });

            migrationBuilder.CreateIndex(
                name: "IX_Comments_LanguageId",
                table: "Comments",
                column: "LanguageId");

            migrationBuilder.CreateIndex(
                name: "IX_Comments_NameMainId",
                table: "Comments",
                column: "NameMainId");

            migrationBuilder.CreateIndex(
                name: "IX_Comments_NameMainId_LanguageId",
                table: "Comments",
                columns: new[] { "NameMainId", "LanguageId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Comments_ParentCommentId",
                table: "Comments",
                column: "ParentCommentId");

            migrationBuilder.CreateIndex(
                name: "IX_Comments_Rating",
                table: "Comments",
                column: "Rating");

            migrationBuilder.CreateIndex(
                name: "IX_Declensions_LanguageId",
                table: "Declensions",
                column: "LanguageId");

            migrationBuilder.CreateIndex(
                name: "IX_Declensions_NameMainId",
                table: "Declensions",
                column: "NameMainId");

            migrationBuilder.CreateIndex(
                name: "IX_Declensions_NameMainId_LanguageId",
                table: "Declensions",
                columns: new[] { "NameMainId", "LanguageId" });

            migrationBuilder.CreateIndex(
                name: "IX_Facts_LanguageId",
                table: "Facts",
                column: "LanguageId");

            migrationBuilder.CreateIndex(
                name: "IX_Facts_NameMainId",
                table: "Facts",
                column: "NameMainId");

            migrationBuilder.CreateIndex(
                name: "IX_Facts_NameMainId_LanguageId",
                table: "Facts",
                columns: new[] { "NameMainId", "LanguageId" });

            migrationBuilder.CreateIndex(
                name: "IX_ForeignVariants_LanguageId",
                table: "ForeignVariants",
                column: "LanguageId");

            migrationBuilder.CreateIndex(
                name: "IX_ForeignVariants_NameMainId",
                table: "ForeignVariants",
                column: "NameMainId");

            migrationBuilder.CreateIndex(
                name: "IX_ForeignVariants_NameMainId_LanguageId",
                table: "ForeignVariants",
                columns: new[] { "NameMainId", "LanguageId" });

            migrationBuilder.CreateIndex(
                name: "IX_HoroscopesOfNames_LanguageId",
                table: "HoroscopesOfNames",
                column: "LanguageId");

            migrationBuilder.CreateIndex(
                name: "IX_HoroscopesOfNames_NameMainId",
                table: "HoroscopesOfNames",
                column: "NameMainId");

            migrationBuilder.CreateIndex(
                name: "IX_HoroscopesOfNames_ZodiacId",
                table: "HoroscopesOfNames",
                column: "ZodiacId");

            migrationBuilder.CreateIndex(
                name: "IX_HoroscopesOfNames_ZodiacId_NameMainId_LanguageId",
                table: "HoroscopesOfNames",
                columns: new[] { "ZodiacId", "NameMainId", "LanguageId" });

            migrationBuilder.CreateIndex(
                name: "IX_Languages_Code",
                table: "Languages",
                column: "Code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Languages_DisplayOrder",
                table: "Languages",
                column: "DisplayOrder");

            migrationBuilder.CreateIndex(
                name: "IX_Languages_IsActive",
                table: "Languages",
                column: "IsActive");

            migrationBuilder.CreateIndex(
                name: "IX_Metals_LanguageId",
                table: "Metals",
                column: "LanguageId");

            migrationBuilder.CreateIndex(
                name: "IX_Metals_NameMainId",
                table: "Metals",
                column: "NameMainId");

            migrationBuilder.CreateIndex(
                name: "IX_Metals_NameMainId_LanguageId",
                table: "Metals",
                columns: new[] { "NameMainId", "LanguageId" });

            migrationBuilder.CreateIndex(
                name: "IX_NameDetails_Gender",
                table: "NameDetails",
                column: "Gender");

            migrationBuilder.CreateIndex(
                name: "IX_NameDetails_LanguageId",
                table: "NameDetails",
                column: "LanguageId");

            migrationBuilder.CreateIndex(
                name: "IX_NameDetails_NameMainId",
                table: "NameDetails",
                column: "NameMainId");

            migrationBuilder.CreateIndex(
                name: "IX_NameDetails_NameMainId_LanguageId",
                table: "NameDetails",
                columns: new[] { "NameMainId", "LanguageId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_NameDetails_Origin",
                table: "NameDetails",
                column: "Origin");

            migrationBuilder.CreateIndex(
                name: "IX_Names_CreatedAt",
                table: "Names",
                column: "CreatedAt");

            migrationBuilder.CreateIndex(
                name: "IX_Names_IsActive",
                table: "Names",
                column: "IsActive");

            migrationBuilder.CreateIndex(
                name: "IX_Names_Name",
                table: "Names",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_NameUrlForParsings_LanguageId",
                table: "NameUrlForParsings",
                column: "LanguageId");

            migrationBuilder.CreateIndex(
                name: "IX_NameUrlForParsings_LastParsedAt",
                table: "NameUrlForParsings",
                column: "LastParsedAt");

            migrationBuilder.CreateIndex(
                name: "IX_NameUrlForParsings_NameMainId",
                table: "NameUrlForParsings",
                column: "NameMainId");

            migrationBuilder.CreateIndex(
                name: "IX_NameUrlForParsings_NameMainId_LanguageId",
                table: "NameUrlForParsings",
                columns: new[] { "NameMainId", "LanguageId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_NameUrlForParsings_Status",
                table: "NameUrlForParsings",
                column: "Status");

            migrationBuilder.CreateIndex(
                name: "IX_NameUrlForParsings_Status_ParseAttempts",
                table: "NameUrlForParsings",
                columns: new[] { "Status", "ParseAttempts" });

            migrationBuilder.CreateIndex(
                name: "IX_Numbers_LanguageId",
                table: "Numbers",
                column: "LanguageId");

            migrationBuilder.CreateIndex(
                name: "IX_Numbers_NameMainId",
                table: "Numbers",
                column: "NameMainId");

            migrationBuilder.CreateIndex(
                name: "IX_Numbers_NameMainId_LanguageId",
                table: "Numbers",
                columns: new[] { "NameMainId", "LanguageId" });

            migrationBuilder.CreateIndex(
                name: "IX_Patrons_LanguageId",
                table: "Patrons",
                column: "LanguageId");

            migrationBuilder.CreateIndex(
                name: "IX_Patrons_NameMainId",
                table: "Patrons",
                column: "NameMainId");

            migrationBuilder.CreateIndex(
                name: "IX_Patrons_NameMainId_LanguageId",
                table: "Patrons",
                columns: new[] { "NameMainId", "LanguageId" });

            migrationBuilder.CreateIndex(
                name: "IX_Planets_LanguageId",
                table: "Planets",
                column: "LanguageId");

            migrationBuilder.CreateIndex(
                name: "IX_Planets_NameMainId",
                table: "Planets",
                column: "NameMainId");

            migrationBuilder.CreateIndex(
                name: "IX_Planets_NameMainId_LanguageId",
                table: "Planets",
                columns: new[] { "NameMainId", "LanguageId" });

            migrationBuilder.CreateIndex(
                name: "IX_Plants_LanguageId",
                table: "Plants",
                column: "LanguageId");

            migrationBuilder.CreateIndex(
                name: "IX_Plants_NameMainId",
                table: "Plants",
                column: "NameMainId");

            migrationBuilder.CreateIndex(
                name: "IX_Plants_NameMainId_LanguageId",
                table: "Plants",
                columns: new[] { "NameMainId", "LanguageId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Professions_LanguageId",
                table: "Professions",
                column: "LanguageId");

            migrationBuilder.CreateIndex(
                name: "IX_Professions_NameMainId",
                table: "Professions",
                column: "NameMainId");

            migrationBuilder.CreateIndex(
                name: "IX_Professions_NameMainId_LanguageId",
                table: "Professions",
                columns: new[] { "NameMainId", "LanguageId" });

            migrationBuilder.CreateIndex(
                name: "IX_RoleClaims_RoleId",
                table: "RoleClaims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "Roles",
                column: "NormalizedName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_SeoData_LanguageId",
                table: "SeoData",
                column: "LanguageId");

            migrationBuilder.CreateIndex(
                name: "IX_SeoData_NameMainId",
                table: "SeoData",
                column: "NameMainId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_SeoData_NameMainId_LanguageId",
                table: "SeoData",
                columns: new[] { "NameMainId", "LanguageId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_SeoData_NoIndex",
                table: "SeoData",
                column: "NoIndex");

            migrationBuilder.CreateIndex(
                name: "IX_SeoData_Priority",
                table: "SeoData",
                column: "Priority");

            migrationBuilder.CreateIndex(
                name: "IX_SeoData_UrlSlug",
                table: "SeoData",
                column: "UrlSlug");

            migrationBuilder.CreateIndex(
                name: "IX_Stones_LanguageId",
                table: "Stones",
                column: "LanguageId");

            migrationBuilder.CreateIndex(
                name: "IX_Stones_NameMainId",
                table: "Stones",
                column: "NameMainId");

            migrationBuilder.CreateIndex(
                name: "IX_Stones_NameMainId_LanguageId",
                table: "Stones",
                columns: new[] { "NameMainId", "LanguageId" });

            migrationBuilder.CreateIndex(
                name: "IX_Synonyms_LanguageId",
                table: "Synonyms",
                column: "LanguageId");

            migrationBuilder.CreateIndex(
                name: "IX_Synonyms_NameMainId",
                table: "Synonyms",
                column: "NameMainId");

            migrationBuilder.CreateIndex(
                name: "IX_Synonyms_NameMainId_LanguageId",
                table: "Synonyms",
                columns: new[] { "NameMainId", "LanguageId" });

            migrationBuilder.CreateIndex(
                name: "IX_Talents_LanguageId",
                table: "Talents",
                column: "LanguageId");

            migrationBuilder.CreateIndex(
                name: "IX_Talents_NameMainId",
                table: "Talents",
                column: "NameMainId");

            migrationBuilder.CreateIndex(
                name: "IX_Talents_NameMainId_LanguageId",
                table: "Talents",
                columns: new[] { "NameMainId", "LanguageId" });

            migrationBuilder.CreateIndex(
                name: "IX_Trees_LanguageId",
                table: "Trees",
                column: "LanguageId");

            migrationBuilder.CreateIndex(
                name: "IX_Trees_NameMainId",
                table: "Trees",
                column: "NameMainId");

            migrationBuilder.CreateIndex(
                name: "IX_Trees_NameMainId_LanguageId",
                table: "Trees",
                columns: new[] { "NameMainId", "LanguageId" });

            migrationBuilder.CreateIndex(
                name: "IX_UserClaims_UserId",
                table: "UserClaims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserLogins_UserId",
                table: "UserLogins",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserRoles_RoleId",
                table: "UserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "Users",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "IX_Users_CreatedAt",
                table: "Users",
                column: "CreatedAt");

            migrationBuilder.CreateIndex(
                name: "IX_Users_Department",
                table: "Users",
                column: "Department");

            migrationBuilder.CreateIndex(
                name: "IX_Users_Email",
                table: "Users",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Users_ExternalProvider",
                table: "Users",
                column: "ExternalProvider");

            migrationBuilder.CreateIndex(
                name: "IX_Users_ExternalProvider_ExternalId",
                table: "Users",
                columns: new[] { "ExternalProvider", "ExternalId" },
                unique: true,
                filter: "\"ExternalProvider\" IS NOT NULL AND \"ExternalId\" IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Users_IsActive",
                table: "Users",
                column: "IsActive");

            migrationBuilder.CreateIndex(
                name: "IX_Users_IsActive_CreatedAt",
                table: "Users",
                columns: new[] { "IsActive", "CreatedAt" });

            migrationBuilder.CreateIndex(
                name: "IX_Users_IsExternalAccount",
                table: "Users",
                column: "IsExternalAccount");

            migrationBuilder.CreateIndex(
                name: "IX_Users_LastLogin",
                table: "Users",
                column: "LastLogin");

            migrationBuilder.CreateIndex(
                name: "IX_Users_TwoFactorEnabled",
                table: "Users",
                column: "TwoFactorEnabled");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "Users",
                column: "NormalizedUserName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserSessions_ExpiresAt",
                table: "UserSessions",
                column: "ExpiresAt");

            migrationBuilder.CreateIndex(
                name: "IX_UserSessions_IsRevoked",
                table: "UserSessions",
                column: "IsRevoked");

            migrationBuilder.CreateIndex(
                name: "IX_UserSessions_RefreshToken_IsRevoked",
                table: "UserSessions",
                columns: new[] { "RefreshToken", "IsRevoked" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserSessions_UserId",
                table: "UserSessions",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserSessions_UserId_IsRevoked_ExpiresAt",
                table: "UserSessions",
                columns: new[] { "UserId", "IsRevoked", "ExpiresAt" });

            migrationBuilder.CreateIndex(
                name: "IX_ZodiacHoroscopes_LanguageId",
                table: "ZodiacHoroscopes",
                column: "LanguageId");

            migrationBuilder.CreateIndex(
                name: "IX_ZodiacHoroscopes_NameMainId",
                table: "ZodiacHoroscopes",
                column: "NameMainId");

            migrationBuilder.CreateIndex(
                name: "IX_ZodiacHoroscopes_ZodiacId",
                table: "ZodiacHoroscopes",
                column: "ZodiacId");

            migrationBuilder.CreateIndex(
                name: "IX_ZodiacHoroscopes_ZodiacId_NameMainId_LanguageId",
                table: "ZodiacHoroscopes",
                columns: new[] { "ZodiacId", "NameMainId", "LanguageId" });

            migrationBuilder.CreateIndex(
                name: "IX_Zodiacs_LanguageId",
                table: "Zodiacs",
                column: "LanguageId");

            migrationBuilder.CreateIndex(
                name: "IX_Zodiacs_NameMainId",
                table: "Zodiacs",
                column: "NameMainId");

            migrationBuilder.CreateIndex(
                name: "IX_Zodiacs_NameMainId_LanguageId",
                table: "Zodiacs",
                columns: new[] { "NameMainId", "LanguageId" });

            migrationBuilder.CreateIndex(
                name: "IX_Zodiacs_ZodiacSign",
                table: "Zodiacs",
                column: "ZodiacSign");

            migrationBuilder.CreateIndex(
                name: "IX_ZodiacTalismans_LanguageId",
                table: "ZodiacTalismans",
                column: "LanguageId");

            migrationBuilder.CreateIndex(
                name: "IX_ZodiacTalismans_NameMainId",
                table: "ZodiacTalismans",
                column: "NameMainId");

            migrationBuilder.CreateIndex(
                name: "IX_ZodiacTalismans_ZodiacId",
                table: "ZodiacTalismans",
                column: "ZodiacId");

            migrationBuilder.CreateIndex(
                name: "IX_ZodiacTalismans_ZodiacId_NameMainId_LanguageId",
                table: "ZodiacTalismans",
                columns: new[] { "ZodiacId", "NameMainId", "LanguageId" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ActivityLogs");

            migrationBuilder.DropTable(
                name: "Anecdotes");

            migrationBuilder.DropTable(
                name: "Animals");

            migrationBuilder.DropTable(
                name: "Colors");

            migrationBuilder.DropTable(
                name: "Comments");

            migrationBuilder.DropTable(
                name: "Declensions");

            migrationBuilder.DropTable(
                name: "Facts");

            migrationBuilder.DropTable(
                name: "ForeignVariants");

            migrationBuilder.DropTable(
                name: "HoroscopesOfNames");

            migrationBuilder.DropTable(
                name: "Metals");

            migrationBuilder.DropTable(
                name: "NameDetails");

            migrationBuilder.DropTable(
                name: "NameUrlForParsings");

            migrationBuilder.DropTable(
                name: "Numbers");

            migrationBuilder.DropTable(
                name: "Patrons");

            migrationBuilder.DropTable(
                name: "Planets");

            migrationBuilder.DropTable(
                name: "Plants");

            migrationBuilder.DropTable(
                name: "Professions");

            migrationBuilder.DropTable(
                name: "RoleClaims");

            migrationBuilder.DropTable(
                name: "Samples");

            migrationBuilder.DropTable(
                name: "SeoData");

            migrationBuilder.DropTable(
                name: "Stones");

            migrationBuilder.DropTable(
                name: "Synonyms");

            migrationBuilder.DropTable(
                name: "Talents");

            migrationBuilder.DropTable(
                name: "Trees");

            migrationBuilder.DropTable(
                name: "UserClaims");

            migrationBuilder.DropTable(
                name: "UserLogins");

            migrationBuilder.DropTable(
                name: "UserRoles");

            migrationBuilder.DropTable(
                name: "UserSessions");

            migrationBuilder.DropTable(
                name: "UserTokens");

            migrationBuilder.DropTable(
                name: "ZodiacHoroscopes");

            migrationBuilder.DropTable(
                name: "ZodiacTalismans");

            migrationBuilder.DropTable(
                name: "Roles");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "Zodiacs");

            migrationBuilder.DropTable(
                name: "Languages");

            migrationBuilder.DropTable(
                name: "Names");
        }
    }
}
