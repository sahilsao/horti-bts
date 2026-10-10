namespace HortiBts.Shared.Dtos.ApplicationVerification.OldFarmersApplication
{
    public record OldFarmerApplicationBankDetailsDto
    {
        public int? HfId { get; set; } // hf_id
        public int? FdId { get; set; } // fd_id
        public int? FsId { get; set; } // fs_id
        public int? UfId { get; set; } // uf_id
        public int? BankState { get; set; } // bank_state
        public int? BankDistrict { get; set; } // bank_district
        public int? BankCode { get; set; } // bank_code
        public string? BankName { get; set; } // bank_name
        public string? BranchName { get; set; } // branch_name
        public string? BranchCode { get; set; } // branch_code
        public string? IfscCode { get; set; } // ifsc_code
        public string? AccountNo { get; set; } // account_no
        public int? BdId { get; set; } // bd_id
        public int? FinancialYear { get; set; } // financial_year
    }
}
