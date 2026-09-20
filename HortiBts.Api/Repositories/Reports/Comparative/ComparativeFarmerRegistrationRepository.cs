using Dapper;
using HortiBts.Api.Data;
using HortiBts.Shared.Common;
using HortiBts.Shared.Dtos.Reports.Comparative;
using Microsoft.AspNetCore.Connections;

namespace HortiBts.Api.Repositories.Reports.Comparative
{
    public interface IComparativeFarmerRegistrationRepository
    {
        Task<Result<List<DistWiseComparativeFarmerRegistrationDto>>> GetRptOfDistWiseComparativeFarmerRegistrationAsync(int financialYear);
        Task<Result<List<BlockWiseComparativeFarmerRegistrationDto>>> GetRptOfBlockWiseComparativeFarmerRegistrationAsync(int districtCode, int financialYear);
        Task<Result<List<RheoWiseComparativeFarmerRegistrationDto>>> GetRptOfRheoWiseComparativeFarmerRegistrationAsync(RheoWiseFarmerFilterDto filter);        
        Task<Result<List<VillageWiseComparativeFarmerRegistrationDto>>> GetRptOfVillageWiseComparativeFarmerRegistrationAsync(int officerCode, int financialYear);
    }
    public class ComparativeFarmerRegistrationRepository(IDbConnectionFactory connectionFactory) : IComparativeFarmerRegistrationRepository
    {
        public async Task<Result<List<DistWiseComparativeFarmerRegistrationDto>>> GetRptOfDistWiseComparativeFarmerRegistrationAsync(int financialYear)
        {
            using var connection = connectionFactory.CreateConnection();
            string Sql = @"
            WITH relevant_villages AS (
            SELECT 
                v.village_code
            FROM view_all_villages v ), farmer_details AS (
            SELECT DISTINCT
                fd.fd_id,
                fd.hf_id,
                fd.rheo_id,
                fd.village_code
            FROM farmer_detail_horti fd
            INNER
            JOIN mas_farmer_horti mf ON mf.hf_id = fd.hf_id
            INNER
            JOIN relevant_villages v ON v.village_code = fd.village_code ), scheme_summary AS (
            SELECT 
                COUNT(DISTINCT CASE WHEN gs.financial_year = @FinancialYear THEN gs.fd_id END) AS farmer_count_fyb4,
                COUNT(DISTINCT CASE WHEN gs.financial_year = @FinancialYear - 1 THEN gs.fd_id END) AS farmer_count_fyb3,
                COUNT(DISTINCT CASE WHEN gs.financial_year = @FinancialYear - 2 THEN gs.fd_id END) AS farmer_count_fyb2,
                COUNT(DISTINCT CASE WHEN gs.financial_year = @FinancialYear - 3 THEN gs.fd_id END) AS farmer_count_fyb1,
                fd.village_code
            FROM equipment_details_govt_schemes gs
            INNER
            JOIN farmer_details fd ON fd.fd_id = gs.fd_id
            GROUP BY fd.village_code )
            SELECT 
                SUM(IFNULL(ss.farmer_count_fyb4, 0)) AS TotalFarmerCountFyb4,
                SUM(IFNULL(ss.farmer_count_fyb3, 0)) AS TotalFarmerCountFyb3,
                SUM(IFNULL(ss.farmer_count_fyb2, 0)) AS TotalFarmerCountFyb2,
                SUM(IFNULL(ss.farmer_count_fyb1, 0)) AS TotalFarmerCountFyb1,
                va.DistCodeCensus AS DistrictCode,
                rd.DistrictNameHindi AS DistrictNameHi,
                rd.DistrictName AS DistrictNameEn
            FROM view_all_villages va
            INNER
            JOIN rev_district rd ON rd.DistrictCensus = va.DistCodeCensus
            INNER
            JOIN relevant_villages rv ON rv.village_code = va.village_code
            LEFT
            JOIN scheme_summary ss ON ss.village_code = rv.village_code
            GROUP BY va.DistCodeCensus, rd.DistrictNameHindi
            ORDER BY DistrictNameEn;;";

            var result = await connection.QueryAsync<DistWiseComparativeFarmerRegistrationDto>(Sql, new { FinancialYear = financialYear });
            return Result<List<DistWiseComparativeFarmerRegistrationDto>>.Success(result.ToList());
        }

