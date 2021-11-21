<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Albums.aspx.cs" Inherits="Photo_Album_final.Albums" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="style.css" rel="stylesheet" />
</head>
<body>
    <form id="form1" runat="server">
         <div>     
             <asp:Panel ID="photopanel" runat="server" Visible="False" CssClass="photopanel">
                 <asp:Button ID="addbtn" runat="server" Text="remove" CssClass="addbtn" OnClick="addbtn_Click" />
                <asp:Button ID="deletebtn" runat="server" Text="Delete" CssClass="deletebtn" OnClick="deletebtn_Click" />
                <asp:Button ID="cancelbtn" runat="server" Text="Cancel" CssClass="cancelbtn" OnClick="cancelbtn_Click" Visible="False" />
                <asp:Label ID="changelbl" runat="server" Text="test" CssClass="uploaderror" Visible="False"></asp:Label>
                <asp:Button ID="Backbtn" runat="server" Text="Back" CssClass="backbtn" OnClick="Backbtn_Click" />
                <asp:Image ID="Image1" runat="server" class="displayimage"/>  
                <asp:Button ID="Downloadbtn" runat="server" Text="Download" CssClass="downloadbtn" OnClick="Downloadbtn_Click" />
                <asp:Button ID="Sharebtn" runat="server" Text="Share" CssClass="sharebtn" OnClick="Sharebtn_Click" />
                <asp:Button ID="changenamebtn" runat="server" Text="Change name" CssClass="Changenamebtn" OnClick="changenamebtn_Click" />
                <asp:TextBox ID="changenametxb" runat="server" placeholder="Enter new photo name" CssClass="changename" Visible="False"></asp:TextBox>
                 <asp:DropDownList ID="users" runat="server" placeholder="Select user to share with" Visible="False" CssClass="search" OnSelectedIndexChanged="users_SelectedIndexChanged" ></asp:DropDownList>
                 
            </asp:Panel>

            <asp:Panel ID="viewallpanel" runat="server" class="allpanel" Visible="True"></asp:Panel>    
             <asp:Panel ID="Panel1" runat="server" class="allpanel" Visible="false"></asp:Panel> 
             
            <asp:Panel ID="navpanel" runat="server" class="navpanel">  
                <asp:Label ID="Label2" CssClass="welcome" Text="Welcome" runat="server" ></asp:Label>
                <asp:Label ID="welcomelabel" CssClass="labelwelcome" runat="server" ></asp:Label>                    
                <asp:Button ID="logout" CssClass="btnlogout" runat="server"  Text="Logout" OnClick="logout_Click" />
            </asp:Panel>

            <asp:Panel ID="searchpanel" runat="server" class="searchpanel" Visible="True">                      
                <asp:Button ID="btnupload" CssClass="btnupload" runat="server"  Text="Create album" OnClick="btnupload_Click" />
                <asp:Button ID="btnsearch" CssClass="btnsearch" runat="server"  Text="Delete album"  OnClick="btnsearch_Click" Visible="False" />
                <asp:Button ID="btnsharealbum" CssClass="btnsharealbum2" runat="server"  Text="share album"  OnClick="btnsharealbum_Click" Visible="False" />
                <asp:Button ID="btnviewall" CssClass="viewallbtn" runat="server"  Text="viewall" OnClick="btnviewall_Click" />
                <asp:Button ID="btnalbums" CssClass="Albumbtn" runat="server"  Text="Albums" OnClick="btnalbums_Click"  />
                <asp:Button ID="btnshared" CssClass="sharedbtn" runat="server"  Text="Shared" OnClick="btnshared_Click"  />
                <asp:Button ID="btnrecieved" CssClass="recievedbtn" runat="server"  Text="Recieved fotos" OnClick="btnrecieved_Click"  />
                <asp:Button ID="btnrecieved2" CssClass="btnRecieveAlbum" runat="server"  Text="Recieved albums" OnClick="btnrecieved2_Click"  />
            </asp:Panel>
  
            <asp:Panel ID="uploadpanel" runat="server" Visible="False" CssClass="photopanel">.
                <asp:Button ID="Button1" runat="server" Text="Back" CssClass="backbtn" OnClick="Backbtn_Click" />
                <asp:Image ID="ImgPrv" runat="server" class="ImgPrv"/>
                <asp:TextBox ID="fototxb" runat="server" placeholder="Enter album name" CssClass="photoname" ></asp:TextBox>
                <asp:Button ID="savenbtn" runat="server" Text="Create" CssClass="btnssave" OnClick="savenbtn_Click" />
                <asp:Label ID="uploaderror" runat="server" Text="test" CssClass="uploaderror" Visible="False"></asp:Label>
            </asp:Panel>

             <asp:Panel ID="Panel2" runat="server" Visible="False" CssClass="photopanel">.
                <asp:Button ID="Button2" runat="server" Text="Back" CssClass="backbtn" OnClick="Backbtn_Click" />
                <asp:Button ID="Button3" runat="server" Text="share" CssClass="btnsharealbum" OnClick="sharealbum_Click" />
                <asp:Label ID="Label1" runat="server" Text="test" CssClass="uploaderror" Visible="False"></asp:Label>
                 <asp:DropDownList ID="users2" runat="server" placeholder="Select user to share with" Visible="true" CssClass="searchalbum" OnSelectedIndexChanged="users_SelectedIndexChangedalbum" ></asp:DropDownList>
            </asp:Panel>
             
        </div>
    </form>
</body>
</html>
