<%@ Page Language="C#" AutoEventWireup="true"  CodeFile="demo.aspx.cs" Inherits="_Default" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
 <form id="form1" runat="server">
    <asp:Label ID="lbLoginStatus" runat="server"></asp:Label>
        <asp:ImageButton ID="imageButtonLogin" runat="server" 
            ImageUrl="~/Images/loginbutton.png" onclick="ImageButtonLoginClick" />        
    <p> <asp:TextBox ID="tbMessage" runat="server" Height="139px" TextMode="MultiLine" 
            Width="393px"></asp:TextBox>   
        <asp:FileUpload ID="picfileupload" runat="server" />
    </p>
    <p>  
        <asp:ImageButton ID="imageButtonSend" runat="server" ImageUrl="~/Images/sendbtn.png" 
            onclick="BtnSendClick" />    
        
    </p>  
    
    </form>
</body>
</html>
