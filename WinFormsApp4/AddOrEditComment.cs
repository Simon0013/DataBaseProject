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
    public partial class AddOrEditComment : Form
    {
        public AddOrEditComment(Mode mode)
        {
            InitializeComponent();
            this.mode = mode;
        }
        public AddOrEditComment(Mode mode, int row)
        {
            InitializeComponent();
            this.mode = mode;
            this.row = row;
        }
        public enum Mode {Add, Edit};
        private Mode mode;
        private int row;
        private void AddOrEditComment_Load(object sender, EventArgs e)
        {
            Form1.connection.Open();
            string sql = "select concat_ws(' ', surname, name, patronymic) as fio from writer order by id_writer";
            NpgsqlCommand command = new NpgsqlCommand(sql, Form1.connection);
            NpgsqlDataReader dataReader = command.ExecuteReader();
            while (dataReader.Read())
                comboBox1.Items.Add(dataReader["fio"]);
            Form1.connection.Close();
            comboBox1.SelectedIndex = 0;
            textBox1.Enabled = false;
            if (mode == Mode.Edit)
            {
                textBox1.Text = Form1.data.Tables["Comment"].Rows[row]["id_comment"].ToString();
                textBox2.Text = Form1.data.Tables["Comment"].Rows[row]["content"].ToString();
                comboBox1.Text = Form1.data.Tables["Comment"].Rows[row]["fio_author"].ToString();
            }
        }
        private void button1_Click(object sender, EventArgs e)
        {
            string author;
            Form1.connection.Open();
            string sql = "select id_writer from writer where concat_ws(' ', surname, name, patronymic) = '" + comboBox1.Text + "'";
            NpgsqlCommand command = new NpgsqlCommand(sql, Form1.connection);
            NpgsqlDataReader dataReader = command.ExecuteReader();
            dataReader.Read();
            author = dataReader[0].ToString();
            Form1.connection.Close();
            if (mode == Mode.Add)
            {
                Form1.connection.Open();
                sql = "select max(id_comment) + 1 from comment";
                command = new NpgsqlCommand(sql, Form1.connection);
                dataReader = command.ExecuteReader();
                dataReader.Read();
                textBox1.Text = dataReader[0].ToString();
                Form1.connection.Close();
                sql = "insert into comment values (" + textBox1.Text + ", '" + textBox2.Text + "', " + author + ", " + Form1.data.Tables["Discussion"].Rows[Discussion_Data.n]["id_discussion"].ToString() + ")";
                command = new NpgsqlCommand(sql, Form1.connection);
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
                Form1.data.Tables["Comment"].Rows.Add(new object[] { textBox1.Text, textBox2.Text, comboBox1.Text });
            }
            else
            {
                sql = "update comment set content = '" + textBox2.Text + "', author = " + author + ", discussion = " + Form1.data.Tables["Discussion"].Rows[Discussion_Data.n]["id_discussion"].ToString() + " where id_comment = " + textBox1.Text;
                command = new NpgsqlCommand(sql, Form1.connection);
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
                Form1.data.Tables["Comment"].Rows[row]["content"] = textBox2.Text;
                Form1.data.Tables["Comment"].Rows[row]["fio_author"] = comboBox1.Text;
            }
        }
        private void button2_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
