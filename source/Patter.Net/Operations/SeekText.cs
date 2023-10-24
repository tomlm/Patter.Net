﻿using System;

namespace Patter.Operations
{
    internal class SeekText<T> : PatternOp<T>
    {
        private string _seekText;
        private StringComparison _comparisonType;

        internal SeekText(string seekText, StringComparison comparisonType)
        {
            _seekText = seekText;
            _comparisonType = comparisonType;
        }

        internal override T Execute(PatternContext<T> context)
        {
            context.Pos = context.Text.IndexOf(_seekText, context.Pos, _comparisonType);
            return default;
        }
    }
}
