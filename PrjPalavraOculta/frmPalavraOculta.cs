using System;
using System.Collections.Generic;
using System.DirectoryServices.ActiveDirectory;
using System.Security.Cryptography;
using System.Text.Json;
using PrjPalavraOculta.apoio.dto;
using static PrjPalavraOculta.apoio.dto.ListaDto;

namespace PrjPalavraOculta
{
    public partial class frmPalavraOculta : Form
    {

        int tentativas = 6;
        int valorInteiro = 1;
        int letraQuantidade = 0;
        int letraCerta = 0;

        private List<ListaDto.Palavra> palavras;


        private Random random = new Random();



        public frmPalavraOculta()
        {
            InitializeComponent();

            GeraIndiceAleatorioEPreenche();
            InicioJogo();

            lblTentativas.Text = tentativas.ToString();
        }


        private void btnCancel_Click(object sender, EventArgs e)
        {
            
            GeraIndiceAleatorioEPreenche();
            InicioJogo();
        }


        private void GeraIndiceAleatorioEPreenche()
        {
            try
            {
                letraQuantidade = 0;
                letraCerta = 0;

                string json = File.ReadAllText("listaPalavras.json");

                palavras = JsonSerializer.Deserialize<List<ListaDto.Palavra>>(json);

                if (palavras == null || palavras.Count == 0)
                {
                    MessageBox.Show("Lista de palavras vazia ou não carregada.");
                    return;
                }

                Random numAleatorio = new Random();
                int indiceAleatorio = numAleatorio.Next(0, palavras.Count);

                var palavraEscolhida = palavras[indiceAleatorio];

                lblLetra1.Text = palavraEscolhida.Letra1 ?? "";
                lblLetra2.Text = palavraEscolhida.Letra2 ?? "";
                lblLetra3.Text = palavraEscolhida.Letra3 ?? "";
                lblLetra4.Text = palavraEscolhida.Letra4 ?? "";
                lblLetra5.Text = palavraEscolhida.Letra5 ?? "";
                lblLetra6.Text = palavraEscolhida.Letra6 ?? "";
                lblLetra7.Text = palavraEscolhida.Letra7 ?? "";
                lblLetra8.Text = palavraEscolhida.Letra8 ?? "";
                lblLetra9.Text = palavraEscolhida.Letra9 ?? "";
                lblLetra10.Text = palavraEscolhida.Letra10 ?? "";

                foreach (Label letra in pnlPalavra.Controls.OfType<Label>())
                {
                    if (!string.IsNullOrEmpty(letra.Text))
                    {
                        string texto = letra.Text.ToLower();

                        letraQuantidade++;

                        texto = texto.Replace("á", "a")
                                     .Replace("à", "a")
                                     .Replace("ã", "a")
                                     .Replace("â", "a")
                                     .Replace("é", "e")
                                     .Replace("ê", "e")
                                     .Replace("í", "i")
                                     .Replace("ó", "o")
                                     .Replace("ô", "o")
                                     .Replace("õ", "o")
                                     .Replace("ú", "u")
                                     .Replace("ç", "c");

                        letra.Text = texto.ToUpper();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao carregar as palavras: " + ex.Message);
            }
        }
        private void button_Click(object sender, EventArgs e)
        {
            Button btn = (Button)sender;
            bool letraEncontrada = false;

            foreach (Label lblLetras in pnlPalavra.Controls.OfType<Label>())
            {
                if (lblLetras.Text.ToUpper() == btn.Text.ToUpper())
                {
                    lblLetras.ForeColor = Color.Black;
                    letraCerta++;
                    letraEncontrada = true;
                }
            }

            if (!letraEncontrada)
            {
                tentativas--;
                lblTentativas.Text = tentativas.ToString();
            }

            btn.Enabled = false;
            btn.ForeColor = Color.WhiteSmoke;
            btn.BackColor = Color.LightGray;
            btn.FlatStyle = FlatStyle.Flat;

            VerificaVitoriaDerrota();
        }

        private void InicioJogo()
        {
            tentativas = 6;

            lblTentativas.Text = tentativas.ToString();

            foreach (Button btn in pnlTecladoDigital.Controls.OfType<Button>())
            {
                btn.Enabled = true;
                btn.ForeColor = Color.Black;
                btn.BackColor = Color.Silver;
                btn.FlatStyle = FlatStyle.Popup;
            }

            foreach (Label letra in pnlPalavra.Controls.OfType<Label>())
            {
                if (!string.IsNullOrEmpty(letra.Text))
                {
                    letra.ForeColor = Color.Silver;
                    letra.BackColor = Color.Silver;
                    letra.Text.ToUpper();
                }
                else
                {
                    letra.ForeColor = Color.Black;
                    letra.BackColor = Color.White;
                }
            }
        }

        private void VerificaVitoriaDerrota()
        {
            if (tentativas == 0)
            {
                DialogResult resultado = MessageBox.Show("Número de tentativas excedido. A operação será encerrada.", "FIM DE JOGO", MessageBoxButtons.OKCancel, MessageBoxIcon.Error);
                if (resultado == DialogResult.OK)
                {
                    GeraIndiceAleatorioEPreenche();
                    InicioJogo();
                }
                else
                {
                    this.Close();
                }
            }

            if(letraCerta == letraQuantidade)
            {
                DialogResult resultado = MessageBox.Show("Login e senha corretas. Seja Bem Vindo(a)!", "FIM DE JOGO", MessageBoxButtons.OKCancel, MessageBoxIcon.Information);
                if (resultado == DialogResult.OK)
                {
                    GeraIndiceAleatorioEPreenche();
                    InicioJogo();
                }
                else
                {
                    this.Close();
                }
            }
        }
    }
}
