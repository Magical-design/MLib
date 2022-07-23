using System;
using System.Collections.Generic;
using System.ComponentModel;
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
using ViewROI;


namespace VisionTest
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        static string VisionPath = MFile.path4 + @"Vision\";
        static string ShapeModelPath = VisionPath + "Test.Shm";
        private Parameter mmParameter;
        public Parameter mParameter
        {
            get 
            {
                return mmParameter;
            }
            set
            {
                mmParameter = value;
                OnPropertyChanged("mParameter");
            }
        }
        IXml xml = new IXml();
        public MainWindow()
        {
            InitializeComponent();
            DataContext = this;
            Init();
        }

        public void Init()
        {
            try
            {
                
                mParameter = (Parameter)xml.deserialize_from_xml(MFile.path4 , "Parameters.xml", typeof(Parameter));
            }
            catch (Exception)
            {
                mParameter = new Parameter() { lineParameter = new LineParameter() { Row=1.0 } };
                xml.serialize_to_xml(MFile.path4 , "Parameters.xml", mParameter);
            }
        }
        HObject hoimage;
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
            hoimage = ho_image;
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
            MessageBoxResult dr = MessageBox.Show("是否创建区域？", "确认操作", MessageBoxButton.OKCancel, MessageBoxImage.Question);
            if (dr != MessageBoxResult.OK)
                return;
            VisionManager.CreateRegion(VisionPath + "TestRegion.ROI", mImageViewer);
            
        }

        private void BtCreateCircle_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult dr = MessageBox.Show("是否创建⚪？", "确认操作", MessageBoxButton.OKCancel, MessageBoxImage.Question);
            if (dr != MessageBoxResult.OK)
                return;
            VisionManager.CreateRegion(VisionPath + "CircleRegion.ROI", mImageViewer,ROI.ROI_TYPE_CIRCLE);
        }
        
        private void BtFindCircleCenter_Click(object sender, RoutedEventArgs e)
        {
            
            HTuple oarea , orow, ocolumn;
            mImageViewer.ROIList.Clear();
            mImageViewer.AppendHMessage = null;
            mImageViewer.AppendHObject = null;
            ROI rOI= VisionManager.ReadROI(VisionPath + "CircleRegion.ROI");           
            ROI mROI = rOI;
            rOI.SizeEnable = true;
            //rOI.moveByHandle(1000, 1000);
            //rOI.SizeEnable = false;
            mImageViewer.ROIList.Add(rOI);
            mLogS.Mylog(rOI.getModelData().ToString());
            orow = rOI.getModelData()[0];
            ocolumn = rOI.getModelData()[1];
            HObject hObject;
            HOperatorSet.GenCrossContourXld(out hObject, orow, ocolumn, 40,new HTuple(45).TupleRad());
            
            int r1 = (int)orow.D+20;
            int c1 = (int)ocolumn.D;
            if (r1 < 0)
                r1 = 0;
            if (c1 < 0)
                c1 = 0;
            HMsgEntry mhms = new HMsgEntry("("+orow +","+ ocolumn + ")", r1, c1, "green", "image", new HTuple().TupleConcat("box"), new HTuple().TupleConcat("false"));
            Tuple<string, object> t = new Tuple<string, object>("Colored", 12);
            Tuple<string, object> t1 = new Tuple<string, object>("DrawMode", "Fill");

            mImageViewer.GCStyle = t;
            mImageViewer.GCStyle = t1;
            mImageViewer.AppendHObject = hObject;
            mImageViewer.AppendHMessage = mhms;
           
            //刷新图像窗口
            try
            {
                mImageViewer.Repaint = !mImageViewer.Repaint;
            }
            catch (Exception ex)
            {

            }
           
            mLogS.Mylog("X:" + orow.ToString() + ", Y:" + ocolumn.ToString(),Brushes.Red);

        }

        private void BtCreateLine1_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult dr = MessageBox.Show("是否创建直线1？", "确认操作", MessageBoxButton.OKCancel, MessageBoxImage.Question);
            if (dr != MessageBoxResult.OK)
                return;
            VisionManager.CreateRegion(VisionPath + "Line1.ROI", mImageViewer, ROI.ROI_TYPE_LINE);
        }
        private void BtCreateLine2_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult dr = MessageBox.Show("是否创建直线2？", "确认操作", MessageBoxButton.OKCancel, MessageBoxImage.Question);
            if (dr != MessageBoxResult.OK)
                return;
            VisionManager.CreateRegion(VisionPath + "Line2.ROI", mImageViewer, ROI.ROI_TYPE_LINE);
        }
        private void BtCreateRegion1_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult dr = MessageBox.Show("是否创建区域？", "确认操作", MessageBoxButton.OKCancel, MessageBoxImage.Question);
            if (dr != MessageBoxResult.OK)
                return;
            VisionManager.CreateRegion(VisionPath + "TestLineRegion.ROI", mImageViewer);
        }
        List<HObject> HObjectList = new List<HObject>();
        List<HMsgEntry> HMsgList = new List<HMsgEntry>();
        private void BtFindCross_Click(object sender, RoutedEventArgs e)
        {
            //查找直线用的参数
            int Elements = 100;
            int ActiveElements = 50;
            int DetectHeight = 50;
            string Transition = "negative";
            string Select = "first";
            int Sigma = 1;
            int Threshold = 20;
            ImageViewerClear();

            HTuple hom;
            HOperatorSet.VectorAngleToRigid(0, 0, 0, mmParameter.lineParameter.Row, mmParameter.lineParameter.Column, new HTuple(mmParameter.lineParameter.Angle).TupleRad(), out hom);

            double[] mFindLocation1 = new double[5];
            double[] mFindLocation2 = new double[5];
            for (int i = 0; i < 2; i++)
            {
                double[] mFindLocation = new double[5];
                ROILine mROI_Line = (ROILine)VisionManager.ReadROI(VisionPath + "Line"+ (i + 1).ToString() + ".ROI");
                HTuple lrow, lcol;
                HOperatorSet.AffineTransPoint2d(hom, mROI_Line.row1, mROI_Line.col1, out lrow, out lcol);
                mROI_Line.row1 = lrow;
                mROI_Line.col1 = lcol;
                HOperatorSet.AffineTransPoint2d(hom, mROI_Line.row2, mROI_Line.col2, out lrow, out lcol);
                mROI_Line.row2 = lrow;
                mROI_Line.col2 = lcol;


                try
                {
                    mFindLocation = VisionManager.FindLine(mImageViewer.Image, (mROI_Line as ROILine).row1, (mROI_Line as ROILine).col1, (mROI_Line as ROILine).row2, (mROI_Line as ROILine).col2, Transition, Elements, ActiveElements, DetectHeight, Select, Sigma, Threshold);
                }
                catch { }
                if (mFindLocation[0] == 0 && mFindLocation[1] == 0 && mFindLocation[2] == 0 && mFindLocation[3] == 0 && mFindLocation[4] == 0)
                {
                    MessageBox.Show("直线"+ (i + 1).ToString() + "未识别");
                    return;
                }

                ROILine ROI_Result = new ROILine();
                ROI_Result.SizeEnable = false;

                ROI_Result.row1 = mFindLocation[0];
                ROI_Result.col1 = mFindLocation[1];
                ROI_Result.row2 = mFindLocation[2];
                ROI_Result.col2 = mFindLocation[3];
                ROI_Result.ROIColor = "green";


                mImageViewer.ROIList.Add(ROI_Result);
                switch (i)
                {
                    case 0:
                        mFindLocation1 = mFindLocation;
                        break;
                    case 1:
                        mFindLocation2 = mFindLocation;
                        break;
                    default:
                        break;
                }

            }

            HObject hObject = new HObject();
            HTuple row, column,isoverlapping;
            HOperatorSet.IntersectionLines(mFindLocation1[0], mFindLocation1[1], mFindLocation1[2], mFindLocation1[3], mFindLocation2[0], mFindLocation2[1], mFindLocation2[2], mFindLocation2[3],out row, out column, out isoverlapping);
            HOperatorSet.GenCircle(out hObject, row,column, 10);
            HObjectList.Add(hObject);
            HOperatorSet.GenCrossContourXld(out hObject, row, column, 100, new HTuple(0).TupleRad());
            HObjectList.Add(hObject);
            Tuple<string, object> t = new Tuple<string, object>("Colored", 12);
            Tuple<string, object> t1 = new Tuple<string, object>("DrawMode", "fill");
            Tuple<string, object> t2 = new Tuple<string, object>("DrawMode", "margin");

            mImageViewer.GCStyle = t;
            mImageViewer.GCStyle = t1;
            for (int i = 0; i < HObjectList.Count; i++)
            {
                mImageViewer.AppendHObject = HObjectList[i];

            }

            mImageViewer.Repaint = !mImageViewer.Repaint;
            mLogS.Mylog("X:" + row.ToString() + ", Y:" + column.ToString(), Brushes.DeepPink);



        }

        private void BtTransImage_Click(object sender, RoutedEventArgs e)
        {

            xml.serialize_to_xml(MFile.path4 , "Parameters.xml", mParameter);
            HTuple hom;
            HObject ho_image;
            try
            {
                HOperatorSet.VectorAngleToRigid(0, 0, 0, mmParameter.lineParameter.Row, mmParameter.lineParameter.Column, new HTuple(mmParameter.lineParameter.Angle).TupleRad(), out hom);
                HOperatorSet.AffineTransImage(hoimage, out ho_image, hom, "constant", "false");

                mImageViewer.Image = VisionManager.HObjectToHImage(ho_image);
                mImageViewer.Repaint = !mImageViewer.Repaint;
            }
            catch (Exception ex)
            {
                mLogS.Mylog(ex.ToString(), Brushes.Red);
            }

        }
        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string propertyName)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void BtFindGray_Click(object sender, RoutedEventArgs e)
        {
            xml.serialize_to_xml(MFile.path4 , "Parameters.xml", mParameter);
            ImageViewerClear();
            try
            {
                ROI rOI = VisionManager.ReadROI(VisionPath + "TestGrayRegion.ROI");
                mImageViewer.ROIList.Add(rOI);





                Tuple<string, object> t = new Tuple<string, object>("Colored", 12);
                Tuple<string, object> t1 = new Tuple<string, object>("DrawMode", "fill");
                Tuple<string, object> t2 = new Tuple<string, object>("DrawMode", "margin");

                mImageViewer.GCStyle = t;
                mImageViewer.GCStyle = t1;
                for (int i = 0; i < HObjectList.Count; i++)
                {
                    mImageViewer.AppendHObject = HObjectList[i];

                }

                mImageViewer.Repaint = !mImageViewer.Repaint;
            }
            catch (Exception ex)
            {
                mLogS.Mylog(ex.ToString(), Brushes.Red);
            }





        }

        private void BtCreateRegion_Gray_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult dr = MessageBox.Show("是否创建区域？", "确认操作", MessageBoxButton.OKCancel, MessageBoxImage.Question);
            if (dr != MessageBoxResult.OK)
                return;
            VisionManager.CreateRegion(VisionPath + "TestGrayRegion.ROI", mImageViewer);
        }

        public void ImageViewerClear()
        {
            //清空当前窗口
            mImageViewer.AppendHObject = null;
            mImageViewer.AppendHMessage = null;
            mImageViewer.ROIList.Clear();
            HObjectList.Clear();
            HMsgList.Clear();
            mImageViewer.Repaint = !mImageViewer.Repaint;
        }

        private void TxMinGray_TextChanged(object sender, TextChangedEventArgs e)
        {
            
        }

        private void TxMinGray_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            
        }

        private void TxMinGray_LostFocus(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("a");
        }
    }
}
