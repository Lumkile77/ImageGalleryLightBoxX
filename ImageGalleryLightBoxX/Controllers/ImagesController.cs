using System;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using ImageGalleryLightBoxX.Models;

namespace ImageGallerryLightBoxX.Controllers
{
    [Authorize]
    public class ImagesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Images
        public ActionResult Index()
        {
            var images = db.GalleryImages.Include(i => i.Category).OrderByDescending(i => i.UploadedDate);
            return View(images.ToList());
        }

        // GET: Images/Upload
        public ActionResult Upload()
        {
            ViewBag.CategoryId = new SelectList(db.Categories.Where(c => c.IsActive), "CategoryId", "Name");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Upload(GalleryImage model, HttpPostedFileBase file)
        {
            var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".webp" };

            if (file == null || file.ContentLength == 0)
            {
                ModelState.AddModelError("", "Please select an image to upload.");
            }
            else
            {
                string ext = Path.GetExtension(file.FileName).ToLower();

                if (!allowedExtensions.Contains(ext))
                {
                    ModelState.AddModelError("", "Only JPG, PNG and WEBP files are allowed.");
                }
                if (file.ContentLength > 5 * 1024 * 1024)
                {
                    ModelState.AddModelError("", "File size must be under 5MB.");
                }
            }

            // these are set by the server, not the form — remove them from validation
            ModelState.Remove("FileName");
            ModelState.Remove("UploadedDate");

            if (ModelState.IsValid)
            {
                string ext = Path.GetExtension(file.FileName).ToLower();
                string fileName = Guid.NewGuid().ToString() + ext;
                string folderPath = Server.MapPath("~/Content/Uploads");

                if (!Directory.Exists(folderPath))
                    Directory.CreateDirectory(folderPath);

                string savePath = Path.Combine(folderPath, fileName);
                file.SaveAs(savePath);

                model.FileName = fileName;
                model.UploadedDate = DateTime.Now;
                model.IsActive = true;

                db.GalleryImages.Add(model);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.CategoryId = new SelectList(db.Categories.Where(c => c.IsActive), "CategoryId", "Name", model.CategoryId);
            return View(model);
        }

        // GET: Images/Edit/5
        public ActionResult Edit(int id)
        {
            var image = db.GalleryImages.Find(id);
            if (image == null) return HttpNotFound();

            ViewBag.CategoryId = new SelectList(db.Categories.Where(c => c.IsActive), "CategoryId", "Name", image.CategoryId);
            return View(image);
        }

        // POST: Images/Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(GalleryImage model)
        {
            if (ModelState.IsValid)
            {
                db.Entry(model).State = EntityState.Modified;
                // never let the Edit form overwrite the stored file name
                db.Entry(model).Property(m => m.FileName).IsModified = false;
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.CategoryId = new SelectList(db.Categories.Where(c => c.IsActive), "CategoryId", "Name", model.CategoryId);
            return View(model);
        }

        // POST: Images/ToggleActive/5
        [HttpPost]
        public ActionResult ToggleActive(int id)
        {
            var image = db.GalleryImages.Find(id);
            if (image != null)
            {
                image.IsActive = !image.IsActive;
                db.SaveChanges();
            }
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing) db.Dispose();
            base.Dispose(disposing);
        }
    }
}