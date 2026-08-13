namespace HortiBts.Shared.Dtos.Farmers
{
    public record FarmerBankDetailsDto
    {
        public int? UfId { get; set; } // uf_id
        public int HfId { get; set; } // hf_id
        public long FdId { get; set; } // fd_id
        public long BdId { get; set; } // bd_id
        public string BankCode { get; set; } // bank_code
        public string BankName { get; set; } // bank_name
        public string BankState { get; set; } // bank_state
        public string BankStateName { get; set; } // 
        public string BankDistrict { get; set; } // bank_district
        public string DistrictName { get; set; }
        public string PfmsFlag { get; set; } // pfms_flag
        public string AccountNo { get; set; } // account_no
        public string IfscCode { get; set; } // ifsc_code
        public string BranchCode { get; set; } // branch_code
        public string BranchName { get; set; } // branch_name
        public string DataSource { get; set; } // data_source
        public int? FinancialYear { get; set; } // financial_year
    }
}
