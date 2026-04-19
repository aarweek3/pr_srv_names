using DAL.Models.NameModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DAL.Configurations
{
    public class SynonymConfiguration : IEntityTypeConfiguration<Synonym>
    {
        public void Configure(EntityTypeBuilder<Synonym> entity)
        {
            entity.HasIndex(e => new { e.NameMainId, e.LanguageId }).HasDatabaseName("IX_Synonyms_NameMainId_LanguageId");
        }
    }
}