using DungeonLang.lib;
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
            string input = "Word = 2 + 2";
            var tokens = new Lexer(input).Tokenize();
            foreach(var token in tokens)
            {
                Console.WriteLine(token);
            }

            List<Statement> statements = new RDParser(tokens).Parse();
            foreach (var state in statements)
            {
                Console.WriteLine(state);
            }
            foreach (var state in statements)
            {
                state.Execute();
            }
            Console.WriteLine($"Word = {Variables.Get("Word")}");
            Console.WriteLine($"Word2 = {Variables.Get("Word2")}");
            Console.ReadKey();
        }
    }
}
