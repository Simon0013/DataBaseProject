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
    public partial class Writer_Data : Form
    {
        public Writer_Data()
        {
            InitializeComponent();
        }
        int n = 0;
        private void Writer_Data_Load(object sender, EventArgs e)
        {
            textBox1.Enabled = false;
            if (!Form1.isAdmin)
            {
                textBox2.ReadOnly = true;
                textBox3.ReadOnly = true;
                textBox4.ReadOnly = true;
                dateTimePicker1.Enabled = false;
                textBox5.ReadOnly = true;
                button3.Visible = false;
                button4.Visible = false;
            }
            Form1.connection.Open();
            NpgsqlDataAdapter adapter = new NpgsqlDataAdapter("select * from writer order by id_writer", Form1.connection);
            if (Form1.data.Tables["Writer"] != null)
                Form1.data.Tables["Writer"].Clear();
            adapter.Fill(Form1.data, "Writer");
            Form1.connection.Close();
            if (Form1.data.Tables["Writer"].Rows.Count > n)
                FiledsForm_Fill();
        }
        public void FiledsForm_Fill()
        {
            textBox1.Text = Form1.data.Tables["Writer"].Rows[n]["id_writer"].ToString();
            textBox2.Text = Form1.data.Tables["Writer"].Rows[n]["surname"].ToString();
            textBox3.Text = Form1.data.Tables["Writer"].Rows[n]["name"].ToString();
            textBox4.Text = Form1.data.Tables["Writer"].Rows[n]["patronymic"].ToString();
            dateTimePicker1.Text = Form1.data.Tables["Writer"].Rows[n]["registr_date"].ToString();
            textBox5.Text = Form1.data.Tables["Writer"].Rows[n]["rating_count"].ToString();
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
            if (n < Form1.data.Tables["Writer"].Rows.Count) n++;
            if (Form1.data.Tables["Writer"].Rows.Count > n)
                FiledsForm_Fill();
            else
                FiledsForm_Clear();
        }
        private void button3_Click(object sender, EventArgs e)
        {
            if (n == Form1.data.Tables["Writer"].Rows.Count)
            {
                NpgsqlCommand npgsql = Form1.connection.CreateCommand();
                npgsql.CommandText = "select max(id_writer) + 1 from writer";
                Form1.connection.Open();
                NpgsqlDataReader reader = npgsql.ExecuteReader();
                reader.Read();
                textBox1.Text = reader[0].ToString();
                Form1.connection.Close();
                string sql = "insert into writer values (" + textBox1.Text + ", '" + textBox2.Text + "', '" + textBox3.Text + "', '" + textBox4.Text + "', '" + dateTimePicker1.Value.ToShortDateString() + "', " + textBox5.Text + ")";
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
                Form1.data.Tables["Writer"].Rows.Add(new object[] { textBox1.Text, textBox2.Text, textBox3.Text, textBox4.Text, dateTimePicker1.Text, textBox5.Text });
            }
            else
            {
                string sql = "update writer set surname = '" + textBox2.Text + "', name = '" + textBox3.Text + "', patronymic = '" + textBox4.Text + "', registr_date = '" + dateTimePicker1.Value.ToShortDateString() + "', rating_count = " + textBox5.Text + " where id_writer = " + textBox1.Text;
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
                Form1.data.Tables["Writer"].Rows.Add(new object[] { textBox1.Text, textBox2.Text, textBox3.Text, textBox4.Text, dateTimePicker1.Text, textBox5.Text });
            }
        }
        private void button4_Click(object sender, EventArgs e)
        {
            string message = "Удалить запись о писателе с идентификатором " + textBox1.Text + "?";
            string caption = "Удаление";
            MessageBoxButtons buttons = MessageBoxButtons.YesNo;
            DialogResult result = MessageBox.Show(message, caption, buttons);
            if (result == DialogResult.No) { return; }
            string sql = "delete from writer where id_writer = " + textBox1.Text;
            NpgsqlCommand command = new NpgsqlCommand(sql, Form1.connection);
            Form1.connection.Open();
            command.ExecuteNonQuery();
            Form1.connection.Close();
            try
            {
                Form1.data.Tables["Writer"].Rows.RemoveAt(n);
            }
            catch (IndexOutOfRangeException)
            {
                MessageBox.Show("Удаление не было выполнено из-за указания несуществующего экземпляра", "Ошибка");
                return;
            }
            if (Form1.data.Tables["Writer"].Rows.Count > n)
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
            if (Form1.data.Tables["Writer"].Rows.Count > n)
            {
                FiledsForm_Fill();
            }
        }
        private void button6_Click(object sender, EventArgs e)
        {
            n = Form1.data.Tables["Writer"].Rows.Count;
            FiledsForm_Clear();
        }
    }
}
