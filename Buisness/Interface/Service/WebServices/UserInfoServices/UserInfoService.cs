using Buisness.Interface.Abstract.ApiAbstract;
using Buisness.Interface.Abstract.WebAbstract;
using DataAccess.Context;
using DataAccess;
using DataModel.UserContactViewModels;
using Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Buisness.Interface.Service.WebServices.UserInfoServices
{
    public class UserInfoService : IUserInfoService<UserInfoListModel>
    {
        UnitOfWork unitOfWork = new UnitOfWork(new ETicaretContext());
        public UserContact UserAdd(UserInfoListModel model)
        {
            if (model == null)
                throw new ArgumentNullException(nameof(model));
            var existData = unitOfWork.GetRepository<UserContact>().GetAll()
                .FirstOrDefault(x => x.Address == model.Address && x.City == model.City && x.Distirct == model.Distirct &&
                                     x.User.Name == model.FirstName && x.User.Surname == model.LastName &&
                                     x.User.Password == model.Password && x.User.EmailAddress == model.Email);
            if (existData == null)
            {
                var userInfo = new UserContact
                {
                    Address = model.Address,
                    City = model.City,
                    Distirct = model.Distirct,
                    IsDefaultAddress = model.IsDefaultAddress,
                    CreatedBy = model.Id,
                    ModifiedBy = model.Id,
                    CreatedDate = DateTime.Now,
                    ModifiedDate = DateTime.Now,
                    PhoneNumber = model.Phone,
                    Deleted = false,
                    Title = $"{model.FirstName} {model.LastName}",
                    UserId = model.Id,
                    IsActive = true,
                    User = new User
                    {
                        Id = model.Id,
                        Name = model.FirstName,
                        Surname = model.LastName,
                        Password = model.Password,
                        PhoneNumber = model.Phone,
                        Deleted = false,
                        IsActive = true,
                        UserGroupId = 3,
                        CreatedBy = model.Id,
                        ModifiedBy = model.Id,
                        CreatedDate = DateTime.Now,
                        ModifiedDate = DateTime.Now,
                        EmailAddress = model.Email,
                        LastLoginDate = DateTime.Now,
                    }
                };

                unitOfWork.GetRepository<UserContact>().Add(userInfo);
                unitOfWork.Complate();
                return userInfo;
            }
            return existData;
        }
        public List<UserInfoListModel> CheckRegistered(int userId)
        {
            var userInfo = unitOfWork.GetRepository<UserContact>().GetAll()
                .Where(x => x.UserId == userId)
                .Select(c => new UserInfoListModel
                {
                    Id = c.Id,
                    FirstName = c.User.Name,
                    LastName = c.User.Surname,
                    Address = c.Address,
                    City = c.City,
                    Password = c.User.Password,
                    Distirct = c.Distirct,
                    Email = c.User.EmailAddress,
                    IsDefaultAddress = c.IsDefaultAddress,
                    Phone = c.User.PhoneNumber,
                }).ToList();

            return userInfo;
        }
    }
}
