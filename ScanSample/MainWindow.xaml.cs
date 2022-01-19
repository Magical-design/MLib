
using ViewROI;
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
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace ScanSample
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public string _Path = MFile.path4 + @"Vision\";
        public MainWindow()
        {
            InitializeComponent();
            Init();
        }

        public void Init()
        {

        }
        private void btOpenPIC_Click(object sender, RoutedEventArgs e)
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
                mImageViewer.Image = HObjectToHImage(ho_image);
                //刷新图像窗口
                mImageViewer.Repaint = !mImageViewer.Repaint;
            }
        }

        private void btRecognitionRegion_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult dr = MessageBox.Show("是否创建扫码区域？", "确认操作", MessageBoxButton.OKCancel, MessageBoxImage.Question);
            if (dr != MessageBoxResult.OK)
                return;

            CreateRegion(_Path + "ScanRegion.ROI", mImageViewer);
        }
        List<HObject> HObjectList = new List<HObject>();
        List<HMsgEntry> HMsgList = new List<HMsgEntry>();
        HObject ho_SymbolXLDs;
        HObject mImagePart = new HObject();
        HObject newxld = new HObject();
        private void btRecognition_Click(object sender, RoutedEventArgs e)
        {
            mImageViewer.AppendHObject = null;
            mImageViewer.AppendHMessage = null;
            mImageViewer.ROIList.Clear();

            HOperatorSet.GenEmptyObj(out ho_SymbolXLDs);
            HObjectList.Clear();
            HMsgList.Clear();
            ROI mROI = ReadROI(_Path + "ScanRegion.ROI");

            try
            {
                mImageViewer.ROIList.Add(mROI);
             
                HOperatorSet.CropRectangle1(mImageViewer.Image, out mImagePart, (int)(mROI as ROIRectangle1).row1, (int)(mROI as ROIRectangle1).col1, (int)(mROI as ROIRectangle1).row2, (int)(mROI as ROIRectangle1).col2);
                string result = Barcode.GetBarCode(mImagePart,ref ho_SymbolXLDs);
                mImagePart.Dispose();
                HTuple calibMovementOfObject;

                HOperatorSet.VectorAngleToRigid(0, 0, 0, (int)(mROI as ROIRectangle1).row1, (int)(mROI as ROIRectangle1).col1
                    , 0, out calibMovementOfObject);
                HOperatorSet.AffineTransContourXld(ho_SymbolXLDs, out newxld, calibMovementOfObject);
                ho_SymbolXLDs.Dispose();


                int r1 = (int)(mROI as ROIRectangle1).row1 - 20;
                if (r1 < 0)
                    r1 = 0;
                int c1 = (int)(mROI as ROIRectangle1).col1;
                if (c1 < 0)
                    c1 = 0;
                HMsgEntry mhms = new HMsgEntry(result, r1, c1, "orange", "image", new HTuple().TupleConcat("box"), new HTuple().TupleConcat("false"));
                HObjectList.Add(newxld);
                HMsgList.Add(mhms);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Task_Scan:" + ex.Message);
                mImagePart.Dispose();
                ho_SymbolXLDs.Dispose();
            }


            Tuple<string, object> t = new Tuple<string, object>("Colored", 12);
            Tuple<string, object> t1 = new Tuple<string, object>("DrawMode", "margin");

            mImageViewer.GCStyle = t;
            mImageViewer.GCStyle = t1;
            for (int i = 0; i < HObjectList.Count; i++)
            {
                mImageViewer.AppendHObject = HObjectList[i];

            }
            for (int i = 0; i < HMsgList.Count; i++)
            {
               mImageViewer.AppendHMessage = HMsgList[i];
               
            }


            //刷新图像窗口
            try
            {
                mImageViewer.Repaint = !mImageViewer.Repaint;
                
            }
            catch (Exception ex)
            {

            }


        }
        public static void CreateRegion(string _ROIPath, ImageViewer _ImageViewer = null)
        {
            _ImageViewer.ROIList.Clear();
            _ImageViewer.Repaint = !_ImageViewer.Repaint;

            ROI r = _ImageViewer.DrawROI(ROI.ROI_TYPE_RECTANGLE1);

            if (r == null)
                return;
            r.SizeEnable = false;

            WriteROI(r, _ROIPath);

            if (_ImageViewer.ROIList == null)
                _ImageViewer.ROIList = new System.Collections.ObjectModel.ObservableCollection<ROI>();
            _ImageViewer.ROIList.Add(r);
            _ImageViewer.Repaint = !_ImageViewer.Repaint;

        }
        public static void WriteROI(ROI _ROI, string _ROIPath)
        {
            try
            {
                FileStream fileStream = new FileStream(_ROIPath, FileMode.Create);
                BinaryFormatter b = new BinaryFormatter();
                b.Serialize(fileStream, _ROI);
                fileStream.Close();
            }
            catch { }
        }

        public static HImage HObjectToHImage(HObject hObject)
        {
            HImage hImage = null;
            try
            {
                HTuple channels;
                HOperatorSet.CountChannels(hObject, out channels);
                if (channels.I == 1)
                {
                    HTuple pointer, htype, width, height;
                    HOperatorSet.GetImagePointer1(hObject, out pointer, out htype, out width, out height);
                    hImage = new HImage();
                    hImage.GenImage1(htype.S, width.I, height.I, pointer.IP);
                }
                else if (channels.I == 3)
                {
                    HTuple pointerR, pointerG, pointerB, htype, width, height;
                    HOperatorSet.GetImagePointer3(hObject, out pointerR, out pointerG, out pointerB, out htype, out width, out height);
                    hImage = new HImage();
                    hImage.GenImage3(htype.S, width.I, height.I, pointerR.IP, pointerG.IP, pointerB.IP);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("HObjectToHImage;" + ex.Message);
            }
            return hImage;
        }
        public static ROI ReadROI(string _ROIPath)
        {
            ROI mROI = null;
            try
            {
                FileStream fileStream = new FileStream(_ROIPath, FileMode.Open, FileAccess.Read, FileShare.Read);
                BinaryFormatter mBinFmat = new BinaryFormatter();
                mROI = (ROI)mBinFmat.Deserialize(fileStream);
                fileStream.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return mROI;
        }
    }
}
