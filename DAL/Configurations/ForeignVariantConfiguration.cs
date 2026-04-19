using DAL.Models.NameModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DAL.Configurations
{
    public class ForeignVariantConfiguration : IEntityTypeConfiguration<ForeignVariant>
    {
        public void Configure(EntityTypeBuilder<ForeignVariant> entity)
        {
            entity.HasIndex(e => new { e.NameMainId, e.LanguageId }).HasDatabaseName("IX_ForeignVariants_NameMainId_LanguageId");
            entity.Property(e => e.Url).HasComment("URL ссылка на внешний ресурс");
        }
    }
}