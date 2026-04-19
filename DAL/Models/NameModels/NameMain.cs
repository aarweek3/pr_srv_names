using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DAL.Constants;
using DAL.Models.Base;
using Microsoft.EntityFrameworkCore;

namespace DAL.Models.NameModels;

[Table("Names")]
[Index(nameof(Name), IsUnique = true)]
public class NameMain : BaseEntity
{
    [Required(ErrorMessage = "Имя обязательно для заполнения")]
    [StringLength(StringLengths.LongName, ErrorMessage = "Имя не может превышать 255 символов")]
    [Column("Name")]
    public string Name { get; set; } = string.Empty;

    [StringLength(StringLengths.Description, ErrorMessage = "Описание не может превышать 1024 символа")]
    [Column("Description")]
    public string? Description { get; set; }


    // ==========================================
    // Навигационные свойства - Основные данные
    // ==========================================

    public virtual ICollection<NameDetail> NameDetail { get; set; } = new List<NameDetail>();
    public virtual ICollection<Anecdote> Anecdotes { get; set; } = new List<Anecdote>();
    public virtual ICollection<Fact> Facts { get; set; } = new List<Fact>();
    public virtual ICollection<Comment> Comments { get; set; } = new List<Comment>();
    public virtual ICollection<Synonym> Synonyms { get; set; } = new List<Synonym>();
    public virtual ICollection<ForeignVariant> ForeignVariants { get; set; } = new List<ForeignVariant>();
    public virtual ICollection<Declension> Declensions { get; set; } = new List<Declension>();
    public virtual ICollection<NameUrlForParsing> UrlsForParsing { get; set; } = new List<NameUrlForParsing>();

    // ==========================================
    // Навигационные свойства - Символика и астрология
    // ==========================================

    public virtual ICollection<Color> Colors { get; set; } = new List<Color>();
    public virtual ICollection<Metal> Metals { get; set; } = new List<Metal>();
    public virtual ICollection<Stone> Stones { get; set; } = new List<Stone>();
    public virtual ICollection<Number> Numbers { get; set; } = new List<Number>();
    public virtual ICollection<Planet> Planets { get; set; } = new List<Planet>();
    public virtual ICollection<Plant> Plants { get; set; } = new List<Plant>();
    public virtual ICollection<Tree> Trees { get; set; } = new List<Tree>();
    public virtual ICollection<Animal> Animals { get; set; } = new List<Animal>();
    public virtual ICollection<Zodiac> Zodiacs { get; set; } = new List<Zodiac>();
    public virtual ICollection<HoroscopeOfName> HoroscopesOfName { get; set; } = new List<HoroscopeOfName>();
    public virtual ICollection<ZodiacHoroscope> ZodiacHoroscopes { get; set; } = new List<ZodiacHoroscope>();
    public virtual ICollection<ZodiacTalisman> ZodiacTalismans { get; set; } = new List<ZodiacTalisman>();

    // ==========================================
    // Навигационные свойства - Личностные характеристики
    // ==========================================

    public virtual ICollection<Patron> Patrons { get; set; } = new List<Patron>();
    public virtual ICollection<Talent> Talents { get; set; } = new List<Talent>();
    public virtual ICollection<Profession> Professions { get; set; } = new List<Profession>();
}