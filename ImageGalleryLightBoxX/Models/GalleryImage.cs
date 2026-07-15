using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;


namespace ImageGalleryLightBoxX.Models
{
    // Models/GalleryImage.cs
    public class GalleryImage
    {
        public int GalleryImageId { get; set; }

        [Required, StringLength(150)]
        public string Title { get; set; }

        [StringLength(500)]
        public string Description { get; set; }

        [Required]
        public string FileName { get; set; }   // stored file name on disk

        public DateTime UploadedDate { get; set; } = DateTime.Now;

        public bool IsActive { get; set; } = true;   // soft delete flag

        [Required]
        public int CategoryId { get; set; }
        public virtual Category Category { get; set; }
    }
}