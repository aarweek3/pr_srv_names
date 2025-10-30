using DAL.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DAL.Configurations
{
    public class StoneConfiguration : IEntityTypeConfiguration<Stone>
    {
        public void Configure(EntityTypeBuilder<Stone> entity)
        {
            entity.HasIndex(e => new { e.NameMainId, e.LanguageId }).HasDatabaseName("IX_Stones_NameMainId_LanguageId");
        }
    }
}