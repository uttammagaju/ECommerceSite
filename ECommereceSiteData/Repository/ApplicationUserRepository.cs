using ECommereceSiteData.Repository.IRepository;
using ECommereceSiteModels.Models;
using ECommereceSiteData.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using ECommereceSiteModels;

namespace ECommereceSiteData.Repository
{
    public class ApplicationUserRepository : Repository<ApplicationUser>, IApplicationUserRepository
    {
        private ApplicationDbContext _db;
        public ApplicationUserRepository(ApplicationDbContext db) : base(db) //: base(db): This part indicates that the CategoryRepository
        //class inherits from a base class and calls the base class constructor during object creation. The base(db) syntax passes
        //the received db argument to the base class constructor.
        {
            _db = db;
        }
        
    }
}
