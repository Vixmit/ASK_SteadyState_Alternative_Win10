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
using WpfApp1.Model;

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
        public ObservableCollection<Disk> disks;
        public ObservableCollection<Disk> disksFree;

        public User(UserPrincipal principal)
        {
            this.principal = principal;
            principal.Enabled = true;
            name = principal.Name;
            dispName = principal.DisplayName;
            programsLeft = new ObservableCollection<Program>();
            programsRight = new ObservableCollection<Program>();
            disks = new ObservableCollection<Disk>();
            disksFree = new ObservableCollection<Disk>();
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

        public bool checkAccessDir(string fileName)
        {
            bool denied = false;
            string domain = System.Environment.UserDomainName;
            DirectoryInfo fileInfo = new DirectoryInfo(fileName);
            DirectorySecurity fileSec = fileInfo.GetAccessControl();

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
            return denied;
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
            try
            {
                if (denied)
                    AddFileSecurity(fileName, domain + @"\" + this.name, FileSystemRights.Read, AccessControlType.Deny);
                else
                {
                    AddFileSecurity(fileName, domain + @"\" + this.name, FileSystemRights.Read, AccessControlType.Deny);
                    RemoveFileSecurity(fileName, domain + @"\" + this.name, FileSystemRights.Read, AccessControlType.Deny);
                }
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

        public void blockDisks()
        {
            string domain = System.Environment.UserDomainName;
            foreach (Disk d in disks)
            {
                DirectoryInfo directory = new DirectoryInfo(d.name);
                try
                {
                    AddDirectorySecurity(directory.FullName, domain + @"\" + this.name, FileSystemRights.Read, AccessControlType.Deny);
                }catch (Exception x)
                {

                }
            }
        }

        public void freeDisks()
        {
            string domain = System.Environment.UserDomainName;
            foreach (Disk d in disksFree)
            {
                DirectoryInfo directory = new DirectoryInfo(d.name);
                try
                {
                    RemoveDirectorySecurity(directory.FullName, domain + @"\" + this.name, FileSystemRights.Read, AccessControlType.Deny);
                }
                catch (Exception x)
                {

                }
            }
        }

 

        public void AddFileSecurity(string fileName, string account,FileSystemRights rights, AccessControlType controlType)
        {
            FileSecurity fSecurity = File.GetAccessControl(fileName);
            fSecurity.AddAccessRule(new FileSystemAccessRule(account,
                rights, controlType));
            try
            {
                File.SetAccessControl(fileName, fSecurity);
            }
            catch(Exception e)
            {
                throw e;
            }

        }
        public void RemoveFileSecurity(string fileName, string account,FileSystemRights rights, AccessControlType controlType)
        {
            FileSecurity fSecurity = File.GetAccessControl(fileName);
            fSecurity.RemoveAccessRule(new FileSystemAccessRule(account,
                rights, controlType));
            try
            {
                File.SetAccessControl(fileName, fSecurity);
            }
            catch (Exception e)
            {
                throw e;
            }

        }
        public void AddDirectorySecurity(string FileName, string Account, FileSystemRights Rights, AccessControlType ControlType)
        {
            DirectoryInfo dInfo = new DirectoryInfo(FileName);
            DirectorySecurity dSecurity = dInfo.GetAccessControl();
            dSecurity.AddAccessRule(new FileSystemAccessRule(Account,Rights,ControlType));
            try
            {
                dInfo.SetAccessControl(dSecurity);
            }catch(Exception e)
            {
                throw e;
            }
            
        }
        public void RemoveDirectorySecurity(string FileName, string Account, FileSystemRights Rights, AccessControlType ControlType)
        {
            DirectoryInfo dInfo = new DirectoryInfo(FileName);
            DirectorySecurity dSecurity = dInfo.GetAccessControl();
            dSecurity.RemoveAccessRule(new FileSystemAccessRule(Account,Rights,ControlType));
            try
            {
                dInfo.SetAccessControl(dSecurity);
            }catch (Exception x)
            {
                throw x;
            }
        }
    }
}
