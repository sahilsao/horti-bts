using System;
using Dapper;
using HortiBts.Api.Data;
using HortiBts.Shared.Common;
using HortiBts.Shared.Dtos.Reports.PreviousFY.BeneficiaryStatus;

namespace HortiBts.Api.Repositories.Reports.PreviousFY.BeneficiaryStatus
{
    public interface IFarmerBeneficiaryStatusRepository
    {
        Task<Result<List<DistWiseFarmerBeneficiaryStatusDto>>> GetRptOfDistWiseFarmerBeneficiaryStatusAsync();
        Task<Result<List<BlockWiseFarmerBeneficiaryStatusDto>>> GetRptOfBlockWiseFarmerBeneficiaryStatusAsync(int districtCode);
        Task<Result<List<VillageWiseFarmerBeneficiaryStatusDto>>> GetRptOfVillageWiseFarmerBeneficiaryStatusAsync(int subDistrictCode);
        Task<Result<List<FarmerWiseFarmerBeneficiaryStatusDto>>> GetRptOfFarmerWiseFarmerBeneficiaryStatusAsync(int villageCode, int? statusCode = null);
    }
    public class FarmerBeneficiaryStatusRepository(IDbConnectionFactory connectionFactory) : IFarmerBeneficiaryStatusRepository
    {

        public async Task<Result<List<DistWiseFarmerBeneficiaryStatusDto>>> GetRptOfDistWiseFarmerBeneficiaryStatusAsync()
        {
            using var connection = connectionFactory.CreateConnection();
            string Sql = @"
            SELECT 
                rb.DistrictCensus AS district_code,
                rb.DistrictName AS district_name_en,
                rb.DistrictNameHindi AS district_name_hi,
                SUM(IF(h.status_flag=1,1,0)) AS approved,
                SUM(IF(h.status_flag=0,1,0)) AS rejected
            FROM horti_farmer_verify_status h
            RIGHT
            JOIN (
            SELECT 
                rv.village_code,
                CONCAT(rv.village_name,'(',rv.Halka,')') AS village_name,
                CONCAT(rv.village_name_hi,'(',rv.Halka,')') AS village_name_hi,
                rv.TehsilCensus,
                rv.SubDistrictCodeCensus AS subdistrict_code,
                rv.DistCodeCensus
            FROM rev_villages rv
            UNION
            SELECT 
                rvf.vsrno,
                CONCAT(rvf.villcdname,'(',rvf.halka,')') AS village_name,
                CONCAT(rvf.villcdname,'(',rvf.halka,')') AS village_name_hi,
                rvf.Tehsil_census AS TehsillCensus,
                rvf.subdistrict_code,
                rvf.District_Census AS DistCodeCensus
            FROM rev_village_forest rvf ) vv ON vv.village_code=h.village_code
            RIGHT
            JOIN rev_district rb ON rb.DistrictCensus=vv.DistCodeCensus
            GROUP BY rb.DistrictCensus, rb.DistrictNameHindi
            ORDER BY rb.DistrictName";

            var result = await connection.QueryAsync<DistWiseFarmerBeneficiaryStatusDto>(Sql);
            return Result<List<DistWiseFarmerBeneficiaryStatusDto>>.Success(result.ToList());
        }

