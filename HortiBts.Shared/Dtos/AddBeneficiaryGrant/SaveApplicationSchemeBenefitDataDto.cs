using System;

namespace HortiBts.Shared.Dtos.AddBeneficiaryGrant;

public class SaveApplicationSchemeBenefitDataDto
{
    public int ApplicationId { get; set; }
    public int FinancialYear { get; set; }
    public int? SdId { get; set; }
    public int? FdId { get; set; }
    public int? HfId { get; set; }
    public int? UfId { get; set; }
    public decimal Area { get; set; }          // -> approved_area
    public int ItemNo { get; set; }            // -> quantity
    public List<int> BenefitIds { get; set; } = new();
    public decimal TotalCost { get; set; }
    public decimal Subsidy { get; set; }
    public string? Remark { get; set; }
    // created_by and ip_address are set on the server
}
