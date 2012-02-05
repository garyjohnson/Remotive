using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.MediaCenter;
using System.Reflection;

namespace MediaControl.Host.MediaCenter
{
    internal static class Utility
    {
        public static bool IsWindows7OrHigher
        {
            get 
            { 
                OperatingSystem system = System.Environment.OSVersion;
                return (system.Version.Major == 6 && system.Version.Minor >= 1);
            }
        }

        public static void ResetMediaExperienceCache(MediaCenterEnvironment environment)
        {
            if (IsWindows7OrHigher)
            {
                try
                {
                    FieldInfo fieldInfo = environment.GetType().GetField("_checkedMediaExperience", 
                        BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);

                    if (fieldInfo != null)
                    {
                        fieldInfo.SetValue(environment, false);
                    }
                    else
                    {
                        LogUtility.LogMessage("Warning: cannot find MediaCenterEnvironment._checkedMediaExperience field on Windows 7");
                    }
                }
                catch (Exception e)
                {
                    LogUtility.LogMessage("Error setting MediaCenterEnvironment._checkedMediaExperience flag on Windows 7: " + e.ToString());
                }
            }
        } 
    }
}
