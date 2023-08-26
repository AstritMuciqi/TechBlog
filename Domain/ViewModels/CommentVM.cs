using System.ComponentModel.DataAnnotations;

namespace TechBlogApp.Domain.ViewModels
{
    public class CommentVM
    {
        public Guid Id { get; set; }

        public string Content { get; set; }
        public string ArticleId { get; set; }
        public string AppUserId { get; set; }

        public DateTime DatePublished { get; set; }

        public int TimeDifference { get; set; }
    }
}
