using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Microsoft.Practices.Unity;
using FitnessManagement.Data;
using FitnessManagement.Providers;

public partial class MasterItemAccount : System.Web.UI.Page
{
    #region Template
    public int RowID { get { return Convert.ToInt32(ViewState["_ID"]); } set { ViewState["_ID"] = value; } }
    #endregion

    ItemAccountProvider itemAccountProvider = UnityContainerHelper.Container.Resolve<ItemAccountProvider>();

    protected void Page_Load(object sender, EventArgs e)
    {
        this.ApplyUserSecurity(lnbAddNew, lnbDelete, btnSave, tvwAccount);

        if (!this.IsPostBack)
        {
            FillParentAccountDropDown();
            RowID = 0;
            RefreshTreeView();
        }
    }

    private void FillParentAccountDropDown()
    {
        ddlParentAccount.DataSource = itemAccountProvider.GetAll();
        ddlParentAccount.DataTextField = "Description";
        ddlParentAccount.DataValueField = "ID";
        ddlParentAccount.DataBind();
        ddlParentAccount.Items.Insert(0, new ListItem("Root Account", "0"));
    }
    protected void btnSave_Click(object sender, EventArgs e)
    {
        try
        {
            switch (RowID)
            {
                case 0:
                    itemAccountProvider.Add(
                        txtAccountNo.Text,
                        txtAccountDescription.Text,
                        Convert.ToInt32(ddlParentAccount.SelectedValue) == 0 ? null : (int?)Convert.ToInt32(ddlParentAccount.SelectedValue),
                        chkActive.Checked);
                    break;
                default:
                    itemAccountProvider.Update(
                        RowID,
                        txtAccountNo.Text,
                        txtAccountDescription.Text,
                        Convert.ToInt32(ddlParentAccount.SelectedValue) == 0 ? null : (int?)Convert.ToInt32(ddlParentAccount.SelectedValue),
                        chkActive.Checked);
                    break;
            }

            if (chkCascade.Checked)
                itemAccountProvider.DisableAccountCascade(txtAccountNo.Text, chkActive.Checked);

            FillParentAccountDropDown();
            RefreshTreeView();
            RestoreTreeView(txtAccountNo.Text);
            WebFormHelper.SetLabelTextWithCssClass(
                    lblMessage,
                    String.Format("Account <b>{0}</b> has been saved.", txtAccountNo.Text),
                    LabelStyleNames.AlternateMessage);

            lnbAddNew_Click(sender, e);
        }
        catch (Exception ex)
        {           
            WebFormHelper.SetLabelTextWithCssClass(lblMessage, ex.Message, LabelStyleNames.ErrorMessage);
        }
    }

    private void RestoreTreeView(string fromAccountNo)
    {
        Stack<ItemAccount> stack = itemAccountProvider.GetAccountHierarchy(fromAccountNo);
        TreeNodeCollection nodes = tvwAccount.Nodes;
        while (stack.Count > 0)
        {
            ItemAccount account = stack.Pop();            
            foreach(TreeNode node in nodes)
            {
                if (Convert.ToInt32(node.Value) == account.ID)
                {
                    node.Expand();
                    nodes = node.ChildNodes;
                    break;
                }
            }
            
        }
    }



    private void RefreshTreeView()
    {
        tvwAccount.Nodes.Clear();
        foreach (ItemAccount account in itemAccountProvider.GetRootAccount())
        {
            TreeNode node = new TreeNode(String.Format("{0}{1} - {2}", account.IsActive ? String.Empty : "*",  account.AccountNo , account.Description), account.ID.ToString());
            node.PopulateOnDemand = true;
            node.SelectAction = TreeNodeSelectAction.SelectExpand;
            node.Expanded = false;            
            tvwAccount.Nodes.Add(node);
        }
    }

    protected void lnbAddNew_Click(object sender, EventArgs e)
    {
        RowID = 0;
        WebFormHelper.ClearTextBox(txtAccountNo, txtAccountDescription);
        ddlParentAccount.SelectedValue = "0";
        chkActive.Checked = true;
        chkCascade.Checked = false;
        txtAccountNo.Focus();
    }

    protected void lnbDelete_Click(object sender, EventArgs e)
    {
        try
        {
            if (RowID > 0)
            {
                int id = Convert.ToInt32(tvwAccount.SelectedNode.Value);
                ItemAccount currentAccount = itemAccountProvider.Get(id);
                IEnumerable<ItemAccount> account = itemAccountProvider.GetChildAccount(id);
                if (account.Count() > 0)
                {
                    WebFormHelper.SetLabelTextWithCssClass(
                        lblMessage,
                        String.Format("Cannot delete account <b>{0}</b> since it has one or more child accounts", currentAccount.AccountNo),
                        LabelStyleNames.ErrorMessage);
                }
                else
                {
                    itemAccountProvider.Delete(new int[] { id });
                    RefreshTreeView();
                    WebFormHelper.SetLabelTextWithCssClass(
                        lblMessage,
                        String.Format("Account <b>{0}</b> has been deleted.", currentAccount.AccountNo),
                        LabelStyleNames.AlternateMessage);
                }
            }
            else
            {
                WebFormHelper.SetLabelTextWithCssClass(
                        lblMessage,
                        "No account selected, select an account first.",
                        LabelStyleNames.ErrorMessage);
            }
            RefreshTreeView();
        }
        catch (Exception ex)
        {            
            WebFormHelper.SetLabelTextWithCssClass(lblMessage, ex.Message, LabelStyleNames.ErrorMessage);
        }
    }

    protected void tvwAccount_SelectedNodeChanged(object sender, EventArgs e)
    {
        int id = Convert.ToInt32(tvwAccount.SelectedNode.Value);
        RowID = id;
        ItemAccount account = itemAccountProvider.Get(id);
        txtAccountNo.Text = account.AccountNo;
        txtAccountDescription.Text = account.Description;
        chkActive.Checked = account.IsActive;
        chkCascade.Checked = false;
        ddlParentAccount.SelectedValue = account.ParentID.HasValue ? account.ParentID.Value.ToString() : "0";
    }
    protected void tvwAccount_TreeNodePopulate(object sender, TreeNodeEventArgs e)
    {
        int id = Convert.ToInt32(e.Node.Value);
        foreach (ItemAccount account in itemAccountProvider.GetChildAccount(id))
        {
            TreeNode node = new TreeNode();
            node.Text = String.Format("{0}{1} - {2}", account.IsActive ? String.Empty : "*", account.AccountNo, account.Description);
            node.Value = account.ID.ToString();
            node.PopulateOnDemand = true;
            node.SelectAction = TreeNodeSelectAction.SelectExpand;
            node.Expanded = false;
            e.Node.ChildNodes.Add(node);
        }
    }
}