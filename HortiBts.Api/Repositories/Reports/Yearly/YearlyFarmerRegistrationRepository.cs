using Dapper;
using HortiBts.Api.Data;
using HortiBts.Shared.Common;
using HortiBts.Shared.Dtos.Reports;

namespace HortiBts.Api.Repositories.Reports.Yearly
{
    public interface IYearlyFarmerRegistrationRepository
    {
        Task<Result<List<DistWiseFarmerRegistrationDto>>> GetYearlyRptOfDistwiseFarmerRegistrationAsync(int financialYear);
        Task<Result<List<BlockWiseFarmerRegistrationDto>>> GetYearlyRptOfBlockwiseFarmerRegistrationAsync(int districtCode, int financialYear);
        Task<Result<List<RheoWiseFarmerRegistrationDto>>> GetYearlyRptOfRheowiseFarmerRegistrationAsync(RheoWiseFarmerFilterDto filter);
        Task<Result<List<VillageWiseFarmerRegistrationDto>>> GetYearlyRptOfVillagewiseFarmerRegistrationAsync(int officerCode, int financialYear);
        Task<Result<List<FarmerWiseFarmerRegistrationDto>>> GetYearlyRptOfFarmerwiseFarmerRegistrationAsync(FarmerWiseReportFilterDto filter);
    }
    public class YearlyFarmerRegistrationRepository(IDbConnectionFactory connectionFactory) : IYearlyFarmerRegistrationRepository
    {
        public async Task<Result<List<DistWiseFarmerRegistrationDto>>> GetYearlyRptOfDistwiseFarmerRegistrationAsync(int financialYear)
        {
            using var connection = connectionFactory.CreateConnection();
            string Sql = @"
            WITH villages AS (
            SELECT 
                v.village_code,
                v.DistCodeCensus,
                v.DistrictName
            FROM view_all_villages v ), farmer_detail AS (
            SELECT DISTINCT
                fd.fd_id,
                fd.hf_id,
                fd.rheo_id,
                fd.village_code
            FROM farmer_detail_horti fd
            INNER
            JOIN mas_farmer_horti mf ON mf.hf_id=fd.hf_id
            INNER
            JOIN villages v ON v.village_code= fd.village_code ), scheme AS (
            SELECT 
                count(DISTINCT gs.fd_id) f_count,
                COUNT( gs.equipment_id) s_count,
                SUM( (CASE WHEN gs.unit=4 THEN gs.area/2.471 WHEN gs.unit=6 THEN gs.area/10000 ELSE gs.area END) ) area,
                SUM(gs.cash) AS subsidy,
                fd.village_code
            FROM equipment_details_govt_schemes gs
            INNER
            JOIN farmer_detail fd ON fd.fd_id=gs.fd_id
            WHERE gs.financial_year = @FinancialYear
            GROUP BY fd.village_code )
            SELECT 
                IFNULL(r.target_count,0) Target,
                sum(ifnull(s.f_count,0)) AS FarmerCount,
                sum(ifnull(s.s_count,0)) AS SchemeCount,
                truncate(sum(ifnull(s.area,0)),3) AS Area,
                sum(ifnull(s.subsidy,0)) AS Subsidy,
                va.DistCodeCensus DistrictCode,
                rd.DistrictNameHindi DistrictNameHi,
                rd.DistrictName DistrictNameEn
            FROM view_all_villages va
            INNER
            JOIN rev_district rd ON rd.DistrictCensus=va.DistCodeCensus
            LEFT
            JOIN scheme s ON s.village_code=va.village_code
            LEFT
            JOIN reg_target r ON r.user_id=rd.DistrictCensus
            AND r.financial_year = @FinancialYear
            GROUP BY va.DistCodeCensus
            ORDER BY va.DistrictName";

            var result = await connection.QueryAsync<DistWiseFarmerRegistrationDto>(Sql, new { FinancialYear = financialYear });
            return Result<List<DistWiseFarmerRegistrationDto>>.Success(result.ToList());
        }

