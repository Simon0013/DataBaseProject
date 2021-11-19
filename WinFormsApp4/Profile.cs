using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Forms;
using System.Xml.Serialization;

namespace WinFormsApp4
{
    public partial class Profile : Form
    {
        public Profile()
        {
            InitializeComponent();
        }
        private void Profile_Load(object sender, EventArgs e)
        {
            Size = new Size(415, 189);
            label1.Text = "Ваш ID пользователя: " + Form1.acc.Id;
            if (Form1.isAdmin)
                label2.Text = "Ваш уровень прав доступа: Администратор";
            else
                label2.Text = "Ваш уровень прав доступа: Пользователь";
            textBox1.Text = Form1.acc.Login;
            textBox2.UseSystemPasswordChar = true;
            textBox3.UseSystemPasswordChar = true;
            textBox4.UseSystemPasswordChar = true;
        }
        private void button1_Click(object sender, EventArgs e)
        {
            Size = new Size(415, 348);
            label4.Visible = true;
            label5.Visible = true;
            label6.Visible = true;
            textBox2.Visible = true;
            textBox3.Visible = true;
            textBox4.Visible = true;
            button2.Visible = true;
        }
        private void button2_Click(object sender, EventArgs e)
        {
            if (textBox2.Text != Form1.acc.Password)
            {
                MessageBox.Show("Смена пароля не выполнена: текущий пароль введен неверно", "Ошибка смены пароля");
                return;
            }
            if (textBox3.Text != textBox4.Text)
            {
                MessageBox.Show("Смена пароля не выполнена: новый пароль и его подтверждение не совпадают", "Ошибка смены пароля");
                return;
            }
            XmlSerializer serializer = new XmlSerializer(typeof(Form1.Account));
            string fileName = "logins\\id" + Form1.acc.Id + ".xml";
            FileStream fs = File.Open(fileName, FileMode.OpenOrCreate);
            Form1.acc.Login = textBox1.Text;
            Form1.acc.Password = textBox3.Text;
            serializer.Serialize(fs, Form1.acc);
            MessageBox.Show("Смена пароля успешно выполнена!", "Смена пароля");
        }
    }
}