        public async Task<Result<List<BlockWiseFarmerBeneficiaryStatusDto>>> GetRptOfBlockWiseFarmerBeneficiaryStatusAsync(int districtCode)
        {
            using var connection = connectionFactory.CreateConnection();
            string Sql = @"
            SELECT 
                rb.subdistrict_code AS SubdistrictCode,
                rb.BlockNameEng AS subdistrict_name_en,
                rb.BlockNameHin AS subdistrict_name_hi,
                sum(if(h.status_flag=1,1,0)) AS approved,
                sum(if(h.status_flag=0,1,0)) AS rejected
            FROM horti_farmer_verify_status h
            RIGHT
            JOIN (
            SELECT 
                rv.village_code,
                CONCAT(rv.village_name,'(',rv.Halka,')') AS village_name,
                CONCAT(rv.village_name_hi,'(',rv.Halka,')') AS village_name_hi,
                rv.TehsilCensus,
                rv.SubDistrictCodeCensus AS subdistrict_code
            FROM rev_villages rv
            WHERE rv.DistCodeCensus = @DistrictCode
            UNION
            SELECT 
                rvf.vsrno,
                CONCAT(rvf.villcdname,'(',rvf.halka,')') AS village_name,
                CONCAT(rvf.villcdname,'(',rvf.halka,')') AS village_name_hi,
                rvf.Tehsil_census AS TehsillCensus,
                rvf.subdistrict_code
            FROM rev_village_forest rvf
            WHERE rvf.District_Census= @DistrictCode) vv ON vv.village_code=h.village_code
            RIGHT
            JOIN rev_block rb ON rb.subdistrict_code=vv.subdistrict_code
            WHERE rb.DistCodeCensus= @DistrictCode
            GROUP BY rb.BlockNameHin, rb.BlockNameEng
            ORDER BY rb.BlockNameEng";

            var result = await connection.QueryAsync<BlockWiseFarmerBeneficiaryStatusDto>(Sql, new { DistrictCode = districtCode });
            return Result<List<BlockWiseFarmerBeneficiaryStatusDto>>.Success(result.ToList());
        }
        public async Task<Result<List<VillageWiseFarmerBeneficiaryStatusDto>>> GetRptOfVillageWiseFarmerBeneficiaryStatusAsync(int subDistrictCode)
        {
            using var connection = connectionFactory.CreateConnection();
            string Sql = @"
            SELECT 
                vv.village_code AS VillageCode,
                vv.village_name AS VillageNameEn,
                vv.village_name_hi AS VillageNameHi,
                SUM(if(h.status_flag=1,1,0)) AS approved,
                SUM(if(h.status_flag=0,1,0)) AS rejected
            FROM horti_farmer_verify_status h
            RIGHT
            JOIN (
            SELECT 
                rv.village_code,
                CONCAT(rv.village_name,'(',rv.Halka,')') AS village_name,
                CONCAT(rv.village_name_hi,'(',rv.Halka,')') AS village_name_hi,
                rv.TehsilCensus,
                rv.SubDistrictCodeCensus AS subdistrict_code
            FROM rev_villages rv
            WHERE rv.SubDistrictCodeCensus= @SubDistrictCode
            UNION
            SELECT 
                rvf.vsrno,
                CONCAT(rvf.villcdname,'(',rvf.halka,')') AS village_name,
                CONCAT(rvf.villcdname,'(',rvf.halka,')') AS village_name_hi,
                rvf.Tehsil_census AS TehsillCensus,
                rvf.subdistrict_code
            FROM rev_village_forest rvf
            WHERE rvf.subdistrict_code= @SubDistrictCode) vv ON vv.village_code=h.village_code
            GROUP BY vv.village_code, vv.village_name
            ORDER BY vv.village_name";

            var result = await connection.QueryAsync<VillageWiseFarmerBeneficiaryStatusDto>(Sql, new { SubDistrictCode = subDistrictCode });
            return Result<List<VillageWiseFarmerBeneficiaryStatusDto>>.Success(result.ToList());
        }

        public async Task<Result<List<FarmerWiseFarmerBeneficiaryStatusDto>>> GetRptOfFarmerWiseFarmerBeneficiaryStatusAsync(int villageCode, int? statusCode = null)
        {
            using var connection = connectionFactory.CreateConnection(HortiDb.NewUfp);

            const string sql = @"
            SELECT 
                mf.uf_id AS UfId,
                CONCAT(REPEAT('*', 8), mf.aadhar_lastfourdigit) AS MaskAadharNumber,
                mf.aadhar_number AS AadharNumber,
                mf.farmer_name_eng AS FarmerNameEng,
                mf.farmer_name_hi AS FarmerNameHi,
                mf.father_name AS FatherName,
                fs.address AS Address,
                CONCAT(
                    REPEAT('*', CHAR_LENGTH(mf.mobile_no) - 4),
                    SUBSTR(mf.mobile_no, 7, 10)
                ) AS MaskMobileNo,
                mf.mobile_no AS MobileNo,
                fs.ifsc_code AS IfscCode,
                CONCAT(
                    REPEAT('*', CHAR_LENGTH(fs.account_no) - 4),
                    SUBSTR(
                        fs.account_no,
                        CHAR_LENGTH(fs.account_no) - 3,
                        CHAR_LENGTH(fs.account_no)
                    )
                ) AS MaskAccountNo,
                fs.account_no AS AccountNo,
                fs.village_code AS VillageCode,
                h.status AS Status,
                h.status_flag AS StatusFlag,
                h.remark AS Remark,
                h.khasra_no AS KhasraNo,
                h.rakba AS Rakba,
                h.crop_name AS CropName
            FROM mas_farmer mf
            INNER JOIN farmer_society fs 
                ON fs.uf_id = mf.uf_id
            INNER JOIN horti_farmer_verify_status h 
                ON h.uf_id = mf.uf_id
            WHERE fs.village_code = @VillageCode
            AND (@StatusCode IS NULL OR h.status_flag = @StatusCode)
            ORDER BY mf.farmer_name_eng;";

            var result = await connection.QueryAsync<FarmerWiseFarmerBeneficiaryStatusDto>(
                sql,
                new
                {
                    VillageCode = villageCode,
                    StatusCode = statusCode
                });

            return Result<List<FarmerWiseFarmerBeneficiaryStatusDto>>.Success(result.ToList());
        }

    }
}
