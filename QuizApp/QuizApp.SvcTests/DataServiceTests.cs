using Microsoft.VisualStudio.TestTools.UnitTesting;
using QuizApp.Svc;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuizApp.Data.Tests
{
    [TestClass()]
    public class DataServiceTests
    {
        [TestMethod()]
        public void GetDataTest()
        {
            #region DataPrep
            DataTable dtExpected = new DataTable();
            DataColumn[] dtColsExpected = new DataColumn[] { new DataColumn("A", typeof(int)), new DataColumn("B", typeof(string)) };
            dtExpected.Columns.AddRange(dtColsExpected);

            DataRow dRow= dtExpected.NewRow();
            dRow["A"] = 1;
            dRow["B"] = "A";
            dtExpected.Rows.Add(dRow);

            DataRow dRow2 = dtExpected.NewRow();
            dRow2["A"] = 2;
            dRow2["B"] = "B";
            dtExpected.Rows.Add(dRow2);

            DataRow dRow3 = dtExpected.NewRow();
            dRow3["A"] = 3;
            dRow3["B"] = "D";
            dtExpected.Rows.Add(dRow3);

            DataRow dRow4 = dtExpected.NewRow();
            dRow4["A"] = 4;
            dRow4["B"] = "D";
            dtExpected.Rows.Add(dRow4);


            #endregion DataPrep

            DataService ds = new DataService();

            var dtActual = ds.PrepData();
            
            Assert.IsTrue(AreDataTablesSame(dtExpected,dtActual),"Something went wrong in the GetData Service");
        }

        private bool AreDataTablesSame(DataTable t1, DataTable t2)
        {
            if (t1?.Rows.Count != t2?.Rows.Count)
                return false;

            if (t1 != null && t1.Columns.Count != t2.Columns.Count)
                return false;

            if (t1 != null && t1.Columns.Cast<DataColumn>().Any(a => !t2.Columns.Contains(a.ColumnName)))
            {
                return false;
            }

            Debug.Assert(t1 != null, "t1 != null");
            for (int i = 0; i <= t1.Rows.Count - 1; i++)
            {
                if (t1.Columns.Cast<DataColumn>().Any(x => t1.Rows[i][x.ColumnName].ToString() != t2.Rows[i][x.ColumnName].ToString()))
                {
                    return false;
                }
            }

            return true;
        }
    }
}