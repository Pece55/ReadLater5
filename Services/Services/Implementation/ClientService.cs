using Data;
using Entity.AuthEntity;
using Services.Interface;
using System;
using System.Linq;

namespace Services.Implementation
{
    public class ClientService : IClientService
    {
        private readonly ReadLaterDataContext _dbcontext;

        public ClientService(ReadLaterDataContext dbcontext)
        {
            _dbcontext = dbcontext;
        }

        public UserPermission GetClientbyUser(string user)
        {
            return _dbcontext.UserPermissions.FirstOrDefault(x => x.User == user);
        }
    }
}
