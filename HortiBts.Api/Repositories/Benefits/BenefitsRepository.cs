using Dapper;
using DocumentFormat.OpenXml.Spreadsheet;
using HortiBts.Api.Data;
using HortiBts.Api.Helpers;
using HortiBts.Shared.Common;
using HortiBts.Shared.Dtos.Benefits;
using MySqlConnector;

namespace HortiBts.Api.Repositories.Benefits
{
    public interface IBenefitsRepository
    {
        /// <summary>Returns all benefits</summary>
        Task<Result<List<BenefitsTypeDto>>> GetBenefitsTypeAsync();

        /// <summary>
        /// Returns all benefits list
        /// </summary>
        /// <returns></returns>
        Task<Result<List<BenefitsListDto>>> GetBenefitsListAsync();

        /// <summary>
        /// Saves a new benefit
        /// </summary>
        Task<Result<int>> SaveBenefitAsync(AddBenefitDto dto, string insertedBy, string clientIp);

        /// <summary>
        /// Updates the flag status of a benefit
        /// </summary>        
        Task<Result<bool>> UpdateFlagAsync(int benefitId, bool flag, string userId, string clientIp);
    }
    public class BenefitsRepository(IDbConnectionFactory dbFactory) : IBenefitsRepository
    {
        public async Task<Result<List<BenefitsTypeDto>>> GetBenefitsTypeAsync()
        {
            try
            {
                using var connection = dbFactory.CreateConnection(HortiDb.Bts);
                const string sql = """
                SELECT 
                    mu.benefit_type_id AS BenefitTypeId,
                    mu.benefit_name_en AS BenefitNameEn,
                    mu.benefit_name_hi AS BenefitNameHi
                FROM mas_benefit_type mu
                WHERE mu.flag=1
                
                """;
                var result = await connection.QueryAsync<BenefitsTypeDto>(sql);
                return Result<List<BenefitsTypeDto>>.Success(result.ToList());
            }
            catch (Exception ex)
            {
                return Result<List<BenefitsTypeDto>>.Failure($"Failed to fetch benefits: {ex.Message}");
            }
        }

        public async Task<Result<List<BenefitsListDto>>> GetBenefitsListAsync()
        {
            try
            {
                using var connection = dbFactory.CreateConnection(HortiDb.Bts);
                const string sql = """      
                SELECT Row_number()
                         OVER(
                           ORDER BY mu.benefit_id DESC) AS Srno,
                       mu.benefit_id AS BenefitId,
                       mu.benefit_type_id AS BenefitTypeId,
                       mbt.benefit_name_en AS BenefitTypeNameEn,
                       mbt.benefit_name_hi AS BenefitTypeNameHi,
                       mu.benefit_name_en AS BenefitNameEn,
                       mu.benefit_name_hi AS BenefitNameHi,
                       mu.flag	AS BenefitFlag
                FROM   mas_benefit mu
                       INNER JOIN mas_benefit_type mbt
                               ON mbt.benefit_type_id = mu.benefit_type_id
                """;
                var result = await connection.QueryAsync<BenefitsListDto>(sql);
                return Result<List<BenefitsListDto>>.Success(result.ToList());
            }
            catch (Exception ex)
            {
                return Result<List<BenefitsListDto>>.Failure($"Failed to fetch benefits list: {ex.Message}");
            }
        }

        public async Task<Result<int>> SaveBenefitAsync(AddBenefitDto dto, string insertedBy, string clientIp)
        {
            using var connection = dbFactory.CreateConnection(HortiDb.Bts);
            try
            {
                if (!dto.BenefitId.HasValue || dto.BenefitId.Value == 0)
                {
                    // ---------- INSERT ----------
                    const string insertSql = @"
                    INSERT INTO mas_benefit
                        (benefit_type_id, benefit_name_hi, benefit_name_en, flag, created_at, created_by, ip_address)
                    VALUES
                        (@BenefitTypeId, @BenefitNameHi, @BenefitNameEn, @Flag, NOW(), @UserId, @IpAddress);
                    SELECT LAST_INSERT_ID();";

                    var insertedId = await connection.ExecuteScalarAsync<int>(insertSql, new
                    {
                        dto.BenefitTypeId,
                        dto.BenefitNameHi,
                        dto.BenefitNameEn,
                        Flag = dto.Flag ? 1 : 0,
                        UserId = insertedBy,
                        IpAddress = clientIp
                    });

                    return Result<int>.Success(insertedId);
                }
                else
                {
                    // ---------- UPDATE ----------
                    const string updateSql = @"
                    UPDATE mas_benefit
                    SET benefit_type_id = @BenefitTypeId,
                        benefit_name_hi = @BenefitNameHi,
                        benefit_name_en = @BenefitNameEn,
                        flag = @Flag,
                        updated_at = NOW(),
                        updated_by = @UserId,
                        ip_address = @IpAddress
                    WHERE benefit_id = @BenefitId;";

                    var rowsAffected = await connection.ExecuteAsync(updateSql, new
                    {
                        dto.BenefitId,
                        dto.BenefitTypeId,
                        dto.BenefitNameHi,
                        dto.BenefitNameEn,
                        Flag = dto.Flag ? 1 : 0,
                        UserId = insertedBy,
                        IpAddress = clientIp
                    });

                    if (rowsAffected == 0)
                        return Result<int>.Failure("Benefit not found for update.");

                    return Result<int>.Success(dto.BenefitId.Value);
                }
            }
            catch (MySqlException ex) when (ex.Number == 1062)
            {
                return Result<int>.Failure("A benefit with this name already exists.");
            }
            catch (Exception ex)
            {
                return Result<int>.Failure($"Unable to save benefit: {ex.Message}");
            }
        }

        public async Task<Result<bool>> UpdateFlagAsync(int benefitId, bool flag, string userId, string clientIp)
        {
            using var connection = dbFactory.CreateConnection(HortiDb.Bts);
            const string sql = @"
                UPDATE mas_benefit
                SET flag = @Flag, updated_at = NOW(), updated_by = @UserId, ip_address = @IpAddress
                WHERE benefit_id = @BenefitId;";

            try
            {
                var rows = await connection.ExecuteAsync(sql, new
                {
                    BenefitId = benefitId,
                    Flag = flag ? 1 : 0,
                    UserId = userId,
                    IpAddress = clientIp
                });

                return rows > 0
                    ? Result<bool>.Success(true)
                    : Result<bool>.Failure("Benefit not found.");
            }
            catch (Exception ex)
            {
                return Result<bool>.Failure($"Unable to update flag: {ex.Message}");
            }
        }
    }
}
