using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Buget_store.Application.Service.User.Queries.GetUsers
{
    public class GetUsersDto
    {
        public int ID { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
    }
}
