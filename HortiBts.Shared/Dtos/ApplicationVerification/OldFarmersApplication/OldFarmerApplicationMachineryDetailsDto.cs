namespace HortiBts.Shared.Dtos.ApplicationVerification.OldFarmersApplication
{
    public record OldFarmerApplicationMachineryDetailsDto
    {
        public int? EquipmentId { get; set; } // equipment_id
        public int? HfId { get; set; } // hf_id
        public int? FdId { get; set; } // fd_id
        public int? UfId { get; set; } // uf_id
        public int? VillageCode { get; set; } // village_code
        public string? VillageName { get; set; } // village_name
        public int? MachineryId { get; set; } // machinary_id
        public string? MachineryName { get; set; } // 
        public string? MachineryCategory { get; set; } // machinary_category
        public string? MachineryCode { get; set; } // 
        public int? Quantity { get; set; } // quantity
        public int? FinancialYear { get; set; } // financial_year
    }
}
