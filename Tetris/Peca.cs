using System;
using System.Drawing;
using System.Linq;

namespace Tetris
{
    class Peca
    {
        public enum Formatos { Vazia, L, J, O, S, Z, T, I }

        public enum Direcoes { Esquerda, Direita, Baixo }

        private Formatos _Formato;
        private Tile[,] _Corpo;
        private int _Altura;
        private int _Largura;
        private int _Linha;
        private int _Coluna;
        private int _Valor;

        #region Propriedades
        public Formatos Formato
        {
            set { _Formato = value; }
            get { return _Formato; }
        }

        public Tile[,] Corpo
        {
            set { _Corpo = value; }
            get { return _Corpo; }
        }

        public int Altura
        {
            set { _Altura = value; }
            get { return _Altura; }
        }

        public int Largura
        {
            set { _Largura = value; }
            get { return _Largura; }
        }

        public int Linha
        {
            set { _Linha = value; }
            get { return _Linha; }
        }

        public int Coluna
        {
            set { _Coluna = value; }
            get { return _Coluna; }
        }

        public int Valor
        {
            set { _Valor = value;  }
            get { return _Valor; }
        }
        #endregion

        #region Construtores
        public Peca()
        {
            _Formato = Formatos.Vazia;
            _Linha = 0;
            _Coluna = 0;
            _Valor = 0;
            CriarCorpo();
            DefinirCorpo();
        }

        public Peca(Formatos formato)
        {
            _Formato = formato;
            _Linha = 0;
            _Coluna = 4;
            _Valor = 22;
            CriarCorpo();
            DefinirCorpo();
        }
        #endregion
        #region Métodos
        public void CriarCorpo()
        {
            int i, j;
            _Corpo = new Tile[4, 4];
            for (i = 0; i < 4; i++)
                for (j = 0; j < 4; j++)
                    _Corpo[i, j] = new Tile();
        }
        public void DefinirCorpo()
        {
            int i, j;
            for (i = 0; i < 4; i++)
                for (j = 0; j < 4; j++)
                    _Corpo[i, j].Cor = Color.Transparent;
            switch (_Formato)
            {
                case Formatos.Vazia:
                    _Altura = 4;
                    _Largura = 4;
                    break;
                case Formatos.L:
                    _Altura = 3;
                    _Largura = 2;
                    _Corpo[0, 0].Cor = Color.Yellow;
                    _Corpo[1, 0].Cor = Color.Yellow;
                    _Corpo[2, 0].Cor = Color.Yellow;
                    _Corpo[2, 1].Cor = Color.Yellow;
                    break;
                case Formatos.J:
                    _Altura = 3;
                    _Largura = 2;
                    _Corpo[0, 1].Cor = Color.Magenta;
                    _Corpo[1, 1].Cor = Color.Magenta;
                    _Corpo[2, 1].Cor = Color.Magenta;
                    _Corpo[2, 0].Cor = Color.Magenta;
                    break;
                case Formatos.O:
                    _Altura = 2;
                    _Largura = 2;
                    _Corpo[0, 0].Cor = Color.Cyan;
                    _Corpo[0, 1].Cor = Color.Cyan;
                    _Corpo[1, 0].Cor = Color.Cyan;
                    _Corpo[1, 1].Cor = Color.Cyan;
                    break;
                case Formatos.S:
                    _Altura = 2;
                    _Largura = 3;
                    _Corpo[0, 1].Cor = Color.GreenYellow;
                    _Corpo[0, 2].Cor = Color.GreenYellow;
                    _Corpo[1, 0].Cor = Color.GreenYellow;
                    _Corpo[1, 1].Cor = Color.GreenYellow;
                    break;
                case Formatos.Z:
                    _Altura = 2;
                    _Largura = 3;
                    _Corpo[0, 0].Cor = Color.Blue;
                    _Corpo[0, 1].Cor = Color.Blue;
                    _Corpo[1, 1].Cor = Color.Blue;
                    _Corpo[1, 2].Cor = Color.Blue;
                    break;
                case Formatos.T:
                    _Altura = 2;
                    _Largura = 3;
                    _Corpo[0, 1].Cor = Color.Purple;
                    _Corpo[1, 0].Cor = Color.Purple;
                    _Corpo[1, 1].Cor = Color.Purple;
                    _Corpo[1, 2].Cor = Color.Purple;
                    break;
                case Formatos.I:
                    _Altura = 4;
                    _Largura = 1;
                    _Corpo[0, 0].Cor = Color.Red;
                    _Corpo[1, 0].Cor = Color.Red;
                    _Corpo[2, 0].Cor = Color.Red;
                    _Corpo[3, 0].Cor = Color.Red;
                    break;
            }
        }

