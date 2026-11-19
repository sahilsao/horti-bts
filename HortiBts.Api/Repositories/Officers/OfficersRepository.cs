using Dapper;
using HortiBts.Api.Data;
using HortiBts.Shared.Common;
using HortiBts.Shared.Dtos.Officers;
using HortiBts.Shared.Dtos.SubDistricts;
using MySqlConnector;

namespace HortiBts.Api.Repositories.Officers
{
    public interface IOfficersRepository
    {
        Task<Result<List<DeptOfficersDto>>> GetDepartmentOfficersListByAsync(int departmentCode);
        Task<Result<List<RheoOfficersDto>>> GetAllOfficersListByAsync(int departmentCode);
        Task<Result<List<RheoOfficersDto>>> GetOfficersListByDistrictCodeAsync(int departmentCode, int districtCode);
        Task<Result<List<RheoOfficersDto>>> GetOfficersListBySubDistrictCodeAsync(int departmentCode, int subDistrictCode);
        Task<Result<List<RheoOfficersDto>>> GetOfficersListByVillageCodeAsync(int departmentCode, int villageCode);
        Task<Result<List<RheoOfficersPreviousAvailableVillagesDto>>> GetRheoOfficerPreviousAvailableVillagesListAsync(int officerCode, int flag);
        Task<Result<List<RheoOfficerMappedVillagesDto>>> GetRheoOfficerMappedVillagesListAsync(int departmentCode, int officerCode);
        Task<Result<List<RheoOfficersForChargeTransferDto>>> GetRheoOfficersListForChargeTranserAsync(int departmentCode, int districtCode);
        Task<Result<int>> SaveOfficerAsync(SaveOfficerDto dto, string createdBy, string clientIp);
        Task<Result<ChargeTransferResultDto>> TransferChargeAsync(SaveOfficerChargeTransferDto dto, int userId, bool isAdmin, string clientIp);
    }

    public class OfficersRepository(IDbConnectionFactory dbFactory) : IOfficersRepository
    {

