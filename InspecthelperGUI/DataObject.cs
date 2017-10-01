using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InspecthelperGUI
{
    public class DataObject
    {
        public static DataTable DataTableDefinitionFromTextFile(string loc,char delimit = ',')
        {
            DataTable Result;
            string[] LineArray = File.ReadAllLines(loc);
            Result = FormDataTable(LineArray, delimit);
            return Result;
        }
        public static DataTable FormDataTable(string[] LineArray,char delimit)
        {
            DataTable dt = new DataTable();
            AddColumnToTable(LineArray, delimit, ref dt);
            AddRowToTable(LineArray, delimit, ref dt);
            return dt;
        }
        private static void AddRowToTable(string[] valueCollection, char delimiter, ref DataTable dt)
        {
            for (int i = 1; i < valueCollection.Length; i++)
            {
                string[] values = valueCollection[i].Split(delimiter);
                DataRow dr = dt.NewRow();
                for (int j = 0; j < values.Length; j++)
                {
                    dr[j] = values[j];
                }
                dt.Rows.Add(dr);
            }
        }
        private static void AddColumnToTable(string[] colcollection,char delimit,ref DataTable dt)
        {
            string[] col = colcollection[0].Split(delimit);
            foreach (string columnName in col)
            {
                DataColumn dc = new DataColumn(columnName, typeof(string));
                dt.Columns.Add(dc);
            }
        }
    }
}
