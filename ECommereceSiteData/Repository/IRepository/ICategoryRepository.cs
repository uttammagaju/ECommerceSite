﻿using ECommereceSiteModels.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommereceSiteData.Repository.IRepository
{
    public interface IApplictionUserRepository: IRepository<Category>
    {
        void Update(Category obj);
    }
}
