using DAL.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DAL.Configurations
{
    public class AnecdoteConfiguration : IEntityTypeConfiguration<Anecdote>
    {
        public void Configure(EntityTypeBuilder<Anecdote> entity)
        {
            entity.HasIndex(e => new { e.NameMainId, e.LanguageId })
                .HasDatabaseName("IX_Anecdotes_NameMainId_LanguageId");
        }
    }
}