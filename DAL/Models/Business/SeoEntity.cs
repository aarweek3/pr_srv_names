using DAL.Constants;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Models.Business
{
    [Table("seo_entity")]
    public class SeoEntity
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        [MaxLength(SeoEntityConstants.EntityTypeMaxLength)]
        public string EntityType { get; set; } = string.Empty;

        [Required]
        public Guid EntityId { get; set; }

        public bool Index { get; set; } = true;
        public bool Follow { get; set; } = true;

        public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;
        public DateTimeOffset UpdatedAt { get; set; } = DateTimeOffset.UtcNow;

        public ICollection<SeoTranslation> Translations { get; set; } = new List<SeoTranslation>();
    }

}
