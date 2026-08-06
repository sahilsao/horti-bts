namespace HortiBts.Shared.Common
{
    public class Result<T>
    {
        public bool IsSuccess { get; set; }
        public T? Data { get; set; }
        public string? Error { get; set; }

        // Public parameterless constructor — required for System.Text.Json
        public Result() { }

        private Result(bool isSuccess, T? value, string? error)
        {
            IsSuccess = isSuccess;
            Data = value;
            Error = error;
        }

        public static Result<T> Success(T value) => new(true, value, null);
        public static Result<T> Failure(string error) => new(false, default, error);
    }

    // Non-generic for void operations
    public class Result
    {
        public bool IsSuccess { get; set; }
        public string? Error { get; set; }
        public Result() { }
        private Result(bool isSuccess, string? error)
        {
            IsSuccess = isSuccess;
            Error = error;
        }

        public static Result Success() => new(true, null);
        public static Result Failure(string error) => new(false, error);
    }
}
