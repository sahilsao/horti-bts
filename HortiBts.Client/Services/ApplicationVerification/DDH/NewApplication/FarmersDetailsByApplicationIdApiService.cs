using HortiBts.Shared.Common;
using HortiBts.Shared.Dtos.ApplicationVerification.NewFarmersApplication;
using HortiBts.Shared.Dtos.Dashboard.Admin;
using HortiBts.Shared.Dtos.Farmers;
using System.Net.Http.Json;

namespace HortiBts.Client.Services.ApplicationVerification.DDH.NewApplication
{
    public class FarmersDetailsByApplicationIdApiService(HttpClient http)
    {
        public async Task<Result<List<NewFarmerApplicationBasicDetailsDto>>> GetFarmersBasicDetailsByApplicationIdAsync(int applicationId)
        {
            try
            {
                var response = await http.GetAsync($"api/farmer-details-by-application-id/get-basic-details?applicationId={applicationId}");
                return await ApiResultHelper.ReadResultAsync<List<NewFarmerApplicationBasicDetailsDto>>(response);
            }
            catch (Exception ex)
            {
                return Result<List<NewFarmerApplicationBasicDetailsDto>>.Failure($"Failed to fetch farmer basic details: {ex.Message}");
            }
        }

        public async Task<Result<List<NewFarmerApplicationAddressDetailsDto>>> GetFarmersAddressDetailsByApplicationIdAsync(int applicationId)
        {
            try
            {
                var response = await http.GetAsync($"api/farmer-details-by-application-id/get-address-details?applicationId={applicationId}");
                return await ApiResultHelper.ReadResultAsync<List<NewFarmerApplicationAddressDetailsDto>>(response);
            }
            catch (Exception ex)
            {
                return Result<List<NewFarmerApplicationAddressDetailsDto>>.Failure($"Failed to fetch farmer address details: {ex.Message}");
            }
        }

        public async Task<Result<List<NewFarmerApplicationBankDetailsDto>>> GetFarmersBankDetailsByApplicationIdAsync(int applicationId)
        {
            try
            {
                var response = await http.GetAsync($"api/farmer-details-by-application-id/get-bank-details?applicationId={applicationId}");
                return await ApiResultHelper.ReadResultAsync<List<NewFarmerApplicationBankDetailsDto>>(response);
            }
            catch (Exception ex)
            {
                return Result<List<NewFarmerApplicationBankDetailsDto>>.Failure($"Failed to fetch farmer bank details: {ex.Message}");
            }
        }

        public async Task<Result<List<NewFarmerApplicationLandDetailsDto>>> GetFarmersLandDetailsByApplicationIdAsync(int applicationId, int financialYear)
        {
            try
            {
                var response = await http.GetAsync($"api/farmer-details-by-application-id/get-land-details?applicationId={applicationId}&financialYear={financialYear}");
                return await ApiResultHelper.ReadResultAsync<List<NewFarmerApplicationLandDetailsDto>>(response);
            }
            catch (Exception ex)
            {
                return Result<List<NewFarmerApplicationLandDetailsDto>>.Failure($"Failed to fetch farmer land details: {ex.Message}");
            }
        }

        public async Task<Result<List<NewFarmerApplicationSchemeDetailsDto>>> GetFarmersSchemeDetailsByApplicationIdAsync(int applicationId, int financialYear)
        {
            try
            {
                var response = await http.GetAsync($"api/farmer-details-by-application-id/get-scheme-details?applicationId={applicationId}&financialYear={financialYear}");
                return await ApiResultHelper.ReadResultAsync<List<NewFarmerApplicationSchemeDetailsDto>>(response);
            }
            catch (Exception ex)
            {
                return Result<List<NewFarmerApplicationSchemeDetailsDto>>.Failure($"Failed to fetch farmer scheme details: {ex.Message}");
            }
        }

        public async Task<Result<List<NewFarmerApplicationCropDetailsDto>>> GetFarmersCropDetailsByApplicationIdAsync(int applicationId, int financialYear)
        {
            try
            {
                var response = await http.GetAsync($"api/farmer-details-by-application-id/get-crop-details?applicationId={applicationId}&financialYear={financialYear}");
                return await ApiResultHelper.ReadResultAsync<List<NewFarmerApplicationCropDetailsDto>>(response);
            }
            catch (Exception ex)
            {
                return Result<List<NewFarmerApplicationCropDetailsDto>>.Failure($"Failed to fetch farmer crop details: {ex.Message}");
            }
        }
    }
}
