using Dapper;
using HortiBts.Api.Data;
using HortiBts.Shared.Common;
using HortiBts.Shared.Dtos.Reports.BacklogYearly;
using Microsoft.AspNetCore.Connections;

namespace HortiBts.Api.Repositories.Reports.BacklogYearly
{
    public interface IBacklogFarmerRegistrationRepository
    {
        Task<Result<List<DistWiseFarmerRegistrationDto>>> GetBacklogRptOfDistwiseFarmerRegistrationAsync(int financialYear);
        Task<Result<List<BlockWiseFarmerRegistrationDto>>> GetBacklogRptOfBlockwiseFarmerRegistrationAsync(int districtCode, int financialYear);
        Task<Result<List<RheoWiseFarmerRegistrationDto>>> GetBacklogRptOfRheowiseFarmerRegistrationAsync(RheoWiseFarmerFilterDto filter);
        Task<Result<List<VillageWiseFarmerRegistrationDto>>> GetBacklogRptOfVillagewiseFarmerRegistrationAsync(int officerCode, int financialYear);
        Task<Result<List<FarmerWiseFarmerRegistrationDto>>> GetBacklogRptOfFarmerwiseFarmerRegistrationAsync(FarmerWiseReportFilterDto filter);
    }
    public class BacklogYearlyFarmerRegistrationRepository(IDbConnectionFactory connectionFactory) : IBacklogFarmerRegistrationRepository
    {
        public async Task<Result<List<DistWiseFarmerRegistrationDto>>> GetBacklogRptOfDistwiseFarmerRegistrationAsync(int financialYear)
        {
            using var connection = connectionFactory.CreateConnection();
            string Sql = @"
            WITH relevant_villages AS (
                SELECT DISTINCT
                    v.village_code
                FROM view_all_villages v
            ), farmer_details AS (
            SELECT DISTINCT
                fd.fd_id AS farmer_id,
                fd.hf_id AS horti_farmer_id,
                fd.rheo_id AS rheo_id,
                fd.village_code
            FROM farmer_detail_horti fd
            INNER
            JOIN mas_farmer_horti mf ON mf.hf_id = fd.hf_id
            INNER
            JOIN relevant_villages rv ON rv.village_code = fd.village_code ), scheme_summary AS (
            SELECT 
                COUNT(DISTINCT CASE WHEN gs.financial_year = @FinancialYear THEN gs.fd_id END) AS farmer_count_fyb4,
                COUNT(DISTINCT CASE WHEN gs.financial_year = @FinancialYear - 1 THEN gs.fd_id END) AS farmer_count_fyb3,
                COUNT(DISTINCT CASE WHEN gs.financial_year = @FinancialYear - 2 THEN gs.fd_id END) AS farmer_count_fyb2,
                COUNT(DISTINCT CASE WHEN gs.financial_year = @FinancialYear - 3 THEN gs.fd_id END) AS farmer_count_fyb1,
                COUNT(DISTINCT CASE WHEN gs.financial_year = @FinancialYear THEN gs.equipment_id END) AS equipment_count_fyb4,
                COUNT(DISTINCT CASE WHEN gs.financial_year = @FinancialYear - 1 THEN gs.equipment_id END) AS equipment_count_fyb3,
                COUNT(DISTINCT CASE WHEN gs.financial_year = @FinancialYear - 2 THEN gs.equipment_id END) AS equipment_count_fyb2,
                COUNT(DISTINCT CASE WHEN gs.financial_year = @FinancialYear - 3 THEN gs.equipment_id END) AS equipment_count_fyb1,
                SUM(CASE WHEN gs.financial_year = @FinancialYear THEN CASE WHEN gs.unit = 4 THEN gs.area / 2.471 WHEN gs.unit = 6 THEN gs.area / 10000 ELSE gs.area END END) AS area_covered_fyb4,
                SUM(CASE WHEN gs.financial_year = @FinancialYear - 1 THEN CASE WHEN gs.unit = 4 THEN gs.area / 2.471 WHEN gs.unit = 6 THEN gs.area / 10000 ELSE gs.area END END) AS area_covered_fyb3,
                SUM(CASE WHEN gs.financial_year = @FinancialYear - 2 THEN CASE WHEN gs.unit = 4 THEN gs.area / 2.471 WHEN gs.unit = 6 THEN gs.area / 10000 ELSE gs.area END END) AS area_covered_fyb2,
                SUM(CASE WHEN gs.financial_year = @FinancialYear - 3 THEN CASE WHEN gs.unit = 4 THEN gs.area / 2.471 WHEN gs.unit = 6 THEN gs.area / 10000 ELSE gs.area END END) AS area_covered_fyb1,
                SUM(CASE WHEN gs.financial_year = @FinancialYear THEN gs.cash END) AS cash_disbursed_fyb4,
                SUM(CASE WHEN gs.financial_year = @FinancialYear - 1 THEN gs.cash END) AS cash_disbursed_fyb3,
                SUM(CASE WHEN gs.financial_year = @FinancialYear - 2 THEN gs.cash END) AS cash_disbursed_fyb2,
                SUM(CASE WHEN gs.financial_year = @FinancialYear - 3 THEN gs.cash END) AS cash_disbursed_fyb1,
                fd.village_code
            FROM equipment_details_govt_schemes gs
            INNER
            JOIN farmer_details fd ON fd.farmer_id = gs.fd_id
            GROUP BY fd.village_code ), target_summary AS (
            SELECT 
                t.user_id,
                SUM(t.target_fyb4) AS target_fyb4,
                SUM(t.target_fyb3) AS target_fyb3,
                SUM(t.target_fyb2) AS target_fyb2,
                SUM(t.target_fyb1) AS target_fyb1
            FROM (
            SELECT 
                r.user_id,
                IF(r.financial_year = @FinancialYear, r.target_count, 0) AS target_fyb4,
                IF(r.financial_year = @FinancialYear - 1, r.target_count, 0) AS target_fyb3,
                IF(r.financial_year = @FinancialYear - 2, r.target_count, 0) AS target_fyb2,
                IF(r.financial_year = @FinancialYear - 3, r.target_count, 0) AS target_fyb1
            FROM reg_target r ) t
            GROUP BY t.user_id )
            SELECT 
                IFNULL(ts.target_fyb4, 0) AS target_fyb4,
                SUM(IFNULL(ss.farmer_count_fyb4, 0)) AS total_farmer_count_fyb4,
                SUM(IFNULL(ss.equipment_count_fyb4, 0)) AS total_application_count_fyb4,
                TRUNCATE(SUM(IFNULL(ss.area_covered_fyb4, 0)), 3) AS total_area_covered_fyb4,
                SUM(IFNULL(ss.cash_disbursed_fyb4, 0)) AS total_cash_disbursed_fyb4,
                IFNULL(ts.target_fyb3, 0) AS target_fyb3,
                SUM(IFNULL(ss.farmer_count_fyb3, 0)) AS total_farmer_count_fyb3,
                SUM(IFNULL(ss.equipment_count_fyb3, 0)) AS total_application_count_fyb3,
                TRUNCATE(SUM(IFNULL(ss.area_covered_fyb3, 0)), 3) AS total_area_covered_fyb3,
                SUM(IFNULL(ss.cash_disbursed_fyb3, 0)) AS total_cash_disbursed_fyb3,
                IFNULL(ts.target_fyb2, 0) AS target_fyb2,
                SUM(IFNULL(ss.farmer_count_fyb2, 0)) AS total_farmer_count_fyb2,
                SUM(IFNULL(ss.equipment_count_fyb2, 0)) AS total_application_count_fyb2,
                TRUNCATE(SUM(IFNULL(ss.area_covered_fyb2, 0)), 3) AS total_area_covered_fyb2,
                SUM(IFNULL(ss.cash_disbursed_fyb2, 0)) AS total_cash_disbursed_fyb2,
                IFNULL(ts.target_fyb1, 0) AS target_fyb1,
                SUM(IFNULL(ss.farmer_count_fyb1, 0)) AS total_farmer_count_fyb1,
                SUM(IFNULL(ss.equipment_count_fyb1, 0)) AS total_application_count_fyb1,
                TRUNCATE(SUM(IFNULL(ss.area_covered_fyb1, 0)), 3) AS total_area_covered_fyb1,
                SUM(IFNULL(ss.cash_disbursed_fyb1, 0)) AS total_cash_disbursed_fyb1,
                va.DistCodeCensus AS district_code,
                rd.DistrictNameHindi AS district_name_hi,
                rd.DistrictName AS district_name_en
            FROM view_all_villages va
            INNER
            JOIN rev_district rd ON rd.DistrictCensus = va.DistCodeCensus
            INNER
            JOIN relevant_villages rv ON rv.village_code = va.village_code
            LEFT
            JOIN scheme_summary ss ON ss.village_code = rv.village_code
            LEFT
            JOIN target_summary ts ON ts.user_id = rd.DistrictCensus
            GROUP BY va.DistCodeCensus
            ORDER BY va.DistrictName";

            var result = await connection.QueryAsync<DistWiseFarmerRegistrationDto>(Sql, new { FinancialYear = financialYear });
            return Result<List<DistWiseFarmerRegistrationDto>>.Success(result.ToList());
        }

