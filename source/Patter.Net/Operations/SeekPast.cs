using System;
using System.Diagnostics;

namespace Patter.Operations
{

    [DebuggerDisplay("SeekPast('{_seekText}', {_comparisonType})")]
    internal class SeekPast<T> : PatternOp<T>
    {
        private string _seekText;
        private StringComparison _comparisonType;

        internal SeekPast(string seekText, StringComparison comparisonType)
        {
            _seekText = seekText;
            _comparisonType = comparisonType;
        }

        internal override void Execute(PatternContext<T> context)
        {
            if (context.Pos < 0)
                return;

            context.Pos = context.Text.IndexOf(_seekText, context.Pos, _comparisonType);
            if (context.Pos > 0)
                context.Pos += _seekText.Length;

        }
    }
}
