/*
	Utils.js
	
	Miscellaneous javascript functions
	
	(c)Copyright Paulus Iman, 2006-2009
*/

function SaveConfirm(sender, args) {
    args.set_cancel(!window.confirm("Are you sure you want to save?"));
}

function VoidConfirm(sender, args) {
    args.set_cancel(!window.confirm("Are you sure you want to void?"));
}

function DeleteConfirm(sender, args) {
    args.set_cancel(!window.confirm("Are you sure you want to delete?"));
}

function ApproveConfirm(sender, args) {
    args.set_cancel(!window.confirm("Are you sure you want to approve?"));
}

function CancelConfirm(sender, args) {
    args.set_cancel(!window.confirm("Are you sure you want to cancel?"));
}

function CancelReturnKey(e) {
	var blnReturnValue = true;
	var the_key = (window.event) ? window.event.keyCode : e.which;

	if (the_key == 13) {
		blnReturnValue = false;
	}
	return blnReturnValue;

}

function ValidateDate(sender, args) {
	if (args.Value.length < 10) {
		args.IsValid = false;
	}
	else {
	    var dt = args.Value;
        try{
            var dteDate = Date.parse(dt);
            args.IsValid = true;
        }
        catch (e){
            args.IsValid = false;
        }
	}
}

function showPromptPopUp(url, callbackWidget, height, width) {
	showPopUp(url, callbackWidget, 'prompt', height, width, 'yes', 'yes', 'no', 'yes');
}

/*
	ShowPopUp will open a new window using given property values.
	url             web page to be opened
	windowName      window name identifier
	heightWindow    height size (in pixel)
	widthWindow     width size (in pixel)
	resizable       resizable visibility flag (yes | no)
	showScrollBar   scrollbar visibility flag (yes | no)
	showToolBar     toolbar visibility flag (yes | no)
	showStatusBar   statusbar visibility flag (yes | no)
*/
function showPopUp(url, callBackWidgetName, windowName, heightWindow, widthWindow, resizable, showScrollBar, showToolBar, showStatusBar)
{    
	var dimensionValue = 'alwaysRaised=yes,modal=1,dialog=yes,minimizable=no,location=no,left=100,top=50'
	dimensionValue += ',height=' + heightWindow 
	dimensionValue += ',width=' + widthWindow        
	dimensionValue += ',resizable=' + resizable
	dimensionValue += ',scrollbars=' + showScrollBar
	dimensionValue += ',toolbar=' + showToolBar
	dimensionValue += ',status=' + showStatusBar

	url += '&callback=' + callBackWidgetName 
	//alert(dimensionValue);
	var newWindow = window.open(url , windowName, dimensionValue);
	if (window.focus) 
	{
	  newWindow.focus()
	}
}

function showModalPopUp(url, callBackWidgetName, windowName, heightWindow, widthWindow, resizable, showScrollBar, showToolBar, showStatusBar) {
	var dimensionValue = 'alwaysRaised=yes,modal=1,dialog=yes,minimizable=no,location=no'
	dimensionValue += ',height=' + heightWindow
	dimensionValue += ',width=' + widthWindow
	dimensionValue += ',resizable=' + resizable
	dimensionValue += ',scrollbars=' + showScrollBar
	dimensionValue += ',toolbar=' + showToolBar
	dimensionValue += ',status=' + showStatusBar

	url += '&callback=' + callBackWidgetName
	//alert(dimensionValue);
	var newWindow = window.showModalDialog(url, windowName, dimensionValue);
	if (window.focus) {
		newWindow.focus()
	}
}

function showSimplePopUp(url)
{
	var newWindow = window.open(url , "previewReport", 'alwaysRaised=yes,modal=1,dialog=yes,minimizable=no,location=no,resizable=yes,width=930,height=600,scrollbars=yes,toolbar=no,status=no,left=50,top=50');
	if (window.focus) 
	{
	  newWindow.focus()
	}    
}

function NumbersOnly(e) {    
    var unicode = e.charCode ? e.charCode : e.keyCode
    
    if (unicode >= 96 && unicode <= 105)
        return true;
    else
	    if (unicode!=8) { //if the key isn't the backspace key (which we should allow)
		    if (unicode!=9) { //if the key isn't the tab key (which we should allow)
			    if (unicode!=37) { //if the key isn't the left key (which we should allow)
				    if (unicode!=39) { //if the key isn't the right key (which we should allow)
					    if (unicode!=46) { //if the key isn't the delete key (which we should allow)
						    if (unicode<48||unicode>57) //if not a number
							    return false //disable key press
					    }
				    }
			    }
		    }
	    }
}

function NoType(e) {
	return false;
}

function IsPositiveNumber(source, args)
{ 
  args.IsValid = false; 
  if(!isNaN(args.Value))
  {
	args.IsValid = parseFloat(args.Value) >= 0;
	
  }      
}

	
	//-------------------------------------------------------------
	// Select all the checkboxes (Hotmail style)
	// Note: Please note this will select all the check boxes on the
	// form.
	//-------------------------------------------------------------
	function SelectAllCheckboxes(spanChk){
	
	// Added as ASPX uses SPAN for checkbox 
	var xState=spanChk.checked;
	var theBox=spanChk;
	
	// Old code for VS 2002 / .NET framework 1.0 code
	//-----------------------------------------------
	// In .NET 1.0 ASP.NET was using SPAN tag with
	// CheckBox control. 
	//
	//var oItem = spanChk.children;
	//var theBox=oItem.item(0)
	//xState=theBox.checked;	
	//-----------------------------------------------
	
		elm=theBox.form.elements;
		for(i=0;i<elm.length;i++)
		if(elm[i].type=="checkbox" && elm[i].id!=theBox.id)
			{
			//elm[i].click();
			if(elm[i].checked!=xState)
			elm[i].click();
			//elm[i].checked=xState;
			}
	}

	//-------------------------------------------------------------
	//----Select highlish rows when the checkboxes are selected
	//
	// Note: The colors are hardcoded, however you can use 
	//       RegisterClientScript blocks methods to use Grid's
	//       ItemTemplates and SelectTemplates colors.
	//		 for ex: grdEmployees.ItemStyle.BackColor OR
	//				 grdEmployees.SelectedItemStyle.BackColor
	//-------------------------------------------------------------
	function HighlightRow(chkB)	{
	
	// Old code for VS 2002 / .NET framework 1.0 code
	//-----------------------------------------------
	// In .NET 1.0 ASP.NET was using SPAN tag with
	// CheckBox control. 
	//var oItem = chkB.children;
	//xState=oItem.item(0).checked;
	var xState=chkB.checked;
		
	if(xState)
		{//chkB.parentElement.parentElement.style.backgroundColor='lightcoral';  // grdEmployees.SelectedItemStyle.BackColor
		 //chkB.parentElement.parentElement.style.color='white'; // grdEmployees.SelectedItemStyle.ForeColor
		}else 
		{//chkB.parentElement.parentElement.style.backgroundColor='white'; //grdEmployees.ItemStyle.BackColor
		 //chkB.parentElement.parentElement.style.color='black'; //grdEmployees.ItemStyle.ForeColor
		}
	}
