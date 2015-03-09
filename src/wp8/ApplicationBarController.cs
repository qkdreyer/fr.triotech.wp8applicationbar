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
    public class ApplicationBarController : BaseCommand
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
            [DataMember(IsRequired = false, Name = "isMenuEnabled")]
            public bool IsMenuEnabled { get; set; }

            /// <summary>
            /// Mode
            /// </summary>
            [DataMember(IsRequired = false, Name = "mode")]
            public string Mode { get; set; }

            /// <summary>
            /// IsVisible
            /// </summary>
            [DataMember(IsRequired = false, Name = "isVisible")]
            public bool IsVisible { get; set; }

            /// <summary>
            /// Opacity
            /// </summary>
            [DataMember(IsRequired = false, Name = "opacity")]
            public int Opacity { get; set; }

            /// <summary>
            /// BackgroundColor
            /// </summary>
            [DataMember(IsRequired = false, Name = "backgroundColor")]
            public string BackgroundColor { get; set; }

            /// <summary>
            /// ForegroundColor
            /// </summary>
            [DataMember(IsRequired = false, Name = "foregroundColor")]
            public string ForegroundColor { get; set; }


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
            if (appBarOptions.isMenuEnabled)
            {
                appBar.IsMenuEnabled = appBarOptions.isMenuEnabled;
            }

            // Mode
            if (!string.IsNullOrEmpty(appBarOptions.mode))
            {
                if (appBarOptions.mode == "default") 
                    appBar.Mode = ApplicationBarMode.Default;
                else if (appBarOptions.mode == "minimized")
                    appBar.Mode = ApplicationBarMode.Minimized;
            }

            // IsVisible
            if (appBarOptions.isVisible)
            {
                appBar.IsVisible = appBarOptions.isVisible;
            }

            // BackgroundColor
            if (!string.IsNullOrEmpty(appBarOptions.backgroundColor))
            {
                appBar.BackgroundColor = appBarOptions.backgroundColor;
            }

            // BackgroundColor
            if (!string.IsNullOrEmpty(appBarOptions.foregroundColor))
            {
                appBar.ForegroundColor = appBarOptions.foregroundColor;
            }

            // Opacity
            if (appBarOptions.opacity > 0)
            {
                appBar.Opacity = appBarOptions.opacity;
            }

            return appBar;
        }

    }

}