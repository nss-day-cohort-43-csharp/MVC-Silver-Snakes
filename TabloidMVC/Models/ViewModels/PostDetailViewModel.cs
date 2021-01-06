using System.Collections.Generic;

namespace TabloidMVC.Models.ViewModels
{
    public class PostDetailViewModel
    {
        public Post Post { get; set; }
        public List<PostTag> PostTags { get; set; }
    }
}