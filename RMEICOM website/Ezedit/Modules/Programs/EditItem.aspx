<%@ Page Language="VB" MasterPageFile="~/Ezedit/MasterPage.master" AutoEventWireup="false"
    CodeFile="EditItem.aspx.vb" Inherits="Ezedit_Modules_Programs_EditItem" Title="Untitled Page" ValidateRequest="false" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Main" runat="Server">
    <p>
        <a href="Default.aspx?ModuleKey=Programs" class="pageTitle">Programs</a>
        <asp:Label ID="lblPageTitle" CssClass="pagetitle" runat="server" Text="" />
    </p>
    <asp:Label ID="lblAlert" runat="server" CssClass="alert" />
    <VAM:ValidationSummary ID="ValidationSummary1" runat="server" CssClass="message"
        HeaderText="The following field are required:" />
    <hr />
    <table>
        <tr>
            <td>
                <asp:Label runat="server" CssClass="form-label" Text="Program Type:" />
                <VAM:RequiredListValidator runat="server" ControlIDToEvaluate="ddlProgramTypeID"
                    SummaryErrorMessage="Program Type" UnassignedIndex="0" />
            </td>
            <td>
                <asp:DropDownList ID="ddlProgramTypeID" runat="server" AppendDataBoundItems="true"
                    DataSourceID="dataProgramTypes" DataTextField="ResourceName" DataValueField="ProgramTypeID">
                    <asp:ListItem Text="<- Please Choose ->" Value="" />
                </asp:DropDownList>
                <asp:SqlDataSource ID="dataProgramTypes" runat="server" ConnectionString="<%$ ConnectionStrings:WebsiteDB %>"
                    SelectCommand="SELECT * FROM qryCustom_ProgramTypes ORDER BY SortOrder" />
            </td>
        </tr>
        <tr>
            <td>
                <asp:Label ID="Label1" runat="server" CssClass="form-label" Text="Disease:" />
                <VAM:RequiredListValidator runat="server" ControlIDToEvaluate="ddlDiseaseID" SummaryErrorMessage="Disease"
                    UnassignedIndex="0" />
            </td>
            <td>
                <asp:DropDownList ID="ddlDiseaseID" runat="server" AppendDataBoundItems="true" DataSourceID="dataDiseases"
                    DataTextField="ResourceName" DataValueField="DiseaseID">
                    <asp:ListItem Text="<- Please Choose ->" Value="" />
                </asp:DropDownList>
                <asp:SqlDataSource ID="dataDiseases" runat="server" ConnectionString="<%$ ConnectionStrings:WebsiteDB %>"
                    SelectCommand="SELECT * FROM qryCustom_Diseases ORDER BY SortOrder" />
            </td>
        </tr>
        <tr>
            <td>
                <asp:Label runat="server" CssClass="form-label" Text="Program Name:" />
                <VAM:RequiredTextValidator runat="server" ControlIDToEvaluate="txtResourceName" SummaryErrorMessage="Program Name" />
            </td>
            <td>
                <asp:TextBox ID="txtResourceName" runat="server" CssClass="form-textbox" Width="400" />
            </td>
        </tr>
        <tr>
            <td>
                <asp:Label runat="server" CssClass="form-label" Text="Date:" />
            </td>
            <td>
                <Date:DateTextBox ID="txtProgramDate" runat="server" CssClass="form" xPopupCalendar-xToggleText="Click to view the calendar"
                    xPopupCalendar-xToggleImageUrl="/App_Themes/EzEdit/Images/calendar.png" />
            </td>
        </tr>
        <tr>
            <td valign="top">
                <asp:Label ID="Label2" runat="server" CssClass="form-label" Text="Description:" />
            </td>
            <td>
                <EzEdit:ContentEditor EditorWidth="600" EditorHeight="600" ID="txtDescription" runat="server" />
            </td>
        </tr>
        
        <tr>
            <td>
                <asp:Label ID="Label3" runat="server" CssClass="form-label" Text="Program URL:" />
            </td>
            <td>
                <asp:TextBox ID="txtProgramUrl" runat="server" CssClass="form-textbox" Width="400" />
            </td>
        </tr>
        
        
        <tr>
            <td>
            </td>
            <td>
                <VAM:Button ID="btnSave" runat="server" CssClass="form-button" Text="Save" />
                <asp:Button ID="btnCancel" runat="server" CssClass="form-button" Text="Cancel" />
                <asp:HiddenField ID="hdnItemID" runat="server" />
            </td>
        </tr>
    </table>
</asp:Content>
