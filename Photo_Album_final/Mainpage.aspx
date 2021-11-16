<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="mainpage.aspx.cs" Inherits="Photo_Album_final.mainpage" %>

<!DOCTYPE html>

<html>
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

            <asp:Panel ID="viewallpanel" runat="server" class="allpanel" Visible="False"></asp:Panel>      
             
            <asp:Panel ID="navpanel" runat="server" class="navpanel">                    
                <asp:Label ID="welcomelabel" CssClass="labelwelcome" runat="server" ></asp:Label>                    
                <asp:Button ID="logout" CssClass="btnlogout" runat="server"  Text="Logout" OnClick="logout_Click" />
            </asp:Panel>

            <asp:Panel ID="searchpanel" runat="server" class="searchpanel" Visible="False">                    
                <asp:Label ID="Label1" CssClass="labelwelcome" runat="server" ></asp:Label>                    
                <asp:Button ID="btnupload" CssClass="btnupload" runat="server"  Text="upload photo" OnClick="btnupload_Click" />
            </asp:Panel>
        </div>
       
        


    </form>
</body>
</html>
