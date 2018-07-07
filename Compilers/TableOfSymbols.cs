using System;
using System.Collections.Generic;

namespace Compilers {
    public class TableOfSymbols {
        private static Dictionary<String, Token> tabelaSimbolos; // Tabela de simbolos

        /// <summary>
        /// Cria a tabela de simbolos.
        /// </summary>
        public TableOfSymbols () {
            tabelaSimbolos = new Dictionary<String, Token> ();

            #region [PALAVRAS CHAVE]
            tabelaSimbolos.Add ("PROGRAM", new Token (TokenEnum.KW_PROGRAM, "PROGRAM", 0, 0));
            tabelaSimbolos.Add ("IF", new Token (TokenEnum.KW_IF, "IF", 0, 0));
            tabelaSimbolos.Add ("ELSE", new Token (TokenEnum.KW_ELSE, "ELSE", 0, 0));
            tabelaSimbolos.Add ("WHILE", new Token (TokenEnum.KW_WHILE, "WHILE", 0, 0));
            tabelaSimbolos.Add ("WRITE", new Token (TokenEnum.KW_WRITE, "WRITE", 0, 0));
            tabelaSimbolos.Add ("READ", new Token (TokenEnum.KW_READ, "READ", 0, 0));
            tabelaSimbolos.Add ("NUM", new Token (TokenEnum.KW_NUM, "NUM", 0, 0));
            tabelaSimbolos.Add ("CHAR", new Token (TokenEnum.KW_CHAR, "CHAR", 0, 0));
            tabelaSimbolos.Add ("NOT", new Token (TokenEnum.KW_NOT, "NOT", 0, 0));
            tabelaSimbolos.Add ("OR", new Token (TokenEnum.KW_OR, "OR", 0, 0));
            tabelaSimbolos.Add ("AND", new Token (TokenEnum.KW_AND, "AND", 0, 0));
            #endregion

            #region [OUTROS]
            //#region [OPERADORES]
            //tabelaSimbolos.Add("==", new Token(TokenEnum.OP_EQ, "==", 0, 0));
            //tabelaSimbolos.Add("!=", new Token(TokenEnum.OP_NE, "!=", 0, 0));
            //tabelaSimbolos.Add(">", new Token(TokenEnum.OP_GT, ">", 0, 0));
            //tabelaSimbolos.Add("<", new Token(TokenEnum.OP_LT, "<", 0, 0));
            //tabelaSimbolos.Add(">=", new Token(TokenEnum.OP_GE, ">=", 0, 0));
            //tabelaSimbolos.Add("<=", new Token(TokenEnum.OP_LE, "<=", 0, 0));
            //tabelaSimbolos.Add("+", new Token(TokenEnum.OP_AD, "+", 0, 0));
            //tabelaSimbolos.Add("-", new Token(TokenEnum.OP_MIN, "-", 0, 0));
            //tabelaSimbolos.Add("*", new Token(TokenEnum.OP_MUL, "*", 0, 0));
            //tabelaSimbolos.Add("/", new Token(TokenEnum.OP_DIV, "/", 0, 0));
            //tabelaSimbolos.Add("=", new Token(TokenEnum.OP_ASS, "=", 0, 0));
            //#endregion

            //#region [SIMBOLOS]
            //tabelaSimbolos.Add("{", new Token(TokenEnum.SMB_OBC, "{", 0, 0));
            //tabelaSimbolos.Add("}", new Token(TokenEnum.SMB_CBC, "}", 0, 0));
            //tabelaSimbolos.Add("(", new Token(TokenEnum.SMB_OPA, "(", 0, 0));
            //tabelaSimbolos.Add(")", new Token(TokenEnum.SMB_CPA, ")", 0, 0));
            //tabelaSimbolos.Add(",", new Token(TokenEnum.SMB_COM, ",", 0, 0));
            //tabelaSimbolos.Add(";", new Token(TokenEnum.SMB_SEM, ";", 0, 0));
            //#endregion

            //#region [IDENTIFICADORES]
            //tabelaSimbolos.Add("ID", new Token(TokenEnum.ID, "ID", 0, 0));
            //#endregion

            //#region [LITERAL]
            //tabelaSimbolos.Add("LIT", new Token(TokenEnum.LIT, "LIT", 0, 0));
            //#endregion

            //#region [CONSTANTES]
            //tabelaSimbolos.Add("CON_NUM", new Token(TokenEnum.CON_NUM, "CON_NUM", 0, 0));
            //tabelaSimbolos.Add("CON_CHAR", new Token(TokenEnum.CON_CHAR, "CON_CHAR", 0, 0));
            //#endregion
            #endregion

        }

        /// <summary>
        /// Adiciona um token na tabela de simbolos.
        /// </summary>
        /// <param name="symbol">Simbolo do token encontrado.</param>
        /// <param name="token">Token encontrado.</param>
        public static void Add (String symbol, Token token) {
            tabelaSimbolos.Add (symbol, token);
        }

        /// <summary>
        /// Mostra todos os simbolos contidos na tabela de simbolos.
        /// </summary>
        public static void ShowAllTableSymbols () {
            Message.Print ("\nCONTEÚDO CONTIDO NA TABELA DE SÍMBOLOS: ");

            foreach (var item in tabelaSimbolos) {
                Message.ShowTableSymbol (item.Key, item.Value);
            }
        }

        /// <summary>
        /// Verifica se já existe o lexema dentro da tabela de simbolos.
        /// Vai ser usado esse metodo somente para diferenciar ID e KW.
        /// </summary>
        /// <param name="lexema">Lexema para a verificação.</param>
        /// <returns>Retorna token caso o lexema estaja dentro da tabela de simbolos, caso contratio retorna nulo</returns>
        public static Token ReturnToken (String lexema) {
            foreach (Token item in tabelaSimbolos.Values) {
                if (item.Lexema.Equals (lexema.ToUpper ()))
                    return item;
            }
            return null;
        }

        /// <summary>
        /// Sobrepondo o ToString() do c#.
        /// </summary>
        /// <returns>Retorna uma string com a posição e o conteudo da tabela de simbolos.</returns>
        public override String ToString () {
            String saida = "";
            int i = 1;
            foreach (Token item in tabelaSimbolos.Values) {
                saida += ("posicao " + i + ": \t" + item.ToString ()) + "\n";
                i++;
            }
            return saida;
        }

        /// <summary>
        /// Verifica se existe o token (id) dentro da tabela de simbolos, se não existir irá ser adicionado.
        /// </summary>
        /// <param name="token">Token que será validado.</param>
        public static void ExisteLexemaTabela (Token token) {
            if (TableOfSymbols.ReturnToken (token.Lexema) == null && token.EnumToken.ToString () == "ID") {
                TableOfSymbols.Add (token.Lexema.ToUpper (), token);
            }
        }
    }
}