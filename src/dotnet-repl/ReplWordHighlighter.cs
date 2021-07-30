using RadLine;
using Spectre.Console;
using System.Collections.Generic;

namespace dotnet_repl
{
    using WordsToHighlight = IEnumerable<(Style style, string[] words)>;

    internal static class ReplWordHighlighter
    {
        private static void AddWords(this WordHighlighter highlighter, WordsToHighlight wordsToHighlight)
        {
            foreach (var (style, wordsToAdd) in wordsToHighlight)
                foreach (var word in wordsToAdd)
                    highlighter.AddWord(word, style);
        }

        public static WordHighlighter Create(string languageName)
        {
            var wordHighlighter = new WordHighlighter();

            wordHighlighter.AddWords(sharedWordsToHighlight);

            if (languageName is "csharp")
                wordHighlighter.AddWords(csharpOnlyWordsToHighlight);
            else if (languageName is "fsharp")
                wordHighlighter.AddWords(fsharpOnlyWordsToHighlight);
            else
                throw new System.NotSupportedException($"Kernel {languageName} has no keyword support.");

            return wordHighlighter;
        }

        

        private static readonly WordsToHighlight sharedWordsToHighlight = new[]
            {
                "async",
                "await",
                "bool",
                "break",
                "case",
                "catch",
                "class",
                "else",
                "finally",
                "for",
                "foreach",
                "fun",
                "if",
                "in",
                "int",
                "interface",
                "internal",
                "let",
                "match",
                "member",
                "mutable",
                "new",
                "not",
                "null",
                "open",
                "override",
                "private",
                "protected",
                "public",
                "record",
                "typeof",
                "return",
                "string",
                "struct",
                "switch",
                "then",
                "try",
                "type",
                "use",
                "using",
                "var",
                "void",
                "when",
                "while",
                "with",
            };

        private static readonly WordsToHighlight csharpOnlyWordsToHighlight = new[]
            {
                // C#-only keywords
                (new Style(foreground: Color.LightSlateBlue),
                 new []
                 {
                    "async", "await", "break", "case", "catch",
                    "class", "else", "for", "foreach", "if",
                    "in", "interface", "internal", 
                    "override", "or", "return", "record",
                    "switch", "try",  "using", "var", "void",
                    "while"
                 }),

                 // C#-only operators
                 (new Style(foreground: Color.SteelBlue1_1),
                  new []
                  {
                     ";"
                  }),
            };

        private static readonly WordsToHighlight fsharpOnlyWordsToHighlight = new[]
            {
                // F# keywords (ref. https://docs.microsoft.com/en-us/dotnet/fsharp/language-reference/keyword-reference)
                 (new Style(foreground: Color.LightSlateBlue),
                  new []
                  {
                     "elif", "fun", "function", "inline", "lazy", "let",
                     "match", "member", "mutable", "of", "open", "rec", 
                     "then", "to", "type", "val", "with", "yield"
                  }),

                 // F# operators
                 (new Style(foreground: Color.SteelBlue1_1),
                  new []{
                     "|>", "->"
                  })
            };
    }
}
