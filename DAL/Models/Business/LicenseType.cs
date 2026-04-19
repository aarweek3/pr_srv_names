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
    [Table("license_type")]
    public class LicenseType
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        [MaxLength(LicenseTypeConstants.NameMaxLength)]
        public string Name { get; set; } = null!;

        [Required]
        [MaxLength(LicenseTypeConstants.CodeMaxLength)]
        public string Code { get; set; } = null!;

        [MaxLength(LicenseTypeConstants.DescriptionMaxLength)]
        public string? Description { get; set; }

        public bool IsOpenSource { get; set; } = false;
        public bool IsCommercial { get; set; } = false;
        public bool IsActive { get; set; } = true;

        public int SortOrder { get; set; } = 100;

        public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;

        public ICollection<Software> Softwares { get; set; } = new List<Software>();
    }

}
