using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Collections.Specialized.BitVector32;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace notepad
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            richTextBox1.Multiline = true;
        }

        private void toolStripComboBox1_Click(object sender, EventArgs e)
        {

        }

        private void arquivoToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private async void abrirToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                var name = ofd.FileName;
                StreamReader sr = new StreamReader(ofd.FileName);
                var content = await sr.ReadToEndAsync();
                richTextBox1.Rtf = content;
                sr.Close();
            };
        }

        private async void salvarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFileDialog sfd = new SaveFileDialog();
            if (sfd.ShowDialog() == DialogResult.OK)
            {
                using (StreamWriter writer = new StreamWriter(sfd.FileName))
                {
                    await writer.WriteAsync(richTextBox1.Rtf);
                    writer.Close();
                }
            }
        }

        private void fonteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            
        }

        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {
            label1.Text = "c: " + richTextBox1.Text.Length;

            h1System();
            h2System();
        }

        private List<(int start, int end, Font font)> h1Formats = new List<(int start, int end, Font font)>();

        private void h1System()
        {
            string searchText = "<h1>";
            string endTag = "</h1>";

            string text = richTextBox1.Text; 

            int startIndex = text.IndexOf(searchText);
            int endIndex = text.IndexOf(endTag);

            while (startIndex != -1 && endIndex != -1 && endIndex > startIndex + searchText.Length)
            {
                string content = text.Substring(startIndex + searchText.Length, endIndex - startIndex - searchText.Length);

                int selectionStart = richTextBox1.SelectionStart;

                // Armazena as informações de formatação
                Font h1Font = new Font(richTextBox1.Font.FontFamily, 21, FontStyle.Bold);
                h1Formats.Add((startIndex, startIndex + content.Length, h1Font));

                richTextBox1.Text = richTextBox1.Text.Remove(startIndex, endIndex - startIndex + endTag.Length).Insert(startIndex, content);
                richTextBox1.SelectionStart = selectionStart - (endIndex - startIndex + endTag.Length) + content.Length;

                richTextBox1.Select(startIndex, content.Length);
                richTextBox1.SelectionFont = h1Font;

                richTextBox1.Select(selectionStart, 0);
                richTextBox1.SelectionFont = richTextBox1.Font;

                startIndex = text.IndexOf(searchText, startIndex + content.Length);
                endIndex = text.IndexOf(endTag, endIndex + endTag.Length);
            }

            // Aplica a formatação correta para todos os trechos estilizados
            foreach (var format in h1Formats)
            {
                richTextBox1.Select(format.start, format.end - format.start);
                richTextBox1.SelectionFont = format.font;
            }

            richTextBox1.SelectionStart = text.Length;
        }

        private List<(int start, int end, Font font)> h2Formats = new List<(int start, int end, Font font)>();

        private void h2System()
        {
            string searchText = "<h2>";
            string endTag = "</h2>";

            string text = richTextBox1.Text;

            int startIndex = text.IndexOf(searchText);
            int endIndex = text.IndexOf(endTag);

            while (startIndex != -1 && endIndex != -1 && endIndex > startIndex + searchText.Length)
            {
                string content = text.Substring(startIndex + searchText.Length, endIndex - startIndex - searchText.Length);

                int selectionStart = richTextBox1.SelectionStart;

                // Armazena as informações de formatação
                Font h2Font = new Font(richTextBox1.Font.FontFamily, 16, FontStyle.Italic);
                h2Formats.Add((startIndex, startIndex + content.Length, h2Font));

                richTextBox1.Text = richTextBox1.Text.Remove(startIndex, endIndex - startIndex + endTag.Length).Insert(startIndex, content);
                richTextBox1.SelectionStart = selectionStart - (endIndex - startIndex + endTag.Length) + content.Length;

                richTextBox1.Select(startIndex, content.Length);
                richTextBox1.SelectionFont = h2Font;

                richTextBox1.Select(selectionStart, 0);
                richTextBox1.SelectionFont = richTextBox1.Font;

                startIndex = text.IndexOf(searchText, startIndex + content.Length);
                endIndex = text.IndexOf(endTag, endIndex + endTag.Length);
            }

            // Aplica a formatação correta para todos os trechos estilizados
            foreach (var format in h2Formats)
            {
                richTextBox1.Select(format.start, format.end - format.start);
                richTextBox1.SelectionFont = format.font;
            }

            richTextBox1.SelectionStart = text.Length;
        }

        private void fonteToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            FontDialog fd = new FontDialog();
            var box = richTextBox1;
            if (fd.ShowDialog() == DialogResult.OK)
            {
                box.SelectionFont = fd.Font;
            }
        }

        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            ColorDialog cd = new ColorDialog();
            if (cd.ShowDialog() == DialogResult.OK)
            {
                richTextBox1.SelectionColor = cd.Color;
            }
        }

        private void corDeFundoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ColorDialog cd = new ColorDialog();
            if (cd.ShowDialog() == DialogResult.OK)
            {
                richTextBox1.BackColor = cd.Color;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {

        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            var selection = Clipboard.GetText();
            var start = richTextBox1.SelectionStart;

            if (richTextBox1.SelectionLength > 0)
                richTextBox1.Text = richTextBox1.Text.Remove(start, richTextBox1.SelectionLength).Insert(start, selection);
            else
                richTextBox1.Text = richTextBox1.Text.Insert(start, selection);
        }

        private async void toolStripButton2_Click(object sender, EventArgs e)
        {
            SaveFileDialog sfd = new SaveFileDialog();
            if (sfd.ShowDialog() == DialogResult.OK)
            {
                using (StreamWriter writer = new StreamWriter(sfd.FileName))
                {
                    await writer.WriteAsync(richTextBox1.Rtf);
                    writer.Close();
                }
            }
        }

        private void label1_Click(object sender, EventArgs e)
        {
            
        }
    }
}