        public async Task<Result<List<DeptOfficersDto>>> GetDepartmentOfficersListByAsync(int departmentCode)
        {
            try
            {
                using var connection = dbFactory.CreateConnection(HortiDb.Bts);
                const string sql = """
                SELECT 
                    designation_code AS DesignationCode,
                    designation_name AS DesignationName,
                    department_code AS DepartmentCode
                FROM mas_designation md
                WHERE md.department_code= @DepartmentCode
                """;
                var result = await connection.QueryAsync<DeptOfficersDto>(sql, new { DepartmentCode = departmentCode });
                return Result<List<DeptOfficersDto>>.Success(result.ToList());
            }
            catch (Exception ex)
            {
                return Result<List<DeptOfficersDto>>.Failure($"Failed to fetch department officers: {ex.Message}");
            }
        }
        public async Task<Result<List<RheoOfficersDto>>> GetAllOfficersListByAsync(int departmentCode)
        {
            try
            {
                using var connection = dbFactory.CreateConnection(HortiDb.Bts);
                const string sql = """
                SELECT
                    mr.officer_code AS OfficerCode,
                    mr.name AS OfficerName,
                    mr.mobile_no AS MobileNo,
                    COALESCE(mr.alternate_mobile_no, '-') AS AlternateMobileNo,
                    COALESCE(mr.email, '-') AS Email,
                    mr.charge_date AS ChargeTakenDate,
                    v.subdistrict_code AS SubDistrictCode,
                    v.subdistrict_name AS SubDistrictName,
                    v.DistCodeCensus AS DistrictCodeCensus,
                    v.DistrictName AS DistrictName
                    FROM
                    view_all_villages v
                    INNER JOIN officer_village_details ovd ON ovd.village_code = v.village_code
                    INNER JOIN mas_raeo mr ON mr.officer_code = ovd.officer_code
                    WHERE mr.usertype = @DepartmentCode
                    GROUP BY
                    ovd.officer_code
                    ORDER BY
                    mr.name
                """;
                var result = await connection.QueryAsync<RheoOfficersDto>(sql, new { DepartmentCode = departmentCode });
                return Result<List<RheoOfficersDto>>.Success(result.ToList());
            }
            catch (Exception ex)
            {
                return Result<List<RheoOfficersDto>>.Failure($"Failed to fetch officers: {ex.Message}");
            }
        }
        public async Task<Result<List<RheoOfficersDto>>> GetOfficersListByDistrictCodeAsync(int departmentCode, int districtCode)
        {
            try
            {
                using var connection = dbFactory.CreateConnection(HortiDb.Bts);
                const string sql = """
                SELECT
                    mr.officer_code AS OfficerCode,
                    mr.name AS OfficerName,
                    mr.mobile_no AS MobileNo,
                    COALESCE(mr.alternate_mobile_no, '-') AS AlternateMobileNo,
                    COALESCE(mr.email, '-') AS Email,
                    mr.charge_date AS ChargeTakenDate,
                    v.subdistrict_code AS SubDistrictCode,
                    v.subdistrict_name AS SubDistrictName,
                    v.DistCodeCensus AS DistrictCodeCensus,
                    v.DistrictName AS DistrictName
                    FROM
                    view_all_villages v
                    INNER JOIN officer_village_details ovd ON ovd.village_code = v.village_code
                    INNER JOIN mas_raeo mr ON mr.officer_code = ovd.officer_code
                    WHERE
                    v.DistCodeCensus = @DistrictCode
                    AND mr.usertype = @DepartmentCode
                    GROUP BY
                    ovd.officer_code
                    ORDER BY
                    mr.name
                """;
                var result = await connection.QueryAsync<RheoOfficersDto>(sql, new { DepartmentCode = departmentCode, DistrictCode = districtCode });
                return Result<List<RheoOfficersDto>>.Success(result.ToList());
            }
            catch (Exception ex)
            {
                return Result<List<RheoOfficersDto>>.Failure($"Failed to fetch officers: {ex.Message}");
            }
        }

        public async Task<Result<List<RheoOfficersDto>>> GetOfficersListBySubDistrictCodeAsync(int departmentCode, int subDistrictCode)
        {
            try
            {
                using var connection = dbFactory.CreateConnection(HortiDb.Bts);
                const string sql = """
                SELECT
                    mr.officer_code AS OfficerCode,
                    mr.name AS OfficerName,
                    mr.mobile_no AS MobileNo,
                    COALESCE(mr.alternate_mobile_no, '-') AS AlternateMobileNo,
                    COALESCE(mr.email, '-') AS Email,
                    mr.charge_date AS ChargeTakenDate,
                    v.subdistrict_code AS SubDistrictCode,
                    v.subdistrict_name AS SubDistrictName,
                    v.DistCodeCensus AS DistrictCodeCensus,
                    v.DistrictName AS DistrictName
                    FROM
                    view_all_villages v
                    INNER JOIN officer_village_details ovd ON ovd.village_code = v.village_code
                    INNER JOIN mas_raeo mr ON mr.officer_code = ovd.officer_code
                    WHERE
                    v.subdistrict_code = @SubDistrictCode
                    AND mr.usertype = @DepartmentCode
                    GROUP BY
                    ovd.officer_code
                    ORDER BY
                    mr.name
                """;
                var result = await connection.QueryAsync<RheoOfficersDto>(sql, new { DepartmentCode = departmentCode, SubDistrictCode = subDistrictCode });
                return Result<List<RheoOfficersDto>>.Success(result.ToList());
            }
            catch (Exception ex)
            {
                return Result<List<RheoOfficersDto>>.Failure($"Failed to fetch officers: {ex.Message}");
            }
        }

