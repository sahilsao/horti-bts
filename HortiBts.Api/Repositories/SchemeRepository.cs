using Dapper;
using HortiBts.Shared.Dtos.Schemes;
using HortiBts.Api.Data;

namespace HortiBts.Api.Repositories
{
    public interface ISchemeRepository
    {
        Task<IEnumerable<SchemeDto>> GetByTypeAsync(int stId);
    }

    public class SchemeRepository(IDbConnectionFactory connectionFactory) : ISchemeRepository
    {
        private const string Sql = @"
        SELECT
            ms.s_id            AS SId,
            ms.scheme_name     AS SchemeName,
            ms.scheme_name_en  AS SchemeNameEn,
            ms.scheme_code     AS SchemeCode,
            st.s_id            AS SchemeTypeId,
            st.scheme_name     AS SchemeTypeHi,
            st.scheme_name_en  AS SchemeTypeEn,
            ms.description_hi  AS DescriptionHi,
            ms.description_en  AS DescriptionEn,
            ms.isbeneficiary   AS IsBeneficiary,
            ms.flag            AS Flag
        FROM mas_scheme_horti ms
        INNER JOIN mas_scheme_horti st ON st.s_id = ms.st_id
        WHERE ms.flag = 'Y'
          AND ms.isbeneficiary = 'Y'
          AND ms.s_id IN @SchemeIds
        ORDER BY ms.s_id;";

        public async Task<IEnumerable<SchemeDto>> GetByTypeAsync(int type)
        {
            var schemeIds = type switch
            {
                1 => new[] { 136, 137, 138, 139, 140, 141 },
                2 => new[] { 142, 154, 155, 156, 157 },
                _ => Array.Empty<int>()
            };

            if (schemeIds.Length == 0)
                return Enumerable.Empty<SchemeDto>();

            using var connection = connectionFactory.CreateConnection();

            return await connection.QueryAsync<SchemeDto>(
                Sql,
                new { SchemeIds = schemeIds });
        }
    }
}
