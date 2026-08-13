using Dapper;
using HortiBts.Api.Data;
using HortiBts.Shared.Common;
using HortiBts.Shared.Dtos.Districts;
using HortiBts.Shared.Dtos.Farmers;

namespace HortiBts.Api.Repositories.Farmers
{
    public interface IFarmersDetailsRepository
    {
        /// <summary>Returns farmers basic details</summary>
        Task<Result<List<FarmerBasicDetailsDto>>> GetFarmersBasicDetailsAsync(string UFID);
        Task<Result<List<FarmerAddressDetailsDto>>> GetFarmersAddressDetailsAsync(string UFID);
        Task<Result<List<FarmerBankDetailsDto>>> GetFarmersBankDetailsAsync(string UFID);
        Task<Result<List<FarmerLandDetailsDto>>> GetFarmersLandDetailsAsync(string UFID, string FinYear);
        Task<Result<List<FarmerSchemeDetailsDto>>> GetFarmersSchemeDetailsAsync(string UFID, string FinYear);
        Task<Result<List<FarmerCropDetailsDto>>> GetFarmersCropDetailsAsync(string UFID, string FinYear);
    }
    public class FarmersDetailsRepository(IDbConnectionFactory dbFactory, ILogger<FarmersDetailsRepository> logger) : IFarmersDetailsRepository
    {
        public async Task<Result<List<FarmerBasicDetailsDto>>> GetFarmersBasicDetailsAsync(string UFID)
        {
            try
            {
                using var connection = dbFactory.CreateConnection(HortiDb.Bts);
                const string sql = """
                SELECT 
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
                    mf.data_source AS DataSource
                FROM mas_farmer_horti mf
                INNER
                JOIN mas_caste mc ON mc.caste_code=mf.category
                AND mc.subcaste_code=mf.subcategory
                WHERE mf.uf_id=@UFID
                """;
                var result = await connection.QuerySingleAsync<FarmerBasicDetailsDto>(sql, new { Ufid = UFID });
                return Result<List<FarmerBasicDetailsDto>>.Success([result]);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Failed to fetch farmer basic details for UFID {UFID}", UFID);
                return Result<List<FarmerBasicDetailsDto>>.Failure($"Failed to fetch farmer basic details: {ex.Message}");
            }
        }

        public async Task<Result<List<FarmerAddressDetailsDto>>> GetFarmersAddressDetailsAsync(string UFID)
        {
            try
            {
                using var connection = dbFactory.CreateConnection(HortiDb.Bts);
                const string sql = """
                SELECT 
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
                FROM farmer_detail_horti fd
                INNER
                JOIN view_all_villages v ON v.village_code=fd.village_code
                WHERE fd.uf_id=@UFID
                """;
                var result = await connection.QuerySingleAsync<FarmerAddressDetailsDto>(sql, new { Ufid = UFID });
                return Result<List<FarmerAddressDetailsDto>>.Success([result]);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Failed to fetch farmer address details for UFID {UFID}", UFID);
                return Result<List<FarmerAddressDetailsDto>>.Failure($"Failed to fetch farmer address details: {ex.Message}");
            }
        }

        public async Task<Result<List<FarmerBankDetailsDto>>> GetFarmersBankDetailsAsync(string UFID)
        {
            try
            {
                using var connection = dbFactory.CreateConnection(HortiDb.Bts);
                const string sql = """
                SELECT 
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
                FROM bank_detail_horti bd
                INNER
                JOIN mas_bank mb ON mb.bank_code=bd.bank_code
                LEFT
                JOIN mas_bankbranch mbb ON mbb.branch_code=bd.branch_code
                LEFT
                JOIN rev_district v ON v.DistrictCensus=bd.bank_district
                WHERE bd.uf_id=@UFID
                """;
                var result = await connection.QuerySingleAsync<FarmerBankDetailsDto>(sql, new { Ufid = UFID });
                return Result<List<FarmerBankDetailsDto>>.Success([result]);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Failed to fetch farmer bank details for UFID {UFID}", UFID);
                return Result<List<FarmerBankDetailsDto>>.Failure($"Failed to fetch farmer bank details: {ex.Message}");
            }
        }

        public async Task<Result<List<FarmerLandDetailsDto>>> GetFarmersLandDetailsAsync(string UFID, string FinYear)
        {
            try
            {
                using var connection = dbFactory.CreateConnection(HortiDb.Bts);
                const string sql = """
                SELECT 
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
                    ld.type AS Type,
                    v.village_name AS VillageName,
                    v.DistCodeCensus AS DistCodeCensus,
                    v.district_id AS DistrictId,
                    v.DistrictName AS DistrictName,
                    v.subdistrict_code AS SubdistrictCode,
                    v.subdistrict_name AS SubdistrictName,
                    ld.financial_year AS FinancialYear,
                    y.financial_year AS FinancialYearStr
                FROM land_details_horti ld
                INNER
                JOIN view_all_villages v ON v.village_code=ld.village_code
                INNER
                JOIN mas_financialyear y ON y.id=ld.financial_year
                WHERE ld.uf_id=@UFID
                AND ld.financial_year=@FinYear
                """;
                var result = await connection.QueryAsync<FarmerLandDetailsDto>(sql, new { Ufid = UFID, finYear = FinYear });
                return Result<List<FarmerLandDetailsDto>>.Success(result.ToList());
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Failed to fetch farmer land details for UFID {UFID} and financial year {FinYear}", UFID, FinYear);
                return Result<List<FarmerLandDetailsDto>>.Failure($"Failed to fetch farmer land details: {ex.Message}");
            }
        }

