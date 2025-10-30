using DAL.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DAL.Configurations
{
    public class NameMainConfiguration : IEntityTypeConfiguration<NameMain>
    {
        public void Configure(EntityTypeBuilder<NameMain> entity)
        {
            entity.ToTable("Names");
            entity.HasKey(e => e.Id);

            entity.Property(e => e.Id)
                .HasComment("Уникальный идентификатор имени");

            entity.Property(e => e.Name)
                .IsRequired()
                .HasMaxLength(255)
                .HasComment("Само имя на английском языке (уникальное)");

            entity.Property(e => e.Description)
                .HasMaxLength(1024)
                .HasComment("Общее описание имени (не локализованное)");

            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasComment("Дата создания записи");

            entity.Property(e => e.UpdatedAt)
                .HasComment("Дата последнего обновления");

            entity.Property(e => e.IsActive)
                .HasDefaultValue(true)
                .HasComment("Активна ли запись");

            // ==========================================
            // ✅ ВИДАЛЕНО: SeoDataId - його немає в NameMain!
            // ==========================================

            // Индексы
            entity.HasIndex(e => e.Name)
                .IsUnique()
                .HasDatabaseName("IX_Names_Name");

            entity.HasIndex(e => e.IsActive)
                .HasDatabaseName("IX_Names_IsActive");

            entity.HasIndex(e => e.CreatedAt)
                .HasDatabaseName("IX_Names_CreatedAt");

            // ==========================================
            // ✅ ПРАВИЛЬНО: Связь 1:1 с SeoData
            // FK на стороне SeoData (NameMainId)
            // Конфигурируется ТОЛЬКО в SeoDataConfiguration!
            // ==========================================
            // НІЧОГО НЕ ПИШЕМО ТУТ - конфігурація в SeoDataConfiguration!

            // Связи с основными данными
            entity.HasMany(n => n.NameDetail)
                .WithOne(d => d.NameMain)
                .HasForeignKey(d => d.NameMainId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasMany(n => n.Anecdotes)
                .WithOne(a => a.NameMain)
                .HasForeignKey(a => a.NameMainId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasMany(n => n.Facts)
                .WithOne(f => f.NameMain)
                .HasForeignKey(f => f.NameMainId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasMany(n => n.Comments)
                .WithOne(c => c.NameMain)
                .HasForeignKey(c => c.NameMainId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasMany(n => n.Synonyms)
                .WithOne(s => s.NameMain)
                .HasForeignKey(s => s.NameMainId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasMany(n => n.ForeignVariants)
                .WithOne(fv => fv.NameMain)
                .HasForeignKey(fv => fv.NameMainId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasMany(n => n.Declensions)
                .WithOne(d => d.NameMain)
                .HasForeignKey(d => d.NameMainId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasMany(n => n.UrlsForParsing)
                .WithOne(u => u.NameMain)
                .HasForeignKey(u => u.NameMainId)
                .OnDelete(DeleteBehavior.Cascade);

            // Связи с символикой и астрологией
            entity.HasMany(n => n.Colors)
                .WithOne(c => c.NameMain)
                .HasForeignKey(c => c.NameMainId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasMany(n => n.Metals)
                .WithOne(m => m.NameMain)
                .HasForeignKey(m => m.NameMainId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasMany(n => n.Stones)
                .WithOne(s => s.NameMain)
                .HasForeignKey(s => s.NameMainId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasMany(n => n.Numbers)
                .WithOne(num => num.NameMain)
                .HasForeignKey(num => num.NameMainId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasMany(n => n.Planets)
                .WithOne(p => p.NameMain)
                .HasForeignKey(p => p.NameMainId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasMany(n => n.Plants)
                .WithOne(p => p.NameMain)
                .HasForeignKey(p => p.NameMainId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasMany(n => n.Trees)
                .WithOne(t => t.NameMain)
                .HasForeignKey(t => t.NameMainId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasMany(n => n.Animals)
                .WithOne(a => a.NameMain)
                .HasForeignKey(a => a.NameMainId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasMany(n => n.Zodiacs)
                .WithOne(z => z.NameMain)
                .HasForeignKey(z => z.NameMainId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasMany(n => n.HoroscopesOfName)
                .WithOne(h => h.NameMain)
                .HasForeignKey(h => h.NameMainId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasMany(n => n.ZodiacHoroscopes)
                .WithOne(zh => zh.NameMain)
                .HasForeignKey(zh => zh.NameMainId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasMany(n => n.ZodiacTalismans)
                .WithOne(zt => zt.NameMain)
                .HasForeignKey(zt => zt.NameMainId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasMany(n => n.Patrons)
                .WithOne(p => p.NameMain)
                .HasForeignKey(p => p.NameMainId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasMany(n => n.Talents)
                .WithOne(t => t.NameMain)
                .HasForeignKey(t => t.NameMainId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasMany(n => n.Professions)
                .WithOne(p => p.NameMain)
                .HasForeignKey(p => p.NameMainId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}