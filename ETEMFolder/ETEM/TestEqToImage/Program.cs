using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TestEqToImage
{
    class Program
    {
        private static object _MimeTeXCallLocker = new object();

        static void Main(string[] args)
        {

            lock (_MimeTeXCallLocker)
            {
                NativeMethods.CreateGifFromEq("\\Large x^2+y^3=z^2", "c:\\EqToImage_" + DateTime.Now.Ticks.ToString() + ".gif");
            }
        }

    }

    [System.Security.SuppressUnmanagedCodeSecurity()]
    internal class NativeMethods
    {
        private NativeMethods()
        { //all methods in this class would be static
        }

        [System.Runtime.InteropServices.DllImport("MimeTex.dll")]
        internal static extern int CreateGifFromEq(string expr, string fileName);

        [System.Runtime.InteropServices.DllImport("kernel32.dll")]
        internal extern static IntPtr GetModuleHandle(string lpModuleName);

        [System.Runtime.InteropServices.DllImport("kernel32.dll")]
        [return: System.Runtime.InteropServices.MarshalAs(System.Runtime.InteropServices.UnmanagedType.Bool)]
        internal extern static bool FreeLibrary(IntPtr hLibModule);
    }

}
