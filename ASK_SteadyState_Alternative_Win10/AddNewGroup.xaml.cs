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
    /// Interaction logic for AddNewGroup.xaml
    /// </summary>
    public partial class AddNewGroup : Window
    {
        MainWindow window;
        public AddNewGroup(MainWindow window)
        {
            InitializeComponent();
            this.window = window;
        }

        private void save_Click(object sender, RoutedEventArgs e)
        {
            PrincipalContext ctx = new PrincipalContext(ContextType.Machine);
            string groupName = name.Text;
            string groupDescription = description.Text;
            if (groupName == "")
            {
                MessageBox.Show("Group has to have a name!");
                return;
            }
            GroupPrincipal group = new GroupPrincipal(ctx, groupName);
            group.Description = groupDescription;
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
