using System;
using System.Diagnostics;

namespace Patter.Operations
{
    [DebuggerDisplay("CaptureToText('{_endText}, {_comparison})")]
    internal class CaptureToText<T> : PatternOp<T>
    {
        private Action<PatternContext<T>> _func;
        private string _endText;
        private StringComparison _comparison;

        internal CaptureToText(string endText, StringComparison comparison, Action<PatternContext<T>> func)
        {
            _func = func ?? DefaultFunc;
            _endText = endText;
            _comparison = comparison;
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

            var iEnd = context.Text.IndexOf(_endText, context.Pos, _comparison);
            if (iEnd > 0)
            {
                context.MatchText = context.Text.Substring(context.Pos, iEnd - context.Pos);
                context.HasMatch = true;

                _func(context);
            }
            context.Pos = iEnd;
        }
    }
}
