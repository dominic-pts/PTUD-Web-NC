using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TatBlog.Core.Contracts
{
    public interface IPagingParams
    {
        //So mau tin tren 1 trang
        int PageSize { get; set; }
        //So trang tinh bat dau tu 1
        int PageNumber { get; set; }

        //Ten cot muon sap xe
        string SortColumn { get; set; }

        //Thu tu xep: tang hay giam
        string SortOrder { get; set; }
    }
}
