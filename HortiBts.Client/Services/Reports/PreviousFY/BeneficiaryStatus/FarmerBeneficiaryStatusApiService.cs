using HortiBts.Shared.Common;
using HortiBts.Shared.Dtos.Reports.PreviousFY.BeneficiaryStatus;

namespace HortiBts.Client.Services.Reports.PreviousFY.Comparative
{
    public class FarmerBeneficiaryStatusApiService(HttpClient http)
    {
        public async Task<Result<List<DistWiseFarmerBeneficiaryStatusDto>>> GetRptOfDistWiseFarmerBeneficiaryStatusListAsync()
        {
            try
            {
                var response = await http.GetAsync("api/reports/beneficiary/previous/distwise-farmer-status");
                return await ApiResultHelper.ReadResultAsync<List<DistWiseFarmerBeneficiaryStatusDto>>(response);
            }
            catch (Exception ex)
            {
                return Result<List<DistWiseFarmerBeneficiaryStatusDto>>.Failure($"Failed to fetch district-wise farmer beneficiary status report: {ex.Message}");
            }
        }

        public async Task<Result<List<BlockWiseFarmerBeneficiaryStatusDto>>> GetRptOfBlockWiseFarmerBeneficiaryStatusListAsync(int districtCode)
        {
            try
            {
                var response = await http.GetAsync($"api/reports/beneficiary/previous/blockwise-farmer-status?districtCode={districtCode}");
                return await ApiResultHelper.ReadResultAsync<List<BlockWiseFarmerBeneficiaryStatusDto>>(response);
            }
            catch (Exception ex)
            {
                return Result<List<BlockWiseFarmerBeneficiaryStatusDto>>.Failure($"Failed to fetch block-wise farmer beneficiary status report: {ex.Message}");
            }
        }

        public async Task<Result<List<VillageWiseFarmerBeneficiaryStatusDto>>> GetRptOfVillageWiseFarmerBeneficiaryStatusListAsync(int subDistrictCode)
        {
            try
            {
                var response = await http.GetAsync($"api/reports/beneficiary/previous/villagewise-farmer-status?subDistrictCode={subDistrictCode}");
                return await ApiResultHelper.ReadResultAsync<List<VillageWiseFarmerBeneficiaryStatusDto>>(response);
            }
            catch (Exception ex)
            {
                return Result<List<VillageWiseFarmerBeneficiaryStatusDto>>.Failure($"Failed to fetch village-wise farmer beneficiary status report: {ex.Message}");
            }
        }

        public async Task<Result<List<FarmerWiseFarmerBeneficiaryStatusDto>>> GetRptOfFarmerWiseFarmerBeneficiaryStatusListAsync(int villageCode, int? statusCode = null)
        {
            try
            {
                var url = $"api/reports/beneficiary/previous/farmerwise-farmer-status?villageCode={villageCode}";

                if (statusCode.HasValue)
                {
                    url += $"&statusCode={statusCode.Value}";
                }

                var response = await http.GetAsync(url);

                return await ApiResultHelper.ReadResultAsync<List<FarmerWiseFarmerBeneficiaryStatusDto>>(response);
            }
            catch (Exception ex)
            {
                return Result<List<FarmerWiseFarmerBeneficiaryStatusDto>>.Failure($"Failed to fetch farmer-wise farmer beneficiary status report: {ex.Message}");
            }
        }
    }
}
