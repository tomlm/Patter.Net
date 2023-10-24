using System;
using System.Diagnostics;

namespace Patter.Operations
{
    [DebuggerDisplay("SkipText('{_skipText}',{_comparisonType})")]
    internal class SkipText<T> : PatternOp<T>
    {
        private string _skipText;
        private StringComparison _comparison;

        internal SkipText(string skipText, StringComparison comparison)
        {
            _skipText = skipText;
            _comparison = comparison;
        }

        internal override void Execute(PatternContext<T> context)
        {
            if (context.Pos < 0)
                return;

            var currentText = context.Text.Substring(context.Pos, _skipText.Length);
            if (String.Compare(currentText, _skipText, _comparison) == 0)
                context.Pos = context.Pos + _skipText.Length;
        }
    }
}
