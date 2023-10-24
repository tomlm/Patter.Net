namespace Patter
{
    internal abstract class PatternOp<T>
    {
        internal abstract T Execute(PatternContext<T> context);
    }
}
