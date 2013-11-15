using System.Globalization;
using System.Web.Mvc.Ajax;
using System.Web.Routing;

namespace MMLibrarySystem.ViewModels
{
    /// <summary>
    /// A simple factory to create user operations.
    /// </summary>
    public static class UserOperationFactory
    {
        public static UserOperation CreateBookDetailOperation(string title, string bookNumber)
        {
            return new UserOperation(title, "BookDetail", "BookList")
            {
                RouteValues = BuildRouteValues("bookNumber", bookNumber)
            };
        }

        public static UserOperation CreateBorrowOperation(long bookId)
        {
            return new UserOperation("Borrow", "Borrow", "BookList")
            {
                RouteValues = SetBookId(bookId),
                AjaxOptions = SetAjaxUpdateTarget("bookListContainer")
            };
        }

        public static UserOperation CreateCancelOperation(long bookId)
        {
            return new UserOperation("Cancel", "Cancel", "BookList")
            {
                RouteValues = SetBookId(bookId),
                AjaxOptions = SetAjaxUpdateTarget("bookListContainer")
            };
        }

        public static UserOperation CreateReturnOperation(long borrowId)
        {
            return new UserOperation("Return", "Return", "Admin")
            {
                Confirmation = "Are you sure to return this book?",
                RouteValues = SetBorrowId(borrowId)
            };
        }

        public static UserOperation CreateCheckOutOperation(long borrowId)
        {
            return new UserOperation("Check Out", "CheckOut", "Admin")
            {
                Confirmation = "Are you sure to check out this book?",
                RouteValues = SetBorrowId(borrowId)
            };
        }

        private static RouteValueDictionary SetBookId(long bookId)
        {
            return BuildRouteValues("bookId", IdToString(bookId));
        }

        private static RouteValueDictionary SetBorrowId(long borrowId)
        {
            return BuildRouteValues("borrowId", IdToString(borrowId));
        }

        private static RouteValueDictionary BuildRouteValues(string key, string value)
        {
            var routeValues = new RouteValueDictionary { { key, value } };
            return routeValues;
        }

        private static string IdToString(long id)
        {
            return id.ToString(CultureInfo.InvariantCulture);
        }

        private static AjaxOptions SetAjaxUpdateTarget(string targetId, InsertionMode mode = InsertionMode.Replace)
        {
            return new AjaxOptions
            {
                UpdateTargetId = targetId,
                HttpMethod = "GET",
                InsertionMode = mode
            };
        }
    }
}