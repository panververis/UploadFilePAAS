using Microsoft.Azure;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace SampleUploadFile.Controllers {
    public class HomeController : Controller {

        #region Controller Actions

        //  "Index" Action
        public ActionResult Index() {
            return View();
        }

        //  "Upload a file" action
        [HttpPost]
        public async Task<ActionResult> UploadFile(HttpPostedFileBase file) {

            //  First up surrounding everything in a try - catch block, to ensure safe execution
            try {

                // Checking whether a valid file of a size > 0 was selected
                if (file != null && file.ContentLength > 0) {

                    #region Previous (Basic) Uploading functionality (commented out)

                    ////  Getting the Filename
                    //string _FileName = "image.jpg"; //Path.GetFileName(File.FileName);

                    ////  Combining the to-be-uploaded file's name with the pre-specified Server - side "Images" folder
                    //string _path = Path.Combine(Server.MapPath("~/Content/Images"), _FileName);

                    ////  Lastly, attempt to save (upload) the selected file
                    //File.SaveAs(_path);

                    #endregion

                    #region Uploading functionality utilizing Azure Blob Storage

                    //  Initializing a "Blob Storage Account" instance, in order to retrieve the Blob Storage Connection String
                    string blobStorageConnectionString = String.Empty;
                    CloudStorageAccount blobStorageAccount = null;
                    try {
                        blobStorageConnectionString = CloudConfigurationManager.GetSetting("StorageConnectionString");
                        blobStorageAccount = CloudStorageAccount.Parse(blobStorageConnectionString);
                    } catch (Exception ex) {
                        ViewBag.Message = $" File upload failed. Reason: {ex.Message}";
                    }

                    //  Creating a Blob Storage Client, to have access to Blob Containers
                    CloudBlobClient blobStorageClient = blobStorageAccount.CreateCloudBlobClient();

                    //  Grab a reference to a previously created container
                    CloudBlobContainer imagesBlobContainer = blobStorageClient.GetContainerReference("images");

                    // Get the reference to the block blob from the container
                    CloudBlockBlob blockBlob = imagesBlobContainer.GetBlockBlobReference(file.FileName);

                    //  Before attempting to upload the image, check whether it actually IS an image
                    if (!IsImage(file)) {
                        //  Inform the user of the unsupported media type format
                        ViewBag.Message = "Unsupported file type. File upload aborted!";
                    }
                    else {

                        string error = String.Empty;

                        //  Open a Read / Input stream, and attempt to upload the image
                        using (Stream stream = file.InputStream) {
                            try {
                                await blockBlob.UploadFromStreamAsync(stream);
                            }
                            catch (Exception ex) {
                                error = ex.Message;
                            }
                        }

                        //  Inform the user of the succesfully uploaded file 
                        ViewBag.Message = "File Uploaded Successfully!";
                    }

                    #endregion

                    //  And return to the home / landing page
                    return RedirectToAction("Index", "Home");
                }

                //  Inform the user that an invalid file was selected
                else {
                    //  Inform the user of the succesfully uploaded file
                    ViewBag.Message = "Invalid file selected. File upload aborted!";
                }
            }

            //  In case the operation failed for any reason / unccaught exception, return to the View informaing the user as well
            catch {
                //  In case something goes wrong, inform the user that the uploading operation failed
                ViewBag.Message = "File upload failed!";
            }

            //  And return to the home / landing page
            return RedirectToAction("Index", "Home");
        }

        #endregion

        #region Helper methods

        /// <summary>
        /// Helper method denoting whether the provided as input file is an image
        /// </summary>
        public bool IsImage(HttpPostedFileBase file) {

            //  If the ContentType contains the word "image", return true
            if (file.ContentType.Contains("image")) {
                return true;
            }

            //  Initializing a string array holding all the acceptable image formats
            string[] acceptableImageFormatsArr = new string[] { ".jpg", ".png", ".gif", ".jpeg" };

            //  Checking whether the file name ends with either one of the above defined file formats, and returning that value
            bool isAcceptableFileFormat = acceptableImageFormatsArr.Any(item => file.FileName.EndsWith(item, StringComparison.OrdinalIgnoreCase));
            return isAcceptableFileFormat;
        }

        #endregion

    }
}