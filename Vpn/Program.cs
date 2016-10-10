using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net;

namespace Vpn
{
    internal class Program
    {
        [STAThread]
        private static void Main(string[] args)
        {
            //Getting the Password via ugly splits
            string html = new WebClient().DownloadString("https://www.vpnme.me/freevpn.html");
            string split = html.Split(new[] { "features-table2" }, StringSplitOptions.None)[1];
            split = split.Split(new[] { "<tbody>" }, StringSplitOptions.None)[1];
            string[] td = split.Split(new[] { "<td>" }, StringSplitOptions.None);
            string pw = td[7].Split('<')[0];
            Console.WriteLine(pw);
            Console.WriteLine("Initiating VPN...");

            //Opening VPN tunnel
            String pwPath = @"C:\vpn_pw.txt";
            String configPath = "\"" + @"C:\Program Files\OpenVPN\config\vpnme_fr_tcp443.ovpn" + "\"";

            //Creating a File with user credentials
            if (File.Exists(pwPath))
                File.Delete(pwPath);
            File.Create(pwPath).Close();
            File.WriteAllLines(pwPath, new List<string> { "fr-open", pw });

            //Creating console statements
            string vpn = "openvpn.exe --config " + configPath + " --auth-user-pass " + pwPath;
            string color = "color a";
            string cmd = "/C " + color + " & " + vpn;

            //Starting comands
            Process.Start("CMD.exe", cmd);
            System.Threading.Thread.Sleep(2000);
            File.Delete(pwPath);
        }
    }
}