using FitnessManagement.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for BranchRoomCapacityViewModel
/// </summary>
public class BranchRoomCapacityViewModel
{
    public int BranchID { get; set; }
    public Branch CurrentBranch { get; set; }
    public int Capacity { get; set; }

}