namespace pr_srv_names.Pages.Icons.Models
{
    /// <summary>
    /// Модель запроса для переименования иконки
    /// </summary>
    public class RenameIconRequest
    {
        /// <summary>
        /// Текущее имя иконки (например, "av_stop")
        /// </summary>
        public string OldName { get; set; } = string.Empty;

        /// <summary>
        /// Новое имя иконки (например, "av_stop_new")
        /// </summary>
        public string NewName { get; set; } = string.Empty;
    }
}
