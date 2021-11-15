<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="mainpage.aspx.cs" Inherits="Photo_Album_final.mainpage" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="style.css" rel="stylesheet" />
</head>

<body>
            

    <form id="form1" runat="server">
            
        <div>     
            <asp:Panel ID="photopanel" runat="server" Visible="False" CssClass="photopanel">.          
                <asp:Button ID="Backbtn" runat="server" Text="Back" CssClass="backbtn" OnClick="Backbtn_Click" />
                <asp:Image ID="Image1" runat="server" class="displayimage"/>   
            </asp:Panel>

            <asp:Panel ID="viewallpanel" runat="server" class="allpanel"></asp:Panel>
        </div>        

        <div class="navpanel">
            <asp:Panel ID="navpanel"  runat="server"></asp:Panel>
        </div>
        


    </form>
</body>
</html>
