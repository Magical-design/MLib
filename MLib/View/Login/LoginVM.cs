using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MLib.View
{
    public class LoginVM:ViewBase
    {
        private bool result=false;
        public bool Result
        {
            get { return result; }  
            set { result = value; }
        }
        private string passwordSetVal;
        public string PasswordSetVal
        {
            get { return passwordSetVal; }
            set { passwordSetVal = value; }
        }

        private string message; 
        public string Message
        {
            get { return message; }
            set
            {
                message = value;
                OnPropertyChanged("Message");
            }
        }
        private string password;
        public string Password
        {
            get { return password; }
            set { password = value; OnPropertyChanged("Password");}
        }
        public event EventHandler LoginCompleteEvent;

        public DelegateCommand btLoginClick
        {
            get
            {
                return new DelegateCommand(() =>
                {
                    Login();
                }
              );
            }
        }
        private void  Login()
        {
            Result = PasswordSetVal == Password;
            if (Result)
                Message = "密码正确";
            else
                Message = "密码错误";
            LoginCompleteEvent?.Invoke(Result, null);
        }


        public LoginVM()
        {
            PasswordSetVal = "123";
        }
    }
}
