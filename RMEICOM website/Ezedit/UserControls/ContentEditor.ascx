<%@ Control Language="VB" AutoEventWireup="false" CodeFile="ContentEditor.ascx.vb"
    Inherits="Ezedit_UserControls_ContentEditor" %>
<RadControls:RadEditor ID="RadEditor1" runat="server" ConvertTagsToLower="True" ConvertToXhtml="True"
    CopyCssToFormatBlockTool="False" DocumentsFilters="*.*" EnableClientSerialize="True"
    EnableContextMenus="True" EnableDocking="True" EnableEnhancedEdit="True" EnableHtmlIndentation="True"
    EnableServerSideRendering="True" EnableTab="True" ImagesFilters="*.gif,*.xbm,*.xpm,*.png,*.ief,*.jpg,*.jpe,*.jpeg,*.tiff,*.tif,*.rgb,*.g3f,*.xwd,*.pict,*.ppm,*.pgm,*.pbm,*.pnm,*.bmp,*.ras,*.pcd,*.cgm,*.mil,*.cal,*.fif,*.dsf,*.cmx,*.wi,*.dwg,*.dxf,*.svf"
    MediaFilters="*.asf,*.asx,*.wm,*.wmx,*.wmp,*.wma,*.wax,*.wmv,*.wvx,*.avi,*.wav,*.mpeg,*.mpg,*.mpe,*.mov,*.m1v,*.mp2,*.mpv2,*.mp2v,*.mpa,*.mp3,*.m3u,*.mid,*.midi,*.rm,*.rma,*.rmi,*.rmv,*.aif,*.aifc,*.aiff,*.au,*.snd"
    PassSessionData="True" ShowSubmitCancelButtons="false" RenderAsTextArea="False"
    Scheme="Default" Skin="Default" TemplateFilters="*.html,*.htm" ShowPreviewMode="false"
    MaxFlashSize="1024000" MaxMediaSize="20480000" ToolbarMode="Default" ToolsWidth=""
    UseFixedToolbar="False" ConvertFontToSpan="True" NewLineBr="False" AllowThumbGeneration="true"
    ThumbSuffix="_sm" SaveAsXhtml="true" AllowScripts="true">
</RadControls:RadEditor>

<script type="text/javascript" language="javascript">
RadEditorCommandList["InternalLink"] = function anon(commandName, editor, oTool)
{
   var theSelectionObject = editor.GetSelection();
   var theSelectedText = theSelectionObject.GetText();
   var theSelectedHtml = theSelectionObject.GetHtmlText();

    var arg = new Object();        
    arg.SelectedText = theSelectedText;
    arg.SelectedHtml = theSelectedHtml;
    
    editor.ShowDialog("/RadControls/Editor/Custom/InternalLink.aspx", arg, 600, 400, insertInternalLink, null, "Insert Internal Link");
}

RadEditorCommandList["ProtectedLink"] = function anon(commandName, editor, oTool)
{
   var theSelectionObject = editor.GetSelection();
   var theSelectedText = theSelectionObject.GetText();
   var theSelectedHtml = theSelectionObject.GetHtmlText();

    var arg = new Object();        
    arg.SelectedText = theSelectedText;
    arg.SelectedHtml = theSelectedHtml;
    
    editor.ShowDialog("/RadControls/Editor/Custom/ProtectedLink.aspx", arg, 400, 200, insertProtectedLink, null, "Insert Protected Link");
}

RadEditorCommandList["ImageEnlarger"] = function anon(commandName, editor, oTool)
{
   var theSelectionObject = editor.GetSelection();
   var theSelectedText = theSelectionObject.GetText();
   var theSelectedHtml = theSelectionObject.GetHtmlText();

    var arg = new Object();        
    arg.SelectedText = theSelectedText;
    arg.SelectedHtml = theSelectedHtml;
    
    editor.ShowDialog("/RadControls/Editor/Custom/EnlargeImage.aspx", arg, 600, 400, insertInternalLink, null, "Insert Internal Link");
}

RadEditorCommandList["SaveContent"] = function(commandName, editor, oTool)
    {
        editor.Submit();
    }


function insertInternalLink(retValue) {
    if (retValue.html) {
        var editor = GetRadEditor ("<%=RadEditor1.ClientID%>");
        editor.PasteHtml(retValue.html);
    }
}

function insertProtectedLink(retValue) {
    if (retValue.html) {
        var editor = GetRadEditor ("<%=RadEditor1.ClientID%>");
        editor.PasteHtml(retValue.html);
    }
}

</script>

