using CoreBank.DataAccess.Repositories.Impl;
using CoreBank.Infrastructure;
using CoreBank.Infrastructure.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace CoreBank.DataAccess.Repositories.Repo
{
    public class UserRepository : BaseRepository<User>, IUserRepository
    {
        public UserRepository(CoreBankDbContext context) : base(context)
        {
        }



        
    }
}
