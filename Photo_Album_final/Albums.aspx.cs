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
    public partial class Albums : System.Web.UI.Page
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
        String reciever;
        string photourl;
        string albumid = "";
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

                con = new SqlConnection(DbConnect);

                con.Open();

                sql = "SELECT * FROM album WHERE users_user_id = '" + userid + "'";

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

                con.Close();
                datar.Close();
                cmd.Dispose();

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
        protected void btnsearch_Click(object sender, EventArgs e)
        {
            try
            {
                con.Open();
                adpt = new SqlDataAdapter();
                sqldelete = "DELETE album WHERE album_id = '" + Session["albumid"].ToString() + "' AND users_user_id = '" + userid + "'";
                cmd = new SqlCommand(sqldelete, con);

                adpt.DeleteCommand = new SqlCommand(sqldelete, con);
                adpt.DeleteCommand.ExecuteNonQuery();

                con.Close();
                datar.Close();
                cmd.Dispose();
                Response.Redirect("Albums.aspx");
            }
            catch
            {
                Panel2.Visible = true;
                Panel2.Controls.Clear();
                Label lbl = new Label();
                lbl.Text = "Could not delete foto clikc on albums to view albums!";
                Panel2.Controls.Add(lbl);
                
            }
        }
        protected void btn_Click(object sender, EventArgs e)
        {
            if (IsPostBack)
            {
                btnsearch.Visible = true;
                btnsharealbum.Visible = true;
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
        protected void imgbtn2_Click(object sender, ImageClickEventArgs e)
        {
            photourl = ((ImageButton)sender).ImageUrl.ToString();

            Image1.ImageUrl = photourl;

            photopanel.Visible = true;
            Panel1.Visible = false;
            searchpanel.Visible = false;
            viewallpanel.Visible = false;
        }
        protected void Backbtn_Click(object sender, EventArgs e)
        {
            Panel2.Visible = false;
            Panel1.Visible = false;
            viewallpanel.Visible = true;
            searchpanel.Visible = true;
            photopanel.Visible = false;
            uploadpanel.Visible = false;

            uploaderror.Visible = false;
            uploaderror.Text = "";
            fototxb.Text = "";

            cancelbtn.Visible = false;
            changenametxb.Text = "";
            changenametxb.Visible = false;
            changelbl.Visible = false;

            deletebtn.Enabled = true;
            Sharebtn.Enabled = true;
            changenamebtn.Enabled = true;
            addbtn.Enabled = true;
            Downloadbtn.Enabled = true;
            Sharebtn.Text = "Share";
            Downloadbtn.Text = "Download";

            users.Visible = false;

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
            Panel1.Visible = false;
            uploadpanel.Visible = true;
            viewallpanel.Visible = false;
            searchpanel.Visible = false;
        }
        protected void savenbtn_Click(object sender, EventArgs e)
        {
            String lalbumname = fototxb.Text;
            if (fototxb.Text != "")
            {
                con = new SqlConnection(DbConnect);

                con.Open();

                sql = "SELECT * FROM album WHERE album_name = '" + lalbumname + "' AND users_user_id = '" + userid + "'";

                cmd = new SqlCommand(sql, con);

                datar = cmd.ExecuteReader();

                if (datar.Read())
                {
                    uploaderror.Visible = true;
                    uploaderror.Text = "Filename already exists";

                    fototxb.Text = "";

                    con.Close();
                    datar.Close();
                    cmd.Dispose();
                }
                else
                {
                    con.Close();
                    datar.Close();
                    cmd.Dispose();


                    con.Open();

                    adpt = new SqlDataAdapter();

                    sqlinsert = "INSERT INTO album (users_user_id, album_name) values( '" + userid + "','" + lalbumname +  "')";

                    cmd = new SqlCommand(sqlinsert, con);
                    adpt.InsertCommand = new SqlCommand(sqlinsert, con);
                    adpt.InsertCommand.ExecuteNonQuery();

                    Response.Redirect("Albums.aspx");

                }
            }
            else
            {
                uploaderror.Visible = true;
                uploaderror.Text = "Enter a name for your album!";
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

            if (Downloadbtn.Text == "Download")
            {
                String filename = "";


                con = new SqlConnection(DbConnect);

                con.Open();

                sql = "SELECT * FROM send_photo WHERE users_user_id = '" + userid + "'";

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


            }
            else if (Downloadbtn.Text == "Save")
            {
                try
                {
                    if (changenametxb.Text != "")
                    {
                        con = new SqlConnection(DbConnect);

                        con.Open();

                        sql = "SELECT * FROM photos WHERE photo_name = '" + changenametxb.Text + "' AND users_user_id = '" + userid + "'";

                        cmd = new SqlCommand(sql, con);

                        datar = cmd.ExecuteReader();

                        if (datar.Read())
                        {
                            changelbl.Visible = true;
                            changelbl.Text = "Filename already exists";

                            changenametxb.Text = "";

                            con.Close();
                            datar.Close();
                            cmd.Dispose();
                        }
                        else
                        {
                            con.Close();

                            con.Open();

                            adpt = new SqlDataAdapter();

                            sqlupdate = "UPDATE photos SET photo_name = '" + changenametxb.Text + "' WHERE photo_path = '" + Image1.ImageUrl.ToString() + "'";

                            cmd = new SqlCommand(sqlupdate, con);

                            adpt.InsertCommand = new SqlCommand(sqlupdate, con);
                            adpt.InsertCommand.ExecuteNonQuery();
                            Downloadbtn.Text = "Download";
                            changenamebtn.Text = "Change name";
                            changenametxb.Text = "";
                            changenametxb.Visible = false;

                            deletebtn.Enabled = true;
                            Sharebtn.Enabled = true;
                            changenamebtn.Enabled = true;
                            cancelbtn.Visible = false;
                            changelbl.Visible = false;
                        }
                    }
                    else
                    {
                        changelbl.Visible = true;
                        changelbl.Text = "Enter a new name!";
                    }
                }
                catch
                {
                    changelbl.Visible = true;
                    changelbl.Text = "could not find image! ";
                }
            }
        }
        protected void Sharebtn_Click(object sender, EventArgs e)
        {
            users.Items.Clear();
            users.Items.Insert(0, new ListItem("--Select User--", "0"));

            con.Open();

            sql = "SELECT * FROM users WHERE user_name = '" + welcomelabel.Text + "'";

            cmd = new SqlCommand(sql, con);

            datar = cmd.ExecuteReader();

            if (datar.Read())
            {
                myid = datar.GetValue(2).ToString();
                con.Close();
                datar.Close();
                cmd.Dispose();
            }

            con = new SqlConnection(DbConnect);

            con.Open();

            sql = "SELECT * FROM photos WHERE photo_path = '" + Image1.ImageUrl.ToString() + "'";

            cmd = new SqlCommand(sql, con);

            datar = cmd.ExecuteReader();

            if (datar.Read())
            {
                imageid = datar.GetValue(1).ToString();
                con.Close();
                datar.Close();
                cmd.Dispose();
            }

            con = new SqlConnection(DbConnect);

            con.Open();

            sql = "SELECT* FROM users WHERE user_name != '" + myid + "'";

            cmd = new SqlCommand(sql, con);

            datar = cmd.ExecuteReader();
            while (datar.Read())
            {
                users.Items.Add(new ListItem(datar.GetValue(2).ToString(), datar.GetValue(0).ToString()));
            }
            cmd.Dispose();
            con.Close();
            datar.Close();

            if (Sharebtn.Text == "Share")
            {
                users.Visible = true;
                cancelbtn.Visible = true;
                Sharebtn.Text = "Send";

                changenamebtn.Enabled = false;
                deletebtn.Enabled = false;
                Downloadbtn.Enabled = false;
            }
            else if (Sharebtn.Text == "Send")
            {

                if (reciever == "")
                {
                    changelbl.Visible = true;
                    changelbl.Text = "Please select a user to send to!";
                }
                else
                {

                    con = new SqlConnection(DbConnect);
                    con.Open();

                    sql = "SELECT * FROM users WHERE user_name = '" + reciever + "'";

                    cmd = new SqlCommand(sql, con);

                    datar = cmd.ExecuteReader();

                    if (datar.Read())
                    {
                        sendid = datar.GetValue(0).ToString();
                        con.Close();
                        datar.Close();
                        cmd.Dispose();
                    }


                    con.Close();

                    con.Open();

                    adpt = new SqlDataAdapter();

                    sqlinsert = "INSERT INTO send_photo (users_user_id, photos_photo_id, sender_name) values(  '" + sendid + "','" + imageid + "','" + myid + "')";

                    cmd = new SqlCommand(sqlinsert, con);
                    adpt.InsertCommand = new SqlCommand(sqlinsert, con);
                    adpt.InsertCommand.ExecuteNonQuery();


                    cmd.Dispose();
                    con.Close();
                    users.Visible = false;
                    Sharebtn.Text = "Share";
                    cancelbtn.Visible = false;

                    changenamebtn.Enabled = true;
                    deletebtn.Enabled = true;
                    Downloadbtn.Enabled = true;
                }
            }
        }
        protected void sharealbum_Click(object sender, EventArgs e)
        {

                if (reciever == "")
                {
                    Label1.Visible = true;
                    Label1.Text = "Please select a user to send to!";
                }
                else
                {

                    con = new SqlConnection(DbConnect);
                    con.Open();

                    sql = "SELECT * FROM users WHERE user_name = '" + reciever + "'";

                    cmd = new SqlCommand(sql, con);

                    datar = cmd.ExecuteReader();

                    if (datar.Read())
                    {
                        sendid = datar.GetValue(0).ToString();
                        con.Close();
                        datar.Close();
                        cmd.Dispose();
                    }
                    con.Close();
                    

                    con.Open();

                    adpt = new SqlDataAdapter();

                    sqlinsert = "INSERT INTO send_album (users_user_id, album_album_id, sender_name) values(  '" + sendid + "','" + Session["albumid"].ToString() + "','" + welcomelabel.Text + "')";

                    cmd = new SqlCommand(sqlinsert, con);
                    adpt.InsertCommand = new SqlCommand(sqlinsert, con);
                    adpt.InsertCommand.ExecuteNonQuery();

                    Response.Redirect("Mainpage.aspx");
                    cmd.Dispose();
                    con.Close();
                }
        }
        protected void btnsharealbum_Click(object sender, EventArgs e)
        {
            searchpanel.Visible = false;
            Panel2.Visible = true;
            Panel1.Visible = false;
            viewallpanel.Visible = false;
            users2.Items.Clear();
            users2.Items.Insert(0, new ListItem("--Select User--", "0"));

            con.Open();

            sql = "SELECT * FROM users WHERE user_name = '" + welcomelabel.Text + "'";

            cmd = new SqlCommand(sql, con);

            datar = cmd.ExecuteReader();

            if (datar.Read())
            {
                myid = datar.GetValue(2).ToString();
                con.Close();
                datar.Close();
                cmd.Dispose();
            }

            con = new SqlConnection(DbConnect);

            con.Open();

            sql = "SELECT * FROM album WHERE album_name = '" + ((Button)sender).Text + "'";

            cmd = new SqlCommand(sql, con);

            datar = cmd.ExecuteReader();

            if (datar.Read())
            {
                Session["albumid"] = datar.GetValue(1).ToString();
                con.Close();
                datar.Close();
                cmd.Dispose();
            }

            con = new SqlConnection(DbConnect);

            con.Open();

            sql = "SELECT* FROM users WHERE user_name != '" + myid + "'";

            cmd = new SqlCommand(sql, con);

            datar = cmd.ExecuteReader();
            while (datar.Read())
            {
                users2.Items.Add(new ListItem(datar.GetValue(2).ToString(), datar.GetValue(0).ToString()));
            }
            cmd.Dispose();
            con.Close();
            datar.Close();


        }
        protected void cancelbtn_Click(object sender, EventArgs e)
        {
            Downloadbtn.Text = "Download";
            Sharebtn.Text = "Share";
            cancelbtn.Visible = false;
            changenametxb.Text = "";
            changenametxb.Visible = false;

            changenamebtn.Enabled = true;
            deletebtn.Enabled = true;
            Sharebtn.Enabled = true;
            Downloadbtn.Enabled = true;

            users.Visible = false;
        }
        protected void changenamebtn_Click(object sender, EventArgs e)
        {
            changenamebtn.Enabled = false;
            deletebtn.Enabled = false;
            addbtn.Enabled = false;
            Sharebtn.Enabled = false;

            changenametxb.Visible = true;
            Downloadbtn.Text = "Save";
            cancelbtn.Visible = true;

        }
        protected void addbtn_Click(object sender, EventArgs e)
        {

            string photoid = "";
            con = new SqlConnection(DbConnect);

            con.Open();

            sql = "SELECT * FROM photos WHERE photo_path = '" + Image1.ImageUrl.ToString() + "'";

            cmd = new SqlCommand(sql, con);

            datar = cmd.ExecuteReader();
            if (datar.Read())
            {
                photoid = datar.GetValue(1).ToString();
            }

            con.Close();
            datar.Close();
            cmd.Dispose();


            try
            {
                con.Open();
                adpt = new SqlDataAdapter();
                sqldelete = "DELETE album_photos WHERE photos_photo_id = '" + photoid + "' AND album_album_id = '" + Session["albumid"].ToString() + "'";
                cmd = new SqlCommand(sqldelete, con);

                adpt.DeleteCommand = new SqlCommand(sqldelete, con);
                adpt.DeleteCommand.ExecuteNonQuery();

                con.Close();
                datar.Close();
                cmd.Dispose();
                Response.Redirect("Albums.aspx");
            }
            catch
            {
               changelbl.Text = "could not delete photo!";
            }
        }
        protected void deletebtn_Click(object sender, EventArgs e)
        {
            string url = Image1.ImageUrl.ToString();
            String name = System.IO.Path.GetFileName(url);

            try
            {
                con.Open();
                adpt = new SqlDataAdapter();
                sqldelete = "DELETE photos WHERE photo_path = '" + url + "'";
                cmd = new SqlCommand(sqldelete, con);

                adpt.DeleteCommand = new SqlCommand(sqldelete, con);
                adpt.DeleteCommand.ExecuteNonQuery();

                StorageCredentials creden = new StorageCredentials(accountname, connectionString);
                CloudStorageAccount acc = new CloudStorageAccount(creden, useHttps: true);
                CloudBlobClient client = acc.CreateCloudBlobClient();
                CloudBlobContainer cont = client.GetContainerReference(welcomelabel.Text.ToLower());

                CloudBlockBlob cblob = cont.GetBlockBlobReference(name);
                cblob.Delete();

                con.Close();
                datar.Close();
                cmd.Dispose();
                Response.Redirect("Mainpage.aspx");
            }
            catch
            {
                changelbl.Visible = true;
                changelbl.Text = "could not delete photo!";
            }

        }
        protected void users_SelectedIndexChanged(object sender, EventArgs e)
        {
            reciever = users.SelectedItem.Text;
        }
  
        protected void users_SelectedIndexChangedalbum(object sender, EventArgs e)
        {
            reciever = users2.SelectedItem.Text;
        }
        protected void btnviewall_Click(object sender, EventArgs e)
        {
            Session["album"] = null;
            Response.Redirect("Mainpage.aspx");
        }
        protected void btnshared_Click(object sender, EventArgs e)
        {
            Session["album"] = null;
            Response.Redirect("Shared.aspx");
        }
        protected void btnrecieved_Click(object sender, EventArgs e)
        {
            Session["album"] = null;
            Response.Redirect("Recieved.aspx");
        }
        protected void btnalbums_Click(object sender, EventArgs e)
        {
            Session["album"] = null;
            Response.Redirect("Albums.aspx");
        }
        protected void btnrecieved2_Click(object sender, EventArgs e)
        {
            Response.Redirect("Recieved2.aspx");
        }

    }
}