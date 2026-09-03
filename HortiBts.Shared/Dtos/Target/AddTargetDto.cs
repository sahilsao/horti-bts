namespace HortiBts.Shared.Dtos.Target
{
    public class AddTargetDto
    {
        public int? TargetId { get; set; }          // null/0 => insert, else update
        public int UserId { get; set; }
        public int UserType { get; set; }
        public string? UserTypeName { get; set; }
        public int? TargetCount { get; set; }
        public decimal? TargetArea { get; set; }
        public int FinancialYearId { get; set; }
    }
}
