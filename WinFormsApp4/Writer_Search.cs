using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Npgsql;

namespace WinFormsApp4
{
    public partial class Writer_Search : Form
    {
        public Writer_Search()
        {
            InitializeComponent();
        }
        private void Writer_Search_Load(object sender, EventArgs e)
        {
            Form1.connection.Open();
            NpgsqlDataAdapter adapter = new NpgsqlDataAdapter("select * from writer order by id_writer", Form1.connection);
            if (Form1.data.Tables["Writer_S"] != null)
                Form1.data.Tables["Writer_S"].Clear();
            adapter.Fill(Form1.data, "Writer_S");
            Form1.connection.Close();
            dateTimePicker1.Enabled = false;
            comboBox1.Enabled = false;
            textBox1.Enabled = false;
            comboBox2.Enabled = false;
            dataGridView1.DataSource = Form1.data.Tables["Writer_S"];
            dataGridView1.AutoResizeColumns();
            dataGridView1.CurrentCell = null;
            dataGridView1.ReadOnly = true;
        }
        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton1.Checked)
            {
                dateTimePicker1.Enabled = true;
                comboBox1.Enabled = true;
            }
            else
            {
                dateTimePicker1.Enabled = false;
                comboBox1.Enabled = false;
            }
        }
        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton2.Checked)
            {
                textBox1.Enabled = true;
                comboBox2.Enabled = true;
            }
            else
            {
                textBox1.Enabled = false;
                comboBox2.Enabled = false;
            }
        }
        private void button1_Click(object sender, EventArgs e)
        {
            StringBuilder builder = new StringBuilder();
            if (radioButton1.Checked)
            {
                builder.Append("select * from writer where registr_date ");
                if (comboBox1.SelectedIndex == 0)
                    builder.Append("= ");
                else if (comboBox1.SelectedIndex == 1)
                    builder.Append("<= ");
                else if (comboBox1.SelectedIndex == 2)
                    builder.Append("< ");
                else if (comboBox1.SelectedIndex == 3)
                    builder.Append(">= ");
                else if (comboBox1.SelectedIndex == 4)
                    builder.Append("> ");
                builder.Append("'" + dateTimePicker1.Value.ToShortDateString() + "'");
            }
            else if (radioButton2.Checked)
            {
                builder.Append("select * from writer where rating_count ");
                if (comboBox2.SelectedIndex == 0)
                    builder.Append("= ");
                else if (comboBox2.SelectedIndex == 1)
                    builder.Append("<= ");
                else if (comboBox2.SelectedIndex == 2)
                    builder.Append("< ");
                else if (comboBox2.SelectedIndex == 3)
                    builder.Append(">= ");
                else if (comboBox2.SelectedIndex == 4)
                    builder.Append("> ");
                builder.Append(textBox1.Text);
            }
            Form1.connection.Open();
            NpgsqlDataAdapter adapter = new NpgsqlDataAdapter(builder.ToString(), Form1.connection);
            Form1.connection.Close();
            Form1.data.Tables["Writer_S"].Clear();
            adapter.Fill(Form1.data, "Writer_S");
        }
        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked)
            {
                Form1.connection.Open();
                NpgsqlDataAdapter adapter = new NpgsqlDataAdapter("select * from writer order by id_writer", Form1.connection);
                Form1.data.Tables["Writer_S"].Clear();
                adapter.Fill(Form1.data, "Writer_S");
                Form1.connection.Close();
            }
        }
    }
}
