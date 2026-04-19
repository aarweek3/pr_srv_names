using System.ComponentModel.DataAnnotations;

namespace DAL.Models.SampleModels
{
    public class Sample
    {
        [Required]
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
    }
}
