﻿using System.Collections.Generic;
using TabloidMVC.Models;

namespace TabloidMVC.Repositories
{
    public interface IUserProfileRepository
    {
        List<UserProfile> GetAll();

        UserProfile GetByEmail(string email);

        void Add(UserProfile user);

        void DeactivateUserProfile(int id);

        UserProfile GetUserProfileById(int id);
    }
}