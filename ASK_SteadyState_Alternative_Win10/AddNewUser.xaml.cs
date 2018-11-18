using System;
using System.Collections.Generic;
using System.DirectoryServices.AccountManagement;
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
using ASK_SteadyState_Alternative.TreeView_control;

namespace ASK_SteadyState_Alternative_Win10
{
    /// <summary>
    /// Interaction logic for AddNewUser.xaml
    /// </summary>
    public partial class AddNewUser : Window
    {
        MainWindow window;
        public AddNewUser(MainWindow window)
        {
            InitializeComponent();
            this.window = window;
        }

        private void save_Click(object sender, RoutedEventArgs e)
        {
            PrincipalContext ctx = new PrincipalContext(ContextType.Machine);
            string userName = name.Text;
            string userDescription = description.Text;
            if (userName == "")
            {
                MessageBox.Show("Group has to have a name!");
                return;
            }
            Group myGroup = (Group)window.treeView.SelectedItem;
            string groupName = myGroup.Name;
            if(groupName == "")
            {
                MessageBox.Show("There is a problem with group!");
                this.Close();
            }
            UserPrincipal user = new UserPrincipal(ctx);
            user.Name = userName;
            user.Description = userDescription;
            user.Enabled = true;
            user.Save();
            
            GroupPrincipal group = GroupPrincipal.FindByIdentity(ctx, groupName);
            group.Members.Add(user);
            group.Save();
            
            window.refreshTree();
            this.Close();
        }

        private void cancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
