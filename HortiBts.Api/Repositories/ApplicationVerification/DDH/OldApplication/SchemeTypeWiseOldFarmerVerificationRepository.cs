using Dapper;
using HortiBts.Api.Data;
using HortiBts.Shared.Common;
using HortiBts.Shared.Dtos.ApplicationVerification.DDH.OldApplication;

namespace HortiBts.Api.Repositories.ApplicationVerification.DDH.OldApplication;

public interface ISchemeTypeWiseOldFarmerVerificationRepository
{
    Task<Result<List<SchemeTypeWiseOldApplicationsListDto>>> GetSchemeTypeWiseOldFarmerVerificationAsync(
        int districtCode,
        int subDistrictCode,
        int villageCode,
        int officerCode,
        int schemeTypeId,
        int financialYear);
}

public class SchemeTypeWiseOldFarmerVerificationRepository(
    IDbConnectionFactory connectionFactory)
    : ISchemeTypeWiseOldFarmerVerificationRepository
{
    public async Task<Result<List<SchemeTypeWiseOldApplicationsListDto>>> GetSchemeTypeWiseOldFarmerVerificationAsync(
        int districtCode,
        int subDistrictCode,
        int villageCode,
        int officerCode,
        int schemeTypeId,
        int financialYear)
    {
        using var connection =
            connectionFactory.CreateConnection();

        const string sql = @"
            WITH villages AS
            (
                SELECT
                    v.village_code
                FROM view_all_villages v
                WHERE
                    (@DistrictCode IS NULL
                        OR @DistrictCode = 0
                        OR v.DistCodeCensus = @DistrictCode)

                    AND

                    (@SubDistrictCode IS NULL
                        OR @SubDistrictCode = 0
                        OR v.subdistrict_code = @SubDistrictCode)

                    AND

                    (@VillageCode IS NULL
                        OR @VillageCode = 0
                        OR v.village_code = @VillageCode)
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

                LEFT JOIN officer_village_details ovd
                    ON ovd.village_code = fd.village_code

                LEFT JOIN mas_raeo mr
                    ON mr.officer_code = ovd.officer_code
                   AND mr.usertype = 3

                WHERE gs.financial_year = @FinancialYear
                  AND gs.scheme_type = @SchemeTypeId

                  AND
                  (
                      @OfficerCode IS NULL
                      OR @OfficerCode = 0
                      OR mr.officer_code = @OfficerCode
                  )

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

                LEFT JOIN officer_village_details ovd
                    ON ovd.village_code = fd.village_code

                LEFT JOIN mas_raeo mr
                    ON mr.officer_code = ovd.officer_code
                   AND mr.usertype = 3

                WHERE gs.financial_year = @FinancialYear
                  AND gs.scheme_type = @SchemeTypeId
                  AND gs.status IN ('S', 'A')

                  AND
                  (
                      @OfficerCode IS NULL
                      OR @OfficerCode = 0
                      OR mr.officer_code = @OfficerCode
                  )

                GROUP BY gs.scheme_id
            )

            SELECT
                ms.s_id AS SchemeId,
                ms.scheme_name AS SchemeName,
                mt.scheme_type AS SchemeType,
                mt.st_id AS SchemeTypeId,
                IFNULL(sd.f_count, 0) AS ApplicationCount,
                IFNULL(af.f_count, 0) AS ApprovedApplicationCount

            FROM mas_scheme_horti ms

            INNER JOIN scheme_type mt
                ON mt.st_id = ms.st_id

            LEFT JOIN appr_farmer af
                ON af.scheme_id = ms.s_id

            LEFT JOIN scheme_details sd
                ON sd.scheme_id = ms.s_id

            WHERE ms.st_id <> 0
              AND ms.flag = 'Y'
              AND ms.st_id = @SchemeTypeId

            GROUP BY
                ms.s_id,
                ms.scheme_name,
                mt.scheme_type,
                mt.st_id,
                sd.f_count,
                af.f_count

            ORDER BY ms.scheme_name;";

        var result =
            await connection.QueryAsync<SchemeTypeWiseOldApplicationsListDto>(
                sql,
                new
                {
                    DistrictCode = districtCode,
                    SubDistrictCode = subDistrictCode,
                    VillageCode = villageCode,
                    OfficerCode = officerCode,
                    SchemeTypeId = schemeTypeId,
                    FinancialYear = financialYear
                });

        return Result<List<SchemeTypeWiseOldApplicationsListDto>>
            .Success(result.ToList());
    }
}