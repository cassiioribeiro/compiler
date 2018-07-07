using System;
using System.Collections.Generic;

namespace Compilers
{
    public class Parser
    {
        private int quantidadeErros;
        private readonly Lexer Lexer;
        public Token Token { get; private set; }
        public List<TokenEnum> TokensSincronizantes { get; private set; }

        /// <summary>
        /// Contruturos do análisador parser, seta as propriedades, instancia e exibe o token inicial.
        /// </summary>
        /// <param name="lexer"></param>
        public Parser(Lexer lexer)
        {
            this.Lexer = lexer;
            this.Token = Lexer.ProxToken(); // Leitura inicial obrigatoria do primeiro simbolo
            Console.WriteLine("[DEBUG] " + Token.ToString());
            TokensSincronizantes = new List<TokenEnum>();
        }

        /// <summary>
        ///  Fecha os arquivos de entrada e de Tokens
        /// </summary>
        public void FechaArquivos()
        {
            Lexer.FechaArquivo();
        }

        /// <summary>
        /// Exibe o erro sintático e conta quanto erro já foram registrados, ao somar 5 erros, 
        /// o programa será finalizado.
        /// </summary>
        /// <param name="lexema"></param>
        public void ErroSintatico(String lexema)
        {
            quantidadeErros++;

            Console.WriteLine("[ERRO SINTATICO] esperado " + lexema + " encontrado " + Token.Lexema +
                " na linha " + Token.Linha + " e coluna " + Token.Coluna);

            if (quantidadeErros == 5)
            {
                Console.WriteLine("***Estouro de erros sintáticos.***");
                TableOfSymbols.ShowAllTableSymbols();
                Console.ReadLine();
                Environment.Exit(0);
            }

        }

        /// <summary>
        /// Recebe o proximo token e adiciona ele a tabela de simbolos
        /// </summary>
        public void Advance()
        {
            Token = Lexer.ProxToken();
            TableOfSymbols.ExisteLexemaTabela(Token);
            Console.WriteLine("[DEBUG] " + Token.ToString());
        }

