using System;
using System.Collections.Generic;

namespace Patter.Operations
{
    internal class CaptureValueToChars<T> : PatternOp<T>
    {
        private Action<PatternContext<T>, string> _func;
        private HashSet<char> _chars;

        internal CaptureValueToChars(Action<PatternContext<T>, string> func, params char[] chars)
        {
            _func = func;
            _chars = new HashSet<char>(chars);
        }


        internal override void Execute(PatternContext<T> context)
        {
            var iEnd = context.Pos;
            while (iEnd < context.Text.Length)
            {
                var ch = context.Text[iEnd];
                if (_chars.Contains(ch))
                    break;
                iEnd++;
            }
            if (iEnd < context.Text.Length)
            {
                var currentText = context.Text.Substring(context.Pos, iEnd - context.Pos);
                context.HasMatch = true;
                _func(context, currentText);
                context.Pos = iEnd;
            }
            else
            {
                context.Pos = -1;
            }
        }
    }
}