        public async Task<Result<List<BlockWiseFarmerRegistrationDto>>> GetBacklogRptOfBlockwiseFarmerRegistrationAsync(int districtCode, int financialYear)
        {
            using var connection = connectionFactory.CreateConnection();
            string Sql = @"
            WITH relevant_villages AS (
                SELECT DISTINCT
                    v.village_code
                FROM view_all_villages v
                WHERE v.DistCodeCensus = @DistrictCode ), farmer_details AS (
                SELECT DISTINCT
                    fd.fd_id AS farmer_id,
                    fd.hf_id AS horti_farmer_id,
                    fd.rheo_id AS rheo_id,
                    fd.village_code
                FROM farmer_detail_horti fd
                INNER
                JOIN mas_farmer_horti mf ON mf.hf_id = fd.hf_id
                INNER
                JOIN relevant_villages rv ON rv.village_code = fd.village_code ), scheme_summary AS (
                SELECT 
                    COUNT(DISTINCT CASE WHEN gs.financial_year = @FinancialYear THEN gs.fd_id END) AS farmer_count_fyb4,
                    COUNT(DISTINCT CASE WHEN gs.financial_year = @FinancialYear - 1 THEN gs.fd_id END) AS farmer_count_fyb3,
                    COUNT(DISTINCT CASE WHEN gs.financial_year = @FinancialYear - 2 THEN gs.fd_id END) AS farmer_count_fyb2,
                    COUNT(DISTINCT CASE WHEN gs.financial_year = @FinancialYear - 3 THEN gs.fd_id END) AS farmer_count_fyb1,
                    COUNT(DISTINCT CASE WHEN gs.financial_year = @FinancialYear THEN gs.equipment_id END) AS equipment_count_fyb4,
                    COUNT(DISTINCT CASE WHEN gs.financial_year = @FinancialYear - 1 THEN gs.equipment_id END) AS equipment_count_fyb3,
                    COUNT(DISTINCT CASE WHEN gs.financial_year = @FinancialYear - 2 THEN gs.equipment_id END) AS equipment_count_fyb2,
                    COUNT(DISTINCT CASE WHEN gs.financial_year = @FinancialYear - 3 THEN gs.equipment_id END) AS equipment_count_fyb1,
                    SUM(CASE WHEN gs.financial_year = @FinancialYear THEN CASE WHEN gs.unit = 4 THEN gs.area / 2.471 WHEN gs.unit = 6 THEN gs.area / 10000 ELSE gs.area END END) AS area_covered_fyb4,
                    SUM(CASE WHEN gs.financial_year = @FinancialYear - 1 THEN CASE WHEN gs.unit = 4 THEN gs.area / 2.471 WHEN gs.unit = 6 THEN gs.area / 10000 ELSE gs.area END END) AS area_covered_fyb3,
                    SUM(CASE WHEN gs.financial_year = @FinancialYear - 2 THEN CASE WHEN gs.unit = 4 THEN gs.area / 2.471 WHEN gs.unit = 6 THEN gs.area / 10000 ELSE gs.area END END) AS area_covered_fyb2,
                    SUM(CASE WHEN gs.financial_year = @FinancialYear - 3 THEN CASE WHEN gs.unit = 4 THEN gs.area / 2.471 WHEN gs.unit = 6 THEN gs.area / 10000 ELSE gs.area END END) AS area_covered_fyb1,
                    SUM(CASE WHEN gs.financial_year = @FinancialYear THEN gs.cash END) AS cash_disbursed_fyb4,
                    SUM(CASE WHEN gs.financial_year = @FinancialYear - 1 THEN gs.cash END) AS cash_disbursed_fyb3,
                    SUM(CASE WHEN gs.financial_year = @FinancialYear - 2 THEN gs.cash END) AS cash_disbursed_fyb2,
                    SUM(CASE WHEN gs.financial_year = @FinancialYear - 3 THEN gs.cash END) AS cash_disbursed_fyb1,
                    fd.village_code
                FROM equipment_details_govt_schemes gs
                INNER
                JOIN farmer_details fd ON fd.farmer_id = gs.fd_id
                GROUP BY fd.village_code )
                SELECT 
                    SUM(IFNULL(ss.farmer_count_fyb4, 0)) AS total_farmer_count_fyb4,
                    SUM(IFNULL(ss.equipment_count_fyb4, 0)) AS total_application_count_fyb4,
                    TRUNCATE(SUM(IFNULL(ss.area_covered_fyb4, 0)), 3) AS total_area_covered_fyb4,
                    SUM(IFNULL(ss.cash_disbursed_fyb4, 0)) AS total_cash_disbursed_fyb4,
                    SUM(IFNULL(ss.farmer_count_fyb3, 0)) AS total_farmer_count_fyb3,
                    SUM(IFNULL(ss.equipment_count_fyb3, 0)) AS total_application_count_fyb3,
                    TRUNCATE(SUM(IFNULL(ss.area_covered_fyb3, 0)), 3) AS total_area_covered_fyb3,
                    SUM(IFNULL(ss.cash_disbursed_fyb3, 0)) AS total_cash_disbursed_fyb3,
                    SUM(IFNULL(ss.farmer_count_fyb2, 0)) AS total_farmer_count_fyb2,
                    SUM(IFNULL(ss.equipment_count_fyb2, 0)) AS total_application_count_fyb2,
                    TRUNCATE(SUM(IFNULL(ss.area_covered_fyb2, 0)), 3) AS total_area_covered_fyb2,
                    SUM(IFNULL(ss.cash_disbursed_fyb2, 0)) AS total_cash_disbursed_fyb2,
                    SUM(IFNULL(ss.farmer_count_fyb1, 0)) AS total_farmer_count_fyb1,
                    SUM(IFNULL(ss.equipment_count_fyb1, 0)) AS total_application_count_fyb1,
                    TRUNCATE(SUM(IFNULL(ss.area_covered_fyb1, 0)), 3) AS total_area_covered_fyb1,
                    SUM(IFNULL(ss.cash_disbursed_fyb1, 0)) AS total_cash_disbursed_fyb1,
                    va.subdistrict_code AS subdistrict_code,
                    rb.BlockNameHin AS subdistrict_name_hi,
                    rb.BlockNameEng AS subdistrict_name_en,
                    va.DistCodeCensus AS district_code,
                    rd.DistrictNameHindi AS district_name_hi,
                    va.DistrictName AS district_name_en
                FROM view_all_villages va
                INNER
                JOIN rev_block rb ON rb.subdistrict_code = va.subdistrict_code
                INNER
                JOIN rev_district rd ON rd.DistrictCensus = va.distCodeCensus
                INNER
                JOIN relevant_villages rv ON rv.village_code = va.village_code
                LEFT
                JOIN scheme_summary ss ON ss.village_code = rv.village_code
                GROUP BY va.subdistrict_code
                ORDER BY rb.BlockNameEng;";

            var result = await connection.QueryAsync<BlockWiseFarmerRegistrationDto>(Sql, new { DistrictCode = districtCode, FinancialYear = financialYear });
            return Result<List<BlockWiseFarmerRegistrationDto>>.Success(result.ToList());
        }

