using Microsoft.Win32;
using Shell32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.DirectoryServices.AccountManagement;
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

namespace WpfApp1
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class UserWindow : Window
    {
        public User user;

        ObservableCollection<Program> programsLeft;
        ObservableCollection<Program> programsRight;
        string pathToApps = @"C:\ProgramData\Microsoft\Windows\Start Menu\Programs";

        public UserWindow(User user)
        {
            InitializeComponent();
            this.user = user;


            if (user != null)
            {
                UserNameBox.Text = user.name;
            }

            programsLeft = new ObservableCollection<Program>();
            programsRight = new ObservableCollection<Program>();


            programsListBoxLeft.ItemsSource = programsLeft;
            programsListBoxRight.ItemsSource = programsRight;
            getApps(pathToApps);
            programsListBoxLeft.Items.Refresh();
            programsListBoxRight.Items.Refresh();
        }

        public void getPrograms()
        {
            string uninstallKey = @"SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall";
            using (RegistryKey rk = Registry.LocalMachine.OpenSubKey(uninstallKey))
            {
                foreach (string skName in rk.GetSubKeyNames())
                {
                    using (RegistryKey sk = rk.OpenSubKey(skName))
                    {
                        try
                        {

                            Program p = new Program();
                            p.name = sk.GetValue("DisplayName").ToString();
                            //var displayName = sk.GetValue("DisplayName");
                            //var size = sk.GetValue("EstimatedSize");
                            programsLeft.Add(p);
                            //Console.WriteLine(displayName + "\t: " + size);

                        }
                        catch (Exception ex)
                        {
                        }
                    }
                }
                //label1.Text += " (" + lstDisplayHardware.Items.Count.ToString() + ")";
                programsListBoxLeft.Items.Refresh();
                programsListBoxRight.Items.Refresh();
            }
        }


        public void deleteUser_Click(object sender, EventArgs e)
        {
            if (user != null)
            {
                user.Delete();
                returnToMainWindow_Click(null, null);
            }
        }

        void OK_Click(object sender, EventArgs e)
        {
            user.programsLeft = new ObservableCollection<Program>(this.programsLeft);
            user.programsRight = new ObservableCollection<Program>(this.programsRight);
            user.checkBlockList();
            returnToMainWindow_Click(null, null);
        }

        void returnToMainWindow_Click(object sender, EventArgs e)
        {
            MainWindow window = (MainWindow)Application.Current.MainWindow;
            window.Refresh();
            this.Close();
            window.Show();

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }

        void MoveToTheRight_Click(object sender, RoutedEventArgs e)
        {
            if (programsListBoxLeft.SelectedItems.Count > 0)
            {


                List<Program> progr2 = new List<Program>();

                foreach (var v in programsListBoxLeft.SelectedItems)
                {
                    progr2.Add((Program)v);
                    programsRight.Add((Program)v);
                }

                foreach (var v in progr2)
                {
                    programsLeft.Remove(v);
                }

                programsListBoxLeft.Items.Refresh();
                programsListBoxRight.Items.Refresh();

            }

        }

        void MoveToTheLeft_Click(object sender, RoutedEventArgs e)
        {
            if (programsListBoxRight.SelectedItems.Count > 0)
            {


                List<Program> progr2 = new List<Program>();

                foreach (var v in programsListBoxRight.SelectedItems)
                {
                    progr2.Add((Program)v);
                    programsLeft.Add((Program)v);
                }

                foreach (var v in progr2)
                {
                    programsRight.Remove(v);
                }



                programsListBoxLeft.Items.Refresh();
                programsListBoxRight.Items.Refresh();

            }

        }

        void MoveAllToTheRight_Click(object sender, RoutedEventArgs e)
        {
            foreach (var v in programsLeft)
            {
                programsRight.Add(v);
            }

            programsLeft.Clear();

            programsListBoxLeft.Items.Refresh();
            programsListBoxRight.Items.Refresh();

        }

        void MoveAllToTheLeft_Click(object sender, RoutedEventArgs e)
        {
            foreach (var v in programsRight)
            {
                programsLeft.Add(v);
            }

            programsRight.Clear();

            programsListBoxLeft.Items.Refresh();
            programsListBoxRight.Items.Refresh();

        }

        private void getApps(string path)
        {
            DirectoryInfo directory = new DirectoryInfo(path);
            DirectoryInfo[] dirs = directory.GetDirectories();
            foreach (var dir in dirs)
            {
                StringBuilder sb = new StringBuilder(path);
                sb.Append(@"\" + dir.Name);
                getApps(sb.ToString());
            }
            FileInfo[] files = directory.GetFiles("*.lnk");
            foreach (FileInfo file in files)
            {
                Program program = new Program();
                StringBuilder sb = new StringBuilder(file.Name);
                string location = GetShortcutTargetFile(path + @"\" + sb.ToString());
                if (System.IO.Path.GetExtension(location) != ".exe" || !File.Exists(location))
                    continue;
                int i = user.checkAccess(location);
                if (i == 0)
                    continue;
                program.fileLocation = location;
                sb.Length -= 4;
                program.name = sb.ToString();
                if (program.fileLocation != string.Empty)
                    if (i == 10)
                        programsLeft.Add(program);
                    else if (i == 100)
                        programsRight.Add(program);
            }
        }



        public static string GetShortcutTargetFile(string shortcutFilename)
        {
            if (shortcutFilename == "")
                return string.Empty;
            string pathOnly = System.IO.Path.GetDirectoryName(shortcutFilename);
            string filenameOnly = System.IO.Path.GetFileName(shortcutFilename);

            Shell shell = new Shell();
            Folder folder = shell.NameSpace(pathOnly);
            FolderItem folderItem = folder.ParseName(filenameOnly);
            if (folderItem != null)
            {
                Shell32.ShellLinkObject link;
                try
                {
                    link = (Shell32.ShellLinkObject)folderItem.GetLink;
                }
                catch (Exception x)
                {
                    return string.Empty;
                }

                return link.Path;
            }

            return string.Empty;
        }

    }
}

