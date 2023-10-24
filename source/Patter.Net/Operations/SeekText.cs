using System;
using System.Diagnostics;

namespace Patter.Operations
{
    [DebuggerDisplay("SeekText('{_seekText}', {_comparisonType})")]
    internal class SeekText<T> : PatternOp<T>
    {
        private string _seekText;
        private StringComparison _comparisonType;

        internal SeekText(string seekText, StringComparison comparisonType)
        {
            _seekText = seekText;
            _comparisonType = comparisonType;
        }

        internal override void Execute(PatternContext<T> context)
        {
            if (context.Pos < 0)
                return;

            context.Pos = context.Text.IndexOf(_seekText, context.Pos, _comparisonType);
        }
    }
}
