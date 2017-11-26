using Microsoft.Azure;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using SampleUploadFile.Models;
using SampleUploadFile.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace SampleUploadFile.Controllers {
    public class HomeController : Controller {

        #region Properties

        public AzureBlobStorageService _azureBlobStorageService { get; private set; }

        #endregion

        #region Ctor

        /// <summary>
        /// Default parameterless constructor.
        /// Also intializes the _azureBlobStorageService
        /// </summary>
        public HomeController() {
            _azureBlobStorageService = new AzureBlobStorageService();
        }

        #endregion

        #region Controller Actions

        //  "Index" Action
        public ActionResult Index() {

            //  Initializing the ViewModel
            SampleUploadFileViewModel viewModel = new SampleUploadFileViewModel();

            //  Checking whether any values have been passed in within the TempData dictionary.
            //  If so, assign them to the ViewModel
            ////  1) Message
            ////  2) Already selected file name
            string message = (TempData["Message"] as string);                   //  1)
            if (!String.IsNullOrEmpty(message)) {
                viewModel.Message = message;
            }
            string selectedFileName = (TempData["SelectedFile"] as string);     //  2)
            if (!String.IsNullOrEmpty(selectedFileName)) {
                viewModel.SelectedFileName = selectedFileName;
            }

            //  Retrieving a list of all the uploaded Blobs
            IEnumerable<IListBlobItem> uploadedBlobsList = _azureBlobStorageService.GetAllUploadedBlobs();

            //  If there already were any uploaded blobs, populate the ViewModel's List
            if (uploadedBlobsList != null && uploadedBlobsList.Any()) {
                viewModel.UploadedBlobFilesList = uploadedBlobsList.Select(x => new UploadedFileViewModel() { FileName = x.Uri.ToString().Split('/').Last(), FileUri = x.Uri.ToString() }).ToList();

                //  Populate the fancy little delete messages
                int count = 1;
                foreach (UploadedFileViewModel file in viewModel.UploadedBlobFilesList) {
                    file.DeleteMessage = GetDeleteMessage(count);
                    count++;
                }
            }

            return View(viewModel);
        }

        //  "Upload a file" action
        [HttpPost]
        public async Task<ActionResult> UploadFile(HttpPostedFileBase file) {

            //  First up surrounding everything in a try - catch block, to ensure safe execution
            try {
                // Checking whether a valid file of a size > 0 was selected
                if (file != null && file.ContentLength > 0) {

                    //  Initializing a "Blob Storage Account" instance, in order to retrieve the Blob Storage Connection String
                    string blobStorageConnectionString = String.Empty;
                    CloudStorageAccount blobStorageAccount = null;
                    try {
                        blobStorageConnectionString = CloudConfigurationManager.GetSetting("StorageConnectionString");
                        blobStorageAccount = CloudStorageAccount.Parse(blobStorageConnectionString);
                    } catch (Exception ex) {
                        TempData["Message"] = $" File upload failed. Reason: {ex.Message}";
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
                        TempData["Message"] = "Unsupported file type. File upload aborted!";
                    }
                    else {
                        //  Locally defined string variable to keep track of any uploading errors
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

                        //  Inform the user of the operation's outcome
                        if (!String.IsNullOrEmpty(error)) {
                            TempData["Message"] = $" File upload failed. Reason: {error}";
                        }
                        else {
                            TempData["Message"]         = $"File {file.FileName} Uploaded Successfully!";
                            TempData["SelectedFile"]    = file.FileName;
                        }
                    }

                    //  And return to the home / landing page
                    return RedirectToAction("Index", "Home");
                }

                //  Inform the user that an invalid file was selected
                else {
                    //  Inform the user of the file upload being aborted
                    TempData["Message"] = "Invalid file selected. File upload aborted!";
                }
            }

            //  In case the operation failed for any reason / unccaught exception, return to the View informaing the user as well
            catch {
                //  In case something goes wrong, inform the user that the uploading operation failed
                TempData["Message"] = "File upload failed!";
            }

            //  And return to the home / landing page
            return RedirectToAction("Index", "Home");
        }

        //  "Delete a file" action
        public ActionResult DeleteFile(string fileName) {
            //  First up surrounding everything in a try - catch block, to ensure safe execution
            try {
                //  In case something goes wrong, inform the user that the deleting operation failed
                if (String.IsNullOrEmpty(fileName)) {
                    TempData["Message"] = "File deletion failed!";
                }
                else {
                    //  Initializing a "Blob Storage Account" instance, in order to retrieve the Blob Storage Connection String
                    string blobStorageConnectionString = String.Empty;
                    CloudStorageAccount blobStorageAccount = null;
                    try {
                        blobStorageConnectionString = CloudConfigurationManager.GetSetting("StorageConnectionString");
                        blobStorageAccount = CloudStorageAccount.Parse(blobStorageConnectionString);
                    }
                    catch (Exception ex) {
                        TempData["Message"] = $" File deletion failed. Reason: {ex.Message}";
                    }

                    //  Creating a Blob Storage Client, to have access to Blob Containers
                    CloudBlobClient blobStorageClient = blobStorageAccount.CreateCloudBlobClient();

                    //  Grab a reference to a previously created container
                    CloudBlobContainer imagesBlobContainer = blobStorageClient.GetContainerReference("images");

                    // Get the reference to the block blob from the container
                    CloudBlockBlob blockBlob = imagesBlobContainer.GetBlockBlobReference(fileName);

                    //  Attempt to delete the file
                    blockBlob.DeleteIfExists();

                    TempData["Message"] = $"File {fileName} just crashed and burned";
                }
            }
            catch (Exception ex) {
                //  In case something goes wrong, inform the user that the uploading operation failed
                TempData["Message"] = $"File upload failed! Error: {ex.Message}";
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
        
        /// <summary>
        /// Helper function to get the fancy little delete messages
        /// </summary>
        private string GetDeleteMessage(int count) {
            if (count % 5 == 0) {
                return "Begone with this image!";
            }
            else if (count % 4 == 0) {
                return "Shoot it down";
            } else if (count % 3 == 0) {
                return "Land this baby";
            }
            else if (count % 2 == 0) {
                return "Delete it";
            }
            else {
                return "Re-activate gravity";
            }
        }

        #endregion

    }
}