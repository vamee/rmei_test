<?
include("config.php");

if(!isset($_GET['js']) || $_GET['js'] == ""){
	$link = $url.$_GET['id'];
}
else if($_GET['js'] == "on"){
	$link = $_GET['id'];
}

$link = str_replace(" ","+", $link); // get rid of spaces

if(!isset($_POST['action']) || $_POST['action'] == ""){
	if(!isset($link)){
		print "No Link Specified";
	}
	else{
		printpage("","","","");
	} 
} 
else if($_POST['action'] == "submit"){

	$send = true;

	function is_valid_email($sender_mail) { 
		if(ereg("([[:alnum:]\.\-]+)(\@[[:alnum:]\.\-]+\.+)", $sender_mail)) {
			return 0;
	    }
		else{
			return 1;
	    } 
	}
	
	$sender_mail = $_POST['sender_mail'];
	$friend_mail = $_POST['friend_mail'];
	$sender_name = $_POST['sender_name'];
	$optional_message = $_POST['optional_message'];
	$link = $_POST['link'];
	$js = $_POST['js'];
	$id = $_POST['id'];

	$s_emailvalid = is_valid_email($sender_mail);  // check email
	$f_emailvalid = is_valid_email($friend_mail);  // check email

	$emsg = "There were errors in your form:<br><br>\n";

	if ($sender_name == ""){
		$emsg .= "Please enter your name<br>\n";
		$send = false;
	}
	if ($sender_mail == "" || $s_emailvalid == 1){
		$emsg .= "Please enter your valid email address<br>\n";
		$send = false;
	}
	if ($friend_mail == "" || $f_emailvalid == 1){
		$emsg .= "Please enter your friends valid email address<br>\n";
		$send = false;
	}

	if($send){		
		$recipient = "$friend_mail";
		$message .= "$sender_name wanted to share the following link with you:\n$link\n\n";
		if($optional_message != ""){
			$message .= "Note:\n$optional_message\n\n";
		}
		$message .= "For more information on this or any other program sponsored by The Leukemia & Lymphoma Society, please visit http://www.LLS.org/lymphomaeducation \n\nRobert Michael Educational Institute - http://www.rmei.com";
		$headers = "From: $sender_name <$sender_mail>\r\nReply-To: $sender_mail\r\n";
		mail ($recipient, $send_subject, $message, $headers);

		if($auto_responder == "yes"){
			$recipient2 = "$sender_name <$sender_mail>";
			$recipient3 = "jungsen@rmcom.net";
			$subject2 = "email information from NHL send to a friend";
			$message2 = "Name:$sender_name\n\nSender Email:$sender_mail\n\nFriend Email:$friend_mail\n\n";
			$headers2 = "From: Send to a friend <jungsen@rmcom.net>\r\nReply-To: jungsen@rmcom.net\r\n";
			mail ($recipient3, $subject2, $message2, $headers2);
		}
		printthanks($sender_name,$friend_mail,$link);
		exit;
	}
	else if(!$send){
			printpage($sender_name,$sender_mail,$friend_mail,$emsg);
	}
}
else{
	printpage("","","","");
}

function printpage($sender_name,$sender_mail,$friend_mail,$errors){
include("config.php");

global $link, $id, $js;
		?>

<html>
<head>
<META NAME="Title" CONTENT="<? echo $title; ?>">
<META NAME="Author" CONTENT="RMEI">
<title>Send <? echo $link; ?> to a friend!</title>
<link rel="stylesheet" href="stylesheet.php" type="text/css">
</head>
<body>

<form method="post" action="friend.php">
  <input type="hidden" name="link" value="<? echo $link; ?>">
   <input type="hidden" name="id" value="<? echo $id; ?>">
    <input type="hidden" name="js" value="<? echo $js; ?>">
  <input type="hidden" name="action" value="submit">
  <font color="red"><b><? echo $errors; ?></b></font><p>
  <table width="600" border="0" cellspacing="0" cellpadding="5">
    <tr>
      <td colspan="2" width="625"><font size="3"><b><? echo $title; ?></b></font></td>
    </tr>
    <tr>
      <td colspan="2" width="625"><? echo $suggest_exp; ?><br><small><? echo $link; ?></small></td>
    </tr>
    <tr>
      <td width="232"><? echo $yourname; ?>&nbsp;</td>
      <td width="381"><input type="text" value="<? echo $sender_name; ?>" name="sender_name" size="40"></td>
    </tr>
    <tr>
      <td width="232"><? echo $yourmail; ?></td>
      <td width="381"><input type="text" value="<? echo $sender_mail; ?>" name="sender_mail" size="40"></td>
    </tr>
    <tr>
      <td width="232"><? echo $recipientmail; ?></td>
      <td width="381"><input type="text" value="<? echo $friend_mail; ?>" name="friend_mail" size="40"></td>
    </tr>
    <tr>
      <td width="232"><? echo $yourmessage; ?></td>
      <td width="381"><textarea name="optional_message" cols="45" rows="8"></textarea></td>
    </tr>
    <tr>
      <td width="232">&nbsp;</td>
      <td width="381">
        <div align="left">
          <input type="submit" name="submit" value="<? echo $submitbutton; ?>">
        </div>
      </td>
    </tr>
  </table>
</form>

</body>
</html>


<?


}

function printthanks($sender_name,$friend_mail,$link){
include("config.php");
	?>

<html>
<head>
<META NAME="Title" CONTENT="<? echo $title; ?>">
<META NAME="Author" CONTENT="RMEI">
<title>Thanks for sending <? echo $link; ?> to a friend!</title>
<link rel="stylesheet" href="stylesheet.php" type="text/css">
</head>
<body topmargin="20" leftmargin="20">
Thank you <? echo $sender_name; ?> for sending..<br><? echo $link; ?> to a friend!
<p>
<center>
<A HREF="javascript:window.close();">Close window</A>
</center>
</body>
</html>

<?

}

?>