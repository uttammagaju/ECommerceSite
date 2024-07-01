﻿using ECommereceSiteData.Repository.IRepository;
using ECommereceSiteModels.Models;
using ECommereceSiteData.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ECommereceSiteData.Repository
{
    public class OrderDetailRepository : Repository<OrderDetail>, IOrderDetailRepository
    {
        private ApplicationDbContext _db;
        public OrderDetailRepository(ApplicationDbContext db) : base(db) //: base(db): This part indicates that the CategoryRepository
        //class inherits from a base class and calls the base class constructor during object creation. The base(db) syntax passes
        //the received db argument to the base class constructor.
        {
            _db = db;
        }
        public void Update(OrderDetail obj)
        {
            _db.Update(obj);
           
        }
    }
}