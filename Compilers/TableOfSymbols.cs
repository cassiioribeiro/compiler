using System;
using System.Collections.Generic;

namespace Compilers
{
    public class TableOfSymbols
    {
        private static Dictionary<String, Token> tabelaSimbolos;
        public TableOfSymbols()
        {
            tabelaSimbolos = new Dictionary<String, Token>();

            // Inserindo as palavras reservadas
            #region [OPERADORES]
            tabelaSimbolos.Add("==",new Token(TokenEnum.OP_EQ, "==", 0, 0));
            tabelaSimbolos.Add("!=",new Token(TokenEnum.OP_NE, "!=", 0, 0));
            tabelaSimbolos.Add(">",new Token(TokenEnum.OP_GT, ">", 0, 0));
            tabelaSimbolos.Add("<",new Token(TokenEnum.OP_LT, "<", 0, 0));
            tabelaSimbolos.Add(">=",new Token(TokenEnum.OP_GE, ">=", 0, 0));
            tabelaSimbolos.Add("<=",new Token(TokenEnum.OP_LE, "<=", 0, 0));
            tabelaSimbolos.Add("+",new Token(TokenEnum.OP_AD, "+", 0, 0));
            tabelaSimbolos.Add("-",new Token(TokenEnum.OP_MIN, "-", 0, 0));
            tabelaSimbolos.Add("*",new Token(TokenEnum.OP_MUL, "*", 0, 0));
            tabelaSimbolos.Add("/",new Token(TokenEnum.OP_DIV, "/", 0, 0));
            tabelaSimbolos.Add("=",new Token(TokenEnum.OP_ASS, "=", 0, 0));
            #endregion
            
            #region [SIMBOLOS]
            tabelaSimbolos.Add("{",new Token(TokenEnum.SMB_OBC, "{", 0, 0));            
            tabelaSimbolos.Add("}",new Token(TokenEnum.SMB_CBC, "}", 0, 0));
            tabelaSimbolos.Add("(",new Token(TokenEnum.SMB_OPA, "(", 0, 0));
            tabelaSimbolos.Add(")",new Token(TokenEnum.SMB_CPA, ")", 0, 0));
            tabelaSimbolos.Add(",",new Token(TokenEnum.SMB_COM, ",", 0, 0));            
            tabelaSimbolos.Add(";",new Token(TokenEnum.SMB_SEM, ";", 0, 0));                        
            #endregion
            
            #region [PALAVRAS CHAVE]
            tabelaSimbolos.Add("PROGRAM",new Token(TokenEnum.KW_PROGRAM, "PROGRAM", 0, 0));                        
            tabelaSimbolos.Add("IF",new Token(TokenEnum.KW_IF, "IF", 0, 0));                        
            tabelaSimbolos.Add("ELSE",new Token(TokenEnum.KW_ELSE, "ELSE", 0, 0));                        
            tabelaSimbolos.Add("WHILE",new Token(TokenEnum.KW_WHILE, "WHILE", 0, 0));                        
            tabelaSimbolos.Add("WRITE",new Token(TokenEnum.KW_WRITE, "WRITE", 0, 0));                        
            tabelaSimbolos.Add("READ",new Token(TokenEnum.KW_READ, "READ", 0, 0));                        
            tabelaSimbolos.Add("NUM",new Token(TokenEnum.KW_NUM, "NUM", 0, 0));                        
            tabelaSimbolos.Add("CHAR",new Token(TokenEnum.KW_CHAR, "CHAR", 0, 0));                        
            tabelaSimbolos.Add("NOT",new Token(TokenEnum.KW_NOT, "NOT", 0, 0));                        
            tabelaSimbolos.Add("OR",new Token(TokenEnum.KW_OR, "OR", 0, 0));                        
            tabelaSimbolos.Add("AND",new Token(TokenEnum.KW_AND, "AND", 0, 0));                                    
            #endregion

            #region [IDENTIFICADORES]
            tabelaSimbolos.Add("ID",new Token(TokenEnum.ID, "ID", 0, 0));                                    
            #endregion

            #region [LITERAL]
            tabelaSimbolos.Add("LIT",new Token(TokenEnum.LIT, "LIT", 0, 0));                                    
            #endregion

            #region [CONSTANTES]
            tabelaSimbolos.Add("CON_NUM",new Token(TokenEnum.CON_NUM, "CON_NUM", 0, 0));                        
            tabelaSimbolos.Add("CON_CHAR",new Token(TokenEnum.CON_CHAR, "CON_CHAR", 0, 0));                                    
            #endregion

        }
        
        // Pesquisa na tabela de símbolos se há algum tokem com determinado lexema
        // vamos usar esse metodo somente para diferenciar ID e KW
        public Token RetornaToken(String lexema) {
            foreach (Token item in tabelaSimbolos.Values)
            {
                if (item.Lexema.Equals(lexema))                
                    return item;               
            }
            return null;
        }
        
        public override String ToString() {
            String saida = "";
            int i = 1;
            foreach (Token item in tabelaSimbolos.Values) {
                saida += ("posicao " + i + ": \t" + item.ToString()) + "\n";
                i++;
            }
            return saida;
        }
    }
}