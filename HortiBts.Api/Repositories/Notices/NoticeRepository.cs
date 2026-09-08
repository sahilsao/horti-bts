using Dapper;
using HortiBts.Api.Data;
using HortiBts.Shared.Common;
using HortiBts.Shared.Dtos.Notices;

namespace HortiBts.Api.Repositories.Notices;

public interface INoticeRepository
{
    Task<Result<List<NoticesDto>>> GetActiveAsync();
}

public class NoticeRepository(IDbConnectionFactory connectionFactory) : INoticeRepository
{
    public async Task<Result<List<NoticesDto>>> GetActiveAsync()
    {
        using var connection = connectionFactory.CreateConnection();
        string Sql = @"
        SELECT
            nb.notice_id    AS NoticeId,
            nb.subject      AS Subject,
            nb.description  AS Description,
            nb.start_date   AS StartDate,
            nb.end_date     AS EndDate,
            nb.priority     AS Priority,
            nb.notice_type  AS NoticeType,
            p.file_name     AS FileName,
            p.path          AS Path
        FROM noticeboard nb
        LEFT JOIN noticeboard_file_path p ON p.notice_id = nb.notice_id
        WHERE nb.status = 1";

        var notices = await connection.QueryAsync<NoticesDto>(Sql);
        return Result<List<NoticesDto>>.Success(notices.ToList());
    }
}