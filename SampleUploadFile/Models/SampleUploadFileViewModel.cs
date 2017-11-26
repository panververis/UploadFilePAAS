using SampleUploadFile.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SampleUploadFile.Models {

    public class SampleUploadFileViewModel {
        //      Properties
        public string                       SelectedFileName        { get; set; }
        public string                       Message                 { get; set; }
        public List<UploadedFileViewModel>  UploadedBlobFilesList   { get; set; }

        //      Ctor
        public SampleUploadFileViewModel() {
            UploadedBlobFilesList = new List<UploadedFileViewModel>();
        }
    }

    public class UploadedFileViewModel {

        //  Properties
        public string   FileName        { get; set; }
        public string   FileUri         { get; set; }
        public string   DeleteMessage   { get; set; }
        public FileType FileType        { get; set; }

        //  Methods
        /// <summary>
        /// Public object method updating the FileType, as per the file name extension
        /// </summary>
        public void UpdateFileTypeByFileName() {
            if (String.IsNullOrEmpty(FileName)) {
                FileType = FileType.Other;
            }

            //  Is it a zuperman?
            if (SampleUploadFileHelper.IsZuperman(FileName)) {
                FileType = FileType.Zuperman;
            }
            //  Is it an image?
            else if (SampleUploadFileHelper.IsImage(FileName)) {
                FileType = FileType.Image;
            }
            //  Is it a text?
            else if (SampleUploadFileHelper.IsText(FileName)) {
                FileType = FileType.Text;
            }
            //  Is it a pdf?
            else if (SampleUploadFileHelper.IsPdf(FileName)) {
                FileType = FileType.Pdf;
            }
            //  Is it an excel file?
            else if (SampleUploadFileHelper.IsExcel(FileName)) {
                FileType = FileType.Excel;
            }
            //  WHAT IS IT??
            else {
                FileType = FileType.Other;
            }
        }
    }

}