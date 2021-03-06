﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TabloidMVC.Models;

namespace TabloidMVC.Repositories
{
    public interface ITagRepository
    {
        List<Tag> GetTags();
        void Add(Tag tag);
        void Delete(int id);
        Tag GetTagById(int id);
        void UpdateTag(Tag tag);
    }
}
