namespace DAL.Models.Aggregator.Enums
{
    /// <summary>
    /// Статус публикации программы в агрегаторе.
    /// </summary>
    public enum ProgramStatus
    {
        /// <summary>Черновик - не виден на сайте.</summary>
        Draft = 0,
        
        /// <summary>Опубликовано - доступно всем пользователям.</summary>
        Published = 1,
        
        /// <summary>Архив - старое ПО, скрыто из общего поиска.</summary>
        Archived = 2,
        
        /// <summary>Требует проверки - флаг для контент-менеджера.</summary>
        RequiresReview = 3
    }
}
