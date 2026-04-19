namespace Project_Server_Auth.Services.Maintenance.Interfaces
{
    /// <summary>
    /// Интерфейс универсального загрузчика данных для обслуживания БД (Maintenance Standard v3.5).
    /// Позволяет импортировать данные из JSON-файлов в автоматическом режиме.
    /// </summary>
    public interface IMaintenanceSeeder
    {
        /// <summary>
        /// Универсальный метод для сидинга (заселения) данных из JSON-файла.
        /// </summary>
        /// <typeparam name="TDto">Тип DTO, соответствующий структуре данных в JSON</typeparam>
        /// <param name="relativeJsonPath">Путь к файлу относительно корня проекта (например, "Pages/Category/Jsons/Data.json")</param>
        /// <param name="seedAction">Делегат, выполняющий вставку данных через соответствующий сервис</param>
        /// <returns>Количество успешно импортированных записей</returns>
        Task<int> SeedAsync<TDto>(string relativeJsonPath, Func<TDto, Task> seedAction);
    }
}
