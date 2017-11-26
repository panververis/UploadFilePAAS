using Microsoft.Azure;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using System;
using System.Collections.Generic;
using System.Linq;
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

        #endregion

    }
}