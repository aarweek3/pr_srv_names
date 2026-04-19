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
    [Table("developer")]
    public class Developer
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        [MaxLength(DeveloperConstants.SlugMaxLength)]
        public string Slug { get; set; } = string.Empty;

        [Required]
        [MaxLength(DeveloperConstants.NameMaxLength)]
        public string Name { get; set; } = string.Empty;

        [MaxLength(DeveloperConstants.WebsiteMaxLength)]
        public string? Website { get; set; }

        [MaxLength(DeveloperConstants.LogoUrlMaxLength)]
        public string? LogoUrl { get; set; }

        [MaxLength(DeveloperConstants.DescriptionMaxLength)]
        public string? Description { get; set; }

        public bool IsActive { get; set; } = true;

        public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;
        public DateTimeOffset UpdatedAt { get; set; } = DateTimeOffset.UtcNow;

        public ICollection<Software> Softwares { get; set; } = new List<Software>();
    }

}
