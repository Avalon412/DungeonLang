using DungeonLang.Parser;
using DungeonLang.Parser.AST;
using System;
using System.Collections.Generic;

namespace DungeonLang
{
    class Program
    {
        static void Main(string[] args)
        {
            string input = "PI + 4";
            var tokens = new Lexer(input).Tokenize();
            foreach(var token in tokens)
            {
                Console.WriteLine(token);
            }

            List<Expression> expressions = new RDParser(tokens).Parse();
            foreach (var expr in expressions)
            {
                Console.WriteLine(expr + " = " + expr.Evaluate());
            }

            Console.ReadKey();
        }
    }
}
