<%@ Control Language="c#" AutoEventWireUp="false" TargetSchema="http://schemas.microsoft.com/intellisense/ie5" CodeBehind="PageProperties.ascx.cs" Inherits="Telerik.WebControls.EditorDialogControls.PageProperties" %>
<%@ Register TagPrefix="telerik" TagName="ColorPicker" Src="../Controls/ColorPicker.ascx"%>
<%@ Register TagPrefix="telerik" TagName="CssClassSelector" Src="../Controls/CssClassSelector.ascx"%>
<%@ Register TagPrefix="telerik" TagName="ImageDialogCaller" Src="../Controls/ImageDialogCaller.ascx"%>
<table border=0>
	<tr>
		<td>
			<fieldset>
				<legend>				
					<script>localization.showText('HTMLAttributes');</script>
				</legend>
				<table border=0>
					<tr><td class="label" valign="top">
							<script>localization.showText('PageTitle');</script>
						</td>
						<td><input type=text class="RadETextBox" style="width:250px" id="TitleBox"></td>
					</tr>
					<tr>
						<td class="label" valign="top">
							<script>localization.showText('BaseLocation');</script>
						</td>
						<td><input type=text class="RadETextBox" style="width:250px" id="BaseLocationBox"></td>
					</tr>
					<tr>
						<td class="label" valign="top">
							<script>localization.showText('Description');</script>
						</td>
						<td>
							<textarea  class="TextArea" id="Description" style="WIDTH:100%" rows="3"></textarea>
						</td>
					</tr>
					<tr><td class="label" valign="top"><script>localization.showText('Keywords');</script></td>
						<td>
							<textarea  class="TextArea" id="Keywords" style="WIDTH:100%" rows="3"></textarea>
						</td>
					</tr>
				</table>
			</fieldset>
		</td > 
		<td valign=top rowspan=2>
			<button class="button" onclick = "CloseDialog(true)"><script>localization.showText('OK');</script></button><br>
			<button class="button" onclick = "CloseDialog(false)"><script>localization.showText('Cancel');</script></button>
		</td>
	</tr>
	<tr>
		<td >
			<fieldset>
			<legend>
				<script>localization.showText('BodyAttributes');</script>
			</legend>
				<table border="0">
					<tr>
						<td class="label" nowrap>
							<script>localization.showText('BackColor');</script>
						</td>
						<td class="label">
							<telerik:colorpicker id="BgColorPicker" runat="server"></telerik:colorpicker>
						</td>
						<td class="label"><script>localization.showText('ClassName');</script></td>
						<td class="label">
							<telerik:cssclassselector id="theCssClassSelector"
								cssfilter="ALL, BODY"
								width="150px"
								popupwidth="200px"
								popupheight="160px"
								runat="server">
							</telerik:cssclassselector>
						</td>
					</tr>
					<tr>
						<td class="label" nowrap>
							<script>localization.showText('TopMargin');</script>
						</td>
						<td class="label" ><input type=text size="2"id="topMargin"></td>
						<td class="label" nowrap>
							<script>localization.showText('BottomMargin');</script>		
						</td>
						<td class="label" > <input type=text size="2" id="bottomMargin"></td>
					</tr>
					<tr>
						<td class="label" nowrap>
							<script>localization.showText('LeftMargin');</script>
						</td>
						<td class="label" ><input type=text size="2" id="leftMargin"></td>
						<td class="label" nowrap>
							<script>localization.showText('RightMargin');</script>						
						</td>
						<td class="label" ><input type=text size="2"  id="rightMargin"></td>
					</tr>	
					<tr>
						<td class="label"><script>localization.showText('BGImage');</script></td>
						<td class="label" colspan="3">
							<telerik:imagedialogcaller id="BgImageDialogCaller" runat="server" />
						</td>
					</tr>					
				</table>
			</fieldset> 
			
		</td>
	<td></td></tr>	
</table>