        public async Task<Result<List<RheoOfficersDto>>> GetOfficersListByVillageCodeAsync(int departmentCode, int villageCode)
        {
            try
            {
                using var connection = dbFactory.CreateConnection(HortiDb.Bts);
                const string sql = """
                SELECT
                    mr.officer_code AS OfficerCode,
                    mr.name AS OfficerName,
                    mr.mobile_no AS MobileNo,
                    COALESCE(mr.alternate_mobile_no, '-') AS AlternateMobileNo,
                    COALESCE(mr.email, '-') AS Email,
                    mr.charge_date AS ChargeTakenDate,
                    v.subdistrict_code AS SubDistrictCode,
                    v.subdistrict_name AS SubDistrictName,
                    v.DistCodeCensus AS DistrictCodeCensus,
                    v.DistrictName AS DistrictName
                    FROM
                    view_all_villages v
                    INNER JOIN officer_village_details ovd ON ovd.village_code = v.village_code
                    INNER JOIN mas_raeo mr ON mr.officer_code = ovd.officer_code
                    WHERE
                    v.village_code = @VillageCode
                    AND mr.usertype = @DepartmentCode
                    GROUP BY
                    ovd.officer_code
                    ORDER BY
                    mr.name
                """;
                var result = await connection.QueryAsync<RheoOfficersDto>(sql, new { DepartmentCode = departmentCode, VillageCode = villageCode });
                return Result<List<RheoOfficersDto>>.Success(result.ToList());
            }
            catch (Exception ex)
            {
                return Result<List<RheoOfficersDto>>.Failure($"Failed to fetch officers: {ex.Message}");
            }
        }

        public async Task<Result<List<RheoOfficersPreviousAvailableVillagesDto>>> GetRheoOfficerPreviousAvailableVillagesListAsync(int officerCode, int flag)
        {
            try
            {
                using var connection = dbFactory.CreateConnection(HortiDb.Bts);
                const string sql = """
                    SELECT 
                        ROW_NUMBER() OVER(
                    ORDER BY b.village_name) AS srno, b.village_code AS VillageCode, b.village_name AS VillageName, b.Halka AS Halka, b.subdistrict_code AS SubdistrictCode, rb.BlockNameEng AS SubdistrictName, b.DistCodeCensus AS DistrictCode, b.district_id AS DistrictId, b.DistrictName AS DistrictName, ovd.update_datetime AS last_assign_date
                    FROM officer_village_details ovd
                    INNER
                    JOIN view_all_villages b ON b.village_code=ovd.village_code
                    INNER
                    JOIN rev_block rb ON rb.subdistrict_code=b.subdistrict_code
                    WHERE ovd.officer_code= @OfficerCode
                    AND ovd.flag= @Flag
                    ORDER BY b.village_name
                """;
                var result = await connection.QueryAsync<RheoOfficersPreviousAvailableVillagesDto>(sql, new { OfficerCode = officerCode, Flag = flag });
                return Result<List<RheoOfficersPreviousAvailableVillagesDto>>.Success(result.ToList());
            }
            catch (Exception ex)
            {
                return Result<List<RheoOfficersPreviousAvailableVillagesDto>>.Failure($"Failed to fetch rheo officer's previously available villages: {ex.Message}");
            }
        }

