using Dapper;
using HortiBts.Api.Data;
using HortiBts.Shared.Common;
using HortiBts.Shared.Dtos.ApplicationVerification.FarmersApplication;

namespace HortiBts.Api.Repositories.ApplicationVerification.DDH.NewApplication
{
    public interface IFarmersDetailsByApplicationIdRepository
    {
        /// <summary>Returns farmers basic details</summary>
        Task<Result<List<FarmerApplicationBasicDetailsDto>>> GetFarmersBasicDetailsByApplicationIdAsync(int applicationId);
        Task<Result<List<FarmerApplicationAddressDetailsDto>>> GetFarmersAddressDetailsByApplicationIdAsync(int applicationId);
        Task<Result<List<FarmerApplicationBankDetailsDto>>> GetFarmersBankDetailsByApplicationIdAsync(int applicationId);
        Task<Result<List<FarmerApplicationLandDetailsDto>>> GetFarmersLandDetailsByApplicationIdAsync(int applicationId, int financialYear);
        Task<Result<List<FarmerApplicationSchemeDetailsDto>>> GetFarmersSchemeDetailsByApplicationIdAsync(int applicationId, int financialYear);
        Task<Result<List<FarmerApplicationCropDetailsDto>>> GetFarmersCropDetailsByApplicationIdAsync(int applicationId, int financialYear);
    }
    public class FarmersDetailsByApplicationIdRepository(IDbConnectionFactory dbFactory, ILogger<FarmersDetailsByApplicationIdRepository> logger) : IFarmersDetailsByApplicationIdRepository
    {
        public async Task<Result<List<FarmerApplicationBasicDetailsDto>>> GetFarmersBasicDetailsByApplicationIdAsync(int applicationId)
        {
            try
            {
                using var connection = dbFactory.CreateConnection(HortiDb.Bts);
                const string sql = """
                SELECT 
                    fa.application_id AS ApplicationId,
                    mf.uf_id AS UfId,
                    mf.unique_id AS UniqueId,
                    mf.farmer_name_eng AS FarmerNameEng,
                    mf.farmer_name_hi AS FarmerNameHi,
                    mf.father_name AS FatherName,
                    mf.relation AS Relation,
                    mf.dob AS Dob,
                    mf.category AS Category,
                    mc.caste_name AS CasteName,
                    mf.subcategory AS Subcategory,
                    mc.subcaste_name AS SubcasteName,
                    mf.gender AS Gender,
                    mf.mobile_no AS MobileNo,
                    mf.mobile_no1 AS MobileNo1,
                    mf.data_source AS DataSource,
                    fa.created_at AS application_date
                FROM temp_farmer_application fa
                INNER
                JOIN temp_mas_farmer mf ON mf.uf_id=fa.uf_id
                LEFT
                JOIN mas_caste mc ON mc.caste_code=mf.category
                AND mc.subcaste_code=mf.subcategory
                WHERE fa.application_id=@ApplicationId
                """;
                var result = await connection.QuerySingleAsync<FarmerApplicationBasicDetailsDto>(sql, new { ApplicationId = applicationId });
                return Result<List<FarmerApplicationBasicDetailsDto>>.Success([result]);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Failed to fetch farmer basic details for applicationId {applicationId}", applicationId);
                return Result<List<FarmerApplicationBasicDetailsDto>>.Failure($"Failed to fetch farmer basic details: {ex.Message}");
            }
        }

        public async Task<Result<List<FarmerApplicationAddressDetailsDto>>> GetFarmersAddressDetailsByApplicationIdAsync(int applicationId)
        {
            try
            {
                using var connection = dbFactory.CreateConnection(HortiDb.Bts);
                const string sql = """
                SELECT 
                    fa.application_id AS ApplicationId,
                    fd.uf_id AS UfId,
                    fd.hf_id AS HfId,
                    fd.fd_id AS FdId,
                    fd.house_no AS HouseNo,
                    fd.ward_no AS WardNo,
                    fd.address AS Address,
                    fd.pincode AS Pincode,
                    fd.village_code AS VillageCode,
                    v.village_name AS VillageName,
                    v.DistCodeCensus AS DistCodeCensus,
                    v.district_id AS DistrictId,
                    v.DistrictName AS DistrictName,
                    v.subdistrict_code AS SubdistrictCode,
                    v.subdistrict_name AS SubdistrictName,
                    fd.financial_year AS FinancialYear,
                    fd.data_source AS DataSource
                FROM temp_farmer_application fa
                INNER
                JOIN temp_farmer_details fd ON fd.fd_id=fa.fd_id
                INNER
                JOIN view_all_villages v ON v.village_code=fd.village_code
                WHERE fa.application_id=@ApplicationId
                """;
                var result = await connection.QuerySingleAsync<FarmerApplicationAddressDetailsDto>(sql, new { ApplicationId = applicationId });
                return Result<List<FarmerApplicationAddressDetailsDto>>.Success([result]);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Failed to fetch farmer address details for applicationId {applicationId}", applicationId);
                return Result<List<FarmerApplicationAddressDetailsDto>>.Failure($"Failed to fetch farmer address details: {ex.Message}");
            }
        }