<script>
	var theEditor = null;
	var editorDocument = null;
	var editorHead = null;
	var editorBase = null;
		
	//Body attributes
	var oBodyAttribs = 
	{	
		bottomMargin:null,
		leftMargin:null,
		rightMargin:null,
		topMargin:null
	};
			
	var oMetaNames = 
	{
		Description: null,
		Keywords: null
	};
	
			
	window.onload = function(e)
	{
		if (!e) 
		{
			e = window.event;
		}
				
		var dialogArgs = GetDialogArguments();
		if (dialogArgs)
		{			
			theEditor = dialogArgs.EditorObj;
			editorDocument = theEditor.Document;			
			editorHead = editorDocument.getElementsByTagName("HEAD")[0];			
			LoadData();
		}
	};
	
	function GetMetaTag(name)
	{
		var metas = editorDocument.getElementsByTagName("META");			
		name = name.toLowerCase();
		var theMeta = null;
		
		for (var i = 0; i < metas.length; i++)
		{
			var meta = metas[i];					
			var metaName = ("" + meta.getAttribute("name")).toLowerCase();
			if (name == metaName)
			{			
				theMeta = meta;
			}
		}		
		return theMeta;	
	}
	

	function LoadData()
	{	
		if (editorDocument)
		{					
			//alert ("HTML " + editorDocument.getElementsByTagName("HTML")[0].innerHTML);			
			var args = GetDialogArguments();
			
			//TITLE
			document.getElementById("TitleBox").value = editorDocument.title;
			
			var txtDescr = document.getElementById("Description");
			txtDescr.value = "";
			var txtKeywords = document.getElementById("Keywords");
			txtKeywords.value = "";
			
			//&lt;meta name="keywords" content="keywords,keyword,keyword phrase,etc."&gt;	
			var meta = GetMetaTag ("Description");
			if (meta)
			{
				oMetaNames["Description"] = meta.getAttribute("content");
				txtDescr.value = oMetaNames["Description"];
			}
						
			meta = GetMetaTag ("Keywords");
			if (meta)
			{
				oMetaNames["Keywords"] = meta.getAttribute("content");
				txtKeywords.value = oMetaNames["Keywords"];
			}			
			
			// CSS classes
			var availableCssClasses = args.CssClasses;
			var cssClassSelector = <%=theCssClassSelector.ClientID%>;
			cssClassSelector.Initialize(availableCssClasses);
			
			cssClassSelector.SelectCssClass(editorDocument.body.className);
			
			// BG image dialog caller
			var imageDialogCaller = <%=BgImageDialogCaller.ClientID%>
			imageDialogCaller.Initialize(args.EditorObj);
			
			var imagePath = editorDocument.body.getAttribute("background");
			if (!imagePath)
				imagePath = "";
			imageDialogCaller.SetImagePath(imagePath);
			
			//BASES
			var bases = editorDocument.getElementsByTagName("BASE");						
			if (bases.length > 0)
			{
				editorBase = bases[0];
				document.getElementById("BaseLocationBox").value = editorBase.getAttribute("href");
			}
						
			//Body attributes
			for (var item in oBodyAttribs)
			{			
				
				var oBox = document.getElementById(item);
				
				var oVal = GetAttribute(editorDocument.body, item);
				if (oVal)
				{
					oBox.value = oVal;
				}
				else oBox.value = "";				
			}	
			
			//BgColor
			var colorSelector = <%=BgColorPicker.ClientID%>;
			colorSelector.SelectColor(editorDocument.body.bgColor);
		}
	}
	
	function CloseDialog(update)
	{
		if (update)
		{
			UpdateEditorDocument();
		}		
		CloseDlg();
	}
	

	function UpdateEditorDocument()
	{
		// Title
		editorDocument.title = document.getElementById("TitleBox").value;
				
		// Description, Keywords			
		for (var item in oMetaNames)
		{			
			var editorMetaTag = null;
			
			var strDescr = document.getElementById(item).value;											
			if (!oMetaNames[item] && strDescr)
			{			
				if (document.all)
				{
					editorMetaTag = editorHead.appendChild(editorDocument.createElement("<META NAME='" + item + "'></META>"));
				}
				else
				{
					editorMetaTag = editorHead.appendChild(editorDocument.createElement("META"));
					editorMetaTag.setAttribute("name", item);
				}
			}
			else editorMetaTag = GetMetaTag (item);
			
			if (editorMetaTag)
			{			   
				editorMetaTag.setAttribute("content", strDescr);
			}
		}
		
		// CSS
		var cssClassSelector = <%=theCssClassSelector.ClientID%>;
		editorDocument.body.className = cssClassSelector.GetSelectedClassName();
		
		// Base
		var strBase = document.getElementById("BaseLocationBox").value;
		if (!editorBase && strBase)
		{
			editorBase = editorHead.appendChild(editorDocument.createElement("BASE"));
		}
			
		SetAttribute(editorBase, "href", strBase);
		
		// COLOR
		var colorSelector = <%=BgColorPicker.ClientID%>;
		if (colorSelector.SelectedColor) {
			editorDocument.body.bgColor = colorSelector.SelectedColor
		} else {
			editorDocument.body.bgColor = '';
		}
		
		// set the bg image
		var imageDialogCaller = <%=BgImageDialogCaller.ClientID%>
		SetAttribute(editorDocument.body,"background", imageDialogCaller.GetImagePath());
		
		for (var item in oBodyAttribs)
		{			
			SetAttribute(editorDocument.body,
						 item,
						 document.getElementById(item).value);
		}			

		if (theEditor)
		{
			theEditor.FullPage = true;
		}
	}
				
	function GetAttribute(element, attributeName)
	{		
		if (!element || !attributeName) return;		
		return element.getAttribute(attributeName);		
	}
						
	function SetAttribute(element, attributeName, attributeValue)
	{		
		if (!element || !attributeName) return;			
		if (attributeValue)
		{			
			element.setAttribute(attributeName, attributeValue);
		}
		else
		{		
			element.removeAttribute(attributeName);
		}
	}
</script>