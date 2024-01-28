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

        /// <summary>
        /// Current Position
        /// </summary>
        public int Pos { get; set; }
        
        /// <summary>
        /// Full text that is being matched against.
        /// </summary>
        public string Text { get; private set; }

        /// <summary>
        /// Current character 
        /// </summary>
        public char CurrentChar { get => (Pos >= 0) ? Text[Pos] : Char.MinValue;  }

        public string MatchText { get; internal set; }

        public T Match { get; set; }

        public bool HasMatch { get; set; } = false;

        public void ResetMatch()
        {
            HasMatch = false;
            if (typeof(T).IsValueType || typeof(T) == typeof(string))
                Match = default(T);
            else
                Match = Activator.CreateInstance<T>();
        }
    }
}
