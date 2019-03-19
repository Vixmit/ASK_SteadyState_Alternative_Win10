using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.DirectoryServices;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using WpfApp1.View;

namespace WpfApp1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        PrincipalContext ctx;
        GroupPrincipal groupPrincipal;

        public MainWindow()
        {
            InitializeComponent();

            setComputerRestrictions.MouseLeftButtonDown += new MouseButtonEventHandler(computerRestrictions_Click);
            scheduleUpdate.MouseLeftButtonDown += new MouseButtonEventHandler(scheduleUpdate_Click);
            protectDisk.MouseLeftButtonDown += new MouseButtonEventHandler(protectDisk_Click);
            ctx = new PrincipalContext(ContextType.Machine);

            //Sprawdź czy jest grupa Shared jak nie to stwórz
            if ((groupPrincipal = GroupPrincipal.FindByIdentity(ctx, "Shared")) == null)
            {
                groupPrincipal = new GroupPrincipal(ctx, "Shared");
                groupPrincipal.Description = "Group for shared profiles";

                try
                {
                    groupPrincipal.Save();
                }
                catch (UnauthorizedAccessException ex)
                {
                    MessageBox.Show("Application must be run with administrator priviliges!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
            }

        Refresh();
        }
 
        void computerRestrictions_Click(object sender, EventArgs e)
        {
            Window window = new SetComputerRestrictions(ctx, groupPrincipal);
            window.Show();
        }
        void scheduleUpdate_Click(object sender, EventArgs e)
        {

        }
        void protectDisk_Click(object sender, EventArgs e)
        {
            Window window = new ProtectDisk(ctx, groupPrincipal);
            window.Show();
        }

        void userListBox_DoubleClick(object sender, EventArgs e)
        {
            if (userListBox.SelectedItem != null)
            {
                UserWindow window = new UserWindow((User)userListBox.SelectedItem);
                //window.user = (User)userListBox.SelectedItem;
                this.Hide();
                window.Show();
            }
        }

        private void addNewUser_Click(object sender, RoutedEventArgs e)
        {
            Window window = new AddNewUserWindow();
            window.Show();

        }

        public void Refresh()
        {
           
            //Pobierz użytkowników grupy Shared
            PrincipalSearchResult<Principal> result = groupPrincipal.GetMembers();

            ObservableCollection<User> users = new ObservableCollection<User>();

            foreach (var v in result)
            {
                UserPrincipal userPrincipal = (UserPrincipal)v;
                User user = new User(userPrincipal);
                users.Add(user);
            }
            userListBox.ItemsSource = users;
        }
    }
}
