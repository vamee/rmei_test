<%@ Control Language="VB" AutoEventWireup="false" CodeFile="DisplayPrograms.ascx.vb"
    Inherits="modules_Programs_DisplayPrograms" %>
<asp:Repeater ID="rptPrograms" runat="server">
    <ItemTemplate>
        <h3><asp:HyperLink ID="hypProgramUrl" runat="server" Text='<%#Eval("ResourceName")%>'
            NavigateUrl='<%#Eval("ProgramUrl")%>' Target="_blank" /></h3>
        <asp:Literal ID="ltrDescription" runat="server" Text='<%#Eval("Description")%>' />
    </ItemTemplate>
    <SeparatorTemplate>
        <hr />
    </SeparatorTemplate>
</asp:Repeater>
