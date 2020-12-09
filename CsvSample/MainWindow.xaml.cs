
using MLib;
using System;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;


namespace CsvSample
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public string csvFilePath = MFile.path4 + "test.csv";
        DataTable dataTable = new DataTable();
        Csv csv = new Csv();
        string[] insertContent=new string[20];
        int i = 0;

        public MainWindow()
        {
            InitializeComponent();
            Init();
            
        }

        public void Init()
        {
            ImportCsv();
            insertContent[0] = DateTime.Now.ToString();

        }

        public void ImportCsv()
        {
            dataTable=csv.Csv2Dt(csvFilePath);
            dg1.ItemsSource = dataTable.AsDataView();
        }

        public void ExportCsv()
        {
            csv.Dt2Csv(dataTable, csvFilePath);
        }

        private void btImport_Click(object sender, RoutedEventArgs e)
        {
            ImportCsv();
            
        }

        private void btExport_Click(object sender, RoutedEventArgs e)
        {
            ExportCsv();
        }

        private void btInsertLine_Click(object sender, RoutedEventArgs e)
        {
            insertContent[1] = (++i).ToString();
            insertContent[2] = (++i * 10).ToString();
            csv.InsertLine(csvFilePath, insertContent);
        }
    }
}

