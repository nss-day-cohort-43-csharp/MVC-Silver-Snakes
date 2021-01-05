using TabloidMVC.Models;
using System.Collections.Generic;

namespace TabloidMVC.Repositories
{
    public interface ICommentRepository
    {
        List<Comment> getAllCommentsForPost(int id);
    }
}
