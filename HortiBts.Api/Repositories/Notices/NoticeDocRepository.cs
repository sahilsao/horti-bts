using Dapper;
using HortiBts.Shared.Dtos.Notices;
using HortiBts.Api.Data;

namespace HortiBts.Api.Repositories.Notices;

public interface INoticeDocRepository
{
    Task<IEnumerable<NoticeDocDto>> GetByNoticeIdAsync(int noticeId);
}

public class NoticeDocRepository(IDbConnectionFactory connectionFactory) : INoticeDocRepository
{
    public async Task<IEnumerable<NoticeDocDto>> GetByNoticeIdAsync(int noticeId)
    {
        using var connection = connectionFactory.CreateConnection((HortiDb.Bts));
        const string Sql = @"
        SELECT 
            id AS Id,
            notice_id AS NoticeId,
            file_name AS FileName,
            path AS Path
        FROM noticeboard_file_path
        Where notice_id = @NoticeId and flag = 1";

        return await connection.QueryAsync<NoticeDocDto>(Sql, new { NoticeId = noticeId });
    }
}