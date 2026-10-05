using Dapper;
using HortiBts.Api.Data;
using HortiBts.Shared.Common;
using HortiBts.Shared.Dtos.ApplicationVerification.DDH;

namespace HortiBts.Api.Repositories.Reports.HPMIS;

public interface ISchemeTypeWiseFarmerOldVerificationRepository
{
    Task<Result<List<SchemeTypeWiseOldBeneficiaryApplicationDto>>> GetSchemeWiseForDashboardAsync(
        SchemeWiseDashboardQueryParams query);
}

public class SchemeWiseDashboardQueryParams
{
    public string SearchFlag { get; set; } = string.Empty;
    public int FinancialYear { get; set; }
    public int SchemeTypeId { get; set; }
    public int? Id { get; set; }
}

public class SchemeTypeWiseFarmerOldVerificationRepository(
    IDbConnectionFactory connectionFactory)
    : ISchemeTypeWiseFarmerOldVerificationRepository
{
    public async Task<Result<List<SchemeTypeWiseOldBeneficiaryApplicationDto>>> GetSchemeWiseForDashboardAsync(
        SchemeWiseDashboardQueryParams query)
    {
        using var connection =
            connectionFactory.CreateConnection(HortiDb.Hpmis);

        var parameters = new DynamicParameters();

        parameters.Add("FinancialYear", query.FinancialYear);
        parameters.Add("SchemeTypeId", query.SchemeTypeId);

        string subQuery;

        switch (query.SearchFlag)
        {
            case "DIST":

                if (query.Id == 0)
                {
                    subQuery = @"
                        SELECT v.village_code
                        FROM view_all_villages v";
                }
                else
                {
                    subQuery = @"
                        SELECT v.village_code
                        FROM view_all_villages v
                        WHERE v.DistCodeCensus = @Id";

                    parameters.Add("Id", query.Id);
                }

                break;

            case "SUBDIST":

                if (query.Id == 0)
                {
                    subQuery = @"
                        SELECT v.village_code
                        FROM view_all_villages v
                        WHERE v.DistCodeCensus = @Id";

                    parameters.Add("Id", query.Id);
                }
                else
                {
                    subQuery = @"
                        SELECT v.village_code
                        FROM view_all_villages v
                        WHERE v.subdistrict_code = @Id";

                    parameters.Add("Id", query.Id);
                }

                break;

            case "OFFICER":

                if (query.Id == 0)
                {
                    subQuery = @"
                        SELECT v.village_code
                        FROM view_all_villages v
                        WHERE v.subdistrict_code = @Id";

                    parameters.Add("Id", query.Id);
                }
                else
                {
                    subQuery = @"
                        SELECT v.village_code
                        FROM officer_village_details v
                        WHERE v.officer_code = @Id";

                    parameters.Add("Id", query.Id);
                }

                break;

            case "VILL":

                if (query.Id == 0)
                {
                    subQuery = @"
                        SELECT v.village_code
                        FROM officer_village_details v
                        WHERE v.officer_code = @Id";

                    parameters.Add("Id", query.Id);
                }
                else
                {
                    subQuery = @"
                        SELECT v.village_code
                        FROM view_all_villages v
                        WHERE v.village_code = @Id";

                    parameters.Add("Id", query.Id);
                }

                break;

            default:

                subQuery = @"
                    SELECT v.village_code
                    FROM view_all_villages v";

                break;
        }

        var sql = $@"
            WITH villages AS
            (
                {subQuery}
            ),

            scheme_type AS
            (
                SELECT
                    m.s_id AS st_id,
                    m.scheme_name AS scheme_type
                FROM mas_scheme_horti m
                WHERE m.st_id = 0
                  AND m.flag = 'Y'
            ),

            scheme_details AS
            (
                SELECT
                    COUNT(DISTINCT gs.sd_id) AS f_count,
                    gs.scheme_id
                FROM app_scheme_details gs

                INNER JOIN app_farmer_details fd
                    ON fd.fd_id = gs.fd_id

                INNER JOIN app_mas_farmer mf
                    ON mf.hf_id = fd.hf_id

                INNER JOIN app_land_details l
                    ON l.fd_id = fd.fd_id

                INNER JOIN villages v
                    ON v.village_code = fd.village_code

                WHERE gs.financial_year = @FinancialYear
                  AND gs.scheme_type = @SchemeTypeId

                GROUP BY gs.scheme_id
            ),

            appr_farmer AS
            (
                SELECT
                    COUNT(DISTINCT gs.sd_id) AS f_count,
                    gs.scheme_id
                FROM app_scheme_details gs

                INNER JOIN app_farmer_details fd
                    ON fd.fd_id = gs.fd_id

                INNER JOIN villages v
                    ON v.village_code = fd.village_code

                WHERE gs.financial_year = @FinancialYear
                  AND gs.scheme_type = @SchemeTypeId
                  AND gs.status IN ('S', 'A')

                GROUP BY gs.scheme_id
            )

            SELECT
                ms.s_id AS SId,
                ms.scheme_name AS SchemeName,
                mt.scheme_type AS SchemeType,
                mt.st_id AS StId,
                IFNULL(sd.f_count, 0) AS FCount,
                IFNULL(af.f_count, 0) AS AfCount

            FROM mas_scheme_horti ms

            INNER JOIN scheme_type mt
                ON mt.st_id = ms.st_id

            LEFT JOIN appr_farmer af
                ON af.scheme_id = ms.s_id

            LEFT JOIN scheme_details sd
                ON ms.s_id = sd.scheme_id

            WHERE ms.st_id <> 0
              AND ms.flag = 'Y'
              AND ms.st_id = @SchemeTypeId

            GROUP BY ms.s_id

            ORDER BY ms.scheme_name;";

        var result =
            await connection.QueryAsync<SchemeTypeWiseOldBeneficiaryApplicationDto>(
                sql,
                parameters);

        return Result<List<SchemeTypeWiseOldBeneficiaryApplicationDto>>
            .Success(result.ToList());
    }
}