        public async Task<Result<List<BlockWiseFarmerRegistrationDto>>> GetYearlyRptOfBlockwiseFarmerRegistrationAsync(int districtCode, int financialYear)
        {
            using var connection = connectionFactory.CreateConnection();
            string Sql = @"
            WITH villages AS (
            SELECT 
                v.village_code,
                v.subdistrict_code,
                v.subdistrict_name,
                v.DistrictName,
                v.DistCodeCensus   
            FROM view_all_villages v
            WHERE v.DistCodeCensus=@DistrictCode ), farmer_detail AS (
            SELECT DISTINCT
                fd.fd_id,
                fd.hf_id,
                fd.rheo_id,
                fd.village_code
            FROM farmer_detail_horti fd
            INNER
            JOIN mas_farmer_horti mf ON mf.hf_id=fd.hf_id
            INNER
            JOIN villages v ON v.village_code= fd.village_code ), scheme AS (
            SELECT 
                count(DISTINCT gs.fd_id) f_count,
                COUNT( gs.equipment_id) s_count,
                SUM( (CASE WHEN gs.unit=4 THEN gs.area/2.471 WHEN gs.unit=6 THEN gs.area/10000 ELSE gs.area END) ) area,
                SUM(gs.cash) AS subsidy,
                fd.village_code
            FROM equipment_details_govt_schemes gs
            INNER
            JOIN farmer_detail fd ON fd.fd_id=gs.fd_id
            WHERE gs.financial_year=@FinancialYear
            GROUP BY fd.village_code )
            SELECT 
                sum(ifnull(s.f_count,0)) AS FarmerCount,
                sum(ifnull(s.s_count,0)) AS SchemeCount,
                truncate(sum(ifnull(s.area,0)),3) AS Area,
                sum(ifnull(s.subsidy,0)) AS Subsidy,
                va.subdistrict_code AS SubDistrictCode,
                rb.BlockNameHin AS SubDistrictNameHi,
                rb.BlockNameEng AS SubDistrictNameEn,
                va.DistCodeCensus As DistrictCode,
                rd.DistrictNameHindi AS DistrictNameHi,
                rd.DistrictName AS DistrictNameEn
            FROM villages va
            INNER
            JOIN rev_district rd ON rd.DistrictCensus=va.DistCodeCensus
            INNER
            JOIN rev_block rb ON rb.subdistrict_code=va.subdistrict_code
            LEFT
            JOIN scheme s ON s.village_code=va.village_code
            GROUP BY va.subdistrict_code
            ORDER BY rb.BlockNameEng";

            var result = await connection.QueryAsync<BlockWiseFarmerRegistrationDto>(Sql, new { DistrictCode = districtCode, FinancialYear = financialYear });
            return Result<List<BlockWiseFarmerRegistrationDto>>.Success(result.ToList());
        }


        public async Task<Result<List<RheoWiseFarmerRegistrationDto>>> GetYearlyRptOfRheowiseFarmerRegistrationAsync(RheoWiseFarmerFilterDto filter)
        {
            using var connection = connectionFactory.CreateConnection();
            const string sql = """
            WITH villages AS
            (
                SELECT
                    ovd.village_code,
                    ovd.officer_code,
                    v.DistCodeCensus,
                    v.DistrictName,
                    v.subdistrict_code,
                    v.subdistrict_name
                FROM officer_village_details ovd
                INNER JOIN view_all_villages v
                    ON v.village_code = ovd.village_code
                WHERE ovd.department_code = @DepartmentCode
                  AND (@DistrictCode IS NULL OR @DistrictCode = 0
                       OR v.DistCodeCensus = @DistrictCode)
                  AND (@SubDistrictCode IS NULL OR @SubDistrictCode = 0
                       OR v.subdistrict_code = @SubDistrictCode)
            ),

            farmer_details AS
            (
                SELECT DISTINCT
                    fd.fd_id,
                    fd.hf_id,
                    fd.rheo_id,
                    fd.village_code
                FROM farmer_detail_horti fd
                INNER JOIN mas_farmer_horti mf
                    ON mf.hf_id = fd.hf_id
                INNER JOIN villages v
                    ON v.village_code = fd.village_code
            ),

            schemes AS
            (
                SELECT
                    COUNT(DISTINCT gs.fd_id) AS f_count,
                    COUNT(gs.equipment_id) AS s_count,

                    SUM(
                        CASE
                            WHEN gs.unit = 4 THEN gs.area / 2.471
                            WHEN gs.unit = 6 THEN gs.area / 10000
                            ELSE gs.area
                        END
                    ) AS area,

                    SUM(gs.cash) AS subsidy,
                    fd.village_code
                FROM equipment_details_govt_schemes gs
                INNER JOIN farmer_details fd
                    ON fd.fd_id = gs.fd_id

                WHERE (@FinancialYear IS NULL OR @FinancialYear = ''
                       OR gs.financial_year = @FinancialYear)

                GROUP BY fd.village_code
            )

            SELECT
                SUM(COALESCE(s.f_count, 0)) AS FarmerCount,
                SUM(COALESCE(s.s_count, 0)) AS SchemeCount,

                TRUNCATE(
                    SUM(COALESCE(s.area, 0)),
                    3
                ) AS Area,

                SUM(COALESCE(s.subsidy, 0)) AS Subsidy,

                va.officer_code AS OfficerCode,
                m.name AS OfficerName,
                m.mobile_no AS MobileNo,

                rb.BlockNameHin AS SubDistrictNameHi,
               rb.BlockNameEng AS SubDistrictNameEn,
               va.subdistrict_code AS SubDistrictCode,

               va.DistCodeCensus AS DistrictCode,
               rd.DistrictNameHindi AS DistrictNameHi,
               rd.DistrictName AS DistrictNameEn

            FROM villages va

            INNER JOIN rev_block rb
                ON rb.subdistrict_code = va.subdistrict_code

            INNER JOIN rev_district rd
                ON rd.DistrictCensus = va.DistCodeCensus

            INNER JOIN mas_raeo m
                ON m.officer_code = va.officer_code

            LEFT JOIN schemes s
                ON s.village_code = va.village_code

            GROUP BY
                m.officer_code,
                m.name,
                m.mobile_no,
                rb.BlockNameHin,
                va.subdistrict_code,
                va.DistCodeCensus,
                rd.DistrictNameHindi

            ORDER BY
                rd.DistrictNameHindi,
                rb.BlockNameHin,
                m.name;
            """;

            var parameters = new
            {
                DepartmentCode = filter.DepartmentCode,
                DistrictCode = filter.DistrictCode,
                SubDistrictCode = filter.SubDistrictCode,
                FinancialYear = filter.FinancialYear
            };

            var result = await connection.QueryAsync<RheoWiseFarmerRegistrationDto>(sql, parameters);
            return Result<List<RheoWiseFarmerRegistrationDto>>.Success(result.ToList());
        }

