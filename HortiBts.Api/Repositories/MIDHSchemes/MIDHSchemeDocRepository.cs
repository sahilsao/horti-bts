using Dapper;
using HortiBts.Shared.Dtos.MIDHSchemes;
using HortiBts.Api.Data;

namespace HortiBts.Api.Repositories.MIDHSchemes;

public interface IMIDHSchemeDocRepository
{
    Task<IEnumerable<MIDHSchemeDocDto>> GetByMidhSchemeIdAsync(int schemeId);
}

public class MIDHSchemeDocRepository(IDbConnectionFactory connectionFactory) : IMIDHSchemeDocRepository
{

    // NOTE: the Node source has a commented-out mas_scheme_file_path_new table for
    // doc_type=SCHEME_NEW, currently falling back to the same mas_scheme_file_path table
    // as doc_type=SCHEME. So both central and state schemes resolve to this one query for now.
    // If that _new table gets wired back up on their end, split this into two queries again.

    public async Task<IEnumerable<MIDHSchemeDocDto>> GetByMidhSchemeIdAsync(int schemeId)
    {
        using var connection = connectionFactory.CreateConnection((HortiDb.Bts));
        const string Sql = @"
        SELECT
            ms.id          AS Id,
            ms.scheme_id   AS SchemeId,
            ms.file_name   AS FileName,
            ms.path        AS Path
        FROM mas_scheme_file_path_new ms
        WHERE ms.flag = 1
          AND ms.scheme_id = @SchemeId";

        return await connection.QueryAsync<MIDHSchemeDocDto>(Sql, new { SchemeId = schemeId });
    }
}