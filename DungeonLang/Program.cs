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

            IStatement program = new RDParser(tokens).Parse();
            Console.WriteLine(program.ToString());
            program.Execute();
            Console.ReadKey();
        }
    }
}