        public async Task<Result<List<RheoWiseFarmerRegistrationDto>>> GetBacklogRptOfRheowiseFarmerRegistrationAsync(RheoWiseFarmerFilterDto filter)
        {
            using var connection = connectionFactory.CreateConnection();

            const string sql = """
            WITH villages AS
            (
                SELECT
                    DISTINCT
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

            scheme_summary AS
            (
                SELECT
                    COUNT(DISTINCT CASE WHEN gs.financial_year = @FinancialYear THEN gs.fd_id END) AS farmer_count_fyb4,
                    COUNT(DISTINCT CASE WHEN gs.financial_year = @FinancialYear - 1 THEN gs.fd_id END) AS farmer_count_fyb3,
                    COUNT(DISTINCT CASE WHEN gs.financial_year = @FinancialYear - 2 THEN gs.fd_id END) AS farmer_count_fyb2,
                    COUNT(DISTINCT CASE WHEN gs.financial_year = @FinancialYear - 3 THEN gs.fd_id END) AS farmer_count_fyb1,

                    COUNT(DISTINCT CASE WHEN gs.financial_year = @FinancialYear THEN gs.equipment_id END) AS equipment_count_fyb4,
                    COUNT(DISTINCT CASE WHEN gs.financial_year = @FinancialYear - 1 THEN gs.equipment_id END) AS equipment_count_fyb3,
                    COUNT(DISTINCT CASE WHEN gs.financial_year = @FinancialYear - 2 THEN gs.equipment_id END) AS equipment_count_fyb2,
                    COUNT(DISTINCT CASE WHEN gs.financial_year = @FinancialYear - 3 THEN gs.equipment_id END) AS equipment_count_fyb1,

                    SUM(CASE WHEN gs.financial_year = @FinancialYear THEN
                        CASE WHEN gs.unit = 4 THEN gs.area / 2.471
                             WHEN gs.unit = 6 THEN gs.area / 10000
                             ELSE gs.area END
                    END) AS area_covered_fyb4,
                    SUM(CASE WHEN gs.financial_year = @FinancialYear - 1 THEN
                        CASE WHEN gs.unit = 4 THEN gs.area / 2.471
                             WHEN gs.unit = 6 THEN gs.area / 10000
                             ELSE gs.area END
                    END) AS area_covered_fyb3,
                    SUM(CASE WHEN gs.financial_year = @FinancialYear - 2 THEN
                        CASE WHEN gs.unit = 4 THEN gs.area / 2.471
                             WHEN gs.unit = 6 THEN gs.area / 10000
                             ELSE gs.area END
                    END) AS area_covered_fyb2,
                    SUM(CASE WHEN gs.financial_year = @FinancialYear - 3 THEN
                        CASE WHEN gs.unit = 4 THEN gs.area / 2.471
                             WHEN gs.unit = 6 THEN gs.area / 10000
                             ELSE gs.area END
                    END) AS area_covered_fyb1,

                    SUM(CASE WHEN gs.financial_year = @FinancialYear THEN gs.cash END) AS cash_disbursed_fyb4,
                    SUM(CASE WHEN gs.financial_year = @FinancialYear - 1 THEN gs.cash END) AS cash_disbursed_fyb3,
                    SUM(CASE WHEN gs.financial_year = @FinancialYear - 2 THEN gs.cash END) AS cash_disbursed_fyb2,
                    SUM(CASE WHEN gs.financial_year = @FinancialYear - 3 THEN gs.cash END) AS cash_disbursed_fyb1,

                    fd.village_code
                FROM equipment_details_govt_schemes gs
                INNER JOIN farmer_details fd
                    ON fd.fd_id = gs.fd_id
                GROUP BY fd.village_code
            )

            SELECT
                SUM(IFNULL(ss.farmer_count_fyb4, 0)) AS TotalFarmerCountFyb4,
                SUM(IFNULL(ss.equipment_count_fyb4, 0)) AS TotalApplicationCountFyb4,
                TRUNCATE(SUM(IFNULL(ss.area_covered_fyb4, 0)), 3) AS TotalAreaCoveredFyb4,
                SUM(IFNULL(ss.cash_disbursed_fyb4, 0)) AS TotalCashDisbursedFyb4,

                SUM(IFNULL(ss.farmer_count_fyb3, 0)) AS TotalFarmerCountFyb3,
                SUM(IFNULL(ss.equipment_count_fyb3, 0)) AS TotalApplicationCountFyb3,
                TRUNCATE(SUM(IFNULL(ss.area_covered_fyb3, 0)), 3) AS TotalAreaCoveredFyb3,
                SUM(IFNULL(ss.cash_disbursed_fyb3, 0)) AS TotalCashDisbursedFyb3,

                SUM(IFNULL(ss.farmer_count_fyb2, 0)) AS TotalFarmerCountFyb2,
                SUM(IFNULL(ss.equipment_count_fyb2, 0)) AS TotalApplicationCountFyb2,
                TRUNCATE(SUM(IFNULL(ss.area_covered_fyb2, 0)), 3) AS TotalAreaCoveredFyb2,
                SUM(IFNULL(ss.cash_disbursed_fyb2, 0)) AS TotalCashDisbursedFyb2,

                SUM(IFNULL(ss.farmer_count_fyb1, 0)) AS TotalFarmerCountFyb1,
                SUM(IFNULL(ss.equipment_count_fyb1, 0)) AS TotalApplicationCountFyb1,
                TRUNCATE(SUM(IFNULL(ss.area_covered_fyb1, 0)), 3) AS TotalAreaCoveredFyb1,
                SUM(IFNULL(ss.cash_disbursed_fyb1, 0)) AS TotalCashDisbursedFyb1,

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
            LEFT JOIN scheme_summary ss
                ON ss.village_code = va.village_code

            GROUP BY
                va.officer_code,
                m.name,
                m.mobile_no,
                rb.BlockNameHin,
                rb.BlockNameEng,
                va.subdistrict_code,
                va.DistCodeCensus,
                rd.DistrictNameHindi,
                rd.DistrictName

            ORDER BY
                rd.DistrictName,
                rb.BlockNameEng,
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

        public async Task<Result<List<VillageWiseFarmerRegistrationDto>>> GetBacklogRptOfVillagewiseFarmerRegistrationAsync(int officerCode, int financialYear)
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

                               COUNT(DISTINCT CASE WHEN gs.financial_year = @FinancialYear THEN gs.equipment_id END) AS equipment_count_fyb4,
                               COUNT(DISTINCT CASE WHEN gs.financial_year = @FinancialYear - 1 THEN gs.equipment_id END) AS equipment_count_fyb3,
                               COUNT(DISTINCT CASE WHEN gs.financial_year = @FinancialYear - 2 THEN gs.equipment_id END) AS equipment_count_fyb2,
                               COUNT(DISTINCT CASE WHEN gs.financial_year = @FinancialYear - 3 THEN gs.equipment_id END) AS equipment_count_fyb1,

                               SUM(CASE WHEN gs.financial_year = @FinancialYear THEN
                                   CASE WHEN gs.unit = 4 THEN gs.area / 2.471
                                        WHEN gs.unit = 6 THEN gs.area / 10000
                                        ELSE gs.area END
                               END) AS area_covered_fyb4,
                               SUM(CASE WHEN gs.financial_year = @FinancialYear - 1 THEN
                                   CASE WHEN gs.unit = 4 THEN gs.area / 2.471
                                        WHEN gs.unit = 6 THEN gs.area / 10000
                                        ELSE gs.area END
                               END) AS area_covered_fyb3,
                               SUM(CASE WHEN gs.financial_year = @FinancialYear - 2 THEN
                                   CASE WHEN gs.unit = 4 THEN gs.area / 2.471
                                        WHEN gs.unit = 6 THEN gs.area / 10000
                                        ELSE gs.area END
                               END) AS area_covered_fyb2,
                               SUM(CASE WHEN gs.financial_year = @FinancialYear - 3 THEN
                                   CASE WHEN gs.unit = 4 THEN gs.area / 2.471
                                        WHEN gs.unit = 6 THEN gs.area / 10000
                                        ELSE gs.area END
                               END) AS area_covered_fyb1,

                               SUM(CASE WHEN gs.financial_year = @FinancialYear THEN gs.cash END) AS cash_disbursed_fyb4,
                               SUM(CASE WHEN gs.financial_year = @FinancialYear - 1 THEN gs.cash END) AS cash_disbursed_fyb3,
                               SUM(CASE WHEN gs.financial_year = @FinancialYear - 2 THEN gs.cash END) AS cash_disbursed_fyb2,
                               SUM(CASE WHEN gs.financial_year = @FinancialYear - 3 THEN gs.cash END) AS cash_disbursed_fyb1,

                               fd.village_code
                           FROM equipment_details_govt_schemes gs
                           INNER JOIN farmer_details fd
                               ON fd.fd_id = gs.fd_id
                           GROUP BY fd.village_code
                       )

                       SELECT
                           SUM(IFNULL(ss.farmer_count_fyb4, 0)) AS TotalFarmerCountFyb4,
                           SUM(IFNULL(ss.equipment_count_fyb4, 0)) AS TotalApplicationCountFyb4,
                           TRUNCATE(SUM(IFNULL(ss.area_covered_fyb4, 0)), 3) AS TotalAreaCoveredFyb4,
                           SUM(IFNULL(ss.cash_disbursed_fyb4, 0)) AS TotalCashDisbursedFyb4,

                           SUM(IFNULL(ss.farmer_count_fyb3, 0)) AS TotalFarmerCountFyb3,
                           SUM(IFNULL(ss.equipment_count_fyb3, 0)) AS TotalApplicationCountFyb3,
                           TRUNCATE(SUM(IFNULL(ss.area_covered_fyb3, 0)), 3) AS TotalAreaCoveredFyb3,
                           SUM(IFNULL(ss.cash_disbursed_fyb3, 0)) AS TotalCashDisbursedFyb3,

                           SUM(IFNULL(ss.farmer_count_fyb2, 0)) AS TotalFarmerCountFyb2,
                           SUM(IFNULL(ss.equipment_count_fyb2, 0)) AS TotalApplicationCountFyb2,
                           TRUNCATE(SUM(IFNULL(ss.area_covered_fyb2, 0)), 3) AS TotalAreaCoveredFyb2,
                           SUM(IFNULL(ss.cash_disbursed_fyb2, 0)) AS TotalCashDisbursedFyb2,

                           SUM(IFNULL(ss.farmer_count_fyb1, 0)) AS TotalFarmerCountFyb1,
                           SUM(IFNULL(ss.equipment_count_fyb1, 0)) AS TotalApplicationCountFyb1,
                           TRUNCATE(SUM(IFNULL(ss.area_covered_fyb1, 0)), 3) AS TotalAreaCoveredFyb1,
                           SUM(IFNULL(ss.cash_disbursed_fyb1, 0)) AS TotalCashDisbursedFyb1,

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

            var result = await connection.QueryAsync<VillageWiseFarmerRegistrationDto>(sql, new { OfficerCode = officerCode, FinancialYear = financialYear });
            return Result<List<VillageWiseFarmerRegistrationDto>>.Success(result.ToList());
        }

        public async Task<Result<List<FarmerWiseFarmerRegistrationDto>>> GetBacklogRptOfFarmerwiseFarmerRegistrationAsync(FarmerWiseReportFilterDto filter)
        {
            using var connection = connectionFactory.CreateConnection();


            const string sql = """
            WITH basic_detail AS
            (
                SELECT
                    mf.farmer_name_eng,
                    mf.farmer_name_hi,
                    mf.father_name,
                    mf.mobile_no,
                    mf.uf_id,
                    fd.fd_id,
                    fd.hf_id,
                    fd.rheo_id,
                    v.DistCodeCensus,
                    v.subdistrict_code,
                    fd.village_code,
                    v.village_name,

                    (
                        SELECT ovd2.officer_code
                        FROM officer_village_details ovd2
                        WHERE ovd2.village_code = v.village_code
                        AND (
                                @OfficerCode IS NULL
                                OR @OfficerCode = 0
                                OR ovd2.officer_code = @OfficerCode
                            )
                        LIMIT 1
                    ) AS officer_code

                FROM view_all_villages v

                INNER JOIN farmer_detail_horti fd
                    ON fd.village_code = v.village_code

                INNER JOIN mas_farmer_horti mf
                    ON mf.hf_id = fd.hf_id

                WHERE
                    (@DistrictCode IS NULL OR @DistrictCode = 0
                    OR v.DistCodeCensus = @DistrictCode)

                    AND
                    (@SubDistrictCode IS NULL OR @SubDistrictCode = 0
                    OR v.subdistrict_code = @SubDistrictCode)

                    AND
                    (@VillageCode IS NULL OR @VillageCode = 0
                    OR v.village_code = @VillageCode)

                    AND
                    (
                        @OfficerCode IS NULL
                        OR @OfficerCode = 0
                        OR EXISTS
                        (
                            SELECT 1
                            FROM officer_village_details ovd
                            INNER JOIN mas_raeo mr
                                ON mr.officer_code = ovd.officer_code
                                AND mr.usertype = @DepartmentCode
                            WHERE ovd.village_code = v.village_code
                            AND ovd.officer_code = @OfficerCode
                        )
                    )
            ),

            scheme_summary AS
            (
                SELECT
                    gs.fd_id,

                    COUNT(DISTINCT CASE WHEN gs.financial_year = @FinancialYear THEN gs.equipment_id END) AS equipment_count_fy13,
                    COUNT(DISTINCT CASE WHEN gs.financial_year = @FinancialYear - 1 THEN gs.equipment_id END) AS equipment_count_fy12,
                    COUNT(DISTINCT CASE WHEN gs.financial_year = @FinancialYear - 2 THEN gs.equipment_id END) AS equipment_count_fy11,
                    COUNT(DISTINCT CASE WHEN gs.financial_year = @FinancialYear - 3 THEN gs.equipment_id END) AS equipment_count_fy10,

                    SUM(CASE WHEN gs.financial_year = @FinancialYear THEN
                        CASE WHEN gs.unit = 4 THEN gs.area / 2.471
                             WHEN gs.unit = 6 THEN gs.area / 10000
                             ELSE gs.area END
                    END) AS area_covered_fy13,
                    SUM(CASE WHEN gs.financial_year = @FinancialYear - 1 THEN
                        CASE WHEN gs.unit = 4 THEN gs.area / 2.471
                             WHEN gs.unit = 6 THEN gs.area / 10000
                             ELSE gs.area END
                    END) AS area_covered_fy12,
                    SUM(CASE WHEN gs.financial_year = @FinancialYear - 2 THEN
                        CASE WHEN gs.unit = 4 THEN gs.area / 2.471
                             WHEN gs.unit = 6 THEN gs.area / 10000
                             ELSE gs.area END
                    END) AS area_covered_fy11,
                    SUM(CASE WHEN gs.financial_year = @FinancialYear - 3 THEN
                        CASE WHEN gs.unit = 4 THEN gs.area / 2.471
                             WHEN gs.unit = 6 THEN gs.area / 10000
                             ELSE gs.area END
                    END) AS area_covered_fy10,

                    SUM(CASE WHEN gs.financial_year = @FinancialYear THEN gs.cash END) AS cash_disbursed_fy13,
                    SUM(CASE WHEN gs.financial_year = @FinancialYear - 1 THEN gs.cash END) AS cash_disbursed_fy12,
                    SUM(CASE WHEN gs.financial_year = @FinancialYear - 2 THEN gs.cash END) AS cash_disbursed_fy11,
                    SUM(CASE WHEN gs.financial_year = @FinancialYear - 3 THEN gs.cash END) AS cash_disbursed_fy10

                FROM equipment_details_govt_schemes gs
                INNER JOIN basic_detail bd
                    ON bd.fd_id = gs.fd_id

                WHERE gs.financial_year IN (@FinancialYear, @FinancialYear - 1, @FinancialYear - 2, @FinancialYear - 3)

                AND (
                    @SchemeType IS NULL
                    OR @SchemeType = 0
                    OR gs.scheme_type = @SchemeType
                )
                AND (
                    @SchemeId IS NULL
                    OR @SchemeId = 0
                    OR gs.schcme_id = @SchemeId
                )
                AND (
                    @ComponentId IS NULL
                    OR @ComponentId = 0
                    OR gs.component_id = @ComponentId
                )

                GROUP BY gs.fd_id
            ),

            land_data AS
            (
                SELECT
                    GROUP_CONCAT(
                        ld.khasra_no,
                        '(',
                        ld.area,
                        ') '
                    ) AS total_land_khasra,

                    SUM(ld.area) AS total_land_area,
                    ld.fd_id

                FROM land_details_horti ld

                INNER JOIN basic_detail bd
                    ON bd.fd_id = ld.fd_id
                    AND bd.village_code = ld.village_code

                WHERE ld.financial_year = @FinancialYear

                GROUP BY ld.fd_id
            )

            SELECT
                bd.uf_id AS UFID,
                bd.fd_id AS FDID,
                bd.hf_id AS HFID,
                bd.officer_code AS OfficerCode,
                m.name AS OfficerName,
                bd.DistCodeCensus AS DistrictCode,
                rd.DistrictName AS DistrictNameEn,
                rd.DistrictNameHindi AS DistrictNameHi,
                bd.subdistrict_code AS SubDistrictCode,
                rb.BlockNameEng AS SubDistrictNameEn,
                rb.BlockNameHin AS SubDistrictNameHi,
                bd.village_code AS VillageCode,
                bd.village_name AS VillageName,
                bd.farmer_name_eng AS FarmerNameEng,
                bd.farmer_name_hi AS FarmerNameHi,
                bd.father_name AS FarmerFatherName,
                bd.mobile_no AS MobileNo,

                IFNULL(ss.equipment_count_fy13, 0) AS TotalApplicationCountFyb4,
                TRUNCATE(IFNULL(ss.area_covered_fy13, 0), 3) AS TotalAreaCoveredFyb4,
                IFNULL(ss.cash_disbursed_fy13, 0) AS TotalCashDisbursedFyb4,

                IFNULL(ss.equipment_count_fy12, 0) AS TotalApplicationCountFyb3,
                TRUNCATE(IFNULL(ss.area_covered_fy12, 0), 3) AS TotalAreaCoveredFyb3,
                IFNULL(ss.cash_disbursed_fy12, 0) AS TotalCashDisbursedFyb3,

                IFNULL(ss.equipment_count_fy11, 0) AS TotalApplicationCountFyb2,
                TRUNCATE(IFNULL(ss.area_covered_fy11, 0), 3) AS TotalAreaCoveredFyb2,
                IFNULL(ss.cash_disbursed_fy11, 0) AS TotalCashDisbursedFyb2,

                IFNULL(ss.equipment_count_fy10, 0) AS TotalApplicationCountFyb1,
                TRUNCATE(IFNULL(ss.area_covered_fy10, 0), 3) AS TotalAreaCoveredFyb1,
                IFNULL(ss.cash_disbursed_fy10, 0) AS TotalCashDisbursedFyb1,

                ld1.total_land_khasra AS TotalLandKhasra,
                ld1.total_land_area AS TotalLandArea

            FROM basic_detail bd

            INNER JOIN rev_block rb
                ON rb.subdistrict_code = bd.subdistrict_code

            INNER JOIN rev_district rd
                ON rd.DistrictCensus = bd.DistCodeCensus

            INNER JOIN mas_raeo m
                ON m.officer_code = bd.officer_code

            INNER JOIN scheme_summary ss
                ON ss.fd_id = bd.fd_id

            LEFT JOIN land_data ld1
                ON ld1.fd_id = bd.fd_id

            WHERE ss.fd_id IS NOT NULL

            ORDER BY
                bd.village_name,
                bd.farmer_name_eng;
            """;

            var parameters = new
            {
                DepartmentCode = filter.DepartmentCode,
                DistrictCode = filter.DistrictCode,
                SubDistrictCode = filter.SubDistrictCode,
                VillageCode = filter.VillageCode,
                OfficerCode = filter.OfficerCode,

                FinancialYear = filter.FinancialYear,

                SchemeType = filter.SchemeType,
                SchemeId = filter.SchemeId,
                ComponentId = filter.ComponentId
            };

            var result = await connection.QueryAsync<FarmerWiseFarmerRegistrationDto>(sql, parameters);
            return Result<List<FarmerWiseFarmerRegistrationDto>>.Success(result.ToList());
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
