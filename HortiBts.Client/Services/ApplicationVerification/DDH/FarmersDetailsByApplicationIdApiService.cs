using HortiBts.Shared.Common;
using HortiBts.Shared.Dtos.ApplicationVerification.FarmersApplication;
using HortiBts.Shared.Dtos.Dashboard.Admin;
using HortiBts.Shared.Dtos.Farmers;
using System.Net.Http.Json;

namespace HortiBts.Client.Services.Farmers
{
    public class FarmersDetailsByApplicationIdApiService(HttpClient http)
    {
        public async Task<Result<List<FarmerApplicationBasicDetailsDto>>> GetFarmersBasicDetailsByApplicationIdAsync(int applicationId)
        {
            try
            {
                var response = await http.GetAsync($"api/farmer-details-by-application-id/get-basic-details?applicationId={applicationId}");
                return await ApiResultHelper.ReadResultAsync<List<FarmerApplicationBasicDetailsDto>>(response);
            }
            catch (Exception ex)
            {
                return Result<List<FarmerApplicationBasicDetailsDto>>.Failure($"Failed to fetch farmer basic details: {ex.Message}");
            }
        }

        public async Task<Result<List<FarmerApplicationAddressDetailsDto>>> GetFarmersAddressDetailsByApplicationIdAsync(int applicationId)
        {
            try
            {
                var response = await http.GetAsync($"api/farmer-details-by-application-id/get-address-details?applicationId={applicationId}");
                return await ApiResultHelper.ReadResultAsync<List<FarmerApplicationAddressDetailsDto>>(response);
            }
            catch (Exception ex)
            {
                return Result<List<FarmerApplicationAddressDetailsDto>>.Failure($"Failed to fetch farmer address details: {ex.Message}");
            }
        }

        public async Task<Result<List<FarmerApplicationBankDetailsDto>>> GetFarmersBankDetailsByApplicationIdAsync(int applicationId)
        {
            try
            {
                var response = await http.GetAsync($"api/farmer-details-by-application-id/get-bank-details?applicationId={applicationId}");
                return await ApiResultHelper.ReadResultAsync<List<FarmerApplicationBankDetailsDto>>(response);
            }
            catch (Exception ex)
            {
                return Result<List<FarmerApplicationBankDetailsDto>>.Failure($"Failed to fetch farmer bank details: {ex.Message}");
            }
        }

        public async Task<Result<List<FarmerApplicationLandDetailsDto>>> GetFarmersLandDetailsByApplicationIdAsync(int applicationId, int financialYear)
        {
            try
            {
                var response = await http.GetAsync($"api/farmer-details-by-application-id/get-land-details?applicationId={applicationId}&financialYear={financialYear}");
                return await ApiResultHelper.ReadResultAsync<List<FarmerApplicationLandDetailsDto>>(response);
            }
            catch (Exception ex)
            {
                return Result<List<FarmerApplicationLandDetailsDto>>.Failure($"Failed to fetch farmer land details: {ex.Message}");
            }
        }

        public async Task<Result<List<FarmerApplicationSchemeDetailsDto>>> GetFarmersSchemeDetailsByApplicationIdAsync(int applicationId, int financialYear)
        {
            try
            {
                var response = await http.GetAsync($"api/farmer-details-by-application-id/get-scheme-details?applicationId={applicationId}&financialYear={financialYear}");
                return await ApiResultHelper.ReadResultAsync<List<FarmerApplicationSchemeDetailsDto>>(response);
            }
            catch (Exception ex)
            {
                return Result<List<FarmerApplicationSchemeDetailsDto>>.Failure($"Failed to fetch farmer scheme details: {ex.Message}");
            }
        }

        public async Task<Result<List<FarmerApplicationCropDetailsDto>>> GetFarmersCropDetailsByApplicationIdAsync(int applicationId, int financialYear)
        {
            try
            {
                var response = await http.GetAsync($"api/farmer-details-by-application-id/get-crop-details?applicationId={applicationId}&financialYear={financialYear}");
                return await ApiResultHelper.ReadResultAsync<List<FarmerApplicationCropDetailsDto>>(response);
            }
            catch (Exception ex)
            {
                return Result<List<FarmerApplicationCropDetailsDto>>.Failure($"Failed to fetch farmer crop details: {ex.Message}");
            }
        }
    }
}