        public async Task<Result<List<FarmerApplicationBankDetailsDto>>> GetFarmersBankDetailsByApplicationIdAsync(int applicationId)
        {
            try
            {
                using var connection = dbFactory.CreateConnection(HortiDb.Bts);
                const string sql = """
                SELECT 
                    fa.application_id AS ApplicationId,
                    bd.uf_id AS UfId,
                    bd.hf_id AS HfId,
                    bd.fd_id AS FdId,
                    bd.bd_id AS BdId,
                    bd.bank_code AS BankCode,
                    mb.bank_name AS BankName,
                    bd.bank_state AS BankState,
                    if(bd.bank_state=22,'छत्तीसगढ़','अन्य राज्य') bank_state_name,
                    bd.bank_district AS BankDistrict,
                    v.DistrictName AS DistrictName,
                    bd.pfms_flag AS PfmsFlag,
                    bd.account_no AS AccountNo,
                    bd.ifsc_code AS IfscCode,
                    bd.branch_code AS BranchCode,
                    mbb.branch_name AS BranchName,
                    bd.data_source AS DataSource,
                    bd.financial_year AS FinancialYear
                FROM temp_farmer_application fa
                INNER
                JOIN temp_bank_details bd ON bd.bd_id= fa.bd_id
                INNER
                JOIN mas_bank mb ON mb.bank_code=bd.bank_code
                LEFT
                JOIN mas_bankbranch mbb ON mbb.branch_code=bd.branch_code
                LEFT
                JOIN rev_district v ON v.DistrictCensus=bd.bank_district
                WHERE fa.application_id=@ApplicationId
                """;
                var result = await connection.QuerySingleAsync<FarmerApplicationBankDetailsDto>(sql, new { ApplicationId = applicationId });
                return Result<List<FarmerApplicationBankDetailsDto>>.Success([result]);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Failed to fetch farmer bank details for applicationId {applicationId}", applicationId);
                return Result<List<FarmerApplicationBankDetailsDto>>.Failure($"Failed to fetch farmer bank details: {ex.Message}");
            }
        }

        public async Task<Result<List<FarmerApplicationLandDetailsDto>>> GetFarmersLandDetailsByApplicationIdAsync(int applicationId, int financialYear)
        {
            try
            {
                using var connection = dbFactory.CreateConnection(HortiDb.Bts);
                const string sql = """
                SELECT 
                    ld.application_id AS ApplicationId,
                    ld.uf_id AS UfId,
                    ld.hf_id AS HfId,
                    ld.fd_id AS FdId,
                    ld.ld_id AS LdId,
                    ld.village_code AS VillageCode,
                    ld.OwnerName AS OwnerName,
                    ld.ownertype AS Ownertype,
                    ld.FatherName AS FatherName,
                    ld.patwari_halka AS PatwariHalka,
                    ld.khasra_no AS KhasraNo,
                    ld.area AS Area,
                    v.village_name AS VillageName,
                    if(ld.is_joint_khasra=1, 'हाँ', 'नहीं') is_joint_khasra,
                    v.DistCodeCensus AS DistCodeCensus,
                    v.district_id AS DistrictId,
                    v.DistrictName AS DistrictName,
                    v.subdistrict_code AS SubdistrictCode,
                    v.subdistrict_name AS SubdistrictName,
                    ld.financial_year AS FinancialYear,
                    y.financial_year AS FinancialYearStr
                FROM temp_land_details ld
                INNER
                JOIN view_all_villages v ON v.village_code=ld.village_code
                INNER
                JOIN mas_financialyear y ON y.id=ld.financial_year
                WHERE ld.financial_year=@FinancialYear
                AND ld.application_id=@ApplicationId
                AND ld.khasra_no IN(
                SELECT 
                    sk.khasra_no
                FROM temp_scheme_details s
                INNER
                JOIN temp_scheme_khasra_details sk ON s.sd_id= sk.sd_id
                WHERE s.application_id=@ApplicationId
                UNION
                SELECT 
                    cd.khasra_no
                FROM temp_crop_details cd
                WHERE cd.application_id=@ApplicationId)
                """;
                var result = await connection.QueryAsync<FarmerApplicationLandDetailsDto>(sql, new { ApplicationId = applicationId, FinancialYear = financialYear });
                return Result<List<FarmerApplicationLandDetailsDto>>.Success(result.ToList());
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Failed to fetch farmer land details for applicationId {applicationId} and financial year {FinancialYear}", applicationId, financialYear);
                return Result<List<FarmerApplicationLandDetailsDto>>.Failure($"Failed to fetch farmer land details: {ex.Message}");
            }
        }

