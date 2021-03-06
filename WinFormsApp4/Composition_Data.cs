using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Net;
using System.Text;
using System.Windows.Forms;
using Npgsql;

namespace WinFormsApp4
{
    public partial class Composition_Data : Form
    {
        public Composition_Data()
        {
            InitializeComponent();
        }
        private void Composition_Data_Load(object sender, EventArgs e)
        {
            textBox1.Enabled = false;
            textBox2.ReadOnly = true;
            textBox3.ReadOnly = true;
            textBox4.ReadOnly = true;
            dateTimePicker1.Enabled = false;
            textBox5.ReadOnly = true;
            Form1.connection.Open();
            NpgsqlDataAdapter adapter = new NpgsqlDataAdapter("select id_comp, composition.name as name, pub_date, content, genre, concat_ws(' ', surname, writer.name, patronymic) as fio_author from composition inner join writer on author = id_writer order by id_comp", Form1.connection);
            if (Form1.data.Tables["Composition"] != null)
                Form1.data.Tables["Composition"].Clear();
            adapter.Fill(Form1.data, "Composition");
            Form1.connection.Close();
            if (Form1.data.Tables["Composition"].Rows.Count > n)
                FiledsForm_Fill();
            label7.Visible = false;
        }
        int n = 0;
        public void FiledsForm_Fill()
        {
            textBox1.Text = Form1.data.Tables["Composition"].Rows[n]["id_comp"].ToString();
            textBox2.Text = Form1.data.Tables["Composition"].Rows[n]["name"].ToString();
            dateTimePicker1.Text = Form1.data.Tables["Composition"].Rows[n]["pub_date"].ToString();
            textBox3.Text = Form1.data.Tables["Composition"].Rows[n]["content"].ToString();
            textBox4.Text = Form1.data.Tables["Composition"].Rows[n]["genre"].ToString();
            textBox5.Text = Form1.data.Tables["Composition"].Rows[n]["fio_author"].ToString();
        }
        public void FiledsForm_Clear()
        {
            textBox1.Text = "0";
            textBox2.Text = "";
            textBox3.Text = "";
            textBox4.Text = "";
            dateTimePicker1.Value = DateTime.Now;
            textBox5.Text = "";
        }
        private void button1_Click(object sender, EventArgs e)
        {
            if (n > 0)
            {
                --n;
                FiledsForm_Fill();
            }
        }
        private void button2_Click(object sender, EventArgs e)
        {
            if (n < Form1.data.Tables["Composition"].Rows.Count) n++;
            if (Form1.data.Tables["Composition"].Rows.Count > n)
                FiledsForm_Fill();
            else
                FiledsForm_Clear();
        }
        private void button3_Click(object sender, EventArgs e)
        {
            string path = "";
            FolderBrowserDialog folder = new FolderBrowserDialog();
            if (folder.ShowDialog() == DialogResult.OK)
                path = folder.SelectedPath + "\\";
            WebClient web = new WebClient();
            label7.Visible = true;
            web.DownloadFile(textBox3.Text, path + textBox2.Text + ".docx");
            MessageBox.Show("Файл скачан!", "Загрузка файла");
            label7.Visible = false;
            web.Dispose();
        }
        private void button4_Click(object sender, EventArgs e)
        {
            n = 0;
            if (Form1.data.Tables["Composition"].Rows.Count > n)
            {
                FiledsForm_Fill();
            }
        }
        private void button5_Click(object sender, EventArgs e)
        {
            n = Form1.data.Tables["Composition"].Rows.Count;
            FiledsForm_Clear();
        }
    }
}
