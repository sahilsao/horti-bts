namespace HortiBts.Api.Helpers
{
    public class FinancialYearHelper
    {
        public static string GetCurrentFinancialYear(DateTime? today = null)
        {
            var date = today ?? DateTime.Today;
            var startYear = date.Month >= 4 ? date.Year : date.Year - 1;
            return Format(startYear);
        }

        public static string GetLastFinancialYear(DateTime? today = null)
            => GetCurrentFinancialYear(today) is var current
                ? Format(int.Parse(current[..4]) - 1)
                : throw new InvalidOperationException();

        private static string Format(int startYear) => $"{startYear}-{(startYear + 1) % 100:D2}";

    }
}
