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
    public partial class Discussion_Search : Form
    {
        public Discussion_Search()
        {
            InitializeComponent();
        }
        private void Discussion_Search_Load(object sender, EventArgs e)
        {
            Form1.connection.Open();
            NpgsqlDataAdapter adapter = new NpgsqlDataAdapter("select id_discussion, subject, create_date, access_level, concat_ws(' ', surname, name, patronymic) as fio_admin from discussion inner join writer on id_admin = id_writer order by id_discussion", Form1.connection);
            if (Form1.data.Tables["Discussion_S"] != null)
                Form1.data.Tables["Discussion_S"].Clear();
            adapter.Fill(Form1.data, "Discussion_S");
            Form1.connection.Close();
            dateTimePicker1.Enabled = false;
            comboBox1.Enabled = false;
            comboBox2.Enabled = false;
            dataGridView1.DataSource = Form1.data.Tables["Discussion_S"];
            dataGridView1.AutoResizeColumns();
            dataGridView1.CurrentCell = null;
            dataGridView1.ReadOnly = true;
            Form1.connection.Open();
            string sql = "select concat_ws(' ', surname, name, patronymic) as fio from writer order by id_writer";
            NpgsqlCommand command = new NpgsqlCommand(sql, Form1.connection);
            NpgsqlDataReader dataReader = command.ExecuteReader();
            while (dataReader.Read())
                comboBox3.Items.Add(dataReader["fio"]);
            Form1.connection.Close();
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
                comboBox2.Enabled = true;
            else
                comboBox2.Enabled = false;
        }
        private void radioButton3_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton3.Checked)
                comboBox3.Enabled = true;
            else
                comboBox3.Enabled = false;
        }
        private void button1_Click(object sender, EventArgs e)
        {
            StringBuilder builder = new StringBuilder();
            if (radioButton1.Checked)
            {
                builder.Append("select id_discussion, subject, create_date, access_level, concat_ws(' ', surname, name, patronymic) as fio_admin from discussion inner join writer on id_admin = id_writer where create_date ");
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
                builder.Append("select id_discussion, subject, create_date, access_level, concat_ws(' ', surname, name, patronymic) as fio_admin from discussion inner join writer on id_admin = id_writer where access_level = '");
                builder.Append(comboBox2.Text);
                builder.Append("'");
            }
            else if (radioButton3.Checked)
            {
                builder.Append("select id_discussion, subject, create_date, access_level, concat_ws(' ', surname, name, patronymic) as fio_admin from discussion inner join writer on id_admin = id_writer where concat_ws(' ', surname, name, patronymic) = '");
                builder.Append(comboBox3.Text);
                builder.Append("'");
            }
            Form1.connection.Open();
            NpgsqlDataAdapter adapter = new NpgsqlDataAdapter(builder.ToString(), Form1.connection);
            Form1.connection.Close();
            Form1.data.Tables["Discussion_S"].Clear();
            adapter.Fill(Form1.data, "Discussion_S");
        }
        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            Form1.connection.Open();
            NpgsqlDataAdapter adapter = new NpgsqlDataAdapter("select id_discussion, subject, create_date, access_level, concat_ws(' ', surname, name, patronymic) as fio_admin from discussion inner join writer on id_admin = id_writer order by id_discussion", Form1.connection);
            Form1.data.Tables["Discussion_S"].Clear();
            adapter.Fill(Form1.data, "Discussion_S");
            Form1.connection.Close();
        }
    }
}
