using DataModel.BaseModel;

namespace DataModel.UserContactViewModels
{
    public class UserInfoListModel : EntityBase
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string City { get; set; }
        public string Distirct { get; set; }
        public string Address { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public bool IsDefaultAddress { get; set; }
    }
}
