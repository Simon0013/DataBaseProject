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
    public partial class Rating_Data : Form
    {
        public Rating_Data()
        {
            InitializeComponent();
        }
        public static int n = 0;
        public void FiledsForm_Fill()
        {
            textBox1.Text = Form1.data.Tables["Rating"].Rows[n]["id_rating"].ToString();
            dateTimePicker1.Text = Form1.data.Tables["Rating"].Rows[n]["create_date"].ToString();
        }
        public void FiledsForm_Clear()
        {
            textBox1.Text = "0";
            dateTimePicker1.Value = DateTime.Now;
        }
        private void Rating_Data_Load(object sender, EventArgs e)
        {
            textBox1.Enabled = false;
            if (!Form1.isAdmin)
            {
                dateTimePicker1.Enabled = false;
                button3.Visible = false;
                button4.Visible = false;
            }
            Form1.connection.Open();
            NpgsqlDataAdapter adapter = new NpgsqlDataAdapter("select id_rating, create_date from rating order by id_rating", Form1.connection);
            if (Form1.data.Tables["Rating"] != null)
                Form1.data.Tables["Rating"].Clear();
            adapter.Fill(Form1.data, "Rating");
            Form1.connection.Close();
            if (Form1.data.Tables["Rating"].Rows.Count > n)
                FiledsForm_Fill();
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
            if (n < Form1.data.Tables["Rating"].Rows.Count) n++;
            if (Form1.data.Tables["Rating"].Rows.Count > n)
                FiledsForm_Fill();
            else
                FiledsForm_Clear();
        }
        private void button3_Click(object sender, EventArgs e)
        {
            if (n == Form1.data.Tables["Rating"].Rows.Count)
            {
                NpgsqlCommand npgsql = Form1.connection.CreateCommand();
                npgsql.CommandText = "select max(id_rating) + 1 from rating";
                Form1.connection.Open();
                NpgsqlDataReader reader = npgsql.ExecuteReader();
                reader.Read();
                textBox1.Text = reader[0].ToString();
                Form1.connection.Close();
                string sql = "insert into rating values (" + textBox1.Text + "', '" + dateTimePicker1.Value.ToShortDateString() + "')";
                NpgsqlCommand command = new NpgsqlCommand(sql, Form1.connection);
                Form1.connection.Open();
                try
                {
                    command.ExecuteNonQuery();
                }
                catch (NpgsqlException)
                {
                    MessageBox.Show("Добавление экземпляра не было успешно проведено из-за неуказания его данных или несоответствия их типов или попытки добавить экземпляр с уже используемым кодом!!!", "Ошибка");
                    Form1.connection.Close();
                    return;
                }
                Form1.connection.Close();
                textBox1.Enabled = false;
                Form1.data.Tables["Rating"].Rows.Add(new object[] { textBox1.Text, dateTimePicker1.Text });
            }
            else
            {
                string sql = "update rating set create_date = '" + dateTimePicker1.Value.ToShortDateString() + "' where id_rating = " + textBox1.Text;
                NpgsqlCommand command = new NpgsqlCommand(sql, Form1.connection);
                Form1.connection.Open();
                try
                {
                    command.ExecuteNonQuery();
                }
                catch (NpgsqlException)
                {
                    MessageBox.Show("Изменения не были успешно сохранены из-за несовпадения типов значений!!!", "Ошибка");
                    Form1.connection.Close();
                    return;
                }
                Form1.connection.Close();
                Form1.data.Tables["Rating"].Rows.Add(new object[] { textBox1.Text, dateTimePicker1.Text });
            }
        }
        private void button4_Click(object sender, EventArgs e)
        {
            string message = "Удалить запись о рейтинге с идентификатором " + textBox1.Text + "?";
            string caption = "Удаление";
            MessageBoxButtons buttons = MessageBoxButtons.YesNo;
            DialogResult result = MessageBox.Show(message, caption, buttons);
            if (result == DialogResult.No) { return; }
            string sql = "delete from rating where id_rating = " + textBox1.Text;
            NpgsqlCommand command = new NpgsqlCommand(sql, Form1.connection);
            Form1.connection.Open();
            command.ExecuteNonQuery();
            Form1.connection.Close();
            try
            {
                Form1.data.Tables["Rating"].Rows.RemoveAt(n);
            }
            catch (IndexOutOfRangeException)
            {
                MessageBox.Show("Удаление не было выполнено из-за указания несуществующего экземпляра", "Ошибка");
                return;
            }
            if (Form1.data.Tables["Rating"].Rows.Count > n)
            {
                FiledsForm_Fill();
            }
            else
            {
                FiledsForm_Clear();
            }
        }
        private void button5_Click(object sender, EventArgs e)
        {
            n = 0;
            if (Form1.data.Tables["Rating"].Rows.Count > n)
            {
                FiledsForm_Fill();
            }
        }
        private void button6_Click(object sender, EventArgs e)
        {
            n = Form1.data.Tables["Rating"].Rows.Count;
            FiledsForm_Clear();
        }
        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked)
            {
                Members members = new Members(Members.Mode.Rating);
                members.ShowDialog();
                checkBox1.Checked = false;
            }
        }
    }
}
