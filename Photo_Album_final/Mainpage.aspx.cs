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
		String sql;
		protected void Page_Load(object sender, EventArgs e)
		{
           
            con = new SqlConnection(DbConnect);

            con.Open();

            sql = "SELECT * FROM users";

            cmd = new SqlCommand(sql, con);

            datar = cmd.ExecuteReader();

            while (datar.Read())
            {
                ImageButton imgbtn = new ImageButton();
                imgbtn.ImageUrl = datar.GetValue(4).ToString();
                imgbtn.Width = Unit.Pixel(200);
                imgbtn.Height = Unit.Pixel(200);
                imgbtn.Style.Add("padding", "5px");
                imgbtn.Style.Add("margin", "2px");
                imgbtn.Click += new ImageClickEventHandler(imgbtn_Click);
                Panel1.Controls.Add(imgbtn);
            }


            datar.Close();
            cmd.Dispose();
            con.Close();
        }

        void imgbtn_Click(object sender, ImageClickEventArgs e)
        {
            
        }
    }
}