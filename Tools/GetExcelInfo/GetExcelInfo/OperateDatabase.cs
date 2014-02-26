namespace GetExcelInfo
{
    using System.Data.SqlClient;
    using System.Data;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class OperateDatabase
    {
        /// <summary>
        /// Store the value of the connecting string.
        /// </summary>
        private static string constr = System.Configuration.ConfigurationManager.ConnectionStrings["ConDbStr"].ConnectionString;

        public static int UseProcedure(string sql, params SqlParameter[] param)
        {
            int res;
            using (var con = new SqlConnection(constr))
            {
                con.Open();
                using (var cmd = new SqlCommand())
                {
                    cmd.Connection = con;
                    cmd.CommandText = sql;
                    cmd.CommandType = CommandType.StoredProcedure;
                    if (param != null)
                    {
                        foreach (var p in param)
                        {
                            cmd.Parameters.Add(p);
                        }
                    }

                    res = cmd.ExecuteNonQuery();
                }
            }

            return res;
        }
    }
}
