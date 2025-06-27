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
            LoadDadosEmit(doc);
            LoadDadosTransporte(doc);
            LoadDadosProdutos(doc);

        }

        private void LoadDadosTransporte(XmlDocument doc)
        {
            XmlNodeList? transp = doc.GetElementsByTagName("transp");
            foreach (XmlNode node in transp)
            {
                textBox36.Text = node["modFrete"]?.InnerText;
                textBox37.Text = node["transporta"]?["CNPJ"]?.InnerText;
                textBox38.Text = node["transporta"]?["xNome"]?.InnerText;
                textBox39.Text = node["transporta"]?["IE"]?.InnerText;
                textBox40.Text = node["transporta"]?["xEnder"]?.InnerText;
                textBox41.Text = node["transporta"]?["xMun"]?.InnerText;
                textBox42.Text = node["transporta"]?["UF"]?.InnerText;
                textBox43.Text = node["vol"]?["qVol"]?.InnerText;
                textBox44.Text = node["vol"]?["esp"]?.InnerText;
                textBox45.Text = node["vol"]?["marca"]?.InnerText;
                textBox46.Text = node["vol"]?["pesoL"]?.InnerText;
                textBox47.Text = node["vol"]?["pesoB"]?.InnerText;
            }
        }

        private void LoadDadosEmit(XmlDocument doc)
        {
            XmlNodeList? emit = doc.GetElementsByTagName("emit");
            foreach (XmlNode emitNode in emit)
            {
                textBox22.Text = emitNode["CNPJ"]?.InnerText;
                textBox23.Text = emitNode["xNome"]?.InnerText;
                textBox24.Text = emitNode["xFant"]?.InnerText;
                textBox25.Text = emitNode["enderEmit"]?["xLgr"]?.InnerText;
                textBox26.Text = emitNode["enderEmit"]?["nro"]?.InnerText;
                textBox27.Text = emitNode["enderEmit"]?["xBairro"]?.InnerText;
                textBox28.Text = emitNode["enderEmit"]?["xMun"]?.InnerText;
                textBox29.Text = emitNode["enderEmit"]?["UF"]?.InnerText;
                textBox30.Text = emitNode["enderEmit"]?["CEP"]?.InnerText;
                textBox31.Text = emitNode["enderEmit"]?["xPais"]?.InnerText;
                textBox32.Text = emitNode["enderEmit"]?["fone"]?.InnerText;
                textBox33.Text = emitNode["IE"]?.InnerText;
                textBox34.Text = emitNode["IEST"]?.InnerText;
                textBox35.Text = emitNode["CRT"]?.InnerText;
            }
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
                textBox12.Text = node["tpImp"]?.InnerText;
                textBox13.Text = node["tpEmis"]?.InnerText;
                textBox14.Text = node["cDV"]?.InnerText;
                textBox15.Text = node["tpAmb"]?.InnerText;
                textBox16.Text = node["finNFe"]?.InnerText;
                textBox17.Text = node["indFinal"]?.InnerText;
                textBox18.Text = node["indPres"]?.InnerText;
                textBox19.Text = node["indIntermed"]?.InnerText;
                textBox20.Text = node["procEmi"]?.InnerText;
                textBox21.Text = node["verProc"]?.InnerText;
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

            //Imposto IPI
            tabela.Columns.Add("CST.IPI");
            tabela.Columns.Add("BC.IPI");
            tabela.Columns.Add("%.IPI");
            tabela.Columns.Add("Vl.IPI");

            //Imposto PIS
            tabela.Columns.Add("CST.PIS");
            tabela.Columns.Add("BC.PIS");
            tabela.Columns.Add("%.PIS");
            tabela.Columns.Add("Vl.PIS");

            //Imposto COFINS
            tabela.Columns.Add("CST.COFINS");
            tabela.Columns.Add("BC.COFINS");
            tabela.Columns.Add("%.COFINS");
            tabela.Columns.Add("Vl.COFINS");

            //Imposto ICMS
            tabela.Columns.Add("CST");
            tabela.Columns.Add("Mod.BC");
            tabela.Columns.Add("Vl.BC");
            tabela.Columns.Add("%.ICMS");
            tabela.Columns.Add("Vl.ICMS");
            tabela.Columns.Add("Mod.BCST");
            tabela.Columns.Add("%.MVA");
            tabela.Columns.Add("Base de Calc.ST");
            tabela.Columns.Add("%.ICMS ST");
            tabela.Columns.Add("Vl.ICMS ST");

            tabela.Columns.Add("Inf.Adicionais do Produto");

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

                    //imposto icms
                    dados["CST"] = node["imposto"]?["ICMS"]?.ChildNodes?.Item(0)?["CST"]?.InnerText;
                    dados["Mod.BC"] = node["imposto"]?["ICMS"]?.ChildNodes?.Item(0)?["modBC"]?.InnerText;
                    dados["Vl.BC"] = node["imposto"]?["ICMS"]?.ChildNodes?.Item(0)?["vBC"]?.InnerText;
                    dados["%.ICMS"] = node["imposto"]?["ICMS"]?.ChildNodes?.Item(0)?["pICMS"]?.InnerText;
                    dados["Vl.ICMS"] = node["imposto"]?["ICMS"]?.ChildNodes?.Item(0)?["vICMS"]?.InnerText;
                    dados["Mod.BCST"] = node["imposto"]?["ICMS"]?.ChildNodes?.Item(0)?["modBCST"]?.InnerText;
                    dados["%.MVA"] = node["imposto"]?["ICMS"]?.ChildNodes?.Item(0)?["pMVAST"]?.InnerText;
                    dados["Base de Calc.ST"] = node["imposto"]?["ICMS"]?.ChildNodes?.Item(0)?["vBCST"]?.InnerText;
                    dados["%.ICMS ST"] = node["imposto"]?["ICMS"]?.ChildNodes?.Item(0)?["pICMSST"]?.InnerText;
                    dados["Vl.ICMS ST"] = node["imposto"]?["ICMS"]?.ChildNodes?.Item(0)?["vICMSST"]?.InnerText;

                    //imposto IPI
                    dados["CST.IPI"] = node["imposto"]?["IPI"]?["IPITrib"]?["CST"]?.InnerText;
                    dados["BC.IPI"] = node["imposto"]?["IPI"]?["IPITrib"]?["vBC"]?.InnerText;
                    dados["%.IPI"] = node["imposto"]?["IPI"]?["IPITrib"]?["pIPI"]?.InnerText;
                    dados["Vl.IPI"] = node["imposto"]?["IPI"]?["IPITrib"]?["vIPI"]?.InnerText;

                    //imposto PIS
                    dados["CST.PIS"] = node["imposto"]?["PIS"]?["IPIAliq"]?["CST"]?.InnerText;
                    dados["BC.PIS"] = node["imposto"]?["PIS"]?["PISAliq"]?["vBC"]?.InnerText;
                    dados["%.PIS"] = node["imposto"]?["PIS"]?["PISAliq"]?["pPIS"]?.InnerText;
                    dados["Vl.PIS"] = node["imposto"]?["PIS"]?["PISAliq"]?["vPIS"]?.InnerText;

                    //imposto COFINS
                    dados["CST.COFINS"] = node["imposto"]?["COFINS"]?["COFINSAliq"]?["CST"]?.InnerText;
                    dados["BC.COFINS"] = node["imposto"]?["COFINS"]?["COFINSAliq"]?["vBC"]?.InnerText;
                    dados["%.COFINS"] = node["imposto"]?["COFINS"]?["COFINSAliq"]?["pCOFINS"]?.InnerText;
                    dados["Vl.COFINS"] = node["imposto"]?["COFINS"]?["COFINSAliq"]?["vCOFINS"]?.InnerText;

                    dados["Inf.Adicionais do Produto"] = node["infAdProd"]?.InnerText;

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
                if (fbd.ShowDialog() != DialogResult.Cancel)
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
