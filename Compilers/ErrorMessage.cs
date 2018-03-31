using System;
using System.Collections.Generic;

namespace Compilers
{
    public static class ErrorMessage
    {
        public static List<string> errorFound = new List<string>();
        /// <summary>
        /// Erro padrão
        /// </summary>
        /// <param name="message">Mensagem do erro.</param>
        public static void Error(string message)
        {
            string error = "Erro do programa ou falha da tabela de simbolos. Descrição: " + message;
            AddError(error);
        }

        /// <summary>
        /// Erro ao fechar o arquivo.
        /// </summary>
        /// <param name="message">Mensagem do erro.</param>
        public static void ErrorCloseArchive(string message)
        {
            string error = "Erro inesperado ao fechar arquivo. Descrição:" + message;
            AddError(error);
        }

        /// <summary>
        /// Erro lexico.
        /// </summary>
        /// <param name="n_line">Numero da linha do erro.</param>
        /// <param name="n_column">Numero da coluna do erro.</param>
        /// <param name="message">Mensagem do erro.</param>
        public static void ErrorLexer(int n_line, int n_column, string message)
        {
            string error = String.Format("Erro Lexico na linha {0} na coluna {1} : {2}\n", n_line, n_column, message);
            AddError(error);
        }

        /// <summary>
        /// Erro na leitura do caracter ou retorno de um caracter.
        /// </summary>
        /// <param name="message">Mensagem do erro.</param>
        public static void ErrorRead(string message)
        {
            string error = "Erro inesperado na leitudo. Descrição: " + message;
            AddError(error);
        }

        /// <summary>
        /// Erro inicializar a aplicação.
        /// </summary>
        /// <param name="message">Mensagem do erro.</param>
        public static void ErrorStart(string message)
        {
            string error = "Erro inesperado ao iniciar o compilador. Descrição: " + message;
            AddError(error);
        }

        public static void AddError(string error)
        {
            errorFound.Add(error);
        }

        public static void ShowErrorFound()
        {
            foreach (string item in errorFound)
            {
                Print(item);
            }
        }

        public static void Print(string error)
        {
            Console.WriteLine(error);
        }
    }
}
