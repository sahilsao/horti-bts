namespace HortiBts.Shared.Dtos.ApplicationVerification.OldFarmersApplication
{
    public record OldFarmerApplicationLandDetailsDto
    {
        public string? VillageType { get; set; }
        public int? LdId { get; set; }
        public int? HfId { get; set; }
        public int? UfId { get; set; }
        public string? FarmerNameEng { get; set; }
        public int? VillageCode { get; set; }
        public string? Vlocationcode { get; set; } // vlocationcode
        public string? Vsrno { get; set; }
        public string? VillageName { get; set; } // village_name
        public string? BookletNo { get; set; }
        public string? SinchitAsinchit { get; set; } // sinchit_asinchit
        public string? SichaiName { get; set; }
        public int? PatwariHalka { get; set; }
        public string? KhasraNo { get; set; }
        public decimal? Area { get; set; }
        public int? Verify { get; set; } // verify
        public string? IsVerified { get; set; } // is_verified
        public string? SerarchBykhasraNo { get; set; }
        public string? Ownertype { get; set; }
        public DateTime? Bhuiyanlastupdatedon { get; set; }
    }
}
