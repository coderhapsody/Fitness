using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FitnessManagement.Data;

namespace FitnessManagement.Providers
{
    public class BranchProvider
    {
        private FitnessDataContext ctx;
        public BranchProvider(FitnessDataContext context)
        {
            this.ctx = context;
        }

        public string GetBranchName(int branchID)
        {
            string result = String.Empty;
            Branch branch = ctx.Branches.Where(br => br.ID == branchID).SingleOrDefault();
            result = branch == null ? String.Empty : branch.Name;
            return result;
        }

        public IEnumerable<Branch> GetAll()
        {
            return ctx.Branches;
        }

        public IEnumerable<Branch> GetActiveBranches()
        {
            return from branch in ctx.Branches
                   where branch.IsActive == true
                   select branch;
        }

        public IEnumerable<Branch> GetActiveBranches(string userName)
        {
            return from branch in ctx.Branches
                    join userAtBranch in ctx.UserAtBranches on branch equals userAtBranch.Branch
                   where userAtBranch.UserName == userName
                   select branch;
        }   

        public IEnumerable<string> GetBranchName(IEnumerable<int> branchesID)
        {
            return ctx.Branches.Where(row => branchesID.Contains(row.ID)).Select(row => row.Name);
        }

        public void Delete(int[] branchesID)
        {
            ctx.Branches.DeleteAllOnSubmit(ctx.Branches.Where(row => branchesID.Contains(row.ID)));
            ctx.SubmitChanges();
        }

        public void Add(
            string code,
            string name,
            string address,
            string phone,
            string email,
            string merchantCode,
            bool isActive)
        {
            Branch br = new Branch();
            br.Code = code;
            br.Name = name;
            br.Address = address;
            br.Phone = phone;
            br.Email = email;
            br.MerchantCode = merchantCode;
            br.IsActive = isActive;
            EntityHelper.SetAuditFieldForInsert(br, HttpContext.Current.User.Identity.Name);
            ctx.Branches.InsertOnSubmit(br);
            ctx.SubmitChanges();
        }

        public void Update(
            int id,
            string code,
            string name,
            string address,
            string phone,
            string email,
            string merchantCode,
            bool isActive)
        {
            Branch br = ctx.Branches.Where(row => row.ID == id).SingleOrDefault();
            br.Code = code;
            br.Name = name;
            br.Address = address;
            br.Phone = phone;
            br.Email = email;
            br.MerchantCode = merchantCode;
            br.IsActive = isActive;
            EntityHelper.SetAuditFieldForUpdate(br, HttpContext.Current.User.Identity.Name);
            ctx.SubmitChanges();
        }

        public Branch Get(int branchID)
        {
            return ctx.Branches.Where(row => row.ID == branchID).SingleOrDefault();
        }
    }
}
