using System;
using Dapper;
using HortiBts.Api.Data;
using HortiBts.Shared.Common;

namespace HortiBts.Api.Repositories.ApplicationVerification.DDH.NewApplication;

public interface IApproveRejectFarmerApplicationByDDHRepository
{
    Task<Result<int>> ApproveRejectFarmerApplicationAsync(
        int applicationId,
        int status,
        string remark,
        string ipAddress,
        string userId);
}

public class ApproveRejectFarmerApplicationByDDHRepository(
    IDbConnectionFactory dbFactory)
    : IApproveRejectFarmerApplicationByDDHRepository
{
    public async Task<Result<int>> ApproveRejectFarmerApplicationAsync(
        int applicationId,
        int status,
        string remark,
        string ipAddress,
        string userId)
    {
        using var connection = dbFactory.CreateConnection(HortiDb.Bts);
        await connection.OpenAsync();

        await using var transaction = await connection.BeginTransactionAsync();

        try
        {
            // 1. Verify RHEO approval
            const string checkSql = """
                SELECT COUNT(1)
                FROM temp_farmer_application
                WHERE application_id = @ApplicationId
                  AND rheo_approval_status = 1
                  AND status = 1;
                """;

            var exists = await connection.ExecuteScalarAsync<int>(
                checkSql,
                new
                {
                    ApplicationId = applicationId
                },
                transaction);

            if (exists == 0)
            {
                await transaction.RollbackAsync();

                return Result<int>.Failure(
                    "FARMER_NOT_APPROVED_BY_RHEO");
            }

            // 2. Update DDH approval/rejection
            const string updateSql = """
                UPDATE temp_farmer_application
                SET
                    ddh_remark = @Remark,
                    ddh_approval_status = @Status,
                    updated_ip_address = @IpAddress,
                    updated_by = @UserId
                WHERE application_id = @ApplicationId
                  AND rheo_approval_status = 1
                  AND status = 1;
                """;

            var affectedRows = await connection.ExecuteAsync(
                updateSql,
                new
                {
                    ApplicationId = applicationId,
                    Status = status,
                    Remark = remark,
                    IpAddress = ipAddress,
                    UserId = userId
                },
                transaction);

            if (affectedRows == 0)
            {
                await transaction.RollbackAsync();

                return Result<int>.Failure(
                    "Application could not be updated.");
            }

            await transaction.CommitAsync();

            return Result<int>.Success(affectedRows);
        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync();

            return Result<int>.Failure(
                $"Failed to approve/reject farmer application: {ex.Message}");
        }
    }
}