        public async Task<Result<List<BlockWiseComparativeFarmerRegistrationDto>>> GetRptOfBlockWiseComparativeFarmerRegistrationAsync(int districtCode, int financialYear)
        {
            using var connection = connectionFactory.CreateConnection();
            string Sql = @"
            WITH villages AS (
            SELECT 
                v.village_code
            FROM view_all_villages v
            WHERE v.DistCodeCensus= @DistrictCode ), farmer_detail AS (
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
                COUNT(DISTINCT CASE WHEN gs.financial_year = @FinancialYear THEN gs.fd_id END) AS farmer_count_fyb4,
                COUNT(DISTINCT CASE WHEN gs.financial_year = @FinancialYear - 1 THEN gs.fd_id END) AS farmer_count_fyb3,
                COUNT(DISTINCT CASE WHEN gs.financial_year = @FinancialYear - 2 THEN gs.fd_id END) AS farmer_count_fyb2,
                COUNT(DISTINCT CASE WHEN gs.financial_year = @FinancialYear - 3 THEN gs.fd_id END) AS farmer_count_fyb1,
                fd.village_code
            FROM equipment_details_govt_schemes gs
            INNER
            JOIN farmer_detail fd ON fd.fd_id=gs.fd_id
            GROUP BY fd.village_code )
            SELECT 
                SUM(IFNULL(s.farmer_count_fyb4, 0)) AS TotalFarmerCountFyb4,
                SUM(IFNULL(s.farmer_count_fyb3, 0)) AS TotalFarmerCountFyb3,
                SUM(IFNULL(s.farmer_count_fyb2, 0)) AS TotalFarmerCountFyb2,
                SUM(IFNULL(s.farmer_count_fyb1, 0)) AS TotalFarmerCountFyb1,
                va.subdistrict_code AS SubDistrictCode,
                rb.BlockNameHin AS SubDistrictNameHi,
                rb.BlockNameEng SubDistrictNameEn,
                va.DistCodeCensus AS DistrictCode,
                rd.DistrictNameHindi AS DistrictNameHi,
                rd.DistrictName AS DistrictNameEn
            FROM view_all_villages va
            INNER
            JOIN rev_district rd ON rd.DistrictCensus = va.DistCodeCensus
            INNER
            JOIN rev_block rb ON rb.subdistrict_code=va.subdistrict_code
            INNER
            JOIN villages ovd ON ovd.village_code = va.village_code
            LEFT
            JOIN scheme s ON s.village_code=ovd.village_code
            GROUP BY va.subdistrict_code
            ORDER BY DistrictNameEn desc";

            var result = await connection.QueryAsync<BlockWiseComparativeFarmerRegistrationDto>(Sql, new { DistrictCode = districtCode, FinancialYear = financialYear });
            return Result<List<BlockWiseComparativeFarmerRegistrationDto>>.Success(result.ToList());
        }

