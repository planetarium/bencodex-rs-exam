using System;
using System.Runtime.InteropServices;
using System.Text;


namespace CsBindgen
{
    class Program {
        static void Main(string[] args) {
            var str = "test";
            var bytes = Encoding.UTF8.GetBytes(str);
            
            unsafe {
                fixed (byte* p = bytes) {
                    byte* result = NativeMethods.c_encode(p);
                    
                    var resultStr = new string((sbyte*)result);
                    
                    Console.WriteLine(resultStr);
                }
            }
        }
    }
}