using System;
using HortiBts.Shared.Common;
using HortiBts.Shared.Dtos.AddBeneficiaryGrant;

namespace HortiBts.Client.Services.AddBeneficiaryGrant;

public class ApprovedApplicationsWithNoAddedBenefitsApiService(HttpClient http)
{
    public async Task<Result<List<ApprovedApplicationsWithNoAddedBenefitsByDistrictCodeDto>>> GetApprovedApplicationsWithNoAddedBenefitsByDistrictCodeAsync(int districtCode, int financialYear)
    {
        try
        {
            var response = await http.GetAsync($"api/approved-applications/with-no-added-benefits-by-district?districtCode={districtCode}&financialYear={financialYear}");
            return await ApiResultHelper.ReadResultAsync<List<ApprovedApplicationsWithNoAddedBenefitsByDistrictCodeDto>>(response);
        }
        catch (Exception ex)
        {
            return Result<List<ApprovedApplicationsWithNoAddedBenefitsByDistrictCodeDto>>.Failure($"Failed to fetch approved applications with no added benefits: {ex.Message}");
        }
    }

    public async Task<Result<List<ApprovedApplicationsWithNoAddedBenefitsByApplicationIdDto>>> GetApprovedApplicationsWithNoAddedBenefitsByApplicationIdsync(int applicationId, int financialYear)
    {
        try
        {
            var response = await http.GetAsync($"api/approved-applications/with-no-added-benefits-by-application-id?applicationId={applicationId}&financialYear={financialYear}");
            return await ApiResultHelper.ReadResultAsync<List<ApprovedApplicationsWithNoAddedBenefitsByApplicationIdDto>>(response);
        }
        catch (Exception ex)
        {
            return Result<List<ApprovedApplicationsWithNoAddedBenefitsByApplicationIdDto>>.Failure($"Failed to fetch approved applications with no added benefits: {ex.Message}");
        }
    }
}
