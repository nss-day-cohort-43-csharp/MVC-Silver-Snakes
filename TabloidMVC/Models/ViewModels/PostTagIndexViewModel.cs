using System.Collections.Generic;

namespace TabloidMVC.Models.ViewModels
{
    public class PostTagIndexViewModel
    {
        public Post Post { get; set; }
        public List<PostTag> PostTags { get; set; }
        public List<Tag> Tags { get; set; }
    }
}