        public async Task<Result<List<FarmerSchemeDetailsDto>>> GetFarmersSchemeDetailsAsync(string UFID, string FinYear)
        {
            try
            {
                using var connection = dbFactory.CreateConnection(HortiDb.Bts);
                const string sql = """
                SELECT 
                    e.uf_id AS UfId,
                    e.hf_id AS HfId,
                    e.fd_id AS FdId,
                    e.equipment_id AS EquipmentId,
                    e.village_code AS VillageCode,
                    e.scheme_type AS SchemeType,
                    ms.scheme_name AS SchemeName,
                    e.schcme_id AS SchcmeId,
                    e.component_id AS ComponentId,
                    mc.cname AS Cname,
                    e.benefit_type AS BenefitType,
                    mbt.benefit_name_en AS BenefitTypeName,
                    e.benefit AS Benefit,
                    mb.benefit_name_hi AS BenefitName,
                    e.unit AS Unit,
                    mu.unit_name AS UnitName,
                    e.quantity AS Quantity,
                    e.khasra_no AS KhasraNo,
                    e.area AS Area,
                    e.cash AS Cash,
                    e.financial_year AS FinancialYear,
                    v.village_name AS VillageName,
                    v.DistrictName AS DistrictName,
                    v.subdistrict_name AS SubdistrictName,
                    y.financial_year AS FinancialYearStr
                FROM equipment_details_govt_schemes e
                INNER
                JOIN mas_scheme_horti ms ON ms.s_id=e.schcme_id
                INNER
                JOIN mas_component_horti mc ON mc.c_id=e.component_id
                INNER
                JOIN mas_benefit_type mbt ON mbt.benefit_type_id=e.benefit_type
                INNER
                JOIN mas_benefit mb ON mb.benefit_id=e.benefit
                INNER
                JOIN mas_units mu ON mu.unit_id=e.unit
                INNER
                JOIN view_all_villages v ON v.village_code=e.village_code
                INNER
                JOIN mas_financialyear y ON y.id=e.financial_year
                WHERE e.uf_id=@UFID
                AND e.financial_year=@FinYear
                """;
                var result = await connection.QueryAsync<FarmerSchemeDetailsDto>(sql, new { Ufid = UFID, finYear = FinYear });
                return Result<List<FarmerSchemeDetailsDto>>.Success(result.ToList());
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Failed to fetch farmer scheme details for UFID {UFID} and financial year {FinYear}", UFID, FinYear);
                return Result<List<FarmerSchemeDetailsDto>>.Failure($"Failed to fetch farmer scheme details: {ex.Message}");
            }
        }

        public async Task<Result<List<FarmerCropDetailsDto>>> GetFarmersCropDetailsAsync(string UFID, string FinYear)
        {
            try
            {
                using var connection = dbFactory.CreateConnection(HortiDb.Bts);
                const string sql = """
                SELECT 
                    cd.uf_id AS UfId,
                    cd.hf_id AS HfId,
                    cd.fd_id AS FdId,
                    cd.cd_id AS CdId,
                    cd.village_code AS VillageCode,
                    cd.khasra_no AS KhasraNo,
                    cd.crop_code AS CropCode,
                    cd.crop_category AS CropCategory,
                    cd.ccrop_veriety AS CcropVariety,
                    cd.crop_season AS CropSeason,
                    (CASE WHEN cd.crop_season=1 THEN 'खरीफ़' WHEN cd.crop_season=2 THEN 'रबी' WHEN cd.crop_season=3 THEN 'ज़ायद'END ) crop_season_name,
                    cd.ccrop_area AS CcropArea,
                    cd.type AS Type,
                    cd.crop_status AS CropStatus,
                    mc.crop_name AS CropName,
                    mcc.subcrop AS CropCategoryName,
                    v.village_name AS VillageName,
                    v.DistrictName AS DistrictName,
                    v.subdistrict_name AS SubdistrictName,
                    cd.financial_year AS FinancialYear,
                    y.financial_year AS FinancialYearStr
                FROM crop_details_horti cd
                INNER
                JOIN mas_crop mc ON mc.crop_code=cd.crop_code
                INNER
                JOIN mas_crop_categorys mcc ON mcc.id=cd.crop_category
                INNER
                JOIN view_all_villages v ON v.village_code=cd.village_code
                INNER
                JOIN mas_financialyear y ON y.id=cd.financial_year
                WHERE cd.uf_id=@UFID
                AND cd.financial_year=@FinYear
                """;
                var result = await connection.QueryAsync<FarmerCropDetailsDto>(sql, new { Ufid = UFID, finYear = FinYear });
                return Result<List<FarmerCropDetailsDto>>.Success(result.ToList());
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Failed to fetch farmer crop details for UFID {UFID} and financial year {FinYear}", UFID, FinYear);
                return Result<List<FarmerCropDetailsDto>>.Failure($"Failed to fetch farmer crop details: {ex.Message}");
            }
        }
    }
}
