using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Interop;
using System.Drawing;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Runtime.CompilerServices;
using System.Threading;

namespace MLib
{
    public class ViewBase : INotifyPropertyChanged
    {
        /// 根据Int类型的值，返回用1或0(对应True或Flase)填充的数组
        /// <remarks>从右侧开始向左索引(0~31)</remarks>
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static IEnumerable<bool> GetBitList(int value)
        {
            var list = new List<bool>(32);
            for (var i = 0; i <= 31; i++)
            {
                var val = 1 << i;
                list.Add((value & val) == val);
            }
            return list;
        }

        /// <summary>
        /// 返回Int数据中某一位是否为1
        /// </summary>
        /// <param name="value"></param>
        /// <param name="index">32位数据的从右向左的偏移位索引(0~31)</param>
        /// <returns>true表示该位为1，false表示该位为0</returns>
        public static bool GetBitValue(int value, ushort index)
        {
            if (index > 31) throw new ArgumentOutOfRangeException("index"); //索引出错
            var val = 1 << index;
            return (value & val) == val;
        }
        /// <summary>
        /// 设定Int数据中某一位的值
        /// </summary>
        /// <param name="value">位设定前的值</param>
        /// <param name="index">32位数据的从右向左的偏移位索引(0~31)</param>
        /// <param name="bitValue">true设该位为1,false设为0</param>
        /// <returns>返回位设定后的值</returns>
        public static int SetBitValue(int value, ushort index, bool bitValue)
        {
            if (index > 31) throw new ArgumentOutOfRangeException("index"); //索引出错
            var val = 1 << index;
            return bitValue ? (value | val) : (value & ~val);
        }


        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged(string propertyName)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        private void OnPropertyChanged1([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public bool SetProperty<T>(ref T field, T value, [CallerMemberName] string propertyName = null)
        {
            if (EqualityComparer<T>.Default.Equals(field, value))
            {
                return false;
            }

            field = value;
            OnPropertyChanged1(propertyName);
            return true;
        }


    }

    public static class MessageBoxHelper
    {
        public static void PrepToCenterMessageBoxOnForm(Window form)
        {
            MessageBoxCenterHelper helper = new MessageBoxCenterHelper();
            helper.Prep(form);
        }

        private class MessageBoxCenterHelper
        {
            private int messageHook;
            private IntPtr parentFormHandle;

            public void Prep(Window form)
            {
                NativeMethods.CenterMessageCallBackDelegate callBackDelegate = new NativeMethods.CenterMessageCallBackDelegate(CenterMessageCallBack);
                GCHandle.Alloc(callBackDelegate);

                parentFormHandle = new WindowInteropHelper(form).Handle;
                messageHook = NativeMethods.SetWindowsHookEx(5, callBackDelegate, new IntPtr(NativeMethods.GetWindowLong(parentFormHandle, -6)), NativeMethods.GetCurrentThreadId()).ToInt32();
            }

            private int CenterMessageCallBack(int message, int wParam, int lParam)
            {
                NativeMethods.RECT formRect;
                NativeMethods.RECT messageBoxRect;
                int xPos;
                int yPos;

                if (message == 5)
                {
                    NativeMethods.GetWindowRect(parentFormHandle, out formRect);
                    NativeMethods.GetWindowRect(new IntPtr(wParam), out messageBoxRect);

                    xPos = (int)((formRect.Left + (formRect.Right - formRect.Left) / 2) - ((messageBoxRect.Right - messageBoxRect.Left) / 2));
                    yPos = (int)((formRect.Top + (formRect.Bottom - formRect.Top) / 2) - ((messageBoxRect.Bottom - messageBoxRect.Top) / 2));
                    if (xPos < 0)
                        xPos = 100;
                    if (yPos < 0)
                        yPos = 100;
                    NativeMethods.SetWindowPos(wParam, 0, xPos, yPos, 0, 0, 0x1 | 0x4 | 0x10);
                    NativeMethods.UnhookWindowsHookEx(messageHook);
                }

                return 0;
            }
        }

        private static class NativeMethods
        {
            internal struct RECT
            {
                public int Left;
                public int Top;
                public int Right;
                public int Bottom;
            }

            internal delegate int CenterMessageCallBackDelegate(int message, int wParam, int lParam);

            [DllImport("user32.dll")]
            [return: MarshalAs(UnmanagedType.Bool)]
            internal static extern bool UnhookWindowsHookEx(int hhk);

            [DllImport("user32.dll", SetLastError = true)]
            internal static extern int GetWindowLong(IntPtr hWnd, int nIndex);

            [DllImport("kernel32.dll")]
            internal static extern int GetCurrentThreadId();

            [DllImport("user32.dll", SetLastError = true)]
            internal static extern IntPtr SetWindowsHookEx(int hook, CenterMessageCallBackDelegate callback, IntPtr hMod, int dwThreadId);

            [DllImport("user32.dll")]
            [return: MarshalAs(UnmanagedType.Bool)]
            internal static extern bool SetWindowPos(int hWnd, int hWndInsertAfter, int X, int Y, int cx, int cy, int uFlags);

            [DllImport("user32.dll")]
            [return: MarshalAs(UnmanagedType.Bool)]
            internal static extern bool GetWindowRect(IntPtr hWnd, out RECT lpRect);
        }
    }
    public class DelegateCommand : ICommand
    {
        private Action action;
        private Action<object> actionT;
        private Predicate<object> _canExecute;

        public DelegateCommand(Action action)
        {
            this.action = action;
        }

        public DelegateCommand(Action<object> execute) : this(execute, null) { }
        public bool CanExecute(object parameter)
        {
            //return true;
            return _canExecute?.Invoke(parameter) ?? true;
        }
        public DelegateCommand(Action<object> execute, Predicate<object> canExecute)
        {
            actionT = execute ?? throw new ArgumentNullException(nameof(execute));
            _canExecute = canExecute;
        }
        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public void Execute(object parameter)
        {
            if (action != null)
            {
                action();
            }
            if (actionT != null)
            {
                actionT.Invoke(parameter);
            }
        }
        public void ShowDialog(string key, object vm)
        {
            WindowManager.ShowDialog(key, vm);
        }

        public void ShowMessage(string mes, string title = "", MessageBoxButton buttons = MessageBoxButton.OK)
        {
            MessageBox.Show(mes, title, buttons);
        }
    }

    [ValueConversion(typeof(bool), typeof(bool))]
    public class InverseBooleanConverter : IValueConverter
    {
        #region IValueConverter Members

        public object Convert(object value, Type targetType, object parameter,
             System.Globalization.CultureInfo culture)
        {
            if (targetType != typeof(bool))
                throw new InvalidOperationException("The target must be a boolean");

            return !(bool)value;
        }

        public object ConvertBack(object value, Type targetType, object parameter,
            System.Globalization.CultureInfo culture)
        {
            throw new NotSupportedException();
        }

        #endregion
    }
    [ValueConversion(typeof(bool), typeof(bool))]
    public class CheckedConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return value.ToString() == parameter.ToString();
            
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            //bool isChecked = (bool)value;
            //if (!isChecked)
            //{
            //    return null;
            //}

            return parameter.ToString();

        }
    }
    public class BindableRichTextBox : RichTextBox
    {
        public new FlowDocument Document
        {
            get { return (FlowDocument)GetValue(DocumentProperty); }
            set { SetValue(DocumentProperty, value); }
        }
        // Using a DependencyProperty as the backing store for Document.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty DocumentProperty =
            DependencyProperty.Register("Document", typeof(FlowDocument), typeof(BindableRichTextBox), new FrameworkPropertyMetadata(null, new PropertyChangedCallback(OnDucumentChanged)));
        private static void OnDucumentChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            RichTextBox rtb = (RichTextBox)d;
            rtb.Document = (FlowDocument)e.NewValue;
        }
    }
}
