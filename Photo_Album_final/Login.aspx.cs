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
using System.Configuration;
using Azure.Storage.Blobs;

namespace Photo_Album_final
{

    public partial class Login : System.Web.UI.Page
    {

        string DbConnect = ConfigurationManager.ConnectionStrings["dbconection"].ConnectionString;
        SqlConnection con;
        SqlCommand cmd;
        SqlDataReader datar;
        String sqlupdate, sqlinsert, sql;
        SqlDataAdapter adpt;

        string connectionString = ConfigurationManager.AppSettings["Storageconnection"].ToString();
        string accountname = "project2photostorage";


        protected void Page_Load(object sender, EventArgs e)
        {
           
        }

        protected void btnLogin_Click(object sender, EventArgs e)
        {
            con = new SqlConnection(DbConnect);
            con.Open();
            sql = "SELECT * FROM users WHERE user_email = '" + Username.Text + "'AND user_password = '" + Password.Text + "'";
            cmd = new SqlCommand(sql, con);
            datar = cmd.ExecuteReader();

            if (Username.Text == "" || Password.Text == "")
            {
                Label2.Visible = true;
                Label2.Text = "Fill everyting in!";
                Username.Text = "";
                Password.Text = "";
            }
            else if (datar.Read())
            {
                Session["Username"] = Username.Text;
                Response.Redirect("Mainpage.aspx");
                Username.Text = "";
                Password.Text = "";
            }
            else
            {
                Label2.Visible = true;
                Label2.Text = "wrong details!";
            }
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

            con = new SqlConnection(DbConnect);


            if (resetpasword.Text == "" || resetEmail.Text == "")
            {
                Label3.Visible = true;
                Label3.Text = "Fill in all text boxes";
                resetEmail.Text = "";
                resetpasword.Text = "";
            }
            else
            {
                try
                {
                    con.Close();

                    con.Open();

                    adpt = new SqlDataAdapter();

                    sqlupdate = "UPDATE users SET user_password = '" + resetpasword.Text + "' WHERE user_email = '" + resetEmail.Text + "'";

                    cmd = new SqlCommand(sqlupdate, con);

                    adpt.InsertCommand = new SqlCommand(sqlupdate, con);
                    adpt.InsertCommand.ExecuteNonQuery();
                    resetEmail.Text = "";
                    resetpasword.Text = "";
                    cmd.Dispose();
                    con.Close();
                    Loginpanel.Visible = true;
                    forgotpassword.Visible = false;
                }
                catch
                {
                    Label3.Visible = true;
                    Label3.Text = "could not find user!";
                    resetEmail.Text = "";
                    resetpasword.Text = "";
                }

            }
        }

        protected void Signupbtn(object sender, EventArgs e)
        {
            con = new SqlConnection(DbConnect);
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
                try
                {
                    StorageCredentials creden = new StorageCredentials(accountname, connectionString);
                    CloudStorageAccount acc = new CloudStorageAccount(creden, useHttps: true);
                    CloudBlobClient client = acc.CreateCloudBlobClient();
                    CloudBlobContainer cont = client.GetContainerReference(name.Text.ToLower());
                    cont.CreateIfNotExists();
                    cont.SetPermissions(new BlobContainerPermissions
                    {
                        PublicAccess = BlobContainerPublicAccessType.Blob
                    });

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


                    cmd.Dispose();
                    con.Close();
                    Loginpanel.Visible = true;
                    createpanel.Visible = false;
                }
                catch
                {

                    Label1.Visible = true;
                    Label1.Text = "Could not create account";
                }

            }
        }
    }
}
