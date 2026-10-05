using Dapper;
using HortiBts.Api.Data;
using HortiBts.Shared.Common;
using HortiBts.Shared.Dtos.HPMIS;

namespace HortiBts.Api.Repositories.HPMIS
{
    public interface IFarmersDetailsHPMISRepository
    {
        /// <summary>Returns farmers basic details</summary>
        Task<Result<List<FarmerBasicDetailsHPMISDto>>> GetFarmersBasicDetailsAsync(string applicationId);
        Task<Result<List<FarmerApplicationDetailsHPMISDto>>> GetFarmersApplicationDetailsAsync(string applicationId);
        Task<Result<List<FarmerLandDetailsHPMISDto>>> GetFarmersLandDetailsAsync(string applicationId);
        Task<Result<List<FarmerCropDetailsHPMISDto>>> GetFarmersCropDetailsAsync(string applicationId);
        Task<Result<List<FarmerMarketLinkageDetailsHPMISDto>>> GetFarmersMarketLinkageDetailsAsync(string applicationId);
        Task<Result<List<FarmerSchemeDetailsHPMISDto>>> GetFarmersSchemeDetailsAsync(string applicationId);
    }
    public class FarmersDetailsHPMISRepository(IDbConnectionFactory dbFactory, ILogger<FarmersDetailsHPMISRepository> logger) : IFarmersDetailsHPMISRepository
    {
        public async Task<Result<List<FarmerBasicDetailsHPMISDto>>> GetFarmersBasicDetailsAsync(string applicationId)
        {
            try
            {
                using var connection = dbFactory.CreateConnection(HortiDb.Hpmis);
                const string sql = """
                SELECT 
                    f.f_id AS FId,
                    fa.application_id AS ApplicationId,
                    fa.created_at AS ApplicationDate,
                    f.uf_id AS UfId,
                    f.user_id AS UserId,
                    f.mobile_no AS MobileNo,
                    f.aadhar_number AS AadharNumber,
                    f.farmer_name_eng AS FarmerNameEng,
                    f.farmer_name_hi AS FarmerNameHi,
                    f.care_of_name AS CareOfName,
                    f.dob AS Dob,
                    f.relation AS Relation,
                    r.rel_name AS RelName,
                    f.category AS Category,
                    c.caste_name AS CasteName,
                    f.subcategory AS Subcategory,
                    sc.subcaste_name AS SubcasteName,
                    f.gender AS Gender,
                    g.gender_name_hi AS GenderNameHi,
                    f.mobile_no AS MobileNo,
                    f.mobile_no1 AS MobileNo1,
                    f.district_code AS DistrictCode,
                    d.district_name_hi AS DistrictNameHi,
                    f.subdistrict_code AS SubdistrictCode,
                    s.block_name_eng AS SubdistrictName,
                    f.village_code AS VillageCode,
                    v.villcdname AS VillageName,
                    f.house_no AS HouseNo,
                    f.ward_no AS WardNo,
                    f.address AS Address,
                    CONCAT_WS(', ' , f.house_no, f.ward_no, f.address, f.pin_code) final_address,
                    f.pin_code AS PinCode,
                    f.working_member AS WorkingMember,
                    f.family_income AS FamilyIncome,
                    f.data_source AS DataSource
                FROM farmer_application fa
                INNER
                JOIN farmer f ON f.f_id= fa.f_id
                INNER
                JOIN mas_gender g ON g.gender_id = f.gender
                INNER
                JOIN mas_relation r ON r.rel_id = f.relation
                INNER
                JOIN mas_district d ON d.district_census= f.district_code
                INNER
                JOIN mas_sub_district s ON s.subdistrict_code= f.subdistrict_code
                INNER
                JOIN mas_villages v ON v.vsr_census = f.village_code
                INNER
                JOIN mas_caste c ON c.caste_code = f.category
                INNER
                JOIN mas_caste sc ON sc.subcaste_code = f.subcategory
                WHERE fa.application_id =@ApplicationId
                """;
                var result = await connection.QuerySingleAsync<FarmerBasicDetailsHPMISDto>(sql, new { ApplicationId = applicationId });
                return Result<List<FarmerBasicDetailsHPMISDto>>.Success([result]);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Failed to fetch farmer basic details for ApplicationId {ApplicationId}", applicationId);
                return Result<List<FarmerBasicDetailsHPMISDto>>.Failure($"Failed to fetch farmer basic details: {ex.Message}");
            }
        }

