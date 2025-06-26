using System.CodeDom;
using System.Data;
using System.Net.NetworkInformation;
using System.Security.Cryptography;
using System.Xml;

namespace Danfws
{
    public partial class Form1 : Form
    {
        public string Arquivo { get; set; }
        public string Diretorio { get; set; }

        XmlDocument? doc = null;
        public Form1()
        {
            InitializeComponent();
            doc = new();
        }

        private string GetDataConvertida(string data)
        {
            if (data == null || data == string.Empty)
            {
                return "";
            }
            else
            {
                DateTime date = DateTime.Parse(data);
                return date.ToString("d");
            }
        }

        private void LoadDados()
        {
            if (string.IsNullOrEmpty(Arquivo))
                return;
            doc.Load(Arquivo);

            LoadDadosIde(doc);
            LoadDadosProdutos(doc);
        }

        private void LoadDadosIde(XmlDocument doc)
        {       
            XmlNodeList? ide = doc.GetElementsByTagName("ide");

            foreach (XmlNode node in ide)
            {
                textBox1.Text = node["cUF"]?.InnerText;
                textBox2.Text = node["cNF"]?.InnerText;
                textBox3.Text = node["natOp"]?.InnerText;
                textBox4.Text = node["mod"]?.InnerText;
                textBox5.Text = node["serie"]?.InnerText;
                textBox6.Text = node["nNF"]?.InnerText;
                textBox7.Text = GetDataConvertida(node["dhEmi"]?.InnerText ?? string.Empty);
                textBox8.Text = GetDataConvertida(node["dhSaiEnt"]?.InnerText ?? string.Empty);
                textBox9.Text = node["tpNF"]?.InnerText;
                textBox10.Text = node["idDest"]?.InnerText;
                textBox11.Text = node["cMunFG"]?.InnerText;
            }
        }

        private async void LoadDadosProdutos(XmlDocument doc)
        {
            if (string.IsNullOrEmpty(Arquivo))
                return;

            doc.Load(Arquivo);

            DataTable tabela = new DataTable();

            tabela.Columns.Add("Cod.Prod");
            tabela.Columns.Add("Cod.EAN");
            tabela.Columns.Add("Descrição");
            tabela.Columns.Add("NCM");
            tabela.Columns.Add("CEST");
            tabela.Columns.Add("CFOP");
            tabela.Columns.Add("Unid");
            tabela.Columns.Add("Qtd");
            tabela.Columns.Add("Vl.Unit");
            tabela.Columns.Add("Vl.Prod");

            XmlNodeList det = doc.GetElementsByTagName("det");

            await Task.Run(() =>
            {
                foreach (XmlNode node in det)
                {
                    DataRow dados = tabela.NewRow();
                    dados["Cod.Prod"] = node["prod"]?["cProd"]?.InnerText;
                    dados["Cod.EAN"] = node["prod"]?["cEAN"]?.InnerText;
                    dados["Descrição"] = node["prod"]?["xProd"]?.InnerText;
                    dados["NCM"] = node["prod"]?["NCM"]?.InnerText;
                    dados["CEST"] = node["prod"]?["CEST"]?.InnerText;
                    dados["CFOP"] = node["prod"]?["CFOP"]?.InnerText;
                    dados["Unid"] = node["prod"]?["uCom"]?.InnerText;
                    dados["Qtd"] = node["prod"]?["qCom"]?.InnerText;
                    dados["Vl.Unit"] = node["prod"]?["vUnCom"]?.InnerText;
                    dados["Vl.Prod"] = node["prod"]?["vProd"]?.InnerText;

                    tabela.Rows.Add(dados);
                }
                this.Invoke(new Action(() =>
                {
                    dgProdutos.DataSource = tabela;
                }));
            });
        }

        private void btnLoadArquivos_Click(object sender, EventArgs e)
        {
            using (var fbd = new FolderBrowserDialog())
            {
                fbd.Description = "Selecione o diretório para listar os arquivos";
                if (fbd.ShowDialog() == DialogResult.OK && !string.IsNullOrWhiteSpace(fbd.SelectedPath))
                {
                    Diretorio = fbd.SelectedPath;
                    DirectoryInfo dinfo = new DirectoryInfo(Diretorio);

                    lbArquivos.Items.Clear();
                    foreach (FileInfo fi in dinfo.GetFiles())
                    {
                        lbArquivos.Items.Add(fi.FullName);
                    }
                }
            }
        }

        private void lbArquivos_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lbArquivos.SelectedIndex >= 0) // Ensure a valid index is selected
            {
                Arquivo = lbArquivos.Items[lbArquivos.SelectedIndex]?.ToString() ?? string.Empty; // Use null-coalescing operator to handle null values             
            }
            else
            {
                Arquivo = string.Empty; // Assign a default value if no item is selected
            }

            LoadDados();
        }
    }
}
