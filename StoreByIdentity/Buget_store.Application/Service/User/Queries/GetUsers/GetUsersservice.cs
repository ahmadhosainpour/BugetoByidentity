using Buget_store.Application.Interface.Context;
using Bugeto_store.Common;

namespace Buget_store.Application.Service.User.Queries.GetUsers
{
    public class GetUsersservice : IGetUserService
    {
        private readonly IDatabaseContext _context;                         //connect to database 
        public GetUsersservice(IDatabaseContext context)
        {
            _context = context;
        }

        public ResultGetUserDto Execute(RequestGetUserDto request)
        {
            var Users = _context.Users.AsQueryable();
            if (!string.IsNullOrWhiteSpace(request.SearchKey))
            {
                Users = Users.Where(p => p.FullName.Contains(request.SearchKey) && p.Email.Contains(request.SearchKey));
            }

            int rowscount = 0;
            var userList = Users.ToPaged(request.Page, 20, out rowscount).Select(p => new GetUsersDto
            {
                Email = p.Email,
                FullName = p.FullName,
                ID = p.ID

            }).ToList();

            return new ResultGetUserDto
            {
                Rows = rowscount,
                Users = userList
            };

        }


    }
}