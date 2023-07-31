using System;
using System.Runtime.InteropServices;

class Program {
    [DllImport("libbencodex.dylib")]
    public static extern int Encode(int a);

    static void Main(string[] args) {
        var testvalue = Encode(1);
        Console.WriteLine(testvalue);
    }
}