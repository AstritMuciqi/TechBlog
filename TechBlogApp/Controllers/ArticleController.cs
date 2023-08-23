using Domain.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging.Abstractions;
using System.Security.Claims;
using TechBlogApp.Domain.Models;

namespace TechBlogApp.Controllers
{
    public class ArticleController : Controller
    {
        //static List<BlogEntry> Posts = new List<BlogEntry>();
        private readonly IArticleRepository _articleRepository;
        private readonly UserManager<AppUser> _userManager; // If using Identity


        public ArticleController(IArticleRepository articleRepository)
        {
            _articleRepository = articleRepository;
        }

        public IActionResult Index()
        {

            return View();
        }
        public IActionResult ArticleDetails(Guid id)
        {

            var article = _articleRepository.GetArticleById(id);

            return View(article); // Pass the model to the view
        }
        [Authorize(Roles = "Admin, Creator")]
        public IActionResult CreatorPage(Guid id)
        {
            ViewBag.Data = "Create an Article";

            if (id != Guid.Empty)
            {
                ViewBag.Data = "Edit an Article";
                Article existingArticle = _articleRepository.GetArticleById(id);

                return View(model: existingArticle);
            }

            return View();
        }
        [HttpPost]
        public async Task<IActionResult> AddComment(Comment commentModel, string Content)
        {

                // Get the currently logged-in user
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

                // Create a new Comment object and populate it with data
                var newComment = new Comment
                {
                    UserId = userId,
                    //Content = commentModel.Content,
                    Content = Content,
                    ArticleId = commentModel.ArticleId
                };

                _articleRepository.AddComment(newComment);



                // Add newComment to the database using your data access logic

                //return RedirectToAction("ArticleDetails", new { id = commentModel.ArticleId });
            string guidString = commentModel.ArticleId;
            Guid guid = Guid.Parse(guidString);
            // If the model state is not valid, return to the article details view with the validation errors
            var article = _articleRepository.GetArticleById(guid);

            //article.Comments.;
            return View("ArticleDetails", article);
        }


        [HttpPost]
        public async Task<IActionResult> CreatorPage(IFormFile imageFile, Article article)
        {
            //string userId = _userManager.GetUserId(User); // If using Identity
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);




            if (article.ArticleId == Guid.Empty)
            {
                // New article
                Article newArticle = new Article();
                newArticle.Content = article.Content;
                newArticle.ArticleId = Guid.NewGuid();
                newArticle.Title = article.Title;
                newArticle.DatePublished= DateTime.Now;
                newArticle.Tags = article.Tags;
                newArticle.AppUserId = userId;


                //newArticle.ImagePath = article.ImagePath;
                if (imageFile != null && imageFile.Length > 0)
                {
                    // Process the uploaded image here
                    var imagePath = Path.Combine("wwwroot", "images", imageFile.FileName);
                    using (var stream = new FileStream(imagePath, FileMode.Create))
                    {
                        imageFile.CopyTo(stream);
                    }
                }
                newArticle.ImagePath = imageFile.FileName;
                _articleRepository.AddArticle(newArticle);

            }
            else
            {
                // Existing article
                Article existingArticle = _articleRepository.GetArticleById(article.ArticleId);
                existingArticle.Content = article.Content;
                existingArticle.Title = article.Title;
                existingArticle.Tags = article.Tags;
                existingArticle.ImagePath = article.ImagePath;
                if (imageFile != null && imageFile.Length > 0)
                {
                    // Process the uploaded image here
                    var imagePath = Path.Combine("wwwroot", "images", imageFile.FileName);
                    using (var stream = new FileStream(imagePath, FileMode.Create))
                    {
                        imageFile.CopyTo(stream);
                    }
                    existingArticle.ImagePath = imageFile.FileName;
                }
                _articleRepository.UpdateArticle(existingArticle);
            }


            return RedirectToAction("Index");
        }
    }
}
