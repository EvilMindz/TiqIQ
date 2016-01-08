using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.ServiceModel.Channels;
using System.ServiceModel.Web;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace QuizApp.Svc
{
    public class DataService: IDataService
    {
        public Message GetData()
        {
            DataTable dtSvc = PrepData();
            string result = JsonConvert.SerializeObject(dtSvc, new DataTableConverter());

            return WebOperationContext.Current.CreateTextResponse(result, "application/json;charset=utf-8", Encoding.UTF8); ;
        }

        public DataTable PrepData()
        {
            #region DataPrep
            //Prepare Table A
            DataTable dtA = new DataTable("A");

            //Add Columns to Table A
            DataColumn[] dtColsA = new DataColumn[] { new DataColumn("A", typeof(int)), new DataColumn("B", typeof(string)) };
            dtA.Columns.AddRange(dtColsA);

            //Add Rows to Table A
            for (int i = 0; i < 4; i++)
            {
                DataRow dRow = null;
                dRow = dtA.NewRow();

                dRow["A"] = i + 1;
                dRow["B"] = (char)(65 + i);

                dtA.Rows.Add(dRow);
            }

            //Prepare Table AOverride
            DataTable dtAOverride = new DataTable("AOverride");

            //Add Columns to Table AOverride
            DataColumn[] dtColsAOverride = new DataColumn[] { new DataColumn("A", typeof(int)), new DataColumn("B", typeof(string)) };            
            dtAOverride.Columns.AddRange(dtColsAOverride);

            //Add Rows to Table AOverride
            DataRow dRow2 = null;           
            dRow2 = dtAOverride.NewRow();
            dRow2["A"] = 3;
            dRow2["B"] = "D";
            dtAOverride.Rows.Add(dRow2);
            dRow2 = dtAOverride.NewRow();
            dRow2["A"] = 5;
            dRow2["B"] = "E";
            dtAOverride.Rows.Add(dRow2);
            #endregion DataPrep

            #region Data Implementation

            //Resultant Table
            DataTable dtFinal = new DataTable();
            DataColumn[] dtColsFinal = new DataColumn[] { new DataColumn("A", typeof(int)), new DataColumn("B", typeof(string)) };
            dtFinal.Columns.AddRange((dtColsFinal));

            //DefaultRow if no join match found
            var defaultRow = dtAOverride.NewRow();
            defaultRow["A"] = DBNull.Value;
            defaultRow["B"] = DBNull.Value;

            //Corresponding SQL Query = select x.A,coalesce(y.B,x.B) from A x left outer join AOverride y on y.A = x.A
            var final = dtFinal;
            var result = from x in dtA.AsEnumerable()
                join y in dtAOverride.AsEnumerable() on (int) x["A"] equals (int) y["A"]
                    into z
                from row in z.DefaultIfEmpty<DataRow>(defaultRow)
                //select new {A = x["A"], B = (row["B"] == DBNull.Value ? x["B"].ToString():row["B"].ToString())};
                select
                    final.LoadDataRow(new object[]
                    {x["A"], (row["B"] == DBNull.Value ? x["B"].ToString() : row["B"].ToString())},false);

            dtFinal = result.CopyToDataTable();
            #endregion Data Implementation

            return dtFinal;
        }
    }
}
