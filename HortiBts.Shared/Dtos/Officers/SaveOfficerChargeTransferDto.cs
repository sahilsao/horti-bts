using System;

namespace HortiBts.Shared.Dtos.Officers;

public class SaveOfficerChargeTransferDto
{
    public int SourceRole { get; set; }                  // transferorData.role  (designation code, e.g. 3)
    public int TargetRole { get; set; }                  // transfereeData.role
    public int SourceOfficerCode { get; set; }           // 0 when only vacant villages are assigned
    public int TargetOfficerCode { get; set; }           // always required
    public List<int> SourceVillageCodes { get; set; } = new();
    public List<int> VacantVillageCodes { get; set; } = new();

}
public class ChargeTransferResultDto
{
    public int TransferredVillages { get; set; }          // Node: history_village_count
    public int AssignedVacantVillages { get; set; }       // Node: inserted_not_mapped_villages_count
}
