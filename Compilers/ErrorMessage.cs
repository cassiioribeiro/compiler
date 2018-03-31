using System;
using System.Collections.Generic;

namespace Compilers
{
    public static class ErrorMessage
    {
        public static List<string> errorFound = new List<string>(); // Lista de erros encontraodos.

        /// <summary>
        /// Adiciona o erro na lista de erros encontrados.
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
            string error = String.Format("Erro Lexico na linha {0} na coluna {1} : {2}.", n_line, n_column, message);
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
        /// Erro ao inicializar o compilador.
        /// </summary>
        /// <param name="message">Mensagem do erro.</param>
        public static void ErrorStart(string message)
        {
            string error = "Erro inesperado ao iniciar o compilador. Descrição: " + message;
            AddError(error);
        }

        /// <summary>
        /// Adiciona a mensagem de erro dentro da lista de erros.
        /// </summary>
        /// <param name="error">Mensagem de erro.</param>
        public static void AddError(string error)
        {
            errorFound.Add(error);
        }

        /// <summary>
        /// Faz um foreach na lista de erros, exibindo todos os erros encontrados.
        /// </summary>
        public static void ShowErrorFound()
        {
            Print("\nERROS ENCONTRADOS: ");
            if (errorFound != null && errorFound.Count != 0)
            {
                foreach (string item in errorFound)
                {
                    Print(item);
                } 
            }
            else
            {
                Print("Nenhum erro identificado.");
            }
        }

        /// <summary>
        /// Mostra o erro no console.
        /// </summary>
        /// <param name="error">Mensagem de erro.</param>
        public static void Print(string error)
        {
            Console.WriteLine(error);
        }
    }
}
