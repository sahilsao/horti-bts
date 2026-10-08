using System;

namespace HortiBts.Shared.Dtos.ApplicationVerification.FarmersApplication;

public class ApproveRejectFarmerApplicationRequestDto
{
    public int ApplicationId { get; set; }
    public string Remark { get; set; } = string.Empty;
    public int Status { get; set; }
}