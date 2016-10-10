using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Windows.Automation;

namespace Vpn
{
    internal class Program
    {
        [STAThread]
        private static void Main(string[] args)
        {
            //Getting the Password
            string html = new WebClient().DownloadString("https://www.vpnme.me/freevpn.html");
            string split = html.Split(new[] { "features-table2" }, StringSplitOptions.None)[1];
            split = split.Split(new[] { "<tbody>" }, StringSplitOptions.None)[1];
            string[] td = split.Split(new[] { "<td>" }, StringSplitOptions.None);
            string pw = td[7].Split('<')[0];
            Console.WriteLine(pw);
            System.Windows.Forms.Clipboard.SetText(pw);

            //Opening VPN tunnel
            String pwPath = @"C:\vpn_pw.txt";
            String configPath = "\"" + @"C:\Program Files\OpenVPN\config\vpnme_fr_tcp443.ovpn" + "\"";

            if (File.Exists(pwPath))
                File.Delete(pwPath);
            File.Create(pwPath);
            File.WriteAllLines(pwPath, new List<string> { "fr-open", pw });
            string cmd = "openvpn.exe --config " + configPath + " --auth-user-pass " + pwPath;
            Process.Start("CMD.exe", cmd);
            File.Delete(pwPath);

            Console.ReadKey();
        }
    }
}