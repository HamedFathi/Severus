using System;

namespace ParserSample
{     
    class Program
    {
        static void Main(string[] args)
        {
            var query = "  SELECT C1,C2,C3 FROM T1 WHERE C1=5.23 AN C2> 2.6 OR C3<2  ";
            var lexer = new SqlLexer(query).Tokenize();
            var parser = new SqlParser(lexer).Parse();
            Console.WriteLine("Hello World!");
        }
    }
}
