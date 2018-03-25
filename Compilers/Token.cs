namespace Compilers
{
    public class Token
    {
        public TokenEnum Classe {
            get { return Classe; }
            private set { }
        }

        public string Lexema
        {
            get { return Lexema; }
            private set { }
        }

        public int Linha
        {
            get { return Linha; }
            private set { }
        }

        public int Coluna
        {
            get { return Coluna; }
            private set { }
        }

        public string IdValue
        {
            get { return IdValue; }
            private set { }
        }

        public Token(TokenEnum classe, string lexema, int linha, int coluna) 
        {
            Classe = classe;
            Lexema = lexema;
            Linha = linha;
            Coluna = coluna;
        }

        public void SetClasse(TokenEnum classe) 
        {
            Classe = classe;
        }

        public void SetLexema(string lexema) 
        {
            Lexema = lexema;
        }

        public void SetIdValue(string idValue) 
        {    
            IdValue = idValue;
        }

        public void SetLinha(int linha) 
        {
            Linha = linha;
        }

        public void SetColuna(int coluna) 
        {
            Coluna = coluna;
        }

        public override string ToString() {
            return "<" + Classe + ", \"" + Lexema + "\">";
        }
       
    }
}