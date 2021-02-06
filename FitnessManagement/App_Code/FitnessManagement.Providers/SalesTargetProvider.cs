using FitnessManagement.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for SalesTargetProvider
/// </summary>
public class SalesTargetProvider
{
    private FitnessManagement.Data.FitnessDataContext ctx;

	public SalesTargetProvider(FitnessManagement.Data.FitnessDataContext ctx)
	{
        this.ctx = ctx;
	}

    public SalesTarget GetTarget(int id)
    {
        return ctx.SalesTargets.SingleOrDefault(target => target.ID == id);
    }

    public void AddTarget(int branchID, int year, int month,
        int freshMemberUnit,
        int upgradeUnit,
        int renewalUnit,
        decimal freshMemberRevenue,
        decimal upgradeRevenue,
        decimal renewalRevenue,
        decimal pilatesRevenue,
        decimal vocalRevenue,
        decimal eftCollectionRevenue,
        int dropOffUnit,        
        decimal cancelFees,
        int freezeUnit,
        decimal freezeFees,
        decimal otherRevenue)
    {
        SalesTarget salesTarget = new SalesTarget();
        salesTarget.BranchID = branchID;
        salesTarget.Year = year;
        salesTarget.Month = month;
        salesTarget.FreshMemberUnit = freshMemberUnit;
        salesTarget.RenewalUnit = renewalUnit;
        salesTarget.UpgradeUnit = upgradeUnit;
        salesTarget.FreshMemberRevenue = freshMemberRevenue;
        salesTarget.RenewalRevenue = renewalRevenue;
        salesTarget.UpgradeRevenue = upgradeRevenue;
        salesTarget.PilatesRevenue = pilatesRevenue;
        salesTarget.VocalRevenue = vocalRevenue;
        salesTarget.EFTCollectionRevenue = eftCollectionRevenue;
        salesTarget.DropOffUnit = dropOffUnit;        
        salesTarget.CancelFees = cancelFees;
        salesTarget.FreezeFees = freezeFees;
        salesTarget.FreezeUnit = freezeUnit;
        salesTarget.OtherRevenue = otherRevenue;
        EntityHelper.SetAuditFieldForInsert(salesTarget, HttpContext.Current.User.Identity.Name);
        ctx.SalesTargets.InsertOnSubmit(salesTarget);
        ctx.SubmitChanges();
    }

    public void UpdateTarget(int id, int branchID, int year, int month,
        int freshMemberUnit,
        int upgradeUnit,
        int renewalUnit,
        decimal freshMemberRevenue,
        decimal upgradeRevenue,
        decimal renewalRevenue,
        decimal pilatesRevenue,
        decimal vocalRevenue,
        decimal eftCollectionRevenue,
        int dropOffUnit,        
        decimal cancelFees,
        int freezeUnit,
        decimal freezeFees,
        decimal otherRevenue)
    {
        SalesTarget salesTarget = ctx.SalesTargets.SingleOrDefault(target => target.ID == id);
        if (salesTarget != null)
        {
            salesTarget.BranchID = branchID;
            salesTarget.Year = year;
            salesTarget.Month = month;
            salesTarget.FreshMemberUnit = freshMemberUnit;
            salesTarget.RenewalUnit = renewalUnit;
            salesTarget.UpgradeUnit = upgradeUnit;
            salesTarget.FreshMemberRevenue = freshMemberRevenue;
            salesTarget.RenewalRevenue = renewalRevenue;
            salesTarget.UpgradeRevenue = upgradeRevenue;
            salesTarget.PilatesRevenue = pilatesRevenue;
            salesTarget.VocalRevenue = vocalRevenue;
            salesTarget.EFTCollectionRevenue = eftCollectionRevenue;
            salesTarget.DropOffUnit = dropOffUnit;            
            salesTarget.CancelFees = cancelFees;
            salesTarget.FreezeFees = freezeFees;
            salesTarget.FreezeUnit = freezeUnit;
            salesTarget.OtherRevenue = otherRevenue;
            EntityHelper.SetAuditFieldForUpdate(salesTarget, HttpContext.Current.User.Identity.Name);
            ctx.SubmitChanges();
        }
    }

    public void DeleteTarget(int id)
    {
        ctx.SalesTargets.DeleteOnSubmit(
            ctx.SalesTargets.SingleOrDefault(target => target.ID == id));
        ctx.SubmitChanges();
    }

    public void DeleteTarget(int[] id)
    {
        ctx.SalesTargets.DeleteAllOnSubmit(
            ctx.SalesTargets.Where(target => id.Contains(target.ID)));
        ctx.SubmitChanges();
    }
}