        public async Task<Result<List<RheoOfficerMappedVillagesDto>>> GetRheoOfficerMappedVillagesListAsync(int departmentCode, int officerCode)
        {
            try
            {
                using var connection = dbFactory.CreateConnection(HortiDb.Bts);
                const string sql = """
                    SELECT
                    b.village_code AS VillageCode,
                    (
                        CASE WHEN b.Halka IS NULL THEN b.village_name ELSE CONCAT(
                            b.village_name, ' (', b.Halka, ')'
                        ) END
                    ) AS VillageName ,
                    b.Halka,
                    b.subdistrict_code AS SubDistrictCode,
                    rb.BlockNameEng AS SubDistrictName,
                    b.DistCodeCensus AS DistrictCode,
                    b.district_id AS DistrictId,
                    b.DistrictName AS DistrictName,
                    a.update_datetime as LastAssignDate 
                FROM
                    officer_village_details a
                    INNER JOIN view_all_villages b ON a.village_code = b.village_code
                    INNER JOIN rev_block rb ON rb.subdistrict_code = b.subdistrict_code
                WHERE
                    a.officer_code = @OfficerCode AND a.flag = 1
                    AND a.department_code = @DepartmentCode
                ORDER BY
                    b.village_name
                """;
                var result = await connection.QueryAsync<RheoOfficerMappedVillagesDto>(sql, new { DepartmentCode = departmentCode, OfficerCode = officerCode });
                return Result<List<RheoOfficerMappedVillagesDto>>.Success(result.ToList());
            }
            catch (Exception ex)
            {
                return Result<List<RheoOfficerMappedVillagesDto>>.Failure($"Failed to fetch rheo officer's mapped villages: {ex.Message}");
            }
        }

        public async Task<Result<List<RheoOfficersForChargeTransferDto>>> GetRheoOfficersListForChargeTranserAsync(int departmentCode, int districtCode)
        {
            try
            {
                using var connection = dbFactory.CreateConnection(HortiDb.Bts);
                const string sql = """
                SELECT 
                    mr.officer_code AS OfficerCode,
                    mr.name AS OfficerName,
                    mr.mobile_no AS MobileNo,
                    COALESCE(mr.alternate_mobile_no, '-') AS alternate_mobile_no,
                    COALESCE(mr.email, '-') AS email,
                    mr.charge_date AS ChargeDate,
                    1 AS is_village_mapped
                FROM view_all_villages v
                LEFT
                JOIN officer_village_details ovd ON ovd.village_code=v.village_code
                LEFT
                JOIN mas_raeo mr ON mr.officer_code=ovd.officer_code
                WHERE v.DistCodeCensus= @DistrictCode
                AND mr.usertype= @DepartmentCode
                GROUP BY ovd.officer_code
                UNION
                SELECT 
                    mr.officer_code,
                    mr.name,
                    mr.mobile_no,
                    COALESCE(mr.alternate_mobile_no, '-') AS alternate_mobile_no,
                    COALESCE(mr.email, '-') AS email,
                    mr.charge_date,
                    0 AS is_village_mapped
                FROM mas_raeo mr
                WHERE mr.officer_code NOT IN(
                SELECT 
                    m.officer_code
                FROM officer_village_details m)
                AND mr.usertype= @DepartmentCode
                ORDER BY is_village_mapped desc,OfficerName asc
                """;
                var result = await connection.QueryAsync<RheoOfficersForChargeTransferDto>(sql, new { DepartmentCode = departmentCode, DistrictCode = districtCode });
                return Result<List<RheoOfficersForChargeTransferDto>>.Success(result.ToList());
            }
            catch (Exception ex)
            {
                return Result<List<RheoOfficersForChargeTransferDto>>.Failure($"Failed to fetch rheo officer's list for charge transfer: {ex.Message}");
            }
        }

