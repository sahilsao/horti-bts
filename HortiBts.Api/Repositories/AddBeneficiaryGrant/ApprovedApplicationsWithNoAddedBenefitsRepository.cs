using Dapper;
using HortiBts.Api.Data;
using HortiBts.Shared.Common;
using HortiBts.Shared.Dtos.AddBeneficiaryGrant;
using MySqlConnector;

namespace HortiBts.Api.Repositories.AddBeneficiaryGrant;

public interface IApprovedApplicationsWithNoAddedBenefitsRepository
{
    Task<Result<List<ApprovedApplicationsWithNoAddedBenefitsByDistrictCodeDto>>> GetApprovedApplicationsWithNoAddedBenefitsByDistrictCodeAsync(int districtCode, int financialYear);
    Task<Result<List<ApprovedApplicationsWithNoAddedBenefitsByApplicationIdDto>>> GetApprovedApplicationsWithNoAddedBenefitsByApplicationIDAsync(int applicationId, int financialYear);
    Task<Result<int>> SaveApplicationSchemeBenefitDataAsync(SaveApplicationSchemeBenefitDataDto dto, string createdBy, string clientIp);

}

public class ApprovedApplicationsWithNoAddedBenefitsRepository(IDbConnectionFactory connectionFactory) : IApprovedApplicationsWithNoAddedBenefitsRepository
{
    public async Task<Result<List<ApprovedApplicationsWithNoAddedBenefitsByDistrictCodeDto>>> GetApprovedApplicationsWithNoAddedBenefitsByDistrictCodeAsync(int districtCode, int financialYear)
    {
        using var connection = connectionFactory.CreateConnection();
        string Sql = @"
            SELECT 
                sd.application_id AS ApplicationId,
                sd.hf_id AS HfId,
                sd.sd_id AS SdId,
                mf.uf_id AS UfId,
                mf.farmer_name_eng AS FarmerNameEng,
                mf.farmer_name_hi AS FarmerNameHi,
                mf.father_name AS FatherName,
                mf.mobile_no AS MobileNo,
                sd.scheme_type AS SchemeType,
                sd.scheme_id AS SchemeId,
                ms.scheme_name AS SchemeName,
                mc.cname AS component_name,
                sd.component_id AS ComponentId,
                sd.area AS Area,
                sd.quantity AS Quantity,
                sd.financial_year AS FinancialYear,
                sd.sd_id AS SdId,
                fa.hf_id AS HfId,
                fa.village_code AS VillageCode,
                b.ifsc_code AS IfscCode,
                b.account_no AS AccountNo,
                ld1.total_land_khasra AS TotalLandKhasra,
                ld1.total_land_area AS TotalLandArea,
                sk.khasra_no AS KhasraNo,
                sd.status AS Status,
                sd.created_at AS reg_date,
                v.village_code AS VillageCode,
                v.village_name AS VillageName
            FROM temp_scheme_details sd
            INNER
            JOIN mas_scheme_horti ms ON ms.s_id = sd.scheme_id
            INNER
            JOIN mas_component_horti mc ON mc.c_id = sd.component_id
            INNER
            JOIN temp_farmer_application fa ON fa.application_id= sd.application_id
            INNER
            JOIN temp_mas_farmer mf ON mf.hf_id = sd.hf_id
            INNER
            JOIN temp_bank_details b ON b.fd_id = sd.fd_id
            AND b.financial_year = @FinancialYear
            INNER
            JOIN (
            SELECT 
                ld.application_id,
                GROUP_CONCAT(ld.khasra_no, '(', ld.area, ') ') AS total_land_khasra,
                SUM(ld.area) AS total_land_area
            FROM temp_land_details ld
            WHERE ld.financial_year = @FinancialYear
            GROUP BY ld.application_id ) ld1 ON ld1.application_id = fa.application_id
            LEFT
            JOIN (
            SELECT 
                gsk.sd_id,
                GROUP_CONCAT(' ', gsk.khasra_no) AS khasra_no
            FROM temp_scheme_khasra_details gsk
            WHERE gsk.financial_year = @FinancialYear
            GROUP BY gsk.sd_id ) sk ON sk.sd_id = sd.sd_id
            INNER
            JOIN view_all_villages v ON v.village_code=sd.village_code
            WHERE v.DistCodeCensus= @DistrictCode
            AND fa.rheo_approval_status=1
            AND fa.ddh_approval_status=1
            AND NOT EXISTS (
            SELECT 
                a.application_id
            FROM temp_approved_benefit_details a
            WHERE a.application_id=sd.application_id)
            ORDER BY sd.created_at";

        var result = await connection.QueryAsync<ApprovedApplicationsWithNoAddedBenefitsByDistrictCodeDto>(Sql, new { DistrictCode = districtCode, FinancialYear = financialYear });
        return Result<List<ApprovedApplicationsWithNoAddedBenefitsByDistrictCodeDto>>.Success(result.ToList());
    }

