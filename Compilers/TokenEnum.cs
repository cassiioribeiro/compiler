using System;
using System.Collections.Generic;
using System.Text;

namespace Compilers
{
    public enum TokenEnum
    {
        #region[FIM DE ARQUIVO]        
        EOF                          = 1, // Fim do leitura do arquivo
        #endregion

        #region[OPERADORES]
        OP_EQ                        = 2, // ==
        OP_NE                        = 3, // !=
        OP_GT                        = 4, // >
        OP_LT                        = 5, // <
        OP_GE                        = 6, // >=
        OP_LE                        = 7, // <=
        OP_AD                        = 8, // +
        OP_MIN                       = 9, // -
        OP_MUL                       = 10, // *
        OP_DIV                       = 11, // /
        OP_ASS                       = 12, // =
        #endregion
        
        #region[SIMBOLOS]
        SMB_OBC                      = 13, // {
        SMB_CBC                      = 14, // }
        SMB_OPA                      = 15, // (
        SMB_CPA                      = 16, // )
        SMB_COM                      = 17, // ,
        SMB_SEM                      = 18, // ;
        #endregion

        #region [PALAVRAS CHAVE]
        KW_PROGRAM                   = 19, // program
        KW_IF                        = 20, // if
        KW_ELSE                      = 21, // else
        KW_WHILE                     = 22, // while
        KW_WRITE                     = 23, // write    
        KW_READ                      = 24, // read
        KW_NUM                       = 25, // num
        KW_CHAR                      = 26, // char
        KW_NOT                       = 27, // not
        KW_OR                        = 28, // or
        KW_AND                       = 29, // and
        #endregion

        #region[IDENTIFICADORES]
        ID                           = 30, // identificador
        #endregion
        
        #region[LITERAL]
        LIT                          = 31, // pelo menos um dos 256 caracteres do conjunto ASCII entre aspas duplas. Ex: "AA", "/1",...
        #endregion
        
        #region[CONSTANTES] 
        CON_NUM                      = 32, // digit+ (“.” digit+)? Ex: 1, 111, 111.1111
        CON_CHAR                     = 33, // um dos 256 caracteres do conjunto ASCII entre aspas simples. EX: 'A', '.',...
        #endregion
    }
}
