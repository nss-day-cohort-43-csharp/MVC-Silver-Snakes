﻿using System.Collections.Generic;
using TabloidMVC.Models;

namespace TabloidMVC.Repositories
{
    public interface IUserProfileRepository
    {
        List<UserProfile> GetAll();
        UserProfile GetByEmail(string email);
        void Add(UserProfile user);

        UserProfile GetUserProfileById(int id);
    }
}