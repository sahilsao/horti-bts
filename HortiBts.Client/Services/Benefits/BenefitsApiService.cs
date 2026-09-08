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
                var data = await http.GetFromJsonAsync<Result<List<BenefitsTypeDto>>>("api/benefits/benefits-types");
                return data ?? Result<List<BenefitsTypeDto>>.Failure("No response received.");
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
                var data = await http.GetFromJsonAsync<Result<List<BenefitsListDto>>>("api/benefits/benefits-list");
                return data ?? Result<List<BenefitsListDto>>.Failure("No response received.");
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

                if (!response.IsSuccessStatusCode)
                    return Result<int>.Failure($"Failed to save benefit: {response.StatusCode}");

                var benefitId = await response.Content.ReadFromJsonAsync<int>();
                return Result<int>.Success(benefitId);
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

                if (!response.IsSuccessStatusCode)
                    return Result<bool>.Failure($"Failed to update flag: {response.StatusCode}");

                return Result<bool>.Success(true);
            }
            catch (Exception ex)
            {
                return Result<bool>.Failure($"Failed to update flag: {ex.Message}");
            }
        }
    }
}