        public async Task<Result<List<RheoWiseComparativeFarmerRegistrationDto>>> GetRptOfRheoWiseComparativeFarmerRegistrationAsync(RheoWiseFarmerFilterDto filter)
        {
            using var connection = connectionFactory.CreateConnection();

            const string sql = """
            WITH villages AS (
            SELECT DISTINCT
                ovd.village_code,
                ovd.officer_code,
                v.DistCodeCensus,
                v.DistrictName,
                v.subdistrict_code,
                v.subdistrict_name
            FROM officer_village_details ovd
            INNER
            JOIN view_all_villages v ON v.village_code = ovd.village_code
            WHERE ovd.department_code = @DepartmentCode
            AND (@DistrictCode IS NULL
            OR @DistrictCode = 0
            OR v.DistCodeCensus = @DistrictCode)
            AND (@SubDistrictCode IS NULL
            OR @SubDistrictCode = 0
            OR v.subdistrict_code = @SubDistrictCode) ), farmer_details AS (
            SELECT DISTINCT
                fd.fd_id,
                fd.hf_id,
                fd.rheo_id,
                fd.village_code
            FROM farmer_detail_horti fd
            INNER
            JOIN mas_farmer_horti mf ON mf.hf_id = fd.hf_id
            INNER
            JOIN villages v ON v.village_code = fd.village_code ), scheme_summary AS (
            SELECT 
                COUNT(DISTINCT CASE WHEN gs.financial_year = @FinancialYear THEN gs.fd_id END) AS farmer_count_fyb4,
                COUNT(DISTINCT CASE WHEN gs.financial_year = @FinancialYear - 1 THEN gs.fd_id END) AS farmer_count_fyb3,
                COUNT(DISTINCT CASE WHEN gs.financial_year = @FinancialYear - 2 THEN gs.fd_id END) AS farmer_count_fyb2,
                COUNT(DISTINCT CASE WHEN gs.financial_year = @FinancialYear - 3 THEN gs.fd_id END) AS farmer_count_fyb1,
                fd.village_code
            FROM equipment_details_govt_schemes gs
            INNER
            JOIN farmer_details fd ON fd.fd_id = gs.fd_id
            GROUP BY fd.village_code )
            SELECT 
                SUM(IFNULL(ss.farmer_count_fyb4, 0)) AS TotalFarmerCountFyb4,
                SUM(IFNULL(ss.farmer_count_fyb3, 0)) AS TotalFarmerCountFyb3,
                SUM(IFNULL(ss.farmer_count_fyb2, 0)) AS TotalFarmerCountFyb2,
                SUM(IFNULL(ss.farmer_count_fyb1, 0)) AS TotalFarmerCountFyb1,
                va.officer_code AS OfficerCode,
                m.name AS OfficerName,
                m.mobile_no AS MobileNo,
                rb.BlockNameHin AS SubDistrictNameHi,
                rb.BlockNameEng AS SubDistrictNameEn,
                va.subdistrict_code AS SubDistrictCode,
                va.DistCodeCensus AS DistrictCode,
                rd.DistrictNameHindi AS DistrictNameHi,
                rd.DistrictName AS DistrictNameEn
            FROM villages va
            INNER
            JOIN rev_block rb ON rb.subdistrict_code = va.subdistrict_code
            INNER
            JOIN rev_district rd ON rd.DistrictCensus = va.DistCodeCensus
            INNER
            JOIN mas_raeo m ON m.officer_code = va.officer_code
            LEFT
            JOIN scheme_summary ss ON ss.village_code = va.village_code
            GROUP BY va.officer_code, m.name, m.mobile_no, rb.BlockNameHin, rb.BlockNameEng, va.subdistrict_code, va.DistCodeCensus, rd.DistrictNameHindi, rd.DistrictName
            ORDER BY rd.DistrictName, rb.BlockNameEng, m.name
            """;

            var parameters = new
            {
                DepartmentCode = filter.DepartmentCode,
                DistrictCode = filter.DistrictCode,
                SubDistrictCode = filter.SubDistrictCode,
                FinancialYear = filter.FinancialYear
            };

            var result = await connection.QueryAsync<RheoWiseComparativeFarmerRegistrationDto>(sql, parameters);
            return Result<List<RheoWiseComparativeFarmerRegistrationDto>>.Success(result.ToList());
        }

