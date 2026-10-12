using System;
using System.Net.Http.Json;
using HortiBts.Shared.Common;
using HortiBts.Shared.Dtos.ApplicationVerification.NewFarmersApplication;

namespace HortiBts.Client.Services.ApplicationVerification.DDH.NewApplication;

public class ApproveRejectrOldFarmerApplicationByDDHApiService(HttpClient http)
{
    public async Task<Result<int>> ApproveRejectOldFarmerApplicationAsync(int applicationId, int status, string remark)
    {
        try
        {
            var request = new ApproveRejectNewFarmerApplicationRequestDto
            {
                ApplicationId = applicationId,
                Status = status,
                Remark = remark
            };

            var response = await http.PostAsJsonAsync("api/district/old-application-verification-by-ddh/approve-reject", request);
            return await ApiResultHelper.ReadResultAsync<int>(response);
        }
        catch (Exception ex)
        {
            return Result<int>.Failure($"Failed to approve or reject farmer application: {ex.Message}");
        }
    }
}
