using TabloidMVC.Models;
using System.Collections.Generic;

namespace TabloidMVC.Repositories
{
    public interface ICommentRepository
    {
        List<Comment> getAllByPost(int id);
        void Add(Comment comment);
        Comment GetCommentById(int id, int userProfileId);

    }
}
