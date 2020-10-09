using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace MLib.MFile

{
    public class MFile
    {
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
                              Task.Delay(3000);
                              Console.WriteLine("正在创建" + mPath + "文件夹");
                          }
                          mPathR = true;
                      }
                      catch (Exception) { };

                  }
                  else
                      mPathR = true;


                  if (File.Exists(mFliePath) || mFliePath=="")
                      mFileNameR= true;
                  else
                  {
                      try
                      {
                        
                          File.Create(mFliePath).Close();
                          while (!File.Exists(mFliePath))
                          {
                              Task.Delay(3000);
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
