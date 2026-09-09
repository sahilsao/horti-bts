using HortiBts.Shared.Common;
using HortiBts.Shared.Dtos.Benefits;
using HortiBts.Shared.Dtos.Districts;
using System.Net.Http.Json;

namespace HortiBts.Client.Services.Benefits
{
    public class BenefitsApiService(HttpClient http)
    {
        public async Task<Result<List<BenefitsTypeDto>>> GetBenefitsTypesAsync()
        {
            try
            {
                var response = await http.GetAsync("api/benefits/benefits-types");
                return await ApiResultHelper.ReadResultAsync<List<BenefitsTypeDto>>(response);
            }
            catch (Exception ex)
            {
                return Result<List<BenefitsTypeDto>>.Failure($"Failed to fetch benefits types: {ex.Message}");
            }
        }

        public async Task<Result<List<BenefitsListDto>>> GetBenefitsListAsync()
        {
            try
            {
                var response = await http.GetAsync("api/benefits/benefits-list");
                return await ApiResultHelper.ReadResultAsync<List<BenefitsListDto>>(response);
            }
            catch (Exception ex)
            {
                return Result<List<BenefitsListDto>>.Failure($"Failed to fetch benefits list: {ex.Message}");
            }
        }

        public async Task<Result<int>> SaveBenefitAsync(AddBenefitDto dto)
        {
            try
            {
                var response = await http.PostAsJsonAsync("api/benefits/save-benefit-data", dto);
                return await ApiResultHelper.ReadResultAsync<int>(response);
            }
            catch (Exception ex)
            {
                return Result<int>.Failure($"Failed to save benefit: {ex.Message}");
            }
        }

        public async Task<Result<bool>> UpdateBenefitFlagAsync(int benefitId, bool flag)
        {
            try
            {
                var response = await http.PatchAsJsonAsync($"api/benefits/{benefitId}/flag", new { Flag = flag });
                return await ApiResultHelper.ReadResultAsync<bool>(response);
            }
            catch (Exception ex)
            {
                return Result<bool>.Failure($"Failed to update flag: {ex.Message}");
            }
        }
    }
}
