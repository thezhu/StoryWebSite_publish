using System;
using System.Collections.Generic;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Extensions;
using StoryWebSite.Models;
using StoryWebSite.ViewModels;
using StoryWebSite.Data;
using Microsoft.AspNetCore.Mvc.Rendering;//FOR SelectListItem
using System.IO;
using System.Web;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Authorization;


namespace StoryWebSite.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext context_;
        private const string sessionId_ = "SessionId";
        private readonly IHostingEnvironment hostingEnvironment_;
        private string webRootPath = null;
        private string filePath = null;

        public HomeController(ApplicationDbContext context, IHostingEnvironment hostingEnvironment)
        {
            context_ = context;
            hostingEnvironment_ = hostingEnvironment;
            webRootPath = hostingEnvironment_.WebRootPath;
            filePath = Path.Combine(webRootPath, "FileStorage");
        }


        public IActionResult Index()
        {
            var StoryList = context_.Story;
            return View(StoryList.ToList<Story>());
        }


        [Authorize]
        public ActionResult UserDetail()
        {
            System.Security.Claims.ClaimsPrincipal currentUser = this.User;
            var StoryList = from s in context_.Story
                            where (s.Publisher == currentUser.Identity.Name)
                            select s;
            return View(StoryList.ToList<Story>());
        }


        [Authorize(Roles = "Admin")]
        public IActionResult UserManage()
        {
            var stories = context_.Story;
            var orderedStories = stories.OrderBy(l => l.Publisher)
              .OrderBy(l => l.Title)
              .Select(l => l);
            return View(orderedStories);
        }

        [Authorize]
        [HttpGet]
        public IActionResult CreateStory(int id)
        {
            var model = new Story();
            ViewBag.isPublic = new List<SelectListItem>()
            {
                new SelectListItem(){ Value="Yes", Text = "Yes", Selected = true},
                new SelectListItem(){ Value="No", Text = "No"},
            };
            return View(model);
        }

        [Authorize]
        [HttpPost]
        public IActionResult CreateStory(int id, Story s)
        {
            System.Security.Claims.ClaimsPrincipal currentUser = this.User;
            s.Publisher = currentUser.Identity.Name;
            context_.Story.Add(s);
            context_.SaveChanges();
            return RedirectToAction("UserDetail");
        }

        [Authorize]
        [HttpGet]
        public IActionResult EditStory(int? id)
        {
            ViewBag.isPublic = new List<SelectListItem>()
            {
                new SelectListItem(){ Value="Yes", Text = "Yes", Selected = true},
                new SelectListItem(){ Value="No", Text = "No"},
            };

            if (id == null)
            {
                return StatusCode(Microsoft.AspNetCore.Http.StatusCodes.Status400BadRequest);
            }
            Story story = context_.Story.Find(id);
            if (story == null)
            {
                return StatusCode(StatusCodes.Status404NotFound);
            }
            

            var imageblocks = context_.ImageBlock.Where(l => l.Story == story);
            story.ImageBlocks = imageblocks.OrderBy(l => l.ImageBlockIndex).Select(l => l).ToList<ImageBlock>();

            if (story.ImageBlocks == null)
            {
                story.ImageBlocks = new List<ImageBlock>();
                ImageBlock im = new ImageBlock();
                im.ImageBlockIndex = 0;//must add this,or cannot modify
                im.ImageCaption = "none";
                im.ImageName = "none";
                im.ImagePath = "none";
                im.ImageDescription = "none";
                story.ImageBlocks.Add(im);
            }

            return View(story);
        }


        [Authorize]
        [HttpPost]
        public IActionResult EditStory(int? id, Story s)
        {
            if (id == null)
            {
                return StatusCode(StatusCodes.Status400BadRequest);
            }
            var st = context_.Story.Find(id);
            if (st != null)
            {
                st.Title = s.Title;
                st.isPublic = s.isPublic;
                
                var imageblocks = context_.ImageBlock.Where(l => l.Story == st);
                st.ImageBlocks = imageblocks.OrderBy(l => l.ImageBlockIndex).Select(l => l).ToList<ImageBlock>();

                if (st.ImageBlocks == null)
                {
                    st.ImageBlocks = new List<ImageBlock>();
                    ImageBlock im = new ImageBlock();
                    im.ImageBlockIndex = 0;//must add this,or cannot modify
                    im.ImageCaption = "none";
                    im.ImageName = "none";
                    im.ImagePath = "none";
                    im.ImageDescription = "none";
                    st.ImageBlocks.Add(im);
                }

                try
                {
                    context_.SaveChanges();
                }
                catch (Exception)
                {
                    return StatusCode(StatusCodes.Status400BadRequest);
                }
            }
            return RedirectToAction("UserDetail");
        }



        public ActionResult StoryDetail(int? id)
        {
            System.Security.Claims.ClaimsPrincipal currentUser = this.User;
            string currentUserName = this.User.Identity.Name;

            if (id == null)
            {
                return StatusCode(StatusCodes.Status400BadRequest);
            }
            Story story = context_.Story.Find(id);
            if (story == null)
            {
                return StatusCode(StatusCodes.Status404NotFound);
            }
            if (story.isPublic == "Yes" || currentUser.IsInRole("Admin") || currentUserName == story.Publisher)
            {

                var imageblocks = context_.ImageBlock.Where(l => l.Story == story);
                story.ImageBlocks = imageblocks.OrderBy(l => l.ImageBlockIndex).Select(l => l).ToList<ImageBlock>();
                if (story.ImageBlocks == null)
                {
                    story.ImageBlocks = new List<ImageBlock>();
                    ImageBlock im = new ImageBlock();
                    im.ImageBlockIndex = 0;//must add this,or cannot modify
                    im.ImageCaption = "none";
                    im.ImageName = "none";
                    im.ImagePath = "none";
                    im.ImageDescription = "none";
                    story.ImageBlocks.Add(im);
                }


                var comments = context_.Comment.Where(l => l.Story == story);
                story.Comments = comments.OrderBy(l => l.ReviewTime).Select(l => l).ToList<Comment>();
                if (story.Comments == null)
                {
                    story.Comments = new List<Comment>();
                    Comment co = new Comment();
                    co.Content = "none";
                    story.Comments.Add(co);
                }
            }
            else
            {
                return StatusCode(StatusCodes.Status400BadRequest);
            }
            return View(story);
        }

        [Authorize]
        public IActionResult DeleteStory(int? id)
        {
           //System.Security.Claims.ClaimsPrincipal currentUser = this.User;

            if (id == null)
            {
                return StatusCode(StatusCodes.Status400BadRequest);
            }

                var st = context_.Story.Find(id);
            try
            {
                if (st.ImageBlocks == null && st.Comments == null)
                {
                    context_.Remove(st);
                    context_.SaveChanges();
                }
            }
            catch {
                return RedirectToAction("UserDetail");
            }
           
            return RedirectToAction("UserDetail");
        }
        

        [Authorize]
        [HttpGet]
        public IActionResult AddComment(int id)
        {
            HttpContext.Session.SetInt32(sessionId_, id);

            Story story = context_.Story.Find(id);
            if (story == null)
            {
                return StatusCode(StatusCodes.Status404NotFound);
            }
            Comment co = new Comment();
            return View(co);
        }
        
        [Authorize]
        [HttpPost]
        public IActionResult AddComment(int? id, Comment co)
        {
            if (id == null)
            {
                return StatusCode(StatusCodes.Status400BadRequest);
            }

            int? storyId_ = HttpContext.Session.GetInt32(sessionId_);

            var story = context_.Story.Find(storyId_);
            if (story != null)
            {
                if (story.Comments == null)  
                {
                    List<Comment> cos = new List<Comment>();
                    story.Comments = cos;
                }

                System.Security.Claims.ClaimsPrincipal currentUser = this.User;
                co.Reviewer = currentUser.Identity.Name;

                story.Comments.Add(co);

                try
                {
                    context_.SaveChanges();
                }
                catch (Exception)
                {
                    return StatusCode(StatusCodes.Status404NotFound);
                }
            }
            return RedirectToAction("StoryDetail", new { id= storyId_ });
        }

        [Authorize]
        [HttpGet]
        public IActionResult EditComment(int? id)
        {
            if (id == null)
            {
                return StatusCode(StatusCodes.Status400BadRequest);
            }

            Comment comment = context_.Comment.Find(id);

            if (comment == null)
            {
                return StatusCode(StatusCodes.Status404NotFound);
            }
            return View(comment);
        }

        [Authorize]
        [HttpPost]
        public IActionResult EditComment(int? id, Comment co)
        {
            if (id == null)
            {
                return StatusCode(StatusCodes.Status400BadRequest);
            }
            Comment comment = context_.Comment.Find(id);

            if (comment != null)
            {
                comment.Content = co.Content;

                try
                {
                    context_.SaveChanges();
                }
                catch (Exception)
                {
                    return StatusCode(StatusCodes.Status400BadRequest);
                }
            }

            return RedirectToAction("Index");
        }

        [Authorize]
        public IActionResult DeleteComment(int? id)
        {
            if (id == null)
            {
                return StatusCode(StatusCodes.Status400BadRequest);
            }
            try
            {
                Comment comment = context_.Comment.Find(id);
                if (comment != null)
                {
                    context_.Remove(comment);
                    context_.SaveChanges();
                }
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status400BadRequest);
            }

            return RedirectToAction("Index");
        }

        
        [Authorize]
        [HttpGet]
        public IActionResult AddImageBlock(int id)
        {
            HttpContext.Session.SetInt32(sessionId_, id);

            Story story = context_.Story.Find(id);
            if (story == null)
            {
                return StatusCode(StatusCodes.Status404NotFound);
            }
            CreatePost im = new CreatePost();
            return View(im);
        }

        [Authorize]
        [HttpPost]
        public IActionResult AddImageBlock(int? id, CreatePost model)
        {
            if (id == null)
            {
                return StatusCode(StatusCodes.Status400BadRequest);
            }


            var post = new ImageBlock() { ImageCaption = model.ImageCaption };

                if (model.MyImage != null)
                {
                    var uniqueFileName = GetUniqueFileName(model.MyImage.FileName);
                    string extension = Path.GetExtension(model.MyImage.FileName).ToLower();
                    if (extension == ".png" || extension == ".jpg" || extension == ".jpeg")
                    {
                        var FilePath = Path.Combine(filePath, uniqueFileName);
                        model.MyImage.CopyTo(new FileStream(FilePath, FileMode.Create));

                        post.MyImage = GetByteArrayFromImage(model.MyImage);
                        post.ImageName = uniqueFileName;
                        post.ImagePath = FilePath;
                        post.ImageBlockIndex = model.ImageBlockIndex;
                        post.ImageDescription = model.ImageDescription;//must include every attribute

                    }
                    else
                    {
                        Response.WriteAsync("Accepted file format: .jpeg, .jpg");
                    }
                }

                int? storyId_ = HttpContext.Session.GetInt32(sessionId_);
                var story = context_.Story.Find(storyId_);
                if (story != null)
                {
                    if (story.ImageBlocks == null)  
                    {
                        List<ImageBlock> ims = new List<ImageBlock>();
                        story.ImageBlocks = ims;
                    }

                    story.ImageBlocks.Add(post);

                    try
                    {
                        context_.SaveChanges();
                    }
                    catch (Exception)
                    {
                        return StatusCode(StatusCodes.Status404NotFound);
                    }
                }

            return RedirectToAction("EditStory", new { id=storyId_ });
        }

        private byte[] GetByteArrayFromImage(IFormFile file)
        {
            using (var target = new MemoryStream())
            {
                file.CopyTo(target);
                return target.ToArray();
            }
        }

        private string GetUniqueFileName(string fileName)
        {
            fileName = Path.GetFileName(fileName);
            return Path.GetFileNameWithoutExtension(fileName)
                      + "_"
                      + Guid.NewGuid().ToString().Substring(0, 4)
                      + Path.GetExtension(fileName);
        }



        [Authorize]
        [HttpGet]
        public IActionResult EditImageBlock(int? id)
        {
            if (id == null)
            {
                return StatusCode(StatusCodes.Status400BadRequest);
            }

            ImageBlock imageblock = context_.ImageBlock.Find(id);

            if (imageblock == null)
            {
                return StatusCode(StatusCodes.Status404NotFound);
            }
            return View(imageblock);
        }

        [Authorize]
        [HttpPost]
        public IActionResult EditImageBlock(int? id, ImageBlock im)
        {
            if (id == null)
            {
                return StatusCode(StatusCodes.Status400BadRequest);
            }
            ImageBlock imageblock = context_.ImageBlock.Find(id);
            var storyid = imageblock.StoryId;
            if (imageblock != null)
            {

                imageblock.ImageCaption = im.ImageCaption;
                imageblock.ImageBlockIndex = im.ImageBlockIndex;
                imageblock.ImageDescription = im.ImageDescription;
                try
                {
                    context_.SaveChanges();
                }
                catch (Exception)
                {
                    return StatusCode(StatusCodes.Status400BadRequest);
                }
            }
            return RedirectToAction("EditStory", new { id=storyid });
        }


        [Authorize]
        public IActionResult DeleteImageBlock(int? id)
        {
            if (id == null)
            {
                return StatusCode(StatusCodes.Status400BadRequest);
            }

            ImageBlock imageblock = context_.ImageBlock.Find(id);
            var storyid = imageblock.StoryId;

            try
            {
                
                if (imageblock != null)
                {
                    context_.Remove(imageblock);
                    context_.SaveChanges();
                }
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status400BadRequest);
            }
            return RedirectToAction("EditStory", new { id=storyid });
        }




        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
