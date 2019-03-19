using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.DirectoryServices.AccountManagement;
using System.Linq;
using System.Security.Principal;
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
    /// Interaction logic for SetComputerRestrictions.xaml
    /// </summary>
    public partial class SetComputerRestrictions : Window
    {

        ObservableCollection<User> users;
        ObservableCollection<User> disableUsers;
        PrincipalContext ctx;
        GroupPrincipal adminGroup, sharedGroup;
        public SetComputerRestrictions(PrincipalContext ctx, GroupPrincipal group)
        {
            InitializeComponent();
            this.ctx = ctx;
            this.sharedGroup = group;
            SetList();
        }

        public void SetList()
        {
            SecurityIdentifier id = new SecurityIdentifier("S-1-5-32-544");
            string name = id.Translate(typeof(NTAccount)).Value;
            adminGroup = GroupPrincipal.FindByIdentity(ctx, name);
            PrincipalSearchResult<Principal> result = adminGroup.GetMembers();
            PrincipalSearchResult<Principal> result2 = sharedGroup.GetMembers();

            users = new ObservableCollection<User>();
            disableUsers = new ObservableCollection<User>();
            bool enable;
            foreach (var v in result)
            {
                try
                {
                    UserPrincipal userPrincipal = (UserPrincipal)v;
                    enable = (bool)userPrincipal.Enabled;
                    User user = new User(userPrincipal);
                    user.principal.Enabled = enable;
                    user.principal.Save();
                    users.Add(user);
                }catch(Exception x) { }
            }
            foreach (var v in result2)
            { 
                try
                {
                    UserPrincipal userPrincipal = (UserPrincipal)v;
                    enable = (bool)userPrincipal.Enabled;
                    User user = new User(userPrincipal);
                    user.principal.Enabled = enable;
                    user.principal.Save();
                    users.Add(user);
                }catch(Exception x) { }
                
            }
            enableListBox.ItemsSource = users;
            checkUsers();
            
        }
        void checkUsers()
        {
            foreach(User x in users)
            {
                if (x.principal.Enabled == true)
                {
                    try
                    {
                        enableListBox.SelectedItems.Add(x);
                    }
                    catch(Exception e)
                    {
                        enableListBox.SelectedItem = x;
                    }
                }
            }
        }

        bool checkIsSelected(User u)
        {
            foreach (User v in enableListBox.SelectedItems)
            {
                if (u.Equals(v))
                    return true;
            }
            return false;
        }
        private void save_Click(object sender, RoutedEventArgs e)
        {
            foreach (User d in users)
            {
                if (!checkIsSelected(d))
                    disableUsers.Add(d);
            }
            foreach(User u in disableUsers)
            {
                u.principal.Enabled = false;
                u.principal.Save();
            }
            foreach(User u in enableListBox.SelectedItems)
            {
                u.principal.Enabled = true;
                u.principal.Save();
            }
            this.Close();
        }
    }
}
