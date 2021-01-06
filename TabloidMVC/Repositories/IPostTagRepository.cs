using System.Collections.Generic;
using TabloidMVC.Models;

namespace TabloidMVC.Repositories
{
    public interface IPostTagRepository
    {
        List<PostTag> GetPostTagsbyPostId(int id);
        void AddTag(PostTag postTag);
        void DeleteTag(int id);
        List<PostTag> GetAll();
        PostTag GetPostTagbyPostIdAndTagId(int postId, int tagId);
    }
}