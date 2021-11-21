<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="Photo_Album_final.Login" %>

<!DOCTYPE html>

<html>
<head runat="server">
    <title>Login</title>
    <link href="style.css" rel="stylesheet" />
</head>
<body class="bckground">
    <form id="form1" runat="server">
    <div class="loginpanel">

        <asp:Panel ID="createpanel" runat="server" Visible="False">
            <h2>Sign Up</h2>
            <asp:Label ID="Label1" CssClass="errormesage" runat="server" Text="Account already exists" ForeColor="Red" Visible="False"></asp:Label>
            <div >
                <asp:TextBox ID="name" class="alignusername" runat="server" placeholder = "Enter Name"></asp:TextBox>
            </div>
            <div >
                <asp:TextBox ID="createEmail" class="alignpassword" runat="server" placeholder = "Enter Email"></asp:TextBox>
            </div>
              <div >
                <asp:TextBox ID="createPassword" class="aligncreatepassword" runat="server" placeholder = "Enter password" TextMode="Password"></asp:TextBox>
            </div>
            <div >
                <asp:Button ID="signupbtn" class="btnsignup" runat="server" Text="Sign up" OnClick="Signupbtn"/>
            </div>
            <div>
                <p>
                    <asp:Button ID="cancel" class="aligncancel" runat="server" Text="Cancel" OnClick="cancel_Click"/>
                </p>
            </div>
        </asp:Panel>

        <asp:Panel ID="Loginpanel" runat="server">
            <asp:Image CssClass="loginimage" ID="Image1" runat="server" ImageUrl="https://project2photostorage.blob.core.windows.net/projectphotos/470-4703547_icon-user-icon-hd-png-download.png" />
            <h2>Login</h2>
            <asp:Label ID="Label2" CssClass="errormesagelogin" runat="server" Text="Incorrect login details" ForeColor="Red" Visible="False"></asp:Label>
            <div >
                <asp:TextBox ID="Username" class="alignusername" runat="server" placeholder = "Enter Email"></asp:TextBox>
            </div>
            <div >
                <asp:TextBox ID="Password" class="alignpassword" runat="server" placeholder = "Enter password" TextMode="Password"></asp:TextBox>
            </div>
            <div >
                <asp:Button ID="btnLogin" class="btn" runat="server" Text="Login" OnClick="btnLogin_Click" />
            </div>
            <div>
                <p>
                    <asp:Button ID="create" class="alignforgot" runat="server" Text="Create Account" OnClick="create_Click" />
                </p>
                <p>
                    <asp:Button ID="forrgot" class="aligncreatenew" runat="server" Text="Forgot password" OnClick="forrgot_Click" />
                </p>
            </div>
            </asp:Panel>

            <asp:Panel ID="forgotpassword" runat="server" Visible="False">
                <h2>Reset Pasword</h2>
                <asp:Label ID="Label3" CssClass="errormesage" runat="server" Text="Account already exists" ForeColor="Red" Visible="False"></asp:Label>
            <div >
                <asp:TextBox ID="resetEmail" class="alignusername" runat="server" placeholder = "Enter Email"></asp:TextBox>
            </div>
            <div >
                <asp:TextBox ID="resetpasword" class="alignpassword" runat="server" placeholder = "Enter password" TextMode="Password"></asp:TextBox>
            </div>
            <div >
                <asp:Button ID="submit" class="btn" runat="server" Text="submit" OnClick="submit_Click" />
            </div>
            <div>
                <p>
                    <asp:Button ID="cancelreset" class="alignforgot" runat="server" Text="Cancel" OnClick="cancel_Click" />
                </p>
            </div>
        </asp:Panel>
    </div>
    </form>
</body>
</html>
