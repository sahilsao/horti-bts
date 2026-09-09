namespace HortiBts.Shared.Common;

public class Result<T>
{
    public bool IsSuccess { get; set; }
    public int Count { get; set; }
    public T? Data { get; set; }
    public string? Error { get; set; }

    public static Result<T> Success(T data) => new()
    {
        IsSuccess = true,
        Data = data,
        Count = data is System.Collections.ICollection c ? c.Count : 0
    };

    public static Result<T> Failure(string error) => new()
    {
        IsSuccess = false,
        Error = error
    };
}

public class Result
{
    public bool IsSuccess { get; set; }
    public string? Error { get; set; }

    public static Result Success() => new()
    {
        IsSuccess = true
    };

    public static Result Failure(string error) => new()
    {
        IsSuccess = false,
        Error = error
    };
}