namespace HortiBts.Shared.Common
{
    public class Result<T>
    {
        public bool IsSuccess { get; set; }
        public T? Data { get; set; }
        public int Count { get; set; }
        public string? Error { get; set; }

        public Result() { }

        private Result(bool isSuccess, T? value, int count, string? error)
        {
            IsSuccess = isSuccess;
            Data = value;
            Count = count;
            Error = error;
        }

        public static Result<T> Success(T value)
        {
            var count = value is System.Collections.ICollection collection
                ? collection.Count
                : 0;

            return new Result<T>(true, value, count, null);
        }

        public static Result<T> Failure(string error)
            => new(false, default, 0, error);
    }

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