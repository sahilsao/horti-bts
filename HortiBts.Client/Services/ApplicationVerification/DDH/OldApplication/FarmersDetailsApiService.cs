using HortiBts.Shared.Common;
using HortiBts.Shared.Dtos.ApplicationVerification.OldFarmersApplication;

namespace HortiBts.Client.Services.ApplicationVerification.DDH.OldApplication
{
    public class FarmersDetailsApiService(HttpClient http)
    {
        public async Task<Result<List<OldFarmerApplicationBasicDetailsDto>>> GetFarmersBasicDetailsByHfIdAsync(int hfId)
        {
            try
            {
                var response = await http.GetAsync($"api/farmer-details/get-basic-details-by-hfid?hFid={hfId}");
                return await ApiResultHelper.ReadResultAsync<List<OldFarmerApplicationBasicDetailsDto>>(response);
            }
            catch (Exception ex)
            {
                return Result<List<OldFarmerApplicationBasicDetailsDto>>.Failure($"Failed to fetch farmer basic details: {ex.Message}");
            }
        }

        public async Task<Result<List<OldFarmerApplicationAddressDetailsDto>>> GetFarmersAddressDetailsByFdIdAsync(int fdId)
        {
            try
            {
                var response = await http.GetAsync($"api/farmer-details/get-address-details-by-fdid?fdId={fdId}");
                return await ApiResultHelper.ReadResultAsync<List<OldFarmerApplicationAddressDetailsDto>>(response);
            }
            catch (Exception ex)
            {
                return Result<List<OldFarmerApplicationAddressDetailsDto>>.Failure($"Failed to fetch farmer address details: {ex.Message}");
            }
        }

        public async Task<Result<List<OldFarmerApplicationBankDetailsDto>>> GetFarmersBankDetailsByFdIdAsync(int fdId)
        {
            try
            {
                var response = await http.GetAsync($"api/farmer-details/get-bank-details-by-fdid?fdId={fdId}");
                return await ApiResultHelper.ReadResultAsync<List<OldFarmerApplicationBankDetailsDto>>(response);
            }
            catch (Exception ex)
            {
                return Result<List<OldFarmerApplicationBankDetailsDto>>.Failure($"Failed to fetch farmer bank details: {ex.Message}");
            }
        }

        public async Task<Result<List<OldFarmerApplicationLandDetailsDto>>> GetFarmersLandDetailsByFdIdAsync(int fdId, string villageType, int financialYear)
        {
            try
            {
                var response = await http.GetAsync($"api/farmer-details/get-land-details-by-fdid?fdId={fdId}&villageType={villageType}&financialYear={financialYear}");
                return await ApiResultHelper.ReadResultAsync<List<OldFarmerApplicationLandDetailsDto>>(response);
            }
            catch (Exception ex)
            {
                return Result<List<OldFarmerApplicationLandDetailsDto>>.Failure($"Failed to fetch farmer land details: {ex.Message}");
            }
        }

        public async Task<Result<List<OldFarmerApplicationSchemeDetailsDto>>> GetFarmersSchemeDetailsByFdIdAsync(int fdId, int sdId, int financialYear)
        {
            try
            {
                var response = await http.GetAsync($"api/farmer-details/get-scheme-details-by-fdid?fdId={fdId}&sdId={sdId}&financialYear={financialYear}");
                return await ApiResultHelper.ReadResultAsync<List<OldFarmerApplicationSchemeDetailsDto>>(response);
            }
            catch (Exception ex)
            {
                return Result<List<OldFarmerApplicationSchemeDetailsDto>>.Failure($"Failed to fetch farmer scheme details: {ex.Message}");
            }
        }

        public async Task<Result<List<OldFarmerApplicationCropDetailsDto>>> GetFarmersCropDetailsByFdIdAsync(int fdId, int financialYear)
        {
            try
            {
                var response = await http.GetAsync($"api/farmer-details/get-crop-details-by-fdid?fdId={fdId}&financialYear={financialYear}");
                return await ApiResultHelper.ReadResultAsync<List<OldFarmerApplicationCropDetailsDto>>(response);
            }
            catch (Exception ex)
            {
                return Result<List<OldFarmerApplicationCropDetailsDto>>.Failure($"Failed to fetch farmer crop details: {ex.Message}");
            }
        }
        public async Task<Result<List<OldFarmerApplicationMachineryDetailsDto>>> GetFarmersMachineryDetailsByFdIdAsync(int fdId, int financialYear)
        {
            try
            {
                var response = await http.GetAsync($"api/farmer-details/get-machinery-details-by-fdid?fdId={fdId}&financialYear={financialYear}");
                return await ApiResultHelper.ReadResultAsync<List<OldFarmerApplicationMachineryDetailsDto>>(response);
            }
            catch (Exception ex)
            {
                return Result<List<OldFarmerApplicationMachineryDetailsDto>>.Failure($"Failed to fetch farmer machinery details: {ex.Message}");
            }
        }
    }
}
