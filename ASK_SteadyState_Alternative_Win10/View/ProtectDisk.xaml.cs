using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.DirectoryServices.AccountManagement;
using System.DirectoryServices.Protocols;
using System.IO;
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
using System.Windows.Shapes;
namespace WpfApp1.View
{
    /// <summary>
    /// Interaction logic for ProtectDisk.xaml
    /// </summary>
    public partial class ProtectDisk : Window
    {
        PrincipalContext ctx;
        GroupPrincipal group;
        ObservableCollection<User> usersList;
        string path = @"C:\Users";
        string destinationPath = @"C:\Users\Administrator\Documents";
        string[] tab = { "Documents","Downloads", "Desktop" };
        public ProtectDisk(PrincipalContext ctx, GroupPrincipal group)
        {
            InitializeComponent();
            this.ctx = ctx;
            this.group = group;
            usersList = new ObservableCollection<User>();
            setList();
        }

        void setList()
        {
            PrincipalSearchResult<Principal> result = group.GetMembers();
            foreach (UserPrincipal u in result)
            {
                User user = new User(u);
                usersList.Add(user);
            }
            users.ItemsSource = usersList;
        }

        private void freeze_Click(object sender, RoutedEventArgs e)
        {
            foreach(User u in users.SelectedItems)
            {
                string realPath = path + @"\" + u.name;
                string destinationRealPath = destinationPath+@"\"+u.name;
                foreach (string dirName in tab)
                {
                    string lastPath = realPath + @"\" + dirName;
                    if (Directory.Exists(lastPath) == false)
                        continue;
                    try
                    {
                        CopyFolder(lastPath, destinationRealPath, dirName);
                    }catch(Exception x) { }
                    

                }
                    
            }
            

        }

        public void CopyFolder(string sourceFolder, string destFolderTemp, string dirName)
        {
            string destFolder;
            if (dirName != string.Empty)
                destFolder = destFolderTemp + @"\" + dirName;
            else
                destFolder = destFolderTemp;
            if (!Directory.Exists(destFolder))
                Directory.CreateDirectory(destFolder);
            else
            {
                DirectoryInfo dir = new DirectoryInfo(destFolderTemp);
                RecursiveDelete(dir, true);
                Directory.CreateDirectory(destFolder);
            }
                
            string[] files = Directory.GetFiles(sourceFolder);
            foreach (string file in files)
            {   
                try
                {
                    string name = System.IO.Path.GetFileName(file);
                    string dest = System.IO.Path.Combine(destFolder, name);
                    File.Copy(file, dest);
                }catch(Exception x) { }
                
            }
            string[] folders = Directory.GetDirectories(sourceFolder);
            foreach (string folder in folders)
            {
                try
                {
                    string name = System.IO.Path.GetFileName(folder);
                    string dest = System.IO.Path.Combine(destFolder, name);
                    CopyFolder(folder, dest, "");
                }catch(Exception x) {}
                
            }
        }

        private void restore_Click(object sender, RoutedEventArgs e)
        {
            

            foreach (User u in users.SelectedItems)
            {
                string realPath = path + @"\" + u.name;
                string destinationRealPath = destinationPath + @"\" + u.name;
                if (!Directory.Exists(destinationRealPath))
                {
                    MessageBox.Show("You have to create backup(freeze) before restore");
                    continue;
                }
                foreach (string dirName in tab)
                {
                    string finalPath = realPath + @"\" + dirName;
                    DirectoryInfo dir = new DirectoryInfo(finalPath);
                    try
                    {
                        RecursiveDelete(dir, false);
                    }catch(Exception x) { }
                    try
                    {
                        CopyFolder(destinationRealPath+@"\"+dirName, finalPath, "");
                    }
                    catch(Exception x) { }
                    
                }

            }
        }

        public void RecursiveDelete(DirectoryInfo baseDir, bool rm)
        {
            if (!baseDir.Exists)
                return;

            foreach (var dir in baseDir.EnumerateDirectories())
            {
                try
                {
                    RecursiveDelete(dir, true);
                }catch (Exception x) { }
                
            }
            var files = baseDir.GetFiles();
            foreach (var file in files)
            {
                try
                {
                    file.IsReadOnly = false;
                    file.Delete();
                }catch(Exception x) { }
                
            }
            if (rm)
            {
                try
                {
                    baseDir.Delete();
                }catch(Exception x) { }
                
            }
                
        }
    }
}
