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
        public string FileName      { get; set; }
        public string FileUri       { get; set; }
        public string DeleteMessage { get; set; }
        public bool   IsImage       { get; set; }

        //  Methods
        public void UpdateIsImageFlag() {
            IsImage = SampleUploadFileHelper.IsImage(FileName);
        }
    }

}