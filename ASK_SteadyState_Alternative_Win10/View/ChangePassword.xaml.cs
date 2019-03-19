using System;
using System.Collections.Generic;
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
    /// Interaction logic for ChangePassword.xaml
    /// </summary>
    public partial class ChangePassword : Window
    {
        private User user;
        public ChangePassword(User user)
        {
            InitializeComponent();
            this.user = user;
        }

        private void SetPassword_Click(object sender, RoutedEventArgs e)
        {
            if ((newPass1.Password != newPass2.Password))
            {
                MessageBox.Show("ERROR! Passwords are not equal!");
                return;
            }
            else
            {
                try
                {
                    user.principal.ChangePassword(textBox.Password, newPass1.Password);
                    MessageBox.Show("Success! ");
                    this.Close();
                    return;
                }
                catch(Exception x)
                {
                    MessageBox.Show("ERROR! Old Password is not correct!");
                    return;
                }
                
            }
        }
    }
}
