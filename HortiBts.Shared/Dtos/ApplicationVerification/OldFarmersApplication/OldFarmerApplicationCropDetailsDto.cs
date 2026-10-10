namespace HortiBts.Shared.Dtos.ApplicationVerification.OldFarmersApplication
{
    public record OldFarmerApplicationCropDetailsDto
    {
        public int? CdId { get; set; } // cd_id
        public int? FdId { get; set; } // fd_id
        public int? HfId { get; set; } // hf_id
        public int? UfId { get; set; } // uf_id
        public int? VillageCode { get; set; } // village_code
        public string? VillageName { get; set; } // village_name
        public string? KhasraNo { get; set; } // khasra_no
        public int? CropCode { get; set; } // crop_code
        public string? CropName { get; set; } // 
        public int? CropCategory { get; set; } // crop_category
        public string? SubCropCategoryName { get; set; } // subcrop
        public string? CropVariety { get; set; } // ccrop_veriety
        public int? CropSeason { get; set; } // crop_season
        public decimal? CropArea { get; set; } // ccrop_area
        public string? VillageType { get; set; } // village_type
        public int? IsJoin { get; set; } // is_join
        public int? IsSeed { get; set; } // is_seed
        public int? FinancialYear { get; set; } // financial_year
    }
}
