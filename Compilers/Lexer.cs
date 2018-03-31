using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Compilers
{
    public class Lexer
    {
        private readonly static int END_OF_FILE = -1; // contante para fim do arquivo
        private static int lookahead = 0; // armazena o último caractere lido do arquivo	
        public static int n_line = 1; // contador de linhas
        public static int n_column = 1; // contador de linhas
        private FileStream instance_file; // referencia para o arquivo
        private static TableOfSymbols tabelaSimbolos = new TableOfSymbols();  // tabela de simbolos

        /// <summary>
        /// Inicializa a análise lexica, abre o arquivo, faz a chamada para o metodo de impressão dos tokens,
        /// fecha o arquivo e exibe uma mensagem de termino.
        /// </summary>
        /// <param name="entrada"></param> 
        public Lexer(string entrada)
        {
            try
            {
                instance_file = new FileStream(entrada, FileMode.Open, FileAccess.Read);

                ImprimeToken();                

                FechaArquivo();

                Message.EndOfBuild();
            }
            catch (IOException ex)
            {
                ErrorMessage.Error(ex.Message);
                Environment.Exit(1);
            }
            catch (Exception ex)
            {
                ErrorMessage.Error(ex.Message);
                Environment.Exit(2);
            }
        }

        /// <summary>
        /// Busca e depois exibe todos os token que foram encontrados na leitura do arquivo.
        /// </summary>         
        private void ImprimeToken()
        {
            Token token;
            Message.Print("TOKENS ENCONTRADOS: ");
            // Enquanto não houver erros ou não for fim de arquivo.
            do
            {
                token = ProxToken();

                // Imprime token
                if (token != null) 
                {
                    Message.ShowToken(token, n_line, n_column);

                    // Verificar se existe o lexema na tabela de símbolos
                    if (TableOfSymbols.ReturnToken(token.Lexema) == null) 
                    {
                        TableOfSymbols.Add(token.Lexema.ToUpper(), token);
                    }
                }

            } while (token != null && token.EnumToken != TokenEnum.EOF);

            
            if (ErrorMessage.errorFound != null)
            {
                ErrorMessage.ShowErrorFound();
            }
            
            TableOfSymbols.ShowAllTableSymbols();

        }
        
        /// <summary>
        /// Fecha o arquivo instance_file de input_data
        /// </summary>
        public void FechaArquivo()
        {
            try
            {
                instance_file.Close();
            }
            catch (IOException ioex)
            {
                ErrorMessage.ErrorCloseArchive(ioex.Message);
                Environment.Exit(3);
            }
        }

        /// <summary>
        /// Chama o metodo ErrorLexer para exibir a mensagem de erro.
        /// </summary>
        /// <param name="mensagem">Mensagem para ser exibida no erro ocorrido.</param>
        public void SinalizaErro(String mensagem)
        {   
            ErrorMessage.ErrorLexer(n_line, n_column, mensagem);
        }

        /// <summary>
        /// Volta uma posição do buffer de leitura.
        /// </summary>         
        public void RetornaPonteiro()
        {
            try
            {
                // Não é necessário retornar o ponteiro em caso de Fim de Arquivo.
                if (lookahead != END_OF_FILE)
                {
                    instance_file.Seek(-1, SeekOrigin.Current);
                    n_column--;
                }
            }
            catch (IOException ioex)
            {
                ErrorMessage.ErrorRead(ioex.Message);
                Environment.Exit(4);
            }
        }

        /// <summary>
        /// Obtém próximo token.
        /// </summary>
        /// <returns>Retorna o token que foi encontrado.</returns>
        public Token ProxToken()
        {
            StringBuilder lexema = new StringBuilder();
            int estado = 0;
            char c;

            while (true)
            {
                c = '\u0000'; // Caracter nulo.

                // Avança um caracter.
                try
                {
                    lookahead = instance_file.ReadByte();
                    if (lookahead != END_OF_FILE)
                    {
                        c = (char)lookahead;
                        n_column++;
                    }
                    if (c == '\n')
                    {
                        n_line++;
                        n_column = 1;
                    }
                    else if (c == '\t')
                    {
                        n_column += 3;
                    }
                }
                catch (IOException ioex)
                {
                    ErrorMessage.ErrorRead(ioex.Message);
                    Environment.Exit(3);
                }

                // Movimentação do automato.
                switch (estado)
                {
                    #region[CASE 0]
                    case 0:
                        if (lookahead == END_OF_FILE)
                            return new Token(TokenEnum.EOF, "EOF", n_line, n_column);
                        else if (c == ' ' || c == '\t' || c == '\n' || c == '\r')
                        {
                            // Permance no estado = 1.                            
                        }
                        else if (Char.IsLetter(c))
                        {
                            lexema.Append(c);
                            estado = 1;
                        }
                        else if (Char.IsDigit(c))
                        {
                            lexema.Append(c);
                            estado = 24;
                        }

                        #region [OPERADORES]
                        else if (c == '<')
                        {
                            estado = 21;
                        }
                        else if (c == '>')
                        {
                            estado = 19;
                        }
                        else if (c == '=')
                        {
                            estado = 15;
                        }
                        else if (c == '!')
                        {
                            estado = 3;
                        }
                        else if (c == '/')
                        {
                            estado = 5;
                        }
                        else if (c == '*')
                        {
                            estado = 6;
                            return new Token(TokenEnum.OP_MUL, "*", n_line, n_column);
                        }
                        else if (c == '+')
                        {
                            estado = 8;
                            return new Token(TokenEnum.OP_AD, "+", n_line, n_column);
                        }
                        else if (c == '-')
                        {
                            estado = 7;
                            return new Token(TokenEnum.OP_MIN, "-", n_line, n_column);
                        }
                        #endregion

                        #region [SIMBOLOS]
                        else if (c == ';')
                        {
                            estado = 9;
                            return new Token(TokenEnum.SMB_SEM, ";", n_line, n_column);
                        }
                        else if (c == ',')
                        {
                            estado = 10;
                            return new Token(TokenEnum.SMB_COM, ",", n_line, n_column);
                        }
                        else if (c == '(')
                        {
                            estado = 11;
                            return new Token(TokenEnum.SMB_OPA, "(", n_line, n_column);
                        }
                        else if (c == ')')
                        {
                            estado = 12;
                            return new Token(TokenEnum.SMB_CPA, ")", n_line, n_column);
                        }
                        else if (c == '{')
                        {
                            estado = 13;
                            return new Token(TokenEnum.SMB_OBC, "{", n_line, n_column);
                        }
                        else if (c == '}')
                        {
                            estado = 14;
                            return new Token(TokenEnum.SMB_CBC, "}", n_line, n_column);
                        }
                        #endregion

                        #region [CONSTANTES]
                        else if (c == '"')
                        {
                            estado = 30;
                        }
                        else if (c == '\'')
                        {
                            estado = 27;
                        }
                        #endregion

                        else
                        {
                            SinalizaErro("O caractere '" + c +"' é inválido!");
                            estado = 0;
                        }
                        break;
                    #endregion

                    #region [CASE 1]
                    case 1:
                        if (Char.IsLetterOrDigit(c))
                        {
                            lexema.Append(c); 
                        }
                        else
                        {
                            estado = 2;
                            RetornaPonteiro();
                            Token token = TableOfSymbols.ReturnToken(lexema.ToString().ToUpper());

                            if (token == null)
                            {
                                return new Token(TokenEnum.ID, lexema.ToString().ToUpper().ToUpper(), n_line, n_column);
                            }
                            return token;
                        }
                        break;
                    #endregion

                    #region [CASE 3]
                    case 3:
                        if (c == '=')
                        {
                            estado = 4;
                            return new Token(TokenEnum.OP_NE, "!=", n_line, n_column);
                        }
                        else
                        {
                            RetornaPonteiro();
                            SinalizaErro("O Token está incompleto para o caractere '!'");
                            estado = 0;
                        }
                        break;
                    #endregion

                    #region [CASE 5]
                    case 5:
                        if (c == '/')
                        {
                            estado = 34;
                        }
                        else if (c == '*')
                        {
                            estado = 35;
                        }
                        else
                        {
                            RetornaPonteiro();
                            if (lexema.ToString() != "")
                            {
                                return new Token(TokenEnum.OP_DIV, lexema.ToString().ToUpper(), n_line, n_column);
                            }
                            else
                            {
                                return new Token(TokenEnum.OP_DIV, "/", n_line, n_column);
                            }
                        }
                        break;
                    #endregion

                    #region [CASE 6]
                    case 6:
                        if (c == '*')
                        {
                            estado = 35;
                        }
                        else
                        {
                            RetornaPonteiro();
                            return new Token(TokenEnum.OP_MUL, lexema.ToString().ToUpper(), n_line, n_column);
                        }
                        break;
                    #endregion

                    #region [CASE 15]
                    case 15:
                        if (c == '=')
                        {
                            estado = 16;
                            return new Token(TokenEnum.OP_EQ, "==", n_line, n_column);
                        }
                        else
                        {
                            estado = 17;
                            RetornaPonteiro();
                            return new Token(TokenEnum.OP_ASS, "=", n_line, n_column);
                        }
                    #endregion                  

                    #region [CASE 18]
                    #endregion

                    #region [CASE 19]
                    case 19:
                        if (c == '=')
                        {
                            estado = 20;
                            return new Token(TokenEnum.OP_GE, ">=", n_line, n_column);
                        }
                        else
                        {
                            estado = 18;
                            RetornaPonteiro();
                            return new Token(TokenEnum.OP_GT, ">", n_line, n_column);
                        }
                    #endregion

                    #region [CASE 21]
                    case 21:
                        if (c == '=')
                        {
                            estado = 22;
                            return new Token(TokenEnum.OP_LE, "<=", n_line, n_column);
                        }
                        else
                        {
                            estado = 23;
                            RetornaPonteiro();
                            return new Token(TokenEnum.OP_LT, "<", n_line, n_column);
                        }
                    #endregion

                    #region [CASE 24]
                    case 24:
                        if (Char.IsDigit(c))
                        {
                            lexema.Append(c);
                        }
                        else if (c == '.')
                        {
                            lexema.Append(c);
                            estado = 26;
                        }
                        else
                        {
                            estado = 25;
                            RetornaPonteiro();
                            return new Token(TokenEnum.CON_NUM, lexema.ToString().ToUpper(), n_line, n_column);
                        }
                        break;
                    #endregion                    

                    #region [CASE 26]
                    case 26:
                        if (Char.IsDigit(c))
                        {
                            lexema.Append(c);
                            estado = 28;
                        }
                        else
                        {
                            SinalizaErro("Padrão para o NUM_CONST está inválido");
                            estado = 0;
                        }
                        break;
                    #endregion

                    #region [CASE 27]
                    case 27:
                        if (c == '\'')
                        {
                            estado = 25;
                            return new Token(TokenEnum.CON_CHAR, lexema.ToString().ToUpper(), n_line, n_column);
                        }
                        else if (lookahead == END_OF_FILE)
                        {
                            SinalizaErro("CONSTANTE_CHAR deve ser fechado com ''' antes do fim de arquivo");
                            estado = 0;
                        }
                        else
                        {
                            lexema.Append(c); 
                        }
                        break;
                    #endregion

                    #region [CASE 28]
                    case 28:
                        if (Char.IsDigit(c))
                        {
                            lexema.Append(c);
                        }
                        else
                        {
                            RetornaPonteiro();
                            return new Token(TokenEnum.CON_NUM, lexema.ToString().ToUpper(), n_line, n_column);
                        }
                        break;
                    #endregion               

                    #region [CASE 30]
                    case 30:
                        if (c == '"')
                        {
                            estado = 25;
                            return new Token(TokenEnum.LIT, lexema.ToString().ToUpper(), n_line, n_column);
                        }
                        else if (lookahead == END_OF_FILE)
                        {
                            SinalizaErro("LITERAL deve ser fechado com '\"' antes do fim de arquivo");
                            estado = 0;
                        }
                        else
                        {
                            lexema.Append(c);
                        }
                        break;
                    #endregion

                    #region [CASE 34]
                        case 34:
                        if (c == '\n' || c == '\0')
                        {
                            estado = 0;
                        }
                        break;
                    #endregion

                    #region [CASE 35]
                    case 35:
                        if (c == '*')
                        {
                            estado = 36;
                        }
                        else if (lookahead == END_OF_FILE)
                        {
                            SinalizaErro("Comentário com mais de uma linha deve ser fechado com '*/' antes do fim de arquivo");
                            estado = 0;
                        }
                        else
                        {
                            lexema.Append(c);
                        }
                        break;
                    #endregion

                    #region [CASE 36]
                    case 36:
                        if (c == '/')
                        {
                            estado = 0;
                        }
                        else if (lookahead == END_OF_FILE)
                        {
                            SinalizaErro("Comentário com mais de uma linha deve ser fechado com '/' antes do fim de arquivo");
                            estado = 0;
                        }
                        else
                        {
                            lexema.Append(c);
                        }
                        break;
                    #endregion
                }
            }
        }
    }
}