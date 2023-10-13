using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Bakery_Shop.Models
{
	public class ImageStore
	{
        [Key]
        public int ImageID { get; set; }

        public string Title { get; set; }

        public string Image { get; set; }
        [NotMapped]
        public HttpPostedFileBase ImageFile { get; set; }
    }
}