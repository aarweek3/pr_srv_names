using DAL.Constants;
using DAL.Models.NameModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DAL.Configurations
{
    public class CommentConfiguration : IEntityTypeConfiguration<Comment>
    {
        public void Configure(EntityTypeBuilder<Comment> entity)
        {
            entity.ToTable("Comments");
            entity.HasKey(e => e.Id);

            // Свойства из BaseEntity
            entity.Property(e => e.Id)
                .HasComment("Уникальный идентификатор комментария");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasComment("Дата создания записи");
            entity.Property(e => e.UpdatedAt)
                .HasComment("Дата последнего обновления");
            entity.Property(e => e.IsActive)
                .HasDefaultValue(true)
                .HasComment("Активна ли запись");

            // Свойства из LocalizedEntity
            entity.Property(e => e.Name)
                .IsRequired()
                .HasMaxLength(StringLengths.Name)
                .HasComment("Название комментария (может быть заголовком или кратким описанием)");
            entity.Property(e => e.Description)
                .HasMaxLength(StringLengths.Description)
                .HasComment("Текст комментария (основной контент)");
            entity.Property(e => e.NameMainId)
                .IsRequired()
                .HasComment("Внешний ключ на таблицу Names");
            entity.Property(e => e.LanguageId)
                .IsRequired()
                .HasComment("Внешний ключ на таблицу Languages");

            // Свойства из Comment
            entity.Property(e => e.AuthorEmail)
                .HasMaxLength(StringLengths.Email)
                .HasComment("Email автора комментария");
            entity.Property(e => e.Rating)
                .HasDefaultValue(0)
                .HasComment("Рейтинг комментария (количество лайков)");
            entity.Property(e => e.IsApproved)
                .HasDefaultValue(false)
                .HasComment("Одобрен ли комментарий модератором");
            entity.Property(e => e.ParentCommentId)
                .HasComment("Внешний ключ на родительский комментарий (для ответов)");

            // Индексы
            entity.HasIndex(e => e.NameMainId)
                .HasDatabaseName("IX_Comments_NameMainId");
            entity.HasIndex(e => e.LanguageId)
                .HasDatabaseName("IX_Comments_LanguageId");
            entity.HasIndex(e => e.CreatedAt)
                .HasDatabaseName("IX_Comments_CreatedAt");
            entity.HasIndex(e => new { e.NameMainId, e.LanguageId })
                .IsUnique()
                .HasDatabaseName("IX_Comments_NameMainId_LanguageId");
            entity.HasIndex(e => e.ParentCommentId)
                .HasDatabaseName("IX_Comments_ParentCommentId");
            entity.HasIndex(e => e.IsApproved)
                .HasDatabaseName("IX_Comments_IsApproved");
            entity.HasIndex(e => new { e.IsApproved, e.CreatedAt })
                .HasDatabaseName("IX_Comments_IsApproved_CreatedAt");
            entity.HasIndex(e => e.Rating)
                .HasDatabaseName("IX_Comments_Rating");

            // Связи
            entity.HasOne(c => c.NameMain)
                .WithMany(n => n.Comments)
                .HasForeignKey(c => c.NameMainId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(c => c.Language)
                .WithMany() // Без навигационного свойства на стороне Language
                .HasForeignKey(c => c.LanguageId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(c => c.ParentComment)
                .WithMany(c => c.Replies)
                .HasForeignKey(c => c.ParentCommentId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}