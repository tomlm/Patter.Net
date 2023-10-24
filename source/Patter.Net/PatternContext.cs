using System;
using System.Diagnostics;

namespace Patter
{
    [DebuggerDisplay("[{Pos}] {(Pos >=0) ? Text.Substring(Pos) : \"EOF\"}")]
    public class PatternContext<T>
    {
        public PatternContext(string text)
        {
            Text = text;
        }

        public int Pos { get; set; }
        
        public string Text { get; set; }

        public char CurrentChar { get => (Pos > 0) ? Text[Pos] : Char.MinValue;  }

        public T? CurrentMatch { get; set; }

        public bool HasMatch { get; set; } = false;

        public void ResetMatch()
        {
            HasMatch = false;
            if (typeof(T).IsValueType || typeof(T) == typeof(string))
                CurrentMatch = default(T);
            else
                CurrentMatch = Activator.CreateInstance<T>();
        }
    }
}
