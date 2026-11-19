using HortiBts.Shared.Common;
using HortiBts.Shared.Dtos.Officers;
using HortiBts.Shared.Dtos.SubDistricts;
using System.Net.Http.Json;

namespace HortiBts.Client.Services.Officers
{
    public class OfficersApiService(HttpClient http)
    {
        public async Task<Result<List<DeptOfficersDto>>> GetAllDepartmentOfficersListAsync(int departmentCode)
        {
            try
            {
                var response = await http.GetAsync($"api/officers/get-all-dept-officers?departmentCode={departmentCode}");
                return await ApiResultHelper.ReadResultAsync<List<DeptOfficersDto>>(response);
            }
            catch (Exception ex)
            {
                return Result<List<DeptOfficersDto>>.Failure($"Failed to fetch dept officers: {ex.Message}");
            }
        }

        public async Task<Result<List<RheoOfficersDto>>> GetAllRheoOfficersListAsync(int departmentCode)
        {
            try
            {
                var response = await http.GetAsync($"api/officers/get-all-rheo-officers?departmentCode={departmentCode}");
                return await ApiResultHelper.ReadResultAsync<List<RheoOfficersDto>>(response);
            }
            catch (Exception ex)
            {
                return Result<List<RheoOfficersDto>>.Failure($"Failed to fetch rheo officers: {ex.Message}");
            }
        }

        public async Task<Result<List<RheoOfficersDto>>> GetRheoOfficersListFromDistrictCodeAsync(int departmentCode, int districtCode)
        {
            try
            {
                var response = await http.GetAsync($"api/officers/get-rheo-officers-from-district?departmentCode={departmentCode}&districtCode={districtCode}");
                return await ApiResultHelper.ReadResultAsync<List<RheoOfficersDto>>(response);
            }
            catch (Exception ex)
            {
                return Result<List<RheoOfficersDto>>.Failure($"Failed to fetch rheo officers: {ex.Message}");
            }
        }

        public async Task<Result<List<RheoOfficersDto>>> GetRheoOfficersListFromSubDistrictCodeAsync(int departmentCode, int subDistrictCode)
        {
            try
            {
                var response = await http.GetAsync($"api/officers/get-rheo-officers-from-sub-district?departmentCode={departmentCode}&subDistrictCode={subDistrictCode}");
                return await ApiResultHelper.ReadResultAsync<List<RheoOfficersDto>>(response);
            }
            catch (Exception ex)
            {
                return Result<List<RheoOfficersDto>>.Failure($"Failed to fetch rheo officers: {ex.Message}");
            }
        }
        public async Task<Result<List<RheoOfficersDto>>> GetRheoOfficersListFromVillageCodeAsync(int departmentCode, int villageCode)
        {
            try
            {
                var response = await http.GetAsync($"api/officers/get-rheo-officers-from-village?departmentCode={departmentCode}&villageCode={villageCode}");
                return await ApiResultHelper.ReadResultAsync<List<RheoOfficersDto>>(response);
            }
            catch (Exception ex)
            {
                return Result<List<RheoOfficersDto>>.Failure($"Failed to fetch rheo officers: {ex.Message}");
            }
        }

        public async Task<Result<List<RheoOfficersPreviousAvailableVillagesDto>>> GetRheoOfficerPreviousAvailableVillagesListAsync(int officerCode, int flag)
        {
            try
            {
                var response = await http.GetAsync($"api/officers/get-rheo-officer-previously-available-villages?officerCode={officerCode}&flag={flag}");
                return await ApiResultHelper.ReadResultAsync<List<RheoOfficersPreviousAvailableVillagesDto>>(response);
            }
            catch (Exception ex)
            {
                return Result<List<RheoOfficersPreviousAvailableVillagesDto>>.Failure($"Failed to fetch previously available village of rheo officer: {ex.Message}");
            }
        }

        public async Task<Result<List<RheoOfficerMappedVillagesDto>>> GetRheoOfficerMappedVillagesListAsync(int departmentCode, int officerCode)
        {
            try
            {
                var response = await http.GetAsync($"api/officers/get-rheo-officer-mapped-villages?departmentCode={departmentCode}&officerCode={officerCode}");
                return await ApiResultHelper.ReadResultAsync<List<RheoOfficerMappedVillagesDto>>(response);
            }
            catch (Exception ex)
            {
                return Result<List<RheoOfficerMappedVillagesDto>>.Failure($"Failed to fetch mapped villages of rheo officer: {ex.Message}");
            }
        }

        public async Task<Result<List<RheoOfficersForChargeTransferDto>>> GetRheoOfficersListForChargeTranserAsync(int departmentCode, int districtCode)
        {
            try
            {
                var response = await http.GetAsync($"api/officers/get-rheo-officer-list-for-charge-transfer?departmentCode={departmentCode}&districtCode={districtCode}");
                return await ApiResultHelper.ReadResultAsync<List<RheoOfficersForChargeTransferDto>>(response);
            }
            catch (Exception ex)
            {
                return Result<List<RheoOfficersForChargeTransferDto>>.Failure($"Failed to fetch rheo officer available for charge transfer: {ex.Message}");
            }
        }

        public async Task<Result<int>> SaveOfficerAsync(SaveOfficerDto dto)
        {
            try
            {
                var response = await http.PostAsJsonAsync("api/officers/save-officer", dto);
                return await ApiResultHelper.ReadResultAsync<int>(response);   // new officer id
            }
            catch (Exception ex)
            {
                return Result<int>.Failure($"Failed to save officer: {ex.Message}");
            }
        }

        public async Task<Result<ChargeTransferResultDto>> SaveOfficerChargeTransferAsync(SaveOfficerChargeTransferDto dto)
        {
            try
            {
                var response = await http.PostAsJsonAsync("api/officers/transfer-charge", dto);
                return await ApiResultHelper.ReadResultAsync<ChargeTransferResultDto>(response);
            }
            catch (Exception ex)
            {
                return Result<ChargeTransferResultDto>.Failure($"Failed to transfer charge: {ex.Message}");
            }
        }
    }
}
