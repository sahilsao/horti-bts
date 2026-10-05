using System;
using Dapper;
using HortiBts.Api.Data;
using HortiBts.Shared.Common;
using HortiBts.Shared.Dtos.Reports.HPMIS;

namespace HortiBts.Api.Repositories.Reports.HPMIS;

public interface IFarmersListHPMISRepository
{
    Task<Result<List<FarmersListHPMISDto>>> GetFarmerListByIdAsync(FarmerListQueryParams query);
}

public class FarmerListQueryParams
{
    public string SearchFlag { get; set; } = string.Empty; // DIST, SUBDIST, OFFICER, VILL, FARMER
    public int FinancialYear { get; set; }
    public int? Id { get; set; }
}

public class FarmersListHPMISRepository(IDbConnectionFactory connectionFactory) : IFarmersListHPMISRepository
{
    public async Task<Result<List<FarmersListHPMISDto>>> GetFarmerListByIdAsync(FarmerListQueryParams query)
    {
        using var connection = connectionFactory.CreateConnection(HortiDb.Hpmis);

        var parameters = new DynamicParameters();
        parameters.Add("FinancialYear", query.FinancialYear);
        parameters.Add("IsApproved", "P");

        string subQuery;

        switch (query.SearchFlag)
        {
            case "DIST":
                if (query.Id == 100)
                {
                    subQuery = @"
                        INNER JOIN mas_villages v ON v.vsr_census = cd.village_code
                        WHERE fa.is_approved = @IsApproved
                          AND fa.is_land_entered = 1
                          AND fa.is_crop_entered = 1
                          AND fa.is_market_linkage_entered = 1";
                }
                else
                {
                    subQuery = @"
                        INNER JOIN mas_villages v ON v.vsr_census = cd.village_code
                        WHERE fa.is_approved = @IsApproved
                          AND v.distcode_census = @Id
                          AND fa.is_land_entered = 1
                          AND fa.is_crop_entered = 1
                          AND fa.is_market_linkage_entered = 1";
                    parameters.Add("Id", query.Id);
                }
                break;

            case "SUBDIST":
                subQuery = @"
                    INNER JOIN mas_villages v ON v.vsr_census = cd.village_code
                    WHERE fa.is_approved = @IsApproved
                      AND v.subdistrictcode_census = @Id
                      AND fa.is_land_entered = 1
                      AND fa.is_crop_entered = 1
                      AND fa.is_market_linkage_entered = 1";
                parameters.Add("Id", query.Id);
                break;

            case "OFFICER":
                subQuery = @"
                    INNER JOIN mas_villages v ON v.vsr_census = cd.village_code
                    INNER JOIN zone_village_mapping z ON z.village_code = cd.village_code
                    INNER JOIN user_zone_papping u ON u.zone_id = z.zone_id
                    WHERE fa.is_approved = @IsApproved
                      AND u.user_id = @Id
                      AND fa.is_land_entered = 1
                      AND fa.is_crop_entered = 1
                      AND fa.is_market_linkage_entered = 1";
                parameters.Add("Id", query.Id);
                break;

            case "VILL":
                subQuery = @"
                    INNER JOIN mas_villages v ON v.vsr_census = cd.village_code
                    WHERE fa.is_approved = @IsApproved
                      AND v.vsr_census = @Id
                      AND fa.is_land_entered = 1
                      AND fa.is_crop_entered = 1
                      AND fa.is_market_linkage_entered = 1";
                parameters.Add("Id", query.Id);
                break;

            case "FARMER":
                subQuery = @"
                    INNER JOIN mas_villages v ON v.vsr_census = cd.village_code
                    WHERE fa.is_approved = @IsApproved
                      AND f.user_id = @Id
                      AND fa.is_land_entered = 1
                      AND fa.is_crop_entered = 1
                      AND fa.is_market_linkage_entered = 1";
                parameters.Add("Id", query.Id);
                break;

            default:
                subQuery = string.Empty;
                break;
        }

        var sql = $@"
            SELECT fa.application_id, f.f_id, f.uf_id, f.user_id, f.farmer_name_eng, f.farmer_name_hi,
                   f.care_of_name, r.rel_name, cc.caste_name, cc.subcaste_name, g.gender_name_hi, f.mobile_no,
                   v.villcdname AS village_name, ld.village_code, ld.total_area, cd.crop_name, cd.crop_area,
                   cd.season_name_hi, cd.expected_production, cd.financial_year
            FROM farmer_application fa
                INNER JOIN farmer f ON f.f_id = fa.f_id
                INNER JOIN mas_relation r ON r.rel_id = f.relation
                INNER JOIN mas_gender g ON g.gender_id = f.gender
                INNER JOIN mas_caste cc ON cc.subcaste_code = f.subcategory
                INNER JOIN (
                    SELECT l.application_id, l.f_id, l.village_code, SUM(l.land_area) AS total_area
                    FROM land_details l
                    GROUP BY l.application_id, l.f_id
                    LIMIT 1000
                ) ld ON ld.application_id = fa.application_id
                INNER JOIN (
                    SELECT c.application_id, c.f_id, c.village_code, c.crop_code, c.crop_variety, c.crop_season,
                           SUM(c.crop_area) AS crop_area, SUM(c.expected_production) AS expected_production,
                           c.financial_year,
                           GROUP_CONCAT(
                               CONCAT_WS(' ', mc.crop_name_hi, '-', mv.variety_name, '(', cc.category_name_hi, ')', c.crop_area)
                               ORDER BY mc.crop_name_hi) AS crop_name,
                           ms.season_name_hi
                    FROM crop_details c
                        INNER JOIN mas_crop mc ON mc.crop_code = c.crop_code
                        INNER JOIN mas_crop_category cc ON cc.category = c.crop_category
                        INNER JOIN mas_season ms ON ms.season_id = c.crop_season
                        INNER JOIN mas_variety mv ON mv.variety_code = c.crop_variety
                    WHERE c.financial_year = @FinancialYear
                    GROUP BY c.application_id, c.f_id
                    LIMIT 1000
                ) cd ON cd.application_id = fa.application_id
            {subQuery}";
        var result = await connection.QueryAsync<FarmersListHPMISDto>(sql, parameters);
        return Result<List<FarmersListHPMISDto>>.Success(result.ToList());
    }
}
