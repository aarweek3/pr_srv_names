using DAL.Models.NameModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DAL.Configurations
{
    public class PlantConfiguration : IEntityTypeConfiguration<Plant>
    {
        public void Configure(EntityTypeBuilder<Plant> entity)
        {
            entity.HasIndex(e => new { e.NameMainId, e.LanguageId })
                .IsUnique()
                .HasDatabaseName("IX_Plants_NameMainId_LanguageId");
        }
    }
}