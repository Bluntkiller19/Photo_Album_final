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

namespace Photo_Album_final
{
	public partial class mainpage : System.Web.UI.Page
	{
        string DbConnect = ConfigurationManager.ConnectionStrings["dbconection"].ConnectionString;
        SqlConnection con;
		SqlCommand cmd;
		SqlDataReader datar;
		String sql, sqlinsert;
        SqlDataAdapter adpt;

        string connectionString = ConfigurationManager.AppSettings["Storageconnection"].ToString();
        string accountname = "project2photostorage";

        protected void Page_Load(object sender, EventArgs e)
		{
            if (Session["Username"] != null)
            {
                searchpanel.Visible = true;
                viewallpanel.Visible = true;
                con = new SqlConnection(DbConnect);
                String userid = "";

                con.Open();

                sql = "SELECT * FROM users WHERE user_email = '" + Session["Username"].ToString() + "'";

                cmd = new SqlCommand(sql, con);

                datar = cmd.ExecuteReader();
                if (datar.Read())
                {
                    userid = datar.GetValue(0).ToString();
                    welcomelabel.Text = datar.GetValue(2).ToString();
                }

                con.Close();
                datar.Close();
                cmd.Dispose();

                con.Open();

                sql = "SELECT * FROM photos where users_user_id ='" + userid + "'";

                cmd = new SqlCommand(sql, con);

                datar = cmd.ExecuteReader();

                while (datar.Read())
                {
                    ImageButton imgbtn = new ImageButton();
                    imgbtn.ImageUrl = datar.GetValue(3).ToString();
                    imgbtn.Width = Unit.Pixel(150);
                    imgbtn.Height = Unit.Pixel(150);
                    imgbtn.Style.Add("padding", "5px");
                    imgbtn.Style.Add("margin", "2px");
                    imgbtn.Click += new ImageClickEventHandler(imgbtn_Click);
                    viewallpanel.Controls.Add(imgbtn);
                }  
                datar.Close();   
                cmd.Dispose();  
                con.Close();
            }
            else
            {
                welcomelabel.Text = "Please Login!";
                logout.Text = "Login";
            }
        }

        void imgbtn_Click(object sender, ImageClickEventArgs e)
        {
            searchpanel.Visible = false;
            Image1.ImageUrl = ((ImageButton)sender).ImageUrl.ToString();

            viewallpanel.Visible = false;
            photopanel.Visible = true;
        }
        protected void Backbtn_Click(object sender, EventArgs e)
        {
            viewallpanel.Visible = true;
            photopanel.Visible = false;
        }

        protected void logout_Click(object sender, EventArgs e)
        {
            if (logout.Text == "Logout")
            {
                Session.RemoveAll();
                Response.Redirect("Login.aspx");
            }
            else if (logout.Text == "Login")
            {
                welcomelabel.Text = "Please login";
                Response.Redirect("Login.aspx");
            }
        }
        protected void btnupload_Click(object sender, EventArgs e)
        {
            con = new SqlConnection(DbConnect);
            con.Open();

            adpt = new SqlDataAdapter();


            string Userid = "";
            string path = "https://project2photostorage.blob.core.windows.net/" + welcomelabel.Text.ToLower() + "/gamingPFP.png";


            con = new SqlConnection(DbConnect);

            con.Open();

            sql = "SELECT * FROM users WHERE user_name = '" + welcomelabel.Text + "'";

            cmd = new SqlCommand(sql, con);

            datar = cmd.ExecuteReader();
            if (datar.Read())
                Userid = datar.GetValue(0).ToString();

            con.Close();
            datar.Close();
            cmd.Dispose();


            try
            {
                con.Open();
                sqlinsert = "INSERT INTO photos (users_user_id, photo_name, photo_path) values( '" + Userid + "','" + "gamingPFP" + "','" + path + "')";

                cmd = new SqlCommand(sqlinsert, con);
                adpt.InsertCommand = new SqlCommand(sqlinsert, con);
                adpt.InsertCommand.ExecuteNonQuery();

               StorageCredentials creden = new StorageCredentials(accountname, connectionString);
                CloudStorageAccount acc = new CloudStorageAccount(creden, useHttps: true);
                CloudBlobClient client = acc.CreateCloudBlobClient();
                CloudBlobContainer cont = client.GetContainerReference(welcomelabel.Text.ToLower());

                cont.CreateIfNotExists();
                cont.SetPermissions(new BlobContainerPermissions
                {
                    PublicAccess = BlobContainerPublicAccessType.Blob
                });

                CloudBlockBlob cblob = cont.GetBlockBlobReference("gamingPFP.png");

                using (Stream file = System.IO.File.OpenRead(@"C:\Users\mstro\OneDrive\Pictures\gamingPFP.png"))
                {
                    cblob.UploadFromStream(file);
                }
            }
            catch (Exception ex)
            {

            }
            cmd.Dispose();
            con.Close();
        }
    }
}