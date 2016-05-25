<%@ Control Language="c#" AutoEventWireup="false" Codebehind="MozillaPasteHelperDlg.ascx.cs" Inherits="Telerik.WebControls.EditorDialogControls.MozillaPasteHelperDlg" TargetSchema="http://schemas.microsoft.com/intellisense/ie5" %>
<%@ OutputCache Duration="600" VaryByParam="Language;SkinPath" %>
<table id="MainTable" class="MainTable" cellpadding="0" cellspacing="0">
	<tr>
		<td class="MainTableContentCell">
			<table>
				<tr>
					<td>
						<div class="PreviewAreaHolder">
							<iframe id="contentHolderIframe" class="Text" src="javascript:''" style="width:100%;height:100%"></iframe>
							<textarea id="contentHolderTextarea" class="Text" style="display:none;width:100%;height:100%"></textarea>
						</div>
					</td>
					<td valign="top">
						<button class="Button" onclick="return OkClicked();"
							type=button><script>localization.showText('OK');</script></button>
							<button class="Button" onclick="CloseDlg();"
							type=button><script>localization.showText('Cancel');</script></button>
					</td>
				</tr>
			</table>
		</td>
	</tr>
</table>
<script language="javascript">
	var contentHolderIframe = document.getElementById("contentHolderIframe");
	var contentHolderTextarea = document.getElementById("contentHolderTextarea");
	
	function OkClicked()
	{
		var content = contentHolderIframe.style.display != "none" ? 
						contentHolderIframe.contentWindow.document.body.innerHTML :
						contentHolderTextarea.value;						
		CloseDlg(content);
	}

	function InitDialog()
	{
		var args = GetDialogArguments();
		if (true == args.GetPlainText)
		{
			contentHolderIframe.style.display  = "none";
			contentHolderTextarea.style.display  = "block";
			return;
		}
		
	
		var theDocument = contentHolderIframe.contentWindow.document;
		theDocument.open();
		theDocument.write("<head><style></style></head><body></body>");
		theDocument.close();
		contentHolderIframe.contentWindow.document["designMode"] = "on";
	}

	AttachEvent(window, "load", InitDialog);
</script>