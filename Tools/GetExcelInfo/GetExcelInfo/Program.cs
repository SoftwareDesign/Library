using System;
using System.Linq;
using System.Data;
using System.Data.SqlClient;
namespace GetExcelInfo
{
    class Program
    {
        static void Main(string[] args)
        {
            readExcel();
            
            Console.ReadKey();
        }

        static void readExcel()
        {
            var book = new LinqToExcel.ExcelQueryFactory(@"C:\1.xlsx");
            var query =
                from row in book.Worksheet("owssvr")
                let columns = row["columns"].Cast<string>()
                select new
                {
                    BookNumber = row[0].Cast<string>(),
                    Title = row[1].Cast<string>(),
                    Description = row[2].Cast<string>(),
                    UserAndTeam = row[3].Cast<string>(),
                    BorrowedBy = row[4].Cast<string>(),
                    BorrowedDate = row[5].Cast<DateTime>(),
                    Publisher = row[7].Cast<string>(),
                    Supplier = row[8].Cast<string>(),
                    NetPrice = row[9].Cast<double>(),
                    PurchaseDate = row[10].Cast<DateTime>(),
                    RequestedBy = row[11].Cast<string>()
                };
            foreach (var item in query)
            {
                var bookNumber = new SqlParameter("@BookNumber", SqlDbType.NVarChar, int.MaxValue);
                if (item.BookNumber == null)
                {
                    continue;
                }

                bookNumber.Value = item.BookNumber;

                var title = new SqlParameter("@Title", SqlDbType.NVarChar, int.MaxValue)
                    {
                        Value = item.Title ?? string.Empty
                    };

                var description = new SqlParameter("@Description", SqlDbType.NVarChar, int.MaxValue)
                    {
                        Value = item.Description ?? string.Empty
                    };

                var userAndTeam = new SqlParameter("@UserAndTeam", SqlDbType.NVarChar, int.MaxValue)
                    {
                        Value = item.UserAndTeam ?? string.Empty
                    };

                var publisher = new SqlParameter("@Publisher", SqlDbType.NVarChar, int.MaxValue)
                    {
                        Value = item.Publisher ?? string.Empty
                    };

                var supplier = new SqlParameter("@Supplier", SqlDbType.NVarChar, int.MaxValue)
                    {
                        Value = item.Supplier ?? string.Empty
                    };

                var netPrice = new SqlParameter("@NetPrice", SqlDbType.Float) { Value = item.NetPrice };

                var purchaseDate = new SqlParameter("@PurchaseDate", SqlDbType.DateTime)
                    {
                        Value = item.PurchaseDate == DateTime.MinValue ? Convert.ToDateTime("1987/1/1") : item.PurchaseDate
                    };

                var requestedBy = new SqlParameter("@RequestedBy", SqlDbType.NVarChar, int.MaxValue)
                    {
                        Value = item.RequestedBy ?? string.Empty
                    };

                var borrowedBy = new SqlParameter("@BorrowBy", SqlDbType.NVarChar, int.MaxValue)
                    {
                        Value = item.BorrowedBy == null ? string.Empty : item.BorrowedBy.Trim()
                    };

                var borrowedDate = new SqlParameter("@BorrowedDate", SqlDbType.DateTime)
                    {
                        Value = item.BorrowedDate == DateTime.MinValue ? DateTime.Now : item.BorrowedDate
                    };

                OperateDatabase.UseProcedure(
                    "InitialDataBase",
                    bookNumber,
                    title,
                    description,
                    userAndTeam,
                    publisher,
                    supplier,
                    netPrice,
                    purchaseDate,
                    requestedBy,
                    borrowedBy,
                    borrowedDate);
                Console.WriteLine("Import Success!!!");
            }
        }
    }
}
