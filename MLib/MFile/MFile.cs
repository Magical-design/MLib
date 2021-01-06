using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace MLib

{
    public class MFile
    {
        /// <summary>
        /// 获取模块的完整路径。
        /// </summary>
        static public string path1 = System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName;
        /// <summary>
        /// 获取和设置当前目录(该进程从中启动的目录)的完全限定目录，最后不包含“\”；
        /// </summary>
        static public string path2 = System.Environment.CurrentDirectory;
        /// <summary>
        /// 获取应用程序的当前工作目录，最后不包含“\”；
        /// </summary>
        static public string path3 = System.IO.Directory.GetCurrentDirectory();
        /// <summary>
        /// 获取程序的基目录，最后包含“\”；
        /// </summary>
        static public string path4 = System.AppDomain.CurrentDomain.BaseDirectory;
        /// <summary>
        /// 获取和设置包括该应用程序的目录的名称
        /// </summary>
        static public string path5 = System.AppDomain.CurrentDomain.SetupInformation.ApplicationBase;
        /// <summary>
        /// 创建文件夹/文件
        /// </summary>
        /// <param name="mPath">路径</param>
        /// <param name="mFileName">文件名</param>
        /// <returns></returns>
        static public async Task<bool> CreateFile(string mPath, string mFileName)
        {
            bool mPathR = false, mFileNameR = false;
            string mFliePath = mPath+ mFileName;
             return  await Task.Run(() =>
              {
                  if (!Directory.Exists(mPath) && mPath != "")
                  {                     
                      try
                      {
                          Directory.CreateDirectory(mPath);
                          while (!Directory.Exists(mPath))
                          {
                              Task.Delay(10);
                              Console.WriteLine("正在创建" + mPath + "文件夹");
                          }
                          mPathR = true;
                      }
                      catch (Exception) { };

                  }
                  else
                      mPathR = true;


                  if (File.Exists(mFliePath) || mFileName == "")
                      mFileNameR= true;
                  else
                  {
                      try
                      {
                        
                          File.Create(mFliePath).Close();
                          while (!File.Exists(mFliePath))
                          {
                              Task.Delay(10);
                              Console.WriteLine("正在创建" +  mFliePath + "文件");
                          }

                          mFileNameR =true;
                      }
                      catch (Exception e)
                      {
                          Console.WriteLine(e);
                          mFileNameR= false;
                      }
                  }
                  return mPathR && mFileNameR;
              });
        }


    }
}
