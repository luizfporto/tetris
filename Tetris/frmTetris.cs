using System;
using System.Drawing;
using System.Windows.Forms;

namespace Tetris
{
    public partial class frmTetris : Form
    {

        private Graphics graphics;
        private Painel painel;
        private Peca peca;
        private Timer timer;
        private int pontuacao;
        private int nivel;
        private DateTime inicionivel;
        private bool gameover;
        private bool pausa;

        public frmTetris()
        {
            InitializeComponent();
            Tile.Tamanho = picPainel.Width / 10;
            graphics = picPainel.CreateGraphics();
            painel = new Painel();
            timer = new Timer();
            timer.Enabled = false;
            timer.Tick += new EventHandler(EventoTimer);
        }

        private void sairToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void novoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            painel.Desenhar(graphics);
            peca = new Peca(Peca.Sortear());
            peca.Desenhar(graphics);
            timer.Enabled = true;
            timer.Interval = 500;
            timer.Start();
            pontuacao = 0;
            nivel = 1;
            inicionivel = DateTime.Now;
            gameover = false;
            pausa = false;

        }

        private void frmTetris_KeyDown(object sender, KeyEventArgs e)
        {
            if (!gameover)
            {
                if (e.KeyCode == Keys.Escape)
                    Pausar();
                if (!pausa)
                {
                    switch (e.KeyCode)
                    {
                        case Keys.Left:
                            if (peca.PodeMover(painel, Peca.Direcoes.Esquerda))
                                peca.Mover(Peca.Direcoes.Esquerda);
                            break;
                        case Keys.Right:
                            if (peca.PodeMover(painel, Peca.Direcoes.Direita))
                                peca.Mover(Peca.Direcoes.Direita);
                            break;
                        case Keys.Down:
                            if (peca.PodeMover(painel, Peca.Direcoes.Baixo))
                                peca.Mover(Peca.Direcoes.Baixo);
                            else
                            {
                                painel.FixarPeca(peca);
                                peca = new Peca(Peca.Sortear());
                            }
                            break;
                        case Keys.Up:
                            if (peca.PodeGirar(painel))
                                peca.Girar();
                            break;
                        case Keys.Enter:
                            while (peca.PodeMover(painel, Peca.Direcoes.Baixo))
                                peca.Mover(Peca.Direcoes.Baixo);
                            break;
                    }
                    painel.Desenhar(graphics);
                    peca.Desenhar(graphics);

                }
            }
        }

        public void EventoTimer(object sender, EventArgs e)
        {
            if(!gameover && !pausa)
             Atualizar();
        }

        private void Atualizar()
        {
            if (peca.PodeMover(painel, Peca.Direcoes.Baixo))
            {
                peca.Mover(Peca.Direcoes.Baixo);
                peca.Valor--;
            }
            else
            {
                painel.FixarPeca(peca);
                pontuacao += peca.Valor;
                int totlinhas = 0;
                for (int i = 0; i < 20; i++)
                {
                    if (painel.LinhaCheia(i))
                    {
                        painel.RemoverLinha(i);
                        totlinhas++;
                    }
                }
                if (totlinhas > 0)
                pontuacao += peca.Valor * Fatorial(totlinhas);
                peca = new Peca(Peca.Sortear());
                if (!peca.PodeEncaixar(painel))
                {
                    gameover = true;
                    MessageBox.Show("Game Over!", "Aviso",
                        MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
            }
            if(inicionivel.AddSeconds(60) < DateTime.Now)
            {
                nivel++;
                if(timer.Interval > 100)
                timer.Interval -= 100;
                inicionivel = DateTime.Now;
            }
            painel.Desenhar(graphics);
            peca.Desenhar(graphics);
            txtPontuacao.Text = pontuacao.ToString();
            txtNivel.Text = nivel.ToString();
        }

        private static int Fatorial(int n)
        {
            if (n == 1)
                return 1;
            else
                return n = Fatorial(n - 1);
        }

        private void pausaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Pausar();
        }

        private void Pausar()
        {
            pausa = !pausa;
            txtPausa.Visible = pausa;
        }
    }
}