        /// <summary>
        /// Verifica se o Token atual for igual ao tokenEnum do paramentro
        /// avança a entrada 
        /// </summary>
        /// <param name="tokenEnum">parametro a ser comporado</param>
        /// <returns></returns>         
        public bool Eat(TokenEnum tokenEnum)
        {
            if (Token.EnumToken == tokenEnum)
            {
                Advance();
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Exibe o erro sintatico e avança entrada.
        /// </summary>
        /// <param name="mensagem">mensagem do erro</param>
        public void Skip(String mensagem)
        {
            ErroSintatico(mensagem);
            Advance();
        }

        /// <summary>
        /// Metodo que sicroniza o token (modo panico).
        /// É calculado se o follow do não terminal a esquerda está com o token atual, caso não
        /// esteja, é avancçado a entrada, e assim fica dentro de um loop até sincronizar a entrada. 
        /// </summary>
        /// <param name="tokens"></param>
        public void SincronizaToken(String tokens)
        {
            bool casaToken = false;
            string mensagem = "[MODO PANICO] Esperado " + tokens + ", encontrado " + Token.Lexema;

            while (!casaToken && Token.EnumToken != TokenEnum.EOF)
            {
                if (TokensSincronizantes.Contains(Token.EnumToken))
                {
                    casaToken = true;
                }
                else
                {
                    Skip(mensagem);
                }
            }
            TokensSincronizantes.Clear(); // limpa a lista para a proxima sincronizacao
        }

        #region [1 - Prog -> “program” “id” body ]
        public void Prog()
        {
            if (Eat(TokenEnum.KW_PROGRAM))
            {
                if (!Eat(TokenEnum.ID))
                {
                    ErroSintatico("'ID'");
                }

                Body();
            }
            else
            {
                // token sincronizante: FOLLOW(Prog)
                TokensSincronizantes.Add(TokenEnum.EOF);
                SincronizaToken("'EOF'");
                ErroSintatico("'program'");
            }
        }
        #endregion

        #region [2 - Body -> decl-list “{“ stmt-list “}” ]
        private void Body()
        {
            Decl_List();

            if (!Eat(TokenEnum.SMB_OBC))
            {
                ErroSintatico("'{'");
            }

            Smtp_List();

            if (!Eat(TokenEnum.SMB_CBC))
            {
                ErroSintatico("'}'");
            }
        }
        #endregion

        #region [3 - Decl_List -> decl “;” decl-list |  ε ]
        private void Decl_List()
        {
            if (Token.EnumToken == TokenEnum.KW_NUM || Token.EnumToken == TokenEnum.KW_CHAR)
            {
                Decl();

                if (!Eat(TokenEnum.SMB_SEM))
                {
                    ErroSintatico("';'");
                }

                Decl_List();
            }
        }
        #endregion

        #region [4 - Decl -> type id-list]
        private void Decl()
        {
            Type();
            Id_List();
        }
        #endregion

        #region [5 - Type -> “num” | “char” ]
        private void Type()
        {
            if (!Eat(TokenEnum.KW_NUM) && !Eat(TokenEnum.KW_CHAR))
            {
                // token sincronizante: FOLLOW(Type)
                TokensSincronizantes.Add(TokenEnum.ID);
                SincronizaToken("'ID'");
                ErroSintatico("'NUM', 'CHAR'");
            }
        }
        #endregion

        #region [6 - Id_List -> “id” id-list']
        private void Id_List()
        {
            if (Eat(TokenEnum.ID))
            {
                Id_List_Linha();
            }
            else
            {
                // token sincronizante: FOLLOW(Id_List)
                TokensSincronizantes.Add(TokenEnum.SMB_SEM);
                SincronizaToken("';'");
                ErroSintatico("'ID'");
            }
        }
        #endregion

        #region [7 - Id_List_Linha -> “,” id-list  | ε]
        private void Id_List_Linha()
        {
            if (Eat(TokenEnum.SMB_COM))
            {
                Id_List();
            }
        }
        #endregion

        #region [8 - Smtp_List -> stmt “;” stmt-list | ε ]
        private void Smtp_List()
        {
            if (Token.EnumToken == TokenEnum.ID || Token.EnumToken == TokenEnum.KW_IF ||
                Token.EnumToken == TokenEnum.KW_WHILE || Token.EnumToken == TokenEnum.KW_READ ||
                Token.EnumToken == TokenEnum.KW_WRITE)
            {
                Smtp();

                if (!Eat(TokenEnum.SMB_SEM))
                {
                    ErroSintatico("';'");
                }

                Smtp_List();
            }
        }
        #endregion

        #region [9 - Smtp -> assign-stmt | if-stmt | while-stmt | read-stmt | write-stmt ]
        private void Smtp()
        {
            if (Token.EnumToken == TokenEnum.ID)
            {
                Assign_Smtp();
            }
            else if (Token.EnumToken == TokenEnum.KW_IF)
            {
                If_Stmt();
            }
            else if (Token.EnumToken == TokenEnum.KW_WHILE)
            {
                While_Stmt();
            }
            else if (Token.EnumToken == TokenEnum.KW_READ)
            {
                Read_Stmt();
            }
            else if (Token.EnumToken == TokenEnum.KW_WRITE)
            {
                Write_Stmt();
            }
            else
            {
                // token sincronizante: FOLLOW(Smtp)
                TokensSincronizantes.Add(TokenEnum.SMB_SEM);
                SincronizaToken("';'");
                ErroSintatico("'ID','KW_IF','KW_WHILE','KW_READ','KW_WRITE'");
            }
        }
        #endregion

        #region [10 - Assign_Smtp -> “id” “=” simple_expr ]
        private void Assign_Smtp()
        {
            if (Eat(TokenEnum.ID))
            {

                if (!Eat(TokenEnum.OP_ASS))
                {
                    ErroSintatico("'='");
                }

                Simple_Expr();
            }
            else
            {
                // token sincronizante: FOLLOW(Assign_Smtp)
                TokensSincronizantes.Add(TokenEnum.SMB_SEM);
                SincronizaToken("';'");
                ErroSintatico("'ID'");
            }
        }
        #endregion

        #region [11 - If_Stmt -> “if” “(“ condition “)” “{“ stmt-list “}”  if-stmt']
        private void If_Stmt()
        {
            if (Eat(TokenEnum.KW_IF))
            {

                if (!Eat(TokenEnum.SMB_OPA))
                {
                    ErroSintatico("'('");
                }

                Condition();

                if (!Eat(TokenEnum.SMB_CPA))
                {
                    ErroSintatico("')'");
                }

                if (!Eat(TokenEnum.SMB_OBC))
                {
                    ErroSintatico("'{'");
                }

                Smtp_List();

                if (!Eat(TokenEnum.SMB_CBC))
                {
                    ErroSintatico("'}'");
                }

                If_Stmt_Linha();
            }
            else
            {
                // token sincronizante: FOLLOW(If_Stmt)
                TokensSincronizantes.Add(TokenEnum.SMB_SEM);
                SincronizaToken("';'");
                ErroSintatico("'KW_IF'");
            }
        }
        #endregion

        #region [12 - If_Stmt_Linha -> “else” “{“ stmt-list “}” | ε]
        private void If_Stmt_Linha()
        {
            if (Eat(TokenEnum.KW_ELSE))
            {
                if (!Eat(TokenEnum.SMB_OBC))
                {
                    ErroSintatico("'{'");
                }

                Smtp_List();

                if (!Eat(TokenEnum.SMB_CBC))
                {
                    ErroSintatico("'}'");
                }
            }
        }
        #endregion

        #region [13 - Condition -> expression]
        private void Condition()
        {
            Expression();
        }
        #endregion

        #region [14 - While_Stmt -> stmt-prefix “{“ stmt-list “}” ]
        private void While_Stmt()
        {
            if (Token.EnumToken == TokenEnum.KW_WHILE)
            {
                Stmp_Prefix();

                if (!Eat(TokenEnum.SMB_OBC))
                {
                    ErroSintatico("'{'");
                }

                Smtp_List();

                if (!Eat(TokenEnum.SMB_CBC))
                {
                    ErroSintatico("'}'");
                }
            }
            else
            {
                // token sincronizante: FOLLOW(While_Stmt)
                TokensSincronizantes.Add(TokenEnum.SMB_SEM);
                SincronizaToken("';'");
                ErroSintatico("'KW_WHILE'");
            }
        }
        #endregion

        #region [15 - Stmp_Prefix -> “while” “(“ condition “)” ]
        private void Stmp_Prefix()
        {
            if (Eat(TokenEnum.KW_WHILE))
            {

                if (!Eat(TokenEnum.SMB_OPA))
                {
                    ErroSintatico("'('");
                }

                Condition();

                if (!Eat(TokenEnum.SMB_CPA))
                {
                    ErroSintatico("')'");
                }
            }
            else
            {
                // token sincronizante: FOLLOW(Stmp_Prefix)
                TokensSincronizantes.Add(TokenEnum.SMB_OBC);
                SincronizaToken("';'");
                ErroSintatico("'KW_WHILE'");
            }
        }
        #endregion

        #region [16 - Read_Stmt -> “read” “id” ]
        private void Read_Stmt()
        {
            if (Eat(TokenEnum.KW_READ))
            {
                if (!Eat(TokenEnum.ID))
                {
                    ErroSintatico("'ID'");
                }
            }
            else
            {
                // token sincronizante: FOLLOW(Read_Stmt)
                TokensSincronizantes.Add(TokenEnum.SMB_SEM);
                SincronizaToken("';'");
                ErroSintatico("'KW_READ'");
            }
        }
        #endregion

        #region [17 - Write_Stmt -> “write” writable ]
        private void Write_Stmt()
        {
            if (Eat(TokenEnum.KW_WRITE))
            {
                Writable();
            }
            else
            {
                // token sincronizante: FOLLOW(Write_Stmt)
                TokensSincronizantes.Add(TokenEnum.SMB_SEM);
                SincronizaToken("';'");
                ErroSintatico("'KW_READ'");
            }
        }
        #endregion

        #region [18 - Writable -> simple-expr | “literal”]
        private void Writable()
        {
            if (Token.EnumToken == TokenEnum.ID || Token.EnumToken == TokenEnum.CON_NUM ||
                Token.EnumToken == TokenEnum.CON_CHAR || Token.EnumToken == TokenEnum.SMB_OPA ||
                Token.EnumToken == TokenEnum.KW_NOT)
            {
                Simple_Expr();
            }
            else if (Eat(TokenEnum.LIT))
            {
                return;
            }
            else
            {
                // token sincronizante: FOLLOW(Writable)
                TokensSincronizantes.Add(TokenEnum.SMB_SEM);
                SincronizaToken("';'");
                ErroSintatico("'ID', 'CON_NUM', 'CON_CHAR', '(', 'KW_NOT','LIT'");
            }
        }
        #endregion

        #region [19 - Expression -> simple-expr  expression']
        private void Expression()
        {
            if (Token.EnumToken == TokenEnum.ID || Token.EnumToken == TokenEnum.CON_NUM ||
                Token.EnumToken == TokenEnum.CON_CHAR || Token.EnumToken == TokenEnum.SMB_OPA ||
                Token.EnumToken == TokenEnum.KW_NOT)
            {
                Simple_Expr();
                Expression_Linha();
            }
            else
            {
                // token sincronizante: FOLLOW(Expression)
                TokensSincronizantes.Add(TokenEnum.SMB_CPA);
                SincronizaToken("')'");
                ErroSintatico("'ID','CON_NUM','CON_CHAR','SMB_CPA','KW_NOT'");
            }
        }
        #endregion

        #region [20 - Expression_Linha -> relop simple-expr | ε]
        private void Expression_Linha()
        {
            if (Token.EnumToken == TokenEnum.OP_EQ || Token.EnumToken == TokenEnum.OP_GT ||
                Token.EnumToken == TokenEnum.OP_GE || Token.EnumToken == TokenEnum.OP_LT ||
                Token.EnumToken == TokenEnum.OP_LE || Token.EnumToken == TokenEnum.OP_NE)
            {
                Relop();
                Simple_Expr();
            }
        }
        #endregion

        #region [21 - Simple_Expr -> term simple-expr']
        private void Simple_Expr()
        {
            if (Token.EnumToken == TokenEnum.ID || Token.EnumToken == TokenEnum.CON_NUM ||
                Token.EnumToken == TokenEnum.CON_CHAR || Token.EnumToken == TokenEnum.SMB_OPA ||
                Token.EnumToken == TokenEnum.KW_NOT)
            {
                Term();
                Simple_Expr_Linha();
            }
            else
            {
                // token sincronizante: FOLLOW(Simple_Expr)
                TokensSincronizantes.Add(TokenEnum.SMB_SEM);
                TokensSincronizantes.Add(TokenEnum.OP_EQ);
                TokensSincronizantes.Add(TokenEnum.OP_GT);
                TokensSincronizantes.Add(TokenEnum.OP_GE);
                TokensSincronizantes.Add(TokenEnum.OP_LT);
                TokensSincronizantes.Add(TokenEnum.OP_LE);
                TokensSincronizantes.Add(TokenEnum.OP_NE);
                TokensSincronizantes.Add(TokenEnum.SMB_CPA);
                SincronizaToken("';', '==', '>', '>=', '<', '<=', '!=', ')'");
                ErroSintatico("'ID','CON_NUM','CON_CHAR','SMB_CPA','KW_NOT'");
            }
        }
        #endregion

        #region [22 - Simple_Expr_Linha -> addop term  simple-expr' | ε]
        private void Simple_Expr_Linha()
        {
            if (Token.EnumToken == TokenEnum.OP_AD || Token.EnumToken == TokenEnum.OP_MIN ||
                Token.EnumToken == TokenEnum.KW_OR)
            {
                Addop();
                Term();
                Simple_Expr_Linha();
            }
        }
        #endregion

        #region [23 - Term -> factor-a  term']
        private void Term()
        {
            if (Token.EnumToken == TokenEnum.ID || Token.EnumToken == TokenEnum.CON_NUM ||
                Token.EnumToken == TokenEnum.CON_CHAR || Token.EnumToken == TokenEnum.SMB_OPA ||
                Token.EnumToken == TokenEnum.KW_NOT)
            {
                Factor_A();
                Term_Linha();
            }
            else
            {
                // token sincronizante: FOLLOW(Term) 
                TokensSincronizantes.Add(TokenEnum.OP_AD);
                TokensSincronizantes.Add(TokenEnum.OP_MIN);
                TokensSincronizantes.Add(TokenEnum.KW_OR);
                TokensSincronizantes.Add(TokenEnum.SMB_SEM);
                TokensSincronizantes.Add(TokenEnum.OP_EQ);
                TokensSincronizantes.Add(TokenEnum.OP_GT);
                TokensSincronizantes.Add(TokenEnum.OP_GE);
                TokensSincronizantes.Add(TokenEnum.OP_LT);
                TokensSincronizantes.Add(TokenEnum.OP_LE);
                TokensSincronizantes.Add(TokenEnum.OP_NE);
                TokensSincronizantes.Add(TokenEnum.SMB_CPA);
                SincronizaToken("'+', '-', 'or',';', '==', '>', '>=', '<', '<=', '!=', ')'");
                ErroSintatico("'ID','CON_NUM','CON_CHAR','SMB_CPA','KW_NOT'");
            }
        }
        #endregion

        #region [24 - Term_Linha -> mulop factor-a  term' | ε]
        private void Term_Linha()
        {
            if (Token.EnumToken == TokenEnum.OP_MUL || Token.EnumToken == TokenEnum.OP_DIV ||
                Token.EnumToken == TokenEnum.KW_AND)
            {
                Mulop();
                Factor_A();
                Term_Linha();
            }
        }
        #endregion

        #region [25 - Factor_A -> factor | “not” factor]
        private void Factor_A()
        {
            if (Token.EnumToken == TokenEnum.ID || Token.EnumToken == TokenEnum.CON_NUM ||
                Token.EnumToken == TokenEnum.CON_CHAR || Token.EnumToken == TokenEnum.SMB_OPA)
            {
                Factor();
            }
            else if (Eat(TokenEnum.KW_NOT))
            {
                Factor();
            }
            else
            {
                // token sincronizante: FOLLOW(Factor_A) 
                TokensSincronizantes.Add(TokenEnum.OP_MUL);
                TokensSincronizantes.Add(TokenEnum.OP_DIV);
                TokensSincronizantes.Add(TokenEnum.KW_AND);
                TokensSincronizantes.Add(TokenEnum.OP_AD);
                TokensSincronizantes.Add(TokenEnum.OP_MIN);
                TokensSincronizantes.Add(TokenEnum.KW_OR);
                TokensSincronizantes.Add(TokenEnum.SMB_SEM);
                TokensSincronizantes.Add(TokenEnum.OP_EQ);
                TokensSincronizantes.Add(TokenEnum.OP_GT);
                TokensSincronizantes.Add(TokenEnum.OP_GE);
                TokensSincronizantes.Add(TokenEnum.OP_LT);
                TokensSincronizantes.Add(TokenEnum.OP_LE);
                TokensSincronizantes.Add(TokenEnum.OP_NE);
                TokensSincronizantes.Add(TokenEnum.SMB_CPA);
                SincronizaToken("'*', '/', 'and', '+', '-', 'or',';', '==', '>', '>=', '<', '<=', '!=', ')'");
                ErroSintatico("'ID','CON_NUM','CON_CHAR','(', 'KW_NOT'");
            }
        }
        #endregion

        #region [26 - Factor -> “id” | constant | “(“ expression “)” ]
        private void Factor()
        {
            if (Eat(TokenEnum.ID))
            {
                return;
            }
            else if (Token.EnumToken == TokenEnum.CON_NUM || Token.EnumToken == TokenEnum.CON_CHAR)
            {
                Constant();
            }
            else if (Eat(TokenEnum.SMB_OPA))
            {
                Expression();

                if (!Eat(TokenEnum.SMB_CPA))
                {
                    ErroSintatico("')'");
                }
            }
            else
            {
                // token sincronizante: FOLLOW(Factor) 
                TokensSincronizantes.Add(TokenEnum.OP_MUL);
                TokensSincronizantes.Add(TokenEnum.OP_DIV);
                TokensSincronizantes.Add(TokenEnum.KW_AND);
                TokensSincronizantes.Add(TokenEnum.OP_AD);
                TokensSincronizantes.Add(TokenEnum.OP_MIN);
                TokensSincronizantes.Add(TokenEnum.KW_OR);
                TokensSincronizantes.Add(TokenEnum.SMB_SEM);
                TokensSincronizantes.Add(TokenEnum.OP_EQ);
                TokensSincronizantes.Add(TokenEnum.OP_GT);
                TokensSincronizantes.Add(TokenEnum.OP_GE);
                TokensSincronizantes.Add(TokenEnum.OP_LT);
                TokensSincronizantes.Add(TokenEnum.OP_LE);
                TokensSincronizantes.Add(TokenEnum.OP_NE);
                TokensSincronizantes.Add(TokenEnum.SMB_CPA);
                SincronizaToken("'*', '/', 'and', '+', '-', 'or',';', '==', '>', '>=', '<', '<=', '!=', ')'");
                ErroSintatico("'ID','CON_NUM', 'CON_CHAR','('");
            }
        }
        #endregion

        #region [27 - Relop -> “==” | “>” | “>=” | “<” | “<=” | “!=” ]
        private void Relop()
        {
            if (!Eat(TokenEnum.OP_EQ) && !Eat(TokenEnum.OP_GT) && !Eat(TokenEnum.OP_GE) &&
                !Eat(TokenEnum.OP_LT) && !Eat(TokenEnum.OP_LE) && !Eat(TokenEnum.OP_NE))
            {
                // token sincronizante: FOLLOW(Relop) 
                TokensSincronizantes.Add(TokenEnum.ID);
                TokensSincronizantes.Add(TokenEnum.CON_NUM);
                TokensSincronizantes.Add(TokenEnum.CON_CHAR);
                TokensSincronizantes.Add(TokenEnum.SMB_OPA);
                TokensSincronizantes.Add(TokenEnum.KW_NOT);
                SincronizaToken("'id', 'num_const', 'char_const', '(', 'not'");
                ErroSintatico("'==', '>', '>=', '<', '<=', '!='");
            }
        }
        #endregion

        #region [28 - Addop -> “+” | “-” | “or” ]
        private void Addop()
        {
            if (!Eat(TokenEnum.OP_AD) && !Eat(TokenEnum.OP_MIN) && !Eat(TokenEnum.KW_OR))
            {
                // token sincronizante: FOLLOW(Addop) 
                TokensSincronizantes.Add(TokenEnum.ID);
                TokensSincronizantes.Add(TokenEnum.CON_NUM);
                TokensSincronizantes.Add(TokenEnum.CON_CHAR);
                TokensSincronizantes.Add(TokenEnum.SMB_OPA);
                TokensSincronizantes.Add(TokenEnum.KW_NOT);
                SincronizaToken("'id', 'num_const', 'char_const', '(', 'not'");
                ErroSintatico("'+', '-', 'OR'");
            }
        }
        #endregion

        #region [29 - Mulop -> “*” | “/” | “and”  ]
        private void Mulop()
        {
            if (!Eat(TokenEnum.OP_MUL) && !Eat(TokenEnum.OP_DIV) && !Eat(TokenEnum.KW_AND))
            {
                // token sincronizante: FOLLOW(Mulop) 
                TokensSincronizantes.Add(TokenEnum.ID);
                TokensSincronizantes.Add(TokenEnum.CON_NUM);
                TokensSincronizantes.Add(TokenEnum.CON_CHAR);
                TokensSincronizantes.Add(TokenEnum.SMB_OPA);
                TokensSincronizantes.Add(TokenEnum.KW_NOT);
                SincronizaToken("'id', 'num_const', 'char_const', '(', 'not'");
                ErroSintatico("'*', '/', 'AND'");
            }
        }
        #endregion

        #region [30 - Constant -> “num_const” | “char_const”]
        private void Constant()
        {
            if (!Eat(TokenEnum.CON_NUM) && !Eat(TokenEnum.CON_CHAR))
            {
                // token sincronizante: FOLLOW(Constant) 
                TokensSincronizantes.Add(TokenEnum.OP_MUL);
                TokensSincronizantes.Add(TokenEnum.OP_DIV);
                TokensSincronizantes.Add(TokenEnum.KW_AND);
                TokensSincronizantes.Add(TokenEnum.OP_AD);
                TokensSincronizantes.Add(TokenEnum.OP_MIN);
                TokensSincronizantes.Add(TokenEnum.KW_OR);
                TokensSincronizantes.Add(TokenEnum.SMB_SEM);
                TokensSincronizantes.Add(TokenEnum.OP_EQ);
                TokensSincronizantes.Add(TokenEnum.OP_GT);
                TokensSincronizantes.Add(TokenEnum.OP_GE);
                TokensSincronizantes.Add(TokenEnum.OP_LT);
                TokensSincronizantes.Add(TokenEnum.OP_LE);
                TokensSincronizantes.Add(TokenEnum.OP_NE);
                TokensSincronizantes.Add(TokenEnum.SMB_CPA);
                SincronizaToken("'*', '/', 'and', '+', '-', 'or',';', '==', '>', '>=', '<', '<=', '!=', ')'");
                ErroSintatico("'CON_NUM', 'CON_CHAR'");
            }
        }
        #endregion
    }
}