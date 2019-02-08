using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
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

namespace EasyDeploy
{

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            CreateDir("Repos");
            LoadDirs();
           

            // add some data to combo box version
            cmbVersions.Items.Add("Version1");
            cmbVersions.Items.Add("Version2");
            cmbVersions.Items.Add("Version3");
            cmbVersions.Items.Add("Version4");
            cmbVersions.Items.Add("Version5");
            cmbVersions.Items.Add("Version6");
            //---------------------------------------

        }
        public void LoadDirs()
        {
          
            lstJobs.Items.Clear();
            
            string path1 = Directory.GetCurrentDirectory();
            string[] dirs = Directory.GetDirectories(@"" + path1+"\\Repos\\");
                   
            for (int i = 0; i < dirs.Length; i++)
            {
                string dirname = new DirectoryInfo(dirs[i]).Name;
                lstJobs.Items.Add(dirname);
            }
           
        }

        // git clone https://username:password@github.com/username/repository.git
        // https://github.com/agicelectronics/stylenius.git ;           Ovo je moj test repo
        // https://github.com/agicelectronics/testrepo.git      ovo je jos jedan test repo samo jedan html website
       
        string path = Directory.GetCurrentDirectory();
        public string repo;
        private void ReadSettings()
        {
            try
            {
                string path3 = Directory.GetCurrentDirectory();
                //Open the stream and read it back.
                using (FileStream fs = File.OpenRead(path3 + "\\Repos\\" + lstJobs.SelectedItem + "\\Config.txt"))
                {
                    byte[] b = new byte[1024];
                    UTF8Encoding temp = new UTF8Encoding(true);
                    while (fs.Read(b, 0, b.Length) > 0)
                    {
                        // MessageBox.Show(temp.GetString(b));
                        //  Console.WriteLine(temp.GetString(b));
                    //    SetData(temp.GetString(b));

                    }
                }
            }catch (Exception e)
            {

            }
        }
        private void SetData(string data)
        {
            string[] temp = data.Split('_');
            txtNewGitRepo.Text = temp[0];
            txtNewGitUser.Text = temp[1];
            txtNewGitPass.Text = temp[2];
            txtNewFtpServer.Text = temp[3];
            txtNewFtpUsername.Text = temp[4];
            txtNewFtpPass.Text = temp[5];
            txtNewFtpPort.Text = temp[6];
            txtSelectedRepo.Content = temp[0];
         //   chkAutoClone.isChecked(Boolean.Parse(temp[7]));
            txtNewFtpPort.Text = temp[8];
        }
        
        public void Git2ftp(string repo)    // Clone method
        {
           
            //get directory from repo

           string myDir= GetRepoName(repo);
            //
            DateTime date1 = DateTime.Now;
            string datum = date1.ToString("dd_MM_yyyy_HH_mm");
            string toDirectory = " " + path + "\\Repos\\"+myDir+"\\" + datum + "\\";
            Git2FTP g2ftp = new Git2FTP();
            g2ftp.Clone(repo, toDirectory);
            lstJobs.Items.Clear();
            LoadDirs();
          
        }
       
        private string GetRepoName(string reponame)
        {
           
            if (reponame != null)
            {
                string[] temp = reponame.Split('/');

                for (int i = 0; i < temp.Length; i++)
                {

                    if (temp[i].Contains(".git"))
                    {
                        string[] temp2 = temp[i].Split('.');
                        return temp2[0];

                    }

                }
            }
            return "";


        }
        private void Button_Click(object sender, RoutedEventArgs e)  // expand addnew
        {
            grdAddNew.Height = 240;
          
           
        }

        private void BtnSave_Click(object sender, RoutedEventArgs e)  // btnSave
        {
            lstJobs.Items.Clear();
            LoadDirs();
            CreateDir("\\Repos\\"+GetRepoName(txtNewGitRepo.Text));
           
               
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)  // btnClone
        {
            Git2ftp(" "+txtNewGitRepo.Text+" ");
        }

        public void CreateDir(string dirname)
        {
            string myDir = path + "\\" + dirname;
            DirectoryInfo di = Directory.CreateDirectory(myDir);

            if(dirname != "Repos")
            {
                AddNewTask();

            }
            lstJobs.Items.Clear();
            LoadDirs();
            grdAddNew.Height = 0;
        }
        private void AddNewTask()
        {


            Git2FTP g2ftp = new Git2FTP();
            g2ftp.AddNewTask(GetRepoName(txtNewGitRepo.Text), txtNewGitRepo.Text, txtNewGitUser.Text, txtNewGitPass.Text, txtNewFtpServer.Text, txtNewFtpUsername.Text, txtNewFtpPass.Text, txtNewFtpPort.Text,chkAutoClone.Content.ToString(),chkAutoDeploy.Content.ToString());

            lstJobs.Items.Clear();
            LoadDirs();
        }

        private void LstJobs_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ReadSettings();

        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
           
            string server = "ftp://www58.world4you.com/emir/aga";
            string user = "ftp3435435_ftpcsharp";
            string pass = "EmirZlata12";

            WebRequest request = WebRequest.Create(server);
            request.Method = WebRequestMethods.Ftp.MakeDirectory;
            request.Credentials = new NetworkCredential(user, pass);
            MessageBox.Show("Dir created");
        }

        private void ChkAutoDeploy_UnChecked(object sender, RoutedEventArgs e)
        {

        }

        private void ChkAutoClone_UnChecked(object sender, RoutedEventArgs e)
        {

        }
       
        /*  
private void ChkAutoClone_Checked(object sender, RoutedEventArgs e)
{
if (chkAutoClone.IsChecked==true)
{
Git2FTP g2ftp = new Git2FTP();
//    g2ftp.AddNewTask(GetRepoName(txtSelectedRepo.Content.ToString()), txtSelectedRepo.Content.ToString(), txtNewGitUser.Text, txtNewGitPass.Text, txtNewFtpServer.Text, txtNewFtpUsername.Text, txtNewFtpPass.Text, txtNewFtpPort.Text, chkAutoClone.IsChecked.ToString(), chkAutoDeploy.IsChecked.ToString());
MessageBox.Show(chkAutoClone.IsChecked.ToString() + " " + chkAutoDeploy.IsChecked.ToString());
}
if (chkAutoClone.IsChecked == false)
{
Git2FTP g2ftp = new Git2FTP();
//   g2ftp.AddNewTask(GetRepoName(txtSelectedRepo.Content.ToString()), txtSelectedRepo.Content.ToString(), txtNewGitUser.Text, txtNewGitPass.Text, txtNewFtpServer.Text, txtNewFtpUsername.Text, txtNewFtpPass.Text, txtNewFtpPort.Text, chkAutoClone.IsChecked.ToString(), chkAutoDeploy.IsChecked.ToString());
MessageBox.Show(chkAutoClone.IsChecked.ToString() + " " + chkAutoDeploy.IsChecked.ToString());
}

}
*/
public string GetDir()
        {
            string myFolder = "";
            DirectoryInfo d = new DirectoryInfo(@""+path+"\\Repos\\myrepo\\repomy\\");//Assuming Test is your Folder
            FileInfo[] Files = d.GetFiles("*.*"); //Getting Text files
            string str = "";
            foreach (FileInfo file in Files)
            {
              
             //   str = str + " " + file.Name;
                str = str + " " + file.DirectoryName;
                string[] temp = str.Split('\\');

               for (int i = 0; i < temp.Length; i++)
                {
                    if (temp[i] == "Repos")
                    {
                        for (int k = i + 1; k < temp.Length; k++)
                        {
                            myFolder +="\\"+ temp[k];
                            
                        }
                       
                    }

                   
                }
             
            }
            return myFolder;
        }
       
    }

}
