using Dapper;
using HortiBts.Api.Data;
using HortiBts.Shared.Common;
using HortiBts.Shared.Dtos.Reports;

namespace HortiBts.Api.Repositories.Reports.Backlog
{
    public interface IBacklogFarmerRegistrationRepository
    {
        Task<Result<List<DistWiseFarmerRegistrationDto>>> GetBacklogRptOfDistwiseFarmerRegistrationAsync(int financialYear);
        Task<Result<List<BlockWiseFarmerRegistrationDto>>> GetBacklogRptOfBlockwiseFarmerRegistrationAsync(int districtCode, int financialYear);
    }
    public class BacklogFarmerRegistrationRepository(IDbConnectionFactory connectionFactory) : IBacklogFarmerRegistrationRepository
    {
        public async Task<Result<List<DistWiseFarmerRegistrationDto>>> GetBacklogRptOfDistwiseFarmerRegistrationAsync(int financialYear)
        {
            using var connection = connectionFactory.CreateConnection();
            string Sql = @"
            WITH villages AS (
            SELECT 
                v.village_code,
                v.DistCodeCensus,
                v.DistrictName
            FROM view_all_villages v ), farmer_detail AS (
            SELECT DISTINCT
                fd.fd_id,
                fd.hf_id,
                fd.rheo_id,
                fd.village_code
            FROM farmer_detail_horti fd
            INNER
            JOIN mas_farmer_horti mf ON mf.hf_id=fd.hf_id
            INNER
            JOIN villages v ON v.village_code= fd.village_code ), scheme AS (
            SELECT 
                count(DISTINCT gs.fd_id) f_count,
                COUNT( gs.equipment_id) s_count,
                SUM( (CASE WHEN gs.unit=4 THEN gs.area/2.471 WHEN gs.unit=6 THEN gs.area/10000 ELSE gs.area END) ) area,
                SUM(gs.cash) AS subsidy,
                fd.village_code
            FROM equipment_details_govt_schemes gs
            INNER
            JOIN farmer_detail fd ON fd.fd_id=gs.fd_id
            WHERE gs.financial_year = @FinancialYear
            GROUP BY fd.village_code )
            SELECT 
                IFNULL(r.target_count,0) Target,
                sum( ifnull(s.f_count,0)) AS FCount,
                sum(ifnull(s.s_count,0)) AS SCount,
                truncate(sum(ifnull(s.area,0)),3) AS Area,
                sum(ifnull(s.subsidy,0)) AS Subsidy,
                va.DistCodeCensus DistrictCode,
                rd.DistrictNameHindi DistrictNameHi,
                rd.DistrictName DistrictNameEn
            FROM view_all_villages va
            INNER
            JOIN rev_district rd ON rd.DistrictCensus=va.DistCodeCensus
            LEFT
            JOIN scheme s ON s.village_code=va.village_code
            LEFT
            JOIN reg_target r ON r.user_id=rd.DistrictCensus
            AND r.financial_year = @FinancialYear
            GROUP BY va.DistCodeCensus
            ORDER BY va.DistrictName";

            var result = await connection.QueryAsync<DistWiseFarmerRegistrationDto>(Sql, new { FinancialYear = financialYear });
            return Result<List<DistWiseFarmerRegistrationDto>>.Success(result.ToList());
        }

        public async Task<Result<List<BlockWiseFarmerRegistrationDto>>> GetBacklogRptOfBlockwiseFarmerRegistrationAsync(int districtCode, int financialYear)
        {
            using var connection = connectionFactory.CreateConnection();
            string Sql = @"
            WITH villages AS (
            SELECT 
                v.village_code,
                v.subdistrict_code,
                v.subdistrict_name,
                v.DistrictName,
                v.DistCodeCensus   
            FROM view_all_villages v
            WHERE v.DistCodeCensus=@DistrictCode ), farmer_detail AS (
            SELECT DISTINCT
                fd.fd_id,
                fd.hf_id,
                fd.rheo_id,
                fd.village_code
            FROM farmer_detail_horti fd
            INNER
            JOIN mas_farmer_horti mf ON mf.hf_id=fd.hf_id
            INNER
            JOIN villages v ON v.village_code= fd.village_code ), scheme AS (
            SELECT 
                count(DISTINCT gs.fd_id) f_count,
                COUNT( gs.equipment_id) s_count,
                SUM( (CASE WHEN gs.unit=4 THEN gs.area/2.471 WHEN gs.unit=6 THEN gs.area/10000 ELSE gs.area END) ) area,
                SUM(gs.cash) AS subsidy,
                fd.village_code
            FROM equipment_details_govt_schemes gs
            INNER
            JOIN farmer_detail fd ON fd.fd_id=gs.fd_id
            WHERE gs.financial_year=@FinancialYear
            GROUP BY fd.village_code )
            SELECT 
                sum( ifnull(s.f_count,0)) AS FCount,
                sum(ifnull(s.s_count,0)) AS SCount,
                truncate(sum(ifnull(s.area,0)),3) AS Area,
                sum(ifnull(s.subsidy,0)) AS Subsidy,
                va.subdistrict_code AS SubDistrictCode,
                rb.BlockNameHin AS SubDistrictNameHi,
                rb.BlockNameEng AS SubDistrictNameEn,
                va.DistCodeCensus As DistrictCode,
                rd.DistrictNameHindi AS DistrictNameHi,
                rd.DistrictName AS DistrictNameEn
            FROM villages va
            INNER
            JOIN rev_district rd ON rd.DistrictCensus=va.DistCodeCensus
            INNER
            JOIN rev_block rb ON rb.subdistrict_code=va.subdistrict_code
            LEFT
            JOIN scheme s ON s.village_code=va.village_code
            GROUP BY va.subdistrict_code
            ORDER BY rb.BlockNameEng";

            var result = await connection.QueryAsync<BlockWiseFarmerRegistrationDto>(Sql, new { DistrictCode = districtCode, FinancialYear = financialYear });
            return Result<List<BlockWiseFarmerRegistrationDto>>.Success(result.ToList());
        }
    }
}
