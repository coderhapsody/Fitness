using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FitnessManagement.Data;

/// <summary>
/// Summary description for ItemProvider
/// </summary>
namespace FitnessManagement.Providers
{
    public class ItemProvider
    {
        FitnessDataContext ctx;
        public ItemProvider(FitnessDataContext context)
        {
            this.ctx = context;
        }

        public IEnumerable<Item> GetMonthlyDuesItem()
        {
            return from item in ctx.Items
                   join itemType in ctx.ItemTypes on item.ItemTypeID equals itemType.ID
                   where itemType.Description == "Monthly Dues"
                   select item;            
        }

        public void Add(
            string barcode,
            string description,
            int itemAccountID,
            int itemTypeID,
            decimal standardUnitPrice,
            bool isActive)
        {
            Item item = new Item();
            item.Barcode = barcode;
            item.Description = description;
            item.ItemAccountID = itemAccountID;
            item.ItemTypeID = itemTypeID;
            item.UnitPrice = standardUnitPrice;
            item.IsActive = isActive;
            ctx.Items.InsertOnSubmit(item);
            EntityHelper.SetAuditFieldForInsert(item, HttpContext.Current.User.Identity.Name);
            ctx.SubmitChanges();
        }

        public void Update(
            int id,
            string barcode,
            string description,
            int itemAccountID,
            int itemTypeID,
            decimal standardUnitPrice,
            bool isActive)
        {
            Item item = ctx.Items.Single(row => row.ID == id);
            item.Barcode = barcode;
            item.Description = description;
            item.ItemAccountID = itemAccountID;
            item.ItemTypeID = itemTypeID;
            item.UnitPrice = standardUnitPrice;
            item.IsActive = isActive;
            EntityHelper.SetAuditFieldForUpdate(item, HttpContext.Current.User.Identity.Name);
            ctx.SubmitChanges();
        }

        public void Delete(int[] id)
        {
            ctx.Items.DeleteAllOnSubmit(
                ctx.Items.Where(row => id.Contains(row.ID)));
            ctx.SubmitChanges();
        }

        public Item Get(int id)
        {
            return ctx.Items.SingleOrDefault(row => row.ID == id);
        }

        public IEnumerable<Item> GetAll()
        {
            return from item in ctx.Items                   
                   select item;
        }

        public IEnumerable<Item> GetAll(int[] branchesID)
        {
            return from item in ctx.Items
                   join itembranch in ctx.ActiveItemAtBranches on item.ID equals itembranch.ItemID
                   where item.IsActive && branchesID.Contains(itembranch.BranchID)
                   select item;
        }

        public IEnumerable<Item> GetItemsByType(int itemTypeID)
        {
            return ctx.Items.Where(item => item.ItemTypeID == itemTypeID && item.IsActive);
        }

        public IEnumerable<int> GetBranchesByItem(int itemID)
        {
            return from item in ctx.Items
                   join itembranch in ctx.ActiveItemAtBranches on item.ID equals itembranch.ItemID
                   where item.ID == itemID
                   select itembranch.BranchID;
        }

        public void UpdateItemsAtBranch(int itemID, IEnumerable<int> branchesID)
        {
            ctx.ActiveItemAtBranches.DeleteAllOnSubmit(
                ctx.ActiveItemAtBranches.Where(row => row.ItemID == itemID));

            foreach (int branchID in branchesID)
                ctx.ActiveItemAtBranches.InsertOnSubmit(
                    new ActiveItemAtBranch()
                    {
                        ItemID = itemID,
                        BranchID = branchID
                    });

            ctx.SubmitChanges();
        }
    }
}