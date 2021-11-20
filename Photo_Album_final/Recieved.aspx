<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Recieved.aspx.cs" Inherits="Photo_Album_final.Recieved" %>

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
                <asp:Button ID="deletebtn" runat="server" Text="Delete" CssClass="deletebtn" OnClick="deletebtn_Click" />
                <asp:Label ID="changelbl" runat="server" Text="test" CssClass="uploaderror" Visible="False"></asp:Label>
                <asp:Button ID="Backbtn" runat="server" Text="Back" CssClass="backbtn" OnClick="Backbtn_Click" />
                <asp:Image ID="Image1" runat="server" class="displayimage"/>  
                <asp:Button ID="Downloadbtn" runat="server" Text="Download" CssClass="downloadbtn" OnClick="Downloadbtn_Click" />
            </asp:Panel>

            <asp:Panel ID="viewallpanel" runat="server" class="allpanel" Visible="True"></asp:Panel>      
             
            <asp:Panel ID="navpanel" runat="server" class="navpanel">  
                <asp:Label ID="Label2" CssClass="welcome" Text="Welcome" runat="server" ></asp:Label>
                <asp:Label ID="welcomelabel" CssClass="labelwelcome" runat="server" ></asp:Label>                    
                <asp:Button ID="logout" CssClass="btnlogout" runat="server"  Text="Logout" OnClick="logout_Click" />
            </asp:Panel>

            <asp:Panel ID="searchpanel" runat="server" class="searchpanel" Visible="True">                      
                <asp:Button ID="btnviewall" CssClass="viewallbtn" runat="server"  Text="viewall" OnClick="btnviewall_Click" />
                <asp:Button ID="btnalbums" CssClass="Albumbtn" runat="server"  Text="Albums" OnClick="btnalbums_Click"  />
                <asp:Button ID="btnshared" CssClass="sharedbtn" runat="server"  Text="Shared" OnClick="btnshared_Click"  />
                <asp:Button ID="btnrecieved" CssClass="recievedbtn" runat="server"  Text="Recieved" OnClick="btnrecieved_Click"  />
            </asp:Panel>
        </div>
    </form>
</body>
</html>
