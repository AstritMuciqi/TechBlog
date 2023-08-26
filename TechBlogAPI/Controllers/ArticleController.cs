using Domain.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Security.Claims;
using System.Xml.Linq;
using TechBlogApp.Domain.Models;
using TechBlogApp.Domain.ViewModels;
using TechBlogApp.Persistence;
using static System.Net.Mime.MediaTypeNames;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace TechBlogAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ArticleController : ControllerBase
    {
        private readonly IArticleRepository _articleRepository;
        private readonly ApplicationDbContext _context;
        private readonly UserManager<AppUser> userManager;


        public ArticleController(IArticleRepository articleRepository, ApplicationDbContext context, UserManager<AppUser> userManager)
        {

            _articleRepository = articleRepository;
            _context = context;
            this.userManager = userManager;
        }
        // GET: api/<ArticleController>
        [HttpGet]
        [Route("")]
        public async Task<IActionResult> Get(string? search = "")
        {
            var articles = _articleRepository.GetAllArticles(search);

            if (!string.IsNullOrEmpty(search))
            {
                articles = _articleRepository.GetAllArticles(search);
            }

            foreach (var article in articles)
            {
                var timeDifference = DateTime.Now - article.DatePublished;
                article.TimeDifference = (int)timeDifference.TotalMinutes;

            }

            return Ok(articles);
        }
        [HttpPost]
        [Route("AddComment")]
        public async Task<IActionResult> AddComment([FromBody] CommentVM comment)
        {

            // Get the currently logged-in user
            //var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (comment.AppUserId == null)
            {
                comment.AppUserId = "123";
            }

            // Create a new Comment object and populate it with data
            var newComment = new Comment
            {
                AppUserId = comment.AppUserId,
                //Content = commentModel.Content,
                Content = comment.Content,
                ArticleId = comment.ArticleId,
                DatePublished = DateTime.Now,
            };

            _articleRepository.AddComment(newComment);

            //var result =  await _context.SaveChangesAsync() > 0;


            return Ok(new { message = "Comment added successfully!", newComment });

            //return BadRequest(new ProblemDetails { Title = "Problem with creating new comments!" });

        }

        [HttpGet]
        [Route("GetComments")]
        public async Task<ActionResult> GetComments(Guid id)
        {
            string idArticle = id.ToString();
            // If the model state is not valid, return to the article details view with the validation errors

            var comments = _articleRepository.GetCommentsArticleById(idArticle);

            foreach (var comment in comments)
            {
                var timeDifference = DateTime.Now - comment.DatePublished;
                comment.TimeDifference = (int)timeDifference.TotalMinutes;

            }

            return Ok(new { message = "Comment load successfully!", comments });
        }


        // PUT api/<ValuesController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<ValuesController>/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Article>> Delete(Guid id)
        {
 
            _articleRepository.DeleteArticle(id);

            return Ok();

        }
        [HttpDelete("DeleteComment/{id}")]
        public async Task<ActionResult<Comment>> DeleteComment(Guid id)
        {

            _articleRepository.DeleteComment(id);

            return Ok();

        }

        [HttpPost("changeUserRole/{userId}/{oldRoleName}/{newRoleName}")]
        public async Task<ActionResult> ChangeUserRole(string userId, string oldRoleName, string newRoleName)
        {
            var user = await userManager.FindByIdAsync(userId);
            if (user != null)
            {

                await userManager.RemoveFromRoleAsync(user, oldRoleName);
                await userManager.AddToRoleAsync(user, newRoleName);

                var result = await _context.SaveChangesAsync() > 0;
                if (result) return Ok(new { message = "UserRole updated" });
            }
            else
            {
                return BadRequest(new ProblemDetails { Title = "Cannot find this user!" });
            }
            return Ok();
        }

        [HttpDelete("user/{id}")]
        public async Task<ActionResult> DeleteUser(string id)
        {
            var user = await _context.Users.FindAsync(id);

            if (user == null)
            {
                return BadRequest(new ProblemDetails { Title = "Can't find this user!" });
            }
            else
            {
                await userManager.DeleteAsync(user);

                var result = await _context.SaveChangesAsync() > 0;

                if (result) return Ok(new { message = "User Deleted Successful" });
            }
            return Ok(new { message = "User Deleted Successful" });

        }
    }
}
