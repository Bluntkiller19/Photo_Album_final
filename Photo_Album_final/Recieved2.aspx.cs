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

    public partial class Recieved2 : System.Web.UI.Page
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

                sql = "SELECT * FROM album ";
                sql += "JOIN send_album ON album.album_id = send_album.album_album_id ";
                sql += "WHERE send_album.users_user_id ='" + userid + "'";

                cmd = new SqlCommand(sql, con);

                datar = cmd.ExecuteReader();

                while (datar.Read())
                {
                    Button btn = new Button();
                    btn.Text = datar.GetValue(2).ToString();
                    btn.Width = Unit.Pixel(150);
                    btn.Height = Unit.Pixel(50);
                    btn.Style.Add("padding", "5px");
                    btn.Style.Add("margin", "2px");
                    btn.Click += new EventHandler(btn_Click);
                    viewallpanel.Controls.Add(btn);
                }
                datar.Close();
                cmd.Dispose();
                con.Close();
                if (Session["album"] != null)
                {
                    if (IsPostBack)
                    {
                        Panel1.Visible = true;
                        viewallpanel.Visible = false;
                        con.Open();

                        sql = "SELECT * FROM photos ";
                        sql += "JOIN album_photos ON photos.photo_id = album_photos.photos_photo_id ";
                        sql += "WHERE album_photos.album_album_id  ='" + Session["album"].ToString() + "'";

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
                            imgbtn.Click += new ImageClickEventHandler(imgbtn2_Click);
                            Panel1.Controls.Add(imgbtn);
                        }
                        datar.Close();
                        cmd.Dispose();
                        con.Close();
                        Session["album"] = null;
                    }
                }
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
        protected void imgbtn2_Click(object sender, ImageClickEventArgs e)
        {

            photourl = ((ImageButton)sender).ImageUrl.ToString();

            Image1.ImageUrl = photourl;

            photopanel.Visible = true;
            Panel1.Visible = false;
            searchpanel.Visible = false;
            viewallpanel.Visible = false;
        }
        protected void btn_Click(object sender, EventArgs e)
        {
            if (IsPostBack)
            {
                con = new SqlConnection(DbConnect);


                con.Open();

                sql = "SELECT * FROM album WHERE album_name = '" + ((Button)sender).Text + "'";

                cmd = new SqlCommand(sql, con);

                datar = cmd.ExecuteReader();

                if (datar.Read())
                {
                    Session["album"] = datar.GetValue(1).ToString();
                    Session["albumid"] = datar.GetValue(1).ToString();

                }
                con.Close();
                datar.Close();
                cmd.Dispose();
            }


        }
        protected void Backbtn_Click(object sender, EventArgs e)
        {
            viewallpanel.Visible = true;
            searchpanel.Visible = true;
            photopanel.Visible = false;

            changelbl.Visible = false;
            Downloadbtn.Enabled = true;
            Downloadbtn.Text = "Download";
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

            sql = "SELECT * FROM send_album WHERE users_user_id = '" + userid + "'";

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

            /*changelbl.Visible = true;
            changelbl.Text = filename;*/
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
        protected void btnrecieved2_Click(object sender, EventArgs e)
        {
            Response.Redirect("Recieved2.aspx");
        }
    }
}