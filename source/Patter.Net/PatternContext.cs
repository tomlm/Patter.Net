using System;
using System.Collections.Generic;
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
        /// <remarks>
        /// Text[Pos] or Char.MinValue if end of file.
        /// </remarks>
        public char CurrentChar { get => (Pos >= 0) ? Text[Pos] : Char.MinValue;  }

        /// <summary>
        /// The current text that has been captured.
        /// </summary>
        public string MatchText { get; internal set; }

        /// <summary>
        /// The object that will be returned as a result at the end of the pattern evaluation.
        /// </summary>
        public T Match { get; set; }

        /// <summary>
        /// Temporary memory which is scoped to each match
        /// </summary>
        /// <remarks>
        /// You can use this to store additional properties while processing a match. The data in this
        /// object will be reset when a pattern is completed and match is returned to the caller.
        /// </remarks>
        public Dictionary<string, object> MatchMemory { get; private set; } = new Dictionary<string, object>();

        /// <summary>
        /// Temporary memory which is scoped for all matches
        /// </summary>
        /// <remarks>
        /// You can use this memory to track information across matches.
        /// </remarks>
        public Dictionary<string, object> Memory { get; private set; } = new Dictionary<string, object>();

        /// <summary>
        /// Flag which indicates content has been captured.
        /// </summary>
        public bool HasMatch { get; set; } = false;

        public void ResetMatch()
        {
            HasMatch = false;
            if (typeof(T).IsValueType || typeof(T) == typeof(string))
                Match = default(T);
            else
                Match = Activator.CreateInstance<T>();

            MatchMemory = new Dictionary<string, object>();
        }
    }
}
