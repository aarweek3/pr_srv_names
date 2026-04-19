using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace DAL.Migrations
{
    /// <inheritdoc />
    public partial class _2100 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "aggregator_sources",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Slug = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    BaseUrl = table.Column<string>(type: "character varying(2048)", maxLength: 2048, nullable: true),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    SortOrder = table.Column<int>(type: "integer", nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false),
                    DeletedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_aggregator_sources", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "categories_of_aggregator",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CanonicalName = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    Slug = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    ParentId = table.Column<int>(type: "integer", nullable: true),
                    SortOrder = table.Column<int>(type: "integer", nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false),
                    DeletedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_categories_of_aggregator", x => x.Id);
                    table.ForeignKey(
                        name: "FK_categories_of_aggregator_categories_of_aggregator_ParentId",
                        column: x => x.ParentId,
                        principalTable: "categories_of_aggregator",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "category",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Slug = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: false),
                    ParentId = table.Column<Guid>(type: "uuid", nullable: true),
                    Path = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    Level = table.Column<short>(type: "smallint", nullable: false),
                    IsLeaf = table.Column<bool>(type: "boolean", nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    SortOrder = table.Column<int>(type: "integer", nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_category", x => x.Id);
                    table.ForeignKey(
                        name: "FK_category_category_ParentId",
                        column: x => x.ParentId,
                        principalTable: "category",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "developer",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Slug = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: false),
                    Name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    Website = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    LogoUrl = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    Description = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_developer", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "developer_of_aggregators",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CanonicalName = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    Slug = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Website = table.Column<string>(type: "text", nullable: true),
                    IconPath = table.Column<string>(type: "text", nullable: true),
                    CreatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false),
                    DeletedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_developer_of_aggregators", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "IconCategories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    FolderName = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    IsSystem = table.Column<bool>(type: "boolean", nullable: false),
                    MenuIcon = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IconCategories", x => x.Id);
                });

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
                name: "languages_of_aggregator",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Code = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                    ShortCode = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                    Title = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    NativeTitle = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Enabled = table.Column<bool>(type: "boolean", nullable: false),
                    IsDefault = table.Column<bool>(type: "boolean", nullable: false),
                    IsRtl = table.Column<bool>(type: "boolean", nullable: false),
                    SortOrder = table.Column<int>(type: "integer", nullable: false),
                    IsSystem = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_languages_of_aggregator", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "LanguagesApp",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Code = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false, comment: "Код языка по стандарту BCP-47 (например, ru-RU, en-US)"),
                    ShortCode = table.Column<string>(type: "character varying(5)", maxLength: 5, nullable: false, comment: "Краткий код языка для UI (например, RU, EN)"),
                    Title = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false, comment: "Название на английском для админки"),
                    NativeTitle = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false, comment: "Название на родном языке для UI"),
                    Enabled = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true, comment: "Доступность языка для выбора"),
                    Direction = table.Column<string>(type: "character varying(3)", maxLength: 3, nullable: false, defaultValue: "ltr", comment: "Направление письма (ltr/rtl)"),
                    SortOrder = table.Column<int>(type: "integer", nullable: false, defaultValue: 999, comment: "Порядок сортировки в UI"),
                    IsDefault = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false, comment: "Язык по умолчанию (может быть только один)"),
                    IsSystem = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false, comment: "Системный язык (нельзя удалить)"),
                    IconKey = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true, comment: "Ключ иконки/флага"),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "NOW()", comment: "Дата создания записи"),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "NOW()", comment: "Дата последнего обновления")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LanguagesApp", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "license_type",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Code = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Description = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    IsOpenSource = table.Column<bool>(type: "boolean", nullable: false),
                    IsCommercial = table.Column<bool>(type: "boolean", nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    SortOrder = table.Column<int>(type: "integer", nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_license_type", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "license_types_of_aggregator",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CanonicalName = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Slug = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    SortOrder = table.Column<int>(type: "integer", nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false),
                    DeletedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_license_types_of_aggregator", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "MediaFiles",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false, comment: "Уникальный идентификатор медиафайла")
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ImageId = table.Column<Guid>(type: "uuid", nullable: false, comment: "Уникальный идентификатор изображения"),
                    OriginalName = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false, comment: "Исходное имя файла"),
                    RelativePath = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    ContentType = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    FileSize = table.Column<long>(type: "bigint", nullable: false),
                    Width = table.Column<int>(type: "integer", nullable: false),
                    Height = table.Column<int>(type: "integer", nullable: false),
                    UserId = table.Column<string>(type: "text", nullable: false),
                    Purpose = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, comment: "Дата создания записи"),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MediaFiles", x => x.Id);
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
                name: "platform",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Code = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Family = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    UrlPictureMain = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    SortOrder = table.Column<int>(type: "integer", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_platform", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "platforms_of_aggregator",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CanonicalName = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Slug = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    IconPath = table.Column<string>(type: "text", nullable: true),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    SortOrder = table.Column<int>(type: "integer", nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false),
                    DeletedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_platforms_of_aggregator", x => x.Id);
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
                name: "SamplesMain",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false, comment: "Уникальный идентификатор записи")
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false, comment: "Техническое название для идентификации в админке"),
                    SystemCode = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true, comment: "Системный код для использования в логике приложения"),
                    UrlPictureMain = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false, comment: "Флаг активности записи"),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "NOW()", comment: "Дата создания записи"),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "NOW()", comment: "Дата последнего обновления")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SamplesMain", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SamplesMainSeo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false, comment: "Уникальный идентификатор записи")
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false, comment: "Техническое название для идентификации"),
                    SystemCode = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true, comment: "Системный код"),
                    UrlPictureMain = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true, comment: "Главное изображение (по умолчанию для всех языков)"),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false, comment: "Флаг активности записи"),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "NOW()", comment: "Дата создания записи"),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "NOW()", comment: "Дата последнего обновления")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SamplesMainSeo", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SeoData",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false, comment: "Уникальный идентификатор SEO-данных")
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
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
                    ArticleSection = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true, defaultValue: "", comment: "Раздел статьи"),
                    NoIndex = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false, comment: "Запрет индексации (robots noindex)"),
                    NoFollow = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false, comment: "Запрет следования по ссылкам (robots nofollow)"),
                    Priority = table.Column<int>(type: "integer", nullable: true, defaultValue: 5, comment: "Приоритет страницы для sitemap.xml (0-10)"),
                    Region = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true, comment: "Географический регион")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SeoData", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "tagBusiness",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Slug = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: false),
                    UsageCount = table.Column<int>(type: "integer", nullable: false),
                    IsFeatured = table.Column<bool>(type: "boolean", nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tagBusiness", x => x.Id);
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
                name: "article",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Slug = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: false),
                    CategoryId = table.Column<Guid>(type: "uuid", nullable: true),
                    AuthorId = table.Column<Guid>(type: "uuid", nullable: true),
                    ArticleType = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    CreatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    PublishedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    IsPublished = table.Column<bool>(type: "boolean", nullable: false),
                    IsFeatured = table.Column<bool>(type: "boolean", nullable: false),
                    ViewsCount = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_article", x => x.Id);
                    table.ForeignKey(
                        name: "FK_article_category_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "category",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "programs_of_aggregator",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CanonicalName = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    Slug = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    CategoryOfAggregatorId = table.Column<int>(type: "integer", nullable: false),
                    SubCategoryOfAggregatorId = table.Column<int>(type: "integer", nullable: true),
                    DeveloperOfAggregatorId = table.Column<int>(type: "integer", nullable: true),
                    IconPath = table.Column<string>(type: "text", nullable: true),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    TotalDownloads = table.Column<long>(type: "bigint", nullable: true),
                    AverageRating = table.Column<double>(type: "double precision", nullable: true),
                    RatingCount = table.Column<int>(type: "integer", nullable: true),
                    CreatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false),
                    DeletedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_programs_of_aggregator", x => x.Id);
                    table.ForeignKey(
                        name: "FK_programs_of_aggregator_categories_of_aggregator_CategoryOfA~",
                        column: x => x.CategoryOfAggregatorId,
                        principalTable: "categories_of_aggregator",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_programs_of_aggregator_categories_of_aggregator_SubCategory~",
                        column: x => x.SubCategoryOfAggregatorId,
                        principalTable: "categories_of_aggregator",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_programs_of_aggregator_developer_of_aggregators_DeveloperOf~",
                        column: x => x.DeveloperOfAggregatorId,
                        principalTable: "developer_of_aggregators",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Icons",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    SvgContent = table.Column<string>(type: "text", nullable: false),
                    Tags = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    CategoryId = table.Column<int>(type: "integer", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Icons", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Icons_IconCategories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "IconCategories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "category_of_aggregator_localizations",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CategoryOfAggregatorId = table.Column<int>(type: "integer", nullable: false),
                    LanguageOfAggregatorId = table.Column<int>(type: "integer", nullable: false),
                    Name = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    Description = table.Column<string>(type: "text", nullable: true),
                    MetaTitle = table.Column<string>(type: "text", nullable: true),
                    MetaDescription = table.Column<string>(type: "text", nullable: true),
                    CreatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_category_of_aggregator_localizations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_category_of_aggregator_localizations_categories_of_aggregat~",
                        column: x => x.CategoryOfAggregatorId,
                        principalTable: "categories_of_aggregator",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_category_of_aggregator_localizations_languages_of_aggregato~",
                        column: x => x.LanguageOfAggregatorId,
                        principalTable: "languages_of_aggregator",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "developer_of_aggregator_localizations",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    DeveloperOfAggregatorId = table.Column<int>(type: "integer", nullable: false),
                    LanguageOfAggregatorId = table.Column<int>(type: "integer", nullable: false),
                    Name = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    Description = table.Column<string>(type: "text", nullable: true),
                    MetaTitle = table.Column<string>(type: "text", nullable: true),
                    MetaDescription = table.Column<string>(type: "text", nullable: true),
                    CreatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_developer_of_aggregator_localizations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_developer_of_aggregator_localizations_developer_of_aggregat~",
                        column: x => x.DeveloperOfAggregatorId,
                        principalTable: "developer_of_aggregators",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_developer_of_aggregator_localizations_languages_of_aggregat~",
                        column: x => x.LanguageOfAggregatorId,
                        principalTable: "languages_of_aggregator",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "category_translation",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    CategoryId = table.Column<Guid>(type: "uuid", nullable: false),
                    LanguageId = table.Column<int>(type: "integer", nullable: false),
                    Name = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: false),
                    ShortDescription = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    Description = table.Column<string>(type: "character varying(2000)", maxLength: 2000, nullable: true),
                    CreatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_category_translation", x => x.Id);
                    table.ForeignKey(
                        name: "FK_category_translation_LanguagesApp_LanguageId",
                        column: x => x.LanguageId,
                        principalTable: "LanguagesApp",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_category_translation_category_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "category",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "software",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    developer_id = table.Column<Guid>(type: "uuid", nullable: false),
                    Slug = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: false),
                    LicenseTypeId = table.Column<Guid>(type: "uuid", nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    ImportSource = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    ImportId = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_software", x => x.Id);
                    table.ForeignKey(
                        name: "FK_software_developer_developer_id",
                        column: x => x.developer_id,
                        principalTable: "developer",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_software_license_type_LicenseTypeId",
                        column: x => x.LicenseTypeId,
                        principalTable: "license_type",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "license_type_of_aggregator_localizations",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    LicenseTypeOfAggregatorId = table.Column<int>(type: "integer", nullable: false),
                    LanguageOfAggregatorId = table.Column<int>(type: "integer", nullable: false),
                    Name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "text", nullable: true),
                    CreatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_license_type_of_aggregator_localizations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_license_type_of_aggregator_localizations_languages_of_aggre~",
                        column: x => x.LanguageOfAggregatorId,
                        principalTable: "languages_of_aggregator",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_license_type_of_aggregator_localizations_license_types_of_a~",
                        column: x => x.LicenseTypeOfAggregatorId,
                        principalTable: "license_types_of_aggregator",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
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
                name: "platform_of_aggregator_localizations",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    PlatformOfAggregatorId = table.Column<int>(type: "integer", nullable: false),
                    LanguageOfAggregatorId = table.Column<int>(type: "integer", nullable: false),
                    Name = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    Description = table.Column<string>(type: "text", nullable: true),
                    CreatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_platform_of_aggregator_localizations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_platform_of_aggregator_localizations_languages_of_aggregato~",
                        column: x => x.LanguageOfAggregatorId,
                        principalTable: "languages_of_aggregator",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_platform_of_aggregator_localizations_platforms_of_aggregato~",
                        column: x => x.PlatformOfAggregatorId,
                        principalTable: "platforms_of_aggregator",
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
                name: "SamplesMainDescriptions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false, comment: "Уникальный идентификатор перевода")
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    SampleMainId = table.Column<int>(type: "integer", nullable: false),
                    LanguageAppId = table.Column<int>(type: "integer", nullable: false),
                    Name = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false, comment: "Локализованное название"),
                    Description = table.Column<string>(type: "text", nullable: true, comment: "Локализованное описание"),
                    UrlPicture = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "NOW()", comment: "Дата создания записи"),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "NOW()", comment: "Дата последнего обновления")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SamplesMainDescriptions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SamplesMainDescriptions_LanguagesApp",
                        column: x => x.LanguageAppId,
                        principalTable: "LanguagesApp",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SamplesMainDescriptions_SamplesMain",
                        column: x => x.SampleMainId,
                        principalTable: "SamplesMain",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "platform_translation",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    PlatformId = table.Column<Guid>(type: "uuid", nullable: false),
                    LanguageId = table.Column<int>(type: "integer", nullable: false),
                    Name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    DescriptionFull = table.Column<string>(type: "text", nullable: true),
                    UrlPicture = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    SeoDataId = table.Column<int>(type: "integer", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_platform_translation", x => x.Id);
                    table.ForeignKey(
                        name: "FK_platform_translation_LanguagesApp_LanguageId",
                        column: x => x.LanguageId,
                        principalTable: "LanguagesApp",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_platform_translation_SeoData_SeoDataId",
                        column: x => x.SeoDataId,
                        principalTable: "SeoData",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_platform_translation_platform_PlatformId",
                        column: x => x.PlatformId,
                        principalTable: "platform",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SamplesMainDescriptionsSeo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false, comment: "Уникальный идентификатор перевода")
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    SampleMainSeoId = table.Column<int>(type: "integer", nullable: false),
                    LanguageAppId = table.Column<int>(type: "integer", nullable: false),
                    Name = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false, comment: "Локализованное название"),
                    Description = table.Column<string>(type: "text", nullable: true, comment: "Локализованное описание"),
                    HtmlContent = table.Column<string>(type: "text", nullable: true, comment: "HTML контент (статья/полное описание)"),
                    UrlPicture = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true, comment: "Локализованное изображение (если пусто, берется Main)"),
                    SeoDataId = table.Column<int>(type: "integer", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "NOW()", comment: "Дата создания записи"),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "NOW()", comment: "Дата последнего обновления")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SamplesMainDescriptionsSeo", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SamplesMainDescriptionsSeo_LanguagesApp",
                        column: x => x.LanguageAppId,
                        principalTable: "LanguagesApp",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SamplesMainDescriptionsSeo_SamplesMainSeo",
                        column: x => x.SampleMainSeoId,
                        principalTable: "SamplesMainSeo",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SamplesMainDescriptionsSeo_SeoData",
                        column: x => x.SeoDataId,
                        principalTable: "SeoData",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "tag_translation",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    TagId = table.Column<Guid>(type: "uuid", nullable: false),
                    LanguageId = table.Column<int>(type: "integer", nullable: false),
                    Name = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: false),
                    Description = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    CreatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tag_translation", x => x.Id);
                    table.ForeignKey(
                        name: "FK_tag_translation_LanguagesApp_LanguageId",
                        column: x => x.LanguageId,
                        principalTable: "LanguagesApp",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_tag_translation_tagBusiness_TagId",
                        column: x => x.TagId,
                        principalTable: "tagBusiness",
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
                name: "UserSettings",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    UserId = table.Column<string>(type: "text", nullable: false),
                    Theme = table.Column<int>(type: "integer", nullable: false),
                    Density = table.Column<int>(type: "integer", nullable: false),
                    PrimaryColor = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: true),
                    SidebarState = table.Column<int>(type: "integer", nullable: false),
                    NavigationBehavior = table.Column<int>(type: "integer", nullable: false),
                    TableDensity = table.Column<int>(type: "integer", nullable: false),
                    DefaultPageSize = table.Column<int>(type: "integer", nullable: false),
                    ShowAdvancedFilters = table.Column<bool>(type: "boolean", nullable: false),
                    Language = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                    TimeZone = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    AccessibilityLevel = table.Column<int>(type: "integer", nullable: false),
                    NotificationLevel = table.Column<int>(type: "integer", nullable: false),
                    NotificationChannels = table.Column<int>(type: "integer", nullable: false),
                    SessionTerminationMode = table.Column<int>(type: "integer", nullable: false),
                    LoginNotificationMode = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserSettings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserSettings_Users_UserId",
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
                name: "article_comment",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    ArticleId = table.Column<Guid>(type: "uuid", nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    ParentId = table.Column<Guid>(type: "uuid", nullable: true),
                    Content = table.Column<string>(type: "character varying(4000)", maxLength: 4000, nullable: false),
                    Rating = table.Column<short>(type: "smallint", nullable: false),
                    IsApproved = table.Column<bool>(type: "boolean", nullable: false),
                    ContainsSpam = table.Column<bool>(type: "boolean", nullable: false),
                    IsEdited = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_article_comment", x => x.Id);
                    table.ForeignKey(
                        name: "FK_article_comment_article_ArticleId",
                        column: x => x.ArticleId,
                        principalTable: "article",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_article_comment_article_comment_ParentId",
                        column: x => x.ParentId,
                        principalTable: "article_comment",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "article_tag",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    ArticleId = table.Column<Guid>(type: "uuid", nullable: false),
                    TagId = table.Column<Guid>(type: "uuid", nullable: false),
                    SortOrder = table.Column<short>(type: "smallint", nullable: false),
                    IsPrimary = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_article_tag", x => x.Id);
                    table.ForeignKey(
                        name: "FK_article_tag_article_ArticleId",
                        column: x => x.ArticleId,
                        principalTable: "article",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_article_tag_tagBusiness_TagId",
                        column: x => x.TagId,
                        principalTable: "tagBusiness",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "article_translation",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ArticleId = table.Column<Guid>(type: "uuid", nullable: false),
                    language_id = table.Column<int>(type: "integer", nullable: false),
                    Title = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: false),
                    Content = table.Column<string>(type: "character varying(50000)", maxLength: 50000, nullable: false),
                    ShortSummary = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    CoverImageUrl = table.Column<string>(type: "character varying(2048)", maxLength: 2048, nullable: true),
                    CreatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_article_translation", x => x.Id);
                    table.ForeignKey(
                        name: "FK_article_translation_LanguagesApp_language_id",
                        column: x => x.language_id,
                        principalTable: "LanguagesApp",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_article_translation_article_ArticleId",
                        column: x => x.ArticleId,
                        principalTable: "article",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "seo_entity",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    EntityType = table.Column<string>(type: "character varying(40)", maxLength: 40, nullable: false),
                    EntityId = table.Column<Guid>(type: "uuid", nullable: false),
                    Index = table.Column<bool>(type: "boolean", nullable: false),
                    Follow = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    ArticleId = table.Column<Guid>(type: "uuid", nullable: true),
                    CategoryId = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_seo_entity", x => x.Id);
                    table.ForeignKey(
                        name: "FK_seo_entity_article_ArticleId",
                        column: x => x.ArticleId,
                        principalTable: "article",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_seo_entity_category_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "category",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "program_market_data_of_aggregator",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ProgramOfAggregatorId = table.Column<int>(type: "integer", nullable: false),
                    LanguageOfAggregatorId = table.Column<int>(type: "integer", nullable: false),
                    AggregatorSourceId = table.Column<int>(type: "integer", nullable: false),
                    RatingValue = table.Column<double>(type: "double precision", nullable: true),
                    RatingMax = table.Column<double>(type: "double precision", nullable: true),
                    RatingNormalized = table.Column<double>(type: "double precision", nullable: true, computedColumnSql: "CASE WHEN \"RatingMax\" > 0 THEN \"RatingValue\" / \"RatingMax\" ELSE NULL END", stored: true),
                    DownloadCountExact = table.Column<long>(type: "bigint", nullable: true),
                    DownloadCountDisplay = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    Price = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    CreatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false),
                    DeletedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_program_market_data_of_aggregator", x => x.Id);
                    table.ForeignKey(
                        name: "FK_program_market_data_of_aggregator_aggregator_sources_Aggreg~",
                        column: x => x.AggregatorSourceId,
                        principalTable: "aggregator_sources",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_program_market_data_of_aggregator_languages_of_aggregator_L~",
                        column: x => x.LanguageOfAggregatorId,
                        principalTable: "languages_of_aggregator",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_program_market_data_of_aggregator_programs_of_aggregator_Pr~",
                        column: x => x.ProgramOfAggregatorId,
                        principalTable: "programs_of_aggregator",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "program_platforms_of_aggregator",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ProgramOfAggregatorId = table.Column<int>(type: "integer", nullable: false),
                    PlatformOfAggregatorId = table.Column<int>(type: "integer", nullable: false),
                    MinOsVersion = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    CreatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_program_platforms_of_aggregator", x => x.Id);
                    table.ForeignKey(
                        name: "FK_program_platforms_of_aggregator_platforms_of_aggregator_Pla~",
                        column: x => x.PlatformOfAggregatorId,
                        principalTable: "platforms_of_aggregator",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_program_platforms_of_aggregator_programs_of_aggregator_Prog~",
                        column: x => x.ProgramOfAggregatorId,
                        principalTable: "programs_of_aggregator",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "programs_of_aggregator_localizations",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ProgramOfAggregatorId = table.Column<int>(type: "integer", nullable: false),
                    LanguageOfAggregatorId = table.Column<int>(type: "integer", nullable: false),
                    LicenseTypeId = table.Column<int>(type: "integer", nullable: true),
                    LocalizedName = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    ShortDescription = table.Column<string>(type: "text", nullable: true),
                    FullDescription = table.Column<string>(type: "text", nullable: true),
                    Pros = table.Column<string>(type: "text", nullable: true),
                    Cons = table.Column<string>(type: "text", nullable: true),
                    MetaTitle = table.Column<string>(type: "text", nullable: true),
                    MetaDescription = table.Column<string>(type: "text", nullable: true),
                    CreatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_programs_of_aggregator_localizations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_programs_of_aggregator_localizations_languages_of_aggregato~",
                        column: x => x.LanguageOfAggregatorId,
                        principalTable: "languages_of_aggregator",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_programs_of_aggregator_localizations_license_types_of_aggre~",
                        column: x => x.LicenseTypeId,
                        principalTable: "license_types_of_aggregator",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_programs_of_aggregator_localizations_programs_of_aggregator~",
                        column: x => x.ProgramOfAggregatorId,
                        principalTable: "programs_of_aggregator",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "screenshots_of_aggregator",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ProgramOfAggregatorId = table.Column<int>(type: "integer", nullable: false),
                    FilePath = table.Column<string>(type: "character varying(2048)", maxLength: 2048, nullable: false),
                    SortOrder = table.Column<int>(type: "integer", nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false),
                    DeletedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_screenshots_of_aggregator", x => x.Id);
                    table.ForeignKey(
                        name: "FK_screenshots_of_aggregator_programs_of_aggregator_ProgramOfA~",
                        column: x => x.ProgramOfAggregatorId,
                        principalTable: "programs_of_aggregator",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "versions_of_aggregator",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ProgramOfAggregatorId = table.Column<int>(type: "integer", nullable: false),
                    VersionNumber = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    ReleasedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    IsLatest = table.Column<bool>(type: "boolean", nullable: false),
                    SortOrder = table.Column<int>(type: "integer", nullable: false),
                    ExternalChangelogUrl = table.Column<string>(type: "character varying(2048)", maxLength: 2048, nullable: true),
                    Status = table.Column<int>(type: "integer", nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false),
                    DeletedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_versions_of_aggregator", x => x.Id);
                    table.ForeignKey(
                        name: "FK_versions_of_aggregator_programs_of_aggregator_ProgramOfAggr~",
                        column: x => x.ProgramOfAggregatorId,
                        principalTable: "programs_of_aggregator",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "videos_of_aggregator",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ProgramOfAggregatorId = table.Column<int>(type: "integer", nullable: false),
                    VideoUrl = table.Column<string>(type: "character varying(2048)", maxLength: 2048, nullable: false),
                    ThumbnailPath = table.Column<string>(type: "text", nullable: true),
                    SortOrder = table.Column<int>(type: "integer", nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false),
                    DeletedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_videos_of_aggregator", x => x.Id);
                    table.ForeignKey(
                        name: "FK_videos_of_aggregator_programs_of_aggregator_ProgramOfAggreg~",
                        column: x => x.ProgramOfAggregatorId,
                        principalTable: "programs_of_aggregator",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "article_software",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    ArticleId = table.Column<Guid>(type: "uuid", nullable: false),
                    SoftwareId = table.Column<Guid>(type: "uuid", nullable: false),
                    IsPrimary = table.Column<bool>(type: "boolean", nullable: false),
                    SortOrder = table.Column<short>(type: "smallint", nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_article_software", x => x.Id);
                    table.ForeignKey(
                        name: "FK_article_software_article_ArticleId",
                        column: x => x.ArticleId,
                        principalTable: "article",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_article_software_software_SoftwareId",
                        column: x => x.SoftwareId,
                        principalTable: "software",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "review",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    SoftwareId = table.Column<Guid>(type: "uuid", nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    Score = table.Column<short>(type: "smallint", nullable: false),
                    Content = table.Column<string>(type: "character varying(5000)", maxLength: 5000, nullable: true),
                    IsRatingOnly = table.Column<bool>(type: "boolean", nullable: false),
                    IsApproved = table.Column<bool>(type: "boolean", nullable: false),
                    IsEdited = table.Column<bool>(type: "boolean", nullable: false),
                    HelpfulCount = table.Column<int>(type: "integer", nullable: false),
                    UnhelpfulCount = table.Column<int>(type: "integer", nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_review", x => x.Id);
                    table.ForeignKey(
                        name: "FK_review_software_SoftwareId",
                        column: x => x.SoftwareId,
                        principalTable: "software",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "software_alias",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    SoftwareId = table.Column<Guid>(type: "uuid", nullable: false),
                    AliasName = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    IsPrimarySearch = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_software_alias", x => x.Id);
                    table.ForeignKey(
                        name: "FK_software_alias_software_SoftwareId",
                        column: x => x.SoftwareId,
                        principalTable: "software",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "software_asset",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    SoftwareId = table.Column<Guid>(type: "uuid", nullable: false),
                    PlatformId = table.Column<Guid>(type: "uuid", nullable: true),
                    AssetType = table.Column<string>(type: "character varying(40)", maxLength: 40, nullable: false),
                    Url = table.Column<string>(type: "character varying(2048)", maxLength: 2048, nullable: false),
                    Title = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    Description = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    Width = table.Column<int>(type: "integer", nullable: true),
                    Height = table.Column<int>(type: "integer", nullable: true),
                    IsPrimary = table.Column<bool>(type: "boolean", nullable: false),
                    SortOrder = table.Column<short>(type: "smallint", nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_software_asset", x => x.Id);
                    table.ForeignKey(
                        name: "FK_software_asset_platform_PlatformId",
                        column: x => x.PlatformId,
                        principalTable: "platform",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_software_asset_software_SoftwareId",
                        column: x => x.SoftwareId,
                        principalTable: "software",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "software_category",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    SoftwareId = table.Column<Guid>(type: "uuid", nullable: false),
                    CategoryId = table.Column<Guid>(type: "uuid", nullable: false),
                    IsPrimary = table.Column<bool>(type: "boolean", nullable: false),
                    SortOrder = table.Column<short>(type: "smallint", nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_software_category", x => x.Id);
                    table.ForeignKey(
                        name: "FK_software_category_category_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "category",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_software_category_software_SoftwareId",
                        column: x => x.SoftwareId,
                        principalTable: "software",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "software_link",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    SoftwareId = table.Column<Guid>(type: "uuid", nullable: false),
                    Url = table.Column<string>(type: "character varying(2048)", maxLength: 2048, nullable: false),
                    LinkType = table.Column<string>(type: "character varying(40)", maxLength: 40, nullable: false),
                    Title = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    Description = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    SortOrder = table.Column<short>(type: "smallint", nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_software_link", x => x.Id);
                    table.ForeignKey(
                        name: "FK_software_link_software_SoftwareId",
                        column: x => x.SoftwareId,
                        principalTable: "software",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "software_news",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    SoftwareId = table.Column<Guid>(type: "uuid", nullable: false),
                    LanguageId = table.Column<int>(type: "integer", nullable: false),
                    Title = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: false),
                    ShortDescription = table.Column<string>(type: "character varying(800)", maxLength: 800, nullable: true),
                    Content = table.Column<string>(type: "character varying(8000)", maxLength: 8000, nullable: true),
                    ExternalUrl = table.Column<string>(type: "character varying(2048)", maxLength: 2048, nullable: true),
                    PublishedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    IsImportant = table.Column<bool>(type: "boolean", nullable: false),
                    IsPublished = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_software_news", x => x.Id);
                    table.ForeignKey(
                        name: "FK_software_news_LanguagesApp_LanguageId",
                        column: x => x.LanguageId,
                        principalTable: "LanguagesApp",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_software_news_software_SoftwareId",
                        column: x => x.SoftwareId,
                        principalTable: "software",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "software_statistics",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    SoftwareId = table.Column<Guid>(type: "uuid", nullable: false),
                    ViewsCount = table.Column<long>(type: "bigint", nullable: false),
                    DownloadCount = table.Column<long>(type: "bigint", nullable: false),
                    AverageRating = table.Column<double>(type: "double precision", nullable: false),
                    ReviewCount = table.Column<int>(type: "integer", nullable: false),
                    RatingOnlyCount = table.Column<int>(type: "integer", nullable: false),
                    BookmarkCount = table.Column<int>(type: "integer", nullable: false),
                    ShareCount = table.Column<int>(type: "integer", nullable: false),
                    LastCalculatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_software_statistics", x => x.Id);
                    table.ForeignKey(
                        name: "FK_software_statistics_software_SoftwareId",
                        column: x => x.SoftwareId,
                        principalTable: "software",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "software_tag",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    SoftwareId = table.Column<Guid>(type: "uuid", nullable: false),
                    TagId = table.Column<Guid>(type: "uuid", nullable: false),
                    SortOrder = table.Column<short>(type: "smallint", nullable: false),
                    IsMain = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_software_tag", x => x.Id);
                    table.ForeignKey(
                        name: "FK_software_tag_software_SoftwareId",
                        column: x => x.SoftwareId,
                        principalTable: "software",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_software_tag_tagBusiness_TagId",
                        column: x => x.TagId,
                        principalTable: "tagBusiness",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "software_translation",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    software_id = table.Column<Guid>(type: "uuid", nullable: false),
                    language_id = table.Column<int>(type: "integer", nullable: false),
                    Name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    ShortDescription = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    Description = table.Column<string>(type: "character varying(10000)", maxLength: 10000, nullable: true),
                    CreatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_software_translation", x => x.Id);
                    table.ForeignKey(
                        name: "FK_software_translation_LanguagesApp_language_id",
                        column: x => x.language_id,
                        principalTable: "LanguagesApp",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_software_translation_software_software_id",
                        column: x => x.software_id,
                        principalTable: "software",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "software_version",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    SoftwareId = table.Column<Guid>(type: "uuid", nullable: false),
                    VersionNumber = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    ReleaseDate = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    IsLatest = table.Column<bool>(type: "boolean", nullable: false),
                    IsStable = table.Column<bool>(type: "boolean", nullable: false),
                    IsPreRelease = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_software_version", x => x.Id);
                    table.ForeignKey(
                        name: "FK_software_version_software_SoftwareId",
                        column: x => x.SoftwareId,
                        principalTable: "software",
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

            migrationBuilder.CreateTable(
                name: "seo_translation",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    SeoEntityId = table.Column<Guid>(type: "uuid", nullable: false),
                    LanguageId = table.Column<int>(type: "integer", nullable: false),
                    Slug = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    MetaTitle = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    MetaDescription = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    Keywords = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    CanonicalUrl = table.Column<string>(type: "character varying(2048)", maxLength: 2048, nullable: true),
                    CreatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_seo_translation", x => x.Id);
                    table.ForeignKey(
                        name: "FK_seo_translation_LanguagesApp_LanguageId",
                        column: x => x.LanguageId,
                        principalTable: "LanguagesApp",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_seo_translation_seo_entity_SeoEntityId",
                        column: x => x.SeoEntityId,
                        principalTable: "seo_entity",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "screenshot_of_aggregator_localizations",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ScreenshotOfAggregatorId = table.Column<int>(type: "integer", nullable: false),
                    LanguageOfAggregatorId = table.Column<int>(type: "integer", nullable: false),
                    AltText = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    CreatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_screenshot_of_aggregator_localizations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_screenshot_of_aggregator_localizations_languages_of_aggrega~",
                        column: x => x.LanguageOfAggregatorId,
                        principalTable: "languages_of_aggregator",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_screenshot_of_aggregator_localizations_screenshots_of_aggre~",
                        column: x => x.ScreenshotOfAggregatorId,
                        principalTable: "screenshots_of_aggregator",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "download_links_of_aggregator",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    VersionOfAggregatorId = table.Column<int>(type: "integer", nullable: false),
                    Url = table.Column<string>(type: "character varying(2048)", maxLength: 2048, nullable: false),
                    Architecture = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    FileSize = table.Column<long>(type: "bigint", nullable: true),
                    FileHash = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: true),
                    Status = table.Column<int>(type: "integer", nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false),
                    DeletedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_download_links_of_aggregator", x => x.Id);
                    table.ForeignKey(
                        name: "FK_download_links_of_aggregator_versions_of_aggregator_Version~",
                        column: x => x.VersionOfAggregatorId,
                        principalTable: "versions_of_aggregator",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "version_of_aggregator_localizations",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    VersionOfAggregatorId = table.Column<int>(type: "integer", nullable: false),
                    LanguageOfAggregatorId = table.Column<int>(type: "integer", nullable: false),
                    WhatsNew = table.Column<string>(type: "text", nullable: true),
                    Description = table.Column<string>(type: "text", nullable: true),
                    CreatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_version_of_aggregator_localizations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_version_of_aggregator_localizations_languages_of_aggregator~",
                        column: x => x.LanguageOfAggregatorId,
                        principalTable: "languages_of_aggregator",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_version_of_aggregator_localizations_versions_of_aggregator_~",
                        column: x => x.VersionOfAggregatorId,
                        principalTable: "versions_of_aggregator",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "video_of_aggregator_localizations",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    VideoOfAggregatorId = table.Column<int>(type: "integer", nullable: false),
                    LanguageOfAggregatorId = table.Column<int>(type: "integer", nullable: false),
                    Title = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    Description = table.Column<string>(type: "text", nullable: true),
                    CreatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_video_of_aggregator_localizations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_video_of_aggregator_localizations_languages_of_aggregator_L~",
                        column: x => x.LanguageOfAggregatorId,
                        principalTable: "languages_of_aggregator",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_video_of_aggregator_localizations_videos_of_aggregator_Vide~",
                        column: x => x.VideoOfAggregatorId,
                        principalTable: "videos_of_aggregator",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "software_version_platform",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    SoftwareVersionId = table.Column<Guid>(type: "uuid", nullable: false),
                    PlatformId = table.Column<Guid>(type: "uuid", nullable: false),
                    Architecture = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    ReleaseDate = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    SupportStatus = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    CreatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_software_version_platform", x => x.Id);
                    table.ForeignKey(
                        name: "FK_software_version_platform_platform_PlatformId",
                        column: x => x.PlatformId,
                        principalTable: "platform",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_software_version_platform_software_version_SoftwareVersionId",
                        column: x => x.SoftwareVersionId,
                        principalTable: "software_version",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "update_feed",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    SoftwareVersionId = table.Column<Guid>(type: "uuid", nullable: false),
                    LanguageId = table.Column<int>(type: "integer", nullable: false),
                    Title = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    Changes = table.Column<string>(type: "character varying(4000)", maxLength: 4000, nullable: true),
                    PublishedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    IsMajor = table.Column<bool>(type: "boolean", nullable: false),
                    SortOrder = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_update_feed", x => x.Id);
                    table.ForeignKey(
                        name: "FK_update_feed_LanguagesApp_LanguageId",
                        column: x => x.LanguageId,
                        principalTable: "LanguagesApp",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_update_feed_software_version_SoftwareVersionId",
                        column: x => x.SoftwareVersionId,
                        principalTable: "software_version",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "download_link_of_aggregator_localizations",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    DownloadLinkOfAggregatorId = table.Column<int>(type: "integer", nullable: false),
                    LanguageOfAggregatorId = table.Column<int>(type: "integer", nullable: false),
                    Title = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_download_link_of_aggregator_localizations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_download_link_of_aggregator_localizations_download_links_of~",
                        column: x => x.DownloadLinkOfAggregatorId,
                        principalTable: "download_links_of_aggregator",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_download_link_of_aggregator_localizations_languages_of_aggr~",
                        column: x => x.LanguageOfAggregatorId,
                        principalTable: "languages_of_aggregator",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "download_logs_of_aggregator",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    VersionOfAggregatorId = table.Column<int>(type: "integer", nullable: false),
                    DownloadLinkOfAggregatorId = table.Column<int>(type: "integer", nullable: true),
                    IpAddress = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    UserAgent = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                    CreatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_download_logs_of_aggregator", x => x.Id);
                    table.ForeignKey(
                        name: "FK_download_logs_of_aggregator_download_links_of_aggregator_Do~",
                        column: x => x.DownloadLinkOfAggregatorId,
                        principalTable: "download_links_of_aggregator",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_download_logs_of_aggregator_versions_of_aggregator_VersionO~",
                        column: x => x.VersionOfAggregatorId,
                        principalTable: "versions_of_aggregator",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "software_version_platform_file",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    SoftwareVersionPlatformId = table.Column<Guid>(type: "uuid", nullable: false),
                    FileType = table.Column<string>(type: "character varying(40)", maxLength: 40, nullable: false),
                    FileFormat = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    Url = table.Column<string>(type: "character varying(2048)", maxLength: 2048, nullable: false),
                    MirrorId = table.Column<Guid>(type: "uuid", nullable: true),
                    Sha256 = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: true),
                    SizeBytes = table.Column<long>(type: "bigint", nullable: true),
                    IsPrimary = table.Column<bool>(type: "boolean", nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_software_version_platform_file", x => x.Id);
                    table.ForeignKey(
                        name: "FK_software_version_platform_file_software_version_platform_So~",
                        column: x => x.SoftwareVersionPlatformId,
                        principalTable: "software_version_platform",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
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
                name: "IX_aggregator_sources_CreatedAt",
                table: "aggregator_sources",
                column: "CreatedAt");

            migrationBuilder.CreateIndex(
                name: "IX_aggregator_sources_IsDeleted",
                table: "aggregator_sources",
                column: "IsDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_aggregator_sources_Slug",
                table: "aggregator_sources",
                column: "Slug",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_aggregator_sources_UpdatedAt",
                table: "aggregator_sources",
                column: "UpdatedAt");

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
                name: "IX_article_CategoryId",
                table: "article",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_article_comment_ArticleId",
                table: "article_comment",
                column: "ArticleId");

            migrationBuilder.CreateIndex(
                name: "IX_article_comment_ParentId",
                table: "article_comment",
                column: "ParentId");

            migrationBuilder.CreateIndex(
                name: "IX_article_software_ArticleId",
                table: "article_software",
                column: "ArticleId");

            migrationBuilder.CreateIndex(
                name: "IX_article_software_SoftwareId",
                table: "article_software",
                column: "SoftwareId");

            migrationBuilder.CreateIndex(
                name: "IX_article_tag_ArticleId",
                table: "article_tag",
                column: "ArticleId");

            migrationBuilder.CreateIndex(
                name: "IX_article_tag_TagId",
                table: "article_tag",
                column: "TagId");

            migrationBuilder.CreateIndex(
                name: "IX_article_translation_ArticleId",
                table: "article_translation",
                column: "ArticleId");

            migrationBuilder.CreateIndex(
                name: "IX_article_translation_language_id",
                table: "article_translation",
                column: "language_id");

            migrationBuilder.CreateIndex(
                name: "IX_categories_of_aggregator_CreatedAt",
                table: "categories_of_aggregator",
                column: "CreatedAt");

            migrationBuilder.CreateIndex(
                name: "IX_categories_of_aggregator_IsDeleted",
                table: "categories_of_aggregator",
                column: "IsDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_categories_of_aggregator_ParentId",
                table: "categories_of_aggregator",
                column: "ParentId");

            migrationBuilder.CreateIndex(
                name: "IX_categories_of_aggregator_Slug",
                table: "categories_of_aggregator",
                column: "Slug",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_categories_of_aggregator_UpdatedAt",
                table: "categories_of_aggregator",
                column: "UpdatedAt");

            migrationBuilder.CreateIndex(
                name: "IX_category_ParentId",
                table: "category",
                column: "ParentId");

            migrationBuilder.CreateIndex(
                name: "IX_category_of_aggregator_localizations_CategoryOfAggregatorId~",
                table: "category_of_aggregator_localizations",
                columns: new[] { "CategoryOfAggregatorId", "LanguageOfAggregatorId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_category_of_aggregator_localizations_CreatedAt",
                table: "category_of_aggregator_localizations",
                column: "CreatedAt");

            migrationBuilder.CreateIndex(
                name: "IX_category_of_aggregator_localizations_LanguageOfAggregatorId",
                table: "category_of_aggregator_localizations",
                column: "LanguageOfAggregatorId");

            migrationBuilder.CreateIndex(
                name: "IX_category_of_aggregator_localizations_UpdatedAt",
                table: "category_of_aggregator_localizations",
                column: "UpdatedAt");

            migrationBuilder.CreateIndex(
                name: "IX_category_translation_CategoryId",
                table: "category_translation",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_category_translation_LanguageId",
                table: "category_translation",
                column: "LanguageId");

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
                name: "IX_developer_of_aggregator_localizations_CreatedAt",
                table: "developer_of_aggregator_localizations",
                column: "CreatedAt");

            migrationBuilder.CreateIndex(
                name: "IX_developer_of_aggregator_localizations_DeveloperOfAggregator~",
                table: "developer_of_aggregator_localizations",
                columns: new[] { "DeveloperOfAggregatorId", "LanguageOfAggregatorId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_developer_of_aggregator_localizations_LanguageOfAggregatorId",
                table: "developer_of_aggregator_localizations",
                column: "LanguageOfAggregatorId");

            migrationBuilder.CreateIndex(
                name: "IX_developer_of_aggregator_localizations_UpdatedAt",
                table: "developer_of_aggregator_localizations",
                column: "UpdatedAt");

            migrationBuilder.CreateIndex(
                name: "IX_developer_of_aggregators_CreatedAt",
                table: "developer_of_aggregators",
                column: "CreatedAt");

            migrationBuilder.CreateIndex(
                name: "IX_developer_of_aggregators_IsDeleted",
                table: "developer_of_aggregators",
                column: "IsDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_developer_of_aggregators_Slug",
                table: "developer_of_aggregators",
                column: "Slug",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_developer_of_aggregators_UpdatedAt",
                table: "developer_of_aggregators",
                column: "UpdatedAt");

            migrationBuilder.CreateIndex(
                name: "IX_download_link_of_aggregator_localizations_CreatedAt",
                table: "download_link_of_aggregator_localizations",
                column: "CreatedAt");

            migrationBuilder.CreateIndex(
                name: "IX_download_link_of_aggregator_localizations_DownloadLinkOfAgg~",
                table: "download_link_of_aggregator_localizations",
                columns: new[] { "DownloadLinkOfAggregatorId", "LanguageOfAggregatorId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_download_link_of_aggregator_localizations_LanguageOfAggrega~",
                table: "download_link_of_aggregator_localizations",
                column: "LanguageOfAggregatorId");

            migrationBuilder.CreateIndex(
                name: "IX_download_link_of_aggregator_localizations_UpdatedAt",
                table: "download_link_of_aggregator_localizations",
                column: "UpdatedAt");

            migrationBuilder.CreateIndex(
                name: "IX_download_links_of_aggregator_CreatedAt",
                table: "download_links_of_aggregator",
                column: "CreatedAt");

            migrationBuilder.CreateIndex(
                name: "IX_download_links_of_aggregator_IsDeleted",
                table: "download_links_of_aggregator",
                column: "IsDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_download_links_of_aggregator_UpdatedAt",
                table: "download_links_of_aggregator",
                column: "UpdatedAt");

            migrationBuilder.CreateIndex(
                name: "IX_download_links_of_aggregator_VersionOfAggregatorId",
                table: "download_links_of_aggregator",
                column: "VersionOfAggregatorId");

            migrationBuilder.CreateIndex(
                name: "IX_download_logs_of_aggregator_CreatedAt",
                table: "download_logs_of_aggregator",
                column: "CreatedAt");

            migrationBuilder.CreateIndex(
                name: "IX_download_logs_of_aggregator_DownloadLinkOfAggregatorId",
                table: "download_logs_of_aggregator",
                column: "DownloadLinkOfAggregatorId");

            migrationBuilder.CreateIndex(
                name: "IX_download_logs_of_aggregator_VersionOfAggregatorId",
                table: "download_logs_of_aggregator",
                column: "VersionOfAggregatorId");

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
                name: "IX_Icons_CategoryId",
                table: "Icons",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Icons_Name",
                table: "Icons",
                column: "Name",
                unique: true);

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
                name: "IX_languages_of_aggregator_CreatedAt",
                table: "languages_of_aggregator",
                column: "CreatedAt");

            migrationBuilder.CreateIndex(
                name: "IX_languages_of_aggregator_IsDefault",
                table: "languages_of_aggregator",
                column: "IsDefault",
                unique: true,
                filter: "\"IsDefault\" = true");

            migrationBuilder.CreateIndex(
                name: "IX_languages_of_aggregator_UpdatedAt",
                table: "languages_of_aggregator",
                column: "UpdatedAt");

            migrationBuilder.CreateIndex(
                name: "IX_LanguagesApp_Code",
                table: "LanguagesApp",
                column: "Code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_LanguagesApp_Default_Enabled",
                table: "LanguagesApp",
                columns: new[] { "IsDefault", "Enabled" });

            migrationBuilder.CreateIndex(
                name: "IX_LanguagesApp_Default_Unique",
                table: "LanguagesApp",
                column: "IsDefault",
                unique: true,
                filter: "\"IsDefault\" = true");

            migrationBuilder.CreateIndex(
                name: "IX_LanguagesApp_ShortCode",
                table: "LanguagesApp",
                column: "ShortCode");

            migrationBuilder.CreateIndex(
                name: "IX_license_type_of_aggregator_localizations_CreatedAt",
                table: "license_type_of_aggregator_localizations",
                column: "CreatedAt");

            migrationBuilder.CreateIndex(
                name: "IX_license_type_of_aggregator_localizations_LanguageOfAggregat~",
                table: "license_type_of_aggregator_localizations",
                column: "LanguageOfAggregatorId");

            migrationBuilder.CreateIndex(
                name: "IX_license_type_of_aggregator_localizations_LicenseTypeOfAggre~",
                table: "license_type_of_aggregator_localizations",
                columns: new[] { "LicenseTypeOfAggregatorId", "LanguageOfAggregatorId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_license_type_of_aggregator_localizations_UpdatedAt",
                table: "license_type_of_aggregator_localizations",
                column: "UpdatedAt");

            migrationBuilder.CreateIndex(
                name: "IX_license_types_of_aggregator_CreatedAt",
                table: "license_types_of_aggregator",
                column: "CreatedAt");

            migrationBuilder.CreateIndex(
                name: "IX_license_types_of_aggregator_IsDeleted",
                table: "license_types_of_aggregator",
                column: "IsDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_license_types_of_aggregator_Slug",
                table: "license_types_of_aggregator",
                column: "Slug",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_license_types_of_aggregator_UpdatedAt",
                table: "license_types_of_aggregator",
                column: "UpdatedAt");

            migrationBuilder.CreateIndex(
                name: "IX_MediaFiles_CreatedAt",
                table: "MediaFiles",
                column: "CreatedAt");

            migrationBuilder.CreateIndex(
                name: "IX_MediaFiles_ImageId",
                table: "MediaFiles",
                column: "ImageId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_MediaFiles_OriginalName",
                table: "MediaFiles",
                column: "OriginalName");

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
                name: "IX_platform_of_aggregator_localizations_CreatedAt",
                table: "platform_of_aggregator_localizations",
                column: "CreatedAt");

            migrationBuilder.CreateIndex(
                name: "IX_platform_of_aggregator_localizations_LanguageOfAggregatorId",
                table: "platform_of_aggregator_localizations",
                column: "LanguageOfAggregatorId");

            migrationBuilder.CreateIndex(
                name: "IX_platform_of_aggregator_localizations_PlatformOfAggregatorId~",
                table: "platform_of_aggregator_localizations",
                columns: new[] { "PlatformOfAggregatorId", "LanguageOfAggregatorId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_platform_of_aggregator_localizations_UpdatedAt",
                table: "platform_of_aggregator_localizations",
                column: "UpdatedAt");

            migrationBuilder.CreateIndex(
                name: "IX_platform_translation_LanguageId",
                table: "platform_translation",
                column: "LanguageId");

            migrationBuilder.CreateIndex(
                name: "IX_platform_translation_PlatformId",
                table: "platform_translation",
                column: "PlatformId");

            migrationBuilder.CreateIndex(
                name: "IX_platform_translation_SeoDataId",
                table: "platform_translation",
                column: "SeoDataId");

            migrationBuilder.CreateIndex(
                name: "IX_platforms_of_aggregator_CreatedAt",
                table: "platforms_of_aggregator",
                column: "CreatedAt");

            migrationBuilder.CreateIndex(
                name: "IX_platforms_of_aggregator_IsDeleted",
                table: "platforms_of_aggregator",
                column: "IsDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_platforms_of_aggregator_Slug",
                table: "platforms_of_aggregator",
                column: "Slug",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_platforms_of_aggregator_UpdatedAt",
                table: "platforms_of_aggregator",
                column: "UpdatedAt");

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
                name: "IX_program_market_data_of_aggregator_AggregatorSourceId",
                table: "program_market_data_of_aggregator",
                column: "AggregatorSourceId");

            migrationBuilder.CreateIndex(
                name: "IX_program_market_data_of_aggregator_CreatedAt",
                table: "program_market_data_of_aggregator",
                column: "CreatedAt");

            migrationBuilder.CreateIndex(
                name: "IX_program_market_data_of_aggregator_IsDeleted",
                table: "program_market_data_of_aggregator",
                column: "IsDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_program_market_data_of_aggregator_LanguageOfAggregatorId",
                table: "program_market_data_of_aggregator",
                column: "LanguageOfAggregatorId");

            migrationBuilder.CreateIndex(
                name: "IX_program_market_data_of_aggregator_ProgramOfAggregatorId_Lan~",
                table: "program_market_data_of_aggregator",
                columns: new[] { "ProgramOfAggregatorId", "LanguageOfAggregatorId", "AggregatorSourceId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_program_market_data_of_aggregator_UpdatedAt",
                table: "program_market_data_of_aggregator",
                column: "UpdatedAt");

            migrationBuilder.CreateIndex(
                name: "IX_program_platforms_of_aggregator_CreatedAt",
                table: "program_platforms_of_aggregator",
                column: "CreatedAt");

            migrationBuilder.CreateIndex(
                name: "IX_program_platforms_of_aggregator_PlatformOfAggregatorId",
                table: "program_platforms_of_aggregator",
                column: "PlatformOfAggregatorId");

            migrationBuilder.CreateIndex(
                name: "IX_program_platforms_of_aggregator_ProgramOfAggregatorId_Platf~",
                table: "program_platforms_of_aggregator",
                columns: new[] { "ProgramOfAggregatorId", "PlatformOfAggregatorId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_program_platforms_of_aggregator_UpdatedAt",
                table: "program_platforms_of_aggregator",
                column: "UpdatedAt");

            migrationBuilder.CreateIndex(
                name: "IX_programs_of_aggregator_CategoryOfAggregatorId",
                table: "programs_of_aggregator",
                column: "CategoryOfAggregatorId");

            migrationBuilder.CreateIndex(
                name: "IX_programs_of_aggregator_CreatedAt",
                table: "programs_of_aggregator",
                column: "CreatedAt");

            migrationBuilder.CreateIndex(
                name: "IX_programs_of_aggregator_DeveloperOfAggregatorId",
                table: "programs_of_aggregator",
                column: "DeveloperOfAggregatorId");

            migrationBuilder.CreateIndex(
                name: "IX_programs_of_aggregator_IsDeleted",
                table: "programs_of_aggregator",
                column: "IsDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_programs_of_aggregator_Slug",
                table: "programs_of_aggregator",
                column: "Slug",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_programs_of_aggregator_SubCategoryOfAggregatorId",
                table: "programs_of_aggregator",
                column: "SubCategoryOfAggregatorId");

            migrationBuilder.CreateIndex(
                name: "IX_programs_of_aggregator_UpdatedAt",
                table: "programs_of_aggregator",
                column: "UpdatedAt");

            migrationBuilder.CreateIndex(
                name: "IX_programs_of_aggregator_localizations_CreatedAt",
                table: "programs_of_aggregator_localizations",
                column: "CreatedAt");

            migrationBuilder.CreateIndex(
                name: "IX_programs_of_aggregator_localizations_LanguageOfAggregatorId",
                table: "programs_of_aggregator_localizations",
                column: "LanguageOfAggregatorId");

            migrationBuilder.CreateIndex(
                name: "IX_programs_of_aggregator_localizations_LicenseTypeId",
                table: "programs_of_aggregator_localizations",
                column: "LicenseTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_programs_of_aggregator_localizations_ProgramOfAggregatorId_~",
                table: "programs_of_aggregator_localizations",
                columns: new[] { "ProgramOfAggregatorId", "LanguageOfAggregatorId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_programs_of_aggregator_localizations_UpdatedAt",
                table: "programs_of_aggregator_localizations",
                column: "UpdatedAt");

            migrationBuilder.CreateIndex(
                name: "IX_review_SoftwareId",
                table: "review",
                column: "SoftwareId");

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
                name: "IX_SamplesMainDescriptions_LanguageAppId",
                table: "SamplesMainDescriptions",
                column: "LanguageAppId");

            migrationBuilder.CreateIndex(
                name: "IX_SamplesMainDescriptions_Main_Language",
                table: "SamplesMainDescriptions",
                columns: new[] { "SampleMainId", "LanguageAppId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_SamplesMainDescriptionsSeo_LanguageAppId",
                table: "SamplesMainDescriptionsSeo",
                column: "LanguageAppId");

            migrationBuilder.CreateIndex(
                name: "IX_SamplesMainDescriptionsSeo_Main_Language",
                table: "SamplesMainDescriptionsSeo",
                columns: new[] { "SampleMainSeoId", "LanguageAppId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_SamplesMainDescriptionsSeo_SeoDataId",
                table: "SamplesMainDescriptionsSeo",
                column: "SeoDataId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_screenshot_of_aggregator_localizations_CreatedAt",
                table: "screenshot_of_aggregator_localizations",
                column: "CreatedAt");

            migrationBuilder.CreateIndex(
                name: "IX_screenshot_of_aggregator_localizations_LanguageOfAggregator~",
                table: "screenshot_of_aggregator_localizations",
                column: "LanguageOfAggregatorId");

            migrationBuilder.CreateIndex(
                name: "IX_screenshot_of_aggregator_localizations_ScreenshotOfAggregat~",
                table: "screenshot_of_aggregator_localizations",
                columns: new[] { "ScreenshotOfAggregatorId", "LanguageOfAggregatorId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_screenshot_of_aggregator_localizations_UpdatedAt",
                table: "screenshot_of_aggregator_localizations",
                column: "UpdatedAt");

            migrationBuilder.CreateIndex(
                name: "IX_screenshots_of_aggregator_CreatedAt",
                table: "screenshots_of_aggregator",
                column: "CreatedAt");

            migrationBuilder.CreateIndex(
                name: "IX_screenshots_of_aggregator_IsDeleted",
                table: "screenshots_of_aggregator",
                column: "IsDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_screenshots_of_aggregator_ProgramOfAggregatorId",
                table: "screenshots_of_aggregator",
                column: "ProgramOfAggregatorId");

            migrationBuilder.CreateIndex(
                name: "IX_screenshots_of_aggregator_UpdatedAt",
                table: "screenshots_of_aggregator",
                column: "UpdatedAt");

            migrationBuilder.CreateIndex(
                name: "IX_seo_entity_ArticleId",
                table: "seo_entity",
                column: "ArticleId");

            migrationBuilder.CreateIndex(
                name: "IX_seo_entity_CategoryId",
                table: "seo_entity",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_seo_translation_LanguageId",
                table: "seo_translation",
                column: "LanguageId");

            migrationBuilder.CreateIndex(
                name: "IX_seo_translation_SeoEntityId",
                table: "seo_translation",
                column: "SeoEntityId");

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
                name: "IX_software_developer_id",
                table: "software",
                column: "developer_id");

            migrationBuilder.CreateIndex(
                name: "IX_software_LicenseTypeId",
                table: "software",
                column: "LicenseTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_software_alias_SoftwareId",
                table: "software_alias",
                column: "SoftwareId");

            migrationBuilder.CreateIndex(
                name: "IX_software_asset_PlatformId",
                table: "software_asset",
                column: "PlatformId");

            migrationBuilder.CreateIndex(
                name: "IX_software_asset_SoftwareId",
                table: "software_asset",
                column: "SoftwareId");

            migrationBuilder.CreateIndex(
                name: "IX_software_category_CategoryId",
                table: "software_category",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_software_category_SoftwareId",
                table: "software_category",
                column: "SoftwareId");

            migrationBuilder.CreateIndex(
                name: "IX_software_link_SoftwareId",
                table: "software_link",
                column: "SoftwareId");

            migrationBuilder.CreateIndex(
                name: "IX_software_news_LanguageId",
                table: "software_news",
                column: "LanguageId");

            migrationBuilder.CreateIndex(
                name: "IX_software_news_SoftwareId",
                table: "software_news",
                column: "SoftwareId");

            migrationBuilder.CreateIndex(
                name: "IX_software_statistics_SoftwareId",
                table: "software_statistics",
                column: "SoftwareId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_software_tag_SoftwareId",
                table: "software_tag",
                column: "SoftwareId");

            migrationBuilder.CreateIndex(
                name: "IX_software_tag_TagId",
                table: "software_tag",
                column: "TagId");

            migrationBuilder.CreateIndex(
                name: "IX_software_translation_language_id",
                table: "software_translation",
                column: "language_id");

            migrationBuilder.CreateIndex(
                name: "IX_software_translation_software_id",
                table: "software_translation",
                column: "software_id");

            migrationBuilder.CreateIndex(
                name: "IX_software_version_SoftwareId",
                table: "software_version",
                column: "SoftwareId");

            migrationBuilder.CreateIndex(
                name: "IX_software_version_platform_PlatformId",
                table: "software_version_platform",
                column: "PlatformId");

            migrationBuilder.CreateIndex(
                name: "IX_software_version_platform_SoftwareVersionId",
                table: "software_version_platform",
                column: "SoftwareVersionId");

            migrationBuilder.CreateIndex(
                name: "IX_software_version_platform_file_SoftwareVersionPlatformId",
                table: "software_version_platform_file",
                column: "SoftwareVersionPlatformId");

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
                name: "IX_tag_translation_LanguageId",
                table: "tag_translation",
                column: "LanguageId");

            migrationBuilder.CreateIndex(
                name: "IX_tag_translation_TagId",
                table: "tag_translation",
                column: "TagId");

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
                name: "IX_update_feed_LanguageId",
                table: "update_feed",
                column: "LanguageId");

            migrationBuilder.CreateIndex(
                name: "IX_update_feed_SoftwareVersionId",
                table: "update_feed",
                column: "SoftwareVersionId");

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
                name: "IX_UserSettings_UserId",
                table: "UserSettings",
                column: "UserId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_version_of_aggregator_localizations_CreatedAt",
                table: "version_of_aggregator_localizations",
                column: "CreatedAt");

            migrationBuilder.CreateIndex(
                name: "IX_version_of_aggregator_localizations_LanguageOfAggregatorId",
                table: "version_of_aggregator_localizations",
                column: "LanguageOfAggregatorId");

            migrationBuilder.CreateIndex(
                name: "IX_version_of_aggregator_localizations_UpdatedAt",
                table: "version_of_aggregator_localizations",
                column: "UpdatedAt");

            migrationBuilder.CreateIndex(
                name: "IX_version_of_aggregator_localizations_VersionOfAggregatorId_L~",
                table: "version_of_aggregator_localizations",
                columns: new[] { "VersionOfAggregatorId", "LanguageOfAggregatorId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_versions_of_aggregator_CreatedAt",
                table: "versions_of_aggregator",
                column: "CreatedAt");

            migrationBuilder.CreateIndex(
                name: "IX_versions_of_aggregator_IsDeleted",
                table: "versions_of_aggregator",
                column: "IsDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_versions_of_aggregator_ProgramOfAggregatorId_IsLatest",
                table: "versions_of_aggregator",
                columns: new[] { "ProgramOfAggregatorId", "IsLatest" },
                unique: true,
                filter: "\"IsLatest\" = true");

            migrationBuilder.CreateIndex(
                name: "IX_versions_of_aggregator_ProgramOfAggregatorId_VersionNumber",
                table: "versions_of_aggregator",
                columns: new[] { "ProgramOfAggregatorId", "VersionNumber" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_versions_of_aggregator_UpdatedAt",
                table: "versions_of_aggregator",
                column: "UpdatedAt");

            migrationBuilder.CreateIndex(
                name: "IX_video_of_aggregator_localizations_CreatedAt",
                table: "video_of_aggregator_localizations",
                column: "CreatedAt");

            migrationBuilder.CreateIndex(
                name: "IX_video_of_aggregator_localizations_LanguageOfAggregatorId",
                table: "video_of_aggregator_localizations",
                column: "LanguageOfAggregatorId");

            migrationBuilder.CreateIndex(
                name: "IX_video_of_aggregator_localizations_UpdatedAt",
                table: "video_of_aggregator_localizations",
                column: "UpdatedAt");

            migrationBuilder.CreateIndex(
                name: "IX_video_of_aggregator_localizations_VideoOfAggregatorId_Langu~",
                table: "video_of_aggregator_localizations",
                columns: new[] { "VideoOfAggregatorId", "LanguageOfAggregatorId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_videos_of_aggregator_CreatedAt",
                table: "videos_of_aggregator",
                column: "CreatedAt");

            migrationBuilder.CreateIndex(
                name: "IX_videos_of_aggregator_IsDeleted",
                table: "videos_of_aggregator",
                column: "IsDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_videos_of_aggregator_ProgramOfAggregatorId",
                table: "videos_of_aggregator",
                column: "ProgramOfAggregatorId");

            migrationBuilder.CreateIndex(
                name: "IX_videos_of_aggregator_UpdatedAt",
                table: "videos_of_aggregator",
                column: "UpdatedAt");

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
                name: "article_comment");

            migrationBuilder.DropTable(
                name: "article_software");

            migrationBuilder.DropTable(
                name: "article_tag");

            migrationBuilder.DropTable(
                name: "article_translation");

            migrationBuilder.DropTable(
                name: "category_of_aggregator_localizations");

            migrationBuilder.DropTable(
                name: "category_translation");

            migrationBuilder.DropTable(
                name: "Colors");

            migrationBuilder.DropTable(
                name: "Comments");

            migrationBuilder.DropTable(
                name: "Declensions");

            migrationBuilder.DropTable(
                name: "developer_of_aggregator_localizations");

            migrationBuilder.DropTable(
                name: "download_link_of_aggregator_localizations");

            migrationBuilder.DropTable(
                name: "download_logs_of_aggregator");

            migrationBuilder.DropTable(
                name: "Facts");

            migrationBuilder.DropTable(
                name: "ForeignVariants");

            migrationBuilder.DropTable(
                name: "HoroscopesOfNames");

            migrationBuilder.DropTable(
                name: "Icons");

            migrationBuilder.DropTable(
                name: "license_type_of_aggregator_localizations");

            migrationBuilder.DropTable(
                name: "MediaFiles");

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
                name: "platform_of_aggregator_localizations");

            migrationBuilder.DropTable(
                name: "platform_translation");

            migrationBuilder.DropTable(
                name: "Professions");

            migrationBuilder.DropTable(
                name: "program_market_data_of_aggregator");

            migrationBuilder.DropTable(
                name: "program_platforms_of_aggregator");

            migrationBuilder.DropTable(
                name: "programs_of_aggregator_localizations");

            migrationBuilder.DropTable(
                name: "review");

            migrationBuilder.DropTable(
                name: "RoleClaims");

            migrationBuilder.DropTable(
                name: "Samples");

            migrationBuilder.DropTable(
                name: "SamplesMainDescriptions");

            migrationBuilder.DropTable(
                name: "SamplesMainDescriptionsSeo");

            migrationBuilder.DropTable(
                name: "screenshot_of_aggregator_localizations");

            migrationBuilder.DropTable(
                name: "seo_translation");

            migrationBuilder.DropTable(
                name: "software_alias");

            migrationBuilder.DropTable(
                name: "software_asset");

            migrationBuilder.DropTable(
                name: "software_category");

            migrationBuilder.DropTable(
                name: "software_link");

            migrationBuilder.DropTable(
                name: "software_news");

            migrationBuilder.DropTable(
                name: "software_statistics");

            migrationBuilder.DropTable(
                name: "software_tag");

            migrationBuilder.DropTable(
                name: "software_translation");

            migrationBuilder.DropTable(
                name: "software_version_platform_file");

            migrationBuilder.DropTable(
                name: "Stones");

            migrationBuilder.DropTable(
                name: "Synonyms");

            migrationBuilder.DropTable(
                name: "tag_translation");

            migrationBuilder.DropTable(
                name: "Talents");

            migrationBuilder.DropTable(
                name: "Trees");

            migrationBuilder.DropTable(
                name: "update_feed");

            migrationBuilder.DropTable(
                name: "UserClaims");

            migrationBuilder.DropTable(
                name: "UserLogins");

            migrationBuilder.DropTable(
                name: "UserRoles");

            migrationBuilder.DropTable(
                name: "UserSessions");

            migrationBuilder.DropTable(
                name: "UserSettings");

            migrationBuilder.DropTable(
                name: "UserTokens");

            migrationBuilder.DropTable(
                name: "version_of_aggregator_localizations");

            migrationBuilder.DropTable(
                name: "video_of_aggregator_localizations");

            migrationBuilder.DropTable(
                name: "ZodiacHoroscopes");

            migrationBuilder.DropTable(
                name: "ZodiacTalismans");

            migrationBuilder.DropTable(
                name: "download_links_of_aggregator");

            migrationBuilder.DropTable(
                name: "IconCategories");

            migrationBuilder.DropTable(
                name: "aggregator_sources");

            migrationBuilder.DropTable(
                name: "platforms_of_aggregator");

            migrationBuilder.DropTable(
                name: "license_types_of_aggregator");

            migrationBuilder.DropTable(
                name: "SamplesMain");

            migrationBuilder.DropTable(
                name: "SamplesMainSeo");

            migrationBuilder.DropTable(
                name: "SeoData");

            migrationBuilder.DropTable(
                name: "screenshots_of_aggregator");

            migrationBuilder.DropTable(
                name: "seo_entity");

            migrationBuilder.DropTable(
                name: "software_version_platform");

            migrationBuilder.DropTable(
                name: "tagBusiness");

            migrationBuilder.DropTable(
                name: "LanguagesApp");

            migrationBuilder.DropTable(
                name: "Roles");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "languages_of_aggregator");

            migrationBuilder.DropTable(
                name: "videos_of_aggregator");

            migrationBuilder.DropTable(
                name: "Zodiacs");

            migrationBuilder.DropTable(
                name: "versions_of_aggregator");

            migrationBuilder.DropTable(
                name: "article");

            migrationBuilder.DropTable(
                name: "platform");

            migrationBuilder.DropTable(
                name: "software_version");

            migrationBuilder.DropTable(
                name: "Languages");

            migrationBuilder.DropTable(
                name: "Names");

            migrationBuilder.DropTable(
                name: "programs_of_aggregator");

            migrationBuilder.DropTable(
                name: "category");

            migrationBuilder.DropTable(
                name: "software");

            migrationBuilder.DropTable(
                name: "categories_of_aggregator");

            migrationBuilder.DropTable(
                name: "developer_of_aggregators");

            migrationBuilder.DropTable(
                name: "developer");

            migrationBuilder.DropTable(
                name: "license_type");
        }
    }
}
