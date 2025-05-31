using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bugeto_store.Common
{
    public static class pageination
    {

        public static IEnumerable<TSource> ToPaged<TSource>(this IEnumerable<TSource> source, int page, int PageSize, out int rowCount)//tsource  --->Query from database
        {

            rowCount = source.Count();
            return source.Skip((page - 1) * PageSize).Take(PageSize);

        }
         
    }

}
