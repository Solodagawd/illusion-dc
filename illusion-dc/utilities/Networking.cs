using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace illusion_dc.utilities
{
    internal class Networking
    {

        public static string[] GetIP(int pid)
        {
            Process process = Process.GetProcessById(pid);

            if (process == null)
            {
                return null;
            }

            string output = RunNetstat();
            string[] lines = output.Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);

            Regex regexTcp = new Regex(@"(?<protocol>TCP)\s+(?<localAddress>[^\s]+):(?<localPort>\d+)\s+(?<foreignAddress>[^\s]+):(?<foreignPort>\d+)\s+(?<state>[^\s]+)\s+(?<pid>\d+)");
            Regex regexUdp = new Regex(@"(?<protocol>UDP)\s+(?<localAddress>[^\s]+):(?<localPort>\d+)\s+(?<foreignAddress>[^\s]+):(?<foreignPort>\d+)\s+(?<pid>\d+)");

            string[] results = new string[0];
            foreach (string line in lines)
            {
                Match matchTcp = regexTcp.Match(line);
                Match matchUdp = regexUdp.Match(line);

                if (matchTcp.Success && matchTcp.Groups["pid"].Value == pid.ToString())
                {
                    string localIp = matchTcp.Groups["localAddress"].Value;
                    int localPort = int.Parse(matchTcp.Groups["localPort"].Value);

                    string foreignIp = matchTcp.Groups["foreignAddress"].Value;
                    int foreignPort = int.Parse(matchTcp.Groups["foreignPort"].Value);

                    string protocol = matchTcp.Groups["protocol"].Value;

                    string result = $"{localIp}:{localPort}:{foreignIp}:{foreignPort}:{protocol}";
                    Array.Resize(ref results, results.Length + 1);
                    results[results.Length - 1] = result;
                }
                else if (matchUdp.Success && matchUdp.Groups["pid"].Value == pid.ToString())
                {
                    string localIp = matchUdp.Groups["localAddress"].Value;
                    int localPort = int.Parse(matchUdp.Groups["localPort"].Value);

                    string foreignIp = matchUdp.Groups["foreignAddress"].Value;
                    int foreignPort = int.Parse(matchUdp.Groups["foreignPort"].Value);

                    string protocol = matchUdp.Groups["protocol"].Value;

                    string result = $"{localIp}:{localPort}:{foreignIp}:{foreignPort}:{protocol}";
                    Array.Resize(ref results, results.Length + 1);
                    results[results.Length - 1] = result;
                }
            }





            return results;

        }


        static string RunNetstat()
        {
            ProcessStartInfo psi = new ProcessStartInfo("netstat", "-ano");
            psi.RedirectStandardOutput = true;
            psi.UseShellExecute = false;
            psi.CreateNoWindow = true;

            Process process = new Process();
            process.StartInfo = psi;
            process.Start();

            string output = process.StandardOutput.ReadToEnd();

            process.WaitForExit();

            return output;

        }
    }
}
