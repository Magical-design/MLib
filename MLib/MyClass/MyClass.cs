using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;

namespace MLib.MyClass
{
    public class MyClass
    {
        public static Boolean CheckStrName(string StrName)

        {

            StringBuilder description = new StringBuilder();

            Boolean opResult = Regex.IsMatch(StrName, @"(?!((^(con)$)|^(con)\\..*|(^(prn)$)|^(prn)\\..*|(^(aux)$)|^(aux)\\..*|(^(nul)$)|^(nul)\\..*|(^(com)[1-9]$)|^(com)[1-9]\\..*|(^(lpt)[1-9]$)|^(lpt)[1-9]\\..*)|^\\s+|.*\\s$)(^[^\\\\\\/\\:\\<\\>\\*\\?\\\\\\""\\\\|]{1,255}$)");

            if (!opResult)

            {

                description.Append("文件名包含特殊符或系统关键字！");

            }



            if (description.Length > 0)

            {

                MessageBox.Show(description.ToString());

            }

            return opResult;

        }

    }
}
