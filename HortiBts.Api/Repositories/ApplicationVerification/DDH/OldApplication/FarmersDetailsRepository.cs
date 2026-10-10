using Dapper;
using HortiBts.Api.Data;
using HortiBts.Shared.Common;
using HortiBts.Shared.Dtos.ApplicationVerification.OldFarmersApplication;

namespace HortiBts.Api.Repositories.ApplicationVerification.DDH.OldApplication
{
    public interface IFarmersDetailsRepository
    {
        /// <summary>Returns farmers basic details</summary>
        Task<Result<List<OldFarmerApplicationBasicDetailsDto>>> GetFarmersBasicDetailsByHfIdAsync(int hfId);
        Task<Result<List<OldFarmerApplicationAddressDetailsDto>>> GetFarmersAddressDetailsByFdIdAsync(int fdId);
        Task<Result<List<OldFarmerApplicationBankDetailsDto>>> GetFarmersBankDetailsByFdIdAsync(int fdId);
        Task<Result<List<OldFarmerApplicationLandDetailsDto>>> GetFarmersLandDetailsByFdIdAsync(int fdId, string villageType, int financialYear);
        Task<Result<List<OldFarmerApplicationSchemeDetailsDto>>> GetFarmersSchemeDetailsByFdIdAsync(int fdId, int sdId, int financialYear);
        Task<Result<List<OldFarmerApplicationCropDetailsDto>>> GetFarmersCropDetailsByFdIdAsync(int fdId, int financialYear);
        Task<Result<List<OldFarmerApplicationMachineryDetailsDto>>> GetFarmersMachineryDetailsByFdIdAsync(int fdId, int financialYear);
    }
    public class FarmersDetailsRepository(IDbConnectionFactory dbFactory) : IFarmersDetailsRepository
    {
        public async Task<Result<List<OldFarmerApplicationBasicDetailsDto>>> GetFarmersBasicDetailsByHfIdAsync(int hfId)
        {
            try
            {
                using var connection = dbFactory.CreateConnection(HortiDb.Bts);
                const string sql = """
                SELECT 
                    a.membership_no AS MembershipNo,
                    b.aadhar_number AS AadharNumber,
                    b.hf_id AS HfId,
                    b.uf_id AS UfId,
                    b.po_farmercode AS PoFarmercode,
                    b.farmer_name_eng AS FarmerNameEng,
                    b.farmer_name_hi AS FarmerNameHi,
                    b.relation AS Relation,
                    b.father_name AS FatherName,
                    b.dob AS Dob,
                    b.category AS Category,
                    b.subcategory AS Subcategory,
                    b.gender AS Gender,
                    b.mobile_no AS MobileNo,
                    b.data_source AS DataSource
                FROM mas_farmer_horti b
                LEFT
                JOIN farmer_detail_horti a ON b.hf_id = a.hf_id
                WHERE b.hf_id= @HfId
                ORDER BY membership_no DESC LIMIT 1
                """;
                var result = await connection.QuerySingleAsync<OldFarmerApplicationBasicDetailsDto>(sql, new { HfId = hfId });
                return Result<List<OldFarmerApplicationBasicDetailsDto>>.Success([result]);
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
                using var connection = dbFactory.CreateConnection(HortiDb.Bts);
                const string sql = """
                SELECT 
                    a.hf_id AS HfId,
                    a.fd_id AS FdId,
                    a.fs_id AS FsId,
                    a.uf_id AS UfId,
                    a.address AS Address,
                    a.house_no AS HouseNo,
                    a.ward_no AS WardNo,
                    a.pincode AS Pincode,
                    a.membership_no AS MembershipNo,
                    a.financial_year AS FinancialYear,
                    a.village_code AS VillageCode,
                    CONCAT(rv.village_name,'-',rv.village_name_hi) AS village_name,
                    rv.SubDistrictCodeCensus  AS SubdistrictCode,
                    rb.BlockNameEng AS subdistrict_name,
                    rv.DistCodeCensus AS district_code,
                    rv.DistrictName AS district_name
                FROM app_farmer_details a
                INNER
                JOIN rev_villages rv ON rv.village_code=a.village_code
                INNER
                JOIN rev_block rb ON rb.subdistrict_code =rv.SubDistrictCodeCensus
                WHERE a.fd_id = @FdId
                ORDER BY rv.village_name ASC
                """;
                var result = await connection.QuerySingleAsync<OldFarmerApplicationAddressDetailsDto>(sql, new { FdId = fdId });
                return Result<List<OldFarmerApplicationAddressDetailsDto>>.Success([result]);
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
                using var connection = dbFactory.CreateConnection(HortiDb.Bts);
                const string sql = """
                SELECT 
                    a.hf_id AS HfId,
                    a.fd_id AS FdId,
                    a.fs_id AS FsId,
                    a.uf_id AS UfId,
                    bd.bank_state AS BankState,
                    bd.bank_district AS BankDistrict,
                    bd.bank_code AS BankCode,
                    mb.bank_name AS BankName,
                    mb.branch_name AS BranchName,
                    bd.branch_code AS BranchCode,
                    bd.ifsc_code AS IfscCode,
                    bd.account_no AS AccountNo,
                    bd.bd_id AS BdId,
                    a.financial_year AS FinancialYear
                FROM app_bank_details bd
                INNER
                JOIN app_farmer_details a ON bd.fd_id=a.fd_id
                INNER
                JOIN app_mas_farmer b ON b.hf_id = a.hf_id
                LEFT
                JOIN mas_bankbranch mb ON mb.branch_code = bd.branch_code
                WHERE a.fd_id = @FdId
                """;
                var result = await connection.QuerySingleAsync<OldFarmerApplicationBankDetailsDto>(sql, new { FdId = fdId });
                return Result<List<OldFarmerApplicationBankDetailsDto>>.Success([result]);
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
                using var connection = dbFactory.CreateConnection(HortiDb.Bts);
                const string sql = """
                SELECT 
                    ld.village_type AS VillageType,
                    ld.ld_id AS LdId,
                    ld.hf_id AS HfId,
                    ld.uf_id AS UfId,
                    mf.farmer_name_eng AS FarmerNameEng,
                    mv.village_code AS VillageCode,
                    mv.VLocation AS vlocationcode,
                    mv.VSrno AS Vsrno,
                    mv.village_name AS village_name,
                    ld.booklet_no AS BookletNo,
                    CASE WHEN ld.sinchit_asinchit=1 THEN 'सिंचित' WHEN ld.sinchit_asinchit=2 THEN 'असिंचित' END AS sinchit_asinchit,
                    ms.sichai_name AS SichaiName,
                    ld.patwari_halka AS PatwariHalka,
                    ld.khasra_no AS KhasraNo,
                    ld.area AS Area,
                    is_verified AS verify,
                    CASE WHEN ld.is_verified = 1 THEN 'भू-अभिलेख से सत्यापित' ELSE 'अभी तक सत्यापित नहीं हुआ है' END AS is_verified,
                    ld.serarch_bykhasra_no AS SerarchBykhasraNo,
                    ld.ownertype AS Ownertype,
                    ld.bhuiyanlastupdatedon AS Bhuiyanlastupdatedon
                FROM app_land_details ld
                INNER
                JOIN app_mas_farmer mf ON mf.hf_id=ld.hf_id
                LEFT
                JOIN rev_villages mv ON mv.village_code=ld.village_code
                OR mv.VSrno=ld.vsr_no
                LEFT
                JOIN mas_sichai ms ON ms.sichai_id=ld.sichai_id
                WHERE ld.fd_id= @FdId
                AND ld.village_type= @VillageType
                AND ld.financial_year= @Financialyear
                ORDER BY ld.hf_id
                """;
                var result = await connection.QueryAsync<OldFarmerApplicationLandDetailsDto>(sql, new { FdId = fdId, VillageType = villageType, FinancialYear = financialYear });
                return Result<List<OldFarmerApplicationLandDetailsDto>>.Success(result.ToList());
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
                using var connection = dbFactory.CreateConnection(HortiDb.Bts);
                const string sql = """
                SELECT 
                    s.sd_id AS SdId,
                    s.fd_id AS FdId,
                    s.uf_id AS UfId,
                    s.hf_id AS HfId,
                    s.village_code AS VillageCode,
                    v.village_name AS VillageName,
                    s.scheme_type AS SchemeType,
                    a.scheme_name  AS SchemeTypeName,
                    ms.scheme_name AS SchemeName,
                    mc.cname AS Cname,
                    s.quantity AS Quantity,
                    s.khasra_no AS KhasraNo,
                    s.area AS Area,
                    s.financial_year AS FinancialYear,
                    s.status AS Status
                FROM app_scheme_details s
                LEFT
                JOIN mas_scheme_horti ms ON ms.s_id=s.scheme_id
                LEFT
                JOIN mas_component_horti mc ON mc.c_id=s.component_id
                INNER
                JOIN (
                SELECT 
                    m.s_id,
                    m.scheme_name
                FROM mas_scheme_horti m
                WHERE m.st_id=0) a ON a.s_id=s.scheme_type
                INNER
                JOIN app_farmer_details fd ON fd.fd_id=s.fd_id
                INNER
                JOIN view_all_villages v ON v.village_code=s.village_code
                WHERE s.sd_id= @SdId
                AND s.fd_id= @FdId
                AND s.financial_year= @FinancialYear
                ORDER BY s.hf_id
                """;
                var result = await connection.QueryAsync<OldFarmerApplicationSchemeDetailsDto>(sql, new { FdId = fdId, SdId = sdId, FinancialYear = financialYear });
                return Result<List<OldFarmerApplicationSchemeDetailsDto>>.Success(result.ToList());
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
                using var connection = dbFactory.CreateConnection(HortiDb.Bts);
                const string sql = """
                SELECT 
                    cd.cd_id AS CdId,
                    cd.fd_id AS FdId,
                    cd.hf_id AS HfId,
                    cd.uf_id AS UfId,
                    cd.village_code AS VillageCode,
                    v.village_name AS VillageName,
                    cd.khasra_no AS KhasraNo,
                    cd.crop_code AS CropCode,
                    CONCAT(mc.crop_name_en,'/ ',mc.crop_name_hi) crop_name,
                    cd.crop_category AS CropCategory,
                    mcc.subcrop AS SubCropCategoryName,
                    cd.ccrop_veriety AS CropVariety,
                    cd.crop_season AS CropSeason,
                    cd.ccrop_area AS CropArea,
                    cd.village_type AS VillageType,
                    cd.is_join AS IsJoin,
                    cd.is_seed AS IsSeed,
                    cd.financial_year AS FinancialYear
                FROM app_crop_details cd
                INNER
                JOIN mas_crop mc ON mc.crop_code=cd.crop_code
                INNER
                JOIN mas_crop_categorys mcc ON mcc.id=cd.crop_category
                INNER
                JOIN view_all_villages v ON v.village_code=cd.village_code
                WHERE cd.fd_id= @FdId
                AND cd.financial_year= @FinancialYear
                """;
                var result = await connection.QueryAsync<OldFarmerApplicationCropDetailsDto>(sql, new { FdId = fdId, FinancialYear = financialYear });
                return Result<List<OldFarmerApplicationCropDetailsDto>>.Success(result.ToList());
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
                using var connection = dbFactory.CreateConnection(HortiDb.Bts);
                const string sql = """
                SELECT 
                    m.equipment_id AS EquipmentId,
                    m.hf_id AS HfId,
                    m.fd_id AS FdId,
                    m.uf_id AS UfId,
                    m.village_code AS VillageCode,
                    v.village_name AS VillageName,
                    m.machinary_id AS MachineryId,
                    CONCAT(i.item_name_en, '/ ', i.item_name_hi) AS machinery_name,
                    m.machinary_category AS MachineryCategory,
                    CONCAT(ic.itemcategory_name_en, '/ ',ic.itemcategory_name_hi) AS machinery_code,
                    m.quantity AS Quantity,
                    m.financial_year AS FinancialYear
                FROM app_machinary_self_owned m
                INNER
                JOIN view_all_villages v ON v.village_code=m.village_code
                INNER
                JOIN mas_item i ON i.item_code=m.machinary_id
                INNER
                JOIN mas_itemcategory ic ON ic.itemcategory_code=m.machinary_category
                WHERE m.fd_id= @FdId
                AND m.financial_year= @FinancialYear
                """;
                var result = await connection.QueryAsync<OldFarmerApplicationMachineryDetailsDto>(sql, new { FdId = fdId, FinancialYear = financialYear });
                return Result<List<OldFarmerApplicationMachineryDetailsDto>>.Success(result.ToList());
            }
            catch (Exception ex)
            {
                return Result<List<OldFarmerApplicationMachineryDetailsDto>>.Failure($"Failed to fetch farmer machinery details: {ex.Message}");
            }
        }
    }
}
