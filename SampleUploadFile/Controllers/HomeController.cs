using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SampleUploadFile.Controllers {
    public class HomeController : Controller {

        //  "Index" Action
        public ActionResult Index() {
            return View();
        }

        //  "Upload a file" action
        [HttpPost]
        public ActionResult UploadFile(HttpPostedFileBase File) {
            try {

                // Checking whether a valid file of a size > 0 was selected
                if (File.ContentLength > 0) {

                    //  Getting the Filename
                    string _FileName = "image.jpg"; //Path.GetFileName(File.FileName);

                    //  Combining the to-be-uploaded file's name with the pre-specified Server - side "Images" folder
                    string _path = Path.Combine(Server.MapPath("~/Content/Images"), _FileName);

                    //  Lastly, attempt to save (upload) the selected file
                    File.SaveAs(_path);
                }

                //  Inform the user of the succesfully uploaded file
                ViewBag.Message = "File Uploaded Successfully!!";

                //  And return to the home / landing page
                return RedirectToAction("Index", "Home");
            }
            catch {

                //  In case something goes wrong, inform the user that the uploading operation failed
                ViewBag.Message = "File upload failed!!";

                //  And return to the home / landing page
                return RedirectToAction("Index", "Home");
            }
        }
    }
}