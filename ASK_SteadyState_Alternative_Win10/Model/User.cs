using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.DirectoryServices;
using System.DirectoryServices.AccountManagement;
using System.IO;
using System.Linq;
using System.Security.AccessControl;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace WpfApp1
{
    public class User
    {
        public string name { get; set; }
        public string dispName { get; set; }
        //public Image profileImage;
        public UserPrincipal principal;
        public ObservableCollection<Program> programsLeft;
        public ObservableCollection<Program> programsRight;

        public User(UserPrincipal principal)
        {
            this.principal = principal;
            principal.Enabled = true;
            name = principal.Name;
            dispName = principal.DisplayName;
            programsLeft = new ObservableCollection<Program>();
            programsRight = new ObservableCollection<Program>();
        }

        public void Delete()
        {
            PrincipalContext ctx = new PrincipalContext(ContextType.Machine);
            PrincipalSearchResult<Principal> groups = principal.GetGroups(ctx);

            foreach(var group in groups)
            {
                GroupPrincipal groupPrincipal = (GroupPrincipal)group;

                groupPrincipal.Members.Remove(principal);
                groupPrincipal.Save();

            }

            principal.Delete();
        }

        public int checkAccess(string fileName)
        {
            bool denied = false;
            string domain = System.Environment.UserDomainName;
            FileInfo fileInfo = new FileInfo(fileName);
            FileSecurity fileSec = fileInfo.GetAccessControl();

            foreach (FileSystemAccessRule rule in fileSec.GetAccessRules(true, true, typeof(NTAccount)))

            {
                string allow = rule.AccessControlType == AccessControlType.Allow ? "grants" : "denies";
                if (allow == "denies")
                {
                    string name = rule.IdentityReference.ToString();
                    if (name == (domain + @"\" + this.name))
                        denied = true;
                }
            }
            FileSecurity fSecurity = File.GetAccessControl(fileName);
            Console.WriteLine(fileName);
            try
            {
                if (denied)
                    AddFileSecurity(fileName, domain + @"\" + this.name, FileSystemRights.Read, AccessControlType.Deny);
                else
                {
                    AddFileSecurity(fileName, domain + @"\" + this.name, FileSystemRights.Read, AccessControlType.Deny);
                    RemoveFileSecurity(fileName, domain + @"\" + this.name, FileSystemRights.Read, AccessControlType.Deny);
                }
                Console.WriteLine("jestem");
            }
            catch (Exception e)
            {
                return 0;
            }
            if (denied)
                return 100;
            else
                return 10;

        }

        public void checkBlockList()
        {
            string domain = System.Environment.UserDomainName;
            foreach (Program p in programsLeft)
            {
                RemoveFileSecurity(p.fileLocation, domain + @"\" + this.name, FileSystemRights.Read, AccessControlType.Deny);
            }
            foreach(Program p in programsRight)
            {
                AddFileSecurity(p.fileLocation, domain + @"\" + this.name, FileSystemRights.Read, AccessControlType.Deny);
            }
        }

 

            public void AddFileSecurity(string fileName, string account,
         FileSystemRights rights, AccessControlType controlType)
        {
            // Get a FileSecurity object that represents the
            // current security settings
            FileSecurity fSecurity = File.GetAccessControl(fileName);

            // Add the FileSystemAccessRule to the security settings.
            fSecurity.AddAccessRule(new FileSystemAccessRule(account,
                rights, controlType));
            // Set the new access settings.
            try
            {
                File.SetAccessControl(fileName, fSecurity);
            }
            catch(Exception e)
            {
                throw e;
            }

        }

        // Removes an ACL entry on the specified file for the specified account.
        public static void RemoveFileSecurity(string fileName, string account,
            FileSystemRights rights, AccessControlType controlType)
        {

            // Get a FileSecurity object that represents the
            // current security settings.
            FileSecurity fSecurity = File.GetAccessControl(fileName);

            // Remove the FileSystemAccessRule from the security settings.
            fSecurity.RemoveAccessRule(new FileSystemAccessRule(account,
                rights, controlType));

            // Set the new access settings.
            try
            {
                File.SetAccessControl(fileName, fSecurity);
            }
            catch (Exception e)
            {
                throw e;
            }

        }

    }
}
