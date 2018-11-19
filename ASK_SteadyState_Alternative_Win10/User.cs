using System;
using System.Collections.Generic;
using System.DirectoryServices.AccountManagement;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASK_SteadyState_Alternative.TreeView_control
{
    public class User
    {
        public string Name { get; set; }
        public Principal principal { get; set; }
        public MainWindow Window;
        public string Describe()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("Type: User ");
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
            UserPrincipal user = UserPrincipal.FindByIdentity(ctx, Name);
            PrincipalSearchResult<Principal> groups = user.GetGroups(ctx);

            foreach (var x in groups)
            {
                GroupPrincipal group = (GroupPrincipal)x;
                group.Members.Remove(user);
                group.Save();
            }
            this.Window.refreshTree();
        }
        public void setWindow(MainWindow window)
        {
            this.Window = window;
        }
    }
}
