using Bugeto_store.Domain.Entities.User;
using EndPoint.Areas.Admin.ViewModel;

public class UserInRole
    {
        public int Id { get; set; }
        public virtual User user { get; set; }
        public int UserId { get; set; }
        public virtual Role role { get; set; }
        public int RoleId { get; set; }
    }

