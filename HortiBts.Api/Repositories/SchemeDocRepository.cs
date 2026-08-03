using Dapper;
using HortiBts.Shared.Dtos.Schemes;
using HortiBts.Api.Data;

namespace HortiBts.Api.Repositories;

public interface ISchemeDocRepository
{
    Task<IEnumerable<SchemeDocDto>> GetBySchemeIdAsync(int schemeId);
}

public class SchemeDocRepository(IDbConnectionFactory connectionFactory) : ISchemeDocRepository
{

    // NOTE: the Node source has a commented-out mas_scheme_file_path_new table for
    // doc_type=SCHEME_NEW, currently falling back to the same mas_scheme_file_path table
    // as doc_type=SCHEME. So both central and state schemes resolve to this one query for now.
    // If that _new table gets wired back up on their end, split this into two queries again.
    private const string Sql = @"
        SELECT
            ms.id          AS Id,
            ms.scheme_id   AS SchemeId,
            ms.file_name   AS FileName,
            ms.path        AS Path
        FROM mas_scheme_file_path ms
        WHERE ms.flag = 1
          AND ms.scheme_id = @SchemeId";

    public async Task<IEnumerable<SchemeDocDto>> GetBySchemeIdAsync(int schemeId)
    {
        using var connection = connectionFactory.CreateConnection();
        return await connection.QueryAsync<SchemeDocDto>(Sql, new { SchemeId = schemeId });
    }
}