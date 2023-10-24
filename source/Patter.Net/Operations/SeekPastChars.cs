using System.Collections.Generic;
using System.Diagnostics;

namespace Patter.Operations
{
    [DebuggerDisplay("SeekPastChars([{String.Join(String.Empty, _chars}])")]
    internal class SeekPastChars<T> : PatternOp<T>
    {
        private HashSet<char> _chars;

        internal SeekPastChars(char[] chars)
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

            while (context.Pos < context.Text.Length)
            {
                var ch = context.Text[context.Pos];
                if (!_chars.Contains(ch))
                    break;
                context.Pos++;
            }
        }
    }
}
