using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.DirectoryServices.AccountManagement;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASK_SteadyState_Alternative.TreeView_control
{
    public class Group
    {
        public Group()
        {
            this.Members = new ObservableCollection<User>();
        }
        public string Name { get; set; }
        MainWindow Window;
        public ObservableCollection<User> Members { get; set; }
        public Principal principal { get; set; }

        public string Describe()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("Type: Group");
            sb.Append("Name: ");
            sb.AppendLine(this.principal.Name);
            sb.Append("Description: ");
            sb.AppendLine(this.principal.Description);
            return sb.ToString();
        }

        public void Delete()
        {
            if (Window == null)
                return;
            PrincipalContext ctx = new PrincipalContext(ContextType.Machine);
            var group = GroupPrincipal.FindByIdentity(ctx, this.Name);
            foreach(var x in Members)
            {
                x.setWindow(Window);
                x.Delete();
            }
            group.Save();
            group.Delete();
            Window.refreshTree();
        }

        public void setWindow(MainWindow window)
        {
            this.Window = window;
        }
    }
}
