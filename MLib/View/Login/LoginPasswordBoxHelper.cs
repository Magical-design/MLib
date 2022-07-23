using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace MLib.View.Login
{
    public  class LoginPasswordBoxHelper : DependencyObject
    {
        public static string GetPassword(DependencyObject obj)
        {
            return (string)obj.GetValue(PasswordProperty);
        }

        public static void SetPassword(DependencyObject obj, string value)
        {
            obj.SetValue(PasswordProperty, value);
        }

        public static readonly DependencyProperty PasswordProperty =
            DependencyProperty.RegisterAttached("Password", typeof(string), typeof(LoginPasswordBoxHelper), new PropertyMetadata(""));

        public static bool GetIsPasswordBindingEnable(DependencyObject obj)
        {
            return (bool)obj.GetValue(IsPasswordBindingEnableProperty);
        }

        public static void SetIsPasswordBindingEnable(DependencyObject obj, bool value)
        {
            obj.SetValue(IsPasswordBindingEnableProperty, value);
        }

        public static readonly DependencyProperty IsPasswordBindingEnableProperty =
            DependencyProperty.RegisterAttached("IsPasswordBindingEnable", typeof(bool), typeof(LoginPasswordBoxHelper),
                new FrameworkPropertyMetadata(OnIsPasswordBindingEnabledChanged));

        private static void OnIsPasswordBindingEnabledChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {
            var passwordBox = obj as PasswordBox;
            if (passwordBox != null)
            {
                passwordBox.PasswordChanged -= PasswordBoxPasswordChanged;
                if ((bool)e.NewValue)
                {
                    passwordBox.PasswordChanged += PasswordBoxPasswordChanged;
                }
            }
        }

        static void PasswordBoxPasswordChanged(object sender, RoutedEventArgs e)
        {
            var passwordBox = (PasswordBox)sender;
            if (!String.Equals(GetPassword(passwordBox), passwordBox.Password))
            {
                SetPassword(passwordBox, passwordBox.Password);
            }
        }
        public static bool GetIsMonitoring(DependencyObject obj)
        {
            return (bool)obj.GetValue(IsMonitoringProperty);
        }

        public static void SetIsMonitoring(DependencyObject obj, bool value)
        {
            obj.SetValue(IsMonitoringProperty, value);
        }

        public static readonly DependencyProperty IsMonitoringProperty =
            DependencyProperty.RegisterAttached("IsMonitoring", typeof(bool), typeof(LoginPasswordBoxHelper), new UIPropertyMetadata(false, OnIsMonitoringChanged));



        public static int GetPasswordLength(DependencyObject obj)
        {
            return (int)obj.GetValue(PasswordLengthProperty);
        }

        public static void SetPasswordLength(DependencyObject obj, int value)
        {
            obj.SetValue(PasswordLengthProperty, value);
        }

        public static readonly DependencyProperty PasswordLengthProperty =
            DependencyProperty.RegisterAttached("PasswordLength", typeof(int), typeof(LoginPasswordBoxHelper), new UIPropertyMetadata(0));

        private static void OnIsMonitoringChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var pb = d as PasswordBox;
            if (pb == null)
            {
                return;
            }
            if ((bool)e.NewValue)
            {
                pb.PasswordChanged += PasswordChanged;
            }
            else
            {
                pb.PasswordChanged -= PasswordChanged;
            }
        }

        private static void PasswordChanged(object sender, RoutedEventArgs e)
        {
            var pb = sender as PasswordBox;
            if (pb == null)
            {
                return;
            }
            SetPasswordLength(pb, pb.Password.Length);
        }
    }
}
