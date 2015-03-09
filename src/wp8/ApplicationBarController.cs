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
using Color = System.Windows.Media.Color;

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
            public bool isMenuEnabled { get; set; }

            /// <summary>
            /// Mode
            /// </summary>
            [DataMember(IsRequired = false, Name = "mode")]
            public string mode { get; set; }

            /// <summary>
            /// IsVisible
            /// </summary>
            [DataMember(IsRequired = false, Name = "isVisible")]
            public bool isVisible { get; set; }

            /// <summary>
            /// Opacity
            /// </summary>
            [DataMember(IsRequired = false, Name = "opacity")]
            public int opacity { get; set; }

            /// <summary>
            /// BackgroundColor
            /// </summary>
            [DataMember(IsRequired = false, Name = "backgroundColor")]
            public string backgroundColor { get; set; }

            /// <summary>
            /// ForegroundColor
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
            catch (Exception)
            {
                DispatchCommandResult(new PluginResult(PluginResult.Status.JSON_EXCEPTION), callbackId);
                return;
            }

            try
            {
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

            // Opacity
            if (appBarOptions.opacity > 0)
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