        public async Task<Result<List<FarmerApplicationSchemeDetailsDto>>> GetFarmersSchemeDetailsByApplicationIdAsync(int applicationId, int financialYear)
        {
            try
            {
                using var connection = dbFactory.CreateConnection(HortiDb.Bts);
                const string sql = """
                SELECT 
                    e.application_id AS ApplicationId,
                    e.uf_id AS UfId,
                    e.hf_id AS HfId,
                    e.fd_id AS FdId,
                    e.sd_id AS SdId,
                    e.village_code AS VillageCode,
                    e.scheme_type AS SchemeType,
                    mst.scheme_name AS SchemeTypeName,
                    ms.scheme_name AS SchemeName,
                    e.scheme_id AS SchemeId,
                    e.component_id AS ComponentId,
                    mc.cname AS Cname,
                    e.benefit AS Benefit,
                    e.quantity AS Quantity,
                    e.khasra_no AS KhasraNo,
                    e.area AS Area,
                    e.financial_year AS FinancialYear,
                    v.village_name AS VillageName,
                    v.DistrictName AS DistrictName,
                    v.subdistrict_name AS SubdistrictName,
                    y.financial_year AS FinancialYearStr
                FROM temp_scheme_details e
                INNER
                JOIN mas_scheme_horti ms ON ms.s_id=e.scheme_id
                INNER
                JOIN mas_scheme_horti mst ON mst.s_id=e.scheme_type
                INNER
                JOIN mas_component_horti mc ON mc.c_id=e.component_id
                INNER
                JOIN view_all_villages v ON v.village_code=e.village_code
                INNER
                JOIN mas_financialyear y ON y.id=e.financial_year
                WHERE e.application_id=@ApplicationId
                AND e.financial_year=@FinancialYear
                """;
                var result = await connection.QueryAsync<FarmerApplicationSchemeDetailsDto>(sql, new { ApplicationId = applicationId, FinancialYear = financialYear });
                return Result<List<FarmerApplicationSchemeDetailsDto>>.Success(result.ToList());
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Failed to fetch farmer scheme details for applicationId {applicationId} and financial year {FinancialYear}", applicationId, financialYear);
                return Result<List<FarmerApplicationSchemeDetailsDto>>.Failure($"Failed to fetch farmer scheme details: {ex.Message}");
            }
        }

        public async Task<Result<List<FarmerApplicationCropDetailsDto>>> GetFarmersCropDetailsByApplicationIdAsync(int applicationId, int financialYear)
        {
            try
            {
                using var connection = dbFactory.CreateConnection(HortiDb.Bts);
                const string sql = """
                SELECT 
                    cd.application_id AS ApplicationId,
                    cd.uf_id AS UfId,
                    cd.hf_id AS HfId,
                    cd.fd_id AS FdId,
                    cd.cd_id AS CdId,
                    cd.village_code AS VillageCode,
                    cd.khasra_no AS KhasraNo,
                    cd.crop_code AS CropCode,
                    cd.crop_category AS CropCategory,
                    cd.ccrop_veriety AS CropVariety,
                    cd.crop_season AS CropSeason,
                    (CASE WHEN cd.crop_season=1 THEN 'खरीफ़' WHEN cd.crop_season=2 THEN 'रबी' WHEN cd.crop_season=3 THEN 'ज़ायद'END ) crop_season_name,
                    cd.ccrop_area AS CropArea,
                    cd.financial_year AS FinancialYear,
                    mc.crop_name AS CropName,
                    mcc.subcrop  AS SubCropCategoryName,
                    v.village_name AS VillageName,
                    v.DistrictName AS DistrictName,
                    v.subdistrict_name AS SubdistrictName,
                    cd.financial_year AS FinancialYear,
                    y.financial_year AS FinancialYearStr
                FROM temp_crop_details cd
                INNER
                JOIN temp_farmer_application a ON a.application_id = cd.application_id
                INNER
                JOIN mas_crop mc ON mc.crop_code=cd.crop_code
                INNER
                JOIN mas_crop_categorys mcc ON mcc.id=cd.crop_category
                INNER
                JOIN view_all_villages v ON v.village_code=cd.village_code
                INNER
                JOIN mas_financialyear y ON y.id=cd.financial_year
                WHERE cd.application_id= @ApplicationId
                AND cd.financial_year= @FinancialYear
                """;
                var result = await connection.QueryAsync<FarmerApplicationCropDetailsDto>(sql, new { ApplicationId = applicationId, FinancialYear = financialYear });
                return Result<List<FarmerApplicationCropDetailsDto>>.Success(result.ToList());
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Failed to fetch farmer crop details for applicationId {applicationId} and financial year {FinancialYear}", applicationId, financialYear);
                return Result<List<FarmerApplicationCropDetailsDto>>.Failure($"Failed to fetch farmer crop details: {ex.Message}");
            }
        }
    }
}
