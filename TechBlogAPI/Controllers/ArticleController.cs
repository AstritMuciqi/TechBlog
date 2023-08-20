using Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TechBlogApp.Domain.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace TechBlogAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ArticleController : ControllerBase
    {
        private readonly IArticleRepository _articleRepository;

        public ArticleController(IArticleRepository articleRepository)
        {

            _articleRepository = articleRepository;
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

        // GET api/<ValuesController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<ValuesController>
        [HttpPost]
        public void Post([FromBody] string value)
        {
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
