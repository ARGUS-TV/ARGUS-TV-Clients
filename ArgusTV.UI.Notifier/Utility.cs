/*
 *	Copyright (C) 2007-2014 ARGUS TV
 *	http://www.argus-tv.com
 *
 *  This Program is free software; you can redistribute it and/or modify
 *  it under the terms of the GNU General Public License as published by
 *  the Free Software Foundation; either version 2, or (at your option)
 *  any later version.
 *
 *  This Program is distributed in the hope that it will be useful,
 *  but WITHOUT ANY WARRANTY; without even the implied warranty of
 *  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
 *  GNU General Public License for more details.
 *
 *  You should have received a copy of the GNU General Public License
 *  along with GNU Make; see the file COPYING.  If not, write to
 *  the Free Software Foundation, 675 Mass Ave, Cambridge, MA 02139, USA.
 *  http://www.gnu.org/copyleft/gpl.html
 *
 */
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Security.Cryptography;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Diagnostics;
using Microsoft.Win32;

using ArgusTV.DataContracts;

namespace ArgusTV.UI.Notifier
{
    public static class Utility
    {
        public static bool IsVlcInstalled()
        {
            string vlcPath = GetVlcPath();
            return !String.IsNullOrEmpty(vlcPath)
                && File.Exists(vlcPath);
        }

        public static void RunVlc(string fileName)
        {
            string vlcPath = GetVlcPath();
            if (!String.IsNullOrEmpty(vlcPath)
                && File.Exists(vlcPath))
            {
                ProcessStartInfo startInfo = new ProcessStartInfo(vlcPath,
                    String.Format(CultureInfo.InvariantCulture, "\"{0}\"", fileName));
                startInfo.WorkingDirectory = Path.GetDirectoryName(vlcPath);
                startInfo.UseShellExecute = true;
                System.Diagnostics.Process.Start(startInfo);
            }
        }

        private static string GetVlcPath()
        {
            string vlcPath = ReadRegKeyValue(@"SOFTWARE\Wow6432Node\VideoLAN\VLC");
            if (String.IsNullOrEmpty(vlcPath))
            {
                vlcPath = ReadRegKeyValue(@"SOFTWARE\VideoLAN\VLC");
            }
            return vlcPath;
        }

        private static string ReadRegKeyValue(string keyPath)
        {
            using (RegistryKey regKey = Registry.LocalMachine.OpenSubKey(keyPath))
            {
                if (regKey != null)
                {
                    return (string)regKey.GetValue(null);
                }
            }
            return null;
        }

        public static void EnsureMinimumTime(DateTime startTime, int milliseconds)
        {
            TimeSpan loadingTime = DateTime.Now - startTime;
            TimeSpan minimumLoadingTime = new TimeSpan(0, 0, 0, 0, milliseconds);
            if (loadingTime < minimumLoadingTime)
            {
                System.Threading.Thread.Sleep(minimumLoadingTime - loadingTime);
            }
        }
    }
}
