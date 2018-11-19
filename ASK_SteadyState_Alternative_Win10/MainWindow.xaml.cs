using System;
using System.Collections.Generic;
using System.Windows;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.DirectoryServices;
using System.DirectoryServices.AccountManagement;
using System.Windows.Input;
using System.Windows.Controls;
using System.Text;
using ASK_SteadyState_Alternative_Win10;

namespace ASK_SteadyState_Alternative.TreeView_control
{
    public partial class MainWindow : Window
    {
        private StreamReader runCommandLine(string command)
        {
            Process cmd = new Process();
            cmd.StartInfo.FileName = "cmd.exe";
            cmd.StartInfo.RedirectStandardInput = true;
            cmd.StartInfo.RedirectStandardOutput = true;
            cmd.StartInfo.CreateNoWindow = true;
            cmd.StartInfo.UseShellExecute = false;
            cmd.Start();

            cmd.StandardInput.WriteLine(command);
            cmd.StandardInput.Flush();
            cmd.StandardInput.Close();

            StreamReader reader = cmd.StandardOutput;
            cmd.Close();

            return reader;
        }

       
        public MainWindow()
        {
            InitializeComponent();
            refreshTree();
        }

        public void treeView_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            var tree = (TreeView)sender;
            var selectedItem = tree.SelectedItem;

            if (selectedItem.GetType() == typeof(Group))
            {
                Group temp = (Group)selectedItem;
                textBoxInformations.Text = temp.Describe();
                editButton.Visibility = Visibility.Visible;
                deleteButton.Visibility = Visibility.Visible;
                addUserButton.Visibility = Visibility.Visible;
            }

            else if (selectedItem.GetType() == typeof(User))
            {
                User temp = (User)selectedItem;
                textBoxInformations.Text = temp.Describe();
                editButton.Visibility = Visibility.Visible;
                deleteButton.Visibility = Visibility.Visible;
                addUserButton.Visibility = Visibility.Hidden;
            }
        }

        /*     private void treeView_MouseDown(object sender, MouseButtonEventArgs e)
             {
                 var tree = (TreeView)sender;
                 var selectedItem = tree.SelectedItem;
                 if (selectedItem == null)
                     return;

                 if (e.RightButton == MouseButtonState.Pressed)
                 {
                     if (selectedItem.GetType() == typeof(Group))
                     {

                     }
                     else if(selectedItem.GetType() == typeof(User))
                     {

                     } 
                 }
             }*/

        private void addNewGroup_Click(object sender, RoutedEventArgs e)
        {
            AddNewGroup window = new AddNewGroup(this);
            window.Show();
        }

        public void refreshTree()
        {
            List<Group> groups = new List<Group>();

            PrincipalContext ctx = new PrincipalContext(ContextType.Machine);
            GroupPrincipal gr = new GroupPrincipal(ctx);
            PrincipalSearcher ps = new PrincipalSearcher();


            ps.QueryFilter = new GroupPrincipal(ctx);
            PrincipalSearchResult<Principal> list = ps.FindAll();

            foreach (var v in list)
            {
                GroupPrincipal gp = (GroupPrincipal)v;
                Group newGroup = new Group() { Name = v.Name, principal = gp };

                PrincipalSearchResult<Principal> grList = gp.GetMembers();

                foreach (var s in grList)
                    newGroup.Members.Add(new User() { Name = s.Name, principal = s });
                groups.Add(newGroup);

            }
            treeView.ItemsSource = groups;
        }

        private void editButton_Click(object sender, RoutedEventArgs e)
        {
            User user = (User)this.treeView.SelectedItem;
            EditUser window = new EditUser(user);
            window.Show();
            this.Close();
        }

        private void deleteButton_Click(object sender, RoutedEventArgs e)
        {
            var selectedItem = treeView.SelectedItem;

            if (selectedItem.GetType() == typeof(Group))
            {
                Group temp = (Group)selectedItem;
                temp.setWindow(this);
                temp.Delete();
            }

            else if (selectedItem.GetType() == typeof(User))
            {
                User temp = (User)selectedItem;
                temp.setWindow(this);
                temp.Delete();
            }
        }

        private void addUserButton_Click(object sender, RoutedEventArgs e)
        {
            AddNewUser window = new AddNewUser(this);
            window.Show();
        }
    }

}