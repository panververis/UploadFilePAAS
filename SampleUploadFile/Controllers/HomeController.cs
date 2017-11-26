using Microsoft.WindowsAzure.Storage.Blob;
using SampleUploadFile.Models;
using SampleUploadFile.Services;
using System;
using System.Collections.Generic;
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
                viewModel.UploadedBlobFilesList = uploadedBlobsList
                                                        .Select(x => new UploadedFileViewModel() {
                                                            FileName = x.Uri.ToString().Split('/').Last(),
                                                            FileUri = x.Uri.ToString()
                                                        }).ToList();

                //  Also updating the "IsImage" flag
                viewModel.UploadedBlobFilesList.ForEach(x => x.UpdateFileTypeByFileName());

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
                await _azureBlobStorageService.UploadFile(file);
                TempData["Message"]         = $"File {file.FileName} Uploaded Successfully!";
                TempData["SelectedFile"]    = file.FileName;
            }
            //  In case the operation failed for any reason / uncaught exception, return to the View, informing the user as well about the operation failing
            catch (Exception ex) {
                //  In case something goes wrong, inform the user that the uploading operation failed
                TempData["Message"] = $"File upload failed! Reason: {ex.Message}";
            }

            //  And return to the home / landing page
            return RedirectToAction("Index", "Home");
        }

        //  "Delete a file" action
        public ActionResult DeleteFile(string fileName) {
            //  First up surrounding everything in a try - catch block, to ensure safe execution
            try {
                _azureBlobStorageService.DeleteFile(fileName);
                TempData["Message"] = $"File {fileName} just crashed and burned";
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
        /// Helper function to get the fancy little delete messages
        /// </summary>
        private string GetDeleteMessage(int count) {
            if (count % 5 == 0) {
                return "Begone with this file!";
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