        public async Task<Result<int>> SaveOfficerAsync(SaveOfficerDto dto, string createdBy, string clientIp)
        {
            const string DefaultPasswordHash = "$2y$10$bxLphQHZp96xv8mFT6iicutES5GmXmQh2W.SsDWiydIxtv7qNOjd.";

            using var connection = dbFactory.CreateConnection(HortiDb.Bts);
            connection.Open();
            using var tran = connection.BeginTransaction();

            try
            {
                // 1) mas_raeo
                const string sql1 = @"
                INSERT INTO mas_raeo
                    (name, role, password, circle_name, usertype, mobile_no, alternate_mobile_no,
                    email, totalcount, subdistrict_code, district_code, charge_date,
                    created_by, ip_address)
                VALUES
                    (@Name, @Role, @Password, @CircleName, @Role, @MobileNo, @AlternateMobileNo,
                    @Email, @VillageCount, @SubDistrictCode, @DistrictCode, @ChargeDate,
                    @CreatedBy, @IpAddress);
                SELECT LAST_INSERT_ID();";

                var officerCode = await connection.ExecuteScalarAsync<int>(sql1, new
                {
                    dto.Name,
                    dto.Role,
                    Password = DefaultPasswordHash,
                    dto.CircleName,
                    dto.MobileNo,
                    dto.AlternateMobileNo,
                    dto.Email,
                    dto.VillageCount,
                    dto.SubDistrictCode,
                    dto.DistrictCode,
                    dto.ChargeDate,
                    CreatedBy = createdBy,
                    IpAddress = clientIp
                }, tran);

                // 2) officer_village_details, one row per selected village
                const string sql2 = @"
                INSERT INTO officer_village_details
                    (village_code, officer_code, department_code, remark, created_by, ip_address)
                VALUES
                    (@VillageCode, @OfficerCode, @DepartmentCode, 'HORTI', @CreatedBy, @IpAddress);";

                var villageRows = dto.VillageCodes.Distinct().Select(code => new
                {
                    VillageCode = code,
                    OfficerCode = officerCode,
                    DepartmentCode = dto.Role,
                    CreatedBy = createdBy,
                    IpAddress = clientIp
                });
                await connection.ExecuteAsync(sql2, villageRows, tran);

                tran.Commit();
                return Result<int>.Success(officerCode);
            }
            catch (MySqlException ex) when (ex.Number == 1062)
            {
                tran.Rollback();
                return Result<int>.Failure("यह अधिकारी या ग्राम मैपिंग पहले से मौजूद है।");
            }
            catch (Exception ex)
            {
                tran.Rollback();
                return Result<int>.Failure($"Unable to save officer: {ex.Message}");
            }
        }

