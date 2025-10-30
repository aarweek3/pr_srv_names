using pr_srv_names.Deepl;

namespace pr_srv_names.Supports.Deepl
{
    public interface IDeepLTranslationService
    { /// <summary>
        /// Выполняет перевод текста с указанного языка на целевой язык через API DeepL.
        /// </summary>
        /// <param name="input">Текст для перевода.</param>
        /// <param name="sourceLang">Исходный язык (опционально, по умолчанию автоопределение).</param>
        /// <param name="targetLang">Целевой язык (по умолчанию "EN").</param>
        /// <returns>Ответ с результатом перевода или информацией об ошибке.</returns>
        Task<IDeepLTranslationResponse> TranslateAsync(string input, string? sourceLang = null, string targetLang = "EN");

        /// <summary>
        /// Получает статистику использования API DeepL.
        /// </summary>
        /// <returns>Ответ с данными о количестве символов и лимитах или информацией об ошибке.</returns>
        Task<DeepLUsageResponse> GetUsageAsync();
    }
}
