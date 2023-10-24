namespace Patter
{
    public class PatternContext<T>
    {
        public PatternContext(string text)
        {
            Text = text;
        }

        public int Pos { get; set; }
        public string Text { get; set; }
        public T Capture { get; set; }
    }
}
