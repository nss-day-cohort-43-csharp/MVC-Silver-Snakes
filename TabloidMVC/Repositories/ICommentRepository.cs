﻿using TabloidMVC.Models;
using System.Collections.Generic;

namespace TabloidMVC.Repositories
{
    public interface ICommentRepository
    {
        List<Comment> getAllByPost(int id);
        Comment GetCommentById(int commentId);
        void Add(Comment comment);
        void UpdateComment(Comment comment);
        void DeleteComment(int id);
    }
}
