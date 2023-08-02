using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text;
using Bencodex.Types;

namespace CsBindgen
{
    public class Program
    {
        public static void Main(string[] args)
        {
            // string cleanedHexforDecode = Regex.Replace(TestData.BencodexHex, @"[\s.,:;_-]+", "");
            // byte[] bytes = StringToByteArray(TestData.EncodedBencodexString);
            byte[] bytes = Encoding.UTF8.GetBytes(TestData.EncodedBencodexString); ;
            string encodeInput = TestData.BencodexHex;

            var program = new Program();

            // RunBenchmark("Rust Bencodex Encode", () => program.RunRustBencodexEncode(encodeInput));
            RunBenchmark("Rust Bencodex Decode", () => program.RunRustBencodexDecode(bytes));
            // RunBenchmark("Dotnet Bencodex Encode", () => program.RunDotnetBencodexEncode(encodeInput), 1);
            RunBenchmark("Dotnet Bencodex Decode", () => program.RunDotnetBencodexDecode(bytes));
        }

        public static void RunBenchmark(string name, Action action, int iterations = 1000)
        {
            var stopwatch = new Stopwatch();
            stopwatch.Start();
            for (int i = 0; i < iterations; i++)
            {
                action();
            }
            stopwatch.Stop();
            Console.WriteLine($"{name} took {stopwatch.ElapsedMilliseconds}ms");
        }

        public void RunRustBencodexEncode(string input)
        {
            byte[] strBytes = Encoding.ASCII.GetBytes(input);
            byte[] nullTerminatedStrBytes = new byte[strBytes.Length + 1];
            Array.Copy(strBytes, nullTerminatedStrBytes, strBytes.Length);
            nullTerminatedStrBytes[strBytes.Length] = 0;

            unsafe
            {
                fixed (byte* pStrBytes = nullTerminatedStrBytes)
                {
                    byte* result = NativeMethods.c_encode(pStrBytes);

                    string resultStr = Marshal.PtrToStringUTF8(new IntPtr(result));
                    // Console.WriteLine(resultStr);

                    NativeMethods.c_free(result);
                }
            }
        }

        public void RunRustBencodexDecode(byte[] input)
        {
            // Get pointer to the byte array.
            unsafe 
            {
                fixed (byte* pInput = input)
                {
                    byte* result = NativeMethods.c_decode(pInput, (nuint)input.Length);
                    IValue resultString = (Text) Marshal.PtrToStringAnsi((IntPtr)result);
                    NativeMethods.c_free(result);
                    // Console.WriteLine("Decoded string: " + resultString);
                }
            }
        }

        public void RunDotnetBencodexEncode(string input)
        {
            var r = new DotnetBencodex().Encode(input);
            string resultStr = Encoding.ASCII.GetString(r);
            // Console.WriteLine(resultStr);
        }

        public void RunDotnetBencodexDecode(byte[] input)
        {
            var r = new DotnetBencodex().Decode(input);
            // Console.WriteLine(r);
        }

        public static byte[] StringToByteArray(string hex)
        {
            int length = hex.Length;
            byte[] bytes = new byte[length / 2];

            for (int i = 0; i < length; i += 2)
            {
                bytes[i / 2] = Convert.ToByte(hex.Substring(i, 2), 16);
            }

            return bytes;
        }
    }
}
