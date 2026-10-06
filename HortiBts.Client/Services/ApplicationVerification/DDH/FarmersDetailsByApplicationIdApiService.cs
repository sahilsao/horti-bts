using HortiBts.Shared.Common;
using HortiBts.Shared.Dtos.ApplicationVerification.FarmersApplication;
using HortiBts.Shared.Dtos.Dashboard.Admin;
using HortiBts.Shared.Dtos.Farmers;
using System.Net.Http.Json;

namespace HortiBts.Client.Services.Farmers
{
    public class FarmersDetailsByApplicationIdApiService(HttpClient http)
    {
        public async Task<Result<List<FarmerApplicationBasicDetailsDto>>> GetFarmersBasicDetailsByApplicationIdAsync(int ApplicationId)
        {
            try
            {
                var response = await http.GetAsync($"api/farmer-details-by-applicationid/get-basic-details?ApplicationId={ApplicationId}");
                return await ApiResultHelper.ReadResultAsync<List<FarmerApplicationBasicDetailsDto>>(response);
            }
            catch (Exception ex)
            {
                return Result<List<FarmerApplicationBasicDetailsDto>>.Failure($"Failed to fetch farmer basic details: {ex.Message}");
            }
        }

        public async Task<Result<List<FarmerApplicationAddressDetailsDto>>> GetFarmersAddressDetailsByApplicationIdAsync(int ApplicationId)
        {
            try
            {
                var response = await http.GetAsync($"api/farmer-details-by-applicationid/get-address-details?ApplicationId={ApplicationId}");
                return await ApiResultHelper.ReadResultAsync<List<FarmerApplicationAddressDetailsDto>>(response);
            }
            catch (Exception ex)
            {
                return Result<List<FarmerApplicationAddressDetailsDto>>.Failure($"Failed to fetch farmer address details: {ex.Message}");
            }
        }

        public async Task<Result<List<FarmerApplicationBankDetailsDto>>> GetFarmersBankDetailsByApplicationIdAsync(int ApplicationId)
        {
            try
            {
                var response = await http.GetAsync($"api/farmer-details-by-applicationid/get-bank-details?ApplicationId={ApplicationId}");
                return await ApiResultHelper.ReadResultAsync<List<FarmerApplicationBankDetailsDto>>(response);
            }
            catch (Exception ex)
            {
                return Result<List<FarmerApplicationBankDetailsDto>>.Failure($"Failed to fetch farmer bank details: {ex.Message}");
            }
        }

        public async Task<Result<List<FarmerApplicationLandDetailsDto>>> GetFarmersLandDetailsByApplicationIdAsync(int ApplicationId, int FinancialYear)
        {
            try
            {
                var response = await http.GetAsync($"api/farmer-details-by-applicationid/get-land-details?ApplicationId={ApplicationId}&FinancialYear={FinancialYear}");
                return await ApiResultHelper.ReadResultAsync<List<FarmerApplicationLandDetailsDto>>(response);
            }
            catch (Exception ex)
            {
                return Result<List<FarmerApplicationLandDetailsDto>>.Failure($"Failed to fetch farmer land details: {ex.Message}");
            }
        }

        public async Task<Result<List<FarmerApplicationSchemeDetailsDto>>> GetFarmersSchemeDetailsByApplicationIdAsync(int ApplicationId, int FinancialYear)
        {
            try
            {
                var response = await http.GetAsync($"api/farmer-details-by-applicationid/get-scheme-details?ApplicationId={ApplicationId}&FinancialYear={FinancialYear}");
                return await ApiResultHelper.ReadResultAsync<List<FarmerApplicationSchemeDetailsDto>>(response);
            }
            catch (Exception ex)
            {
                return Result<List<FarmerApplicationSchemeDetailsDto>>.Failure($"Failed to fetch farmer scheme details: {ex.Message}");
            }
        }

        public async Task<Result<List<FarmerApplicationCropDetailsDto>>> GetFarmersCropDetailsByApplicationIdAsync(int ApplicationId, int FinancialYear)
        {
            try
            {
                var response = await http.GetAsync($"api/farmer-details-by-applicationid/get-crop-details?ApplicationId={ApplicationId}&FinancialYear={FinancialYear}");
                return await ApiResultHelper.ReadResultAsync<List<FarmerApplicationCropDetailsDto>>(response);
            }
            catch (Exception ex)
            {
                return Result<List<FarmerApplicationCropDetailsDto>>.Failure($"Failed to fetch farmer crop details: {ex.Message}");
            }
        }
    }
}