        public async Task<Result<List<VillageWiseComparativeFarmerRegistrationDto>>> GetRptOfVillageWiseComparativeFarmerRegistrationAsync(int officerCode, int financialYear)
        {
            using var connection = connectionFactory.CreateConnection();
            const string sql = """
                WITH villages AS
                       (
                           SELECT
                               ovd.village_code,
                               ovd.officer_code
                           FROM officer_village_details ovd
                           WHERE ovd.officer_code = @OfficerCode
                       ),

                       farmer_details AS
                       (
                           SELECT DISTINCT
                               fd.fd_id,
                               fd.hf_id,
                               fd.rheo_id,
                               fd.village_code
                           FROM farmer_detail_horti fd
                           INNER JOIN mas_farmer_horti mf
                               ON mf.hf_id = fd.hf_id
                           INNER JOIN villages v
                               ON v.village_code = fd.village_code
                       ),

                       scheme_summary AS
                       (
                           SELECT
                               COUNT(DISTINCT CASE WHEN gs.financial_year = @FinancialYear THEN gs.fd_id END) AS farmer_count_fyb4,
                               COUNT(DISTINCT CASE WHEN gs.financial_year = @FinancialYear - 1 THEN gs.fd_id END) AS farmer_count_fyb3,
                               COUNT(DISTINCT CASE WHEN gs.financial_year = @FinancialYear - 2 THEN gs.fd_id END) AS farmer_count_fyb2,
                               COUNT(DISTINCT CASE WHEN gs.financial_year = @FinancialYear - 3 THEN gs.fd_id END) AS farmer_count_fyb1,

                               fd.village_code
                           FROM equipment_details_govt_schemes gs
                           INNER JOIN farmer_details fd
                               ON fd.fd_id = gs.fd_id
                           GROUP BY fd.village_code
                       )

                       SELECT
                           SUM(IFNULL(ss.farmer_count_fyb4, 0)) AS TotalFarmerCountFyb4,

                           SUM(IFNULL(ss.farmer_count_fyb3, 0)) AS TotalFarmerCountFyb3,

                           SUM(IFNULL(ss.farmer_count_fyb2, 0)) AS TotalFarmerCountFyb2,

                           SUM(IFNULL(ss.farmer_count_fyb1, 0)) AS TotalFarmerCountFyb1,

                           va.village_code AS VillageCode,
                           va.village_name AS VillageName,
                
                           rb.BlockNameEng AS SubDistrictNameEn,
                           va.subdistrict_code AS SubDistrictCode,
                           rb.BlockNameHin AS SubDistrictNameHi,
                
                           va.DistCodeCensus AS DistrictCode,
                           rd.DistrictNameHindi AS DistrictNameHi,
                           rd.DistrictName AS DistrictNameEn,
                           v.officer_code AS OfficerCode,

                           m.name AS OfficerName,
                           m.mobile_no AS MobileNo

                       FROM view_all_villages va
                       INNER JOIN villages v
                            ON v.village_code = va.village_code
                       INNER JOIN rev_block rb
                            ON rb.subdistrict_code = va.subdistrict_code
                       INNER JOIN rev_district rd
                            ON rd.DistrictCensus = va.DistCodeCensus
                       INNER JOIN mas_raeo m
                           ON m.officer_code = v.officer_code
                       LEFT JOIN scheme_summary ss
                           ON ss.village_code = v.village_code

                       GROUP BY
                           va.village_code,
                           va.village_name,
                           v.officer_code,
                           m.name,
                           m.mobile_no

                       ORDER BY
                           va.village_name                       
                """;

            var result = await connection.QueryAsync<VillageWiseComparativeFarmerRegistrationDto>(sql, new { OfficerCode = officerCode, FinancialYear = financialYear });
            return Result<List<VillageWiseComparativeFarmerRegistrationDto>>.Success(result.ToList());
        }

    }

    public class RheoWiseFarmerFilterDto
    {
        public int DepartmentCode { get; set; }
        public int? DistrictCode { get; set; }
        public int? SubDistrictCode { get; set; }
        public int? FinancialYear { get; set; }
    }
    public class FarmerWiseReportFilterDto
    {
        public int DepartmentCode { get; set; }
        public int? DistrictCode { get; set; }
        public int? SubDistrictCode { get; set; }
        public int? VillageCode { get; set; }
        public int? OfficerCode { get; set; }
        public int FinancialYear { get; set; }
        public int? SchemeType { get; set; }
        public int? SchemeId { get; set; }
        public int? ComponentId { get; set; }
    }
}
