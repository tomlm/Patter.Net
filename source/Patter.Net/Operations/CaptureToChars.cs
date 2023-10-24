using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Patter.Operations
{

    [DebuggerDisplay("CaptureToChars([{_chars}])")]
    internal class CaptureToChars<T> : PatternOp<T>
    {
        private readonly bool _skipPast;
        private Action<PatternContext<T>> _func;
        private HashSet<char> _chars;

        internal CaptureToChars(char[] chars, bool skipPast, Action<PatternContext<T>>? func)
        {
            _skipPast = skipPast;
            _func = func ?? DefaultFunc;
            _chars = new HashSet<char>(chars);
        }

        private void DefaultFunc(PatternContext<T> context)
        {
            if (typeof(T) == typeof(string))
            {
                context.Match = (T)(object)context.MatchText!;
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
                if (_chars.Contains(ch))
                    break;
                iEnd++;
            }

            if (_skipPast)
            {
                while (iEnd < context.Text.Length)
                {
                    var ch = context.Text[iEnd];
                    if (!_chars.Contains(ch))
                        break;
                    iEnd++;
                }
            }

            if (iEnd < context.Text.Length)
            {
                context.MatchText = context.Text.Substring(context.Pos, iEnd - context.Pos);
                context.HasMatch = true;
                _func(context);

                context.Pos = iEnd;
            }
            else
            {
                context.Pos = -1;
            }
        }
    }
}
