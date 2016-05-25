<%@ Control Language="VB" AutoEventWireup="false" CodeFile="DisplaySlideshow.ascx.vb" Inherits="modules_ContentTabs_DisplaySlideshow" %>
<%@ Register Assembly="RadRotator.Net2, Culture=neutral, PublicKeyToken=de81a6506e5a433a"
    Namespace="Telerik.WebControls" TagPrefix="radR" %>
    
<radR:RadRotator ID="objSlideshow" runat="server">
<FrameTemplate>
 <%#Eval("Content") %>   
</FrameTemplate>
</radR:RadRotator>