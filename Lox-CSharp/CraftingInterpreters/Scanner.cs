using System.Data.Common;
using System.Runtime.ConstrainedExecution;

namespace Lox_CSharp.CraftingInterpreters
{
    internal class Scanner
    {
        private static readonly Dictionary<string, TokenType> Keywords;

        static Scanner()
        {
            Keywords = new Dictionary<string, TokenType>
            {
                { "and", TokenType.And },
                { "class", TokenType.Class },
                { "else", TokenType.Else },
                { "false", TokenType.False },
                { "for", TokenType.For },
                { "fun", TokenType.Fun },
                { "if", TokenType.If },
                { "nil", TokenType.Nil },
                { "or", TokenType.Or },
                { "print", TokenType.Print },
                { "return", TokenType.Return },
                { "super", TokenType.Super },
                { "this", TokenType.This },
                { "true", TokenType.True },
                { "var", TokenType.Var },
                { "while", TokenType.While }
            };
        }

        private readonly string Source;
        private readonly List<Token> Tokens = [];

        private int Start = 0;
        private int Current = 0;
        private int Line = 1;

        public Scanner(string source)
        {
            Source = source;
        }

        internal List<Token> ScanTokens()
        {
            while (!IsAtEnd())
            {
                Start = Current;
                ScanToken();
            }

            Tokens.Add(new Token(TokenType.EOF, "", null, Line));
            return Tokens;
        }

        private bool IsAtEnd()
        {
            return Current >= Source.Length;
        }

        private void ScanToken()
        {
            char c = Advance();
            switch (c)
            {
                case '(': AddToken(TokenType.LeftParen); break;
                case ')': AddToken(TokenType.RightParen); break;
                case '{': AddToken(TokenType.LeftBrace); break;
                case '}': AddToken(TokenType.RightBrace); break;
                case ',': AddToken(TokenType.Comma); break;
                case '.': AddToken(TokenType.Dot); break;
                case '-': AddToken(TokenType.Minus); break;
                case '+': AddToken(TokenType.Plus); break;
                case ';': AddToken(TokenType.Semicolon); break;
                case '*': AddToken(TokenType.Star); break;
                case '!':
                    AddToken(Match('=') ? TokenType.BangEqual : TokenType.Bang);
                    break;
                case '=':
                    AddToken(Match('=') ? TokenType.EqualEqual : TokenType.Equal);
                    break;
                case '<':
                    AddToken(Match('=') ? TokenType.LessEqual : TokenType.Less);
                    break;
                case '>':
                    AddToken(Match('=') ? TokenType.GreaterEqual : TokenType.Greater);
                    break;
                case '/':
                    if (Match('/'))
                    {
                        while (Peek() != '\n' && !IsAtEnd()) Advance();
                    }
                    else
                    {
                        AddToken(TokenType.Slash);
                    }
                    break;
                case ' ':
                case '\r':
                case '\t':
                    break;
                case '\n':
                    Line++;
                    break;
                case '"': String(); break;
                case 'o':
                    if (Peek() == 'r')
                    {
                        AddToken(TokenType.Or);
                    }
                    break;
                default:
                    if (char.IsDigit(c))
                    {
                        Number();
                    }
                    else if (IsAlpha(c))
                    {
                        Identifier();
                    }
                    else
                    {
                        Lox.Error(Line, "Unexpected character.");
                    }
                    break;
            }
        }

        private char Advance()
        {
            return Source[Current++];
        }

        private void AddToken(TokenType type)
        {
            AddToken(type, null);
        }

        private void AddToken(TokenType type, object? value)
        {
            string text = Source.Substring(Start, Current - Start);
            Tokens.Add(new Token(type, text, value, Line));
        }

        private bool Match(char expected)
        {
            if (IsAtEnd()) return false;
            if (Source[Current] != expected) return false;
            Current++;
            return true;
        }

        private char Peek()
        {
            if (IsAtEnd()) return '\0';
            return Source[Current];
        }

        private void String()
        {
            while (Peek() != '"' && !IsAtEnd())
            {
                if (Peek() == '\n') Line++;
                Advance();
            }

            if (IsAtEnd())
            {
                Lox.Error(Line, "Unterminated string.");
                return;
            }

            Advance();
            string value = Source.Substring(Start + 1, Current - Start - 2);
            AddToken(TokenType.String, value);
        }

        private void Number()
        {
            while (char.IsDigit(Peek())) Advance();

            if (Peek() == '.' && char.IsDigit(PeekNext()))
            {
                Advance();
                while (char.IsDigit(Peek())) Advance();
            }
            AddToken(TokenType.Number, double.Parse(Source.Substring(Start, Current - Start)));
        }

        private char PeekNext()
        {
            if (Current + 1 >= Source.Length) return '\0';
            return Source[Current + 1];
        }

        private static bool IsAlpha(char c) =>
            (c >= 'a' && c <= 'z') ||
            (c >= 'A' && c <= 'Z') ||
            c == '_';

        private void Identifier()
        {
            while (IsAlphaNumeric(Peek())) Advance();
            string text = Source.Substring(Start, Current - Start);
            TokenType tokenType = Keywords.TryGetValue(text, out var type) ? type : TokenType.Identifier;
            AddToken(tokenType);
        }

        private static bool IsAlphaNumeric(char c)
        {
            return IsAlpha(c) || char.IsDigit(c);
        }
    }
}
