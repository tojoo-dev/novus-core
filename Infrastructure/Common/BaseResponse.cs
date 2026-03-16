using System.ComponentModel;

namespace Novus.Infrastructure.Common;

/// <summary>
/// A standardized response wrapper for all API endpoints.
/// </summary>
/// <typeparam name="T">The type of the data returned in the response.</typeparam>
public class BaseResponse<T>
{
    /// <summary>
    /// Indicates whether the request was successful.
    /// </summary>
    [Description("Indicates whether the request was successful")]
    public bool Success { get; init; }

    /// <summary>
    /// A descriptive message about the response.
    /// </summary>
    [Description("A descriptive message about the response")]
    public string? Message { get; init; }

    /// <summary>
    /// The actual data returned by the API.
    /// </summary>
    [Description("The actual data returned by the API")]
    public T? Data { get; init; }

    /// <summary>
    /// A list of error messages if the request failed.
    /// </summary>
    [Description("A list of error messages if the request failed")]
    public List<string>? Errors { get; init; }

    public static BaseResponse<T> Successful(T data, string? message = null)
        => new() { Success = true, Data = data, Message = message };

    public static BaseResponse<T> Failure(List<string> errors, string? message = null)
        => new() { Success = false, Errors = errors, Message = message };

    public static BaseResponse<T> Failure(string error, string? message = null)
        => new() { Success = false, Errors = new List<string> { error }, Message = message };
}

/// <summary>
/// A standardized response wrapper for API endpoints that don't return a specific data type.
/// </summary>
public class BaseResponse : BaseResponse<object>
{
    public static BaseResponse Successful(string? message = null)
        => new() { Success = true, Message = message };

    public new static BaseResponse Failure(List<string> errors, string? message = null)
        => new() { Success = false, Errors = errors, Message = message };

    public new static BaseResponse Failure(string error, string? message = null)
        => new() { Success = false, Errors = new List<string> { error }, Message = message };
}
