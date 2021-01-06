using System.Collections.Generic;

namespace TabloidMVC.Models.ViewModels
{
    public class CommentIndexViewModel
    {
        public Post Post { get; set; }
        public List<Comment> Comments { get; set; }
    }
}
