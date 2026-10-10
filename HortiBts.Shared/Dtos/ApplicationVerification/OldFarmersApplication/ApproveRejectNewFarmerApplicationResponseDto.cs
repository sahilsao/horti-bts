using System;

namespace HortiBts.Shared.Dtos.ApplicationVerification.OldFarmersApplication;

public class ApproveRejectOldFarmerApplicationResponseDto
{
    public int ApplicationId { get; set; }
    public int Status { get; set; }
    public string Message { get; set; } = string.Empty;
}
