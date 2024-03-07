using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Diagnostics;

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
        static public async Task<bool> CreateFileAsy(string mPath, string mFileName)
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
        static public void DeleteFile(string mPath, string mFileName)
        {
            string mFliePath = mPath + mFileName;
            if (File.Exists(mFliePath) || mFileName == "")
            {
                try
                {
                    File.Delete(mFliePath);
                }
                catch (Exception ex)
                { 
                    Trace.WriteLine(ex);
                }
            }

        }

        /// <summary>
        /// 创建文件夹/文件
        /// </summary>
        /// <param name="mPath">路径</param>
        /// <param name="mFileName">文件名</param>
        /// <returns></returns>
        static public bool CreateFile(string mPath, string mFileName)
        {
            bool mPathR = false, mFileNameR = false;
            string mFliePath = mPath + mFileName;
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
                    mFileNameR = true;
                else
                {
                    try
                    {

                        File.Create(mFliePath).Close();
                        while (!File.Exists(mFliePath))
                        {
                            Task.Delay(10);
                            Console.WriteLine("正在创建" + mFliePath + "文件");
                        }

                        mFileNameR = true;
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e);
                        mFileNameR = false;
                    }
                }
                return mPathR && mFileNameR;
        }
        public static void CopyFileOnly(string souce, string target)
        {
            System.IO.FileInfo file = new FileInfo(souce);
            System.IO.FileInfo file1 = new FileInfo(target);
            string destDirectoryFullName = target.Replace(file1.Name, "");
            if (!System.IO.Directory.Exists(destDirectoryFullName))
            {
                System.IO.Directory.CreateDirectory(destDirectoryFullName);
            }

            file.CopyTo(target, true);
            
        }

        public static void MoveFileOnly(string souce, string target)
        {
            System.IO.FileInfo file = new FileInfo(souce);

            string destDirectoryFullName = target.Replace(file.Name, "");
            if (!System.IO.Directory.Exists(destDirectoryFullName))
            {
                System.IO.Directory.CreateDirectory(destDirectoryFullName);
            }
            file.MoveTo(target + file.Name);
        }
        public static void MoveFileOnly(string souce, string target,string newName)
        {
            System.IO.FileInfo file = new FileInfo(souce);

            string destDirectoryFullName = target.Replace(file.Name, "");
            if (!System.IO.Directory.Exists(destDirectoryFullName))
            {
                System.IO.Directory.CreateDirectory(destDirectoryFullName);
            }
            file.MoveTo(target + newName);
        }
        public static void copyDirectory(string sPath, string dPath)
        {
            string[] directories = System.IO.Directory.GetDirectories(sPath);
            if (!System.IO.Directory.Exists(dPath))
            {
                System.IO.Directory.CreateDirectory(dPath);
            }

            System.IO.DirectoryInfo dir = new System.IO.DirectoryInfo(sPath);
            System.IO.DirectoryInfo[] dirs = dir.GetDirectories();
            CopyFile(dir, dPath);
            if (dirs.Length > 0)
            {
                foreach (System.IO.DirectoryInfo temDirectoryInfo in dirs)
                {
                    string sourceDirectoryFullName = temDirectoryInfo.FullName;
                    string destDirectoryFullName = sourceDirectoryFullName.Replace(sPath, dPath);
                    if (!System.IO.Directory.Exists(destDirectoryFullName))
                    {
                        System.IO.Directory.CreateDirectory(destDirectoryFullName);
                    }
                    CopyFile(temDirectoryInfo, destDirectoryFullName);
                    copyDirectory(sourceDirectoryFullName, destDirectoryFullName);
                }
            }

        }
        /// <summary>
        /// 拷贝目录下的所有文件到目的目录。
        /// </summary>
        /// <param >源路径</param>
        /// <param >目的路径</param>
        public static void CopyFile(System.IO.DirectoryInfo path, string desPath)
        {
            string sourcePath = path.FullName;
            System.IO.FileInfo[] files = path.GetFiles();
            foreach (System.IO.FileInfo file in files)
            {
                string sourceFileFullName = file.FullName;
                string destFileFullName = sourceFileFullName.Replace(sourcePath, desPath);
                file.CopyTo(destFileFullName, true);
            }
        }



    }
}
