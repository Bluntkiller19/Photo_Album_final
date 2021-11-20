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
    public partial class Recieved : System.Web.UI.Page
    {

        string DbConnect = ConfigurationManager.ConnectionStrings["dbconection"].ConnectionString;
        SqlConnection con;
        SqlCommand cmd;
        SqlDataReader datar;
        String sql, sqlinsert, sqlupdate, sqldelete;
        SqlDataAdapter adpt;
        String userid = "";
        string connectionString = ConfigurationManager.AppSettings["Storageconnection"].ToString();
        string accountname = "project2photostorage";
        string myid = "";
        string imageid = "";
        string sendid = "";
        String reciever = "";
        string photourl;


        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["Username"] != null)
            {
                con = new SqlConnection(DbConnect);


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

                sql = "SELECT * FROM photos ";
                sql += "JOIN send_photo ON photos.photo_id = send_photo.photos_photo_id ";
                sql += "WHERE send_photo.users_user_id ='" + userid + "'";

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
                searchpanel.Visible = false;
                viewallpanel.Visible = false;
                welcomelabel.Text = "Please Login!";
                logout.Text = "Login";
            }
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
        protected void imgbtn_Click(object sender, ImageClickEventArgs e)
        {

            photourl = ((ImageButton)sender).ImageUrl.ToString();

            Image1.ImageUrl = photourl;

            photopanel.Visible = true;
            searchpanel.Visible = false;
            viewallpanel.Visible = false;
        }
        protected void Backbtn_Click(object sender, EventArgs e)
        {
            viewallpanel.Visible = true;
            searchpanel.Visible = true;
            photopanel.Visible = false;

            changelbl.Visible = false;

            deletebtn.Enabled = true;
            Downloadbtn.Enabled = true;
            Downloadbtn.Text = "Download";
        }
        protected void deletebtn_Click(object sender, EventArgs e)
        {
            string url = Image1.ImageUrl.ToString();
            String name = System.IO.Path.GetFileName(url);
            String Id = "", pid = "";

            con.Open();

            sql = "SELECT * FROM photos Where photo_path ='" + url + "'";

            cmd = new SqlCommand(sql, con);

            datar = cmd.ExecuteReader();

            if (datar.Read())
            {
                pid = datar.GetValue(1).ToString();
            }
            datar.Close();
            cmd.Dispose();
            con.Close();

            con.Open();

            sql = "SELECT * FROM users Where user_name ='" + welcomelabel.Text + "'";

            cmd = new SqlCommand(sql, con);

            datar = cmd.ExecuteReader();

            if (datar.Read())
            {
                Id = datar.GetValue(0).ToString();
            }
            datar.Close();
            cmd.Dispose();
            con.Close();
            try
            {
                con.Open();
                adpt = new SqlDataAdapter();
                sqldelete = "DELETE send_photo WHERE users_user_id = '" + Id + "' AND photos_photo_id = '" + pid +"'";
                cmd = new SqlCommand(sqldelete, con);

                adpt.DeleteCommand = new SqlCommand(sqldelete, con);
                adpt.DeleteCommand.ExecuteNonQuery();

                con.Close();
                datar.Close();
                cmd.Dispose();
                Response.Redirect("Recieved.aspx");
            }
            catch
            {
                changelbl.Visible = true;
                changelbl.Text = "could not delete photo!";
            }

        }
        protected void Downloadbtn_Click(object sender, EventArgs e)
        {
            string url = Image1.ImageUrl.ToString();
            String Imagename = System.IO.Path.GetFileName(url);
            String ext = System.IO.Path.GetExtension(Imagename);
            String name = "";
            Response.ClearContent();
            Response.Buffer = true;
            String filename = "";
                

                con = new SqlConnection(DbConnect);

                con.Open();

                sql = "SELECT * FROM users ";
                sql += "JOIN photos ON users.user_id = photos.users_user_id ";
                sql += "WHERE users.user_id = photos.users_user_id";

                cmd = new SqlCommand(sql, con);

                datar = cmd.ExecuteReader();

                if (datar.Read())
                {
                    filename = datar.GetValue(2).ToString();
                }
                con.Close();
                datar.Close();
                cmd.Dispose();


                con = new SqlConnection(DbConnect);

                con.Open();

                sql = "SELECT * FROM photos WHERE photo_path = '" + url + "'";

                cmd = new SqlCommand(sql, con);

                datar = cmd.ExecuteReader();

                if (datar.Read())
                {
                    name = datar.GetValue(2).ToString();
                }
                name += ext;
                con.Close();
                datar.Close();
                cmd.Dispose();

                StorageCredentials creden = new StorageCredentials(accountname, connectionString);
                CloudStorageAccount acc = new CloudStorageAccount(creden, useHttps: true);
                CloudBlobClient client = acc.CreateCloudBlobClient();
                CloudBlobContainer cont = client.GetContainerReference(filename.ToLower());

                CloudBlockBlob cblob = cont.GetBlockBlobReference(Imagename);

                MemoryStream memstream = new MemoryStream();


                cblob.DownloadToStream(memstream);


                Response.ClearContent();
                Response.Buffer = true;
                Response.ContentType = cblob.Properties.ContentType.ToString();
                Response.AddHeader("content-disposition", string.Format("attachment; filename={0}", name));
                Response.AddHeader("Content-Length", cblob.Properties.Length.ToString());
                Response.BinaryWrite(memstream.ToArray());

                StringWriter stringWriter = new StringWriter();
                HtmlTextWriter htmlTextWriter = new HtmlTextWriter(stringWriter);
                Response.Write(stringWriter.ToString());

                Response.End();
                changelbl.Text = cblob.Properties.Length.ToString();

                changelbl.Visible = true;
                changelbl.Text = filename;
        }
        protected void btnviewall_Click(object sender, EventArgs e)
        {
            Response.Redirect("Mainpage.aspx");
        }
        protected void btnshared_Click(object sender, EventArgs e)
        {
            Response.Redirect("Shared.aspx");
        }
        protected void btnrecieved_Click(object sender, EventArgs e)
        {
            Response.Redirect("Recieved.aspx");
        }
        protected void btnalbums_Click(object sender, EventArgs e)
        {
            Response.Redirect("Albums.aspx");
        }
    }
}