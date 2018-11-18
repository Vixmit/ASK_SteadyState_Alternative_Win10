using ASK_SteadyState_Alternative.TreeView_control;
using System;
using System.Collections.Generic;
using System.DirectoryServices;
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

namespace ASK_SteadyState_Alternative_Win10
{
    /// <summary>
    /// Interaction logic for EditUser.xaml
    /// </summary>
    public partial class EditUser : Window
    {
        private User user;
        private UserPrincipal up;
        public EditUser(User user)
        {
            InitializeComponent();
            this.user = user;
            up = (UserPrincipal)user.principal;
            name.Text = up.Name;
            description.Text = up.Description;
        }

        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            MainWindow window = new MainWindow();
            window.Show();
            this.Close();
           
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            PrincipalContext ctx = new PrincipalContext(ContextType.Machine);
            up = UserPrincipal.FindByIdentity(ctx,up.Name);
            up.Description = description.Text;
            if (password.Text != "")
                up.SetPassword(password.Text);
            up.Save();
        }
    }
}
