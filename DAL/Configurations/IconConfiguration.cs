using DAL.Models.GeneralModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DAL.Configurations
{
    public class IconConfiguration : IEntityTypeConfiguration<Icon>
    {
        public void Configure(EntityTypeBuilder<Icon> builder)
        {
            // Уникальность имени иконки в рамках всей базы.
            // Это гарантирует корректную работу компонента av-icon.
            builder.HasIndex(i => i.Name)
                   .IsUnique();

            // При удалении категории иконки не удаляются (или наоборот - зависит от бизнес логики)
            // По умолчанию Restrict или Cascade. Оставим каскад т.к. иконка без категории не имеет смысла.
            builder.HasOne(i => i.Category)
                   .WithMany(c => c.Icons)
                   .HasForeignKey(i => i.CategoryId)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
