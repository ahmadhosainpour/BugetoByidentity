﻿using EndPoint.Areas.Admin.ViewModel;

namespace Bugeto_store.Domain.Entities
{
    public class AddUserToRoleViewModel
    {
        public AddUserToRoleViewModel()
        {
            UserRoles = new List<UserRolesViewModel>();
        }

        public AddUserToRoleViewModel(string userId, List<UserRolesViewModel> userRoles)
        {
            UserId = userId;
            UserRoles = userRoles;
        }
        public string UserId { get; set; }
        public List<UserRolesViewModel> UserRoles { get; set; }


        public class UserRolesViewModel
        {
            public UserRolesViewModel()
            {
                
            }
            public UserRolesViewModel(string roleName)
            {
                RoleName = roleName;    
            }
            public string RoleName { get; set; }
            public bool IsSelected { get; set; }
        }


    }
}
