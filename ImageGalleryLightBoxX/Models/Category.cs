using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ImageGalleryLightBoxX.Models

{
    // Models/Category.cs
    public class Category
    {
        public int CategoryId { get; set; }

        [Required, StringLength(100)]
        public string Name { get; set; }

        public bool IsActive { get; set; } = true;

        public virtual ICollection<GalleryImage> Images { get; set; }
    }
}