        public async Task<Result<List<FarmerApplicationDetailsHPMISDto>>> GetFarmersApplicationDetailsAsync(string applicationId)
        {
            try
            {
                using var connection = dbFactory.CreateConnection(HortiDb.Hpmis);
                const string sql = """
                SELECT 
                    fa.application_id AS ApplicationId,
                    fa.f_id AS FId,
                    fa.mobile_number AS MobileNumber,
                    fa.district_code AS DistrictCode,
                    fa.subdistrict_code AS SubdistrictCode,
                    fa.village_code AS VillageCode,
                    fa.financial_year AS FinancialYear,
                    fa.is_land_entered AS IsLandEntered,
                    fa.is_crop_entered AS IsCropEntered,
                    fa.is_market_linkage_entered AS IsMarketLinkageEntered,
                    fa.is_scheme_entered AS IsSchemeEntered,
                    fa.created_at AS CreatedAt,
                    is_approved AS IsApproved,
                    remark AS Remark
                FROM farmer_application fa
                WHERE fa.application_id = @ApplicationId
                """;
                var result = await connection.QuerySingleAsync<FarmerApplicationDetailsHPMISDto>(sql, new { ApplicationId = applicationId });
                return Result<List<FarmerApplicationDetailsHPMISDto>>.Success([result]);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Failed to fetch farmer application details for ApplicationId {ApplicationId}", applicationId);
                return Result<List<FarmerApplicationDetailsHPMISDto>>.Failure($"Failed to fetch farmer application details: {ex.Message}");
            }
        }

        public async Task<Result<List<FarmerLandDetailsHPMISDto>>> GetFarmersLandDetailsAsync(string applicationId)
        {
            try
            {
                using var connection = dbFactory.CreateConnection(HortiDb.Hpmis);
                const string sql = """
                SELECT 
                    fa.application_id AS ApplicationId,
                    fa.is_land_entered AS IsLandEntered,
                    l.ld_id AS LdId,
                    l.f_id AS FId,
                    f.uf_id AS UfId,
                    f.user_id AS UserId,
                    f.mobile_no AS MobileNo,
                    l.village_code AS VillageCode,
                    v.villcdname AS VillageName,
                    l.district_code AS DistrictCode,
                    l.block_code AS SubdistrictCode,
                    l.owner_name AS OwnerName,
                    l.father_name AS FatherName,
                    l.patwari_halka AS PatwariHalka,
                    l.khasra_no AS KhasraNo,
                    l.basra_no AS BasraNo,
                    l.land_area AS LandArea,
                    l.owner_type AS OwnerType,
                    l.owner_type_code AS OwnerTypeCode,
                    l.village_type AS VillageType,
                    l.financial_year AS FinancialYear,
                    l.joint_khasra_id AS JointKhasraId,
                    l.joint_khasra_details AS JointKhasraDetails,
                    d.district_name_hi AS DistrictNameHi,
                    sd.block_name_hi AS SubdistrictNameHi
                FROM farmer_application fa
                INNER
                JOIN farmer f ON f.f_id= fa.f_id
                INNER
                JOIN land_details l ON fa.application_id= l.application_id
                INNER
                JOIN mas_villages v ON v.vsr_census=l.village_code
                INNER
                JOIN mas_district d ON d.district_census = v.distcode_census
                INNER
                JOIN mas_sub_district sd ON sd.subdistrict_code = v.subdistrictcode_census
                WHERE fa.application_id =@ApplicationId
                """;
                var result = await connection.QueryAsync<FarmerLandDetailsHPMISDto>(sql, new { ApplicationId = applicationId });
                return Result<List<FarmerLandDetailsHPMISDto>>.Success(result.ToList());
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Failed to fetch farmer land details for applicationId {ApplicationId}", applicationId);
                return Result<List<FarmerLandDetailsHPMISDto>>.Failure($"Failed to fetch farmer land details: {ex.Message}");
            }
        }

