using System;

namespace Patter.Operations
{
    internal class SkipText<T> : PatternOp<T>
    {
        private string _skipText;
        private StringComparer _comparer;

        internal SkipText(string skipText, StringComparer? comparer = null)
        {
            _skipText = skipText;
            _comparer = comparer ?? StringComparer.OrdinalIgnoreCase;
        }

        internal override void Execute(PatternContext<T> context)
        {
            var currentText = context.Text.Substring(context.Pos, _skipText.Length);
            if (_comparer.Compare(currentText, _skipText) == 0)
                context.Pos = context.Pos + _skipText.Length;
        }
    }
}
