using DAL.Models.NameModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DAL.Configurations
{
    public class TalentConfiguration : IEntityTypeConfiguration<Talent>
    {
        public void Configure(EntityTypeBuilder<Talent> entity)
        {
            entity.HasIndex(e => new { e.NameMainId, e.LanguageId }).HasDatabaseName("IX_Talents_NameMainId_LanguageId");
        }
    }
}