using System;
using System.ComponentModel.DataAnnotations;

namespace HortiBts.Shared.Dtos.Users;

public class DistrictUsersDto
{
    public int? DistrictCode { get; set; } // district_code
    public string? DistrictName { get; set; } // district_name
    public string? DistrictNameHindi { get; set; }
    public int? DepartmentCode { get; set; } // department_code
    public string? DepartmentNameEng { get; set; } // department_name_eng
    public string? DepartmentName { get; set; } // department_name
    public int? Usertype { get; set; } // usertype
}