        public async Task<Result<List<FarmerCropDetailsHPMISDto>>> GetFarmersCropDetailsAsync(string applicationId)
        {
            try
            {
                using var connection = dbFactory.CreateConnection(HortiDb.Hpmis);
                const string sql = """
                SELECT 
                    fa.application_id AS ApplicationId,
                    c.cd_id AS CdId,
                    c.f_id AS FId,
                    f.uf_id AS UfId,
                    f.user_id AS UserId,
                    f.mobile_no AS MobileNo,
                    c.village_code AS VillageCode,
                    vi.villcdname  AS VillageName,
                    (
                SELECT 
                    group_concat(cd.khasra_no) khasra_no
                FROM crop_khasra_details cd
                WHERE cd.cd_id= c.cd_id
                GROUP BY cd.cd_id) khasra_no, c.crop_code, mc.crop_name_hi, c.crop_category, mcc.category_name_hi, c.crop_variety, v.variety_name, c.crop_season, c.crop_area, c.total_area, c.expected_production, c.actual_production, c.financial_year, y.financial_year AS financial_year_text
                FROM farmer_application fa
                INNER
                JOIN farmer f ON f.f_id= fa.f_id
                INNER
                JOIN crop_details c ON fa.application_id= c.application_id
                INNER
                JOIN mas_season s ON s.season_id = c.crop_season
                INNER
                JOIN mas_crop mc ON mc.crop_code = c.crop_code
                INNER
                JOIN mas_crop_category mcc ON mcc.category = c.crop_category
                INNER
                JOIN mas_variety v ON v.variety_code = c.crop_variety
                INNER
                JOIN mas_villages vi ON vi.vsr_census= c.village_code
                INNER
                JOIN mas_financialyear y ON y.id = c.financial_year
                WHERE fa.application_id =@ApplicationId
                """;
                var result = await connection.QueryAsync<FarmerCropDetailsHPMISDto>(sql, new { ApplicationId = applicationId });
                return Result<List<FarmerCropDetailsHPMISDto>>.Success(result.ToList());
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Failed to fetch farmer crop details for applicationId {ApplicationId}", applicationId);
                return Result<List<FarmerCropDetailsHPMISDto>>.Failure($"Failed to fetch farmer crop details: {ex.Message}");
            }
        }

