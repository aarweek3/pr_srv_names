---
description: Порядок создания backend-компонента (модуля) по стандарту Aurora
---

# Регламент создания Backend модуля



Этот документ описывает последовательность шагов и стандарты кода для реализации нового функционала в проекте. Следуйте этому порядку, чтобы обеспечить чистоту архитектуры и совместимость с DI.

---

## 1. Слой DAL (Data Access Layer)

### 1.1. Модель данных (Entity)
- **Файл**: `DAL/Models/[EntityName].cs`
- **Пример**:
    ```csharp
    public class Product : BaseEntity 
    {
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
        public decimal Price { get; set; }
    }
    ```

### 1.2. Регистрация в AppDbContext
- **Файл**: `DAL/AppDbContext.cs`
- **Действия**:
    - Добавить `DbSet<[EntityName]>`.
    - Настроить в `OnModelCreating` (через конфигурацию или напрямую).
- **Пример**:
    ```csharp
    public DbSet<Product> Products { get; set; }

    // В OnModelCreating
    builder.Entity<Product>(entity => {
        entity.ToTable("Products");
        entity.HasIndex(e => e.Name).IsUnique();
        entity.Property(e => e.Price).HasPrecision(18, 2);
    });
    ```

### 1.2.1 Миграция БД -- НЕ ЗАБЫВАЕМ СДЕЛАТЬ МИГРАЦИЮ !!!!
- **Add-Migration**: `Add-Migration `
- **Update-Database**: `Update-Database`



### 1.3. Интерфейс репозитория
- **Файл**: `DAL/Repositories/Interfaces/I[EntityName]Repository.cs`
- **Пример**:
    ```csharp
    public interface IProductRepository : IRepository<Product>
    {
        Task<bool> IsNameUniqueAsync(string name, int? excludeId = null);
    }
    ```

### 1.4. Реализация репозитория
- **Файл**: `DAL/Repositories/[EntityName]Repository.cs`
- **ВАЖНО**: Конструктор должен принимать `AppDbContext context`.
- **Пример**:
    ```csharp
    public class ProductRepository : Repository<Product>, IProductRepository
    {
        public ProductRepository(AppDbContext context) : base(context) { }

        public async Task<bool> IsNameUniqueAsync(string name, int? excludeId = null)
        {
            var query = Entities.AsNoTracking().Where(x => x.Name == name);
            if (excludeId.HasValue) query = query.Where(x => x.Id != excludeId.Value);
            return !await query.AnyAsync();
        }
    }
    ```

### 1.5. Регистрация в UnitOfWork
- **Файл**: `DAL/UnitOfWork.cs`
- **Пример**:
    ```csharp
    private IProductRepository? _productRepository;
    public IProductRepository Products => _productRepository ??= new ProductRepository(_context);
    ```

---

## 2. Слой инфраструктуры (DTO, Mapping, Validation)

### 2.1. Создание DTO
- **Файл**: `Project_Server_Auth/Pages/[Feature]/Dtos/[Entity]Dto.cs`
- **Пример**:
    ```csharp
    public class ProductDto { public int Id { get; set; } ... }
    public class CreateProductDto { [Required] string Name { get; set; } ... }
    public class UpdateProductDto { public int Id { get; set; } string? Name { get; set; } ... } // Поля nullable!
    ```

### 2.2. Профиль AutoMapper (Verbose Style)
- **Файл**: `Project_Server_Auth/Pages/[Feature]/Services/[Entity]Profile.cs`
- **Требование**: Явное перечисление всех полей через `MapFrom`.
- **Пример**:
    ```csharp
    public class ProductProfile : Profile
    {
        public ProductProfile()
        {
            CreateMap<DAL.Models.Product, ProductDto>()
                .ForMember(d => d.Id, o => o.MapFrom(s => s.Id))
                .ForMember(d => d.Name, o => o.MapFrom(s => s.Name));

            CreateMap<CreateProductDto, DAL.Models.Product>()
                .ForMember(d => d.Id, o => o.Ignore())
                .ForMember(d => d.Name, o => o.MapFrom(s => s.Name));

            CreateMap<UpdateProductDto, DAL.Models.Product>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
        }
    }
    ```

### 2.3. Валидация (FluentValidation)
- **Файл**: `Project_Server_Auth/Pages/[Feature]/Services/[Entity]Validator.cs`
- **Пример**:
    ```csharp
    public class CreateProductDtoValidator : AbstractValidator<CreateProductDto>
    {
        public CreateProductDtoValidator() { RuleFor(x => x.Name).NotEmpty().MaximumLength(100); }
    }
    ```

---

## 3. Слой логики (Service и Controller)

### 3.1. Реализация сервиса
- **Файл**: `Project_Server_Auth/Pages/[Feature]/Services/[EntityName]Service.cs`
- **Пример**:
    ```csharp
    public class ProductService : IProductService
    {
        private readonly IUnitOfWork _uow;
        private readonly IMapper _mapper;
        public ProductService(IUnitOfWork uow, IMapper mapper) { _uow = uow; _mapper = mapper; }

        public async Task<ProductDto> CreateAsync(CreateProductDto dto)
        {
            var entity = _mapper.Map<DAL.Models.Product>(dto);
            await _uow.Products.AddAsync(entity);
            await _uow.SaveChangesAsync();
            return _mapper.Map<ProductDto>(entity);
        }
    }
    ```

### 3.2. Контроллер API
- **Файл**: `Project_Server_Auth/Controllers/[EntityName]sController.cs`
- **Пример**:
    ```csharp
    [ApiController]
    [Route("api/v1/products")]
    public class ProductsController : ControllerBase
    {
        private readonly IProductService _service;
        public ProductsController(IProductService service) { _service = service; }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateProductDto dto) => Ok(await _service.CreateAsync(dto));
    }
    ```

---

## 4. Регистрация зависимостей (DI)

- **Файл**: `Project_Server_Auth/Program.cs`
- **Действие**:
    ```csharp
    builder.Services.AddScoped<IProductRepository, ProductRepository>();
    builder.Services.AddScoped<IProductService, ProductService>();
    ```

---

// turbo-all