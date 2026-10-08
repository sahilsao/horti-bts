using System;

namespace HortiBts.Shared.Dtos.ApplicationVerification.FarmersApplication;

public class ApproveRejectFarmerApplicationResponseDto
{
    public int ApplicationId { get; set; }
    public int Status { get; set; }
    public string Message { get; set; } = string.Empty;
}
