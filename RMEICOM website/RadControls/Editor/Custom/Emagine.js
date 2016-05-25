// JScript File
RadEditorCommandList["InternalLink"] = function anon(commandName, editor, oTool)
{
   var theSelectionObject = editor.GetSelection();
   var theSelectedText = theSelectionObject.GetText();
   var theSelectedHtml = theSelectionObject.GetHtmlText();

    var arg = new Object();        
    //Using an Object as a argument is convenient as it allows setting many properties.
    arg.SelectedText = theSelectedText;
    arg.SelectedHtml = theSelectedHtml;
    
    editor.ShowDialog("/RadControls/Editor/Custom/InternalLink.aspx", arg, 600, 400, insertInternalLink, null, "Insert Internal Link");
}

function insertInternalLink(retValue) {
    if (retValue.html) {
        var editor = GetRadEditor ("<%=RadEditor1.ClientID%>");
        editor.PasteHtml(retValue.html);
    }
}
