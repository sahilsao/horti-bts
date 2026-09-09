// HortiBts.Client/Services/Common/ApiResultHelper.cs
using System.Text.Json;
using HortiBts.Shared.Common;

namespace HortiBts.Shared.Common
{
    public static class ApiResultHelper
    {
        public static async Task<Result<T>> ReadResultAsync<T>(
            HttpResponseMessage response,
            CancellationToken cancellationToken = default)
        {
            string raw;

            try
            {
                raw = await response.Content.ReadAsStringAsync(cancellationToken);
            }
            catch (OperationCanceledException)
            {
                throw;
            }
            catch (Exception ex)
            {
                return Result<T>.Failure($"Failed to read response body: {ex.Message}");
            }

            if (string.IsNullOrWhiteSpace(raw))
            {
                return response.IsSuccessStatusCode
                    ? Result<T>.Failure("Server returned an empty response.")
                    : Result<T>.Failure($"Request failed: {response.StatusCode}");
            }

            var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };

            if (!response.IsSuccessStatusCode)
            {
                // Some controllers use Problem(...) on failure (ProblemDetails shape),
                // others return Result<T> directly with a non-2xx status code.
                // Try ProblemDetails first so 'detail' isn't silently lost.
                try
                {
                    var problem = JsonSerializer.Deserialize<ProblemDetailsBody>(raw, options);
                    if (problem is not null && !string.IsNullOrWhiteSpace(problem.Detail))
                        return Result<T>.Failure(problem.Detail);
                }
                catch (JsonException) { /* fall through */ }

                try
                {
                    var parsedFailure = JsonSerializer.Deserialize<Result<T>>(raw, options);
                    if (parsedFailure is not null && !string.IsNullOrWhiteSpace(parsedFailure.Error))
                        return Result<T>.Failure(parsedFailure.Error);
                }
                catch (JsonException) { /* fall through */ }

                return Result<T>.Failure($"Request failed ({response.StatusCode}): {raw}");
            }

            Result<T>? parsed;

            try
            {
                parsed = JsonSerializer.Deserialize<Result<T>>(raw, options);
            }
            catch (JsonException)
            {
                return Result<T>.Failure($"Unexpected response format: {raw}");
            }

            return parsed ?? Result<T>.Failure("Server returned no data.");
        }

        // Non-generic overload for endpoints returning bare Result (not Result<T>)
        public static async Task<Result> ReadResultAsync(
            HttpResponseMessage response,
            CancellationToken cancellationToken = default)
        {
            string raw;

            try
            {
                raw = await response.Content.ReadAsStringAsync(cancellationToken);
            }
            catch (OperationCanceledException)
            {
                throw;
            }
            catch (Exception ex)
            {
                return Result.Failure($"Failed to read response body: {ex.Message}");
            }

            if (string.IsNullOrWhiteSpace(raw))
            {
                return response.IsSuccessStatusCode
                    ? Result.Failure("Server returned an empty response.")
                    : Result.Failure($"Request failed: {response.StatusCode}");
            }

            var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };

            if (!response.IsSuccessStatusCode)
            {
                try
                {
                    var problem = JsonSerializer.Deserialize<ProblemDetailsBody>(raw, options);
                    if (problem is not null && !string.IsNullOrWhiteSpace(problem.Detail))
                        return Result.Failure(problem.Detail);
                }
                catch (JsonException) { /* fall through */ }

                try
                {
                    var parsedFailure = JsonSerializer.Deserialize<Result>(raw, options);
                    if (parsedFailure is not null && !string.IsNullOrWhiteSpace(parsedFailure.Error))
                        return Result.Failure(parsedFailure.Error);
                }
                catch (JsonException) { /* fall through */ }

                return Result.Failure($"Request failed ({response.StatusCode}): {raw}");
            }

            Result? parsed;

            try
            {
                parsed = JsonSerializer.Deserialize<Result>(raw, options);
            }
            catch (JsonException)
            {
                return Result.Failure($"Unexpected response format: {raw}");
            }

            return parsed ?? Result.Failure("Server returned no data.");
        }

        private sealed class ProblemDetailsBody
        {
            public string? Title { get; set; }
            public string? Detail { get; set; }
            public int? Status { get; set; }
            public string? TraceId { get; set; }
        }
    }
}