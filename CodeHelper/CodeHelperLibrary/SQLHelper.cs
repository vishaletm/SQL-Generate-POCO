using System;
using System.Collections.Generic;

using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using System.Data;
using System.Data.SqlClient;
using CodeHelperLibrary.Object;

namespace CodeHelperLibrary
{
  public  class SQLHelper
    {
        public List<SQLObject> Generate(string connection,string query,string name)
        {
            using (IDbConnection db = new SqlConnection("Data Source=.;Initial Catalog=MessageXpress Dispatch;Integrated Security=True;MultipleActiveResultSets=true"))
            {

                //return db.Query<SQLObject>
                //().ToList();
                // db.QueryMultiple
                int index = query.IndexOf("FROM ");
                if(index<0)
                    index = query.IndexOf("from ");
                if (index < 0)
                    index = query.IndexOf("From ");
                query = query.Insert(index, " INTO #temp ");
                var sql = @" IF OBJECT_ID('tempdb..#temp') IS NOT NULL
                                DROP TABLE #temp; "+ query+@" 
                                EXEC tempdb.dbo.sp_help N'#temp';
                                drop table #temp; ";
                using (var multi = db.QueryMultiple(sql, new { InvoiceID = 1 }))
                {
                    var sqlobject = multi.Read<SQLObject>();
                    var sqlobject2 = multi.Read<SQLObject>();
                     return sqlobject2.ToList();
                    ;
                   // var invoiceItems = multi.Read<InvoiceItem>().ToList();
                }


                return null;
            }
        }
    }
}
