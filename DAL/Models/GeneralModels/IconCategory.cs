using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using DAL.Models.Base;

namespace DAL.Models.GeneralModels
{
    /// <summary>
    /// Модель категории (раздела) иконок в БД
    /// </summary>
    public class IconCategory : BaseEntity
    {
        /// <summary>
        /// Отображаемое имя в интерфейсе (например, "Погода")
        /// </summary>
        [Required]
        [MaxLength(100)]
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Техническое имя папки (для обратной совместимости или путей)
        /// </summary>
        [Required]
        [MaxLength(100)]
        public string FolderName { get; set; } = string.Empty;

        /// <summary>
        /// Флаг системной коллекции (нельзя удалить)
        /// </summary>
        public bool IsSystem { get; set; } = false;

        /// <summary>
        /// Иконка для отображения в боковом меню
        /// </summary>
        [MaxLength(100)]
        public string MenuIcon { get; set; } = "av_folder";

        /// <summary>
        /// Список иконок в этой категории
        /// </summary>
        public ICollection<Icon> Icons { get; set; } = new List<Icon>();
    }
}
