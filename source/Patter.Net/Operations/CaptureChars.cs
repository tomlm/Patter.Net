using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Patter.Operations
{
    [DebuggerDisplay("CaptureChars([{String.Join(String.Empty,_chars)}])")]
    internal class CaptureChars<T> : PatternOp<T>
    {
        private Action<PatternContext<T>> _func;
        private HashSet<char> _chars;

        internal CaptureChars(char[] chars, Action<PatternContext<T>> func = null)
        {
            _func = func ?? DefaultFunc;
            _chars = new HashSet<char>(chars);
        }

        private void DefaultFunc(PatternContext<T> context)
        {
            if (typeof(T) == typeof(string))
            {
                context.Match = (T)(object)context.MatchText;
            }
        }

        internal override void Execute(PatternContext<T> context)
        {
            if (context.Pos < 0)
                return;

            var iEnd = context.Pos;
            while (iEnd < context.Text.Length)
            {
                var ch = context.Text[iEnd];
                if (!_chars.Contains(ch))
                    break;
                context.HasMatch = true;
                iEnd++;
            }
            if (context.HasMatch)
            {
                context.MatchText = context.Text.Substring(context.Pos, iEnd - context.Pos);
                _func(context);
                context.Pos = iEnd;
            }
            
            if (iEnd  == context.Text.Length)
            {
                context.Pos = -1;
            }
        }
    }
}
