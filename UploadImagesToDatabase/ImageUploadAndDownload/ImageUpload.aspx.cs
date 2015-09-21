using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;

namespace UploadImagesToDatabase.ImageUploadAndDownload
{
    public partial class ImageUpload : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                lblMessage.Visible = false;
                hyperlink.Visible = false;
                LoadImages();
            }
        }

        private void LoadImages()
        {
            string connectionString = ConfigurationManager.ConnectionStrings["ConnHomeworkDB"].ConnectionString;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand("Select * from tblImages", connection);
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                GridView1.DataSource = reader;
                GridView1.DataBind();
            }
        }

        protected void btnUpload_Click(object sender, EventArgs e)
        {
            HttpPostedFile postedFile = FileUpload1.PostedFile;
            string fileName = Path.GetFileName(postedFile.FileName);
            string extension = Path.GetExtension(fileName);
            int fileSize = postedFile.ContentLength;

            if (extension.ToLower() == ".jpg" || extension.ToLower() == ".bmp" ||
                extension.ToLower() == ".png" || extension.ToLower() == ".gif")
            {
                Stream stream = postedFile.InputStream;
                BinaryReader binaryReader=new BinaryReader(stream);
                byte[] bytes=binaryReader.ReadBytes((int) stream.Length);

                string connectionString = ConfigurationManager.ConnectionStrings["ConnHomeworkDB"].ConnectionString;
                using (SqlConnection connection=new SqlConnection(connectionString))
                {
                    SqlCommand command = new SqlCommand("UploadImages",connection);
                    command.CommandType=CommandType.StoredProcedure;

                    SqlParameter paramName = new SqlParameter()
                    {
                        ParameterName = "@name",
                        Value = fileName
                    };
                    command.Parameters.Add(paramName);

                    SqlParameter paramSize = new SqlParameter()
                    {
                        ParameterName = "@size",
                        Value = fileSize
                    };
                    command.Parameters.Add(paramSize);

                    SqlParameter paramImageData = new SqlParameter()
                    {
                        ParameterName = "@imageData",
                        Value = bytes
                    };
                    command.Parameters.Add(paramImageData);

                    SqlParameter paramNewId = new SqlParameter()
                    {
                        ParameterName = "@newId",
                        Value = -1,
                        Direction = ParameterDirection.Output
                    };
                    command.Parameters.Add(paramNewId);

                    connection.Open();
                    command.ExecuteNonQuery();
                    connection.Close();

                    lblMessage.Visible = true;
                    lblMessage.Text = "Upload succesfully";
                    lblMessage.ForeColor = System.Drawing.Color.Green;
                    hyperlink.Visible = true;
                    hyperlink.NavigateUrl = "~/ImageUploadAndDownload/RedirectFromImageUpload.aspx?Id=" +
                                            command.Parameters["@newId"].Value.ToString();

                    LoadImages();
                }

            }
            else
            {
                lblMessage.Visible = true;
                lblMessage.Text = "Only images (.jpg, .bmp, .png and .gif) can be uploaded";
                lblMessage.ForeColor = System.Drawing.Color.Red;
                hyperlink.Visible = false;
            }
        }
    }
}