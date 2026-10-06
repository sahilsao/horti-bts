namespace HortiBts.Shared.Dtos.ApplicationVerification.FarmersApplication
{
    public record FarmerApplicationBankDetailsDto
    {
        public int? ApplicationId { get; set; } // application_id
        public int? UfId { get; set; } // uf_id
        public int? HfId { get; set; } // hf_id
        public int? FdId { get; set; } // fd_id
        public int? BdId { get; set; } // bd_id
        public int? BankCode { get; set; } // bank_code
        public string? BankName { get; set; } // bank_name
        public int? BankState { get; set; } // bank_state
        public string? BankStateName { get; set; } // 
        public int? BankDistrict { get; set; } // bank_district
        public string? DistrictName { get; set; }
        public string? PfmsFlag { get; set; } // pfms_flag
        public string? AccountNo { get; set; } // account_no
        public string? IfscCode { get; set; } // ifsc_code
        public string? BranchCode { get; set; } // branch_code
        public string? BranchName { get; set; } // branch_name
        public string? DataSource { get; set; } // data_source
        public int? FinancialYear { get; set; } // financial_year
    }
}
