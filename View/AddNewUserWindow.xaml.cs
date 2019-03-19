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

namespace WpfApp1
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class AddNewUserWindow : Window
    {
        public AddNewUserWindow()
        {
            InitializeComponent();
        }

        void save_Click(object sender, EventArgs e)
        {
            if((password.Password != password2.Password)||name.Text==string.Empty)
            {
                //MBox error
                MessageBox.Show("ERROR! Passwords are not equal or name field is empty");
                return;
            }

            PrincipalContext ctx = new PrincipalContext(ContextType.Machine);
            string userName = name.Text;
            string userDescription = description.Text;
            string userPassword = password.Password;

            GroupPrincipal group = GroupPrincipal.FindByIdentity(ctx,"Shared");
            GroupPrincipal group2;
            if ((group2 = GroupPrincipal.FindByIdentity(ctx, "Users")) == null){
                group2 = GroupPrincipal.FindByIdentity(ctx, "Użytkownicy");
            }

            UserPrincipal newUser = new UserPrincipal(ctx);
            newUser.Name = userName;
            newUser.Description = userDescription;
            newUser.SetPassword(userPassword);
            newUser.Enabled = true;
            newUser.Save();


            group.Members.Add(newUser);
            group.Save();
            group2.Members.Add(newUser);
            group2.Save();
            this.Close();
            MainWindow window = (MainWindow)Application.Current.MainWindow;
            window.Refresh();

            //Application.Current.MainWindow.
        }

        void cancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }


    }
}
