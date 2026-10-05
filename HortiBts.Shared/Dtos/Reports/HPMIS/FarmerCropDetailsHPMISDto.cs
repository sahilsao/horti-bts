namespace HortiBts.Shared.Dtos.HPMIS
{
    public record FarmerCropDetailsHPMISDto
    {
        public int? ApplicationId { get; set; } // application_id
        public int? CdId { get; set; } // cd_id
        public int? Fid { get; set; } // f_id
        public int? UfId { get; set; } // uf_id
        public int? UserId { get; set; } // user_id
        public string? MobileNo { get; set; } // mobile_no
        public int? VillageCode { get; set; } // village_code
        public string? VillageName { get; set; } // villcdname
        public string? KhasraNo { get; set; } // 
        public int? CropCode { get; set; } // crop_code
        public string? CropNameHi { get; set; } // crop_name_hi
        public int? CropCategory { get; set; } // crop_category
        public string? CategoryNameHi { get; set; } // category_name_hi
        public string? CropVariety { get; set; } // crop_variety
        public string? VarietyName { get; set; } // variety_name
        public int? CropSeason { get; set; } // crop_season
        public decimal? CropArea { get; set; } // crop_area
        public decimal? TotalArea { get; set; } // total_area
        public decimal? ExpectedProduction { get; set; } // expected_production
        public decimal? ActualProduction { get; set; } // actual_production
        public int? FinancialYear { get; set; } // financial_year
        public string? FinancialYearText { get; set; } // financial_year
    }
}
