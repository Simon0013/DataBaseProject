using Npgsql;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace WinFormsApp4
{
    public partial class Rating_Search : Form
    {
        public Rating_Search()
        {
            InitializeComponent();
        }
        private void Rating_Search_Load(object sender, EventArgs e)
        {
            Form1.connection.Open();
            NpgsqlDataAdapter adapter = new NpgsqlDataAdapter("select rating.id_rating, create_date, count(id_member) from rating inner join members_of_ratings on members_of_ratings.id_rating = rating.id_rating group by rating.id_rating, create_date order by rating.id_rating", Form1.connection);
            if (Form1.data.Tables["Rating_S"] != null)
                Form1.data.Tables["Rating_S"].Clear();
            adapter.Fill(Form1.data, "Rating_S");
            Form1.connection.Close();
            dateTimePicker1.Enabled = false;
            comboBox1.Enabled = false;
            comboBox2.Enabled = false;
            textBox1.Enabled = false;
            dataGridView1.DataSource = Form1.data.Tables["Rating_S"];
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
                builder.Append("select rating.id_rating, create_date, count(id_member) from rating inner join members_of_ratings on members_of_ratings.id_rating = rating.id_rating where create_date ");
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
                builder.Append(" group by rating.id_rating, create_date");
            }
            else if (radioButton2.Checked)
            {
                builder.Append("select rating.id_rating, create_date, count(id_member) from rating inner join members_of_ratings on members_of_ratings.id_rating = rating.id_rating group by rating.id_rating, create_date having count(id_member) ");
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
            Form1.data.Tables["Rating_S"].Clear();
            adapter.Fill(Form1.data, "Rating_S");
        }
        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            Form1.connection.Open();
            NpgsqlDataAdapter adapter = new NpgsqlDataAdapter("select rating.id_rating, create_date, count(id_member) from rating inner join members_of_ratings on members_of_ratings.id_rating = rating.id_rating group by rating.id_rating, create_date order by rating.id_rating", Form1.connection);
            Form1.data.Tables["Rating_S"].Clear();
            adapter.Fill(Form1.data, "Rating_S");
            Form1.connection.Close();
        }
    }
}
