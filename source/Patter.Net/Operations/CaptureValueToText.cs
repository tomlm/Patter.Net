using System;

namespace Patter.Operations
{
    internal class CaptureValueToText<T> : PatternOp<T>
    {
        private Action<T, string, int, int> _func;
        private string _endText;
        private StringComparison _comparison;

        internal CaptureValueToText(Action<T, string, int, int> func, string endText, StringComparison comparison = StringComparison.OrdinalIgnoreCase)
        {
            _func = func;
            _endText = endText;
            _comparison = comparison;
        }

        internal override T Execute(PatternContext<T> context)
        {
            var iEnd = context.Text.IndexOf(_endText, context.Pos, _comparison);
            if (iEnd < 0)
                iEnd = context.Text.Length;
            var currentText = context.Text.Substring(context.Pos, iEnd - context.Pos);
            _func(context.Capture, currentText, context.Pos, iEnd);
            context.Pos = iEnd;
            return default(T);
        }
    }
}
