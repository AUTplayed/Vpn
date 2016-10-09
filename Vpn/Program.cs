using System;
using System.Diagnostics;
using System.Net;
using System.Windows.Automation;

namespace Vpn
{
    internal class Program
    {
        [STAThread]
        private static void Main(string[] args)
        {
            string html = new WebClient().DownloadString("https://www.vpnme.me/freevpn.html");
            string split = html.Split(new[] { "features-table2" }, StringSplitOptions.None)[1];
            split = split.Split(new[] { "<tbody>" }, StringSplitOptions.None)[1];
            string[] td = split.Split(new[] { "<td>" }, StringSplitOptions.None);
            string pw = td[7].Split('<')[0];
            Console.WriteLine(pw);
            System.Windows.Forms.Clipboard.SetText(pw);

            //Autoinput tries
            string automationId = "181";
            string newTextBoxValue = "testing";
            var condition = new PropertyCondition(AutomationElement.AutomationIdProperty, automationId);
            var textBox = AutomationElement.RootElement.FindFirst(TreeScope.Subtree, condition);
            object test = null;
            var pat = textBox.GetSupportedPatterns();
            Console.WriteLine(pat.Length);
            foreach (var p in pat)
            {
                Console.WriteLine(p.ProgrammaticName);
            }
            Console.WriteLine(textBox.TryGetCurrentPattern(ValuePattern.Pattern, out test));
            Console.WriteLine(test);
            /*ValuePattern vPattern = (ValuePattern)textBox.GetCurrentPattern(ValuePattern.Pattern);
            vPattern.SetValue(newTextBoxValue);*/
            Console.ReadKey();
        }
    }
}