<%@ Page Language="VB" MasterPageFile="~/Ezedit/MasterPage.master" AutoEventWireup="false"
    CodeFile="EditField.aspx.vb" Inherits="Ezedit_Modules_Forms01_EditForm" ValidateRequest="false" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Main" runat="Server">
    <p>
        <asp:Label ID="lblPageTitle" runat="server" CssClass="pageTitle" Text="" />
    </p>
    <p>
        <asp:Label ID="lblAlert" runat="server" CssClass="message"></asp:Label>
    </p>
    <p>
        <asp:ValidationSummary ID="ValidationSummary1" runat="server" CssClass="message"
            HeaderText="Please correct the following errors: "></asp:ValidationSummary>
    </p>
    <asp:HiddenField ID="hidAction" runat="server" />
    <table width="100%" cellpadding="0" cellspacing="0" border="0" bordercolor="#999999">
        <tr>
            <td>
                <table border="0" width="100%" cellpadding="5" cellspacing="0">
                    <tr>
                        <td colspan="2">
                            <hr />
                        </td>
                    </tr>
                    <tr>
                        <td valign="top" nowrap>
                            <asp:Label ID="lblFieldTypeId" Text="Field Type:" CssClass="form_label" runat="server"></asp:Label>
                        </td>
                        <td width="100%">
                            <asp:DropDownList ID="ddlFieldTypeId" runat="server" AutoPostBack="true" AppendDataBoundItems="true">
                            </asp:DropDownList>
                            <asp:RequiredFieldValidator ID="FieldTypeValidator" runat="server" ErrorMessage="Please choose a field type."
                                ControlToValidate="ddlFieldTypeId" InitialValue=""></asp:RequiredFieldValidator>
                        </td>
                    </tr>
                    <tr id="trFieldName" runat="server" visible="false">
                        <td valign="top" nowrap>
                            <asp:Label ID="lblFieldName" Text="Field Name:" CssClass="form_label" runat="server"></asp:Label>
                        </td>
                        <td width="100%">
                            <asp:TextBox ID="txtFieldName" runat="server" Width="250" MaxLength="100"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="rfvFieldName" runat="server" ErrorMessage="Field name is required."
                                ControlToValidate="txtFieldName" Display="Dynamic">*</asp:RequiredFieldValidator>
                        </td>
                    </tr>
                    <tr id="trSalesForceAPI" runat="server" visible="false">
                        <td valign="top" nowrap>
                            <asp:Label ID="Label2" Text="SalesForce Field:" CssClass="form_label" runat="server" />
                        </td>
                        <td width="100%">
                            <asp:DropDownList ID="ddlSalesForceFieldName" runat="server" AutoPostBack="true"
                                AppendDataBoundItems="true">
                                <asp:ListItem Text="None" Value="" />
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr id="trColCount" runat="server" visible="false">
                        <td valign="top" nowrap>
                            <asp:Label ID="lblColCount" Text="# Columns:" CssClass="form_label" runat="server"
                                EnableViewState="false"></asp:Label>
                        </td>
                        <td width="100%">
                            <asp:DropDownList ID="ddlColCount" runat="server" AppendDataBoundItems="true">
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr id="trListTypeID" runat="server" visible="false">
                        <td valign="top" nowrap>
                            <asp:Label ID="lblListTypeID" Text="List Type:" CssClass="form_label" runat="server"
                                EnableViewState="false"></asp:Label>
                        </td>
                        <td width="100%">
                            <asp:DropDownList ID="ddlListTypeID" runat="server" AppendDataBoundItems="true" AutoPostBack="true">
                            </asp:DropDownList>
                            <asp:RequiredFieldValidator ID="rfvListTypeID" runat="server" ErrorMessage="Please choose a list type."
                                ControlToValidate="ddlListTypeID" InitialValue="0" Display="Static">*</asp:RequiredFieldValidator>
                        </td>
                    </tr>
                    <tr id="trLookupName" runat="server" visible="false">
                        <td valign="top" nowrap>
                            <asp:Label ID="lblLookupName" Text="Lookup Name:" CssClass="form_label" runat="server"
                                EnableViewState="false"></asp:Label>
                        </td>
                        <td width="100%">
                            <asp:TextBox ID="txtLookupName" Width="250" Height="20" CssClass="form" runat="server"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="rfvLookupName" runat="server" ErrorMessage="Lookup name is required."
                                ControlToValidate="txtLookupName" Display="Dynamic">*</asp:RequiredFieldValidator>
                        </td>
                    </tr>
                    <tr id="trListOptions" runat="server" visible="false">
                        <td valign="top" nowrap>
                            <asp:Label ID="lblListOptions" Text="List Options:" CssClass="form_label" runat="server"
                                EnableViewState="false"></asp:Label>
                        </td>
                        <td width="100%" valign="top">
                            <asp:TextBox ID="txtListOptions" Width="350" Height="100" CssClass="form" runat="server"
                                TextMode="MultiLine"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="rfvListOptions" runat="server" ErrorMessage="" ControlToValidate="txtListOptions"
                                Display="Dynamic">*</asp:RequiredFieldValidator>
                        </td>
                    </tr>
                    <tr id="trDataTypeId" runat="server" visible="false">
                        <td valign="top" nowrap>
                            <asp:Label ID="lblDataTypeId" Text="Data Type:" CssClass="form_label" runat="server"
                                EnableViewState="false"></asp:Label>
                        </td>
                        <td width="100%">
                            <asp:DropDownList ID="ddlDataTypeId" runat="server" AppendDataBoundItems="true">
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr id="trContentBlock" runat="server" visible="false">
                        <td valign="top" nowrap>
                            <asp:Label ID="lblContentBlock" Text="Content:" CssClass="form_label" runat="server"></asp:Label>
                        </td>
                        <td width="100%">
                            <EzEdit:ContentEditor ID="txtContentEditor" EditorWidth="600" EditorHeight="500"
                                runat="server" />
                        </td>
                    </tr>
                    <tr id="trDefaultValue" runat="server" visible="false">
                        <td valign="top" nowrap>
                            <asp:Label ID="lblDefaultValue" Text="Default Value:" CssClass="form_label" runat="server"
                                EnableViewState="false"></asp:Label>
                        </td>
                        <td width="100%">
                            <asp:TextBox ID="txtDefaultValue" Width="250" Height="20" CssClass="form" runat="server"></asp:TextBox>
                        </td>
                    </tr>
                    <tr id="trLabel" runat="server" visible="false">
                        <td valign="top" nowrap>
                            <asp:Label ID="lblLabel" Text="Label:" CssClass="form_label" runat="server" EnableViewState="false"></asp:Label>
                        </td>
                        <td width="100%">
                            <asp:TextBox ID="txtLabel" Width="250" Height="20" CssClass="form" runat="server"></asp:TextBox>
                        </td>
                    </tr>
                    <tr id="trLabelAlign" runat="server" visible="false">
                        <td valign="top" nowrap>
                            <asp:Label ID="lblLabelAlign" Text="Label Align:" CssClass="form_label" runat="server"
                                EnableViewState="false"></asp:Label>
                        </td>
                        <td width="100%">
                            <asp:DropDownList ID="ddlLabelAlign" runat="server" AppendDataBoundItems="true">
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr id="trWidth" runat="server" visible="false">
                        <td valign="top" nowrap>
                            <asp:Label ID="lblWidth" Text="Field Width:" CssClass="form_label" runat="server"
                                EnableViewState="false"></asp:Label>
                        </td>
                        <td width="100%">
                            <asp:TextBox ID="txtWidth" Width="250" Height="20" CssClass="form" runat="server"></asp:TextBox>
                            <asp:CompareValidator ID="cpvWidth" runat="server" ErrorMessage="Width must be a number."
                                ControlToValidate="txtWidth" Type="Integer" Operator="DataTypeCheck">*</asp:CompareValidator>
                        </td>
                    </tr>
                    <tr id="trHeight" runat="server" visible="false">
                        <td valign="top" nowrap>
                            <asp:Label ID="lblHeight" Text="Field Height:" CssClass="form_label" runat="server"
                                EnableViewState="false"></asp:Label>
                        </td>
                        <td width="100%">
                            <asp:TextBox ID="txtHeight" Width="250" Height="20" CssClass="form" runat="server"></asp:TextBox>
                            <asp:CompareValidator ID="cpvHeight" runat="server" ErrorMessage="Height must be a number."
                                ControlToValidate="txtHeight" Type="Integer" Operator="DataTypeCheck">*</asp:CompareValidator>
                        </td>
                    </tr>
                    <tr id="trMinRequired" runat="server" visible="false">
                        <td valign="top" nowrap>
                            <asp:Label ID="lblMinRequired" Text="Validation - Min Length:" CssClass="form_label"
                                runat="server" EnableViewState="false"></asp:Label>
                        </td>
                        <td width="100%">
                            <asp:TextBox ID="txtMinRequired" Width="250" Height="20" CssClass="form" runat="server"></asp:TextBox>
                            <asp:CompareValidator ID="cpvMinRequired" runat="server" ErrorMessage="Min length required must be a number."
                                ControlToValidate="txtMinRequired" Type="Integer" Operator="DataTypeCheck">*</asp:CompareValidator>
                        </td>
                    </tr>
                    <tr id="trMaxRequired" runat="server" visible="false">
                        <td valign="top" nowrap>
                            <asp:Label ID="lblMaxRequired" Text="Validation - Max Length:" CssClass="form_label"
                                runat="server" EnableViewState="false"></asp:Label>
                        </td>
                        <td width="100%">
                            <asp:TextBox ID="txtMaxRequired" Width="250" Height="20" CssClass="form" runat="server"></asp:TextBox>
                            <asp:CompareValidator ID="cpvMaxRequired" runat="server" ErrorMessage="Max length required must be a number."
                                ControlToValidate="txtMaxRequired" Type="Integer" Operator="DataTypeCheck">*</asp:CompareValidator>
                        </td>
                    </tr>
                    <tr id="trRequired" runat="server" visible="false">
                        <td valign="top" nowrap>
                            <asp:Label ID="lblRequired" Text="Validation - Required:" CssClass="form_label" runat="server"
                                EnableViewState="false"></asp:Label>
                        </td>
                        <td width="100%">
                            <asp:RadioButtonList ID="rdoRequired" runat="Server" CssClass="form" RepeatDirection="Horizontal">
                                <asp:ListItem Text="Yes" Value="1"></asp:ListItem>
                                <asp:ListItem Text="No" Value="0" Selected="True"></asp:ListItem>
                            </asp:RadioButtonList>
                        </td>
                    </tr>
                    <tr id="trMinValue" runat="server" visible="false">
                        <td valign="top" nowrap>
                            <asp:Label ID="lblMinValue" Text="Validation - Min Value:" CssClass="form_label"
                                runat="server" EnableViewState="false"></asp:Label>
                        </td>
                        <td width="100%">
                            <asp:TextBox ID="txtMinValue" Width="250" Height="20" CssClass="form" runat="server"></asp:TextBox>
                        </td>
                    </tr>
                    <tr id="trMaxValue" runat="server" visible="false">
                        <td valign="top" nowrap>
                            <asp:Label ID="lblMaxValue" Text="Validation - Max Value:" CssClass="form_label"
                                runat="server" EnableViewState="false"></asp:Label>
                        </td>
                        <td width="100%">
                            <asp:TextBox ID="txtMaxValue" Width="250" Height="20" CssClass="form" runat="server"></asp:TextBox>
                        </td>
                    </tr>
                    <tr id="trAllowedChars" runat="server" visible="false">
                        <td valign="top" nowrap>
                            <asp:Label ID="lblAllowedChars" Text="Validation - Allowed Chars:" CssClass="form_label"
                                runat="server" EnableViewState="false"></asp:Label>
                        </td>
                        <td width="100%">
                            <asp:TextBox ID="txtAllowedChars" Width="250" Height="50" CssClass="form" runat="server"
                                TextMode="MultiLine"></asp:TextBox>
                        </td>
                    </tr>
                    <tr id="trDisallowedChars" runat="server" visible="false">
                        <td valign="top" nowrap>
                            <asp:Label ID="lblDisallowedChars" Text="Validation - Disallowed Chars:" CssClass="form_label"
                                runat="server" EnableViewState="false"></asp:Label>
                        </td>
                        <td width="100%">
                            <asp:TextBox ID="txtDisallowedChars" Width="250" Height="50" CssClass="form" runat="server"
                                TextMode="MultiLine"></asp:TextBox>
                        </td>
                    </tr>
                    <tr id="trValidationText" runat="server" visible="false">
                        <td valign="top" nowrap>
                            <asp:Label ID="lblValidationText" Text="Validation - Error Message:" CssClass="form_label"
                                runat="server" EnableViewState="false"></asp:Label>
                        </td>
                        <td width="100%">
                            <asp:TextBox ID="txtValidationText" Width="250" Height="50" CssClass="form" runat="server"
                                Wrap="true" TextMode="MultiLine"></asp:TextBox>
                        </td>
                    </tr>
                    <tr id="trLabelCSS" runat="server" visible="false">
                        <td valign="top" nowrap>
                            <asp:Label ID="lblLabelCSS" Text="Style - Label CSS Class:" CssClass="form_label"
                                runat="server" EnableViewState="false"></asp:Label>
                        </td>
                        <td width="100%">
                            <asp:DropDownList ID="ddlLabelCSS" runat="server">
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr id="trFieldCss" runat="server" visible="false">
                        <td valign="top" nowrap>
                            <asp:Label ID="Label1" Text="Style - Field CSS Class:" CssClass="form_label" runat="server"
                                EnableViewState="false"></asp:Label>
                        </td>
                        <td width="100%">
                            <asp:DropDownList ID="ddlFieldCss" runat="server">
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr id="trValidationCSS" runat="server" visible="false">
                        <td valign="top" nowrap>
                            <asp:Label ID="lblValidationCSS" Text="Style - Error Message CSS Class:" CssClass="form_label"
                                runat="server" EnableViewState="false"></asp:Label>
                        </td>
                        <td width="100%">
                            <asp:DropDownList ID="ddlValidationCSS" runat="server">
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr id="trHelpText" runat="server" visible="false">
                        <td valign="top" nowrap>
                            <asp:Label ID="lblHelpText" Text="Help Text:" CssClass="form_label" runat="server"
                                EnableViewState="false"></asp:Label>
                        </td>
                        <td width="100%">
                            <asp:TextBox ID="txtHelpText" Width="250" Height="50" CssClass="form" runat="server"
                                TextMode="MultiLine"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td></td>
                        <td>
                            <br />
                            <asp:Button ID="btnSave" runat="server" Text="Save" CssClass="form-button" />
                            <asp:Button ID="btnCancel" runat="server" Text="Cancel" CssClass="form-button" CausesValidation="false" />
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
</asp:Content>
