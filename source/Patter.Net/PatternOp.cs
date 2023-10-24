namespace Patter
{
    internal abstract class PatternOp<T>
    {
        internal abstract void Execute(PatternContext<T> context);
    }
}
