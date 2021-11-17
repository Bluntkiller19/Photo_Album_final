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
        String sql, sqlinsert, sqlupdate;
        SqlDataAdapter adpt;
        String userid = "";
        string connectionString = ConfigurationManager.AppSettings["Storageconnection"].ToString();
        string accountname = "project2photostorage";

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
                searchpanel.Visible = false;
                viewallpanel.Visible = false;
                welcomelabel.Text = "Please Login!";
                logout.Text = "Login";
            }
        }
        string photourl;
        void imgbtn_Click(object sender, ImageClickEventArgs e)
        {
            photourl = ((ImageButton)sender).ImageUrl.ToString();
            searchpanel.Visible = false;
            Image1.ImageUrl = photourl;

            viewallpanel.Visible = false;
            photopanel.Visible = true;
        }
        protected void Backbtn_Click(object sender, EventArgs e)
        {
            viewallpanel.Visible = true;
            searchpanel.Visible = true;
            photopanel.Visible = false;
            uploadpanel.Visible = false;

            uploaderror.Visible = false;
            uploaderror.Text = "";
            fototxb.Text = "";
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
            uploadpanel.Visible = true;
            viewallpanel.Visible = false;
            searchpanel.Visible = false;
        }
        protected void savenbtn_Click(object sender, EventArgs e)
        {
            string ext, path, filename, photoname, Userid = "", azurepath;

            uploaderror.Visible = false;

            uploaderror.Text = "";

            if (FileUpload1.HasFile)
            {
                filename = FileUpload1.FileName;
                path = Server.MapPath("~\\photos\\") + System.IO.Path.GetFileName(FileUpload1.FileName);
                ext = System.IO.Path.GetExtension(filename);

                if (ext == ".jpg" || ext == ".png" || ext == ".gif")
                {
                    if (fototxb.Text != "")
                    {

                        photoname = fototxb.Text;
                        azurepath = "https://project2photostorage.blob.core.windows.net/";
                        azurepath += welcomelabel.Text.ToLower();
                        azurepath += "/";
                        azurepath += FileUpload1.FileName;

                        con = new SqlConnection(DbConnect);

                        con.Open();

                        sql = "SELECT * FROM photos WHERE photo_name = '" + photoname + "' AND users_user_id = '" + userid + "'";

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
                            try
                            {

                                FileUpload1.SaveAs(path);

                                StorageCredentials creden = new StorageCredentials(accountname, connectionString);
                                CloudStorageAccount acc = new CloudStorageAccount(creden, useHttps: true);
                                CloudBlobClient client = acc.CreateCloudBlobClient();
                                CloudBlobContainer cont = client.GetContainerReference(welcomelabel.Text.ToLower());

                                cont.CreateIfNotExists();
                                cont.SetPermissions(new BlobContainerPermissions
                                {
                                    PublicAccess = BlobContainerPublicAccessType.Blob
                                });


                                CloudBlockBlob cblob = cont.GetBlockBlobReference(FileUpload1.FileName);

                                using (Stream file = System.IO.File.OpenRead(path))
                                {
                                    cblob.UploadFromStream(file);
                                }

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


                                con.Open();

                                adpt = new SqlDataAdapter();

                                sqlinsert = "INSERT INTO photos (users_user_id, photo_name, photo_path) values( '" + Userid + "','" + photoname + "','" + azurepath + "')";

                                cmd = new SqlCommand(sqlinsert, con);
                                adpt.InsertCommand = new SqlCommand(sqlinsert, con);
                                adpt.InsertCommand.ExecuteNonQuery();


                                File.Delete(path);

                                Response.Redirect("Mainpage.aspx");
                                viewallpanel.Visible = true;
                                searchpanel.Visible = true;
                                photopanel.Visible = false;
                                uploadpanel.Visible = false;
                                uploaderror.Visible = false;
                                uploaderror.Text = "";
                                fototxb.Text = "";
                            }
                            catch (Exception ex)
                            {
                                uploaderror.Visible = true;
                                uploaderror.Text = "File Not Uploaded!!" + ex.Message.ToString();
                            }
                        }
                    }
                    else
                    {
                        uploaderror.Visible = true;
                        uploaderror.Text = "Please enter a name for the photo!";
                        fototxb.Text = "";

                    }
                }
                else
                {
                    uploaderror.Visible = true;
                    uploaderror.Text = "photos need to be in .jpg, .png or .gif format!";
                    fototxb.Text = "";
                }
            }
            else
            {
                uploaderror.Visible = true;
                uploaderror.Text = "Please select a photo!";
            }
        }
        protected void Downloadbtn_Click(object sender, EventArgs e)
        {
            if (Downloadbtn.Text == "Download")
            {

            }
            else if (Downloadbtn.Text == "Save")
            {
                try
                {
                    if (changenametxb.Text != "")
                    {
                        con = new SqlConnection(DbConnect);

                        con.Open();

                        sql = "SELECT * FROM photos WHERE photo_path = '" + Image1.ImageUrl.ToString() + "' AND users_user_id = '" + userid + "'";

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

        }
        protected void changenamebtn_Click(object sender, EventArgs e)
        {
            if (changenamebtn.Text == "Change name")
            {
                changenametxb.Visible = true;
                Downloadbtn.Text = "Save";
                changenamebtn.Text = "Cancel";
            }
            else if (changenamebtn.Text == "Cancel")
            {
                Downloadbtn.Text = "Download";
                changenamebtn.Text = "Change name";
                changenametxb.Text = "";
                changenametxb.Visible = false;
            }
            else
            {
                changelbl.Visible = true;
                changelbl.Text = "Enter a new name!";
            }

        }

        protected void deletebtn_Click(object sender, EventArgs e)
        {

        }
    }
}