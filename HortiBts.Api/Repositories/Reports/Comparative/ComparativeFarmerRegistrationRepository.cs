using Dapper;
using HortiBts.Api.Data;
using HortiBts.Shared.Common;
using HortiBts.Shared.Dtos.Reports.Comparative;
using Microsoft.AspNetCore.Connections;

namespace HortiBts.Api.Repositories.Reports.Comparative
{
    public interface IComparativeFarmerRegistrationRepository
    {
        Task<Result<List<DistwiseComparativeFarmerRegistrationDto>>> GetRptOfDistwiseComparativeFarmerRegistrationAsync(int financialYear);
    }
    public class ComparativeFarmerRegistrationRepository(IDbConnectionFactory connectionFactory) : IComparativeFarmerRegistrationRepository
    {
        public async Task<Result<List<DistwiseComparativeFarmerRegistrationDto>>> GetRptOfDistwiseComparativeFarmerRegistrationAsync(int financialYear)
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

            var result = await connection.QueryAsync<DistwiseComparativeFarmerRegistrationDto>(Sql, new { FinancialYear = financialYear });
            return Result<List<DistwiseComparativeFarmerRegistrationDto>>.Success(result.ToList());
        }
    }
}
