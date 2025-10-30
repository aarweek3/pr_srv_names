NameMain.cs - главная модель c Id и Name на английском
Language.cs - модель языков
NameDetail.cs - модель общие свойства имени
ForeignVariants - модель Имя на других языках
Привязка

// Добавляем внешний ключ для связи с NameMain
[ForeignKey("NameMain")]
public int NameMainId  { get; set; } // Идентификатор записи в NameMain
public NameMain NameMain { get; set; } = null!;// Навигационное свойство для связи
    
// Внешний ключ на таблицу Languages Указывает, на каком языке написано это описание
[ForeignKey("Language")]
public int LanguageId { get; set; }
public Language Language { get; set; } = null!;

   