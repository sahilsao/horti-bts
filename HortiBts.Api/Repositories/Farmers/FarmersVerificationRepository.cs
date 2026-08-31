using Dapper;
using HortiBts.Api.Data;
using HortiBts.Shared.Common;
using HortiBts.Shared.Dtos.Farmers;

namespace HortiBts.Api.Repositories.Farmers
{
    public interface IFarmersVerificationRepository
    {
        /// <summary>Returns farmers verification details from ufp db</summary>
        Task<Result<List<FarmersListByVillageForUFPVerificationDto>>> GetFarmersListByVillageFromUFPAsync(string VillageCode);
    }
    public class FarmersVerificationRepository(IDbConnectionFactory dbFactory, ILogger<FarmersVerificationRepository> logger) : IFarmersVerificationRepository
    {
        public async Task<Result<List<FarmersListByVillageForUFPVerificationDto>>> GetFarmersListByVillageFromUFPAsync(string VillageCode)
        {
            try
            {
                using var connection = dbFactory.CreateConnection(HortiDb.NewUfp);
                const string sql = """
                WITH crop AS(
                SELECT DISTINCT
                    ld.khasra_no AS khasra_no,
                    ld.uf_id AS uf_id,
                    SUM(gir.CropArea) AS total_crop_area
                FROM land_details ld
                INNER
                JOIN farmer_society fs ON fs.uf_id = ld.uf_id
                INNER
                JOIN mas_farmer mf ON mf.uf_id = fs.uf_id
                INNER
                JOIN mas_financialyear fy ON fy.id = ld.financial_year
                INNER
                JOIN (
                SELECT DISTINCT
                    gd.KhasraNo AS khasra_no,
                    gd.uf_id AS uf_id,
                    gd.CropArea,
                    gd.village_code
                FROM rgkny_2122.girdawari_2022 gd
                INNER
                JOIN rgkny_2122.mas_crop mc ON mc.crop_code = gd.SubCropCode
                INNER
                JOIN rgkny_2122.mas_crop_categorys mcc ON mcc.cropcode = gd.CropCode
                INNER
                JOIN mas_financialyear fy ON fy.financial_year = gd.CropYear
                WHERE fy.financial_year = gd.CropYear
                AND gd.village_code = @villageCode ) AS gir ON gir.uf_id = ld.uf_id
                AND gir.khasra_no = ld.khasra_no
                WHERE ld.village_code = @villageCode
                GROUP BY ld.uf_id ), f_detail AS(
                SELECT 
                    mf.uf_id,
                    CONCAT(REPEAT('*',8), mf.aadhar_lastfourdigit) AS mask_aadhar_number,
                    mf.aadhar_number,
                    mf.farmer_name_eng,
                    mf.farmer_name_hi,
                    mf.father_name,
                    fs.address,
                    CONCAT(REPEAT('*', CHAR_LENGTH(mf.mobile_no)-4), SUBSTR(mf.mobile_no,7,10)) AS mask_mobile_no,
                    mf.mobile_no,
                    fs.ifsc_code,
                    CONCAT(REPEAT('*', CHAR_LENGTH(fs.account_no)-4), SUBSTR(fs.account_no, CHAR_LENGTH(fs.account_no)-3, CHAR_LENGTH(fs.account_no))) AS mask_account_no,
                    fs.account_no,
                    fs.village_code
                FROM farmer_society fs
                INNER
                JOIN mas_farmer mf ON fs.uf_id= mf.uf_id
                WHERE mf.uf_id NOT IN(
                SELECT 
                    h.uf_id
                FROM horti_farmer_verify_status h
                WHERE h.uf_id IN(mf.uf_id))
                AND fs.village_code = @villageCode )
                SELECT 
                    f.uf_id AS UfId,
                    f.mask_aadhar_number AS MaskAadharNumber,
                    f.aadhar_number AS AadharNumber,
                    f.farmer_name_eng AS FarmerNameEng,
                    f.farmer_name_hi AS FarmerNameHi,
                    f.father_name AS FatherName,
                    f.address AS Address,
                    f.mask_mobile_no AS MaskMobileNo,
                    f.ifsc_code AS IfscCode,
                    f.mask_account_no AS MaskAccountNo,
                    f.account_no AS AccountNo,
                    c.total_crop_area AS TotalCropArea,
                    f.village_code AS VillageCode
                FROM f_detail f,crop c
                WHERE f.uf_id=c.uf_id
                ORDER BY f.farmer_name_eng;
                """;
                var result = await connection.QueryAsync<FarmersListByVillageForUFPVerificationDto>(sql, new { villageCode = VillageCode });
                return Result<List<FarmersListByVillageForUFPVerificationDto>>.Success(result.ToList());
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Failed to fetch farmers list by VillageCode {VillageCode}", VillageCode);
                return Result<List<FarmersListByVillageForUFPVerificationDto>>.Failure($"Failed to fetch farmers list by VillageCode {VillageCode}: {ex.Message}");
            }
        }
    }
}
