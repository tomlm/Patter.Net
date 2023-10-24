using System;

namespace Patter.Operations
{
    internal class StartCapture<T> : PatternOp<T>
    {
        internal override T Execute(PatternContext<T> context)
        {
            context.Capture = Activator.CreateInstance<T>();
            return default(T);
        }
    }
}
