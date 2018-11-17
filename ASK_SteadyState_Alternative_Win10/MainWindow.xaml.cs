using System;
using System.Collections.Generic;
using System.Windows;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.DirectoryServices;
using System.DirectoryServices.AccountManagement;

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
                    newGroup.Members.Add(new User() { Name = s.Name, Age = 0, principal = s });
                groups.Add(newGroup);

            }



            treeView.ItemsSource = groups;
        }
    }

    public class Group
    {
        public Group()
        {
            this.Members = new ObservableCollection<User>();
        }

        public string Name { get; set; }

        public ObservableCollection<User> Members { get; set; }

        public Principal principal { get; set; }
    }
    public class User
    {
        public string Name { get; set; }

        public int Age { get; set; }

        public Principal principal { get; set; }
    }
}