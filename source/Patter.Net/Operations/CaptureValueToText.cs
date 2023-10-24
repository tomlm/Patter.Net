using System;

namespace Patter.Operations
{
    internal class CaptureValueToText<T> : PatternOp<T>
    {
        private Action<PatternContext<T>, string> _func;
        private string _endText;
        private StringComparison _comparison;

        internal CaptureValueToText(Action<PatternContext<T>, string> func, string endText, StringComparison comparison = StringComparison.OrdinalIgnoreCase)
        {
            _func = func;
            _endText = endText;
            _comparison = comparison;
        }

        internal override void Execute(PatternContext<T> context)
        {
            var iEnd = context.Text.IndexOf(_endText, context.Pos, _comparison);
            if (iEnd > 0)
            {
                var currentText = context.Text.Substring(context.Pos, iEnd - context.Pos);
                context.HasMatch = true;

                _func(context, currentText);
            }
            context.Pos = iEnd;
        }
    }
}
