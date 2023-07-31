using System;
using System.Runtime.InteropServices;

namespace CsBindgen
{
    class Program {
        static void Main(string[] args) {
            var r = NativeMethods.my_add(1, 2);
            Console.WriteLine(r);
        }
    }
}