namespace DAL.DTOs;

/// <summary>
/// Базовый ответ API
/// Используется для единообразного формата ответов от всех API endpoints
/// </summary>
public class BaseResponse
{
    /// <summary>
    /// Флаг успешности выполнения операции
    /// </summary>
    public bool Success { get; set; }

    /// <summary>
    /// Сообщение о результате операции
    /// </summary>
    public string? Message { get; set; }

    /// <summary>
    /// Список ошибок валидации или выполнения
    /// </summary>
    public List<string> Errors { get; set; } = new();

    /// <summary>
    /// Данные ответа при успешном выполнении
    /// </summary>
    public object? Data { get; set; }

    /// <summary>
    /// Создаёт успешный ответ
    /// </summary>
    public static BaseResponse Ok(string? message = null, object? data = null)
    {
        return new BaseResponse
        {
            Success = true,
            Message = message,
            Data = data
        };
    }

    /// <summary>
    /// Создаёт ответ с ошибкой
    /// </summary>
    public static BaseResponse Fail(string message, List<string>? errors = null)
    {
        return new BaseResponse
        {
            Success = false,
            Message = message,
            Errors = errors ?? new List<string>()
        };
    }

    /// <summary>
    /// Создаёт ответ с единственной ошибкой
    /// </summary>
    public static BaseResponse Fail(string message, string error)
    {
        return new BaseResponse
        {
            Success = false,
            Message = message,
            Errors = new List<string> { error }
        };
    }
}

/// <summary>
/// Типизированный базовый ответ API
/// </summary>
public class BaseResponse<T> : BaseResponse
{
    /// <summary>
    /// Типизированные данные ответа
    /// </summary>
    public new T? Data { get; set; }

    /// <summary>
    /// Создаёт успешный типизированный ответ
    /// </summary>
    public static BaseResponse<T> Ok(T data, string? message = null)
    {
        return new BaseResponse<T>
        {
            Success = true,
            Message = message,
            Data = data
        };
    }
}