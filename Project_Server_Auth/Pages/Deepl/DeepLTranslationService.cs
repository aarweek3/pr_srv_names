using System.Text.Json;
using System.Text;
using pr_srv_names.Supports.Deepl;

namespace pr_srv_names.Deepl

{
    public class DeepLTranslationService : IDeepLTranslationService
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;
        private readonly ILogger<DeepLTranslationService> _logger;
        private const int MaxRetries = 5;
        private const int BaseDelayMs = 1000;

        public DeepLTranslationService(HttpClient httpClient, IConfiguration configuration, ILogger<DeepLTranslationService> logger)
        {
            _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<IDeepLTranslationResponse> TranslateAsync(string input, string? sourceLang = null, string targetLang = "EN")
        {
            if (string.IsNullOrWhiteSpace(input))
            {
                _logger.LogWarning("Входная строка пуста.");
                return new DeepLTranslationResponse
                {
                    Success = false,
                    Body = string.Empty,
                    MessageDeep = "Входная строка пуста.",
                    StatusCode = null
                };
            }

            if (string.IsNullOrWhiteSpace(targetLang))
            {
                _logger.LogWarning("Целевой язык не указан.");
                return new DeepLTranslationResponse
                {
                    Success = false,
                    Body = string.Empty,
                    MessageDeep = "Целевой язык не указан.",
                    StatusCode = null
                };
            }

            var apiKey = _configuration["DeepL:ApiKey"];
            if (string.IsNullOrWhiteSpace(apiKey))
            {
                _logger.LogError("API ключ DeepL не настроен.");
                return new DeepLTranslationResponse
                {
                    Success = false,
                    Body = string.Empty,
                    MessageDeep = "Ошибка: API ключ не настроен.",
                    StatusCode = null
                };
            }

            var requestBody = new
            {
                text = new[] { input },
                source_lang = sourceLang,
                target_lang = targetLang
            };

            int delayMs = BaseDelayMs;

            for (int attempt = 1; attempt <= MaxRetries; attempt++)
            {
                _logger.LogInformation("Попытка перевода №{Attempt} для текста: {Input}, с языка: {SourceLang}, на язык: {TargetLang}", attempt, input, sourceLang ?? "авто", targetLang);

                try
                {
                    using var request = new HttpRequestMessage(HttpMethod.Post, "https://api-free.deepl.com/v2/translate")
                    {
                        Content = new StringContent(JsonSerializer.Serialize(requestBody), Encoding.UTF8, "application/json")
                    };
                    request.Headers.Add("Authorization", $"DeepL-Auth-Key {apiKey}");

                    _logger.LogDebug("Тело запроса к DeepL API: {RequestBody}", JsonSerializer.Serialize(requestBody));

                    var response = await _httpClient.SendAsync(request);

                    _logger.LogInformation("Получен ответ от DeepL API с кодом: {StatusCode}", (int)response.StatusCode);

                    if (response.IsSuccessStatusCode)
                    {
                        var json = await response.Content.ReadAsStringAsync();
                        _logger.LogDebug("Полный ответ API: {Response}", json);
                        using var doc = JsonDocument.Parse(json);

                        var translated = doc.RootElement
                                           .GetProperty("translations")[0]
                                           .GetProperty("text")
                                           .GetString()?.Trim();

                        _logger.LogInformation("Перевод успешен: {Translated}", translated);

                        return new DeepLTranslationResponse
                        {
                            Success = true,
                            Body = translated ?? string.Empty,
                            MessageDeep = null,
                            StatusCode = (int)response.StatusCode
                        };
                    }
                    else if ((int)response.StatusCode == 429 || (int)response.StatusCode == 456)
                    {
                        _logger.LogWarning("Превышен лимит запросов DeepL API (HTTP {StatusCode}), попытка {Attempt}. Ожидание {Delay} мс", (int)response.StatusCode, attempt, delayMs);
                        await Task.Delay(delayMs);
                        delayMs *= 2;
                        continue;
                    }
                    else
                    {
                        var errorContent = await response.Content.ReadAsStringAsync();
                        _logger.LogError("Ошибка API DeepL: Код {StatusCode} - {Reason}. Ответ: {ErrorContent}",
                            (int)response.StatusCode, response.ReasonPhrase, errorContent);

                        return new DeepLTranslationResponse
                        {
                            Success = false,
                            Body = string.Empty,
                            MessageDeep = $"Ошибка API: {(int)response.StatusCode} - {response.ReasonPhrase}. {errorContent}",
                            StatusCode = (int)response.StatusCode
                        };
                    }
                }
                catch (HttpRequestException ex)
                {
                    _logger.LogWarning(ex, "Сетевая ошибка при попытке №{Attempt}.", attempt);

                    if (attempt == MaxRetries)
                    {
                        _logger.LogError("Превышено количество попыток из-за сетевой ошибки: {Message}", ex.Message);
                        return new DeepLTranslationResponse
                        {
                            Success = false,
                            Body = string.Empty,
                            MessageDeep = $"Ошибка сети: {ex.Message}",
                            StatusCode = null
                        };
                    }

                    await Task.Delay(delayMs);
                    delayMs *= 2;
                }
                catch (JsonException ex)
                {
                    _logger.LogError(ex, "Ошибка парсинга JSON ответа от DeepL API.");
                    return new DeepLTranslationResponse
                    {
                        Success = false,
                        Body = string.Empty,
                        MessageDeep = $"Ошибка парсинга: {ex.Message}",
                        StatusCode = null
                    };
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Неожиданная ошибка при переводе.");
                    return new DeepLTranslationResponse
                    {
                        Success = false,
                        Body = string.Empty,
                        MessageDeep = $"Ошибка: {ex.Message}",
                        StatusCode = null
                    };
                }
            }

            _logger.LogError("Превышено максимальное количество попыток перевода текста.");
            return new DeepLTranslationResponse
            {
                Success = false,
                Body = string.Empty,
                MessageDeep = "Ошибка: превышено количество попыток.",
                StatusCode = null
            };
        }

        public async Task<DeepLUsageResponse> GetUsageAsync()
        {
            var apiKey = _configuration["DeepL:ApiKey"];
            if (string.IsNullOrWhiteSpace(apiKey))
            {
                _logger.LogError("API ключ DeepL не настроен.");
                return new DeepLUsageResponse
                {
                    Success = false,
                    Message = "Ошибка: API ключ не настроен."
                };
            }

            int delayMs = BaseDelayMs;

            for (int attempt = 1; attempt <= MaxRetries; attempt++)
            {
                _logger.LogInformation("Попытка получения статистики использования №{Attempt}", attempt);

                try
                {
                    using var request = new HttpRequestMessage(HttpMethod.Get, "https://api-free.deepl.com/v2/usage");
                    request.Headers.Add("Authorization", $"DeepL-Auth-Key {apiKey}");

                    var response = await _httpClient.SendAsync(request);

                    _logger.LogInformation("Получен ответ от DeepL API с кодом: {StatusCode}", (int)response.StatusCode);

                    if (response.IsSuccessStatusCode)
                    {
                        var json = await response.Content.ReadAsStringAsync();
                        _logger.LogDebug("Полный ответ API: {Response}", json);
                        using var doc = JsonDocument.Parse(json);

                        var root = doc.RootElement;
                        var usageResponse = new DeepLUsageResponse
                        {
                            Success = true,
                            CharacterCount = root.GetProperty("character_count").GetInt64(),
                            CharacterLimit = root.GetProperty("character_limit").GetInt64(),
                            ApiKeyCharacterCount = root.TryGetProperty("api_key_character_count", out var apiKeyCount) ? apiKeyCount.GetInt64() : null,
                            ApiKeyCharacterLimit = root.TryGetProperty("api_key_character_limit", out var apiKeyLimit) ? apiKeyLimit.GetInt64() : null,
                            TextCharacterCount = root.TryGetProperty("text_character_count", out var textCount) ? textCount.GetInt64() : null,
                            DocumentCharacterCount = root.TryGetProperty("document_character_count", out var docCount) ? docCount.GetInt64() : null,
                            WriteCharacterCount = root.TryGetProperty("write_character_count", out var writeCount) ? writeCount.GetInt64() : null,
                            Message = null
                        };

                        _logger.LogInformation("Статистика использования получена: {CharacterCount}/{CharacterLimit} символов", usageResponse.CharacterCount, usageResponse.CharacterLimit);

                        return usageResponse;
                    }
                    else if ((int)response.StatusCode == 429)
                    {
                        _logger.LogWarning("Превышен лимит запросов DeepL API (HTTP 429), попытка {Attempt}. Ожидание {Delay} мс", attempt, delayMs);
                        await Task.Delay(delayMs);
                        delayMs *= 2;
                        continue;
                    }
                    else
                    {
                        var errorContent = await response.Content.ReadAsStringAsync();
                        _logger.LogError("Ошибка API DeepL: Код {StatusCode} - {Reason}. Ответ: {ErrorContent}",
                            (int)response.StatusCode, response.ReasonPhrase, errorContent);

                        return new DeepLUsageResponse
                        {
                            Success = false,
                            Message = $"Ошибка API: {(int)response.StatusCode} - {response.ReasonPhrase}. {errorContent}"
                        };
                    }
                }
                catch (HttpRequestException ex)
                {
                    _logger.LogWarning(ex, "Сетевая ошибка при попытке №{Attempt}.", attempt);

                    if (attempt == MaxRetries)
                    {
                        _logger.LogError("Превышено количество попыток из-за сетевой ошибки: {Message}", ex.Message);
                        return new DeepLUsageResponse
                        {
                            Success = false,
                            Message = $"Ошибка сети: {ex.Message}"
                        };
                    }

                    await Task.Delay(delayMs);
                    delayMs *= 2;
                }
                catch (JsonException ex)
                {
                    _logger.LogError(ex, "Ошибка парсинга JSON ответа от DeepL API.");
                    return new DeepLUsageResponse
                    {
                        Success = false,
                        Message = $"Ошибка парсинга: {ex.Message}"
                    };
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Неожиданная ошибка при получении статистики.");
                    return new DeepLUsageResponse
                    {
                        Success = false,
                        Message = $"Ошибка: {ex.Message}"
                    };
                }
            }

            _logger.LogError("Превышено максимальное количество попыток получения статистики.");
            return new DeepLUsageResponse
            {
                Success = false,
                Message = "Ошибка: превышено количество попыток."
            };
        }
    }

    public interface IDeepLTranslationResponse
    {
        bool Success { get; }
        string Body { get; }
        string? MessageDeep { get; }
        int? StatusCode { get; }
    }

    public class DeepLTranslationResponse : IDeepLTranslationResponse
    {
        public bool Success { get; set; }
        public string Body { get; set; } = string.Empty;
        public string? MessageDeep { get; set; }
        public int? StatusCode { get; set; }
    }

    public class DeepLUsageResponse
    {
        public bool Success { get; init; }
        public long CharacterCount { get; init; }
        public long CharacterLimit { get; init; }
        public long? ApiKeyCharacterCount { get; init; }
        public long? ApiKeyCharacterLimit { get; init; }
        public long? TextCharacterCount { get; init; }
        public long? DocumentCharacterCount { get; init; }
        public long? WriteCharacterCount { get; init; }
        public string? Message { get; init; }
    }
}