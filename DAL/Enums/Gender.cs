using System.ComponentModel.DataAnnotations;

namespace DAL.Enums
{
    /// <summary>
    /// Перечисление для указания пола, к которому относится имя
    /// </summary>
    public enum Gender
    {
        [Display(Name = "Мужское")] Male = 1,

        [Display(Name = "Женское")] Female = 2,

        [Display(Name = "Универсальное")] Unisex = 3
    }
}