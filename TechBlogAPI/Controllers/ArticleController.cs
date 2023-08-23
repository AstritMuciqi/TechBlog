using Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Security.Claims;
using TechBlogApp.Domain.Models;
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


        public ArticleController(IArticleRepository articleRepository, ApplicationDbContext context)
        {

            _articleRepository = articleRepository;
            _context = context;
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
        public async Task<IActionResult> AddComment([FromBody] Comment comment)
        {

            // Get the currently logged-in user
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (userId == null)
            {
                userId = "123";
            }

            // Create a new Comment object and populate it with data
            var newComment = new Comment
            {
                UserId = userId,
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
    }
}
