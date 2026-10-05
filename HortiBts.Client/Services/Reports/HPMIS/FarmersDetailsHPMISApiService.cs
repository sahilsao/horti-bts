using HortiBts.Shared.Common;
using HortiBts.Shared.Dtos.HPMIS;

namespace HortiBts.Client.Services.HPMIS
{
    public class FarmersDetailsHPMISApiService(HttpClient http)
    {
        public async Task<Result<List<FarmerBasicDetailsHPMISDto>>> GetFarmersBasicDetailsAsync(string applicationId)
        {
            try
            {
                var response = await http.GetAsync($"api/reports/hpmis/get-basic-details?applicationId={Uri.EscapeDataString(applicationId)}");
                return await ApiResultHelper.ReadResultAsync<List<FarmerBasicDetailsHPMISDto>>(response);
            }
            catch (Exception ex)
            {
                return Result<List<FarmerBasicDetailsHPMISDto>>.Failure($"Failed to fetch farmer basic details: {ex.Message}");
            }
        }

        public async Task<Result<List<FarmerApplicationDetailsHPMISDto>>> GetFarmersApplicationDetailsAsync(string applicationId)
        {
            try
            {
                var response = await http.GetAsync($"api/reports/hpmis/get-application-details?applicationId={Uri.EscapeDataString(applicationId)}");
                return await ApiResultHelper.ReadResultAsync<List<FarmerApplicationDetailsHPMISDto>>(response);
            }
            catch (Exception ex)
            {
                return Result<List<FarmerApplicationDetailsHPMISDto>>.Failure($"Failed to fetch farmer application details: {ex.Message}");
            }
        }

        public async Task<Result<List<FarmerLandDetailsHPMISDto>>> GetFarmersLandDetailsAsync(string applicationId)
        {
            try
            {
                var response = await http.GetAsync($"api/reports/hpmis/get-land-details?applicationId={Uri.EscapeDataString(applicationId)}");
                return await ApiResultHelper.ReadResultAsync<List<FarmerLandDetailsHPMISDto>>(response);
            }
            catch (Exception ex)
            {
                return Result<List<FarmerLandDetailsHPMISDto>>.Failure($"Failed to fetch farmer land details: {ex.Message}");
            }
        }

        public async Task<Result<List<FarmerCropDetailsHPMISDto>>> GetFarmersCropDetailsAsync(string applicationId)
        {
            try
            {
                var response = await http.GetAsync($"api/reports/hpmis/get-crop-details?applicationId={Uri.EscapeDataString(applicationId)}");
                return await ApiResultHelper.ReadResultAsync<List<FarmerCropDetailsHPMISDto>>(response);
            }
            catch (Exception ex)
            {
                return Result<List<FarmerCropDetailsHPMISDto>>.Failure($"Failed to fetch farmer crop details: {ex.Message}");
            }
        }

        public async Task<Result<List<FarmerMarketLinkageDetailsHPMISDto>>> GetFarmersMarketLinkageDetailsAsync(string applicationId)
        {
            try
            {
                var response = await http.GetAsync($"api/reports/hpmis/get-market-linkage-details?applicationId={Uri.EscapeDataString(applicationId)}");
                return await ApiResultHelper.ReadResultAsync<List<FarmerMarketLinkageDetailsHPMISDto>>(response);
            }
            catch (Exception ex)
            {
                return Result<List<FarmerMarketLinkageDetailsHPMISDto>>.Failure($"Failed to fetch farmer market linkage details: {ex.Message}");
            }
        }
        public async Task<Result<List<FarmerSchemeDetailsHPMISDto>>> GetFarmersSchemeDetailsAsync(string applicationId)
        {
            try
            {
                var response = await http.GetAsync($"api/reports/hpmis/get-scheme-details?applicationId={Uri.EscapeDataString(applicationId)}");
                return await ApiResultHelper.ReadResultAsync<List<FarmerSchemeDetailsHPMISDto>>(response);
            }
            catch (Exception ex)
            {
                return Result<List<FarmerSchemeDetailsHPMISDto>>.Failure($"Failed to fetch farmer scheme details: {ex.Message}");
            }
        }
    }
}
