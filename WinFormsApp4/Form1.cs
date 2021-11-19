using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Serialization;
using Npgsql;

namespace WinFormsApp4
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            button2.Location = new Point(214, 361);
            Size = new Size(544, 446);
        }
        private const string authPath = "logins\\";
        public static bool isAdmin;
        public static NpgsqlConnection connection = new NpgsqlConnection("Host = localhost; User Id = postgres; Database = writer_project; Port = 5432; Password = a;");
        public static DataSet data = new DataSet();
        public static Account acc;
        public class Account
        {
            public int Id { get; set; }
            public string Login { get; set; }
            public string Password { get; set; }
            public bool IsAdmin { get; set; }
        }
        private void button1_Click(object sender, EventArgs e)
        {
            if ((textBox1.Text.Length == 0) || (textBox2.Text.Length == 0))
            {
                MessageBox.Show("Вход не выполнен: поля должны быть заполнены", "Ошибка входа");
                return;
            }
            var files = Directory.GetFiles(authPath);
            foreach (var file in files)
            {
                XmlSerializer serializer = new XmlSerializer(typeof(Account));
                FileStream fs = File.OpenRead(file);
                acc = (Account)serializer.Deserialize(fs);
                fs.Close();
                if ((acc.Login == textBox1.Text) && (acc.Password == textBox2.Text))
                {
                    isAdmin = acc.IsAdmin;
                    Main main = new Main();
                    Hide();
                    main.ShowDialog();
                    Close();
                    return;
                }
            }
            MessageBox.Show("Вход не выполнен: неверный логин или пароль", "Ошибка входа");
        }
        private void button2_Click(object sender, EventArgs e)
        {
            string token;
            if ((textBox3.Text.Length == 0) || (textBox4.Text.Length == 0))
            {
                MessageBox.Show("Регистрация не выполнена: поля должны быть заполнены", "Ошибка регистрации");
                return;
            }
            if (checkBox1.Checked)
            {
                StreamReader reader = new StreamReader("token.txt");
                token = reader.ReadLine();
                reader.Close();
                if (token != textBox5.Text)
                {
                    MessageBox.Show("Регистрация не выполнена: неверный токен для регистрации адиминистратора", "Ошибка регистрации");
                    return;
                }
            }
            string fileName = "id" + (Directory.GetFiles(authPath).Length + 1) + ".xml";
            XmlSerializer serializer = new XmlSerializer(typeof(Account));
            acc = new Account();
            acc.Login = textBox3.Text;
            acc.Password = textBox4.Text;
            acc.Id = Directory.GetFiles(authPath).Length + 1;
            acc.IsAdmin = checkBox1.Checked ? true : false;
            FileStream fs = File.Open(authPath + fileName, FileMode.Create);
            serializer.Serialize(fs, acc);
            fs.Close();
            isAdmin = acc.IsAdmin;
            Hide();
            Main main = new Main();
            main.ShowDialog();
            Close();
        }
        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked)
            {
                button2.Location = new Point(214, 445);
                Size = new Size(544, 529);
                textBox5.Visible = true;
                label7.Visible = true;
            }
            else
            {
                button2.Location = new Point(214, 361);
                Size = new Size(544, 446);
                textBox5.Visible = false;
                label7.Visible = false;
            }
        }
    }
}
