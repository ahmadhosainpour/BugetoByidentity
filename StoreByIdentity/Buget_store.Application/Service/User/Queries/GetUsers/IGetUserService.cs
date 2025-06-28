using Microsoft.EntityFrameworkCore.Metadata;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Buget_store.Application.Service.User.Queries.GetUsers
{
    public interface IGetUserService
    {

        ResultGetUserDto Execute(RequestGetUserDto request);//search users
    }
}
 