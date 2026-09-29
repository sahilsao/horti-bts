using System;

namespace HortiBts.Shared.Dtos.Reports.Component;

public class ComponentWiseRegistrationDto
{
    public int CId { get; set; }
    public string? CName { get; set; }
    public int SId { get; set; }
    public string? SchemeName { get; set; }
    public string? SchemeType { get; set; }
    public int StId { get; set; }
    public int F13 { get; set; }
    public int F12 { get; set; }
    public int F11 { get; set; }
    public int F10 { get; set; }
}
