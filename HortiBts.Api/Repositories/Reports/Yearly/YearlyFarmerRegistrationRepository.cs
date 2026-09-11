using Dapper;
using HortiBts.Api.Data;
using HortiBts.Shared.Common;
using HortiBts.Shared.Dtos.Reports;

namespace HortiBts.Api.Repositories.Reports.Yearly
{
    public interface IYearlyFarmerRegistrationRepository
    {
        Task<Result<List<DistwiseFarmerRegistrationDto>>> GetYearlyRptOfDistwiseFarmerRegistrationAsync(int financialYear);
        Task<Result<List<BlockwiseFarmerRegistrationDto>>> GetYearlyRptOfBlockwiseFarmerRegistrationAsync(int districtCode, int financialYear);
        Task<Result<List<RheoWiseFarmerRegistrationDto>>> GetYearlyRptOfRheowiseFarmerRegistrationAsync(RheoWiseFarmerFilterDto filter);
    }
    public class YearlyFarmerRegistrationRepository(IDbConnectionFactory connectionFactory) : IYearlyFarmerRegistrationRepository
    {
        public async Task<Result<List<DistwiseFarmerRegistrationDto>>> GetYearlyRptOfDistwiseFarmerRegistrationAsync(int financialYear)
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
                sum(ifnull(s.f_count,0)) AS FarmerCount,
                sum(ifnull(s.s_count,0)) AS SchemeCount,
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

            var result = await connection.QueryAsync<DistwiseFarmerRegistrationDto>(Sql, new { FinancialYear = financialYear });
            return Result<List<DistwiseFarmerRegistrationDto>>.Success(result.ToList());
        }

        public async Task<Result<List<BlockwiseFarmerRegistrationDto>>> GetYearlyRptOfBlockwiseFarmerRegistrationAsync(int districtCode, int financialYear)
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
                sum(ifnull(s.f_count,0)) AS FarmerCount,
                sum(ifnull(s.s_count,0)) AS SchemeCount,
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

            var result = await connection.QueryAsync<BlockwiseFarmerRegistrationDto>(Sql, new { DistrictCode = districtCode, FinancialYear = financialYear });
            return Result<List<BlockwiseFarmerRegistrationDto>>.Success(result.ToList());
        }


        public async Task<Result<List<RheoWiseFarmerRegistrationDto>>> GetYearlyRptOfRheowiseFarmerRegistrationAsync(RheoWiseFarmerFilterDto filter)
        {
            using var connection = connectionFactory.CreateConnection();
            const string sql = """
            WITH villages AS
            (
                SELECT
                    ovd.village_code,
                    ovd.officer_code,
                    v.DistCodeCensus,
                    v.DistrictName,
                    v.subdistrict_code,
                    v.subdistrict_name
                FROM officer_village_details ovd
                INNER JOIN view_all_villages v
                    ON v.village_code = ovd.village_code
                WHERE ovd.department_code = @DepartmentCode
                  AND (@DistrictCode IS NULL OR @DistrictCode = 0
                       OR v.DistCodeCensus = @DistrictCode)
                  AND (@SubDistrictCode IS NULL OR @SubDistrictCode = 0
                       OR v.subdistrict_code = @SubDistrictCode)
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

            schemes AS
            (
                SELECT
                    COUNT(DISTINCT gs.fd_id) AS f_count,
                    COUNT(gs.equipment_id) AS s_count,

                    SUM(
                        CASE
                            WHEN gs.unit = 4 THEN gs.area / 2.471
                            WHEN gs.unit = 6 THEN gs.area / 10000
                            ELSE gs.area
                        END
                    ) AS area,

                    SUM(gs.cash) AS subsidy,
                    fd.village_code
                FROM equipment_details_govt_schemes gs
                INNER JOIN farmer_details fd
                    ON fd.fd_id = gs.fd_id

                WHERE (@FinancialYear IS NULL OR @FinancialYear = ''
                       OR gs.financial_year = @FinancialYear)

                GROUP BY fd.village_code
            )

            SELECT
                SUM(COALESCE(s.f_count, 0)) AS FarmerCount,
                SUM(COALESCE(s.s_count, 0)) AS SchemeCount,

                TRUNCATE(
                    SUM(COALESCE(s.area, 0)),
                    3
                ) AS Area,

                SUM(COALESCE(s.subsidy, 0)) AS Subsidy,

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

            INNER JOIN rev_block rb
                ON rb.subdistrict_code = va.subdistrict_code

            INNER JOIN rev_district rd
                ON rd.DistrictCensus = va.DistCodeCensus

            INNER JOIN mas_raeo m
                ON m.officer_code = va.officer_code

            LEFT JOIN schemes s
                ON s.village_code = va.village_code

            GROUP BY
                m.officer_code,
                m.name,
                m.mobile_no,
                rb.BlockNameHin,
                va.subdistrict_code,
                va.DistCodeCensus,
                rd.DistrictNameHindi

            ORDER BY
                rd.DistrictNameHindi,
                rb.BlockNameHin,
                m.name;
            """;

            var parameters = new
            {
                DepartmentCode = filter.DepartmentCode,
                DistrictCode = filter.DistrictCode,
                SubDistrictCode = filter.SubDistrictCode,
                FinancialYear = filter.FinancialYear
            };

            var result = await connection.QueryAsync<RheoWiseFarmerRegistrationDto>(sql, parameters);
            return Result<List<RheoWiseFarmerRegistrationDto>>.Success(result.ToList());
        }    
    }
}


public class RheoWiseFarmerFilterDto
{
    public int DepartmentCode { get; set; }
    public int? DistrictCode { get; set; }
    public int? SubDistrictCode { get; set; }
    public int? FinancialYear { get; set; }
}