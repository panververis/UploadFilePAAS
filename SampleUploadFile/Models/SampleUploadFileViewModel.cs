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
        public string FileName { get; set; }
        public string FileUri  { get; set; }
    }

}