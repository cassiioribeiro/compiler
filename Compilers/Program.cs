using System;

namespace Compilers
{
    class Program
    {
        static void Main(string[] args)
        {
            Lexico lexer = new Lexico("testeJavinha3.jvn"); // parametro do Lexer: Um programa de acordo com a gramatica




            Console.WriteLine("Compilação completada!");
            Console.ReadLine();
        }
    }
}
