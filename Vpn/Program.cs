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
            bool failed;
            string pw = "";
            do
            {
                failed = false;
                try
                {
                    //Getting the Password via ugly splits
                    string html = new WebClient().DownloadString("https://www.vpnme.me/freevpn.html");
                    string split = html.Split(new[] { "features-table2" }, StringSplitOptions.None)[1];
                    split = split.Split(new[] { "<tbody>" }, StringSplitOptions.None)[1];
                    string[] td = split.Split(new[] { "<td>" }, StringSplitOptions.None);
                    pw = td[7].Split('<')[0];
                    Console.WriteLine(pw);
                    Console.WriteLine("Initiating VPN...");
                }
                catch (Exception e)
                {
                    //Error handling
                    failed = true;
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine($"\nFailed to read password from website.\nError:{e.Message} \nTrying again...\n");
                    Console.ResetColor();
                }
            } while (failed);

            //Opening VPN tunnel
            String pwPath = @"C:\vpn_pw.txt";
            String configPath = "\"" + @"C:\Program Files\OpenVPN\config\vpnme_fr_tcp443.ovpn" + "\"";

            do
            {
                failed = false;
                try
                {
                    //Creating a File with user credentials
                    if (File.Exists(pwPath))
                        File.Delete(pwPath);
                    File.Create(pwPath).Close();
                    File.WriteAllLines(pwPath, new List<string> { "fr-open", pw });
                }
                catch (Exception e)
                {
                    //Error handling
                    failed = true;
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine($"\nFailed to create and write to File.\nError: {e.Message} \nTrying again...");
                    Console.ResetColor();
                }
            } while (failed);

            //Creating console statements
            string vpn = "openvpn.exe --config " + configPath + " --auth-user-pass " + pwPath;
            string color = "color a";
            string cmd = "/C " + color + " & " + vpn;

            do
            {
                failed = false;
                try
                {
                    //Starting console with commands
                    Process.Start("CMD.exe", cmd);
                }
                catch (Exception e)
                {
                    //Error handling
                    failed = true;
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine($"\nFailed to open console.\nError: {e.Message} \nTrying again...");
                    Console.ResetColor();
                }
            } while (failed);
            System.Threading.Thread.Sleep(2000);
            File.Delete(pwPath);
        }
    }
}