using Microsoft.Azure;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SampleUploadFile.Controllers {
    public class HomeController : Controller {

        //  "Index" Action
        public ActionResult Index() {
            return View();
        }

        //  "Upload a file" action
        [HttpPost]
        public ActionResult UploadFile(HttpPostedFileBase File) {

            //  First up surrounding everything in a try - catch block, to ensure safe execution
            try {

                // Checking whether a valid file of a size > 0 was selected
                if (File.ContentLength > 0) {

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

                    //  Creating a BLob Storage Client, to have access to Blob Containers
                    CloudBlobClient blobStorageClient = blobStorageAccount.CreateCloudBlobClient();

                    //  Grab a reference to a previously created container
                    CloudBlobContainer imagesBlobContainer = blobStorageClient.GetContainerReference("images");



                    #endregion

                    //  Inform the user of the succesfully uploaded file 
                    ViewBag.Message = "File Uploaded Successfully!";

                    //  And return to the home / landing page
                    return RedirectToAction("Index", "Home");

                }

                //  Inform the user that an invalid file was selected
                else {
                    //  Inform the user of the succesfully uploaded file
                    ViewBag.Message = "Invalid file selected. File upload aborted!";

                    //  And return to the home / landing page
                    return RedirectToAction("Index", "Home");
                }
            }

            //  In case the operation failed for any reason / unccaught exception, return to the View informaing the user as well
            catch {
                //  In case something goes wrong, inform the user that the uploading operation failed
                ViewBag.Message = "File upload failed!";

                //  And return to the home / landing page
                return RedirectToAction("Index", "Home");
            }
        }
    }
}