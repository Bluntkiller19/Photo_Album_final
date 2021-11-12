using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Auth;
using Microsoft.WindowsAzure.Storage.Blob;
using System.IO;

namespace Photo_Album_final
{
    public partial class WebForm1 : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            /*string accountname = "project2photostorage";

            string accesskey = "xBEHJgBInQrP0OxrmOO8eHZrK2h76JwO4YNIL2ZHWBhLB5TjL917QGUWNQpUDQaHT/AWbIYBl6/MHZdDOhV2PA==";

            try

            {

                StorageCredentials creden = new StorageCredentials(accountname, accesskey);

                CloudStorageAccount acc = new CloudStorageAccount(creden, useHttps: true);

                CloudBlobClient client = acc.CreateCloudBlobClient();

                CloudBlobContainer cont = client.GetContainerReference("usersphotos");

                cont.CreateIfNotExists();

                cont.SetPermissions(new BlobContainerPermissions
            

        {

                    PublicAccess = BlobContainerPublicAccessType.Blob


        });

                CloudBlockBlob cblob = cont.GetBlockBlobReference("test.jpg");

                using (Stream file = System.IO.File.OpenRead(@"C:\Users\mstro\OneDrive\Documents\Klas\2021\Semester 2\Cmpg 323\Projek2\farm.jpg"))

                {

                    cblob.UploadFromStream(file);

                }

            }
            catch (Exception ex)
            {

            }*/
        }
    }
}