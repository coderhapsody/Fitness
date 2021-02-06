using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FitnessManagement.Data;

/// <summary>
/// Summary description for ItemAccountProvider
/// </summary>
public class ItemAccountProvider
{
	private FitnessDataContext ctx;

	public ItemAccountProvider(FitnessDataContext context)
	{
		this.ctx = context;
	}

    public void DisableAccountCascade(string accountNo, bool isActive)
    {
        ItemAccount account = Get(accountNo);
        account.IsActive = isActive;
        List<ItemAccount> accounts = GetChildAccount(accountNo).ToList();
        if (accounts.Count() == 0)
            return;
        foreach (var item in accounts)
            DisableAccountCascade(item.AccountNo, isActive);
        ctx.SubmitChanges();
    }

	public void Add(string accountNo, string description, int? parentID, bool isActive)
	{
        ItemAccount obj = new ItemAccount();
        obj.AccountNo = accountNo;
        obj.Description = description;
        obj.ParentID = parentID;
        obj.IsActive = isActive;
        EntityHelper.SetAuditFieldForInsert(obj, HttpContext.Current.User.Identity.Name);
        ctx.ItemAccounts.InsertOnSubmit(obj);
        ctx.SubmitChanges();
	}

    public void Update(int id, string accountNo, string description, int? parentID, bool isActive)
    {
        ItemAccount obj = ctx.ItemAccounts.Single(row => row.ID == id);
        obj.AccountNo = accountNo;
        obj.Description = description;
        obj.ParentID = parentID;
        obj.IsActive = isActive;
        EntityHelper.SetAuditFieldForInsert(obj, HttpContext.Current.User.Identity.Name);
        ctx.SubmitChanges();
    }

    public void Delete(int[] id)
    {
        ctx.ItemAccounts.DeleteAllOnSubmit(
            ctx.ItemAccounts.Where(row => id.Contains(row.ID)));
        ctx.SubmitChanges();
    }

    public ItemAccount Get(int id)
    {
        return ctx.ItemAccounts.SingleOrDefault(row => row.ID == id);
    }

    public ItemAccount Get(string accountNo)
    {
        return ctx.ItemAccounts.SingleOrDefault(row => row.AccountNo == accountNo);
    }

    public IEnumerable<ItemAccount> GetRootAccount()
    {
        return ctx.ItemAccounts.Where(row => !row.ParentID.HasValue);
    }

    public IEnumerable<ItemAccount> GetAll()
    {
        return ctx.ItemAccounts;
    }

    public IEnumerable<ItemAccount> GetChildAccount(int parentID)
    {
        return ctx.ItemAccounts.Where(row => row.ParentID == parentID);
    }

    public IEnumerable<ItemAccount> GetChildAccount(string accountNo)
    {
        return ctx.ItemAccounts.Where(row => row.ItemAccount1 != null && row.ItemAccount1.AccountNo == accountNo);
    }

    public IEnumerable<ItemAccount> GetValuedAccounts()
    {
        foreach (var account in ctx.ItemAccounts)
        {
            if (ctx.ItemAccounts.Count(row => row.ParentID.Value == account.ID) == 0)
                yield return account;
        }
    }

    public Stack<ItemAccount> GetAccountHierarchy(string accountNo)
    {
        Stack<ItemAccount> stack = new Stack<ItemAccount>();
        return _GetAccountHierarchy(stack, accountNo);
    }

    private Stack<ItemAccount> _GetAccountHierarchy(Stack<ItemAccount> stack, string accountNo)
    {
        ItemAccount account = ctx.ItemAccounts.Single(row => row.AccountNo == accountNo);
        if (account.ParentID.HasValue)
        {
            stack.Push(account);
            _GetAccountHierarchy(stack, Get(account.ParentID.Value).AccountNo);
        }
        else
            stack.Push(account);
        return stack;
    }
}