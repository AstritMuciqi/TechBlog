using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TechBlogApp.Domain.Models
{
    public  class Comment
    {

        public Guid Id { get; set; }

        public string Content { get; set; }
        public string ArticleId { get; set; }
        public string AppUserId { get; set; }

        public DateTime DatePublished { get; set; }

        public int TimeDifference { get; set; }

        [ForeignKey("AppUserId")]
        public virtual AppUser User { get; set; }
    }
}
