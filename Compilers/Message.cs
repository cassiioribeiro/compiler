using System;

namespace Compilers
{
    public static class Message
    {
        /// <summary>
        /// Mostra o token.
        /// </summary>
        /// <param name="token">Valor do token.</param>
        /// <param name="n_line">Numero da linha do token.</param>
        /// <param name="n_column">Numero da coluna do token.</param>
        public static void ShowToken(Token token, int n_line, int n_column)
        {
            Console.WriteLine("Token: " + token.ToString() + "\t Linha: " + n_line + "\t Coluna: " + n_column);
        }

        /// <summary>
        /// Mostra o simbolo da tabela.
        /// </summary>
        /// <param name="key">Chave da tabela de simbolos.</param>
        /// <param name="value">Valor da tabela de simbolos.</param>
        public static void ShowTableSymbol(string key, Token value)
        {
            Console.WriteLine("Nome: \"" + key + "\" - Token: " + value);
        }

        /// <summary>
        /// Mostra a mensagem de termino da compilação do arquivo lido.
        /// </summary>
        public static void EndOfBuild()
        {
            Console.WriteLine("\nCOMPILAÇÃO FINALIZADA!");
            Console.ReadLine();
        }

        /// <summary>
        /// Mostra a mensagem no console.
        /// </summary>
        /// <param name="message">Mensagem a ser exibida.</param>
        public static void Print(string message)
        {
            Console.WriteLine(message);
        }
    }
}