        public async Task<Result<List<VillageWiseFarmerRegistrationDto>>> GetYearlyRptOfVillagewiseFarmerRegistrationAsync(int officerCode, int financialYear)
        {
            using var connection = connectionFactory.CreateConnection();
            const string sql = """
                WITH villages AS (
                    SELECT 
                        ovd.village_code,
                        ovd.officer_code
                    FROM officer_village_details ovd
                    WHERE ovd.officer_code = @OfficerCode ), farmer_detail AS (
                    SELECT DISTINCT
                        fd.fd_id,
                        fd.hf_id,
                        fd.rheo_id,
                        fd.village_code
                    FROM farmer_detail_horti fd
                    INNER
                    JOIN mas_farmer_horti mf ON mf.hf_id = fd.hf_id
                    INNER
                    JOIN villages v ON v.village_code = fd.village_code ), scheme AS (
                    SELECT 
                        COUNT(DISTINCT gs.fd_id) f_count,
                        COUNT(gs.equipment_id) s_count,
                        SUM(gs.area) area,
                        SUM(gs.cash) AS subsidy,
                        fd.village_code
                    FROM equipment_details_govt_schemes gs
                    INNER
                    JOIN farmer_detail fd ON fd.fd_id = gs.fd_id
                    WHERE gs.financial_year = @FinancialYear
                    GROUP BY fd.village_code )
                    SELECT 
                        IFNULL(s.f_count, 0) AS FarmerCount,
                        IFNULL(s.s_count, 0) AS SchemeCount,
                        IFNULL(s.area, 0) AS Area,
                        SUM( IFNULL(s.subsidy, 0) ) AS Subsidy,
                        va.distcodecensus AS DistrictCode,
                        va.subdistrict_code AS SubDistrictCode,
                        va.village_code as VillageCode,
                        va.village_name AS VillageName,
                        ovd.officer_code as OfficerCode,
                        m.name AS OfficerName,
                        m.mobile_no as MobileNo
                    FROM view_all_villages va
                    INNER
                    JOIN villages ovd ON ovd.village_code = va.village_code
                    INNER
                    JOIN mas_raeo m ON m.officer_code = ovd.officer_code
                    LEFT
                    JOIN scheme s ON s.village_code = ovd.village_code
                    WHERE ovd.officer_code = @OfficerCode
                    GROUP BY va.village_code
                    ORDER BY va.village_name;
                """;

            var result = await connection.QueryAsync<VillageWiseFarmerRegistrationDto>(sql, new { OfficerCode = officerCode, FinancialYear = financialYear });
            return Result<List<VillageWiseFarmerRegistrationDto>>.Success(result.ToList());
        }

