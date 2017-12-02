using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SampleUploadFile.Common {

    #region (Public) Enums

    /// <summary>
    /// Public enum, denoting the File Type
    /// </summary>
    public enum FileType {
        Image       = 1,
        Text        = 2,
        Pdf         = 3,
        Excel       = 4,
        Zuperman    = 5,
        Other       = 6
    }

    #endregion

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

        /// <summary>
        /// Helper method denoting whether the provided as input fileName is referring to a text document
        /// </summary>
        public static bool IsText(string fileName) {
            //  Initially checking whether the file name is not null or empty
            if (String.IsNullOrEmpty(fileName)) {
                return false;
            }

            //  Initializing a string array holding all the acceptable text formats
            string[] acceptableTextFormatsArr = new string[] { ".doc", ".dot", ".docx", ".docm", ".odt", ".txt", ".rtf", ".xml" };

            //  Checking whether the file name ends with either one of the above defined file formats, and returning that value
            bool isAcceptableTextFormat = acceptableTextFormatsArr.Any(item => fileName.EndsWith(item, StringComparison.OrdinalIgnoreCase));
            return isAcceptableTextFormat;
        }

        /// <summary>
        /// Helper method denoting whether the provided as input fileName is referring to a pdf document
        /// </summary>
        public static bool IsPdf(string fileName) {
            //  Initially checking whether the file name is not null or empty
            if (String.IsNullOrEmpty(fileName)) {
                return false;
            }

            //  Initializing a string array holding all the acceptable pdf formats
            string[] acceptablePdfFormatsArr = new string[] { ".pdf" };

            //  Checking whether the file name ends with either one of the above defined file formats, and returning that value
            bool isAcceptablePdfFormat = acceptablePdfFormatsArr.Any(item => fileName.EndsWith(item, StringComparison.OrdinalIgnoreCase));
            return isAcceptablePdfFormat;
        }

        /// <summary>
        /// Helper method denoting whether the provided as input fileName is referring to an excel document
        /// </summary>
        public static bool IsExcel(string fileName) {
            //  Initially checking whether the file name is not null or empty
            if (String.IsNullOrEmpty(fileName)) {
                return false;
            }

            //  Initializing a string array holding all the acceptable excel formats
            string[] acceptableExcelFormatsArr = new string[] { ".xlsx", ".xlsm", ".xlsb", ".xltx", ".xltm", ".xls", ".xlt", ".xlt" };

            //  Checking whether the file name ends with either one of the above defined file formats, and returning that value
            bool isAcceptableExcelFormat = acceptableExcelFormatsArr.Any(item => fileName.EndsWith(item, StringComparison.OrdinalIgnoreCase));
            return isAcceptableExcelFormat;
        }

        /// <summary>
        /// Helper method denoting whether the provided as input fileName is referring to a ZUPERMAN file
        /// </summary>
        public static bool IsZuperman(string fileName) {
            //  Initially checking whether the file name is not null or empty
            if (String.IsNullOrEmpty(fileName)) {
                return false;
            }

            //  Initializing a string array holding all the acceptable excel formats
            string[] acceptableZupermanFormatsArr = new string[] { "zuperman" };

            //  Checking whether the file name ends with either one of the above defined file formats, and returning that value
            bool isAcceptableZupermanFormat = acceptableZupermanFormatsArr.Any(item => fileName.Contains(item));
            return isAcceptableZupermanFormat;
        }
    }
}