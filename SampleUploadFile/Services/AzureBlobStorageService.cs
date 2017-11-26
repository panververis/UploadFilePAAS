using Microsoft.Azure;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace SampleUploadFile.Services {

    /// <summary>
    /// "Service" class aiming to provide "Common" API functionality for the associated with this project Azure Blob Storage
    /// </summary>
    public class AzureBlobStorageService {

        #region Properties

        public string               _blobStorageConnectionString    { get; private set; }
        public CloudStorageAccount  _blobStorageAccount             { get; private set; }
        public CloudBlobClient      _blobStorageClient              { get; private set; }

        #endregion

        #region Ctor

        /// <summary>
        /// Default / parameterless constructor.
        /// Also initializes the "_blobStorageConnectionString" and the "_blobStorageAccount"
        /// </summary>
        public AzureBlobStorageService() {
            _blobStorageConnectionString    = CloudConfigurationManager.GetSetting("StorageConnectionString");
            _blobStorageAccount             = CloudStorageAccount.Parse(_blobStorageConnectionString);
            _blobStorageClient              = _blobStorageAccount.CreateCloudBlobClient();
        }

        #endregion

        #region Methods

        /// <summary>
        /// Public method returning a blob container, or the "images"
        /// container by default, if a container name is not provided
        /// </summary>
        public CloudBlobContainer GetBlobContainer(string containerName = "images") {
            CloudBlobContainer imagesBlobContainer = _blobStorageClient.GetContainerReference("images");
            return imagesBlobContainer;
        }

        /// <summary>
        /// Public method returning an IEnumerable of IListBlobItems, contained within the
        /// container identified by name, as per the provided as input ContainerName. If no
        /// value is provided as input, the "images" container is accessed by default.
        /// Optional boolean prameter denoting whether the Blobs are to be returned in a
        /// hierarchical / folder-like structure, or simply in a flat (List-like) structure
        /// </summary>
        public IEnumerable<IListBlobItem> GetAllUploadedBlobs(
            string  containerName       = "images", 
            bool    useFlatBlobListing  = false) 
        {
            CloudBlobContainer          imagesBlobContainer = GetBlobContainer(containerName);
            IEnumerable<IListBlobItem>  uploadedBlobsList   = imagesBlobContainer.ListBlobs(null, useFlatBlobListing);
            return uploadedBlobsList;
        }

        /// <summary>
        /// Public async method uploading a file onto the appointed container.
        /// If no containerName is specified, it gets uploaded to the "images" folder
        /// </summary>
        public async Task<bool> UploadFile(HttpPostedFileBase file, string containerName = "images") {
            //  Guard clause, checking against a null or "empty" file
            if (file != null && file.ContentLength > 0) {
                //  Grabbing a reference to the appropriate container
                CloudBlobContainer imagesBlobContainer = GetBlobContainer(containerName);

                // Get the reference to the block blob from the container
                CloudBlockBlob blockBlob = imagesBlobContainer.GetBlockBlobReference(file.FileName);

                //  Open a Read / Input stream, and attempt to upload the image
                using (Stream stream = file.InputStream) {
                        await blockBlob.UploadFromStreamAsync(stream);
                }

                //  Lastly, if all went OK, return true
                return true;
            }
            else {
                throw new Exception("Invalid file selected. Upload aborted");
            }
        }

        /// <summary>
        /// Public method deleting the file denoted by the fileName provided as input.
        /// Returns a boolean denoting whether the delete operation succeeded.
        /// Optional parameter denotes from which repository the user wishes to delete the file from
        /// </summary>
        public bool DeleteFile(string fileName, string containerName = "images") {
            if (String.IsNullOrEmpty(fileName)) {
                throw new Exception("No file name has been provided for deletion");
            }

            //  Grab a reference to a previously created container
            CloudBlobContainer imagesBlobContainer = GetBlobContainer();

            // Get the reference to the block blob from the container
            CloudBlockBlob blockBlob = imagesBlobContainer.GetBlockBlobReference(fileName);

            //  Attempt to delete the file
            blockBlob.DeleteIfExists();

            //  Since everything seems to have gone OK, return true
            return true;
        }

        #endregion

    }
}