namespace Compilers
{
    public class Entidade
    {
        #region[OPERADORES]
        public readonly static int OP_EQ                        = 1; // ==
        public readonly static int OP_NE                        = 2; // !=
        public readonly static int OP_GT                        = 3; // >
        public readonly static int OP_LT                        = 4; // <
        public readonly static int OP_GE                        = 5; // >=
        public readonly static int OP_LE                        = 6; // <=
        public readonly static int OP_AD                        = 7; // +
        public readonly static int OP_MIN                       = 8; // -
        public readonly static int OP_MUL                       = 9; // *
        public readonly static int OP_DIV                       = 10; // /
        public readonly static int OP_ASS                       = 11; // =
        #endregion
        
        #region[SIMBOLOS]
        public readonly static int SMB_OBC                      = 12; // {
        public readonly static int SMB_CBC                      = 13; // }
        public readonly static int SMB_OPA                      = 14; // (
        public readonly static int SMB_CPA                      = 15; // )
        public readonly static int SMB_COM                      = 16; // ,
        public readonly static int SMB_SEM                      = 17; // ;
        #endregion

        #region [PALAVRAS CHAVE]
        public readonly static int KW_PROGRAM                   = 18; // program
        public readonly static int KW_IF                        = 19; // if
        public readonly static int KW_ELSE                      = 20; // else
        public readonly static int KW_WHILE                     = 21; // while
        public readonly static int KW_WRITE                     = 22; // write    
        public readonly static int KW_READ                      = 23; // read
        public readonly static int KW_NUM                       = 24; // num
        public readonly static int KW_CHAR                      = 25; // char
        public readonly static int KW_NOT                       = 26; // not
        public readonly static int KW_OR                        = 27; // or
        public readonly static int KW_AND                       = 28; // and
        #endregion

        #region[IDENTIFICADORES]
        public readonly static int ID                           = 29; // identificador
        #endregion
        
        #region[LITERAL]
        public readonly static int LIT                          = 30; // pelo menos um dos 256 caracteres do conjunto ASCII entre aspas duplas. Ex: "AA", "/1",...
        #endregion
        
        #region[CONSTANTES] 
        public readonly static int CON_NUM                      = 31; // digit+ (“.” digit+)? Ex: 1, 111, 111.1111
        public readonly static int CON_CHAR                     = 32; // um dos 256 caracteres do conjunto ASCII entre aspas simples. EX: 'A', '.',...
        #endregion
    }
}