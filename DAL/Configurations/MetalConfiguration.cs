using DAL.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DAL.Configurations
{
    public class MetalConfiguration : IEntityTypeConfiguration<Metal>
    {
        public void Configure(EntityTypeBuilder<Metal> entity)
        {
            entity.HasIndex(e => new { e.NameMainId, e.LanguageId }).HasDatabaseName("IX_Metals_NameMainId_LanguageId");
        }
    }
}