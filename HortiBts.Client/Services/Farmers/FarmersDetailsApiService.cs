using HortiBts.Shared.Common;
using HortiBts.Shared.Dtos.Farmers;
using System.Net.Http.Json;

namespace HortiBts.Client.Services.Farmers
{
    public class FarmersDetailsApiService(HttpClient http)
    {
        public async Task<Result<List<FarmerBasicDetailsDto>>> GetFarmersBasicDetailsAsync(string UFID)
        {
            try
            {
                var result = await http.GetFromJsonAsync<Result<List<FarmerBasicDetailsDto>>>($"api/farmer-details/get-basic-details?UFID={Uri.EscapeDataString(UFID)}");

                return result ?? Result<List<FarmerBasicDetailsDto>>.Failure("No response received.");
            }
            catch (Exception ex)
            {
                return Result<List<FarmerBasicDetailsDto>>.Failure($"Failed to fetch farmer basic details: {ex.Message}");
            }
        }

        public async Task<Result<List<FarmerAddressDetailsDto>>> GetFarmersAddressDetailsAsync(string UFID)
        {
            try
            {
                var result = await http.GetFromJsonAsync<Result<List<FarmerAddressDetailsDto>>>($"api/farmer-details/get-address-details?UFID={Uri.EscapeDataString(UFID)}");

                return result ?? Result<List<FarmerAddressDetailsDto>>.Failure("No response received.");
            }
            catch (Exception ex)
            {
                return Result<List<FarmerAddressDetailsDto>>.Failure($"Failed to fetch farmer address details: {ex.Message}");
            }
        }

        public async Task<Result<List<FarmerBankDetailsDto>>> GetFarmersBankDetailsAsync(string UFID)
        {
            try
            {
                var result = await http.GetFromJsonAsync<Result<List<FarmerBankDetailsDto>>>($"api/farmer-details/get-bank-details?UFID={Uri.EscapeDataString(UFID)}");

                return result ?? Result<List<FarmerBankDetailsDto>>.Failure("No response received.");
            }
            catch (Exception ex)
            {
                return Result<List<FarmerBankDetailsDto>>.Failure($"Failed to fetch farmer bank details: {ex.Message}");
            }
        }

        public async Task<Result<List<FarmerLandDetailsDto>>> GetFarmersLandDetailsAsync(string UFID, string FinYear)
        {
            try
            {
                var result = await http.GetFromJsonAsync<Result<List<FarmerLandDetailsDto>>>($"api/farmer-details/get-land-details?UFID={Uri.EscapeDataString(UFID)}&FinYear={Uri.EscapeDataString(FinYear)}");

                return result ?? Result<List<FarmerLandDetailsDto>>.Failure("No response received.");
            }
            catch (Exception ex)
            {
                return Result<List<FarmerLandDetailsDto>>.Failure($"Failed to fetch farmer land details: {ex.Message}");
            }
        }

        public async Task<Result<List<FarmerSchemeDetailsDto>>> GetFarmersSchemeDetailsAsync(string UFID, string FinYear)
        {
            try
            {
                var result = await http.GetFromJsonAsync<Result<List<FarmerSchemeDetailsDto>>>($"api/farmer-details/get-scheme-details?UFID={Uri.EscapeDataString(UFID)}&FinYear={Uri.EscapeDataString(FinYear)}");

                return result ?? Result<List<FarmerSchemeDetailsDto>>.Failure("No response received.");
            }
            catch (Exception ex)
            {
                return Result<List<FarmerSchemeDetailsDto>>.Failure($"Failed to fetch farmer scheme details: {ex.Message}");
            }
        }

        public async Task<Result<List<FarmerCropDetailsDto>>> GetFarmersCropDetailsAsync(string UFID, string FinYear)
        {
            try
            {
                var result = await http.GetFromJsonAsync<Result<List<FarmerCropDetailsDto>>>($"api/farmer-details/get-crop-details?UFID={Uri.EscapeDataString(UFID)}&FinYear={Uri.EscapeDataString(FinYear)}");

                return result ?? Result<List<FarmerCropDetailsDto>>.Failure("No response received.");
            }
            catch (Exception ex)
            {
                return Result<List<FarmerCropDetailsDto>>.Failure($"Failed to fetch farmer crop details: {ex.Message}");
            }
        }
    }
}
