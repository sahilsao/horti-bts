namespace HortiBts.Shared.Dtos.Farmers
{
    public record FarmerCropDetailsDto
    {
        public int? UfId { get; set; } // uf_id
        public int HfId { get; set; } // hf_id
        public long FdId { get; set; } // fd_id
        public int CdId { get; set; } // cd_id
        public int VillageCode { get; set; } // village_code
        public string? KhasraNo { get; set; } // khasra_no
        public int CropCode { get; set; } // crop_code
        public int CropCategory { get; set; } // crop_category
        public string? CropVariety { get; set; } // ccrop_veriety
        public SByte CropSeason { get; set; } // crop_season
        public string? CropSeasonName { get; set; } // 
        public decimal CropArea { get; set; } // ccrop_area
        public string? Type { get; set; } // type
        public string? CropStatus { get; set; } // crop_status
        public string? CropName { get; set; } // crop_name
        public string? CropCategoryName { get; set; } // subcrop
        public string? VillageName { get; set; } // village_name
        public string? DistrictName { get; set; }
        public string? SubdistrictName { get; set; } // subdistrict_name
        public int? FinancialYear { get; set; } // financial_year
        public string? FinancialYearStr { get; set; } // financial_year
    }
}
