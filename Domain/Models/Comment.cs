using System;
using System.Collections.Generic;
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
        public string UserId { get; set; }

        public DateTime DatePublished { get; set; }

        public int TimeDifference { get; set; }
        //public virtual AppUser User { get; set; }
    }
}
