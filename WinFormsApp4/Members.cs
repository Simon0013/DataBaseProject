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
    public partial class Members : Form
    {
        public Members(Mode mode)
        {
            InitializeComponent();
            this.mode = mode;
        }
        public enum Mode {Discussion, Rating};
        private Mode mode;
        private void Members_Load(object sender, EventArgs e)
        {
            string sql;
            if (mode == Mode.Discussion)
                sql = "select id_writer, concat_ws(' ', surname, name, patronymic) as fio from writer inner join members_of_discussions on id_member = id_writer where id_discussion = " + Form1.data.Tables["Discussion"].Rows[Discussion_Data.n]["id_discussion"].ToString() + " order by id_writer";
            else
                sql = "select id_writer, concat_ws(' ', surname, name, patronymic) as fio from writer inner join members_of_ratings on id_member = id_writer where id_rating = " + Form1.data.Tables["Rating"].Rows[Rating_Data.n]["id_rating"].ToString() + " order by id_writer";
            Form1.connection.Open();
            NpgsqlDataAdapter adapter = new NpgsqlDataAdapter(sql, Form1.connection);
            if (Form1.data.Tables["Member"] != null)
                Form1.data.Tables["Member"].Clear();
            adapter.Fill(Form1.data, "Member");
            Form1.connection.Close();
            dataGridView1.DataSource = Form1.data.Tables["Member"];
            dataGridView1.AutoResizeColumns();
            dataGridView1.CurrentCell = null;
            dataGridView1.ReadOnly = true;
            NpgsqlCommand command = Form1.connection.CreateCommand();
            command.CommandText = "select concat_ws(' ', surname, name, patronymic) from writer order by id_writer";
            Form1.connection.Open();
            NpgsqlDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                comboBox1.Items.Add(reader[0].ToString());
            }
            Form1.connection.Close();
            comboBox1.SelectedIndex = 0;
            if (!Form1.isAdmin)
            {
                button1.Visible = false;
                button2.Visible = false;
                comboBox1.Visible = false;
                Size = new Size(678, 334);
            }
        }
        private void button1_Click(object sender, EventArgs e)
        {
            dataGridView1.CurrentCell = null;
            NpgsqlCommand command = Form1.connection.CreateCommand();
            command.CommandText = "select id_writer from writer where concat_ws(' ', surname, name, patronymic) = '" + comboBox1.Text + "'";
            Form1.connection.Open();
            NpgsqlDataReader reader = command.ExecuteReader();
            reader.Read();
            string id = reader[0].ToString();
            Form1.connection.Close();
            for (int i = 0; i < Form1.data.Tables["Member"].Rows.Count; i++)
                if (id == Form1.data.Tables["Member"].Rows[i]["id_writer"].ToString())
                {
                    MessageBox.Show("Добавление не выполнено: этот пользователь уже участвует в дискуссии!", "Ошибка добавления");
                    return;
                }
            command = Form1.connection.CreateCommand();
            if (mode == Mode.Discussion)
                command.CommandText = "insert into members_of_discussions values (" + id + ", " + Form1.data.Tables["Discussion"].Rows[Discussion_Data.n]["id_discussion"].ToString() + ")";
            else if (mode == Mode.Rating)
                command.CommandText = "insert into members_of_ratings values (" + id + ", " + Form1.data.Tables["Rating"].Rows[Rating_Data.n]["id_rating"].ToString() + ")";
            Form1.connection.Open();
            command.ExecuteNonQuery();
            Form1.connection.Close();
            Form1.data.Tables["Member"].Rows.Add(new object[] { id, comboBox1.Text });
        }
        private void button2_Click(object sender, EventArgs e)
        {
            if (dataGridView1.CurrentCell == null)
            {
                MessageBox.Show("Сначала надо выбрать экземпляр для удаления!", "Ошибка удаления");
                return;
            }
            NpgsqlCommand command = Form1.connection.CreateCommand();
            if (mode == Mode.Discussion)
                command.CommandText = "delete from members_of_discussions where id_member = " + Form1.data.Tables["Member"].Rows[dataGridView1.CurrentCell.RowIndex]["id_writer"].ToString() + " and id_discussion = " + Form1.data.Tables["Discussion"].Rows[Discussion_Data.n]["id_discussion"].ToString();
            else if (mode == Mode.Rating)
                command.CommandText = "delete from members_of_ratings where id_member = " + Form1.data.Tables["Member"].Rows[dataGridView1.CurrentCell.RowIndex]["id_writer"].ToString() + " and id_rating = " + Form1.data.Tables["Rating"].Rows[Rating_Data.n]["id_rating"].ToString();
            Form1.connection.Open();
            try
            {
                command.ExecuteNonQuery();
            }
            catch (NpgsqlException ne)
            {
                MessageBox.Show(ne.Message, "Ошибка удаления");
                Form1.connection.Close();
                return;
            }
            Form1.connection.Close();
            Form1.data.Tables["Member"].Rows.RemoveAt(dataGridView1.CurrentCell.RowIndex);
            dataGridView1.CurrentCell = null;
        }
    }
}
