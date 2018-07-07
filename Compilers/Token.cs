using System;

namespace Compilers
{
    public class Token
    {
        /// <summary>
        /// Atributos do Token.
        /// </summary>
        public string Lexema { get; private set; }
        public TokenEnum EnumToken { get; private set; }
        public int Linha { get; set; }
        public int Coluna { get; set; }

        /// <summary>
        /// Construtor da classe, atribui valores para os atributos.
        /// </summary>
        /// <param name="enumToken">Enum do token</param>
        /// <param name="lexema">Lexema do token </param>
        /// <param name="linha">Linha do token</param>
        /// <param name="coluna">Coluna do token</param>
        public Token(TokenEnum enumToken, String lexema, int linha, int coluna)
        {
            EnumToken = enumToken;
            Lexema = lexema;
            Linha = linha;
            Coluna = coluna;
        }

        /// <summary>
        /// Sobrepondo o ToString() do c#.
        /// </summary>
        /// <returns>Retorna uma string com a classe e o lexema.</returns>
        public override string ToString() {
            return String.Format("<{0}, \"{1}\">", EnumToken, Lexema);
        }
       
    }
}