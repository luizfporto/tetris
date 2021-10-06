using System.Drawing;

namespace Tetris
{
    class Painel
    {
        private Tile[,] _Corpo;

        #region Propriedades

        public Tile[,] Corpo
        {
            set { _Corpo = value; }
            get { return _Corpo; }
        }
        #endregion

        #region Construtores
        public Painel()
        {
            int i, j;
            _Corpo = new Tile[20, 10];
            for (i = 0; i < 20; i++)
                for (j = 0; j < 10; j++)
                    _Corpo[i, j] = new Tile();
        }
        #endregion
        #region Métodos
        public void Desenhar(Graphics graphics)
        {
            int i, j;
            for (i = 0; i < 20; i++)
                for (j = 0; j < 10; j++)
                    if (_Corpo[i, j].Cor == Color.Transparent)
                        _Corpo[i, j].Desenhar(graphics, i, j, Color.Black);
                    else
                        _Corpo[i, j].Desenhar(graphics, i, j);
        }
        public void FixarPeca(Peca peca)
        {
            int i, j;
            for (i = 0; i < peca.Altura; i++)
                for (j = 0; j < peca.Largura; j++)
                    if (peca.Corpo[i, j].Cor != Color.Transparent)
                        _Corpo[peca.Linha + i, peca.Coluna + j].Cor = peca.Corpo[i, j].Cor;
        }

        public bool LinhaCheia(int linha)
        {
            bool cheia = true;
            int j;
            for (j = 0; j < 10 && cheia; j++)
                if (_Corpo[linha, j].Cor == Color.Transparent)
                    cheia = false;
            return cheia;
        }

        public void RemoverLinha(int linha)
        {
            int i, j;
            for (i = linha; i > 0; i--)
                for (j = 0; j < 10; j++)
                    _Corpo[i, j].Cor = _Corpo[i - 1, j].Cor;
            for (j = 0; j < 10; j++)
                _Corpo[0, j].Cor = Color.Transparent;
        }
        #endregion
    }
}