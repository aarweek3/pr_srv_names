using DAL.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DAL.Configurations
{
    public class ProfessionConfiguration : IEntityTypeConfiguration<Profession>
    {
        public void Configure(EntityTypeBuilder<Profession> entity)
        {
            entity.HasIndex(e => new { e.NameMainId, e.LanguageId }).HasDatabaseName("IX_Professions_NameMainId_LanguageId");
        }
    }
}