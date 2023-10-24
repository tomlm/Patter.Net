using System;
using System.Collections.Generic;

namespace Patter.Operations
{
    internal class CaptureValueToChars<T> : PatternOp<T>
    {
        private Action<T, string, int, int> _func;
        private HashSet<char> _chars;

        internal CaptureValueToChars(Action<T, string, int, int> func, params char[] chars)
        {
            _func = func;
            _chars = new HashSet<char>(chars);
        }


        internal override T Execute(PatternContext<T> context)
        {
            var iEnd = context.Pos;
            while (iEnd < context.Text.Length)
            {
                var ch = context.Text[iEnd];
                if (_chars.Contains(ch))
                    break;
                iEnd++;
            }
            var currentText = context.Text.Substring(context.Pos, iEnd - context.Pos);
            _func(context.Capture, currentText, context.Pos, iEnd);
            context.Pos = iEnd;
            return default(T);
        }
    }
}
