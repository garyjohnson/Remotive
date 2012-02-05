using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Deployment.WindowsInstaller;
using HttpNamespaceManager.Lib;
using HttpNamespaceManager.Lib.AccessControl;

namespace MediaControl.Host.Installer.CustomActions
{
    public class CustomActions
    {
        [CustomAction("AddNamespace")]
        public static ActionResult AddNamespace(Session session)
        {
            session.Log("Begin AddNamespace");
            HttpApi nsManager = null;
            try
            {
                nsManager = new HttpApi();
                List<SecurityIdentity> userList = new List<SecurityIdentity>();
                Dictionary<string, SecurityDescriptor> nsTable = nsManager.QueryHttpNamespaceAcls();

                string url = "http://+:8888/";

                SecurityDescriptor newSd = new SecurityDescriptor();
                newSd.DACL = new AccessControlList();
                foreach (AccessControlEntry ace in newSd.DACL)
                {
                    if (!userList.Contains(ace.AccountSID))
                    {
                        userList.Add(ace.AccountSID);
                    }
                }

                try
                {
                    SecurityIdentity sid = SecurityIdentity.SecurityIdentityFromWellKnownSid(WELL_KNOWN_SID_TYPE.WinWorldSid);
                    if (!userList.Contains(sid))
                    {
                        AccessControlEntry ace = new AccessControlEntry(sid);
                        ace.AceType = AceType.AccessAllowed;
                        ace.Add(AceRights.GenericAll);
                        ace.Add(AceRights.GenericExecute);
                        ace.Add(AceRights.GenericRead);
                        ace.Add(AceRights.GenericWrite);
                        newSd.DACL.Add(ace);
                        userList.Add(sid);
                    }
                }
                catch (Exception ex)
                {
                    session.Log("User or group name was not found. " + ex.Message);
                    return ActionResult.Failure;
                }

                // If entry already exists, rebuild it
                // as security settings could be wrong
                if (nsTable.ContainsKey(url))
                {
                    AccessControlList original = nsTable[url].DACL;
                    bool removed = false;

                    try
                    {
                        nsManager.RemoveHttpHamespaceAcl(url);
                        removed = true;
                        nsTable[url].DACL = newSd.DACL;
                        nsManager.SetHttpNamespaceAcl(url, nsTable[url]);
                    }
                    catch (Exception ex)
                    {
                        session.Log("Error Setting ACL. " + ex.Message);
                        if (removed)
                        {
                            try
                            {
                                nsTable[url].DACL = original;
                                nsManager.SetHttpNamespaceAcl(url, nsTable[url]);
                            }
                            catch (Exception ex2)
                            {
                                session.Log("Unable to Restore Original ACL, ACL may be corrupt. " + ex2.Message);
                                return ActionResult.Failure;
                            }
                        }

                        session.Log("Original ACL restored.");
                        return ActionResult.Failure;
                    }
                }
                else
                {
                    try
                    {
                        nsManager.SetHttpNamespaceAcl(url, newSd);
                        nsTable.Add(url, newSd);
                    }
                    catch (Exception ex)
                    {
                        session.Log("Error Adding ACL. " + ex.Message);
                        return ActionResult.Failure;
                    }
                }

                return ActionResult.Success;
            }
            finally
            {
                if (nsManager != null)
                {
                    nsManager.Dispose();
                }
            }
        }

        [CustomAction("RemoveNamespace")]
        public static ActionResult RemoveNamespace(Session session)
        {
            session.Log("Begin RemoveNamespace");

            HttpApi nsManager = null;
            try
            {
                nsManager = new HttpApi();
                Dictionary<string, SecurityDescriptor> nsTable = nsManager.QueryHttpNamespaceAcls();

                string url = "http://+:8888/";
                if (nsTable.ContainsKey(url))
                {
                    try
                    {
                        nsManager.RemoveHttpHamespaceAcl(url);
                    }
                    catch (Exception ex)
                    {
                        session.Log("Error removing ACL. " + ex.Message);
                        return ActionResult.Failure;
                    }
                }

                return ActionResult.Success;
            }
            finally
            {
                if (nsManager != null)
                {
                    nsManager.Dispose();
                }
            }
        }
    }
}

