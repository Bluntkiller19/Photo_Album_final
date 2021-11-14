using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Microsoft.Azure;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.Storage.Auth;
using Microsoft.WindowsAzure.Storage.Blob;
using System.IO;
using System.Data.SqlClient;
using System.Data;

namespace Photo_Album_final
{
   
	public partial class Login : System.Web.UI.Page
	{

        string connetionString;
        SqlConnection con;
        SqlCommand cmd;
        SqlDataReader datar;
        String sqlinsert, sql;
        SqlDataAdapter adpt;
        
        protected void Page_Load(object sender, EventArgs e)
		{
           
        }

        protected void btnLogin_Click(object sender, EventArgs e)
        {

        }

        protected void create_Click(object sender, EventArgs e)
        {
            Loginpanel.Visible = false;
            createpanel.Visible = true;
        }

        protected void cancel_Click(object sender, EventArgs e)
        {
            forgotpassword.Visible = false;
            Loginpanel.Visible = true;
            createpanel.Visible = false;
        }
        protected void forrgot_Click(object sender, EventArgs e)
        {
            Loginpanel.Visible = false;
            forgotpassword.Visible = true;
        }

        protected void submit_Click(object sender, EventArgs e)
        {
            Loginpanel.Visible = true;
            forgotpassword.Visible = false;
        }

        protected void Signupbtn(object sender, EventArgs e)
        {
            connetionString = @"Data Source=cmpg323project2sever.database.windows.net;Initial Catalog=Project2DB;User ID=CmpgAdmin;Password=Glasstuk1!";
            con = new SqlConnection(connetionString);
            con.Open();
            sql = "SELECT * FROM users WHERE user_email = '" + createEmail.Text + "'";
            cmd = new SqlCommand(sql, con);
            datar = cmd.ExecuteReader();

            if (name.Text == "" || createEmail.Text == "" || createPassword.Text == "")
            {
                Label1.Visible = true;
                Label1.Text = "Fill in all text boxes";
                createEmail.Text = "";
                createPassword.Text = "";
                name.Text = "";
            }
            else if (datar.Read())
            {
                Label1.Visible = true;
                Label1.Text = "Email already exists";
                createEmail.Text = "";
                createPassword.Text = "";
                name.Text = "";

            }
            else
            {
                con.Close();

                con.Open();

                adpt = new SqlDataAdapter();

                sqlinsert = "INSERT INTO users (user_email, user_name, user_password) values(  '" + createEmail.Text + "','" + name.Text + "','" + createPassword.Text + "')";

                cmd = new SqlCommand(sqlinsert, con);
                adpt.InsertCommand = new SqlCommand(sqlinsert, con);
                adpt.InsertCommand.ExecuteNonQuery();
                createEmail.Text = "";
                createPassword.Text = "";
                name.Text = "";

                Loginpanel.Visible = true;
                createpanel.Visible = false;

            }
            datar.Close();
            cmd.Dispose();
            con.Close();
        }
    }
}
