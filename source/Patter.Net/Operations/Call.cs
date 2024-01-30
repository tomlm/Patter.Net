using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Patter.Operations
{
 
    [DebuggerDisplay("Call()")]
    internal class Call<T> : PatternOp<T>
    {
        private Action<PatternContext<T>> _func;

        internal Call(Action<PatternContext<T>> func)
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
