using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace StoryWebSite.ViewModels
{
    public class CreatePost
    {
        public string ImageCaption { get; set; }

        [Required]
        public int ImageBlockIndex { get; set; }
        public string ImageDescription { get; set; }

        [DisplayName("Upload File")]
        [Required(ErrorMessage = "Please choose an image to upload.")]
        public IFormFile MyImage { get; set; }

        [FileExtensions(Extensions = "jpg, jpeg, png, JPG, JPEG")]
        public string filename {
            get {
                if (MyImage != null) return MyImage.FileName;
                else return "";
            }
        }
    }
}
