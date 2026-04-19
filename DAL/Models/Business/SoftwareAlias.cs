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
    [Table("software_alias")]
    public class SoftwareAlias
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        public Guid SoftwareId { get; set; }

        [ForeignKey(nameof(SoftwareId))]
        public Software Software { get; set; } = null!;

        [Required]
        [MaxLength(SoftwareAliasConstants.AliasNameMaxLength)]
        public string AliasName { get; set; } = string.Empty;

        public bool IsPrimarySearch { get; set; } = false;

        public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;
    }

}
