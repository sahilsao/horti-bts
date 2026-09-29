using System;

namespace HortiBts.Shared.Dtos.Reports.HPMIS;

public class FarmersListHPMISDto
{
    public int? ApplicationId { get; set; }      // fa.application_id
    public int? FId { get; set; }                 // f.f_id
    public string? UfId { get; set; }              // f.uf_id
    public int? UserId { get; set; }               // f.user_id
    public string? FarmerNameEng { get; set; }      // f.farmer_name_eng
    public string? FarmerNameHi { get; set; }       // f.farmer_name_hi
    public string? CareOfName { get; set; }         // f.care_of_name
    public string? RelName { get; set; }            // r.rel_name
    public string? CasteName { get; set; }          // cc.caste_name
    public string? SubcasteName { get; set; }       // cc.subcaste_name
    public string? GenderNameHi { get; set; }       // g.gender_name_hi
    public string? MobileNo { get; set; }           // f.mobile_no
    public string? VillageName { get; set; }        // v.villcdname AS village_name
    public int? VillageCode { get; set; }           // ld.village_code
    public decimal? TotalArea { get; set; }         // ld.total_area
    public string? CropName { get; set; }           // cd.crop_name (GROUP_CONCAT result)
    public decimal? CropArea { get; set; }          // cd.crop_area
    public string? SeasonNameHi { get; set; }       // cd.season_name_hi
    public decimal? ExpectedProduction { get; set; }// cd.expected_production
    public string? FinancialYear { get; set; }      // cd.financial_year
}
