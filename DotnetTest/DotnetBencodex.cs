using Bencodex;
using Bencodex.Types;

namespace CsBindgen
{
    public class DotnetBencodex
    {
        public DotnetBencodex()
        {

        }

        public byte[] Encode(string input) {
            var codec = new Codec();

            return codec.Encode((Text) input);
        }

        public IValue Decode(byte[] input) {
            var codec = new Codec();

            return codec.Decode(input);
        }
    }
}