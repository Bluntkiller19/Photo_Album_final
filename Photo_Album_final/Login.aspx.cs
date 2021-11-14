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
            Loginpanel.Visible = true;
            createpanel.Visible = false;
        }
    }
}
