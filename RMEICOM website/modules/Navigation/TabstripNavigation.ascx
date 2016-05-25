<%@ Control Language="VB" AutoEventWireup="false" CodeFile="TabstripNavigation.ascx.vb" Inherits="modules_Navigation_TabstripNavigation" %>
<%@ Register Assembly="RadTabStrip.Net2, Culture=neutral, PublicKeyToken=a87324b017ca21ce"
    Namespace="Telerik.WebControls" TagPrefix="radTS" %>
    
<script language="javascript" type="text/javascript">
function ClientMouseOverHandler(sender, eventArgs)
{
    var tabStrip = sender;
    var tab = eventArgs.Tab;
    var browserEvents = eventArgs.EventObject;
    tab.Select();

    //alert("You have just moved over the " + tab.Text + " tabs in the " + tabStrip.ID + " tabstrip");
}

function ClientMouseOutHandler(sender, eventArgs)
{
    var tabStrip = sender;
    var tab = eventArgs.Tab;
    var browserEvents = eventArgs.EventObject;
    tab.UnSelect();
    

    //alert("You have just moved over the " + tab.Text + " tabs in the " + tabStrip.ID + " tabstrip");
}

function ClientTabUnSelectedHandler(sender, eventArgs){
    var tabStrip = sender;
    var tab = eventArgs.Tab;
    alert(tabStrip.SelectedIndex);
    //alert("You have unselected the " + tab.Text + " tab in the " + tabStrip.ID + " tabstrip.");
}

function ClientTabSelectingHandler(sender, eventArgs) {
    var tabStrip = sender;
    var tab = eventArgs.Tab;
    alert(tabStrip.SelectedIndex);
}
</script>
<radTS:RadTabStrip ID="Menu" runat="server" Skin="Default" OnClientMouseOver="ClientMouseOverHandler"  >
    
</radTS:RadTabStrip>