    public async Task<Result<List<ApprovedApplicationsWithNoAddedBenefitsByApplicationIdDto>>> GetApprovedApplicationsWithNoAddedBenefitsByApplicationIDAsync(int applicationId, int financialYear)
    {
        using var connection = connectionFactory.CreateConnection();
        string Sql = @"
            SELECT 
                s.application_id AS ApplicationId,
                s.sd_id AS SdId,
                s.fd_id AS FdId,
                s.uf_id AS UfId,
                s.hf_id AS HfId,
                mf.mobile_no AS MobileNo,
                s.scheme_type AS SchemeType,
                s.scheme_id AS SchemeId,
                s.financial_year AS FinancialYear,
                v.DistCodeCensus  AS DistrictCode,
                s.village_code AS VillageCode,
                v.village_name AS VillageName,
                a.scheme_name AS SchemeTypeName,
                ms.scheme_name AS SchemeName,
                mc.cname AS ComponentName,
                s.quantity AS Quantity,
                s.khasra_no AS KhasraNo,
                s.area AS Area,
                s.financial_year AS FinancialYear,
                s.status AS Status
            FROM temp_scheme_details s
            LEFT
            JOIN mas_scheme_horti ms ON ms.s_id=s.scheme_id
            LEFT
            JOIN mas_component_horti mc ON mc.c_id=s.component_id
            INNER
            JOIN temp_farmer_application fa ON fa.application_id=s.application_id
            INNER
            JOIN (
            SELECT 
                m.s_id,
                m.scheme_name
            FROM mas_scheme_horti m
            WHERE m.st_id=0) a ON a.s_id=s.scheme_type
            INNER
            JOIN temp_farmer_details fd ON fd.fd_id=s.fd_id
            INNER
            JOIN temp_mas_farmer mf ON mf.hf_id = s.hf_id
            INNER
            JOIN view_all_villages v ON v.village_code=s.village_code
            WHERE s.application_id= @ApplicationId
            AND s.financial_year= @FinancialYear
            AND fa.rheo_approval_status=1";

        var result = await connection.QueryAsync<ApprovedApplicationsWithNoAddedBenefitsByApplicationIdDto>(Sql, new { ApplicationId = applicationId, FinancialYear = financialYear });
        return Result<List<ApprovedApplicationsWithNoAddedBenefitsByApplicationIdDto>>.Success(result.ToList());
    }

    public async Task<Result<int>> SaveApplicationSchemeBenefitDataAsync(SaveApplicationSchemeBenefitDataDto dto, string createdBy, string clientIp)
    {
        using var connection = connectionFactory.CreateConnection(HortiDb.Bts);
        connection.Open(); 
        using var tran = connection.BeginTransaction();

        try
        {
            // insert 1: temp_approved_benefit_details
            const string sql1 = @"
                    INSERT INTO temp_approved_benefit_details
                        (application_id, sd_id, fd_id, hf_id, uf_id, financial_year,
                         approved_area, quantity, benefit, total_cost, subsidy,
                         remark, ip_address, created_by)
                    VALUES
                        (@ApplicationId, @SdId, @FdId, @HfId, @UfId, @FinancialYear,
                         @Area, @ItemNo, @Benefit, @TotalCost, @Subsidy,
                         @Remark, @IpAddress, @CreatedBy);
                    SELECT LAST_INSERT_ID();";

            var abId = await connection.ExecuteScalarAsync<int>(sql1, new
            {
                dto.ApplicationId,
                dto.SdId,
                dto.FdId,
                dto.HfId,
                dto.UfId,
                dto.FinancialYear,
                dto.Area,
                dto.ItemNo,
                Benefit = string.Join(",", dto.BenefitIds),
                dto.TotalCost,
                dto.Subsidy,
                dto.Remark,
                IpAddress = clientIp,
                CreatedBy = createdBy
            }, tran);

            // insert 2: temp_scheme_benefit_details, one row per benefit
            const string sql2 = @"
                    INSERT INTO temp_scheme_benefit_details
                        (ab_id, sd_id, hf_id, fd_id, uf_id, benefit_id, benefit_type,
                         financial_year, ip_address, created_by)
                    VALUES
                        (@AbId, @SdId, @HfId, @FdId, @UfId, @BenefitId, 1,
                         @FinancialYear, @IpAddress, @CreatedBy);";

            var rows = dto.BenefitIds.Select(bid => new
            {
                AbId = abId,
                dto.SdId,
                dto.HfId,
                dto.FdId,
                dto.UfId,
                BenefitId = bid,
                dto.FinancialYear,
                IpAddress = clientIp,
                CreatedBy = createdBy
            });
            await connection.ExecuteAsync(sql2, rows, tran);

            tran.Commit();
            return Result<int>.Success(abId);
        }
        catch (MySqlException ex) when (ex.Number == 1062)
        {
            tran.Rollback();
            return Result<int>.Failure("Grant already added for this application.");
        }
        catch (Exception ex)
        {
            tran.Rollback();
            return Result<int>.Failure($"Unable to save benefit data: {ex.Message}");
        }
    }
}
