using DAL.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DAL.Configurations
{
    public class ColorConfiguration : IEntityTypeConfiguration<Color>
    {
        public void Configure(EntityTypeBuilder<Color> entity)
        {
            entity.HasIndex(e => new { e.NameMainId, e.LanguageId }).HasDatabaseName("IX_Colors_NameMainId_LanguageId");
        }
    }
}