namespace DAL.Models.LocalizationModels
{
    /// <summary>
    /// Базовый класс для простых локализованных справочников
    /// Используется для небольших справочных таблиц (Color, Metal, Number и т.д.)
    /// Содержит только базовые поля: Name, Description, связи с NameMain и Language
    /// </summary>
    /// <remarks>
    /// Этот класс наследует все свойства от LocalizedEntity:
    /// - Id (из BaseEntity)
    /// - CreatedAt, UpdatedAt, IsActive (из BaseEntity)
    /// - Name, Description (из LocalizedEntity)
    /// - NameMainId + NameMain (из LocalizedEntity)
    /// - LanguageId + Language (из LocalizedEntity)
    /// 
    /// Используйте этот класс для моделей, которым не нужны дополнительные поля.
    /// Примеры: Color, Metal, Number, Patron, Planet, Plant, Profession, 
    ///          StoneTalisman, Synonym, Talent, TotemAnimal, Tree, Anecdote, Fact
    /// 
    /// Если модели нужны дополнительные поля - наследуйтесь напрямую от LocalizedEntity
    /// </remarks>
    public abstract class SimpleLocalizedEntity : LocalizedEntity
    {
        // Этот класс специально пуст - он просто маркер для моделей,
        // которым не нужны дополнительные поля.
        // Все необходимые поля уже унаследованы от LocalizedEntity и BaseEntity.

        // В будущем сюда можно добавить общие методы или поля,
        // которые понадобятся всем простым справочникам.
        // Например:
        // - Методы валидации
        // - Вычисляемые свойства
        // - Общие бизнес-правила
    }
}