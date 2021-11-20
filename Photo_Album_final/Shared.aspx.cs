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
    public partial class Shared : System.Web.UI.Page
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
                DataTable table = new DataTable();
                // get the connection    
                using (SqlConnection conn = new SqlConnection(DbConnect))
                {
                    // write the sql statement to execute    
                    sql = "SELECT user_name, photo_name FROM send_photo ";
                    sql += "JOIN photos ON send_photo.photos_photo_id= photos.photo_id ";
                    sql += "JOIN users ON send_photo.users_user_id = users.user_id ";
                    sql += "WHERE send_photo.sender_name ='" + welcomelabel.Text + "'";
                    // instantiate the command object to fire    
                    using (SqlCommand cmd = new SqlCommand(sql, conn))
                    {
                        // get the adapter object and attach the command object to it    
                        using (SqlDataAdapter ad = new SqlDataAdapter(cmd))
                        {
                            // fire Fill method to fetch the data and fill into DataTable    
                            ad.Fill(table);
                        }
                    }
                }
                // specify the data source for the GridView    
                Shareview.DataSource = table;
                // bind the data now    
                Shareview.DataBind();
            }
            else
            {
                searchpanel.Visible = false;
                viewallpanel.Visible = false;
                welcomelabel.Text = "Please Login!";
                logout.Text = "Login";
            }
        }
        protected void OnRowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            String username = Shareview.Rows[e.RowIndex].Cells[1].Text;
            String photoname = Shareview.Rows[e.RowIndex].Cells[0].Text;
            String photoid = "", recuserid = "";

            welcomelabel.Text = username;
            con.Close();
            con.Open();
            sql = "SELECT * from users where user_name = '" + username + "'";

            cmd = new SqlCommand(sql, con);

            datar = cmd.ExecuteReader();
            if (datar.Read())
            {
                recuserid = datar.GetValue(0).ToString();

            }
            con.Close();
            datar.Close();
            cmd.Dispose();

            con.Open();
            sql = "SELECT * from photos where photo_name = '" + photoname + "'";

            cmd = new SqlCommand(sql, con);

            datar = cmd.ExecuteReader();
            if (datar.Read())
            {
                photoid = datar.GetValue(1).ToString();

            }
            con.Close();
            datar.Close();
            cmd.Dispose();

            con.Open();

            string sqldelete = "DELETE send_photo WHERE users_user_id = '" + recuserid + "' AND photos_photo_id ='" + photoid + "'";
            adpt = new SqlDataAdapter();
            cmd = new SqlCommand(sqldelete, con);

            adpt.DeleteCommand = new SqlCommand(sqldelete, con);
            adpt.DeleteCommand.ExecuteNonQuery();
            con.Close();
            datar.Close();
            cmd.Dispose();

            Response.Redirect("Shared.aspx");

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