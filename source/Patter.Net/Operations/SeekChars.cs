using System.Collections.Generic;
using System.Diagnostics;

namespace Patter.Operations
{
    [DebuggerDisplay("SeekChars([{String.Join(String.Empty, _chars}])")]
    internal class SeekChars<T> : PatternOp<T>
    {
        private HashSet<char> _chars;

        internal SeekChars(char[] chars)
        {
            _chars = new HashSet<char>(chars);
        }

        internal override void Execute(PatternContext<T> context)
        {
            if (context.Pos < 0)
                return;

            while (context.Pos < context.Text.Length)
            {
                var ch = context.Text[context.Pos];
                if (_chars.Contains(ch))
                    break;
                context.Pos++;
            }
            if (context.Pos == context.Text.Length)
            {
                context.Pos = -1;
            }
        }
    }
}