        public async Task<Result<List<FarmerWiseFarmerRegistrationDto>>> GetYearlyRptOfFarmerwiseFarmerRegistrationAsync(FarmerWiseReportFilterDto filter)
        {
            using var connection = connectionFactory.CreateConnection();

            const string sql = """
                WITH basic_detail AS
                (
                    SELECT
                        mf.farmer_name_eng,
                        mf.farmer_name_hi,
                        mf.father_name,
                        mf.mobile_no,
                        mf.uf_id,
                        fd.fd_id,
                        fd.hf_id,
                        fd.rheo_id,
                        v.DistCodeCensus,
                        v.subdistrict_code,
                        fd.village_code,
                        v.village_name,

                        -- FIX: scalar subquery instead of LEFT JOIN, so this can never
                        -- produce more than one row per fd_id/village.
                        (
                            SELECT ovd2.officer_code
                            FROM officer_village_details ovd2
                            WHERE ovd2.village_code = v.village_code
                            AND (
                                    @OfficerCode IS NULL
                                    OR @OfficerCode = 0
                                    OR ovd2.officer_code = @OfficerCode
                                )
                            LIMIT 1
                        ) AS officer_code

                    FROM view_all_villages v

                    INNER JOIN farmer_detail_horti fd
                        ON fd.village_code = v.village_code

                    INNER JOIN mas_farmer_horti mf
                        ON mf.hf_id = fd.hf_id

                    WHERE
                        (@DistrictCode IS NULL OR @DistrictCode = 0
                        OR v.DistCodeCensus = @DistrictCode)

                        AND

                        (@SubDistrictCode IS NULL OR @SubDistrictCode = 0
                        OR v.subdistrict_code = @SubDistrictCode)

                        AND

                        (@VillageCode IS NULL OR @VillageCode = 0
                        OR v.village_code = @VillageCode)

                        AND
                        (
                            @OfficerCode IS NULL
                            OR @OfficerCode = 0
                            OR EXISTS
                            (
                                SELECT 1
                                FROM officer_village_details ovd
                                INNER JOIN mas_raeo mr
                                    ON mr.officer_code = ovd.officer_code
                                    AND mr.usertype = @DepartmentCode
                                WHERE ovd.village_code = v.village_code
                                AND ovd.officer_code = @OfficerCode
                            )
                        )
                ),

                gov_scheme AS
                (
                    SELECT
                        gs.scheme_type,
                        gs.schcme_id,
                        ms.scheme_name,
                        mc.cname,
                        gs.component_id,
                        gs.fd_id,
                        bd.village_code,
                        gs.khasra_no,
                        gs.unit,

                        SUM(gs.area) AS area,
                        SUM(gs.cash) AS cash,
                        SUM(gs.quantity) AS quantity,

                        gs.financial_year

                    FROM equipment_details_govt_schemes gs

                    INNER JOIN basic_detail bd
                        ON bd.fd_id = gs.fd_id

                    INNER JOIN mas_scheme_horti ms
                        ON ms.s_id = gs.schcme_id

                    INNER JOIN mas_component_horti mc
                        ON mc.c_id = gs.component_id

                    INNER JOIN mas_units mu
                        ON mu.unit_id = gs.unit

                    INNER JOIN mas_benefit_type mbt
                        ON mbt.benefit_type_id = gs.benefit_type

                    WHERE gs.financial_year = @FinancialYear

                    AND (
                        @SchemeType IS NULL
                        OR @SchemeType = 0
                        OR gs.scheme_type = @SchemeType
                    )

                    AND (
                        @SchemeId IS NULL
                        OR @SchemeId = 0
                        OR gs.schcme_id = @SchemeId
                    )

                    AND (
                        @ComponentId IS NULL
                        OR @ComponentId = 0
                        OR gs.component_id = @ComponentId
                    )

                    GROUP BY
                        bd.fd_id,
                        gs.component_id
                ),

                scheme_benefit AS
                (
                    SELECT
                        gsb.fd_id,
                        gsb.uf_id,
                        gsb.village_code,
                        gsb.schcme_id,
                        gsb.component_id,
                        gsb.benefit_type,
                        gsb.benefit,

                        mbt.benefit_name_hi AS benefit_type_name,

                        GROUP_CONCAT(
                            ' ',
                            mb.benefit_name_hi
                        ) AS benefit_name

                    FROM govt_schemes_benefit_details gsb

                    INNER JOIN basic_detail bd
                        ON bd.fd_id = gsb.fd_id

                    INNER JOIN mas_benefit_type mbt
                        ON mbt.benefit_type_id = gsb.benefit_type

                    INNER JOIN mas_benefit mb
                        ON mb.benefit_id = gsb.benefit

                    WHERE gsb.financial_year = @FinancialYear

                    GROUP BY
                        gsb.fd_id
                ),

                scheme_khasra AS
                (
                    SELECT
                        gsk.fd_id,
                        gsk.uf_id,
                        gsk.village_code,
                        gsk.schcme_id,
                        gsk.component_id,

                        GROUP_CONCAT(
                            ' ',
                            gsk.khasra_no
                        ) AS khasra_no

                    FROM govt_schemes_khasra_details gsk

                    INNER JOIN basic_detail bd
                        ON bd.fd_id = gsk.fd_id

                    WHERE gsk.financial_year = @FinancialYear

                    GROUP BY
                        gsk.fd_id
                ),

                land_data AS
                (
                    SELECT
                        GROUP_CONCAT(
                            ld.khasra_no,
                            '(',
                            ld.area,
                            ') '
                        ) AS total_land_khasra,

                        SUM(ld.area) AS total_land_area,
                        ld.fd_id

                    FROM land_details_horti ld

                    INNER JOIN basic_detail bd
                        ON bd.fd_id = ld.fd_id
                        AND bd.village_code = ld.village_code

                    WHERE ld.financial_year = @FinancialYear

                    GROUP BY
                        ld.fd_id
                )

                SELECT
                    bd.uf_id AS UFID,
                    bd.fd_id AS FDID,
                    bd.hf_id AS HFID,
                    bd.officer_code AS OfficerCode,
                    bd.subdistrict_code AS SubDistrictCode,
                    bd.distcodecensus AS DistrictCode,
                    bd.village_code AS VillageCode,
                    bd.village_name AS VillageName,
                    bd.farmer_name_eng AS FarmerNameEng,
                    bd.farmer_name_hi AS FarmerNameHi,
                    bd.father_name AS FarmerFatherName,
                    bd.mobile_no AS MobileNo,
                    gsc.scheme_type AS SchemeTypeID,
                    gsc.schcme_id AS SchemeID,
                    gsc.scheme_name AS SchemeName,
                    gsc.cname AS ComponentName, 
                    gsc.component_id AS ComponentID,
                    gsc.unit AS Unit,
                    TRUNCATE(
                    CASE WHEN gsc.unit = 4 THEN gsc.area / 2.471 WHEN gsc.unit = 6 THEN gsc.area / 10000 ELSE gsc.area END,
                    @DepartmentCode
                    ) AS Area,
                    gsc.cash AS Cash,
                    gsc.quantity AS Quantity,
                    sb.benefit_type AS BenefitType,
                    sb.benefit AS Benefit,
                    sb.benefit_type_name AS BenefitTypeName,
                    sb.benefit_name AS BenefitName,
                    sk.khasra_no AS KhasraNo,
                    ld1.total_land_khasra AS TotalLandKhasra,
                    ld1.total_land_area AS TotalLandArea

                FROM basic_detail bd

                INNER JOIN gov_scheme gsc
                    ON gsc.fd_id = bd.fd_id

                INNER JOIN land_data ld1
                    ON ld1.fd_id = bd.fd_id

                INNER JOIN scheme_benefit sb
                    ON sb.fd_id = bd.fd_id

                INNER JOIN scheme_khasra sk
                    ON sk.fd_id = bd.fd_id

                WHERE gsc.financial_year = @FinancialYear

                GROUP BY
                    bd.fd_id,
                    gsc.component_id

                ORDER BY
                    bd.village_name,
                    bd.farmer_name_eng;
                """;

            var parameters = new
            {
                DepartmentCode = filter.DepartmentCode,
                DistrictCode = filter.DistrictCode,
                SubDistrictCode = filter.SubDistrictCode,
                VillageCode = filter.VillageCode,
                OfficerCode = filter.OfficerCode,

                FinancialYear = filter.FinancialYear,

                SchemeType = filter.SchemeType,
                SchemeId = filter.SchemeId,
                ComponentId = filter.ComponentId
            };

            var result = await connection.QueryAsync<FarmerWiseFarmerRegistrationDto>(
                sql,
                parameters);

            return Result<List<FarmerWiseFarmerRegistrationDto>>.Success(result.ToList());
        }
    }
}


public class RheoWiseFarmerFilterDto
{
    public int DepartmentCode { get; set; }
    public int? DistrictCode { get; set; }
    public int? SubDistrictCode { get; set; }
    public int? FinancialYear { get; set; }
}

public class FarmerWiseReportFilterDto
{
    public int DepartmentCode { get; set; }
    public int? DistrictCode { get; set; }
    public int? SubDistrictCode { get; set; }
    public int? VillageCode { get; set; }
    public int? OfficerCode { get; set; }
    public int FinancialYear { get; set; }
    public int? SchemeType { get; set; }
    public int? SchemeId { get; set; }
    public int? ComponentId { get; set; }
}