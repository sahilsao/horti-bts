using Dapper;
using HortiBts.Api.Data;
using HortiBts.Api.Repositories.Components;
using HortiBts.Shared.Common;
using HortiBts.Shared.Dtos.Components;
using HortiBts.Shared.Dtos.Target;
using MySqlConnector;

namespace HortiBts.Api.Repositories.Target
{
    public interface ITargetRepository
    {
        Task<Result<List<TargetDto>>> GetTargetListAsync();
        Task<Result<int>> SaveTargetAsync(AddTargetDto dto, string userId, string clientIp);
    }
    public class TargetRepository(IDbConnectionFactory dbFactory) : ITargetRepository
    {
        public async Task<Result<List<TargetDto>>> GetTargetListAsync()
        {
            try
            {
                using var connection = dbFactory.CreateConnection(HortiDb.Bts);
                const string sql = """
                SELECT                   
                    r.id AS TargetId,
                    r.user_id AS UserId,
                    rd.DistrictNameHindi AS UserNameHi,
                    rd.DistrictName AS UserNameEn,
                    r.user_type AS UserType,
                    r.user_type_name AS UserTypeName,
                    r.target_count AS TargetCount,
                    r.target_area AS TargetArea,
                    r.financial_year AS FinancialYearID,
                    fy.financial_year AS FinancialYear
                FROM reg_target r
                INNER
                JOIN rev_district rd ON rd.DistrictCensus = r.user_id
                INNER
                JOIN mas_financialyear fy ON fy.id = r.financial_year
                """;
                var result = await connection.QueryAsync<TargetDto>(sql);
                return Result<List<TargetDto>>.Success(result.ToList());
            }
            catch (Exception ex)
            {
                return Result<List<TargetDto>>.Failure($"Failed to fetch targets: {ex.Message}");
            }
        }


        public async Task<Result<int>> SaveTargetAsync(AddTargetDto dto, string insertedBy, string clientIp)
        {
            using var connection = dbFactory.CreateConnection(HortiDb.Bts);
            try
            {
                if (!dto.TargetId.HasValue || dto.TargetId.Value == 0)
                {
                    const string insertSql = @"
                    INSERT INTO reg_target(
                        user_id, user_type, 
                        user_type_name, 
                        target_count, 
                        target_area, 
                        financial_year, 
                        created_at, 
                        created_by, 
                        ip_address)
                    VALUES(
                        @UserId, 
                        @UserType, 
                        @UserTypeName, 
                        @TargetCount, 
                        @TargetArea, 
                        @FinancialYear, 
                        NOW(), 
                        @CreatedBy, 
                        @IpAddress);
                    SELECT LAST_INSERT_ID();";

                    var insertedId = await connection.ExecuteScalarAsync<int>(insertSql, new
                    {
                        dto.UserId,
                        dto.UserType,
                        dto.UserTypeName,
                        TargetCount = dto.TargetCount ?? 0,
                        TargetArea = dto.TargetArea ?? 0,
                        FinancialYear = dto.FinancialYearId,
                        CreatedBy = insertedBy,
                        IpAddress = clientIp
                    });

                    return Result<int>.Success(insertedId);
                }
                else
                {
                    const string updateSql = @"
                    UPDATE reg_target
                    SET user_id = @UserId,
                        user_type = @UserType,
                        user_type_name = @UserTypeName,
                        target_count = @TargetCount,
                        target_area = @TargetArea,
                        financial_year = @FinancialYear,
                        updated_at = NOW(),
                        updated_by = @UpdatedBy,
                        ip_address = @IpAddress
                    WHERE id = @TargetId;";

                    var rowsAffected = await connection.ExecuteAsync(updateSql, new
                    {
                        dto.TargetId,
                        dto.UserId,
                        dto.UserType,
                        dto.UserTypeName,
                        TargetCount = dto.TargetCount ?? 0,
                        TargetArea = dto.TargetArea ?? 0,
                        FinancialYear = dto.FinancialYearId,
                        UpdatedBy = insertedBy,
                        IpAddress = clientIp
                    });

                    if (rowsAffected == 0)
                        return Result<int>.Failure("Target not found for update.");

                    return Result<int>.Success(dto.TargetId.Value);
                }
            }
            catch (MySqlException ex) when (ex.Number == 1062)
            {
                return Result<int>.Failure("A target for this user and financial year already exists.");
            }
            catch (Exception ex)
            {
                return Result<int>.Failure($"Unable to save target: {ex.Message}");
            }
        }
    }
}
