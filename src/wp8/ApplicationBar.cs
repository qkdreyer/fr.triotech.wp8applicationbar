using System.Runtime.Serialization;
using Microsoft.Phone.Shell;
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Phone.Controls;
using System.Windows;
using WPCordovaClassLib.Cordova.Commands;
using WPCordovaClassLib.Cordova.JSON;
using WPCordovaClassLib.Cordova;

namespace Cordova.Extension.Commands
{
    /// <summary>
    /// Implements access to application live tiles
    /// </summary>
    public class ApplicationBar : BaseCommand
    {

        #region Live tiles options
        
        /// <summary>
        /// Represents LiveTile options
        /// </summary>
        [DataContract]
        public class ApplicationBarOptions
        {
            /// <summary>
            /// IsMenuEnabled
            /// </summary>
            [DataMember(IsRequired = false, Name = "IsMenuEnabled")]
            public string IsMenuEnabled { get; set; }

            /// <summary>
            /// Mode
            /// </summary>
            [DataMember(IsRequired = false, Name = "Mode")]
            public string Mode { get; set; }

            /// <summary>
            /// IsVisible
            /// </summary>
            [DataMember(IsRequired = false, Name = "IsVisible")]
            public string IsVisible { get; set; }

            /// <summary>
            /// Opacity
            /// </summary>
            [DataMember(IsRequired = false, Name = "Opacity")]
            public int Opacity { get; set; }
        }
        #endregion

        /// <summary>
        /// Updates application live tile
        /// </summary>
        public void update(string options)
        {
            string[] args = JsonHelper.Deserialize<string[]>(options);
            string callbackId = args[1];

            ApplicationBarOptions appBarOptions;
            try
            {
                appBarOptions = JsonHelper.Deserialize<ApplicationBarOptions>(args[0]);
            }
            catch (Exception)
            {
                DispatchCommandResult(new PluginResult(PluginResult.Status.JSON_EXCEPTION), callbackId);
                return;
            }

            try
            {
                InitializeComponent();

                ApplicationBar appBar = CreateApplicationBar(appBarOptions);
            }
            catch(Exception)
            {
                DispatchCommandResult(new PluginResult(PluginResult.Status.ERROR, "Error updating application bar"), callbackId);
            }
        }

        /// <summary>
        /// Creates standard tile data
        /// </summary>
        private ApplicationBar CreateApplicationBar(ApplicationBarOptions appBarOptions)
        {
            ApplicationBar appBar = new ApplicationBar();

            // IsMenuEnabled
            if (!string.IsNullOrEmpty(appBarOptions.IsMenuEnabled))
            {
                appBar.IsMenuEnabled = appBarOptions.IsMenuEnabled;
            }

            // Mode
            if (!string.IsNullOrEmpty(appBarOptions.Mode))
            {
                appBar.Mode = appBarOptions.Mode;
            }

            // IsVisible
            if (!string.IsNullOrEmpty(appBarOptions.IsVisible))
            {
                appBar.IsVisible = appBarOptions.IsVisible;
            }

            // Opacity
            if (appBarOptions.Opacity > 0)
            {
                appBar.Opacity = appBarOptions.Opacity;
            }

            return appBar;
        }

    }

}