using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DAL.Models;

/// <summary>
/// Модель для хранения знаков зодиака, связанных с именем
/// </summary>
[Table("Zodiacs")]
[Index(nameof(NameMainId))]
[Index(nameof(LanguageId))]
[Index(nameof(ZodiacSign))]
public class Zodiac : LocalizedEntity
{
    /// <summary>
    /// Номер знака зодиака (1-12)
    /// 1=Овен, 2=Телец, 3=Близнецы, 4=Рак, 5=Лев, 6=Дева,
    /// 7=Весы, 8=Скорпион, 9=Стрелец, 10=Козерог, 11=Водолей, 12=Рыбы
    /// </summary>
    [Range(1, 12, ErrorMessage = "Номер знака зодиака должен быть от 1 до 12")]
    public int ZodiacSign { get; set; }

    // ==========================================
    // ПЕРИОД ЗНАКА ЗОДИАКА (типобезопасное хранение)
    // ==========================================

    /// <summary>
    /// Месяц начала периода знака (1-12)
    /// </summary>
    [Range(1, 12, ErrorMessage = "Месяц должен быть от 1 до 12")]
    public byte StartMonth { get; set; }

    /// <summary>
    /// День начала периода знака (1-31)
    /// </summary>
    [Range(1, 31, ErrorMessage = "День должен быть от 1 до 31")]
    public byte StartDay { get; set; }

    /// <summary>
    /// Месяц окончания периода знака (1-12)
    /// </summary>
    [Range(1, 12, ErrorMessage = "Месяц должен быть от 1 до 12")]
    public byte EndMonth { get; set; }

    /// <summary>
    /// День окончания периода знака (1-31)
    /// </summary>
    [Range(1, 31, ErrorMessage = "День должен быть от 1 до 31")]
    public byte EndDay { get; set; }

    /// <summary>
    /// Астрологический символ знака (♈, ♉, ♊ и т.д.)
    /// </summary>
    [StringLength(10)]
    public string? Symbol { get; set; }

    // ==========================================
    // НАВИГАЦИОННЫЕ СВОЙСТВА
    // ==========================================

    /// <summary>
    /// Навигационное свойство - коллекция талисманов этого знака
    /// </summary>
    public virtual ICollection<ZodiacTalisman> Talismans { get; set; } = new List<ZodiacTalisman>();

    /// <summary>
    /// Навигационное свойство - коллекция гороскопов для этого знака
    /// </summary>
    public virtual ICollection<HoroscopeOfName> Horoscopes { get; set; } = new List<HoroscopeOfName>();

    // ==========================================
    // ВЫЧИСЛЯЕМЫЕ СВОЙСТВА
    // ==========================================

    /// <summary>
    /// Строковое представление периода знака для отображения
    /// Пример: "21.03 - 20.04"
    /// </summary>
    [NotMapped]
    public string DateRangeDisplay => $"{StartDay:D2}.{StartMonth:D2} - {EndDay:D2}.{EndMonth:D2}";

    /// <summary>
    /// Форматированное представление начальной даты
    /// Пример: "March 21"
    /// </summary>
    [NotMapped]
    public string StartDateFormatted => $"{GetMonthName(StartMonth)} {StartDay}";

    /// <summary>
    /// Форматированное представление конечной даты
    /// Пример: "April 20"
    /// </summary>
    [NotMapped]
    public string EndDateFormatted => $"{GetMonthName(EndMonth)} {EndDay}";

    /// <summary>
    /// Проверяет, попадает ли указанная дата в период этого знака зодиака
    /// </summary>
    /// <param name="month">Месяц (1-12)</param>
    /// <param name="day">День (1-31)</param>
    /// <returns>True, если дата попадает в период знака</returns>
    public bool IsDateInZodiacPeriod(int month, int day)
    {
        // Простая проверка для знаков в пределах одного года
        if (StartMonth < EndMonth)
        {
            return (month > StartMonth || (month == StartMonth && day >= StartDay)) &&
                   (month < EndMonth || (month == EndMonth && day <= EndDay));
        }
        // Для знаков на границе года (например, Козерог: 22.12 - 20.01)
        else if (StartMonth > EndMonth)
        {
            return (month > StartMonth || (month == StartMonth && day >= StartDay)) ||
                   (month < EndMonth || (month == EndMonth && day <= EndDay));
        }
        // Если знак в пределах одного месяца
        else
        {
            return month == StartMonth && day >= StartDay && day <= EndDay;
        }
    }

    /// <summary>
    /// Возвращает название месяца по номеру (на английском)
    /// </summary>
    private static string GetMonthName(int month) => month switch
    {
        1 => "January",
        2 => "February",
        3 => "March",
        4 => "April",
        5 => "May",
        6 => "June",
        7 => "July",
        8 => "August",
        9 => "September",
        10 => "October",
        11 => "November",
        12 => "December",
        _ => throw new ArgumentOutOfRangeException(nameof(month), "Month must be between 1 and 12")
    };
}