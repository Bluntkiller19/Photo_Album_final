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
            <asp:Panel ID="photopanel" runat="server" Visible="False" CssClass="photopanel">
                 <asp:Button ID="deletebtn" runat="server" Text="Delete" CssClass="deletebtn" OnClick="deletebtn_Click" />
                <asp:Label ID="changelbl" runat="server" Text="test" CssClass="uploaderror" Visible="False"></asp:Label>
                <asp:Button ID="Backbtn" runat="server" Text="Back" CssClass="backbtn" OnClick="Backbtn_Click" />
                <asp:Image ID="Image1" runat="server" class="displayimage"/>  
                <asp:Button ID="Downloadbtn" runat="server" Text="Download" CssClass="downloadbtn" OnClick="Downloadbtn_Click" />
                <asp:Button ID="Sharebtn" runat="server" Text="Share" CssClass="sharebtn" OnClick="Sharebtn_Click" />
                <asp:Button ID="changenamebtn" runat="server" Text="Change name" CssClass="Changenamebtn" OnClick="changenamebtn_Click" />
                <asp:TextBox ID="changenametxb" runat="server" placeholder="Enter new photo name" CssClass="changename" Visible="False"></asp:TextBox>
            </asp:Panel>

            <asp:Panel ID="viewallpanel" runat="server" class="allpanel" Visible="True"></asp:Panel>      
             
            <asp:Panel ID="navpanel" runat="server" class="navpanel">  
                <asp:Label ID="Label2" CssClass="welcome" Text="Welcome" runat="server" ></asp:Label>
                <asp:Label ID="welcomelabel" CssClass="labelwelcome" runat="server" ></asp:Label>                    
                <asp:Button ID="logout" CssClass="btnlogout" runat="server"  Text="Logout" OnClick="logout_Click" />
            </asp:Panel>

            <asp:Panel ID="searchpanel" runat="server" class="searchpanel" Visible="True">   
                <asp:Label ID="Label1" CssClass="labelwelcome" runat="server" ></asp:Label>                    
                <asp:Button ID="btnupload" CssClass="btnupload" runat="server"  Text="upload photo" OnClick="btnupload_Click" />
            </asp:Panel>
  
            <asp:Panel ID="uploadpanel" runat="server" Visible="False" CssClass="photopanel">.
                 <asp:FileUpload ID="FileUpload1" runat="server"  CssClass="upload"/>
                <asp:Button ID="Button1" runat="server" Text="Back" CssClass="backbtn" OnClick="Backbtn_Click" />
                <asp:Image ID="Image2" runat="server" class="displayimage"/>
                <asp:TextBox ID="fototxb" runat="server" placeholder="Enter photo name" CssClass="photoname" ></asp:TextBox>
                <asp:Button ID="savenbtn" runat="server" Text="Uplaod" CssClass="btnssave" OnClick="savenbtn_Click" />
                <asp:Label ID="uploaderror" runat="server" Text="test" CssClass="uploaderror" Visible="False"></asp:Label>
            </asp:Panel>
        </div>
       
        


    </form>
</body>
</html>
