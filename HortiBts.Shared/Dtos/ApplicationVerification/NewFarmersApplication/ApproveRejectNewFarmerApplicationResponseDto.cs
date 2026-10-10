using System;

namespace HortiBts.Shared.Dtos.ApplicationVerification.NewFarmersApplication;

public class ApproveRejectNewFarmerApplicationResponseDto
{
    public int ApplicationId { get; set; }
    public int Status { get; set; }
    public string Message { get; set; } = string.Empty;
}
