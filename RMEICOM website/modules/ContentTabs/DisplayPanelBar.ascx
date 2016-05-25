<%@ Control Language="VB" AutoEventWireup="false" CodeFile="DisplayPanelBar.ascx.vb"
    Inherits="modules_ContentTabs_DisplayPanelBar" %>
<%@ Register Assembly="RadPanelbar.Net2, Version=4.1.0.0, Culture=neutral, PublicKeyToken=e0d16f6f4c7e05de"
    Namespace="Telerik.WebControls" TagPrefix="radP" %>
<script language="javascript" type="text/javascript">
function PanelBar_OnMouseOver(sender, eventArgs) {
    eventArgs.Item.Expand();
    }
</script>
    
<radP:RadPanelbar ID="RadPanelbar1" runat="server" ExpandMode="FullExpandedItem" Skin="Default" OnClientMouseOver="PanelBar_OnMouseOver" CollapseAnimation-Duration="2" ExpandAnimation-Duration="2000" ExpandAnimation-Type="None">
</radP:RadPanelbar>
