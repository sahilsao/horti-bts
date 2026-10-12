using System;
using Dapper;
using HortiBts.Api.Data;
using HortiBts.Shared.Common;

namespace HortiBts.Api.Repositories.ApplicationVerification.DDH.OldApplication;

public interface IApproveRejectOldFarmerApplicationByDDHRepository
{
    Task<Result<int>> ApproveRejectOldFarmerApplicationAsync(
        int applicationId,
        int status,
        string remark,
        string ipAddress,
        string userId);
}

public class ApproveRejectOldFarmerApplicationByDDHRepository(
    IDbConnectionFactory dbFactory)
    : IApproveRejectOldFarmerApplicationByDDHRepository
{
    public async Task<Result<int>> ApproveRejectOldFarmerApplicationAsync(
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
            // Update DDH approval/rejection
            const string updateSql = """
                UPDATE app_scheme_details
                    SET status= @Status , remark= @Remark , updated_by= @UpdatedBy
                    WHERE status= @Status
                    AND sd_id= @SdId
                    AND financial_year= @FinancialYear;
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
