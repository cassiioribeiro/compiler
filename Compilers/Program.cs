using System;
using System.IO;
using System.Reflection;

namespace Compilers
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                string file = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location); // pega o diretorio da aplicação         
                file = file.Replace(@"bin\Debug\netcoreapp2.0", @"Tests\Test_Success_1.txt"); // (arquivo de sucesso) substitui o diretorio da aplicação para a pasta que está na solução do projeto
                //file = file.Replace(@"bin\Debug\netcoreapp2.0", @"Testes\Test_Error_3.txt"); // (arquivo de erro) substitui o diretorio da aplicação para a pasta que está na solução do projeto

                Lexer lexer = new Lexer(file); // parametro do Lexer: Um programa de acordo com a gramatica                
                


               
            }
            catch (Exception ex)
            {
                ErrorMessage.ErrorStart(ex.Message);
            }
        }
    }
}
