using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace WinFormsApp4
{
    public partial class Main : Form
    {
        public Main()
        {
            InitializeComponent();
            if (Form1.isAdmin)
                label2.Text = "Ваш уровень прав доступа: Администратор";
            else
                label2.Text = "Ваш уровень прав доступа: Пользователь";
        }
        private void ToolStripMenuItem4_Click(object sender, System.EventArgs e)
        {
            Help help = new Help();
            help.Show();
        }
        private void ToolStripMenuItem3_Click(object sender, System.EventArgs e)
        {
            Profile profile = new Profile();
            profile.Show();
        }
        private void ToolStripMenuItem12_Click(object sender, System.EventArgs e)
        {
            Rating_Search rating = new Rating_Search();
            rating.Show();
        }
        private void ToolStripMenuItem11_Click(object sender, System.EventArgs e)
        {
            Discussion_Search discussion = new Discussion_Search();
            discussion.Show();
        }
        private void ToolStripMenuItem10_Click(object sender, System.EventArgs e)
        {
            Composition_Search composition = new Composition_Search();
            composition.Show();
        }
        private void ToolStripMenuItem9_Click(object sender, System.EventArgs e)
        {
            Writer_Search writer = new Writer_Search();
            writer.Show();
        }
        private void ToolStripMenuItem8_Click(object sender, System.EventArgs e)
        {
            Rating_Data rating = new Rating_Data();
            rating.Show();
        }
        private void ToolStripMenuItem7_Click(object sender, System.EventArgs e)
        {
            Discussion_Data discussion = new Discussion_Data();
            discussion.Show();
        }
        private void ToolStripMenuItem6_Click(object sender, System.EventArgs e)
        {
            Composition_Data composition = new Composition_Data();
            composition.Show();
        }
        private void ToolStripMenuItem5_Click(object sender, System.EventArgs e)
        {
            Writer_Data writer = new Writer_Data();
            writer.Show();
        }
    }
}
