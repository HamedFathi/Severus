﻿using System;
using System.Collections.Generic;
using System.IO;

namespace Severus
{
    public abstract class LexerWand<TToken> where TToken : Enum
    {
        private readonly string _input;
        protected int Position { get; set; } = 0;
        protected int Line { get; set; } = 1;
        protected int Column { get; set; } = 0;
        private StringReader Reader { get; set; }
        protected LexerWand(string input)
        {
            Reader = new StringReader(input);
            _input = input;
        }
        protected abstract Token<TToken> ReadToken();

        public LexerResult<TToken> Tokenize()
        {
            var tokens = new List<Token<TToken>>();
            while (true)
            {
                if (IsEndOfFile()) break;
                var token = ReadToken();
                if (token == null) break;
                tokens.Add(token);
            }
            return new LexerResult<TToken> { Input = _input, Tokens = tokens };
        }

        protected bool IsMatch(char ch)
        {
            if (!IsEndOfFile())
            {
                return ch == Peek();
            }
            return false;
        }

        protected string ReadWhile(Func<char, bool> predicate)
        {
            var result = "";
            while (!IsEndOfFile() && predicate(Peek()))
            {
                result += Read();
            }
            return result;
        }
        protected void SkipWhile(Func<char, bool> predicate)
        {
            ReadWhile(predicate);
        }

        protected string ReadLine(bool movePoinerToNextLine = false)
        {
            var result = ReadWhile(ch => ch != '\n');
            if (movePoinerToNextLine)
            {
                Skip();
            }
            return result;
        }

        protected void SkipLine(bool movePoinerToNextLine = false)
        {
            if (movePoinerToNextLine)
                ReadLine(movePoinerToNextLine: true);
            else
                ReadLine();
        }

        protected string ReadEscaped(char ch, char indicator)
        {
            var escaped = false;
            var result = "";
            Read();
            while (!IsEndOfFile())
            {
                var c = Read();
                if (escaped)
                {
                    result += c;
                    escaped = false;
                }
                else if (c == indicator)
                {
                    escaped = true;
                }
                else if (c == ch)
                {
                    break;
                }
                else
                {
                    result += c;
                }
            }
            return result;
        }
        protected virtual string ReadIntegerNumber()
        {
            var num = ReadWhile(ch => ch.IsDigit());
            return num;
        }

        protected virtual string ReadFloatNumber()
        {
            var hasDot = false;
            var num = ReadWhile(ch =>
            {
                if (ch == '.')
                {
                    if (hasDot) return false;
                    hasDot = true;
                    return true;
                }
                return ch.IsDigit();
            });

            return num;
        }


        protected char Peek()
        {
            return (char)Reader.Peek();
        }

        protected char? Read()
        {
            var value = Reader.Read();
            ++Position;

            if (value == -1)
                return null;

            var ch = (char)value;

            if (ch == '\n')
            {
                Line++;
                Column = 0;
            }
            else
            {
                ++Column;
            }
            return ch;
        }

        protected void Skip()
        {
            Read();
        }

        protected bool IsEndOfFile()
        {
            return Reader.Peek() == -1;
        }
    }
}
