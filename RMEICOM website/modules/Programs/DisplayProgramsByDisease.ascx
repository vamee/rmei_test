<%@ Control Language="VB" AutoEventWireup="false" CodeFile="DisplayProgramsByDisease.ascx.vb"
    Inherits="modules_Programs_DisplayProgramsByDisease" %>
<asp:Repeater ID="rptDiseases" runat="server">
    <ItemTemplate>
        <h2 style="padding: 15px 0px 10px;"><asp:Label
            ID="lblResourceName" runat="server" Text='<%#Eval("ResourceName")%>' /></h2>
        <asp:Repeater ID="rptPrograms" runat="server" OnItemDataBound="rptPrograms_ItemDataBound">
            <ItemTemplate>
                <h3><asp:Label ID="lblProgramUrl" runat="server" /></h3>
                <asp:Literal ID="ltrDescription" runat="server" Text='<%#Eval("Description")%>' />
                <hr />
            </ItemTemplate>
            <SeparatorTemplate>
            </SeparatorTemplate>
        </asp:Repeater>
    </ItemTemplate>
</asp:Repeater>
