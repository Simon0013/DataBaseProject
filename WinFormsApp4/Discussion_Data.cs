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
    public partial class Discussion_Data : Form
    {
        public Discussion_Data()
        {
            InitializeComponent();
        }
        public static int n = 0;
        public void FiledsForm_Fill()
        {
            textBox1.Text = Form1.data.Tables["Discussion"].Rows[n]["id_discussion"].ToString();
            textBox2.Text = Form1.data.Tables["Discussion"].Rows[n]["subject"].ToString();
            dateTimePicker1.Text = Form1.data.Tables["Discussion"].Rows[n]["create_date"].ToString();
            comboBox1.Text = Form1.data.Tables["Discussion"].Rows[n]["access_level"].ToString();
            comboBox2.Text = Form1.data.Tables["Discussion"].Rows[n]["fio_admin"].ToString();
            Form1.connection.Open();
            NpgsqlDataAdapter adapter = new NpgsqlDataAdapter("select id_comment, content, concat_ws(' ', surname, name, patronymic) as fio_author from comment inner join writer on author = id_writer where discussion = " + textBox1.Text + " order by id_comment", Form1.connection);
            if (Form1.data.Tables["Comment"] != null)
                Form1.data.Tables["Comment"].Clear();
            adapter.Fill(Form1.data, "Comment");
            Form1.connection.Close();
            dataGridView1.DataSource = Form1.data.Tables["Comment"];
            dataGridView1.CurrentCell = null;
            dataGridView1.AutoResizeColumns();
        }
        public void FiledsForm_Clear()
        {
            textBox1.Text = "0";
            textBox2.Text = "";
            dateTimePicker1.Value = DateTime.Now;
            comboBox1.SelectedIndex = 0;
            comboBox2.SelectedIndex = 0;
            dataGridView1.DataSource = null;
        }
        private void Show_Comments(bool isShow)
        {
            dataGridView1.Visible = isShow;
            if (Form1.isAdmin) button7.Visible = isShow;
            else button7.Visible = false;
            if (Form1.isAdmin) button8.Visible = isShow;
            else button8.Visible = false;
            if (Form1.isAdmin) button9.Visible = isShow;
            else button9.Visible = false;
            if (isShow)
                Size = new Size(751, 730);
            else
                Size = new Size(751, 305);
        }
        private void Discussion_Data_Load(object sender, EventArgs e)
        {
            textBox1.Enabled = false;
            if (!Form1.isAdmin)
            {
                textBox2.ReadOnly = true;
                dateTimePicker1.Enabled = false;
                comboBox1.Enabled = false;
                comboBox2.Enabled = false;
                button3.Visible = false;
                button4.Visible = false;
            }
            Form1.connection.Open();
            NpgsqlDataAdapter adapter = new NpgsqlDataAdapter("select id_discussion, subject, create_date, access_level, concat_ws(' ', surname, name, patronymic) as fio_admin from discussion inner join writer on id_admin = id_writer order by id_discussion", Form1.connection);
            if (Form1.data.Tables["Discussion"] != null)
                Form1.data.Tables["Discussion"].Clear();
            adapter.Fill(Form1.data, "Discussion");
            Form1.connection.Close();
            if (Form1.data.Tables["Discussion"].Rows.Count > n)
                FiledsForm_Fill();
            Form1.connection.Open();
            string sql = "select concat_ws(' ', surname, name, patronymic) as fio from writer order by id_writer";
            NpgsqlCommand command = new NpgsqlCommand(sql, Form1.connection);
            NpgsqlDataReader dataReader = command.ExecuteReader();
            while (dataReader.Read())
                comboBox2.Items.Add(dataReader["fio"]);
            Form1.connection.Close();
            Show_Comments(false);
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
            if (n < Form1.data.Tables["Discussion"].Rows.Count) n++;
            if (Form1.data.Tables["Discussion"].Rows.Count > n)
                FiledsForm_Fill();
            else
                FiledsForm_Clear();
        }
        private void button3_Click(object sender, EventArgs e)
        {
            if (n == Form1.data.Tables["Discussion"].Rows.Count)
            {
                string admin;
                NpgsqlCommand npgsql = Form1.connection.CreateCommand();
                npgsql.CommandText = "select max(id_discussion) + 1 from discussion";
                Form1.connection.Open();
                NpgsqlDataReader reader = npgsql.ExecuteReader();
                reader.Read();
                textBox1.Text = reader[0].ToString();
                Form1.connection.Close();
                npgsql = Form1.connection.CreateCommand();
                npgsql.CommandText = "select id_writer from writer where concat_ws(' ', surname, name, patronymic) = '" + comboBox2.Text + "'";
                Form1.connection.Open();
                reader = npgsql.ExecuteReader();
                reader.Read();
                admin = reader[0].ToString();
                Form1.connection.Close();
                string sql = "insert into discussion values (" + textBox1.Text + ", '" + textBox2.Text +  "', '" + dateTimePicker1.Value.ToShortDateString() + "', '" + comboBox1.Text + "', " + admin + ")";
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
                Form1.data.Tables["Discussion"].Rows.Add(new object[] { textBox1.Text, textBox2.Text, dateTimePicker1.Text, comboBox1.Text, comboBox2.Text });
            }
            else
            {
                string admin;
                NpgsqlCommand npgsql = Form1.connection.CreateCommand();
                npgsql.CommandText = "select id_writer from writer where concat_ws(' ', surname, name, patronymic) = '" + comboBox2.Text + "'";
                Form1.connection.Open();
                NpgsqlDataReader reader = npgsql.ExecuteReader();
                reader.Read();
                admin = reader[0].ToString();
                Form1.connection.Close();
                string sql = "update discussion set subject = '" + textBox2.Text + "', create_date = '" + dateTimePicker1.Value.ToShortDateString() + "', access_level = '" + comboBox1.Text + "', id_admin = " + admin + " where id_discussion = " + textBox1.Text;
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
                Form1.data.Tables["Discussion"].Rows.Add(new object[] { textBox1.Text, textBox2.Text, dateTimePicker1.Text, comboBox1.Text, comboBox2.Text });
            }
        }
        private void button4_Click(object sender, EventArgs e)
        {
            string message = "Удалить запись о дискуссии с идентификатором " + textBox1.Text + "?";
            string caption = "Удаление";
            MessageBoxButtons buttons = MessageBoxButtons.YesNo;
            DialogResult result = MessageBox.Show(message, caption, buttons);
            if (result == DialogResult.No) { return; }
            string sql = "delete from discussion where id_discussion = " + textBox1.Text;
            NpgsqlCommand command = new NpgsqlCommand(sql, Form1.connection);
            Form1.connection.Open();
            command.ExecuteNonQuery();
            Form1.connection.Close();
            try
            {
                Form1.data.Tables["Discussion"].Rows.RemoveAt(n);
            }
            catch (IndexOutOfRangeException)
            {
                MessageBox.Show("Удаление не было выполнено из-за указания несуществующего экземпляра", "Ошибка");
                return;
            }
            if (Form1.data.Tables["Discussion"].Rows.Count > n)
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
            if (Form1.data.Tables["Discussion"].Rows.Count > n)
            {
                FiledsForm_Fill();
            }
        }
        private void button6_Click(object sender, EventArgs e)
        {
            n = Form1.data.Tables["Discussion"].Rows.Count;
            FiledsForm_Clear();
        }
        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            Show_Comments(checkBox1.Checked);
        }
        private void button7_Click(object sender, EventArgs e)
        {
            dataGridView1.CurrentCell = null;
            AddOrEditComment add = new AddOrEditComment(AddOrEditComment.Mode.Add);
            add.ShowDialog();
        }
        private void button8_Click(object sender, EventArgs e)
        {
            if (dataGridView1.CurrentCell == null)
            {
                MessageBox.Show("Сначала необходимо выбрать экземпляр, который подлежит редактированию.", "Ошибка редактирования");
                return;
            }
            AddOrEditComment edit = new AddOrEditComment(AddOrEditComment.Mode.Edit, dataGridView1.CurrentCell.RowIndex);
            edit.ShowDialog();
            dataGridView1.CurrentCell = null;
        }
        private void button9_Click(object sender, EventArgs e)
        {
            if (dataGridView1.CurrentCell == null)
            {
                MessageBox.Show("Сначала необходимо выбрать экземпляр, который подлежит удалению.", "Ошибка удаления");
                return;
            }
            NpgsqlCommand command = Form1.connection.CreateCommand();
            command.CommandText = "delete from comment where id_comment = " + dataGridView1.Rows[dataGridView1.CurrentCell.RowIndex].Cells["id_comment"].Value.ToString();
            Form1.connection.Open();
            try
            {
                command.ExecuteNonQuery();
                Form1.connection.Close();
                Form1.data.Tables["Comment"].Rows.RemoveAt(dataGridView1.CurrentCell.RowIndex);
                dataGridView1.CurrentCell = null;
            }
            catch (NpgsqlException ne)
            {
                MessageBox.Show(ne.Message, "Ошибка удаления");
                Form1.connection.Close();
                return;
            }
        }
        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox2.Checked)
            {
                Members members = new Members(Members.Mode.Discussion);
                members.ShowDialog();
                checkBox2.Checked = false;
            }
        }
    }
}
