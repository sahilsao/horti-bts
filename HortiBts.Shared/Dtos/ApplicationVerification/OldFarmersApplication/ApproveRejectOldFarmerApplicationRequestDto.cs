using System;

namespace HortiBts.Shared.Dtos.ApplicationVerification.OldFarmersApplication;

public class ApproveRejectOldFarmerApplicationRequestDto
{
    public int ApplicationId { get; set; }
    public string Remark { get; set; } = string.Empty;
    public int Status { get; set; }
}