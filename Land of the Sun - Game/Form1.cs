using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Common;
using System.Diagnostics.Eventing.Reader;
using System.Drawing;
using System.Drawing.Text;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.Remoting.Channels;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using AxWMPLib;
using NAudio.Wave;
using static System.Net.Mime.MediaTypeNames;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace Land_of_the_Sun___Game
{
    public partial class Form1 : Form
    {
        #region Configurações Player e Animação
        System.Drawing.Image player;
        sbyte larguraPlayer = 70;
        sbyte alturaPlayer = 125;
        int playerX = 2000;
        int playerY = 2000;

        bool direita, esquerda, cima, baixo;

        sbyte velocidadePlayer = 10;

        sbyte steps = 0;
        sbyte taxaDeQuadros = 0;

        List<string> lista = new List<string>(); //É criado uma lista de 'strings' para armazenar os CAMINHOS DOS ARQUIVOS e não as imagens em sí.
        Graphics Canvas;
        Rectangle jogador;
        #endregion

        #region Configurações Gerais
        byte tempo = 210, intervalotempo = 0, tempoerro = 0;

        sbyte tempoPreparo = 0;

        sbyte cuscuz = 0, cuscuzPronto = 0, ovos = 0, ovosfrito = 0, carne = 0, carnePronta = 0, calabresa = 0, calabresaPronta = 0;
        sbyte fogaoCheio = 0;
        sbyte preparando = 0;

        bool OVOPRONTO = false, CUSCUZPRONTO = false, CARNEPRONTA = false, CALABRESAPRONTA = false;

        bool ovoColocadoNaBancada, carneColocadoNaBancada, cuscuzColocadoNaBancada, calabresaColocadoNaBancada;
        bool pedidoProntoCOMPLETO, pedidoProntoCUSCUZ_OVO, pedidoProntoCUSCUZ_CARNE_CALABRESA, pedidoProntoCUSCUZ_CARNE, pedidoPronto_CUSCUZ_OVO_CALABRESA, pedidoPronto_CUSCUZ_OVO_CARNE, pedidoPronto_CUSCUZ_CALABRESA;
        bool necessitaCOMPLETO, necessitaCUSCUZ_OVO, necessitaCUSCUZ_CARNE_CALABRESA, necessitaCUSCUZ_CARNE, necessitaCUSCUZ_OVO_CALABRESA, necessitaCUSCUZ_OVO_CARNE, necessitaCUSCUZ_CALABRESA;

        bool pegouPedido = false;
        bool pause;
        bool jogando;
        bool panelaNoFogo;

        bool cursorVisivel;
        bool interseccao;

        System.Drawing.Image[] ingredienteCru = new System.Drawing.Image[4];

        #endregion

        #region Configurações de Músicas
        private AudioFileReader musicaFundo1;
        private AudioFileReader musicaFundo2;
        private AudioFileReader efeitoSonoroDerrota;
        private AudioFileReader erroPedido;
        private AudioFileReader acertoPedido;
        private AudioFileReader fritando;
        private AudioFileReader fritandoCalabresa;
        private AudioFileReader fritandoCarne;
        private AudioFileReader cozinhandoCuscuz;
        private AudioFileReader cozinhandoCarne;
        private AudioFileReader efeitoTimer;
        private WaveOutEvent saidaMusicaFundo1;
        private WaveOutEvent saidaMusicaFundo2;
        private WaveOutEvent somEfeitoSonoroDerrota;
        private WaveOutEvent somErroPedido;
        private WaveOutEvent somAcertoPedido;
        private WaveOutEvent somFritando;
        private WaveOutEvent somFritandoCalabresa;
        private WaveOutEvent somFritandoCarne;
        private WaveOutEvent somCozinhandoCuscuz;
        private WaveOutEvent somCozinhandoCarne;
        private WaveOutEvent somEfeitoTimer;

        private AudioFileReader vitoriaSOM;
        private WaveOutEvent vitoria;

        bool musicaDerrota = false;
        int tem = 0;
        #endregion

        #region Configurações de Pedidos

        //----Pedidos-----//
        Random random = new Random();
        List<PictureBox> pbs = new List<PictureBox>();
        sbyte quantidadeDePedidos = 0, tempoPedido = 0;

        

        sbyte PedidosParaVitoria = 4;

        bool[] sortearCorretamente = new bool[4];

        int sorteioPedido = 0;
        int sorteioPossibilidade = 0;
        bool[] possiblidadesDePedidos = new bool[7];
        System.Drawing.Image SPRITECUSCUZCOMOVOCARNE, SPRITECUSCUZCOMOVOCALABRESA, cuzcuzovo, cuzcuzcalabresa, cucuzcompleto, cucuzcarnecalabresa, cucuzcarne;
        System.Drawing.Image[] receitasNAO_PRONTAS = new System.Drawing.Image[8];

        #endregion

        private PrivateFontCollection fonte = new PrivateFontCollection();
        string fontePack;

        sbyte tempoButtonE = 0;
        bool buttonE = false;

        #region Inicialização de Formulário
        public Form1()
        {
            InitializeComponent();

            Cursor.Hide();
            cursorVisivel = false;

            this.DoubleBuffered = true;
            this.FormBorderStyle = FormBorderStyle.None;

            musicaFundo1 = new AudioFileReader("menuinicial.wav");
            saidaMusicaFundo1 = new WaveOutEvent();
            saidaMusicaFundo1.Init(musicaFundo1);

            musicaFundo2 = new AudioFileReader("gameplay.wav");
            saidaMusicaFundo2 = new WaveOutEvent();
            saidaMusicaFundo2.Init(musicaFundo2);

            vitoriaSOM = new AudioFileReader("vitoria.wav");
            vitoria = new WaveOutEvent();
            vitoria.Init(vitoriaSOM);

            efeitoSonoroDerrota = new AudioFileReader("fimdetempo.wav");
            somEfeitoSonoroDerrota = new WaveOutEvent();
            somEfeitoSonoroDerrota.Init(efeitoSonoroDerrota);

            fritando = new AudioFileReader("fritandoOvo.wav");
            somFritando = new WaveOutEvent();
            somFritando.Init(fritando);

            fritandoCalabresa = new AudioFileReader("fritandoCalabresa.wav");
            somFritandoCalabresa = new WaveOutEvent();
            somFritandoCalabresa.Init(fritandoCalabresa);

            fritandoCarne = new AudioFileReader("fritandoCarne.wav");
            somFritandoCarne = new WaveOutEvent();
            somFritandoCarne.Init(fritandoCarne);

            cozinhandoCuscuz = new AudioFileReader("cozinhandoCuscuz.wav");
            somCozinhandoCuscuz = new WaveOutEvent();
            somCozinhandoCuscuz.Init(cozinhandoCuscuz);

            cozinhandoCarne = new AudioFileReader("cozinhandoCarne.wav");
            somCozinhandoCarne = new WaveOutEvent();
            somCozinhandoCarne.Init(cozinhandoCarne);

            efeitoTimer = new AudioFileReader("EfeitoTimer.wav");
            efeitoTimer.Volume = 0.5f;
            somEfeitoTimer = new WaveOutEvent();
            somEfeitoTimer.Init(efeitoTimer);
            

            erroPedido = new AudioFileReader("erroPedido.wav");
            acertoPedido = new AudioFileReader("acertoPedido.wav");
            somErroPedido = new WaveOutEvent();
            somAcertoPedido = new WaveOutEvent();
            somErroPedido.Init(erroPedido);
            somAcertoPedido.Init(acertoPedido);

            ReiniciarMusica();
            ReiniciarEfeitosSonoros();

            lista = Directory.GetFiles("tia", "*.png").ToList(); // Aqui estão armazenados os caminhos dos arquivos

            pbs.Add(pedido1);
            pbs.Add(pedido2);
            pbs.Add(pedido3);
            pbs.Add(pedido4);

            foreach (Control control in this.Controls)
            {
                if (control is PictureBox)
                {
                    control.Visible = false;
                }
            }
            foreach (Control x in this.Controls)
            {
                if (x is PictureBox && (string)x.Tag == "panelas")
                {
                    x.BackColor = Color.Black;
                }
            }

            musicaFundo1.Volume = 0.7f;
            musicaFundo2.Volume = 0.7f;
            vitoria.Volume = 0.7f;

            cucuzcarne = System.Drawing.Image.FromFile("cucuzcarne.png");
            cuzcuzovo = System.Drawing.Image.FromFile("cuzcuzovo.png");
            cuzcuzcalabresa = System.Drawing.Image.FromFile("cuzcuzcalabresa.png");
            cucuzcompleto = System.Drawing.Image.FromFile("cucuzcompleto.png");
            cucuzcarnecalabresa = System.Drawing.Image.FromFile("cucuzcarnecalabresa.png");
            SPRITECUSCUZCOMOVOCALABRESA = System.Drawing.Image.FromFile("SPRITE CUSCUZ COM OVO CALABRESA.png");
            SPRITECUSCUZCOMOVOCARNE = System.Drawing.Image.FromFile("SPRITE CUSCUZ COM OVO CARNE.png");

            receitasNAO_PRONTAS[7] = System.Drawing.Image.FromFile("CALABRESA E CARNE.png");
            receitasNAO_PRONTAS[6] = System.Drawing.Image.FromFile("SPRITE OVO, CARNE PRONTA E CALABRESA.png");
            receitasNAO_PRONTAS[5] = System.Drawing.Image.FromFile("SPRITE OVO E  CARNE PRONTA.png");
            receitasNAO_PRONTAS[4] = System.Drawing.Image.FromFile("SPRITE OVO E  CALABRESA.png");
            receitasNAO_PRONTAS[3] = System.Drawing.Image.FromFile("ovoprato.png");
            receitasNAO_PRONTAS[2] = System.Drawing.Image.FromFile("calabresacortadaprato.png");
            receitasNAO_PRONTAS[1] = System.Drawing.Image.FromFile("SPRITE CARNE PRONTA.png");
            receitasNAO_PRONTAS[0] = System.Drawing.Image.FromFile("cuzcuzprato.png");

            ingredienteCru[0] = System.Drawing.Image.FromFile("ovos.png");
            ingredienteCru[1] = System.Drawing.Image.FromFile("calabresacortada.png");
            ingredienteCru[2] = System.Drawing.Image.FromFile("SPRITE CARNE CRUA.png");
            ingredienteCru[3] = System.Drawing.Image.FromFile("cuzcuz.png");

            player = System.Drawing.Image.FromFile(lista[4]);

            pictureBox29.Visible = true;
            OpcoesButton.Visible = true;
            pictureBox31.Visible = true;

            CustomizarFonte();

            axWindowsMediaPlayer1.URL = @"ganbiarraStudio.mp4";
            // Configura o Media Player para não mostrar controles
            axWindowsMediaPlayer1.uiMode = "none";
            axWindowsMediaPlayer1.Ctlcontrols.play();
        }
        #endregion

        private void CustomizarFonte()
        {
            // Caminho para a fonte adicionada ao projeto
            fontePack = Path.Combine(System.Windows.Forms.Application.StartupPath, "Fonte", "Buycat.ttf");

            // Carregar a fonte
            fonte.AddFontFile(fontePack);

            // Aplicar a fonte personalizada a um label

            label1.Font = new Font(fonte.Families[0], 26, FontStyle.Regular);
            label2.Font = new Font(fonte.Families[0], 24, FontStyle.Regular);
        }

        #region Reinicio De Músicas e Efeitos Sonoros
        private void ReiniciarMusica()
        {
            // Detecta quando a música acaba
            saidaMusicaFundo1.PlaybackStopped += (sender, e) =>
            {
                // Verifica se a posição chegou ao final do arquivo
                if (musicaFundo1.Position >= musicaFundo1.Length)
                {
                    musicaFundo1.Position = 0; // Reseta a posição da música
                    saidaMusicaFundo1.Volume = volumeSlider1.Volume;
                    saidaMusicaFundo1.Play(); // Reproduz novamente
                }
            };

            // Detecta quando a música acaba
            saidaMusicaFundo2.PlaybackStopped += (sender, e) =>
            {
                // Verifica se a posição chegou ao final do arquivo
                if (musicaFundo2.Position >= musicaFundo2.Length)
                {
                    musicaFundo2.Position = 0; // Reseta a posição da música
                    saidaMusicaFundo2.Volume = volumeSlider1.Volume;
                    saidaMusicaFundo2.Play(); // Reproduz novamente
                }
            };
        }

        private void ReiniciarEfeitosSonoros()
        {
            somErroPedido.PlaybackStopped += (sender, e) =>
            {
                if (erroPedido.Position >= erroPedido.Length)
                {
                    erroPedido.Position = 0;
                    erroPedido.Volume = volumeSlider2.Volume;
                }
            };

            somAcertoPedido.PlaybackStopped += (sender, e) =>
            {
                if (acertoPedido.Position >= acertoPedido.Length)
                {
                    acertoPedido.Position = 0;
                    somAcertoPedido.Volume = volumeSlider2.Volume;
                }
            };

            somFritando.PlaybackStopped += (sender, e) =>
            {
                if (fritando.Position >= fritando.Length)
                {
                    fritando.Position = 0;
                    fritando.Volume = volumeSlider2.Volume;
                }
            };

            somFritandoCalabresa.PlaybackStopped += (sender, e) =>
            {
                if (fritandoCalabresa.Position >= fritandoCalabresa.Length)
                {
                    fritandoCalabresa.Position = 0;
                    fritandoCalabresa.Volume = volumeSlider2.Volume;
                }
            };

            somCozinhandoCuscuz.PlaybackStopped += (sender, e) =>
            {
                if (cozinhandoCuscuz.Position >= cozinhandoCuscuz.Length)
                {
                    cozinhandoCuscuz.Position = 0;
                    cozinhandoCuscuz.Volume = volumeSlider2.Volume;
                }
            };

            somCozinhandoCarne.PlaybackStopped += (sender, e) =>
            {
                if (cozinhandoCarne.Position >= cozinhandoCarne.Length)
                {
                    cozinhandoCarne.Position = 0;
                    cozinhandoCarne.Volume = volumeSlider2.Volume;
                }
            };

            vitoria.PlaybackStopped += (sender, e) =>
            {
                if (vitoriaSOM.Position >= vitoriaSOM.Length)
                {
                    vitoriaSOM.Position = 0;
                    vitoriaSOM.Volume = volumeSlider2.Volume;
                }
            };

            somEfeitoSonoroDerrota.PlaybackStopped += (sender, e) =>
            {
                if (efeitoSonoroDerrota.Position >= efeitoSonoroDerrota.Length)
                {
                    efeitoSonoroDerrota.Position = 0;
                    efeitoSonoroDerrota.Volume = volumeSlider2.Volume;
                }
            };

            somEfeitoTimer.PlaybackStopped += (sender, e) =>
            {
                if (efeitoTimer.Position >= efeitoTimer.Length)
                {
                    efeitoTimer.Position = 0;
                    if (volumeSlider2.Volume > 0.6f)
                    {
                        efeitoTimer.Volume = volumeSlider2.Volume - 0.3f;
                    }
                    else if (volumeSlider2.Volume > 0.3f && volumeSlider2.Volume <= 0.6f)
                    {
                        efeitoTimer.Volume = volumeSlider2.Volume - 0.2f;
                    }
                    else if (volumeSlider2.Volume > 0.1 && volumeSlider2.Volume <= 0.3)
                    {
                        efeitoTimer.Volume = volumeSlider2.Volume - 0.1f;
                    }
                    else if (volumeSlider2.Volume <= 0.1)
                    {
                        efeitoTimer.Volume = 0f;
                    }
                }
            };
        }
        #endregion

        private void PlayStateChange(object sender, AxWMPLib._WMPOCXEvents_PlayStateChangeEvent e)
        {
            // Verifica se o vídeo terminou
            if (e.newState == 8)// 8 é o estado 'MediaEnded'
            {
                axWindowsMediaPlayer1.Hide();
                saidaMusicaFundo1.Play();

                Cursor.Show();
                cursorVisivel = true;
                e.newState = 1;
                axWindowsMediaPlayer1.Dispose();
            }
        }

        #region Botão Opções
        //Botão Opções
        private void CliqueOpcoes(object sender, EventArgs e)
        {
            this.BackgroundImage = System.Drawing.Image.FromFile("menuOpcoes.png");

            timer5.Start();

            foreach (Control control in this.Controls)
            {
                if (control is PictureBox)
                {
                    control.Visible = false;
                }
            }
            label2.BackColor = Color.Transparent;
            label2.Visible = true;

            volumeSlider1.Visible = true;
            volumeSlider2.Visible = true;
            pictureBox32.Visible = true;

        }
        #endregion

        //Botão Voltar
        private void pictureBox32_Click(object sender, EventArgs e)
        {
            VoltarAoMenu();

            label2.Visible = false;
            label2.BackColor = Color.White;
            saidaMusicaFundo2.Pause();
            saidaMusicaFundo1.Play();
        }

        //Botão Sair
        private void pictureBox31_Click(object sender, EventArgs e)
        {
            System.Windows.Forms.Application.Exit();
        }


        #region Botão Jogar
        private void pictureBox29_Click(object sender, EventArgs e)
        {
            foreach (Control control in this.Controls)
            {
                if (control is PictureBox)
                {
                    control.Visible = true;
                }
            }
            foreach (Control x in this.Controls)
            {
                if (x is PictureBox && (string)x.Tag == "panelas")
                {
                    x.Visible = false;
                }
            }
            if (cursorVisivel)
            {
                Cursor.Hide();
                cursorVisivel = false;
            }

            

            ZerarValores();

            PBpause.Visible = false;

            pictureBox6.Visible = false;
            pictureBox29.Visible = false;
            OpcoesButton.Visible = false;
            pictureBox31.Visible = false;

            pictureBox29.BackColor = Color.White;
            OpcoesButton.BackColor = Color.White;
            pictureBox31.BackColor = Color.White;

            panela10.Visible = false;
            panela4.Visible = false;
            PBingrediente.Visible = false;
            panela7.Visible = false;
            pedido1.Visible = false;
            pedido2.Visible = false;
            pedido3.Visible = false;
            pedido4.Visible = false;

            this.BackgroundImage = System.Drawing.Image.FromFile("FASE 1_114236.png");

            pbs.Add(pedido1);
            pbs.Add(pedido2);
            pbs.Add(pedido3);
            pbs.Add(pedido4);

            saidaMusicaFundo1.Pause();
            saidaMusicaFundo2.Play();

            

            timer1.Start();
            timer3.Start();
            timer4.Start();

            E.Visible = false;

            label1.Visible = true;

            player = System.Drawing.Image.FromFile(lista[4]); // Aqui inicia o player com a primeira imagem que foi adicionada a lista


            //-------------------------------------------------------------//
            sorteioPossibilidade = random.Next(0, 7);
            sorteioPedido = random.Next(0, 4);

            sortearCorretamente[sorteioPedido] = true;
            possiblidadesDePedidos[sorteioPossibilidade] = true;
            ImagemParaPedidoPosSorteio(sorteioPossibilidade, pbs[sorteioPedido]);

            pbs[sorteioPedido].Visible = true;

            quantidadeDePedidos++;



            this.Focus();



        }
        #endregion

        #region Zerar Valores
        private void ZerarValores()
        {
            musicaDerrota = false;
            tem = 0;

            tempo = 210;
            jogando = true;
            pause = false;

            cuscuz = 0; cuscuzPronto = 0; ovos = 0; ovosfrito = 0; carne = 0; carnePronta = 0; calabresa = 0; calabresaPronta = 0;
            fogaoCheio = 0;
            preparando = 0;
            tempoPreparo = 0;

            OVOPRONTO = false; CUSCUZPRONTO = false; CARNEPRONTA = false; CALABRESAPRONTA = false;

            quantidadeDePedidos = 0; tempoPedido = 0;
            PedidosParaVitoria = 4;

            sorteioPedido = 0;

            ovoColocadoNaBancada = false; carneColocadoNaBancada = false; cuscuzColocadoNaBancada = false; calabresaColocadoNaBancada = false;
            pedidoProntoCOMPLETO = false; pedidoProntoCUSCUZ_OVO = false; pedidoProntoCUSCUZ_CARNE_CALABRESA = false; pedidoProntoCUSCUZ_CARNE = false; pedidoPronto_CUSCUZ_OVO_CALABRESA = false; pedidoPronto_CUSCUZ_OVO_CARNE = false; pedidoPronto_CUSCUZ_CALABRESA = false;
            necessitaCOMPLETO = false; necessitaCUSCUZ_OVO = false; necessitaCUSCUZ_CARNE_CALABRESA = false; necessitaCUSCUZ_CARNE = false; necessitaCUSCUZ_OVO_CALABRESA = false; necessitaCUSCUZ_OVO_CARNE = false; necessitaCUSCUZ_CALABRESA = false;

            panelaNoFogo = false;

            playerX = 255;
            playerY = 340;

            sortearCorretamente[0] = false;
            sortearCorretamente[1] = false;
            sortearCorretamente[2] = false;
            sortearCorretamente[3] = false;

            sorteioPossibilidade = 0;
            for (int i = 0; i < 7; i++)
            {
                possiblidadesDePedidos[i] = false;
            }

            efeitoSonoroDerrota.Position = 0;
            vitoriaSOM.Position = 0;
            cozinhandoCarne.Position = 0;
            cozinhandoCuscuz.Position = 0;
            fritandoCalabresa.Position = 0;
            fritando.Position = 0;
            acertoPedido.Position = 0;
            erroPedido.Position = 0;
            musicaFundo2.Position = 0;
            musicaFundo1.Position = 0;
            efeitoTimer.Position = 0;





            

            tempoButtonE = 0;
            buttonE = false;

            pegouPedido = false;
        }
        #endregion

        #region Voltar Ao Menu

        private void VoltarAoMenu()
        {
            timer1.Stop();
            timer2.Stop();
            timer3.Stop();
            timer4.Stop();
            timer5.Stop();

            jogando = false;



            foreach (Control control in this.Controls)
            {
                if (control is PictureBox)
                {
                    control.Visible = false;
                }
            }
            volumeSlider1.Visible = false;
            volumeSlider2.Visible = false;
            pictureBox32.Visible = false;

            pictureBox29.Visible = true;
            OpcoesButton.Visible = true;
            pictureBox31.Visible = true;
            label1.Visible = false;
            progressBar1.Visible = false;

            

            this.BackgroundImage = System.Drawing.Image.FromFile("MENU FUNDO 2.png");

            playerX = 2000;
            playerY = 2000;

            pictureBox29.BackColor = Color.Transparent;
            OpcoesButton.BackColor = Color.Transparent;
            pictureBox31.BackColor = Color.Transparent;
        }

        #endregion

        #region Animação

        //------------------------------------------------------------//
        //----------Método para realizar a animação-----------//
        //------------------------------------------------------------//
        private void AnimatePlayer(sbyte inicio, sbyte fim)
        {
            taxaDeQuadros++; // Sempre que o AnimatePlayer estiver sendo chamado no Timer, a taxaDeQuadros vai aumentar

            if (taxaDeQuadros == 4)  //sempre que a quarta imagem aparecer adiciona mais outra imagem
            {
                steps++;
                taxaDeQuadros = 0;
            }

            if (steps < inicio || steps > fim) // Se a numeração de tal imagem for menor ou maior que o intervalo escolhido, a imagem volta para o loop 
            {
                steps = inicio;
            }

            player = System.Drawing.Image.FromFile(lista[steps]); // Coloca a imagem ao player conforme o loop vai alterando as imagens
        }

        private void EventoDePintura(object sender, PaintEventArgs e)
        {
            Canvas = e.Graphics;
            Canvas.DrawImage(player, playerX, playerY, larguraPlayer, alturaPlayer);
        }
        #endregion

        #region Eventos de Teclas
        private void EventoKeyDown(object sender, KeyEventArgs e)
        {

            switch (e.KeyCode)
            {
                case Keys.W:
                    {
                        cima = true;
                        break;
                    }
                case Keys.S:
                    {
                        baixo = true;
                        break;
                    }
                case Keys.A:
                    {
                        esquerda = true;
                        break;
                    }
                case Keys.D:
                    {
                        direita = true;
                        break;
                    }
            }
        }

        private void EventoKeyUp(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.W:
                    {
                        cima = false;
                        break;
                    }
                case Keys.S:
                    {
                        baixo = false;
                        break;
                    }
                case Keys.A:
                    {
                        esquerda = false;
                        break;
                    }
                case Keys.D:
                    {
                        direita = false;
                        break;
                    }
            }

            if (jogador.IntersectsWith(PBbalcao.Bounds) && e.KeyCode == Keys.E && !pegouPedido && PBingrediente.Visible == false && panela4.BackgroundImage != null)
            {
                pegouPedido = !pegouPedido;

                PBingrediente.BackgroundImage = panela4.BackgroundImage;
                PBingrediente.Visible = true;

                panela4.Visible = false;
            }
            else if (jogador.IntersectsWith(PBbalcao.Bounds) && e.KeyCode == Keys.E && pegouPedido && PBingrediente.Visible == true)
            {
                pegouPedido = !pegouPedido;

                panela4.BackgroundImage = PBingrediente.BackgroundImage;
                PBingrediente.Visible = false;

                panela4.Visible = true;
            }

            if ((e.KeyCode == Keys.Escape || e.KeyCode == Keys.Tab) && jogando)
            {
                switch (pause)
                {
                    case false:
                        {
                            pause = true;

                            somCozinhandoCarne.Pause();
                            somCozinhandoCuscuz.Pause();
                            somFritando.Pause();
                            somFritandoCalabresa.Pause();
                            somEfeitoTimer.Pause();


                            if (!cursorVisivel)
                            {
                                cursorVisivel = true;
                                Cursor.Show();
                            }

                            timer1.Stop();
                            timer3.Stop();
                            timer4.Stop();

                            saidaMusicaFundo2.Pause();

                            PBpause.Visible = true;
                            PBpause.BackColor = Color.Transparent;

                            if (panelaNoFogo)
                            {
                                timer2.Stop();
                            }

                            break;
                        }
                    case true:
                        {
                            pause = false;

                            if (cursorVisivel)
                            {
                                cursorVisivel = false;
                                Cursor.Hide();
                            }

                            timer1.Start();
                            timer3.Start();
                            timer4.Start();

                            saidaMusicaFundo2.Play();

                            PBpause.Visible = false;
                            PBpause.BackColor = Color.White;

                            if (panelaNoFogo)
                            {
                                timer2.Start();
                            }

                            if (panelaNoFogo && preparando == 0 && cozinhandoCuscuz.CurrentTime >= TimeSpan.FromSeconds(0) && cozinhandoCuscuz.CurrentTime <= TimeSpan.FromSeconds(1))
                            {
                                cozinhandoCuscuz.CurrentTime = TimeSpan.FromSeconds(0);
                                somCozinhandoCuscuz.Play();
                            }
                            else if (panelaNoFogo && preparando == 0 && cozinhandoCuscuz.CurrentTime >= TimeSpan.FromSeconds(1) && cozinhandoCuscuz.CurrentTime <= TimeSpan.FromSeconds(2))
                            {
                                cozinhandoCuscuz.CurrentTime = TimeSpan.FromSeconds(1);
                                somCozinhandoCuscuz.Play();
                            }
                            else if (panelaNoFogo && preparando == 0 && cozinhandoCuscuz.CurrentTime >= TimeSpan.FromSeconds(2) && cozinhandoCuscuz.CurrentTime <= TimeSpan.FromSeconds(3))
                            {
                                cozinhandoCuscuz.CurrentTime = TimeSpan.FromSeconds(2);
                                somCozinhandoCuscuz.Play();
                            }
                            else if (panelaNoFogo && preparando == 0 && cozinhandoCuscuz.CurrentTime >= TimeSpan.FromSeconds(3) && cozinhandoCuscuz.CurrentTime <= TimeSpan.FromSeconds(4))
                            {
                                cozinhandoCuscuz.CurrentTime = TimeSpan.FromSeconds(3);
                                somCozinhandoCuscuz.Play();
                            }
                            else if (panelaNoFogo && preparando == 0 && cozinhandoCuscuz.CurrentTime >= TimeSpan.FromSeconds(4) && cozinhandoCuscuz.CurrentTime <= TimeSpan.FromSeconds(5))
                            {
                                cozinhandoCuscuz.CurrentTime = TimeSpan.FromSeconds(4);
                                somCozinhandoCuscuz.Play();
                            }

                            else if (panelaNoFogo && preparando == 1 && fritando.CurrentTime >= TimeSpan.FromSeconds(0) && fritando.CurrentTime <= TimeSpan.FromSeconds(1))
                            {
                                fritando.CurrentTime = TimeSpan.FromSeconds(0);
                                somFritando.Play();
                            }
                            else if (panelaNoFogo && preparando == 1 && fritando.CurrentTime >= TimeSpan.FromSeconds(1) && fritando.CurrentTime <= TimeSpan.FromSeconds(2))
                            {
                                fritando.CurrentTime = TimeSpan.FromSeconds(1);
                                somFritando.Play();
                            }
                            else if (panelaNoFogo && preparando == 1 && fritando.CurrentTime >= TimeSpan.FromSeconds(2) && fritando.CurrentTime <= TimeSpan.FromSeconds(3))
                            {
                                fritando.CurrentTime = TimeSpan.FromSeconds(2);
                                somFritando.Play();
                            }
                            else if (panelaNoFogo && preparando == 1 && fritando.CurrentTime >= TimeSpan.FromSeconds(3) && fritando.CurrentTime <= TimeSpan.FromSeconds(4))
                            {
                                fritando.CurrentTime = TimeSpan.FromSeconds(3);
                                somFritando.Play();
                            }
                            else if (panelaNoFogo && preparando == 1 && fritando.CurrentTime >= TimeSpan.FromSeconds(4) && fritando.CurrentTime <= TimeSpan.FromSeconds(5))
                            {
                                fritando.CurrentTime = TimeSpan.FromSeconds(4);
                                somFritando.Play();
                            }

                            else if (panelaNoFogo && preparando == 2 && cozinhandoCarne.CurrentTime >= TimeSpan.FromSeconds(0) && cozinhandoCarne.CurrentTime <= TimeSpan.FromSeconds(1))
                            {
                                cozinhandoCarne.CurrentTime = TimeSpan.FromSeconds(0);
                                somCozinhandoCarne.Play();
                            }
                            else if (panelaNoFogo && preparando == 2 && cozinhandoCarne.CurrentTime >= TimeSpan.FromSeconds(1) && cozinhandoCarne.CurrentTime <= TimeSpan.FromSeconds(2))
                            {
                                cozinhandoCarne.CurrentTime = TimeSpan.FromSeconds(1);
                                somCozinhandoCarne.Play();
                            }
                            else if (panelaNoFogo && preparando == 2 && cozinhandoCarne.CurrentTime >= TimeSpan.FromSeconds(2) && cozinhandoCarne.CurrentTime <= TimeSpan.FromSeconds(3))
                            {
                                cozinhandoCarne.CurrentTime = TimeSpan.FromSeconds(2);
                                somCozinhandoCarne.Play();
                            }
                            else if (panelaNoFogo && preparando == 2 && cozinhandoCarne.CurrentTime >= TimeSpan.FromSeconds(3) && cozinhandoCarne.CurrentTime <= TimeSpan.FromSeconds(4))
                            {
                                cozinhandoCarne.CurrentTime = TimeSpan.FromSeconds(3);
                                somCozinhandoCarne.Play();
                            }
                            else if (panelaNoFogo && preparando == 2 && cozinhandoCarne.CurrentTime >= TimeSpan.FromSeconds(4) && cozinhandoCarne.CurrentTime <= TimeSpan.FromSeconds(5))
                            {
                                cozinhandoCarne.CurrentTime = TimeSpan.FromSeconds(4);
                                somCozinhandoCarne.Play();
                            }

                            else if (panelaNoFogo && preparando == 3 && fritandoCalabresa.CurrentTime >= TimeSpan.FromSeconds(0) && fritandoCalabresa.CurrentTime <= TimeSpan.FromSeconds(1))
                            {
                                fritandoCalabresa.CurrentTime = TimeSpan.FromSeconds(0);
                                somFritandoCalabresa.Play();
                            }
                            else if (panelaNoFogo && preparando == 3 && fritandoCalabresa.CurrentTime >= TimeSpan.FromSeconds(1) && fritandoCalabresa.CurrentTime <= TimeSpan.FromSeconds(2))
                            {
                                fritandoCalabresa.CurrentTime = TimeSpan.FromSeconds(1);
                                somFritandoCalabresa.Play();
                            }
                            else if (panelaNoFogo && preparando == 3 && fritandoCalabresa.CurrentTime >= TimeSpan.FromSeconds(2) && fritandoCalabresa.CurrentTime <= TimeSpan.FromSeconds(3))
                            {
                                fritandoCalabresa.CurrentTime = TimeSpan.FromSeconds(2);
                                somFritandoCalabresa.Play();
                            }
                            else if (panelaNoFogo && preparando == 3 && fritandoCalabresa.CurrentTime >= TimeSpan.FromSeconds(3) && fritandoCalabresa.CurrentTime <= TimeSpan.FromSeconds(4))
                            {
                                fritandoCalabresa.CurrentTime = TimeSpan.FromSeconds(3);
                                somFritandoCalabresa.Play();
                            }
                            else if (panelaNoFogo && preparando == 3 && fritandoCalabresa.CurrentTime >= TimeSpan.FromSeconds(4) && fritandoCalabresa.CurrentTime <= TimeSpan.FromSeconds(5))
                            {
                                fritandoCalabresa.CurrentTime = TimeSpan.FromSeconds(4);
                                somFritandoCalabresa.Play();
                            }



                            if (panelaNoFogo && efeitoTimer.CurrentTime >= TimeSpan.FromSeconds(0) && efeitoTimer.CurrentTime <= TimeSpan.FromSeconds(1))
                            {
                                efeitoTimer.CurrentTime = TimeSpan.FromSeconds(0);
                                somEfeitoTimer.Play();
                            }
                            else if (panelaNoFogo && efeitoTimer.CurrentTime >= TimeSpan.FromSeconds(1) && efeitoTimer.CurrentTime <= TimeSpan.FromSeconds(2))
                            {
                                efeitoTimer.CurrentTime = TimeSpan.FromSeconds(1);
                                somEfeitoTimer.Play();
                            }
                            else if (panelaNoFogo && efeitoTimer.CurrentTime >= TimeSpan.FromSeconds(2) && efeitoTimer.CurrentTime <= TimeSpan.FromSeconds(3))
                            {
                                efeitoTimer.CurrentTime = TimeSpan.FromSeconds(2);
                                somEfeitoTimer.Play();
                            }
                            else if (panelaNoFogo && efeitoTimer.CurrentTime >= TimeSpan.FromSeconds(3) && efeitoTimer.CurrentTime <= TimeSpan.FromSeconds(4))
                            {
                                efeitoTimer.CurrentTime = TimeSpan.FromSeconds(3);
                                somEfeitoTimer.Play();
                            }
                            else if (panelaNoFogo && efeitoTimer.CurrentTime >= TimeSpan.FromSeconds(4) && efeitoTimer.CurrentTime <= TimeSpan.FromSeconds(5))
                            {
                                efeitoTimer.CurrentTime = TimeSpan.FromSeconds(4);
                                somEfeitoTimer.Play();
                            }
                            else if (!panelaNoFogo && efeitoTimer.CurrentTime >= TimeSpan.FromSeconds(5) && efeitoTimer.CurrentTime <= TimeSpan.FromSeconds(5.5))
                            {
                                efeitoTimer.CurrentTime = TimeSpan.FromSeconds(5);
                                somEfeitoTimer.Play();
                            }
                            else if (!panelaNoFogo && efeitoTimer.CurrentTime >= TimeSpan.FromSeconds(5.5) && efeitoTimer.CurrentTime <= TimeSpan.FromSeconds(6))
                            {
                                efeitoTimer.CurrentTime = TimeSpan.FromSeconds(5.5);
                                somEfeitoTimer.Play();
                            }
                            else if (!panelaNoFogo && efeitoTimer.CurrentTime >= TimeSpan.FromSeconds(6) && efeitoTimer.CurrentTime <= TimeSpan.FromSeconds(6.5))
                            {
                                efeitoTimer.CurrentTime = TimeSpan.FromSeconds(6);
                                somEfeitoTimer.Play();
                            }
                            else if (!panelaNoFogo && efeitoTimer.CurrentTime >= TimeSpan.FromSeconds(6.5) && efeitoTimer.CurrentTime <= TimeSpan.FromSeconds(7))
                            {
                                efeitoTimer.CurrentTime = TimeSpan.FromSeconds(6.5);
                                somEfeitoTimer.Play();
                            }
                            else if (!panelaNoFogo && efeitoTimer.CurrentTime >= TimeSpan.FromSeconds(7) && efeitoTimer.CurrentTime <= TimeSpan.FromSeconds(7.5))
                            {
                                efeitoTimer.CurrentTime = TimeSpan.FromSeconds(7);
                                somEfeitoTimer.Play();
                            }

                            break;
                        }

                }
            }

        }
        #endregion

        #region Timer Secundários

        #region Timer Volume
        private void TimerVolume(object sender, EventArgs e)
        {
            musicaFundo1.Volume = volumeSlider1.Volume;
            musicaFundo2.Volume = volumeSlider1.Volume;

            erroPedido.Volume = volumeSlider2.Volume;
            acertoPedido.Volume = volumeSlider2.Volume;
            cozinhandoCarne.Volume = volumeSlider2.Volume;
            cozinhandoCuscuz.Volume = volumeSlider2.Volume;
            fritando.Volume = volumeSlider2.Volume;
            fritandoCalabresa.Volume = volumeSlider2.Volume;
            fritandoCarne.Volume = volumeSlider2.Volume;
            efeitoSonoroDerrota.Volume = volumeSlider2.Volume;
            vitoriaSOM.Volume = volumeSlider2.Volume;

            if (volumeSlider2.Volume > 0.6f)
            {
                efeitoTimer.Volume = volumeSlider2.Volume - 0.3f;
            }
            else if (volumeSlider2.Volume > 0.3f && volumeSlider2.Volume <= 0.6f)
            {
                efeitoTimer.Volume = volumeSlider2.Volume - 0.2f;
            }
            else if (volumeSlider2.Volume > 0.1 && volumeSlider2.Volume <= 0.3)
            {
                efeitoTimer.Volume = volumeSlider2.Volume - 0.1f;
            }
            else if (volumeSlider2.Volume <= 0.1)
            {
                efeitoTimer.Volume = 0f;
            }
        }
        #endregion

        #region Timer Tempo
        private void TimerTempo(object sender, EventArgs e)
        {
            intervalotempo++;

            if (intervalotempo == 1)
            {
                intervalotempo = 0;
                tempo--;
            }
        }
        #endregion

        #region Timer De Músicas
        private void TimerParaMusicas(object sender, EventArgs e)
        {
            if (musicaDerrota)
            {
                tem++;

                if (tem == 2)
                {
                    somEfeitoSonoroDerrota.Stop();
                    saidaMusicaFundo1.Play();
                    tem = 0;
                    musicaDerrota = false;
                }
            }
            if (!jogando && !cursorVisivel && axWindowsMediaPlayer1.Visible == false)
            {
                Cursor.Show();
                cursorVisivel = true;
            }

        }
        #endregion

        #region Timer Pedidos de Clientes
        private void TimerPedidos(object sender, EventArgs e)
        {
            tempoPedido++;

            if (tempoPedido == 10 && quantidadeDePedidos < 4)
            {
                tempoPedido = 0;
                int pedido;

                do
                {
                    sorteioPossibilidade = random.Next(0, 7);
                }
                while (possiblidadesDePedidos[sorteioPossibilidade] == true && quantidadeDePedidos < 4);

                do
                {
                    sorteioPedido = random.Next(0, 4);
                }
                while (sortearCorretamente[sorteioPedido] == true && quantidadeDePedidos < 4);

                sortearCorretamente[sorteioPedido] = true;
                possiblidadesDePedidos[sorteioPossibilidade] = true;

                pedido = sorteioPossibilidade;
                pbs[sorteioPedido].Visible = true;

                ImagemParaPedidoPosSorteio(pedido, pbs[sorteioPedido]);

                quantidadeDePedidos++;


            }

            if (quantidadeDePedidos == 4)
            {
                timer3.Stop();
            }
            
        }


        private void ImagemParaPedidoPosSorteio(int x, PictureBox pb)
        {
            switch (x)
            {
                case 0:
                    {
                        pb.BackgroundImage = cucuzcarne;
                        necessitaCUSCUZ_CARNE = true;
                        break;
                    }
                case 1:
                    {
                        pb.BackgroundImage = cuzcuzovo;
                        necessitaCUSCUZ_OVO = true;
                        break;
                    }
                case 2:
                    {
                        pb.BackgroundImage = cuzcuzcalabresa;
                        necessitaCUSCUZ_CALABRESA = true;
                        break;
                    }
                case 3:
                    {
                        pb.BackgroundImage = cucuzcarnecalabresa;
                        necessitaCUSCUZ_CARNE_CALABRESA = true;
                        break;
                    }
                case 4:
                    {
                        pb.BackgroundImage = SPRITECUSCUZCOMOVOCARNE;
                        necessitaCUSCUZ_OVO_CARNE = true;
                        break;
                    }
                case 5:
                    {
                        pb.BackgroundImage = SPRITECUSCUZCOMOVOCALABRESA;
                        necessitaCUSCUZ_OVO_CALABRESA = true;
                        break;
                    }
                case 6:
                    {
                        pb.BackgroundImage = cucuzcompleto;
                        necessitaCOMPLETO = true;
                        break;
                    }
            }
        }

        private void LimparPedidosProntos()
        {
            pedidoProntoCUSCUZ_OVO = false;
            pedidoProntoCUSCUZ_CARNE = false;
            pedidoProntoCUSCUZ_CARNE_CALABRESA = false;
            pedidoProntoCOMPLETO = false;
            pedidoPronto_CUSCUZ_CALABRESA = false;
            pedidoPronto_CUSCUZ_OVO_CALABRESA = false;
            pedidoPronto_CUSCUZ_OVO_CARNE = false;

            ovoColocadoNaBancada = false;
            calabresaColocadoNaBancada = false;
            carneColocadoNaBancada = false;
            cuscuzColocadoNaBancada = false;

            pegouPedido = false;
            PBingrediente.Visible = false;
            PBingrediente.BackgroundImage = null;

            panela4.BackgroundImage = null;
        }
        #endregion

        #region Timer Fogão
        private void TimerFogao(object sender, EventArgs e)
        {
            tempoPreparo++;
            progressBar1.Value++;

            if (tempoPreparo == 5 && preparando == 0)
            {
                tempoPreparo = 0;
                timer2.Stop();
                CUSCUZPRONTO = true;
            }

            else if (tempoPreparo == 5 && preparando == 1)
            {
                tempoPreparo = 0;
                timer2.Stop();
                OVOPRONTO = true;
            }
            else if (tempoPreparo == 5 && preparando == 2)
            {
                tempoPreparo = 0;
                timer2.Stop();
                CARNEPRONTA = true;
            }
            else if (tempoPreparo == 5 && preparando == 3)
            {
                tempoPreparo = 0;
                timer2.Stop();
                CALABRESAPRONTA = true;
            }
        }
        #endregion

        #endregion

        #region Timer Principal
        private void EventoTimer(object sender, EventArgs e)
        {
            // Verificado = ÓTIMO

            //------------------------------------------------------------//
            //------------Intersecção com pictureBoxes-------------//
            //------------------------------------------------------------//            

            foreach (Control x in this.Controls)
            {
                if (x is PictureBox && (string)x.Tag == "panelas")
                {
                    if (playerX <= x.Bounds.Right && playerX >= (x.Bounds.Right - 10) && ((playerY + alturaPlayer) >= x.Bounds.Top && playerY + 50 <= x.Bounds.Bottom))
                    {
                        playerX = x.Bounds.Right + 1;
                    }

                    if (playerX + larguraPlayer >= x.Bounds.Left && playerX + larguraPlayer <= (x.Bounds.Left + 10) && (playerY + alturaPlayer >= x.Bounds.Top && playerY + 50 <= x.Bounds.Bottom))
                    {
                        playerX = x.Bounds.Left - larguraPlayer - 1;
                    }

                    if ((playerY + alturaPlayer >= x.Bounds.Top && playerY + alturaPlayer <= x.Bounds.Top + 10) && (playerX + larguraPlayer >= x.Bounds.Left && playerX <= x.Bounds.Right))
                    {
                        playerY = x.Bounds.Top - alturaPlayer - 1;
                    }

                    if ((playerY + 50 <= x.Bounds.Bottom && playerY  + 50 >= x.Bounds.Bottom - 10) && (playerX + larguraPlayer >= x.Bounds.Left && playerX <= x.Bounds.Right))
                    {
                        playerY = x.Bounds.Bottom - 50;
                    }
                }
            }



            // Verificado = ÓTIMO

            //------------------------------------------------------------//
            //------------Animação-------------//
            //------------------------------------------------------------//



            if (cima)
            {
                playerY -= velocidadePlayer;
                AnimatePlayer(0, 3);
            }
            else if (baixo)
            {
                playerY += velocidadePlayer;
                AnimatePlayer(4, 7);
            }
            else if (esquerda)
            {
                playerX -= velocidadePlayer;
                AnimatePlayer(12, 15);
            }
            else if (direita)
            {
                playerX += velocidadePlayer;
                AnimatePlayer(8, 11);
            }
            else
            {
                AnimatePlayer(16, 19);
            }

            this.Invalidate();


            // Verificado = ÓTIMO

            //------------------------------------------------------------//
            //------------FIM DE TEMPO-------------//
            //------------------------------------------------------------//

            Fase1();
            label1.Text = $"{tempo}";

            if (tempo <= 0)
            {
                VoltarAoMenu();

                saidaMusicaFundo2.Pause();
                somEfeitoSonoroDerrota.Play();
                musicaDerrota = true;

                if (!cursorVisivel)
                {
                    Cursor.Show();
                    cursorVisivel = true;
                }
            }

            //------------------------------------------------------------//
            //------------Vitória-------------//
            //------------------------------------------------------------//

            if (PedidosParaVitoria == 0)
            {
                if (!cursorVisivel)
                {
                    Cursor.Show();
                    cursorVisivel = true;
                }

                saidaMusicaFundo2.Stop();
                saidaMusicaFundo1.Pause();
                vitoria.Play();

                foreach (Control control in this.Controls)
                {
                    if (control is PictureBox)
                    {
                        control.Visible = false;
                    }
                }

                pictureBox29.BackColor = Color.Transparent;
                OpcoesButton.BackColor = Color.Transparent;
                pictureBox31.BackColor = Color.Transparent;

                jogando = false;

                pictureBox32.Visible = true;
                label1.Visible = false;

                this.BackgroundImage = System.Drawing.Image.FromFile("VitoriaGame.png");

                playerX = 2000;
                playerY = 2000;
            }



        }
        #endregion

        #region Método Fase 1

        private void MetodoVerificarLixeira_ReceitasProntas_ReceitasNaoProntas()
        {

            cuscuzColocadoNaBancada = false; ovoColocadoNaBancada = false; calabresaColocadoNaBancada = false; carneColocadoNaBancada = false;

        }

        private void LoopButtonE()
        {
            tempoButtonE++;
            if (tempoButtonE == 10 && !buttonE)
            {
                tempoButtonE = 0;
                buttonE = !buttonE;
                E.Size = new Size(40, 40);
                E.Location = new Point(470, 56);
            }
            else if (tempoButtonE == 10 && buttonE)
            {
                tempoButtonE = 0;
                buttonE = !buttonE;
                E.Size = new Size(30, 30);
                E.Location = new Point(475, 61);
            }
        }
        private void Fase1()
        {
            jogador = new Rectangle(playerX, playerY + 30, larguraPlayer, alturaPlayer);
            PBingrediente.Location = new Point(playerX + 7, playerY - 52);

            #region Intersecção com lixeira
            //------------------------------------------------------------//
            //--------Lixeira---------//
            //------------------------------------------------------------//

            //Verifica somente se o jogador pegou um ingrediente errado seja ele cru ou não
            if (jogador.IntersectsWith(PBlixaeira.Bounds) && (cuscuz == 1 || carne == 1 || calabresa == 1 || ovos == 1 || ovosfrito == 1 || carnePronta == 1 || calabresaPronta == 1 || cuscuzPronto == 1))
            {
                cuscuz = 0;
                carne = 0;
                calabresa = 0;
                ovos = 0;
                ovosfrito = 0; OVOPRONTO = false;
                cuscuzPronto = 0; CUSCUZPRONTO = false;
                calabresaPronta = 0; CALABRESAPRONTA = false;
                carnePronta = 0; CARNEPRONTA = false;
                preparando = 0; 

                PBingrediente.Visible = false;
                PBingrediente.BackgroundImage = null;

                tempo -= 3;
            }
            //Verifica se a receita que ele pegou foi a receita não estando pronta
            else if (jogador.IntersectsWith(PBlixaeira.Bounds) && 
                ((PBingrediente.BackgroundImage == SPRITECUSCUZCOMOVOCARNE || PBingrediente.BackgroundImage == SPRITECUSCUZCOMOVOCALABRESA || PBingrediente.BackgroundImage == cuzcuzovo || PBingrediente.BackgroundImage == cuzcuzcalabresa || PBingrediente.BackgroundImage == cucuzcompleto || PBingrediente.BackgroundImage == cucuzcarnecalabresa || PBingrediente.BackgroundImage == cucuzcarne) || 
                (PBingrediente.BackgroundImage == receitasNAO_PRONTAS[0] || PBingrediente.BackgroundImage == receitasNAO_PRONTAS[1] || PBingrediente.BackgroundImage == receitasNAO_PRONTAS[2] || PBingrediente.BackgroundImage == receitasNAO_PRONTAS[3] || PBingrediente.BackgroundImage == receitasNAO_PRONTAS[4] || PBingrediente.BackgroundImage == receitasNAO_PRONTAS[5] || PBingrediente.BackgroundImage == receitasNAO_PRONTAS[6] || PBingrediente.BackgroundImage == receitasNAO_PRONTAS[7])))
            {
                MetodoVerificarLixeira_ReceitasProntas_ReceitasNaoProntas();

                CARNEPRONTA = false;
                CUSCUZPRONTO = false;
                OVOPRONTO = false;
                CALABRESAPRONTA = false;


                PBingrediente.BackgroundImage = null;
                PBingrediente.Visible = false;

                panela4.BackgroundImage = null;

                pegouPedido = false;

                tempo -= 3;
                preparando = 0;
            }

            #endregion

            #region Receita Pronta

            //------------------------------------------------------------//
            //--------Receita Pronta---------//
            //------------------------------------------------------------//


            if (cuscuzColocadoNaBancada && ovoColocadoNaBancada && !calabresaColocadoNaBancada && !carneColocadoNaBancada)
            {
                panela4.BackgroundImage = cuzcuzovo;
                pedidoProntoCUSCUZ_OVO = true;

                pedidoProntoCUSCUZ_CARNE = false;
                pedidoProntoCUSCUZ_CARNE_CALABRESA = false;
                pedidoProntoCOMPLETO = false;
                pedidoPronto_CUSCUZ_CALABRESA = false;
                pedidoPronto_CUSCUZ_OVO_CALABRESA = false;
                pedidoPronto_CUSCUZ_OVO_CARNE = false;

                E.Visible = true;

                LoopButtonE();
            }
            else if (cuscuzColocadoNaBancada && !ovoColocadoNaBancada && !calabresaColocadoNaBancada && !carneColocadoNaBancada)
            {
                panela4.BackgroundImage = receitasNAO_PRONTAS[0];

                E.Visible = true;

                LoopButtonE();
            }
            else if (cuscuzColocadoNaBancada && !ovoColocadoNaBancada && !calabresaColocadoNaBancada && carneColocadoNaBancada)
            {
                panela4.BackgroundImage = cucuzcarne;
                pedidoProntoCUSCUZ_CARNE = true;

                pedidoProntoCUSCUZ_OVO = false;
                pedidoProntoCUSCUZ_CARNE_CALABRESA = false;
                pedidoProntoCOMPLETO = false;
                pedidoPronto_CUSCUZ_OVO_CARNE = false;
                pedidoPronto_CUSCUZ_OVO_CALABRESA = false;
                pedidoPronto_CUSCUZ_CALABRESA = false;

                E.Visible = true;

                LoopButtonE();
            }
            else if (cuscuzColocadoNaBancada && !ovoColocadoNaBancada && calabresaColocadoNaBancada && carneColocadoNaBancada)
            {
                panela4.BackgroundImage = cucuzcarnecalabresa;
                pedidoProntoCUSCUZ_CARNE_CALABRESA = true;

                pedidoProntoCUSCUZ_OVO = false;
                pedidoProntoCUSCUZ_CARNE = false;
                pedidoProntoCOMPLETO = false;
                pedidoPronto_CUSCUZ_CALABRESA = false;
                pedidoPronto_CUSCUZ_OVO_CALABRESA = false;
                pedidoPronto_CUSCUZ_OVO_CARNE = false;

                E.Visible = true;

                LoopButtonE();
            }
            else if (cuscuzColocadoNaBancada && ovoColocadoNaBancada && calabresaColocadoNaBancada && carneColocadoNaBancada)
            {
                panela4.BackgroundImage = cucuzcompleto;
                pedidoProntoCOMPLETO = true;

                pedidoProntoCUSCUZ_CARNE_CALABRESA = false;
                pedidoProntoCUSCUZ_CARNE = false;
                pedidoProntoCUSCUZ_OVO = false;
                pedidoPronto_CUSCUZ_CALABRESA = false;
                pedidoPronto_CUSCUZ_OVO_CALABRESA = false;
                pedidoPronto_CUSCUZ_OVO_CARNE = false;

                E.Visible = true;

                LoopButtonE();
            }
            else if (!cuscuzColocadoNaBancada && !ovoColocadoNaBancada && !calabresaColocadoNaBancada && carneColocadoNaBancada)
            {
                panela4.BackgroundImage = receitasNAO_PRONTAS[1];

                E.Visible = true;

                LoopButtonE();
            }
            else if (!cuscuzColocadoNaBancada && !ovoColocadoNaBancada && calabresaColocadoNaBancada && !carneColocadoNaBancada)
            {
                panela4.BackgroundImage = receitasNAO_PRONTAS[2];

                E.Visible = true;

                LoopButtonE();
            }
            else if (!cuscuzColocadoNaBancada && ovoColocadoNaBancada && !calabresaColocadoNaBancada && !carneColocadoNaBancada)
            {
                panela4.BackgroundImage = receitasNAO_PRONTAS[3];

                E.Visible = true;

                LoopButtonE();
            }

            else if (cuscuzColocadoNaBancada && !ovoColocadoNaBancada && calabresaColocadoNaBancada && !carneColocadoNaBancada)
            {
                panela4.BackgroundImage = cuzcuzcalabresa;

                pedidoPronto_CUSCUZ_CALABRESA = true;

                pedidoProntoCOMPLETO = false;
                pedidoProntoCUSCUZ_CARNE = false;
                pedidoProntoCUSCUZ_CARNE_CALABRESA = false;
                pedidoProntoCUSCUZ_OVO = false;
                pedidoPronto_CUSCUZ_OVO_CALABRESA = false;
                pedidoPronto_CUSCUZ_OVO_CARNE = false;

                E.Visible = true;

                LoopButtonE();
            }
            else if (cuscuzColocadoNaBancada && ovoColocadoNaBancada && calabresaColocadoNaBancada && !carneColocadoNaBancada)
            {
                panela4.BackgroundImage = SPRITECUSCUZCOMOVOCALABRESA;

                pedidoPronto_CUSCUZ_OVO_CALABRESA = true;

                pedidoProntoCOMPLETO = false;
                pedidoProntoCUSCUZ_CARNE = false;
                pedidoProntoCUSCUZ_CARNE_CALABRESA = false;
                pedidoProntoCUSCUZ_OVO = false;
                pedidoPronto_CUSCUZ_OVO_CARNE = false;
                pedidoPronto_CUSCUZ_CALABRESA = false;

                E.Visible = true;

                LoopButtonE();
            }
            else if (cuscuzColocadoNaBancada && ovoColocadoNaBancada && !calabresaColocadoNaBancada && carneColocadoNaBancada)
            {
                panela4.BackgroundImage = SPRITECUSCUZCOMOVOCARNE;

                pedidoPronto_CUSCUZ_OVO_CARNE = true;

                pedidoProntoCOMPLETO = false;
                pedidoProntoCUSCUZ_CARNE = false;
                pedidoProntoCUSCUZ_CARNE_CALABRESA = false;
                pedidoProntoCUSCUZ_OVO = false;
                pedidoPronto_CUSCUZ_CALABRESA = false;
                pedidoPronto_CUSCUZ_OVO_CALABRESA = false;

                E.Visible = true;
            }
            else if (!cuscuzColocadoNaBancada && ovoColocadoNaBancada && calabresaColocadoNaBancada && !carneColocadoNaBancada)
            {
                panela4.BackgroundImage = receitasNAO_PRONTAS[4];

                E.Visible = true;

                LoopButtonE();
            }
            else if (!cuscuzColocadoNaBancada && ovoColocadoNaBancada && !calabresaColocadoNaBancada && carneColocadoNaBancada)
            {
                panela4.BackgroundImage = receitasNAO_PRONTAS[5];

                E.Visible = true;

                LoopButtonE();
            }
            else if (!cuscuzColocadoNaBancada && ovoColocadoNaBancada && calabresaColocadoNaBancada && carneColocadoNaBancada)
            {
                panela4.BackgroundImage = receitasNAO_PRONTAS[6];

                E.Visible = true;

                LoopButtonE();
            }
            else if (!cuscuzColocadoNaBancada && !ovoColocadoNaBancada && calabresaColocadoNaBancada && carneColocadoNaBancada)
            {
                panela4.BackgroundImage = receitasNAO_PRONTAS[7];

                E.Visible = true;

                LoopButtonE();
            }
            else if (!cuscuzColocadoNaBancada && !ovoColocadoNaBancada && !calabresaColocadoNaBancada && !carneColocadoNaBancada)
            {
                E.Visible = false;
            }


            #endregion

            #region Intersecção com PB de ingredientes

            // Verificado = ÓTIMO

            //------------------------------------------------------------//
            //--------Intersecção PB de ingredientes---------//
            //------------------------------------------------------------//

            if (jogador.IntersectsWith(PBovos.Bounds) && (fogaoCheio == 0) && cuscuz == 0 && cuscuzPronto == 0 && ovos == 0 && ovosfrito == 0 && carne == 0 && carnePronta == 0 && calabresa == 0 && calabresaPronta == 0 && fogaoCheio == 0 && PBingrediente.Visible == false)
            {
                ovos = 1;
                PBingrediente.Visible = true;
                PBingrediente.BackgroundImage = ingredienteCru[0];
            }
            else if (jogador.IntersectsWith(PBcalabresa.Bounds) && (fogaoCheio == 0) && cuscuz == 0 && cuscuzPronto == 0 && ovos == 0 && ovosfrito == 0 && carne == 0 && carnePronta == 0 && calabresa == 0 && calabresaPronta == 0 && fogaoCheio == 0 && PBingrediente.Visible == false)
            {
                calabresa = 1;
                PBingrediente.Visible = true;
                PBingrediente.BackgroundImage = ingredienteCru[1];
            }
            else if (jogador.IntersectsWith(PBcarne.Bounds) && (fogaoCheio == 0) && cuscuz == 0 && cuscuzPronto == 0 && ovos == 0 && ovosfrito == 0 && carne == 0 && carnePronta == 0 && calabresa == 0 && calabresaPronta == 0 && fogaoCheio == 0 && PBingrediente.Visible == false)
            {
                carne = 1;
                PBingrediente.Visible = true;
                PBingrediente.BackgroundImage = ingredienteCru[2];
            }
            else if (jogador.IntersectsWith(PBcuscuz.Bounds) && (fogaoCheio == 0) && cuscuz == 0 && cuscuzPronto == 0 && ovos == 0 && ovosfrito == 0 && carne == 0 && carnePronta == 0 && calabresa == 0 && calabresaPronta == 0 && fogaoCheio == 0 && PBingrediente.Visible == false)
            {
                cuscuz = 1;
                PBingrediente.Visible = true;
                PBingrediente.BackgroundImage = ingredienteCru[3];
            }

            #endregion

            #region Intersecção com fogão e colocar ingredientes no fogo

            //--------------------------------------------------------------------//
            //------------Intersecção com fogão---------------//
            //--------------------------------------------------------------------//

            if (jogador.IntersectsWith(PBfogao.Bounds) && cuscuz == 1 && fogaoCheio == 0)
            {
                InterseccaoComFogao(0);
                cuscuz = 0;
                panela10.BackgroundImage = System.Drawing.Image.FromFile("cuzcuzeira.png");
                PBingrediente.Visible = false;

                somEfeitoTimer.Play();
                somCozinhandoCuscuz.Play();

                panelaNoFogo = true;
            }
            else if (jogador.IntersectsWith(PBfogao.Bounds) && ovos == 1 && fogaoCheio == 0)
            {
                InterseccaoComFogao(1);
                ovos = 0;
                panela10.BackgroundImage = System.Drawing.Image.FromFile("frigideira.png");
                PBingrediente.Visible = false;

                somEfeitoTimer.Play();
                somFritando.Play();

                panelaNoFogo = true;
            }
            else if (jogador.IntersectsWith(PBfogao.Bounds) && carne == 1 && fogaoCheio == 0)
            {
                InterseccaoComFogao(2);
                carne = 0;
                panela10.BackgroundImage = System.Drawing.Image.FromFile("panela.png");
                PBingrediente.Visible = false;

                somEfeitoTimer.Play();
                somCozinhandoCarne.Play();

                panelaNoFogo = true;
            }
            else if (jogador.IntersectsWith(PBfogao.Bounds) && calabresa == 1 && fogaoCheio == 0)
            {
                InterseccaoComFogao(3);
                calabresa = 0;
                panela10.BackgroundImage = System.Drawing.Image.FromFile("frigideira.png");
                PBingrediente.Visible = false;

                somEfeitoTimer.Play();
                somFritandoCalabresa.Play();

                panelaNoFogo = true;
            }

            #endregion

            #region Intersecção com fogão RETIRADA

            // Verificado = ÓTIMO
            //------------Intersecção com fogão RETIRADA---------------//
            //                                                                                        XXX Cuscuz Pronto XXX
            if (jogador.IntersectsWith(PBfogao.Bounds) && fogaoCheio == 1 && progressBar1.Value == 5 && (CUSCUZPRONTO) && (cuscuz == 0 && cuscuzPronto == 0 && ovos == 0 && ovosfrito == 0 && carne == 0 && carnePronta == 0 && calabresa == 0 && calabresaPronta == 0 && fogaoCheio == 1))
            {
                InterseccaoComFogao(4);
                cuscuzPronto = 1;

                PBingrediente.BackgroundImage = receitasNAO_PRONTAS[0];
                PBingrediente.Visible = true;

                panelaNoFogo = false;
            }
            //                                                                                        XXX Carne Pronta XXX
            if (jogador.IntersectsWith(PBfogao.Bounds) && fogaoCheio == 1 && progressBar1.Value == 5 && (CARNEPRONTA) && (cuscuz == 0 && cuscuzPronto == 0 && ovos == 0 && ovosfrito == 0 && carne == 0 && carnePronta == 0 && calabresa == 0 && calabresaPronta == 0 && fogaoCheio == 1))
            {
                InterseccaoComFogao(5);
                carnePronta = 1;

                PBingrediente.BackgroundImage = receitasNAO_PRONTAS[1];
                PBingrediente.Visible = true;

                panelaNoFogo = false;
            }
            //                                                                                        XXX Calabresa Pronta XXX
            if (jogador.IntersectsWith(PBfogao.Bounds) && fogaoCheio == 1 && progressBar1.Value == 5 && (CALABRESAPRONTA) && (cuscuz == 0 && cuscuzPronto == 0 && ovos == 0 && ovosfrito == 0 && carne == 0 && carnePronta == 0 && calabresa == 0 && calabresaPronta == 0 && fogaoCheio == 1))
            {
                InterseccaoComFogao(6);
                calabresaPronta = 1;

                PBingrediente.BackgroundImage = receitasNAO_PRONTAS[2];
                PBingrediente.Visible = true;

                panelaNoFogo = false;
            }
            //                                                                                       XXX Ovo Pronto XXX
            if (jogador.IntersectsWith(PBfogao.Bounds) && fogaoCheio == 1 && progressBar1.Value == 5 && (OVOPRONTO) && (cuscuz == 0 && cuscuzPronto == 0 && ovos == 0 && ovosfrito == 0 && carne == 0 && carnePronta == 0 && calabresa == 0 && calabresaPronta == 0 && fogaoCheio == 1))
            {
                InterseccaoComFogao(7);
                ovosfrito = 1;

                PBingrediente.BackgroundImage = receitasNAO_PRONTAS[3];
                PBingrediente.Visible = true;

                panelaNoFogo = false;
            }

            #endregion

            #region Intersecção com balcão
            //--------------------------------------------------------------------//
            //------------Intersecção com balcão---------------//
            //--------------------------------------------------------------------//

            
            if (jogador.IntersectsWith(PBbalcao.Bounds) && cuscuzPronto == 1 && !cuscuzColocadoNaBancada)
            {
                panela4.Visible = true;
                cuscuzColocadoNaBancada = true;
                cuscuzPronto = 0;
                cuscuz = 0;
                cuscuzPronto = 0;

                PBingrediente.Visible = false;

                CUSCUZPRONTO = false;
            }

            else if (jogador.IntersectsWith(PBbalcao.Bounds) && carnePronta == 1 && !carneColocadoNaBancada)
            {
                panela4.Visible = true;
                carneColocadoNaBancada = true;
                carnePronta = 0;
                cuscuz = 0;
                cuscuzPronto = 0;

                PBingrediente.Visible = false;

                CARNEPRONTA = false;
            }

            else if (jogador.IntersectsWith(PBbalcao.Bounds) && calabresaPronta == 1 && !calabresaColocadoNaBancada)
            {
                panela4.Visible = true;
                calabresaColocadoNaBancada = true;
                calabresaPronta = 0;
                cuscuz = 0;
                cuscuzPronto = 0;

                PBingrediente.Visible = false;

                CALABRESAPRONTA = false;
            }

            else if (jogador.IntersectsWith(PBbalcao.Bounds) && ovosfrito == 1 && !ovoColocadoNaBancada)
            {
                panela4.Visible = true;
                ovoColocadoNaBancada = true;
                ovosfrito = 0;
                cuscuz = 0;
                cuscuzPronto = 0;

                PBingrediente.Visible = false;

                OVOPRONTO = false;
            }

            #endregion

            #region Entrega de Pedidos

            //-------------------------------------------------//
            //---------------ENTREGA--------------//
            //-------------------------------------------------//

            if (jogador.IntersectsWith(PBentrega.Bounds))
            {

                if (pedido4.BackgroundImage == PBingrediente.BackgroundImage && PBingrediente.Visible == true && ((necessitaCOMPLETO && pedidoProntoCOMPLETO) || (necessitaCUSCUZ_CALABRESA && pedidoPronto_CUSCUZ_CALABRESA) || (necessitaCUSCUZ_CARNE && pedidoProntoCUSCUZ_CARNE) || (necessitaCUSCUZ_CARNE_CALABRESA && pedidoProntoCUSCUZ_CARNE_CALABRESA) || (necessitaCUSCUZ_OVO && pedidoProntoCUSCUZ_OVO) || (necessitaCUSCUZ_OVO_CALABRESA && pedidoPronto_CUSCUZ_OVO_CALABRESA) || (necessitaCUSCUZ_OVO_CARNE && pedidoPronto_CUSCUZ_OVO_CARNE)))
                {
                    interseccao = true;
                    pedido4.Visible = false;
                    PedidosParaVitoria--;

                    somAcertoPedido.Play();

                    LimparPedidosProntos();

                    tempo += 5;
                }
                else if (pedido3.BackgroundImage == PBingrediente.BackgroundImage && PBingrediente.Visible == true && ((necessitaCOMPLETO && pedidoProntoCOMPLETO) || (necessitaCUSCUZ_CALABRESA && pedidoPronto_CUSCUZ_CALABRESA) || (necessitaCUSCUZ_CARNE && pedidoProntoCUSCUZ_CARNE) || (necessitaCUSCUZ_CARNE_CALABRESA && pedidoProntoCUSCUZ_CARNE_CALABRESA) || (necessitaCUSCUZ_OVO && pedidoProntoCUSCUZ_OVO) || (necessitaCUSCUZ_OVO_CALABRESA && pedidoPronto_CUSCUZ_OVO_CALABRESA) || (necessitaCUSCUZ_OVO_CARNE && pedidoPronto_CUSCUZ_OVO_CARNE)))
                {
                    interseccao = true;
                    pedido3.Visible = false;
                    PedidosParaVitoria--;

                    somAcertoPedido.Play();

                    LimparPedidosProntos();

                    tempo += 5;
                }
                else if (pedido2.BackgroundImage == PBingrediente.BackgroundImage && PBingrediente.Visible == true && ((necessitaCOMPLETO && pedidoProntoCOMPLETO) || (necessitaCUSCUZ_CALABRESA && pedidoPronto_CUSCUZ_CALABRESA) || (necessitaCUSCUZ_CARNE && pedidoProntoCUSCUZ_CARNE) || (necessitaCUSCUZ_CARNE_CALABRESA && pedidoProntoCUSCUZ_CARNE_CALABRESA) || (necessitaCUSCUZ_OVO && pedidoProntoCUSCUZ_OVO) || (necessitaCUSCUZ_OVO_CALABRESA && pedidoPronto_CUSCUZ_OVO_CALABRESA) || (necessitaCUSCUZ_OVO_CARNE && pedidoPronto_CUSCUZ_OVO_CARNE)))
                {
                    interseccao = true;
                    pedido2.Visible = false;
                    PedidosParaVitoria--;

                    somAcertoPedido.Play();

                    LimparPedidosProntos();

                    tempo += 5;
                }
                else if (pedido1.BackgroundImage == PBingrediente.BackgroundImage && PBingrediente.Visible == true && ((necessitaCOMPLETO && pedidoProntoCOMPLETO) || (necessitaCUSCUZ_CALABRESA && pedidoPronto_CUSCUZ_CALABRESA) || (necessitaCUSCUZ_CARNE && pedidoProntoCUSCUZ_CARNE) || (necessitaCUSCUZ_CARNE_CALABRESA && pedidoProntoCUSCUZ_CARNE_CALABRESA) || (necessitaCUSCUZ_OVO && pedidoProntoCUSCUZ_OVO) || (necessitaCUSCUZ_OVO_CALABRESA && pedidoPronto_CUSCUZ_OVO_CALABRESA) || (necessitaCUSCUZ_OVO_CARNE && pedidoPronto_CUSCUZ_OVO_CARNE)))
                {
                    interseccao = true;
                    pedido1.Visible = false;
                    PedidosParaVitoria--;

                    somAcertoPedido.Play();

                    LimparPedidosProntos();

                    tempo += 5;
                }
                else if (!interseccao)
                {
                    tempoerro++;

                    if (tempoerro == 1)
                    {
                        tempo -= 7;

                        somErroPedido.Play();
                    }
                }
                
            }
            else
            {
                tempoerro = 0;
                interseccao = false;
            }

            #endregion
        }
        #endregion

        #region Método Intersecção com Fogão

        public void InterseccaoComFogao(int i)
        {
            if (i == 0)
            {
                timer2.Start();

                panela10.Visible = true;
                progressBar1.Visible = true;
                progressBar1.Value = 0;
                fogaoCheio = 1;

                preparando = 0;
            }
            if (i == 1)
            {
                timer2.Start();

                panela10.Visible = true;
                progressBar1.Visible = true;
                progressBar1.Value = 0;
                fogaoCheio = 1;

                preparando = 1;
            }
            if (i == 2)
            {
                timer2.Start();

                panela10.Visible = true;
                progressBar1.Visible = true;
                progressBar1.Value = 0;
                fogaoCheio = 1;

                preparando = 2;
            }
            if (i == 3)
            {
                timer2.Start();

                panela10.Visible = true;
                progressBar1.Visible = true;
                progressBar1.Value = 0;
                fogaoCheio = 1;

                preparando = 3;
            }

            //--------------------------------------------------------------------//
            //------------Retirar comida do fogão---------------//
            //--------------------------------------------------------------------//

            if (i == 4)
            {
                fogaoCheio = 0;
                panela10.Visible = false;
                progressBar1.Visible = false;
            }

            if (i == 5)
            {
                fogaoCheio = 0;
                panela10.Visible = false;
                progressBar1.Visible = false;
            }

            if (i == 6)
            {
                fogaoCheio = 0;
                panela10.Visible = false;
                progressBar1.Visible = false;
            }
            if (i == 7)
            {
                fogaoCheio = 0;
                panela10.Visible = false;
                progressBar1.Visible = false;
            }

        }


        #endregion

    }    
    
}
