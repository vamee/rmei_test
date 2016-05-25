// Copyright 2002 - 2007 Peter L. Blum, All Rights Reserved, www.PeterBlum.com
// Peter's Date Package Release 2.0.3
function TP_Initialize(pTPId,pAO,pICB){TP_ScrollIntoView(pTPId);var vTPFld=PDP_GetById(pTPId);var vMO=pAO.MOFC!=null;for(var vI=0;vI<pAO.Times.length;vI++){var vC=PDP_GetById(pTPId+"_T_"+vI.toString());vC.onclick=new Function("TP_TimeClick('"+pTPId+"', this.id, "+vI+");");if(vMO){vC.onmouseover=new Function("TP_OnMouseOver('"+vC.id+"','"+pAO.MOFC+"','"+pAO.MOBrdC+"');");vC.onmouseout=new Function("TP_OnMouseOut('"+vC.id+"');");}}if(!gPDP_IEMac&&(vTPFld.offsetWidth!=0))TP_InitSize(pTPId);}function TP_InitSize(pTPId){var vTPFld=PDP_GetById(pTPId);var vAO=vTPFld.AO;if(vAO.Inited)return;var vOT=PDP_GetById(pTPId+"_OuterTable");var vComW=vOT.offsetWidth;var vClW=0;if(vTPFld.clientWidth)vClW=vTPFld.clientWidth;else vClW=vTPFld.width;if(vClW>vComW)vComW=vClW;vOT.style.width=vComW+"px";var vH=PDP_GetById(pTPId+"_Header");if(vH)vH.style.width=vComW+"px";if(gPDP_Opera7)vTPFld.style.width=(vComW+PDP_GetLeftBorder(vTPFld)*2)+"px";else vTPFld.style.width=vComW+"px";vAO.Inited=true;}function TP_TimeClick(pTPId,pCellId,pIdx){var vTPFld=PDP_GetById(pTPId);var vAO=vTPFld.AO;if(vAO==null)return false;if(TP_ChangeSelection(vTPFld,pCellId,pIdx))TP_OnSelectionChanged(vTPFld.AO);if(vAO.IsPopup)PDP_DelayedClosePopup();}function TP_ChangeSelection(pTPFld,pCellId,pIdx){var vAO=pTPFld.AO;if(vAO.SelCID==pCellId)return false;if(vAO.SelCID!=""){var vC=PDP_GetById(vAO.SelCID);vC.className=vAO.TCSS;}if(pCellId!=""){var vC2=PDP_GetById(pCellId);vC2.className=vAO.SelTCSS;}vAO.SelCID=pCellId;var vF=PDP_GetById(pTPFld.id+"_Time");if(pIdx>-1)vF.value=vAO.Times[pIdx].toString();else vF.value="";return true;}function TP_OnSelectionChanged(pAO){var vOSCFnc=pAO.OnSelChg;if(vOSCFnc)eval(vOSCFnc);}function TP_OnMouseOver(pMCId,pFC,pBrdC){PDP_OnMouseEvent(PDP_GetById(pMCId),pFC,pBrdC);}function TP_OnMouseOut(pMCId){PDP_OnMouseEvent(PDP_GetById(pMCId),"","");}function TP_GetValue(pTPId){var vH=PDP_GetById(pTPId+"_Time");if(vH.value=="")return null;else return parseInt(vH.value);}function TP_SetValue(pTPId,pSeconds){var vTPFld=PDP_GetById(pTPId);var vAO=vTPFld.AO;var vIdx=-1;if(pSeconds!=null)for(var vI=0;vI<vAO.Times.length;vI++)if(vAO.Times[vI]==pSeconds){vIdx=vI;break;}var vCellId="";if(vIdx>-1)vCellId=pTPId+"_T_"+vIdx.toString();TP_ChangeSelection(vTPFld,vCellId,vIdx);TP_InitSize(pTPId);}function TP_IsTimeInPicker(pTPId,pSeconds){var vTPFld=PDP_GetById(pTPId);var vAO=vTPFld.AO;for(var vI=0;vI<vAO.Times.length;vI++)if(vAO.Times[vI]==pSeconds)return true;return false;}function TP_ScrollIntoView(pTPId){var vTPFld=PDP_GetById(pTPId);var vAO=vTPFld.AO;if(vAO.SelCID!=""){var vAS=PDP_GetById(vAO.SelCID);if(vAS&&vAS.scrollIntoView)vAS.scrollIntoView(false);}}function TMTB_OnPopup(pTBId,pBId){var vR=false;if(!PDP_CmdCanEdit(pTBId))return;var vTBFld=PDP_GetById(pTBId);var vAO=vTBFld.AO;vAO.APUOn=0;var vTPId=vAO.TPId;var vTP=PDP_GetById(vTPId);if(vTP.style&&(vTP.style.visibility=='hidden')){vTP.AO.TBId=pTBId;vR=TMTB_TransferToPicker(pTBId,vTPId);if(vR)PDP_TogglePopup(pBId+"_TG",vTPId);}return vR;}function TMTB_TransferToPicker(pTBId,pTPId){var vR=true;var vTime=TMTB_GetTimeValue(pTBId);if(vTime==null)TP_SetValue(pTPId,null);else{TP_SetValue(pTPId,vTime);}return vR;}function TMTB_TransferTimePickerToTextBox(pTBId,pTPId){var vTime=TP_GetValue(pTPId);if(!pTBId)pTBId=PDP_GetById(pTPId).AO.TBId;TMTB_SetTimeValue(pTBId,vTime,11);var vTBFld=PDP_GetById(pTBId);if(vTBFld.focus&&vTBFld.select){vTBFld.focus();vTBFld.select();}}function TMTB_AutoClose(pTBId){var vTBFld=PDP_GetById(pTBId);var vAO=vTBFld.AO;if(vAO.APUOn){vAO.APUOn=0;var vTP=PDP_GetById(vAO.TPId);if(vTP.style&&(vTP.style.visibility!='hidden'))PDP_ClosePopup();}}