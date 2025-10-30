using DAL.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DAL.Configurations
{
    public class NumberConfiguration : IEntityTypeConfiguration<Number>
    {
        public void Configure(EntityTypeBuilder<Number> entity)
        {
            entity.HasIndex(e => new { e.NameMainId, e.LanguageId }).HasDatabaseName("IX_Numbers_NameMainId_LanguageId");
        }
    }
}