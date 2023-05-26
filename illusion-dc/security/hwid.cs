using System;
using System.Collections.Generic;
using System.Linq;
using System.Management;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace illusion_dc.security
{
    internal class hwid
    {
        public static string GenerateHWID()
        {
            int cpuCores = Environment.ProcessorCount;
            int cpuThreads = cpuCores * 2; // Assuming hyper-threading
            long ramInGB = GetTotalRAM() / (1024 * 1024 * 1024);
            string gpuModel = GetGPUModel();

            string hwid = cpuCores.ToString() + cpuThreads.ToString() + ramInGB.ToString() + gpuModel;
            return CalculateSHA256(hwid);
        }

        static long GetTotalRAM()
        {
            var wmi = new System.Management.ManagementObjectSearcher("SELECT Capacity FROM Win32_PhysicalMemory");
            long capacity = 0;
            foreach (var memory in wmi.Get())
            {
                capacity += long.Parse(memory["Capacity"].ToString());
            }

            return capacity;
        }

        static string GetGPUModel()
        {
            var wmi = new System.Management.ManagementObjectSearcher("SELECT Name FROM Win32_VideoController");
            foreach (var gpu in wmi.Get())
            {
                string gpuModel = gpu["Name"].ToString();
                return gpuModel;
            }

            return string.Empty;
        }

        static string CalculateSHA256(string input)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] inputBytes = Encoding.UTF8.GetBytes(input);
                byte[] hashBytes = sha256.ComputeHash(inputBytes);

                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < hashBytes.Length; i++)
                {
                    builder.Append(hashBytes[i].ToString("x2"));
                }

                return builder.ToString();
            }
        }
    }
}
