using System.Collections.Generic;
using Microsoft.MediaCenter.Hosting;
using System;
using Microsoft.MediaCenter;
using System.Threading;

namespace MediaControl.Host.MediaCenter
{
    public class MyAddIn : IAddInModule, IAddInEntryPoint     
    {
        public static AddInHost MediaCenterHost = null;

        public void Initialize(Dictionary<string, object> appInfo, Dictionary<string, object> entryPointInfo) { }  
        
        public void Uninitialize()
        {
            try
            {
                EndpointService.Current.TeardownService();
                PlaylistService.Current.TeardownService();
            }
            catch (Exception ex)
            {
                LogUtility.LogException(ex);
                throw;
            }
        }

        public void Launch(AddInHost host)
        {
            MediaCenterHost = host;

            try
            {
                // Start caching
                MediaLibraryService.CacheMediaLibrary();
                PlaylistService.Current.InitializeService();
                EndpointService.Current.InitializeService();
            }
            catch (Exception ex)
            {
                LogUtility.LogException(ex);
                throw;
            }

            do
            {
                System.Threading.Thread.Sleep(500);
            } while (true);
        }
    }
} 
