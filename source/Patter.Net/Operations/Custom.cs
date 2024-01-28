using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Patter.Operations
{
 
    [DebuggerDisplay("Custom()")]
    internal class Custom<T> : PatternOp<T>
    {
        private Action<PatternContext<T>> _func;

        internal Custom(Action<PatternContext<T>> func)
        {
            if (func == null) throw new ArgumentNullException(nameof(func));
            _func = func ;
        }

        internal override void Execute(PatternContext<T> context)
        {
            if (context.Pos < 0)
                return;

            _func(context);
        }
    }
}