        public async Task<Result<ChargeTransferResultDto>> TransferChargeAsync(SaveOfficerChargeTransferDto dto, int userId, bool isAdmin, string clientIp)
        {
            var sourceCodes = dto.SourceVillageCodes.Distinct().ToList();
            var vacantCodes = dto.VacantVillageCodes.Distinct().ToList();

            if (sourceCodes.Intersect(vacantCodes).Any())
                return Result<ChargeTransferResultDto>.Failure("A village cannot be in both lists.");

            using var connection = dbFactory.CreateConnection(HortiDb.Bts);   // ADAPT
            connection.Open();
            using var tran = connection.BeginTransaction();

            try
            {
                // ---- A) a District user may only touch officers of their own district ----
                if (!isAdmin)
                {
                    var officers = new List<int> { dto.TargetOfficerCode };
                    if (sourceCodes.Count > 0) officers.Add(dto.SourceOfficerCode);

                    string SqlCountOfficersInDistrict = """
                    SELECT COUNT(*) FROM mas_raeo WHERE officer_code IN @Officers AND district_code = @District
                    """;

                    var ownOfficers = await connection.ExecuteScalarAsync<int>(SqlCountOfficersInDistrict,
                        new { Officers = officers, District = userId }, tran);

                    if (ownOfficers != officers.Count)
                    {
                        tran.Rollback();
                        return Result<ChargeTransferResultDto>.Failure("Officers do not belong to your district.");
                    }
                }

                // ---- B) vacant villages must be in the target officer's district ----
                if (vacantCodes.Count > 0)
                {
                    string SqlCountVillagesInTargetDistrict = """
                    SELECT COUNT(*)
                    FROM view_all_villages v
                    WHERE v.village_code IN @Codes
                    AND v.DistCodeCensus = (SELECT CAST(district_code AS UNSIGNED)
                                            FROM mas_raeo
                                            WHERE officer_code = @Target)
                    """;

                    var validVacant = await connection.ExecuteScalarAsync<int>(SqlCountVillagesInTargetDistrict,
                        new { Codes = vacantCodes, Target = dto.TargetOfficerCode }, tran);

                    if (validVacant != vacantCodes.Count)
                    {
                        tran.Rollback();
                        return Result<ChargeTransferResultDto>.Failure("Some vacant villages are not in the target officer's district.");
                    }
                }

                var transferred = 0;

                if (sourceCodes.Count > 0)
                {
                    string SqlInsertVillageHistory = """
                    INSERT INTO officer_village_history
                        (officer_code_old, officer_code_new, department_code, village_code,
                        flag, vill_assign_date, ip_address, created_by)
                    SELECT officer_code, @Target, department_code, village_code,
                        'T', `datetime`, @IpAddress, @UserId
                    FROM officer_village_details
                    WHERE officer_code = @Source
                    AND department_code = @SourceRole
                    AND village_code IN @Codes
                    """;

                    // 1) history rows (flag 'T'); INSERT..SELECT also proves the villages belong to the source officer
                    transferred = await connection.ExecuteAsync(SqlInsertVillageHistory,
                        new
                        {
                            Source = dto.SourceOfficerCode,
                            Target = dto.TargetOfficerCode,
                            dto.SourceRole,
                            Codes = sourceCodes,
                            IpAddress = clientIp,
                            UserId = userId
                        }, tran);

                    if (transferred != sourceCodes.Count)
                    {
                        tran.Rollback();
                        return Result<ChargeTransferResultDto>.Failure("Some villages are not mapped to the source officer.");
                    }

                    string SqlMoveVillagesToTarget = """
                    UPDATE officer_village_details
                    SET officer_code = @Target,
                        remark = 'HORTI',
                        ip_address = @IpAddress,
                        updated_by = @UserId
                    WHERE officer_code = @Source
                    AND department_code = @SourceRole
                    AND village_code IN @Codes
                    """;

                    // 2) move the mapping to the target officer
                    var updated = await connection.ExecuteAsync(SqlMoveVillagesToTarget,
                        new
                        {
                            Source = dto.SourceOfficerCode,
                            Target = dto.TargetOfficerCode,
                            dto.SourceRole,
                            Codes = sourceCodes,
                            IpAddress = clientIp,
                            UserId = userId
                        }, tran);

                    if (updated != sourceCodes.Count)
                    {
                        tran.Rollback();
                        return Result<ChargeTransferResultDto>.Failure("Unable to move all villages to the target officer.");
                    }
                }

                // 3) assign vacant villages to the target officer, with the TARGET officer's role as department_code
                var assigned = 0;
                if (vacantCodes.Count > 0)
                {
                    string SqlInsertVacantVillage = """
                    INSERT INTO officer_village_details
                        (officer_code, department_code, village_code, remark, ip_address, created_by)
                    VALUES
                        (@Target, @TargetRole, @VillageCode, 'HORTI', @IpAddress, @UserId)
                    """;

                    assigned = await connection.ExecuteAsync(SqlInsertVacantVillage,
                        vacantCodes.Select(code => new
                        {
                            Target = dto.TargetOfficerCode,
                            dto.TargetRole,
                            VillageCode = code,
                            IpAddress = clientIp,
                            UserId = userId
                        }), tran);
                }

                tran.Commit();
                return Result<ChargeTransferResultDto>.Success(
                    new ChargeTransferResultDto { TransferredVillages = transferred, AssignedVacantVillages = assigned });
            }
            catch (MySqlException ex) when (ex.Number == 1062)
            {
                tran.Rollback();
                return Result<ChargeTransferResultDto>.Failure("One or more villages are already mapped.");
            }
            catch (Exception ex)
            {
                tran.Rollback();
                return Result<ChargeTransferResultDto>.Failure($"Unable to transfer charge: {ex.Message}");
            }
        }
    }
}