        public void Mover(Direcoes direcao)
        {
            switch (direcao)
            {
                case Direcoes.Esquerda: _Coluna--; break;
                case Direcoes.Direita: _Coluna++; break;
                case Direcoes.Baixo: _Linha++; break;
            }
        }

        public void Desenhar(Graphics graphics)
        {
            int i, j;
            for (i = 0; i < 4; i++)
                for (j = 0; j < 4; j++)
                    if (_Corpo[i, j].Cor != Color.Transparent)
                        _Corpo[i, j].Desenhar(graphics, _Linha + i, _Coluna + j);
        }

        public bool PodeMover(Painel painel, Direcoes direcao)
        {
            Peca peca = Clonar();
            peca.Mover(direcao);
            return peca.PodeEncaixar(painel);

        }

        public bool PodeEncaixar(Painel painel)
        {
            bool ok = true, tilepeca, tilepainel;
            int i, j;
            // Testa os limites do painel
            if (_Coluna < 0
                || _Coluna + _Largura > 10
                || _Linha + _Altura > 20)
                ok = false;
            // Testa em relação às outras peças
            if (ok)
            {
                for (i = 0; i < _Altura && ok; i++)
                {
                    for (j = 0; j < _Largura && ok; j++)
                    {
                        tilepeca = (_Corpo[i, j].Cor != Color.Transparent);
                        tilepainel = (painel.Corpo[i + _Linha, j + _Coluna].Cor !=
                        Color.Transparent);
                        if (tilepeca && tilepainel)
                            ok = false;

                    }
                }
            }
            return ok;
        }

        private Peca Clonar()
        {
            int i, j;
            Peca peca = new Peca();
            for (i = 0; i < 4; i++)
                for (j = 0; j < 4; j++)
                    peca.Corpo[i, j].Cor = _Corpo[i, j].Cor;
            peca.Formato = _Formato;
            peca.Altura = _Altura;
            peca.Largura = _Largura;
            peca.Linha = _Linha;
            peca.Coluna = _Coluna;
            return peca;

        }
        #endregion

        public static Formatos Sortear()
        {
            return (Formatos)Program.random.Next(1,
                (int)Enum.GetValues(typeof(Formatos)).Cast<Formatos>().Last());
        }

        public void Girar()
        {
            Tile[,] corpo = new Tile[4, 4];
            int largura, altura, i, j, k;
            bool vazia;
            // Rotaciona a matriz para a esquerda
            for (i = 0; i < 4; i++)
                for (j = 0; j < 4; j++)
                    corpo[3 - j, i] = new Tile(_Corpo[i, j].Cor);
            largura = _Altura;
            altura = _Largura;
            // Alinha ao topo
            for (i = 0; i < 4; i++)
            {
                vazia = true;
                for (j = 0; j < 4 && vazia; j++)
                    vazia = corpo[i, j].Cor == Color.Transparent;
                if (vazia)
                {
                    for (k = i; k < 3; k++)
                        for (j = 0; j < 4; j++)
                            corpo[k, j].Cor = corpo[k + 1, j].Cor;
                    for (j = 0; j < 4; j++)
                        corpo[k, j].Cor = Color.Transparent;
                }
            } // Alinha a esquerda
            for (j = 0; j < 4; j++)
            {
                vazia = true;
                for (i = 0; i < 4 && vazia; i++)
                    vazia = corpo[i, j].Cor == Color.Transparent;
                if (vazia)
                {
                    for (k = j; k < 3; k++)
                        for (i = 0; i < 4; i++)
                            corpo[i, k].Cor = corpo[i, k + 1].Cor;
                    for (i = 0; i < 4; i++)
                        corpo[i, k].Cor = Color.Transparent;
                }
            }
            // Atribui o corpo transposto e espelhado
            for (i = 0; i < 4; i++)
                for (j = 0; j < 4; j++)
                    _Corpo[i, j].Cor = corpo[i, j].Cor;
            _Altura = altura;
            _Largura = largura;
        }
        public bool PodeGirar(Painel painel)
        {
            Peca peca = Clonar();
            peca.Girar();
            return peca.PodeEncaixar(painel);
        }
    }
}
        
    

