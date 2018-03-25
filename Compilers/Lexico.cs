using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Compilers
{
    public class Lexico
    {
        private readonly static int END_OF_FILE = -1; // contante para fim do arquivo
        private static int lookahead = 0; // armazena o último caractere lido do arquivo	
        public static int n_line = 1; // contador de linhas
        public static int n_column = 1; // contador de linhas
        private FileStream instance_file; // referencia para o arquivo
        private static TableOfSymbols tabelaSimbolos;  // tabela de simbolos

        public Lexico(string entrada)
        {
            try
            {
                instance_file = new FileStream(entrada, FileMode.Append, FileAccess.Read, FileShare.Read);
            }
            catch (IOException ex)
            {
                Console.WriteLine("Erro do programa ou falha da tabela de simbolos\n" + ex.Message);
                Environment.Exit(1);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Erro do programa ou falha da tabela de simbolos\n" + ex.Message);
                Environment.Exit(2);
            }
        }

        // Fecha instance_file de input_data
        public void FechaArquivo()
        {
            try
            {
                instance_file.Close();
            }
            catch (IOException ioex)
            {
                Console.WriteLine("Erro ao fechar arquivo\n" + ioex.Message);
                Environment.Exit(3);
            }
        }

        public void SinalizaErro(String mensagem)
        {
             Console.WriteLine("Erro Lexico na linha " + n_line + " na coluna " + n_column + " : " + mensagem + "\n");
        }

        //Volta uma posição do buffer de leitura
        public void RetornaPonteiro()
        {
            try
            {
                // Não é necessário retornar o ponteiro em caso de Fim de Arquivo
                if (lookahead != END_OF_FILE)
                {
                    //instance_file.Seek(instance_file.GetFilePointer() - 1);
                    instance_file.Seek(instance_file.Position - 1, SeekOrigin.Current);
                }
            }
            catch (IOException ioex)
            {
                Console.WriteLine("Falha ao retornar a leitura\n" + ioex.Message);
                Environment.Exit(4);
            }
        }

        // Obtém próximo token
        public Token ProxToken()
        {

            StringBuilder lexema = new StringBuilder();
            int estado = 0;
            char c;

            while (true)
            {
                c = '\u0000'; // null char

                // avanca caractere
                try
                {
                    lookahead = instance_file.ReadByte();
                    if (lookahead != END_OF_FILE)
                    {
                        c = (char)lookahead;
                    }
                }
                catch (IOException ioex)
                {
                    Console.WriteLine("Erro na leitura do arquivo: " + ioex.Message);
                    Environment.Exit(3);
                }

                // movimentacao do automato
                switch (estado)
                {
                    #region [CASE 0]
                    case 0:
                        if (lookahead == END_OF_FILE)
                            return new Token(TokenEnum.EOF, "EOF", n_line, n_column);
                        else if (c == ' ' || c == '\t' || c == '\n' || c == '\r')
                        {
                            // Permance no estado = 1
                            if (c == '\n')
                            {

                            }
                            else if (c == '\t')
                            {
                                
                            }
                        }
                        else if (Char.IsLetter(c))
                        {
                            lexema.Append(c);
                            //stado = 14;
                            estado = 1;
                        }
                        else if (Char.IsDigit(c))
                        {
                            lexema.Append(c);
                            //estado = 12;
                            estado = 24;
                        }
                        #region [OPERADORES]
                        else if (c == '<')
                        {
                            //estado = 6;
                            estado = 21;
                        }
                        else if (c == '>')
                        {
                            //estado = 9;
                            estado = 19;
                        }
                        else if (c == '=')
                        {
                            //estado = 2;
                            estado = 15;
                        }
                        else if (c == '!')
                        {
                            //estado = 4;
                            estado = 3;
                        }
                        else if (c == '/')
                        {
                            //estado = 16;
                            estado = 5;
                        }
                        else if (c == '*')
                        {
                            //estado = 18;
                            estado = 6;
                            return new Token(TokenEnum.OP_MUL, "*", n_line, n_column);
                        }
                        else if (c == '+')
                        {
                            //estado = 19;
                            estado = 8;
                            return new Token(TokenEnum.OP_AD, "+", n_line, n_column);
                        }
                        else if (c == '-')
                        {
                            //estado = 20;
                            estado = 7;
                            return new Token(TokenEnum.OP_MIN, "-", n_line, n_column);
                        }
                        #endregion
                        #region [SIMBOLOS]
                        else if (c == ';')
                        {
                            //estado = 21;
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
                            //estado = 22;
                            estado = 11;
                            return new Token(TokenEnum.SMB_OPA, "(", n_line, n_column);
                        }
                        else if (c == ')')
                        {
                            //estado = 23;
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
                            //estado = 24;
                            estado = 30;
                        }
                        else if (c == '\'')
                        {
                            estado = 27;
                        }
                        #endregion
                        else
                        {
                            SinalizaErro("Caractere invalido " + c + " na linha " + n_line + " e coluna " + n_column);
                            return null;
                        }
                        break;
                    #endregion
                    #region [CASE 1]
                    case 1:
                        if (Char.IsLetterOrDigit(c) || c == '_') {
                            lexema.Append(c);
                            // Permanece no estado 1
                        }
                        else { // Estado 15
                            //estado = 2;
                            RetornaPonteiro();  
                            Token token = tabelaSimbolos.RetornaToken(lexema.ToString());
                            
                            if (token == null) {
                                return new Token(TokenEnum.ID, lexema.ToString(), n_line, n_column);
                            }
                            return token;
                        }
                        break;
                    #endregion
                    #region [CASE 2]
                    case 2:
                        // if (c == '=') { // Estado 3
                        //     estado = 3;
                        //     return new Token(TokenEnum.OP_EQ, "==", n_line, n_column);
                        // }
                        // else {
                        //     RetornaPonteiro();
                        //     return new Token(TokenEnum.OP_ASS, "=", n_line, n_column);
                        // }
                    #endregion
                    #region [CASE 3]
                    case 3:
                        if (c == '=') { // Estado 5
                            //estado = 4;
                            return new Token(TokenEnum.OP_NE, "!=", n_line, n_column);
                        }
                        else {
                            RetornaPonteiro();
                            SinalizaErro("Token incompleto para o caractere ! na linha " + n_line + " e coluna " + n_column);
                            return null;
                        }
                    #endregion
                    #region [CASE 5]
                    case 5:
                        if (c == '/') {
                            estado = 34;
                        }
                        else {
                            RetornaPonteiro();
                            return new Token(TokenEnum.OP_DIV, lexema.ToString(), n_line, n_column);
                        }
                        break;
                    #endregion
                    #region [CASE 6]
                    case 6:
                        if (c == '*')
                        {
                            estado = 35;
                        }
                        else{
                            RetornaPonteiro();
                            return new Token(TokenEnum.OP_MUL, lexema.ToString(), n_line, n_column);
                        }
                        break;
                        // if (c == '=') { // Estado 7
                        //     estado = 7;
                        //     return new Token(TokenEnum.OP_LE, "<=", n_line, n_column);
                        // }
                        // else { // Estado 8
                        //     estado = 8;
                        //     RetornaPonteiro();
                        //     return new Token(TokenEnum.OP_LT, "<", n_line, n_column);
                        // }
                    #endregion
                    #region [CASE 9]
                    case 9:
                        // if (c == '=') { // Estado 10
                        //     estado = 10;
                        //     return new Token(TokenEnum.OP_GE, ">=", n_line, n_column);
                        // }
                        // else { // Estado 11
                        //     estado = 11;
                        //     RetornaPonteiro();
                        //     return new Token(TokenEnum.OP_GT, ">", n_line, n_column);
                        // }
                    #endregion
                    #region [CASE 12]
                    case 12:
                        // if (Char.IsDigit(c)) {
                        //     lexema.Append(c);
                        //     // Permanece no estado 12
                        // }
                        // else if (c == '.') {
                        //     lexema.Append(c);
                        //     estado = 26;
                        // }
                        // else { // Estado 13
                        //     estado = 13;
                        //     RetornaPonteiro();						
                        //     return new Token(Tag.INTEGER, lexema.ToString(), n_line, n_column);
                        // }
                        // break;
                    #endregion
                    #region [CASE 14]
                    case 14:
                        // if (Char.IsLetterOrDigit(c) || c == '_') {
                        //     lexema.Append(c);
                        //     // Permanece no estado 14
                        // }
                        // else { // Estado 15
                        //     estado = 15;
                        //     RetornaPonteiro();  
                        //     Token token = tabelaSimbolos.retornaToken(lexema.ToString());
                            
                        //     if (token == null) {
                        //         return new Token(TokenEnum.ID, lexema.ToString(), n_line, n_column);
                        //     }
                        //     return token;
                        // }
                        // break;
                    #endregion
                    #region [CASE 15]
                    case 15:
                        if (c == '=') { // Estado 3
                            //estado = 16;
                            return new Token(TokenEnum.OP_EQ, "==", n_line, n_column);
                        }
                        else {
                            //estado = 17;
                            RetornaPonteiro();
                            return new Token(TokenEnum.OP_ASS, "=", n_line, n_column);
                        }
                    #endregion
                    #region [CASE 16]
                    case 16:
                        // if (c == '/') {
                        //     estado = 17;
                        // }
                        // else {
                        //     RetornaPonteiro();
                        //     return new Token(TokenEnum.OP_DIV, lexema.ToString(), n_line, n_column);
                        // }
                        // break;
                    #endregion
                    #region [CASE 17]
                    case 17:
                        // if (c == '\n') {
                            
                        // } 
                        // // Se vier outro, permanece no estado 17
                        // break;
                    #endregion
                    #region [CASE 18]
                    #endregion
                    #region [CASE 19]
                    case 19:
                        if (c == '=') { // Estado 10
                            //estado = 20;
                            return new Token(TokenEnum.OP_GE, ">=", n_line, n_column);
                        }
                        else { // Estado 11
                            //estado = 18;
                            RetornaPonteiro();
                            return new Token(TokenEnum.OP_GT, ">", n_line, n_column);
                        }
                    #endregion
                    #region [CASE 21]
                    case 21:
                        if (c == '=') { // Estado 7
                            //estado = 22;
                            return new Token(TokenEnum.OP_LE, "<=", n_line, n_column);
                        }
                        else { // Estado 8
                            //estado = 23;
                            RetornaPonteiro();
                            return new Token(TokenEnum.OP_LT, "<", n_line, n_column);
                        }
                    #endregion
                    #region [CASE 24]
                    case 24:
                        if (Char.IsDigit(c)) {
                            lexema.Append(c);
                            // Permanece no estado 24
                        }
                        else if (c == '.') {
                            lexema.Append(c);
                            estado = 26;
                        }
                        else { // Estado 13
                            //estado = 25;
                            RetornaPonteiro();						
                            return new Token(TokenEnum.CON_NUM, lexema.ToString(), n_line, n_column);
                        }
                        break;

                        // if (c == '"') {
                        //     estado = 25;
                        //     return new Token(Tag.STRING, lexema.ToString(), n_line, n_column);
                        // }
                        // else if (lookahead == END_OF_FILE) {
                        //     SinalizaErro("String deve ser fechada com \" antes do fim de arquivo");
                        //     return null;
                        // }
                        // else { // Se vier outro, permanece no estado 24
                        //     lexema.Append(c);
                        // }
                        // break;
                    #endregion                    
                    #region [CASE 26]
                    case 26:
                        if (Char.IsDigit(c)) {
                            lexema.Append(c);
                            estado = 28;
                        }
                        else {
                            SinalizaErro("Padrao para double invalido na linha " + n_line + " coluna " + n_column);
                            return null;
                        }
                        break;
                    #endregion
                    #region [CASE 27]
                    case 27:
                        if (c == '\'') {
                            estado = 25;
                            return new Token(TokenEnum.CON_CHAR, lexema.ToString(), n_line, n_column);
                        }
                        else if (lookahead == END_OF_FILE) {
                            SinalizaErro("String deve ser fechada com \" antes do fim de arquivo");
                            return null;
                        }
                        else { // Se vier outro, permanece no estado 25
                            lexema.Append(c);
                        }
                        break;
                    #endregion
                    #region [CASE 28]
                    case 28:
                        if (Char.IsDigit(c)) {
                            lexema.Append(c);
                        }
                        else {
                            RetornaPonteiro();						
                            return new Token(TokenEnum.CON_NUM, lexema.ToString(), n_line, n_column);
                        }
                        break;
                    #endregion               
                    #region [CASE 30]
                    case 30:
                        if (c == '"') {
                            estado = 25;
                            return new Token(TokenEnum.LIT, lexema.ToString(), n_line, n_column);
                        }
                        else if (lookahead == END_OF_FILE) {
                            SinalizaErro("String deve ser fechada com \" antes do fim de arquivo");
                            return null;
                        }
                        else { // Se vier outro, permanece no estado 30
                            lexema.Append(c);
                        }
                        break;
                    #endregion
                    #region [CASE 34]
                    case 34:
                        if (c == '\n') {
                            
                        } 
                        // Se vier outro, permanece no estado 34
                        break;
                    #endregion

                }
            }
        }
    }
}