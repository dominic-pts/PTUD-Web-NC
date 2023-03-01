using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TatBlog.Core.Contracts
{
    public interface IPagedList
    {
        //Tong so trang(so tap con)
        int PageCount { get; }

        //Tong so phan tu tra ve tu truy van
        int TotalItemCount { get; }

        //Chi so trang hien tai (bat dau to 0)
        int PageIndex { get; }

        //Vi tri cua trang hien tai(bat dau tu 1)
        int PageNumber { get; }
        //So luong phan tu toi da tren 1 trang
        int PageSize { get; }

        //Kiem tra co trang truoc hay khong
        bool HasPreviousPage { get; }

        //Kiem tra co trang tiep theo hay khong
        bool HasNextPage { get; }

        //Trang hien tai co phai la trang dau tien khong
        bool IsFirstPage { get; }
        //Trang hien tai co phai la trang cuoi cung khong
        bool IsLastPage { get; }

        //Thu tu cua phan tu dau trang trong truy van (bat dau tu 1)
        int FirstItemIndex { get; }

        //Thu tu cua phan tu cuoi trang trong truy van (bat dau tu 1)
        int LastItemIndex { get; }
    }

    public interface IPagedList<out T> : IPagedList, IEnumerable<T>
    {
        //Lay phan tu tai vi tri index (bat dau tu 0)
        T this[int index] { get; }

        //Dem so luong phan tu chua trong trang
        int Count { get; }
    }
}
