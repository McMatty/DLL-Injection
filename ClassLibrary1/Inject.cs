using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace InjectedLibrary
{
    public class Inject
    {
        public static int CallInjected(string argument)
        {
           var result = string.Format(@"Aww shit, injected into a different process: {0}. Running as {1}", Process.GetCurrentProcess().ProcessName, WindowsIdentity.GetCurrent().Name);
           File.WriteAllText(@"C:\temp\injection.txt", result);

           return 0;
        }
    }
}
