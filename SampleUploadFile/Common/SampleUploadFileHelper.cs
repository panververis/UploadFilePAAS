using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SampleUploadFile.Common {
    public static class SampleUploadFileHelper {

        /// <summary>
        /// Helper method denoting whether the provided as input fileName is referring to an image
        /// </summary>
        public static bool IsImage(string fileName) {
            //  Initially checking whether the file name is not null or empty
            if (String.IsNullOrEmpty(fileName)) {
                return false;
            }

            //  Initializing a string array holding all the acceptable image formats
            string[] acceptableImageFormatsArr = new string[] { ".jpg", ".png", ".gif", ".jpeg" };

            //  Checking whether the file name ends with either one of the above defined file formats, and returning that value
            bool isAcceptableImageFormat = acceptableImageFormatsArr.Any(item => fileName.EndsWith(item, StringComparison.OrdinalIgnoreCase));
            return isAcceptableImageFormat;
        }
    }
}