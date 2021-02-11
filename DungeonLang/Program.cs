using DungeonLang.lib;
using DungeonLang.Parser;
using DungeonLang.Parser.AST;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace DungeonLang
{
    class Program
    {
        static void Main(string[] args)
        {
            string input;
            using (var sr = new StreamReader("program.txt", Encoding.UTF8))
            {
                input = sr.ReadToEnd();
            }
            List<Token> tokens = new Lexer(input).Tokenize();
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
            Console.ReadKey();
        }
    }
}
