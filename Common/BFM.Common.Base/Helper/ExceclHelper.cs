using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.IO;
using System.Runtime.InteropServices;
using System.Windows.Controls;
using Excel = Microsoft.Office.Interop.Excel;
using DataTable = System.Data.DataTable;

namespace BFM.Common.Base.Helper
{
    public abstract class ExceclHelper
    {

        [DllImport("User32.dll", CharSet = CharSet.Auto)]
        private static extern int GetWindowThreadProcessId(IntPtr hwnd, out int id); //根据Handel获取程序的进程


        /// <summary>
        /// 根据Excel文件获取内容，采用Ole读取方式
        /// </summary>
        /// <param name="fileName">Excel文件名</param>
        /// <returns></returns>
        public static List<DataTable> GetDataByOle(string fileName)
        {
            List<DataTable> result = new List<DataTable>();
            string fileSuffix = System.IO.Path.GetExtension(fileName);

            string connString = "";
            if (fileSuffix == ".xls")

                connString = "Provider=Microsoft.Jet.OLEDB.4.0;"
                             + "Data Source=" + fileName + ";"
                             + ";Extended Properties=\"Excel 8.0;HDR=YES;IMEX=1\"";
            else

                connString = "Provider=Microsoft.ACE.OLEDB.12.0;"
                             + "Data Source=" + fileName + ";"
                             + ";Extended Properties=\"Excel 12.0;HDR=YES;IMEX=1\"";

            string sqlSelect = " SELECT * FROM {0}";
            using (OleDbConnection conn = new OleDbConnection(connString))
            {
                conn.Open();
                DataTable sheetsNames = conn.GetOleDbSchemaTable(OleDbSchemaGuid.Tables,
                    new object[] {null, null, null, "Table"}); //得到所有sheet的名字

                if (sheetsNames == null) return null;
                foreach (DataRow dr in sheetsNames.Rows)
                {
                    using (DataSet ds = new DataSet())
                    {
                        string sheetsName = dr[2].ToString();
                        sqlSelect = string.Format(" SELECT * FROM {0}", sheetsName);
                        using (OleDbDataAdapter cmd = new OleDbDataAdapter(sqlSelect, conn))
                        {
                            cmd.Fill(ds);
                        }

                        if (ds == null || ds.Tables.Count <= 0) continue;

                        result.Add(ds.Tables[0]);
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// 读取Excel返回表
        /// </summary>
        /// <param name="fileName">Excel文件名（含路径）</param>
        /// <param name="isDataContinuous">数据是否连续，true:当检测到空行数据后则不继续读取后面行的数据</param>
        /// <returns></returns>
        public static List<DataTable> GetDataByCom(string fileName, bool isDataContinuous)
        {
            List<DataTable> result = new List<DataTable>();
            Excel.Application app = new Excel.Application();
            app.DisplayAlerts = false;

            Excel.Sheets sheets;
            Excel.Workbook workbook = null;
            object oMissiong = System.Reflection.Missing.Value;

            if (app == null) return null;

            try
            {
                workbook = app.Workbooks.Open(fileName, oMissiong, oMissiong, oMissiong, oMissiong, oMissiong, oMissiong,
                    oMissiong, oMissiong, oMissiong, oMissiong, oMissiong, oMissiong, oMissiong, oMissiong);
                //将数据读入到DataTable中——Start 
                sheets = workbook.Sheets;

                for (int i = 1; i <= sheets.Count; i++)
                {
                    Excel.Worksheet worksheet = sheets.get_Item(i); //依次读取表信息
                    if (worksheet == null) continue;

                    Excel.Range range;

                    int iRowCount = worksheet.UsedRange.Rows.Count;
                    int iColCount = worksheet.UsedRange.Columns.Count;


                    DataTable dt = new DataTable("GetDataByCom");
                    for (int col = 1; col <= iColCount; col++)
                    {
                        DataColumn dc = new DataColumn();
                        dc.DataType = Type.GetType("System.String");
                        dc.ColumnName = string.Format("Col{0}", col);
                        dt.Columns.Add(dc);
                    }

                    for (int iRow = 1; iRow <= iRowCount; iRow++)
                    {
                        DataRow dr = dt.NewRow();
                        string rowValue = "";
                        for (int iCol = 1; iCol <= iColCount; iCol++)
                        {
                            range = (Excel.Range) worksheet.Cells[iRow, iCol];
                            string cellText = (range.Value2 == null) ? "" : range.Text.ToString();
                            dr[iCol - 1] = cellText;
                            rowValue += cellText.Trim();
                        }
                        if (string.IsNullOrEmpty(rowValue) && isDataContinuous) break; //数据如果
                        if (!string.IsNullOrEmpty(rowValue)) dt.Rows.Add(dr);
                    }
                    result.Add(dt);
                }
            }
            catch (Exception)
            {
                return null;
            }
            finally
            {
                workbook.Close(false, oMissiong, oMissiong);
                System.Runtime.InteropServices.Marshal.ReleaseComObject(workbook);
                workbook = null;
                app.Workbooks.Close();
                app.Quit();
                if (app != null)
                {
                    IntPtr t = new IntPtr(app.Hwnd);
                    int k = 0;
                    GetWindowThreadProcessId(t, out k);
                    System.Diagnostics.Process p = System.Diagnostics.Process.GetProcessById(k);
                    p.Kill();
                }
            }

            return result;
        }
        
        public static void DataGridExport(DataGrid dataGrid, string fileName, string sheetName = "查询结果")
        {
            string tempPath = Environment.CurrentDirectory + "/temp";
            if (!Directory.Exists(tempPath))
            {
                Directory.CreateDirectory(tempPath);
            }

            fileName = tempPath + "/" + fileName;
            Microsoft.Office.Interop.Excel.Application xlApp = new Microsoft.Office.Interop.Excel.Application();
            Microsoft.Office.Interop.Excel.Workbooks workbooks = xlApp.Workbooks;
            Microsoft.Office.Interop.Excel.Workbook workbook = workbooks.Add(Microsoft.Office.Interop.Excel.XlWBATemplate.xlWBATWorksheet);
            Microsoft.Office.Interop.Excel.Worksheet worksheet = (Microsoft.Office.Interop.Excel.Worksheet)workbook.Worksheets[1]; //取得sheet1
            worksheet.Name = sheetName;

            //写入行
            //+++++++++为提高效率，采用复制粘贴的方式写入数据+++++++++++++
            for (int i = 0; i < dataGrid.Columns.Count; i++)
            {
                worksheet.Cells[1, i + 1] = dataGrid.Columns[i].Header;
            }
            for (int r = 0; r < dataGrid.Items.Count; r++)
            {
                for (int i = 0; i < dataGrid.Columns.Count; i++)
                {
                    string value = (dataGrid.Columns[i].GetCellContent(dataGrid.Items[r]) as TextBlock)?.Text ?? (r + 1).ToString();  //读取DataGrid某一行某一列的信息内容，与DataGridView不同的地方

                    worksheet.Cells[r + 2, i + 1] = "'" + value;
                }
            }
            worksheet.Columns.EntireColumn.AutoFit();
            workbook.Saved = true;
            workbook.SaveCopyAs(fileName);
            xlApp.Visible = true;
        }

    }
}
