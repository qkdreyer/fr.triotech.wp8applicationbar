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
using Convert = System.Convert;
using Color = System.Windows.Media.Color;

namespace Cordova.Extension.Commands
{
    /// <summary>
    /// Implements access to application bar
    /// </summary>
    public class ApplicationBarController : BaseCommand
    {        
        #region ApplibationBar Options

        /// <summary>
        /// Represents Application options
        /// </summary>
        [DataContract]
        public class ApplicationBarOptions
        {
            /// <summary>
            /// isMenuEnabled
            /// </summary>
            [DataMember(IsRequired = false, Name = "isMenuEnabled")]
            public string isMenuEnabled { get; set; }

            /// <summary>
            /// mode
            /// </summary>
            [DataMember(IsRequired = false, Name = "mode")]
            public string mode { get; set; }

            /// <summary>
            /// isVisible
            /// </summary>
            [DataMember(IsRequired = false, Name = "isVisible")]
            public string isVisible { get; set; }

            /// <summary>
            /// opacity
            /// </summary>
            [DataMember(IsRequired = false, Name = "opacity")]
            public float opacity { get; set; }

            /// <summary>
            /// backgroundColor
            /// </summary>
            [DataMember(IsRequired = false, Name = "backgroundColor")]
            public string backgroundColor { get; set; }

            /// <summary>
            /// foregroundColor
            /// </summary>
            [DataMember(IsRequired = false, Name = "foregroundColor")]
            public string foregroundColor { get; set; }
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
            catch (Exception e)
            {
                DispatchCommandResult(new PluginResult(PluginResult.Status.JSON_EXCEPTION, "Error derializing options: " + e.Message), callbackId);
                return;
            }

            ApplicationBar appBar = new ApplicationBar();
            try
            {
                appBar = CreateApplicationBar(appBar, appBarOptions);
                DispatchCommandResult(new PluginResult(PluginResult.Status.OK), callbackId);
            }
            catch(Exception e)
            {
                DispatchCommandResult(new PluginResult(PluginResult.Status.ERROR, "Error updating application bar: " + e.Message), callbackId);
            }
        }

        /// <summary>
        /// Updates ApplicationBar with ApplicationBarOptions
        /// </summary>
        private ApplicationBar CreateApplicationBar(ApplicationBar appBar, ApplicationBarOptions appBarOptions)
        {
            // IsMenuEnabled
            if (!string.IsNullOrEmpty(appBarOptions.isMenuEnabled))
            {
                appBar.IsMenuEnabled = Convert.ToBoolean(appBarOptions.isMenuEnabled);
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
            if (!string.IsNullOrEmpty(appBarOptions.isVisible))
            {
                appBar.IsVisible = Convert.ToBoolean(appBarOptions.isVisible);
            }

            // Opacity
            if (appBarOptions.opacity >= 0)
            {
                appBar.Opacity = appBarOptions.opacity;
            }

            // BackgroundColor
            if (!string.IsNullOrEmpty(appBarOptions.backgroundColor))
            {
                appBar.BackgroundColor = ConvertStringToColor(appBarOptions.backgroundColor);
            }

            // BackgroundColor
            if (!string.IsNullOrEmpty(appBarOptions.foregroundColor))
            {
                appBar.ForegroundColor = ConvertStringToColor(appBarOptions.foregroundColor);
            }

            return appBar;
        }

        /// <summary>
        /// Converts string to color
        /// source: http://stackoverflow.com/a/11739523/3861558
        /// </summary>
        public Color ConvertStringToColor(String hex)
        {
            hex = hex.Replace("#", "");
            byte a = 255;
            byte r = 255;
            byte g = 255;
            byte b = 255;

            int start = 0;
            if (hex.Length == 8)
            {
                a = byte.Parse(hex.Substring(0, 2), System.Globalization.NumberStyles.HexNumber);
                start = 2;
            }
            r = byte.Parse(hex.Substring(start, 2), System.Globalization.NumberStyles.HexNumber);
            g = byte.Parse(hex.Substring(start + 2, 2), System.Globalization.NumberStyles.HexNumber);
            b = byte.Parse(hex.Substring(start + 4, 2), System.Globalization.NumberStyles.HexNumber);

            return Color.FromArgb(a, r, g, b);
        }

    }

}