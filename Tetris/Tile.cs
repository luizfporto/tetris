using System.Drawing;

namespace Tetris
{
    class Tile
    {
        public static int Tamanho;

        Color _Cor;

        #region Propriedades
        public Color Cor
        {
            set { _Cor = value; }
            get { return _Cor; }
        }
        #endregion

        #region Construtores

        public Tile()
        {
            _Cor = Color.Transparent;
        }

        public Tile(Color cor)
        {
            _Cor = cor;
        }

        #endregion

        #region Métodos
        public void Desenhar(Graphics graphics, int i, int j)
        {
            Desenhar(graphics, i, j, _Cor);
        }
        public void Desenhar(Graphics graphics, int i, int j, Color cor)
        {
            Pen pen;
            Rectangle r = new Rectangle(j * Tamanho, i * Tamanho, Tamanho, Tamanho);
            graphics.FillRectangle(new SolidBrush(cor), r);
            if (cor == Color.Black)
            {
                pen = new Pen(Color.Gray, 1);
                graphics.DrawRectangle(pen, r);
            }
            else
            {
                pen = new Pen(CorClara(cor), 2);
                graphics.DrawLine(pen, new Point(j * Tamanho + 1, i * Tamanho + 1),
                new Point((j + 1) * Tamanho - 1, i * Tamanho + 1));
                graphics.DrawLine(pen, new Point(j * Tamanho + 1, i * Tamanho + 1),
                new Point(j * Tamanho + 1, (i + 1) * Tamanho - 1));
                pen = new Pen(CorEscura(cor), 2);
                graphics.DrawLine(pen, new Point(j * Tamanho + 1, (i + 1) * Tamanho - 1),
                new Point((j + 1) * Tamanho - 1, (i + 1) * Tamanho - 1));
                graphics.DrawLine(pen, new Point((j + 1) * Tamanho - 1, i * Tamanho + 1),
                new Point((j + 1) * Tamanho - 1, (i + 1) * Tamanho - 1));
            }
        }
        private Color CorEscura(Color cor)
        {
            return Color.FromArgb(
            (int)(cor.R * 0.75),
            (int)(cor.G * 0.75),
            (int)(cor.B * 0.75));
        }
        private Color CorClara(Color cor)
        {
            return Color.FromArgb(
            (cor.R + 50) < 256 ? (cor.R + 50) : 255,
            (cor.G + 50) < 256 ? (cor.G + 50) : 255,
            (cor.B + 50) < 256 ? (cor.B + 50) : 255);
        }
        #endregion
    }
}
