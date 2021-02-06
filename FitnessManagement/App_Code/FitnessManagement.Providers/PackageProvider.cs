using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FitnessManagement.Data;
using FitnessManagement.ViewModels;

/// <summary>
/// Summary description for PackageProvider
/// </summary>
namespace FitnessManagement.Providers
{
    public class PackageProvider
    {
        private FitnessDataContext ctx;

        public PackageProvider(FitnessDataContext context)
        {
            this.ctx = context;
        }

        public void Add(
            string name,
            int packageDuesInMonth,
            bool isActive,
            bool isOpenEnd,
            decimal freezeFee,
            IEnumerable<PackageDetailViewModel> detail)            
        {            
            PackageHeader head = new PackageHeader();
            head.Name = name;
            head.PackageDuesInMonth = packageDuesInMonth;
            head.IsActive = isActive;
            head.OpenEnd = isOpenEnd;
            head.FreezeFee = freezeFee;
            EntityHelper.SetAuditFieldForInsert(head, HttpContext.Current.User.Identity.Name);
            ctx.PackageHeaders.InsertOnSubmit(head);
            foreach (PackageDetailViewModel item in detail)
            {
                PackageDetail obj = new PackageDetail();
                obj.ItemID = item.ItemID;
                obj.PackageID = item.PackageID;
                obj.Quantity = item.Quantity;
                obj.UnitPrice = item.UnitPrice;
                head.PackageDetails.Add(obj);
                ctx.PackageDetails.InsertOnSubmit(obj);
            }
            ctx.SubmitChanges();
        }

        public void Update(
            int id,
            string name,
            int packageDuesInMonth,
            bool isActive,
            bool isOpenEnd,
            decimal freezeFee,
            IEnumerable<PackageDetailViewModel> detail)
        {
            ctx.PackageDetails.DeleteAllOnSubmit(
                ctx.PackageDetails.Where(row => row.PackageID == id));

            PackageHeader head = ctx.PackageHeaders.Single(row => row.ID == id);
            head.Name = name;
            head.PackageDuesInMonth = packageDuesInMonth;
            head.IsActive = isActive;
            head.OpenEnd = isOpenEnd;
            head.FreezeFee = freezeFee;
            EntityHelper.SetAuditFieldForUpdate(head, HttpContext.Current.User.Identity.Name);
            foreach (PackageDetailViewModel item in detail)
            {                
                PackageDetail obj = new PackageDetail();
                obj.ItemID = item.ItemID;
                obj.PackageID = item.PackageID;
                obj.Quantity = item.Quantity;
                obj.UnitPrice = item.UnitPrice;                
                head.PackageDetails.Add(obj);
                ctx.PackageDetails.InsertOnSubmit(obj); 
            }
            ctx.SubmitChanges();
        }

        public void Delete(int[] id)
        {
            foreach (int item in id)
            {
                ctx.PackageDetails.DeleteAllOnSubmit(
                    ctx.PackageDetails.Where(row => row.PackageID == item));

                ctx.ActivePackageAtBranches.DeleteAllOnSubmit(
                    ctx.ActivePackageAtBranches.Where(package => package.PackageID == item));
            }
            ctx.PackageHeaders.DeleteAllOnSubmit(
                ctx.PackageHeaders.Where(row => id.Contains(row.ID)));

            

            ctx.SubmitChanges();
        }

        public PackageHeader Get(int id)
        {
            return ctx.PackageHeaders.SingleOrDefault(row => row.ID == id);
        }

        public IEnumerable<PackageDetailViewModel> GetDetail(int id)
        {
            return from head in ctx.PackageHeaders
                   join detail in ctx.PackageDetails on head.ID equals detail.PackageID
                   join item in ctx.Items on detail.ItemID equals item.ID
                   where head.ID == id && detail.PackageID == id
                   select new PackageDetailViewModel
                   {
                       ID = detail.ID,
                       ItemID = item.ID,
                       ItemBarcode = item.Barcode,
                       ItemDescription = item.Description,
                       PackageID = head.ID,
                       Quantity = detail.Quantity,
                       UnitPrice = detail.UnitPrice,
                       Discount = 0,
                       IsTaxed = true,
                       NetAmount = detail.Quantity * detail.UnitPrice / 1.1M,
                       Total = detail.Quantity * detail.UnitPrice
                   };
        }

        public IEnumerable<int> GetBranchesByPackage(int packageID)
        {
            return from package in ctx.PackageHeaders
                   join packagebranch in ctx.ActivePackageAtBranches on package.ID equals packagebranch.PackageID
                   where package.ID == packageID
                   select packagebranch.BranchID;
        }

        public IEnumerable<PackageHeader> GetPackagesInBranch(int branchID)
        {
            return from package in ctx.PackageHeaders
                   join packagebranch in ctx.ActivePackageAtBranches on package.ID equals packagebranch.PackageID
                   where packagebranch.BranchID == branchID
                   orderby package.Name
                   select package;
        }

        public IEnumerable<PackageHeader> GetAll()
        {
            return from package in ctx.PackageHeaders
                   where package.IsActive
                   select package;
        }

        public void UpdatePackagesAtBranch(int packageID, IEnumerable<int> branchesID)
        {
            ctx.ActivePackageAtBranches.DeleteAllOnSubmit(
                ctx.ActivePackageAtBranches.Where(row => row.PackageID == packageID));
            foreach (int branchID in branchesID)
                ctx.ActivePackageAtBranches.InsertOnSubmit(
                    new ActivePackageAtBranch()
                    {
                        BranchID = branchID,
                        PackageID = packageID
                    });
            ctx.SubmitChanges();
        }

        public IEnumerable<Class> PopulateClassForSelectedPackage(int packageID)
        {
            Dictionary<int, bool> result = new Dictionary<int, bool>();

            var classes = from acls in ctx.ActiveClassInPackages
                          join cls in ctx.Classes on acls.ClassID equals cls.ID
                          where cls.IsActive && acls.PackageID == packageID
                          select cls;
            return classes;
        }


        public void UpdateClassPackage(int packageID, List<int> classesID)
        {
            ctx.ActiveClassInPackages.DeleteAllOnSubmit(
                ctx.ActiveClassInPackages.Where(item => item.PackageID == packageID));
            
            foreach (int classID in classesID)
            {
                ActiveClassInPackage activeClassInPackage = new ActiveClassInPackage();
                activeClassInPackage.PackageID = packageID;
                activeClassInPackage.ClassID = classID;

                ctx.ActiveClassInPackages.InsertOnSubmit(activeClassInPackage);                
            }
            ctx.SubmitChanges();
        }
    }
}