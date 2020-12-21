using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using MLib;
using OfficeOpenXml;

namespace ExcelSample
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public string ExcFilePath = MFile.path4 + "test.xlsx";
        public MainWindow()
        {
            InitializeComponent();
            Init();
        }
        DataTable dt=new DataTable();
        DataTable dt1 = new DataTable();
        public void Init()
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            try
            {
                using (ExcelPackage package = new ExcelPackage(new FileInfo(ExcFilePath)))
                {
                    var ws = package.Workbook.Worksheets.Add("Sheet1");
                    var ws1 = package.Workbook.Worksheets.Add("Sheet2");                    
                    ws.Cells[1, 1].Value = "时间";
                    ws.Cells[1, 2].Value = "数据1";
                    ws.Cells[1, 3].Value = "数据2";
                    ws1.Cells[1, 1].Value = "时间";
                    ws1.Cells[1, 2].Value = "数据3";
                    ws1.Cells[1, 3].Value = "数据4";
                    package.SaveAs(new FileInfo(ExcFilePath));
                }
            }
            catch (Exception)
            {


            }

        }
        public void ImportExc()
        {
            using (ExcelPackage package = new ExcelPackage(new FileInfo(ExcFilePath)))
            {
                ExcelWorksheet worksheet = package.Workbook.Worksheets.FirstOrDefault(s=> s.Name== "Sheet1");
                if (worksheet == null)
                    worksheet = package.Workbook.Worksheets.Add("Sheet1");
                    
                ExcelWorksheet worksheet1 = package.Workbook.Worksheets.FirstOrDefault(s => s.Name == "Sheet2");
                if (worksheet1 == null)
                    worksheet1 = package.Workbook.Worksheets.Add("Sheet2");

                dt = Excel.WorksheetToTable(worksheet);
                dt1 = Excel.WorksheetToTable(worksheet1);
                dg1.ItemsSource = dt.AsDataView();
                dg2.ItemsSource = dt1.AsDataView();
                package.SaveAs(new FileInfo(ExcFilePath));
            }
        }
        public void ExportExc()
        {
            using (ExcelPackage package = new ExcelPackage(new FileInfo(ExcFilePath)))
            {
                ExcelWorksheet worksheet = package.Workbook.Worksheets.FirstOrDefault(s => s.Name == "Sheet1");
                ExcelWorksheet worksheet1 = package.Workbook.Worksheets.FirstOrDefault(s => s.Name == "Sheet2");
                worksheet.Cells["A1"].LoadFromDataTable(dt, true);
                worksheet1.Cells.LoadFromDataTable(dt1, true);
                package.SaveAs(new FileInfo(ExcFilePath));
            }
        }
        private void btInsertLine1_Click(object sender, RoutedEventArgs e)
        {
            int row;
            using (ExcelPackage package = new ExcelPackage(new FileInfo(ExcFilePath)))
            {
                ExcelWorksheet worksheet = package.Workbook.Worksheets.FirstOrDefault(s => s.Name == "Sheet1");
                row = worksheet.Dimension.Rows + 1;
                worksheet.Cells[row, 1].Value = System.DateTime.Now.ToString();
                worksheet.Cells[row, 2].Value = "第"+ row + "行B";
                worksheet.Cells[row, 3].Value = "第" + row + "行C";
                dt = Excel.WorksheetToTable(worksheet);
                dg1.ItemsSource = dt.AsDataView();
                package.SaveAs(new FileInfo(ExcFilePath));
            }
        }


        private void btInsertLine2_Click(object sender, RoutedEventArgs e)
        {
            int row;
            using (ExcelPackage package = new ExcelPackage(new FileInfo(ExcFilePath)))
            {
                ExcelWorksheet worksheet1 = package.Workbook.Worksheets.FirstOrDefault(s => s.Name == "Sheet2");
                row = worksheet1.Dimension.Rows + 1;
                worksheet1.Cells[row, 1].Value = System.DateTime.Now.ToString();
                worksheet1.Cells[row, 2].Value = "第" + row + "行B";
                worksheet1.Cells[row, 3].Value = "第" + row + "行C";
                dt1 = Excel.WorksheetToTable(worksheet1);
                dg2.ItemsSource = dt1.AsDataView();
                package.SaveAs(new FileInfo(ExcFilePath));
            }
        }

        private void btImport_Click(object sender, RoutedEventArgs e)
        {
            ImportExc();
        }

        private void btExport_Click(object sender, RoutedEventArgs e)
        {
            ExportExc();
        }
    }
}
