namespace HortiBts.Shared.Dtos.ApplicationVerification.NewFarmersApplication
{
    public record NewFarmerApplicationGirdawariDetailsDto
    {
        public int? Status { get; set; }
        public int? CropSeason { get; set; }
        public string? SubCropName { get; set; }
        public string? KhasraNo { get; set; }
        public decimal? SubCropArea { get; set; }
        public string? VillageName { get; set; }
        public string? CropYear { get; set; }
        public string CropSeasonLabel => CropSeason switch
        {
            1 => "Rabi",
            2 => "Kharif",
            3 => "Jaid",
            _ => string.Empty
        };
    }
}
