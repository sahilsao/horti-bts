using Dapper;
using HortiBts.Api.Data;
using HortiBts.Shared.Common;
using HortiBts.Shared.Dtos.Reports.Scheme;

namespace HortiBts.Api.Repositories.Reports.Scheme
{
    public interface IBacklogYearlySchemeWiseRegistrationRepository
    {
        Task<Result<List<SchemeWiseRegistrationDto>>> GetYearlyRptOfSchemeWiseFarmerRegistrationAsync(SchemeWiseReportFilterDto filter);
    }
    public class BacklogYearlySchemeWiseRegistrationRepository(IDbConnectionFactory connectionFactory) : IBacklogYearlySchemeWiseRegistrationRepository
    {
        public async Task<Result<List<SchemeWiseRegistrationDto>>> GetYearlyRptOfSchemeWiseFarmerRegistrationAsync(SchemeWiseReportFilterDto filter)
        {
            using var connection = connectionFactory.CreateConnection();

            const string sql = @"
            WITH villages AS (
    SELECT v.village_code
    FROM view_all_villages v
    WHERE (@OfficerCode IS NULL OR @OfficerCode = 0)
      AND (@DistrictCode IS NULL OR @DistrictCode = 0 OR v.DistCodeCensus = @DistrictCode)
      AND (@SubDistrictCode IS NULL OR @SubDistrictCode = 0 OR v.subdistrict_code = @SubDistrictCode)
      AND (@VillageCode IS NULL OR @VillageCode = 0 OR v.village_code = @VillageCode)

    UNION

    SELECT ov.village_code
    FROM officer_village_details ov
    WHERE @OfficerCode IS NOT NULL
      AND @OfficerCode <> 0
      AND ov.officer_code = @OfficerCode
),
scheme_type AS (
    SELECT m.s_id AS st_id, m.scheme_name AS scheme_type
    FROM mas_scheme_horti m
    WHERE m.st_id = 0 AND m.flag = 'Y'
),
scheme_details AS (
    SELECT
        gs.schcme_id,
        COUNT(DISTINCT CASE WHEN gs.financial_year = @FinancialYear     THEN gs.fd_id END) AS f_13,
        COUNT(DISTINCT CASE WHEN gs.financial_year = @FinancialYear - 1 THEN gs.fd_id END) AS f_12,
        COUNT(DISTINCT CASE WHEN gs.financial_year = @FinancialYear - 2 THEN gs.fd_id END) AS f_11,
        COUNT(DISTINCT CASE WHEN gs.financial_year = @FinancialYear - 3 THEN gs.fd_id END) AS f_10
    FROM equipment_details_govt_schemes gs
        INNER JOIN farmer_detail_horti fd ON fd.fd_id = gs.fd_id
        INNER JOIN villages v ON v.village_code = fd.village_code
    WHERE gs.financial_year BETWEEN @FinancialYear - 3 AND @FinancialYear
    GROUP BY gs.schcme_id
)
SELECT
    ms.s_id AS SId,
    ms.scheme_name AS SchemeName,
    mt.scheme_type AS SchemeType,
    mt.st_id AS StId,
    IFNULL(sd.f_13, 0) AS F13,
    IFNULL(sd.f_12, 0) AS F12,
    IFNULL(sd.f_11, 0) AS F11,
    IFNULL(sd.f_10, 0) AS F10
FROM mas_scheme_horti ms
    INNER JOIN scheme_type mt ON mt.st_id = ms.st_id
    LEFT JOIN scheme_details sd ON ms.s_id = sd.schcme_id
WHERE ms.st_id <> 0 AND ms.flag = 'Y'
ORDER BY ms.scheme_name";

            var parameters = new
            {
                DistrictCode = filter.DistrictCode,
                SubDistrictCode = filter.SubDistrictCode,
                VillageCode = filter.VillageCode,
                OfficerCode = filter.OfficerCode,
                FinancialYear = filter.FinancialYear
            };

            var result = await connection.QueryAsync<SchemeWiseRegistrationDto>(
                sql,
                parameters);

            return Result<List<SchemeWiseRegistrationDto>>.Success(result.ToList());
        }
    }

    public class SchemeWiseReportFilterDto
    {
        public int? DistrictCode { get; set; }
        public int? SubDistrictCode { get; set; }
        public int? VillageCode { get; set; }
        public int? OfficerCode { get; set; }
        public int FinancialYear { get; set; }
    }
}


