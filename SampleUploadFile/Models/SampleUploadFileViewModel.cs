using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SampleUploadFile.Models {

    public class SampleUploadFileViewModel {
        //      Properties
        public string           SelectedFileName        { get; set; }
        public string           Message                 { get; set; }
        public List<string>     UploadedBlobNamesList   { get; set; }

        //      Ctor
        public SampleUploadFileViewModel() {
            UploadedBlobNamesList = new List<string>();
        }
    }

}