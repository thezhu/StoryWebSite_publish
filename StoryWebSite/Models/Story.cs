using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace StoryWebSite.Models
{
    public class Story
    {
        public Story()
        {
            CreateTime = DateTime.Now;
        }

        public int StoryId { get; set; }

        [Required]
        public string Title { get; set; }
        public string Publisher { get; set; }
        public DateTime CreateTime { get; set; }
        public string isPublic { get; set; }

        public ICollection<ImageBlock> ImageBlocks { get; set; }
        public ICollection<Comment> Comments { get; set; }
    }
    

    public class ImageBlock
    {
        public int ImageBlockId { get; set; }
        public int ImageBlockIndex { get; set; }
        public string ImageCaption { get; set; }
        public string ImageName { get; set; }
        public string ImagePath { get; set; }
        public byte[] MyImage { get; set; }
        
        public string ImageDescription { get; set; }

        public int? StoryId { get; set; }
        public Story Story { get; set; }
    }

    public class Comment
    {
        public Comment()
        {
            ReviewTime = DateTime.Now;
        }


        public int CommentId { get; set; }
        public string Reviewer { get; set; }
        public DateTime ReviewTime { get; set; }
        public string Content { get; set; }

        public int? StoryId { get; set; }
        public Story Story { get; set; }
    }
}