        public async Task<Result<List<FarmerMarketLinkageDetailsHPMISDto>>> GetFarmersMarketLinkageDetailsAsync(string applicationId)
        {
            try
            {
                using var connection = dbFactory.CreateConnection(HortiDb.Hpmis);
                const string sql = """
                SELECT 
                    l.m_id AS MId,
                    f.f_id AS FId,
                    f.uf_id AS UfId,
                    f.user_id AS UserId,
                    f.mobile_no AS MobileNo,
                    l.market_link_type AS MarketLinkType,
                    ml.market_name_hi AS MarketNameHi,
                    l.buyer_name AS BuyerName,
                    l.financial_year AS FinancialYear,
                    l.mandi_code AS MandiCode,
                    m.mandi_name_hi AS MandiNameHi,
                    mc.crop_name_hi AS CropNameHi,
                    mcc.category_name_hi AS CategoryNameHi,
                    v.variety_name AS VarietyName,
                    CASE l.market_link_type WHEN 1 THEN l.buyer_name WHEN 2 THEN CONCAT_WS(', ', m.mandi_name_hi, 'मंडी', d.district_name_hi, s.state_name_hi) WHEN 3 THEN CONCAT_WS(', ', l.buyer_name, l.address,mt.Tehsil_Name, d.district_name_hi, s.state_name_hi) WHEN 4 THEN CONCAT_WS(', ', l.buyer_name, l.address, s.state_name_hi) WHEN 5 THEN CONCAT_WS(', ', l.buyer_name, l.address) ELSE "Self" END AS address,
                    l.district_code AS DistrictCode,
                    l.tehsil_code AS TehsilCode,
                    l.state_code AS StateCode,
                    l.country_code AS CountryCode
                FROM farmer_application fa
                INNER
                JOIN farmer f ON f.f_id= fa.f_id
                INNER
                JOIN market_linkages l ON l.application_id = fa.application_id
                INNER
                JOIN crop_details cd ON cd.cd_id = l.cd_id
                INNER
                JOIN mas_crop mc ON mc.crop_code = cd.crop_code
                INNER
                JOIN mas_crop_category mcc ON mcc.category = cd.crop_category
                INNER
                JOIN mas_variety v ON v.variety_code = cd.crop_variety
                INNER
                JOIN mas_market_link_type ml ON ml.market_type_id = l.market_link_type
                LEFT
                JOIN mas_mandi m ON m.mandi_id = l.mandi_code
                LEFT
                JOIN mas_district d ON d.district_census = l.district_code
                LEFT
                JOIN mas_tehsil mt ON mt.CensusCode = l.tehsil_code
                LEFT
                JOIN mas_state s ON s.state_census = l.state_code
                WHERE fa.application_id =@ApplicationId
                """;
                var result = await connection.QueryAsync<FarmerMarketLinkageDetailsHPMISDto>(sql, new { ApplicationId = applicationId });
                return Result<List<FarmerMarketLinkageDetailsHPMISDto>>.Success(result.ToList());
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Failed to fetch farmer market linkage details for applicationId {ApplicationId}", applicationId);
                return Result<List<FarmerMarketLinkageDetailsHPMISDto>>.Failure($"Failed to fetch farmer market linkage details: {ex.Message}");
            }
        }

        public async Task<Result<List<FarmerSchemeDetailsHPMISDto>>> GetFarmersSchemeDetailsAsync(string applicationId)
        {
            try
            {
                using var connection = dbFactory.CreateConnection(HortiDb.Hpmis);
                const string sql = """
                SELECT 
                    sd.s_id AS SId,
                    sd.f_id AS FId,
                    sd.scheme_id AS SchemeId,
                    sd.scheme_type_id AS SchemeTypeId,
                    sd.component_id AS ComponentId,
                    sd.financial_year AS FinancialYear,
                    s.scheme_name AS SchemeName,
                    st.scheme_name AS SchemeTypeName,
                    c.cname AS ComponentName
                FROM scheme_details sd
                INNER
                JOIN farmer f ON f.f_id = sd.f_id
                INNER
                JOIN mas_scheme_horti st ON st.s_id = sd.scheme_id
                INNER
                JOIN mas_scheme_horti s ON s.s_id = sd.scheme_id
                INNER
                JOIN mas_component_horti c ON c.c_id = sd.component_id
                INNER
                JOIN farmer_application fa ON fa.f_id = f.f_id
                WHERE fa.application_id =@ApplicationId
                """;
                var result = await connection.QueryAsync<FarmerSchemeDetailsHPMISDto>(sql, new { ApplicationId = applicationId });
                return Result<List<FarmerSchemeDetailsHPMISDto>>.Success(result.ToList());
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Failed to fetch farmer scheme details for applicationId {ApplicationId}", applicationId);
                return Result<List<FarmerSchemeDetailsHPMISDto>>.Failure($"Failed to fetch farmer scheme details: {ex.Message}");
            }
        }
    }
}
