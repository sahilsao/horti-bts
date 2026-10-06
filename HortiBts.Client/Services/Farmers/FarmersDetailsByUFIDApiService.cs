using HortiBts.Shared.Common;
using HortiBts.Shared.Dtos.Dashboard.Admin;
using HortiBts.Shared.Dtos.Farmers;
using System.Net.Http.Json;

namespace HortiBts.Client.Services.Farmers
{
    public class FarmersDetailsByUFIDApiService(HttpClient http)
    {
        public async Task<Result<List<FarmerBasicDetailsDto>>> GetFarmersBasicDetailsByUFIDAsync(string UFID)
        {
            try
            {
                var response = await http.GetAsync($"api/farmer-details-by-ufid/get-basic-details?UFID={Uri.EscapeDataString(UFID)}");
                return await ApiResultHelper.ReadResultAsync<List<FarmerBasicDetailsDto>>(response);
            }
            catch (Exception ex)
            {
                return Result<List<FarmerBasicDetailsDto>>.Failure($"Failed to fetch farmer basic details: {ex.Message}");
            }
        }

        public async Task<Result<List<FarmerAddressDetailsDto>>> GetFarmersAddressDetailsByUFIDAsync(string UFID)
        {
            try
            {
                var response = await http.GetAsync($"api/farmer-details-by-ufid/get-address-details?UFID={Uri.EscapeDataString(UFID)}");
                return await ApiResultHelper.ReadResultAsync<List<FarmerAddressDetailsDto>>(response);
            }
            catch (Exception ex)
            {
                return Result<List<FarmerAddressDetailsDto>>.Failure($"Failed to fetch farmer address details: {ex.Message}");
            }
        }

        public async Task<Result<List<FarmerBankDetailsDto>>> GetFarmersBankDetailsByUFIDAsync(string UFID)
        {
            try
            {
                var response = await http.GetAsync($"api/farmer-details-by-ufid/get-bank-details?UFID={Uri.EscapeDataString(UFID)}");
                return await ApiResultHelper.ReadResultAsync<List<FarmerBankDetailsDto>>(response);
            }
            catch (Exception ex)
            {
                return Result<List<FarmerBankDetailsDto>>.Failure($"Failed to fetch farmer bank details: {ex.Message}");
            }
        }

        public async Task<Result<List<FarmerLandDetailsDto>>> GetFarmersLandDetailsByUFIDAsync(string UFID, string FinYear)
        {
            try
            {
                var response = await http.GetAsync($"api/farmer-details-by-ufid/get-land-details?UFID={Uri.EscapeDataString(UFID)}&FinYear={Uri.EscapeDataString(FinYear)}");
                return await ApiResultHelper.ReadResultAsync<List<FarmerLandDetailsDto>>(response);
            }
            catch (Exception ex)
            {
                return Result<List<FarmerLandDetailsDto>>.Failure($"Failed to fetch farmer land details: {ex.Message}");
            }
        }

        public async Task<Result<List<FarmerSchemeDetailsDto>>> GetFarmersSchemeDetailsByUFIDAsync(string UFID, string FinYear)
        {
            try
            {
                var response = await http.GetAsync($"api/farmer-details-by-ufid/get-scheme-details?UFID={Uri.EscapeDataString(UFID)}&FinYear={Uri.EscapeDataString(FinYear)}");
                return await ApiResultHelper.ReadResultAsync<List<FarmerSchemeDetailsDto>>(response);
            }
            catch (Exception ex)
            {
                return Result<List<FarmerSchemeDetailsDto>>.Failure($"Failed to fetch farmer scheme details: {ex.Message}");
            }
        }

        public async Task<Result<List<FarmerCropDetailsDto>>> GetFarmersCropDetailsByUFIDAsync(string UFID, string FinYear)
        {
            try
            {
                var response = await http.GetAsync($"api/farmer-details-by-ufid/get-crop-details?UFID={Uri.EscapeDataString(UFID)}&FinYear={Uri.EscapeDataString(FinYear)}");
                return await ApiResultHelper.ReadResultAsync<List<FarmerCropDetailsDto>>(response);
            }
            catch (Exception ex)
            {
                return Result<List<FarmerCropDetailsDto>>.Failure($"Failed to fetch farmer crop details: {ex.Message}");
            }
        }
    }
}
