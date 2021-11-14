<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="Photo_Album_final.Login" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Login</title>
    <link href="style.css" rel="stylesheet" />
</head>
<body class="bckground">
    <form id="form1" runat="server">
    <div class="loginpanel">

        <asp:Panel ID="createpanel" runat="server" Visible="False">
            <h2>Login</h2>
            <div >
                <asp:TextBox ID="TextBox1" class="alignusername" runat="server" placeholder = "Enter Email"></asp:TextBox>
            </div>
            <div >
                <asp:TextBox ID="TextBox2" class="alignpassword" runat="server" placeholder = "Enter password"></asp:TextBox>
            </div>
            <div >
                <asp:Button ID="Button1" class="btn" runat="server" Text="Login"/>
            </div>
            <div>
                <p>
                    <asp:Button ID="cancel" class="alignforgot" runat="server" Text="Cancel" OnClick="cancel_Click"/>
                </p>
            </div>
        </asp:Panel>

        <asp:Panel ID="Loginpanel" runat="server">
            <asp:Image CssClass="loginimage" ID="Image1" runat="server" ImageUrl="https://project2photostorage.blob.core.windows.net/projectphotos/470-4703547_icon-user-icon-hd-png-download.png" />
            <h2>Login</h2>
            <div >
                <asp:TextBox ID="Username" class="alignusername" runat="server" placeholder = "Enter Email"></asp:TextBox>
            </div>
            <div >
                <asp:TextBox ID="Password" class="alignpassword" runat="server" placeholder = "Enter password"></asp:TextBox>
            </div>
            <div >
                <asp:Button ID="btnLogin" class="btn" runat="server" Text="Login" OnClick="btnLogin_Click" />
            </div>
            <div>
                <p>
                    <asp:Button ID="create" class="alignforgot" runat="server" Text="Create Account" OnClick="create_Click" />
                </p>
                <p>
                    <asp:Button ID="forrgot" class="aligncreatenew" runat="server" Text="Forgot password" />
                </p>
            </div>
        </asp:Panel>
    </div>
    </form>
</body>
</html>
