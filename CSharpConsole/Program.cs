using System;
using System.Text;
using System.Runtime.InteropServices;
using System.Diagnostics;
using System.IO;

namespace DllInjector
{
    partial class Program
    {      
        public static string injectionFunctionName = "Inject";       
        public static uint _MAX_PATH = 260;

        static void Main(string[] args)
        {
            Process.EnterDebugMode(); // Get those permissions

            //string libPath = Path.Combine(Environment.CurrentDirectory, @"InjectionFiles\BootStrap.dll");
            string libPath = @"C:\GitRepositories\Dll injection\x64\Release\BootStrap.dll";
            Console.WriteLine("[*] DLL injection demo");         

            if(!File.Exists(libPath))
            {
                Console.WriteLine(string.Format("Path to injection library not found : {0}. Exiting.", libPath));
                Console.ReadLine();
                return;
            }

            byte[] LibPathBytes = Encoding.Unicode.GetBytes(libPath);

            Console.WriteLine(string.Format("'{0}' will attempt to loaded into the process.", libPath));
            Console.WriteLine("PID: ");
            int PID = int.Parse(Console.ReadLine());                   

            // Open the process and get a nice handle
            IntPtr hProcess = OpenProcess(ProcessAccessFlags.All, false, PID);
            
            // Create space size of our LibPath
            IntPtr pLibRemote = VirtualAllocEx(hProcess, IntPtr.Zero, (uint)libPath.Length, AllocationType.Commit, MemoryProtection.ReadWrite);
            IntPtr Out;
            // Write our bytes into the created space
            WriteProcessMemory(hProcess, pLibRemote, LibPathBytes, LibPathBytes.Length, out Out);

            // Get a nice kernel32 handle
            IntPtr krnl32 = GetModuleHandle("Kernel32");

            //IntPtr handle = CreateRemoteThread(hProcess, (IntPtr)null, 0, GetProcAddress(krnl32, "LoadLibraryW"), pLibRemote, 0, IntPtr.Zero);

            UInt32 handle = RtlCreateUserThread(hProcess, IntPtr.Zero, false, 0, IntPtr.Zero, IntPtr.Zero, GetProcAddress(krnl32, "LoadLibraryW"), pLibRemote, IntPtr.Zero, IntPtr.Zero);

            var address = RetrieveModuleAddress(Process.GetProcessById(PID), libPath);
            if (address == IntPtr.Zero) 
            {
                Console.WriteLine(string.Format("'{0}' was not loaded into the process.", libPath));
            }
            
            Console.ReadKey();
        }

        static IntPtr RetrieveModuleAddress(Process process, string libraryPath)
        {
            foreach (ProcessModule module in process.Modules)
            {
                if (module.FileName.ToLower().Equals(libraryPath.ToLower()))
                {
                    return module.BaseAddress;
                }
            }

            return IntPtr.Zero;
        }

        static IntPtr RetrieveFunctionAddress(Process process, string libraryPath, string functionName)
        {
            var moduleAddress = RetrieveModuleAddress(process, libraryPath);
            if (moduleAddress != IntPtr.Zero)
            {
                return GetProcAddress(moduleAddress, functionName);
            }

            return IntPtr.Zero;
        }
    }
}