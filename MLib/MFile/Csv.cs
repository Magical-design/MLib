using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;



namespace MLib
{
    public class Csv
    {
        public DataTable Csv2Dt(string filePath)
        {
            DataTable dt = new DataTable();
            Regex regex = new Regex(",(?=(?:[^\"]*\"[^\"]*\")*[^\"]*$)");
            using (StreamReader sr = new StreamReader(filePath,Encoding.Default))
            {
                string[] headers = sr.ReadLine().Split(',');
                foreach (string header in headers)
                {
                    dt.Columns.Add(header);
                }
                while (sr.Peek() > 0)
                {
                    string[] rows = regex.Split(sr.ReadLine());
                    DataRow dr = dt.NewRow();
                    for (int i = 0; i < headers.Length; i++)
                    {
                        dr[i] = rows[i];
                    }
                    dt.Rows.Add(dr);
                }
            }
            return dt;
        }
        public bool Dt2Csv(DataTable vContent, string vOutputFilePath)
        {
            bool sResult=false;
            System.Text.StringBuilder sCsvContent;
            try
            {
                sCsvContent = new System.Text.StringBuilder();
                //栏位
                for (int i = 0; i < vContent.Columns.Count; i++)
                {
                    sCsvContent.Append(vContent.Columns[i].ColumnName);
                    sCsvContent.Append(i == vContent.Columns.Count - 1 ? "\r\n" : ",");
                }
                //数据
                foreach (System.Data.DataRow row in vContent.Rows)
                {
                    for (int i = 0; i < vContent.Columns.Count; i++)
                    {
                        sCsvContent.Append(row[i].ToString().Trim());
                        sCsvContent.Append(i == vContent.Columns.Count - 1 ? "\r\n" : ",");
                    }
                }
                File.WriteAllText(vOutputFilePath, sCsvContent.ToString(), Encoding.UTF8);
                sResult = true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                sResult = false;
            }
            return sResult;
        }

        public  bool InsertLine(string strFilePath, string[] value)
        {
            try
            {
                var filewriter = new StreamWriter(strFilePath, true, Encoding.Default);
                filewriter.WriteLine(String.Join(",", value));
                filewriter.Flush();
                filewriter.Close(); 
                return true;
            }
            catch { return false; }
        }

    }
}
