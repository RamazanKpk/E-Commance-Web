using DataAccess;
using DataAccess.Context;
using Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;

namespace Buisness.Interface.Service.WebServices.CustomRoleProvider
{
    public class CustomRoleProvider : RoleProvider
    {
        UnitOfWork unitOfWork = new UnitOfWork(new ETicaretContext());
        public override string ApplicationName { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public override void AddUsersToRoles(string[] usernames, string[] roleNames)
        {
            throw new NotImplementedException();
        }

        public override void CreateRole(string roleName)
        {
            throw new NotImplementedException();
        }

        public override bool DeleteRole(string roleName, bool throwOnPopulatedRole)
        {
            throw new NotImplementedException();
        }

        public override string[] FindUsersInRole(string roleName, string usernameToMatch)
        {
            throw new NotImplementedException();
        }

        public override string[] GetAllRoles()
        {
            throw new NotImplementedException();
        }

        public override string[] GetRolesForUser(string username)
        {
            var userList = unitOfWork.GetRepository<User>().GetAll();
            var user = userList.FirstOrDefault(x=>x.Name == username);

            if (user != null)
            {
                if (user.UserGroupId == 3)
                {
                    return new string[] { "Admin" };
                }
                //diğer roller de eklenebilir
            }
            return new string[] { };
        }

        public override string[] GetUsersInRole(string roleName)
        {
            throw new NotImplementedException();
        }

        public override bool IsUserInRole(string username, string roleName)
        {
            var userList = unitOfWork.GetRepository<User>().GetAll();
            var user = userList.FirstOrDefault(x => x.Name == username);

            if (user != null)
            {
                if (roleName == user.UserGroup.Title && user.UserGroupId ==3)
                {
                    return true;
                }
            }
            return false;
        }

        public override void RemoveUsersFromRoles(string[] usernames, string[] roleNames)
        {
            throw new NotImplementedException();
        }

        public override bool RoleExists(string roleName)
        {
            throw new NotImplementedException();
        }
    }
}