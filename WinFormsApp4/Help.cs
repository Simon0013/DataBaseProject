using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Net;
using System.Text;
using System.Windows.Forms;

namespace WinFormsApp4
{
    public partial class Help : Form
    {
        public Help()
        {
            InitializeComponent();
        }
        private const string docPath = "documentation\\Doc1.docx";
        private void Help_Load(object sender, EventArgs e)
        {

        }
        private void button1_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog dialog = new FolderBrowserDialog();
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                WebClient web = new WebClient();
                web.DownloadFile(docPath, dialog.SelectedPath + "\\Doc1.docx");
                MessageBox.Show("Файл успешно загружен!", "Скачивание файла");
            } 
        }
    }
}
