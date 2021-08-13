using System;
using System.Collections.Generic;
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
using HalconDotNet;
using HalconViewer;
using MLib;

namespace VisionTest
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        static string VisionPath = MFile.path4 + @"Vision\";
        static string ShapeModelPath = VisionPath + "Test.Shm";

        public MainWindow()
        {
            InitializeComponent();
        }

        private void BtOpenImage_Click(object sender, RoutedEventArgs e)
        {
            string _ImagePath;
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
            if (dlg.ShowDialog() == false)
                return;
            _ImagePath = dlg.FileName;
            HObject ho_image;
            //从文件中读取图像
            HOperatorSet.ReadImage(out ho_image, _ImagePath);
            if (mImageViewer != null)
            {
                //将图像传入图像窗口
                mImageViewer.Image = VisionManager.HObjectToHImage(ho_image);
                //刷新图像窗口
                mImageViewer.Repaint = !mImageViewer.Repaint;
            }
        }

        private void BtCreateShapeModel_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult dr = MessageBox.Show("是否要创建模板！", "确认操作", MessageBoxButton.OKCancel, MessageBoxImage.Question);
            if (dr != MessageBoxResult.OK)
                return;
            VisionManager.CreateShapeModel(mImageViewer);

        }

        private void BtSaveShapeModel_Click(object sender, RoutedEventArgs e)
        {
            VisionManager.SaveShapeModel(ShapeModelPath);
        }

        private void BtFindShapeModel_Click(object sender, RoutedEventArgs e)
        {
            mImageViewer.ROIList.Clear();
            double[] a= VisionManager.FindShapeModel(ShapeModelPath, VisionPath + "TestRegion.ROI", mImageViewer.Image, mImageViewer, true, 0.5,0,360);
            mImageViewer.Repaint = !mImageViewer.Repaint;
            mLogS.Mylog("X:"+a[0].ToString()+ ", Y:" + a[1].ToString()+ ", R:" + a[2].ToString());
        }

        private void BtCreateRegion_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult dr = MessageBox.Show("是否创建扫码区域？", "确认操作", MessageBoxButton.OKCancel, MessageBoxImage.Question);
            if (dr != MessageBoxResult.OK)
                return;
            VisionManager.CreateRegion(VisionPath + "TestRegion.ROI", mImageViewer);
            
        }
    }
}
