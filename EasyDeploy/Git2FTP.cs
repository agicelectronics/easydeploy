using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using static System.Net.WebRequestMethods;

namespace EasyDeploy
{
    class Git2FTP

    {
        private string path=Directory.GetCurrentDirectory()+"\\Repos\\";

        //---------- Cloning Git-------------------------------
        public void Clone(string strIn, string toDir)
        {
            try
            {
                string str = "git clone" + strIn;
                System.Diagnostics.ProcessStartInfo p = new System.Diagnostics.ProcessStartInfo("cmd.exe", "/C " + str + toDir);
                System.Diagnostics.Process proc = new System.Diagnostics.Process();

                p.WindowStyle = ProcessWindowStyle.Hidden;
                proc.StartInfo = p;
                proc.Start();
                proc.WaitForExit();
                proc.Close();
            }catch (Exception e )
            {
               //
            }
        }


        //------------ Saving Settings------------------------
        public bool AddNewTask(string mydir,string gitRepo,string gitUser,string gitPass,string ftpServer,string ftpUser,string ftpPass,string ftpPort,string chkAutoClone, string chkAutoDeploy)
        {
            string path = Directory.GetCurrentDirectory();
            try
            {
                    string text = gitRepo + "_" + gitUser + "_" + gitPass + "_" + ftpServer + "_" + ftpUser + "_" + ftpPass + "_" + ftpPort + "_"+chkAutoClone + "_"+chkAutoDeploy+ "_";
                    System.IO.File.WriteAllText(@"" + path + "\\Repos\\" + mydir + "\\Config.txt ", text);
            }
            catch (Exception ex)
            {
                //
            }
           
            return true;
        }

        //--------------Sending to FTP------------------------------
        public void CreateFtpDir()
        {
            string filename = "Config.txt";
            string server = "ftp://www58.world4you.com/emir/";
            string user = "ftp3435435_ftpcsharp";
            string pass = "EmirZlata12";

            WebRequest request = WebRequest.Create(server);
            request.Method = WebRequestMethods.Ftp.MakeDirectory;
            request.Credentials = new NetworkCredential(user, pass);

          /*  using (var resp = (FtpWebResponse)request.GetResponse())
            {
               MesageBox.Show(resp.StatusCode);
            }*/
        }
        public void SendToFTP()
        {
            // Server:  www58.world4you.com
            // User: ftp3435435_ftpcsharp
            // Pass: EmirZlata12
            //
            string filename = "Config.txt";
            string server = "ftp://www58.world4you.com/" +filename;
            string user = "ftp3435435_ftpcsharp";
            string pass = "EmirZlata12";
            //TODO send selected local data/the right version to FTP
            string localPath = @""+path;
            string fileName = "Config.txt";
            Console.WriteLine(path);
            FtpWebRequest requestFTPUploader = (FtpWebRequest)WebRequest.Create(server);
            requestFTPUploader.Credentials = new NetworkCredential(user, pass);
           
            
           requestFTPUploader.Method = WebRequestMethods.Ftp.UploadFile;

            FileInfo fileInfo = new FileInfo(localPath + fileName);
            FileStream fileStream = fileInfo.OpenRead();

            int bufferLength = 2048;
            byte[] buffer = new byte[bufferLength];

            Stream uploadStream = requestFTPUploader.GetRequestStream();
            int contentLength = fileStream.Read(buffer, 0, bufferLength);

            while (contentLength != 0)
            {
                uploadStream.Write(buffer, 0, contentLength);
                contentLength = fileStream.Read(buffer, 0, bufferLength);
            }

            uploadStream.Close();
            fileStream.Close();

            requestFTPUploader = null;

            
        }
    }
}
