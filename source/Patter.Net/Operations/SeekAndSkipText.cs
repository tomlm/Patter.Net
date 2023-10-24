using System;

namespace Patter.Operations
{
    internal class SeekAndSkipText<T> : PatternOp<T>
    {
        private string _seekText;
        private StringComparison _comparisonType;

        internal SeekAndSkipText(string seekText, StringComparison comparisonType)
        {
            _seekText = seekText;
            _comparisonType = comparisonType;
        }

        internal override void Execute(PatternContext<T> context)
        {
            context.Pos = context.Text.IndexOf(_seekText, context.Pos, _comparisonType);
            if (context.Pos > 0)
                context.Pos += _seekText.Length;

        }
    }
}
