using System;
using System.Web;

namespace Edelweiss.Utils.Web.Controls
{
    public static class GridViewColumnSort
    {
        public static String ReturnSortDirection(String field)
        {
            if (HttpContext.Current.Cache[field + "_sortDirection"] == null)
            {
                HttpContext.Current.Cache[field + "_sortDirection"] = "ASC";
            }
            else
            {
                if (HttpContext.Current.Cache[field + "_sortDirection"].ToString() == "ASC")
                {
                    HttpContext.Current.Cache[field + "_sortDirection"] = "DESC";
                    return HttpContext.Current.Cache[field + "_sortDirection"].ToString();
                }
                if (HttpContext.Current.Cache[field + "_sortDirection"].ToString() == "DESC")
                {
                    HttpContext.Current.Cache[field + "_sortDirection"] = "ASC";
                    return HttpContext.Current.Cache[field + "_sortDirection"].ToString();
                }
            }

            return HttpContext.Current.Cache[field + "_sortDirection"].ToString();
        }
    }
}
