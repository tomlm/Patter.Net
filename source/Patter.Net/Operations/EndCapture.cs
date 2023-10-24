namespace Patter.Operations
{
    internal class EndCapture<T> : PatternOp<T>
    {
        internal override T Execute(PatternContext<T> context)
        {
            var capture = context.Capture;
            context.Capture = default(T);
            return capture;
        }
    }
}
