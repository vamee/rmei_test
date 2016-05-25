<%@ Page Language="VB" MasterPageFile="~/Ezedit/MasterPage.master" AutoEventWireup="false"
    CodeFile="EditItem.aspx.vb" Inherits="Ezedit_Modules_DL01_EditItem" ValidateRequest="false" %>

<%@ Register Assembly="PeterBlum.VAM, Version=3.0.8.5000, Culture=neutral, PublicKeyToken=76d6c019e89ec3c9"
    Namespace="PeterBlum.VAM" TagPrefix="VAM" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Main" runat="Server">
    <p>
        <asp:Label ID="lblPageTitle" runat="server" CssClass="pageTitle" />
    </p>
    <p>
        <asp:Label ID="lblAlert" runat="server" CssClass="message"></asp:Label>
    </p>
    <p>
        <VAM:ValidationSummary ID="ValidationSummary1" runat="server" CssClass="message"
            HeaderText="Please correct the following errors: "></VAM:ValidationSummary>
    </p>
    <table border="0" cellpadding="5" cellspacing="0" id="table1">
        <tr>
            <td colspan="3">
                <hr />
            </td>
        </tr>
        <tr>
            <td class="form_label" nowrap>
                Category:<span class="message">*</span>
            </td>
            <td>
                <asp:DropDownList ID="ddlCategoryId" runat="server" TabIndex="1">
                </asp:DropDownList>
            </td>
            <td>
            </td>
        </tr>
        <tr>
            <td class="form_label" nowrap>
                Download Type:<span class="message">*</span>
            </td>
            <td>
                <asp:DropDownList ID="ddlModuleTypeID" runat="server" TabIndex="1" AutoPostBack="true"
                    AppendDataBoundItems="true">
                    <asp:ListItem Text="<- Please Choose ->" Value="0"></asp:ListItem>
                </asp:DropDownList>
                <VAM:RequiredListValidator ID="RequiredListValidator1" runat="server" ControlIDToEvaluate="ddlModuleTypeID"
                    SummaryErrorMessage="Choose a Download Type from the list." UnassignedIndex="0">
                </VAM:RequiredListValidator>
            </td>
            <td>
            </td>
        </tr>
        <tr id="trRegistrationRequired" runat="server" visible="false">
            <td class="form_label" nowrap>
                Requires Registration?:
            </td>
            <td>
                <asp:RadioButtonList ID="rblRegistrationRequired" runat="server" RepeatDirection="Horizontal">
                    <asp:ListItem Text="Yes" Value="1" />
                    <asp:ListItem Text="No" Value="0" />
                </asp:RadioButtonList>
            </td>
            <td>
            </td>
        </tr>
        <tr>
            <td class="form_label" nowrap>
                Date:
            </td>
            <td>
                <Date:DateTextBox ID="pdpDate" runat="server" CssClass="form" xMinDate="1/1/1940"
                    xMaxDate="1/1/2025" xPopupCalendar-xToggleText="Click to view the calendar" xPopupCalendar-xToggleImageUrl="../../../App_Themes/EzEdit/Images/calendar.png"></Date:DateTextBox>
                <%--<VAM:RequiredTextValidator ControlIDToEvaluate="pdpDate" ErrorMessage="Date is Required."
                    runat="server">
                    <ErrorFormatterContainer>
                        <VAM:TextErrorFormatter Display="None" />
                    </ErrorFormatterContainer>
                </VAM:RequiredTextValidator>
                <VAM:DataTypeCheckValidator ID="DataTypeCheckValidator1" DataType="date" ControlIDToEvaluate="pdpDate"
                    SummaryErrorMessage="Date is Invalid." runat="server">
                    <ErrorFormatterContainer>
                        <VAM:TextErrorFormatter Display="None" />
                    </ErrorFormatterContainer>
                </VAM:DataTypeCheckValidator>--%>
            </td>
            <td>
            </td>
        </tr>
        <tr>
            <td class="form_label" nowrap>
                Title:<span class="message">*</span>
            </td>
            <td>
                <VAM:TextBox ID="txtResourceName" Width="400" runat="server" CssClass="form-textbox"
                    MaxLength="255" />
                <VAM:RequiredTextValidator ControlIDToEvaluate="txtResourceName" ErrorMessage="Title is Required."
                    ID="RequiredFieldValidator2" runat="server" />
            </td>
            <td>
            </td>
        </tr>
        <tr runat="server" visible="true">
            <td class="form_label" nowrap>
                Sub Category:
            </td>
            <td>
                <asp:DropDownList ID="ddlResourceCategory" runat="server">
                </asp:DropDownList>
                <VAM:TextBox ID="txtResourceCategory" runat="server" CssClass="form-textbox" Visible="false" />
            </td>
            <td>
            </td>
        </tr>
        <tr>
            <td class="form_label" nowrap valign="top">
                Keywords:
            </td>
            <td>
                <asp:TextBox ID="txtKeywords" runat="server" Width="400" MaxLength="255" Rows="5"
                    TextMode="MultiLine" CssClass="form-textbox" />
            </td>
            <td>
            </td>
        </tr>
        <tr>
            <td class="form_label" nowrap valign="top">
                Description:
            </td>
            <td>
                <asp:TextBox ID="txtDescription" runat="server" Width="400" MaxLength="255" Rows="8"
                    TextMode="MultiLine" CssClass="form-textbox" />
            </td>
            <td>
            </td>
        </tr>
        <asp:Panel ID="pnlFile" runat="server" Visible="False">
            <tr>
                <td class="form_label" nowrap valign="top">
                    File:<span class="message">*</span>
                </td>
                <td>
                    <asp:FileUpload ID="txtFilename" Width="400" runat="server" CssClass="form-textbox" />
                    <VAM:RequiredTextValidator runat="server" ID="rtvFilename" ControlIDToEvaluate="txtFilename"
                        SummaryErrorMessage="Please choose a file to upload." ErrorMessage="*" />
                    <table id="tblFileName" runat="server" visible="false">
                        <tr>
                            <td class="form-label">
                                Current File:
                            </td>
                            <td>
                                <asp:HyperLink ID="hypFileName" runat="server" Text="" CssClass="form" Target="_blank" />
                            </td>
                        </tr>
                    </table>
                </td>
                <td>
                </td>
            </tr>
        </asp:Panel>
        <asp:Panel ID="pnlUrl" runat="server" Visible="false">
            <tr>
                <td class="form_label" nowrap valign="top">
                    Url:
                </td>
                <td>
                    <asp:TextBox ID="txtExternalUrl" runat="server" Width="400" MaxLength="255" CssClass="form-textbox" />
                </td>
                <td>
                </td>
            </tr>
        </asp:Panel>
        <tr>
            <td class="form_label" nowrap valign="top">
                Campaign Info:
            </td>
            <td valign="top">
                <asp:TextBox ID="txtCampaignInfo" runat="server" Width="400" MaxLength="255" CssClass="form-textbox" />
                <img src="/App_Themes/EzEdit/Images/help.png" alt="ex: campaign_id=1234567" />
            </td>
            <td>
                
            </td>
        </tr>
        <tr>
            <td>
                <asp:Label ID="Label1" runat="server" CssClass="form-label" Text="Start Date:" />
            </td>
            <td>
                <Date:DateTextBox ID="txtDisplayStartDate" runat="server" Width="100" CssClass="form-textbox" />
            </td>
        </tr>
        <tr>
            <td>
                <asp:Label ID="Label2" runat="server" CssClass="form-label" Text="End Date:" />
            </td>
            <td>
                <Date:DateTextBox ID="txtDisplayEndDate" runat="server" Width="100" CssClass="form-textbox" />
            </td>
        </tr>
        <tr>
            <td colspan="3">
                <hr />
            </td>
        </tr>
        <tr>
            <td colspan="3" align="center">
                <VAM:Button ID="btnSave" CssClass="button" runat="server" Text=" Save " />
                <VAM:Button ID="btnCancel" CssClass="button" runat="server" Text=" Cancel " CausesValidation="false" />
            </td>
        </tr>
    </table>
</asp:Content>
