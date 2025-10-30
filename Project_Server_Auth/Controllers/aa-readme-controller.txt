// ********************************************
// Работа c изобраениями - Мой редактор
// ********************************************

// установить пакет NuGet - SixLabors.ImageSharp, пакет System.Drawing.Common - гораздо хуже
1. Создать контроллер EditorImageUploadController.cs в папке Controllers
"EditorImageUploadController.cs" - контроллер для работы c изображениями c моим редактором
2. Сервисы - Project_Server_Auth/Services/EditorImageService.cs
builder.Services.AddScoped<IEditorImageService, EditorImageService>();

// *******************************************
// AdvancedImageEditorController - загрузка изображений модальное окно c полным функционалом для моего Редактора  
// *******************************************
1. Создать контроллер AdvancedImageEditorController.cs в папке Controllers
"AdvancedImageEditorController.cs" - контроллер для работы c изображениями c моим редактором
2. Создать папку Pages/AdvancedImageEditor
2.1 Сервисы - Project_Server_Auth/Services/AdvancedImageEditorService.cs
2.2 builder.Services.AddScoped<IAdvancedImageEditorService, AdvancedImageEditorService>();
IAdvancedImageProcessingService - интерфейс для работы с изображениями
AdvancedImageProcessingService.cs - сервис 
2.3 ImageProcessingConfiguration.cs - конфигурация для ImageSharp
 
