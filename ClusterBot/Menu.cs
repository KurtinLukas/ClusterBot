﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows.Forms;

namespace Top_down_shooter
{
    public class Menu
    {
        List<Button> buttons = new List<Button>();
        List<Control> ctrls = new List<Control>();
        Button playBtn = new Button();
        Button resBtn = new Button();
        Button ctrlBtn = new Button();
        Button about = new Button();
        Button back = new Button();
        Label controlsH = new Label();
        Label controlsTxt = new Label();
        Label aboutH = new Label();
        Label aboutTxt = new Label();
        public bool visible = true;

        Form form;
        Timer timer;
        PictureBox menuPic = new PictureBox();

        public Menu(Form form, Timer timer, Image image)
        {
            this.form = form;
            this.timer = timer;

            menuPic.Location = new Point(100, 100);
            menuPic.Size = new Size(800, 750);
            menuPic.Paint += MenuPic_Paint;
            //menuPic.Image = image;
            this.form.Controls.Add(menuPic);
            menuPic.BringToFront();

            playBtn.Text = "PLAY";
            playBtn.Location = new Point(150, 500);
            playBtn.AutoSize = true;
            playBtn.Font = new Font("Verdana", 25);
            playBtn.BackColor = Color.Black;
            playBtn.ForeColor = Color.White;
            playBtn.Click += playBtn_click;
            this.form.Controls.Add(playBtn);
            playBtn.BringToFront();
            buttons.Add(playBtn);

            resBtn.Text = "RESTART";
            resBtn.Location = new Point(300, 500);
            resBtn.AutoSize = true;
            resBtn.Font = new Font("Verdana", 25);
            resBtn.BackColor = Color.Black;
            resBtn.ForeColor = Color.White;
            resBtn.Click += ResBtn_Click;
            this.form.Controls.Add(resBtn);
            resBtn.BringToFront();
            buttons.Add(resBtn);

            ctrlBtn.Text = "CONTROLS";
            ctrlBtn.Location = new Point(525, 500);
            ctrlBtn.AutoSize = true;
            ctrlBtn.Font = new Font("Verdana", 25);
            ctrlBtn.BackColor = Color.Black;
            ctrlBtn.ForeColor = Color.White;
            ctrlBtn.Click += CtrlBtn_Click;
            this.form.Controls.Add(ctrlBtn);
            ctrlBtn.BringToFront();
            buttons.Add(ctrlBtn);
            
            controlsH.Text = "CONTROLS";
            controlsH.Location = new Point(150, 150);
            controlsH.AutoSize = true;
            controlsH.Font = new Font("Verdana", 35);
            controlsH.BackColor = Color.Black;
            controlsH.ForeColor = Color.White;
            this.form.Controls.Add(controlsH);
            controlsH.BringToFront();
            controlsH.Visible = false;
            ctrls.Add(controlsH);

            controlsTxt.Text = "Movement: WASD\nShooting: Left Mouse Button\nMenu: ESC";
            controlsTxt.Location = new Point(175, 250);
            controlsTxt.AutoSize = true;
            controlsTxt.Font = new Font("Verdana", 25);
            controlsTxt.BackColor = Color.Black;
            controlsTxt.ForeColor = Color.White;
            this.form.Controls.Add(controlsTxt);
            controlsTxt.BringToFront();
            controlsTxt.Visible = false;
            ctrls.Add(controlsTxt);

            about.Text = "ABOUT";
            about.Location = new Point(150, 600);
            about.AutoSize = true;
            about.Font = new Font("Verdana", 25);
            about.BackColor = Color.Black;
            about.ForeColor = Color.White;
            about.Click += About_Click;
            this.form.Controls.Add(about);
            about.BringToFront();
            buttons.Add(about);

            aboutH.Text = "ABOUT";
            aboutH.Location = new Point(150, 150);
            aboutH.AutoSize = true;
            aboutH.Font = new Font("Verdana", 35);
            aboutH.BackColor = Color.Black;
            aboutH.ForeColor = Color.White;
            this.form.Controls.Add(aboutH);
            aboutH.BringToFront();
            aboutH.Visible = false;
            ctrls.Add(aboutH);

            aboutTxt.Text = "ClusterBot je druhý projekt do předmětu\n\"Řízení projektů\" na škole SPŠT, jedná se\no hru vytvořenou ve Windows forms\nv jazyce C# a byla vytvořena ve třech\ntýdenních sprintech. Podrobněji je to\narkádová top - down střílečka na skóre.\n\nVývojový tým:\nLukáš Kurtin - Kapitán, programátor, SFX\nJan Mátl - Programátor, web designer\nVojtěch Mastný - Grafický designer";
            aboutTxt.Location = new Point(175, 250);
            aboutTxt.AutoSize = true;
            aboutTxt.Font = new Font("Verdana", 20);
            aboutTxt.BackColor = Color.Black;
            aboutTxt.ForeColor = Color.White;
            this.form.Controls.Add(aboutTxt);
            aboutTxt.BringToFront();
            aboutTxt.Visible = false;
            ctrls.Add(aboutTxt);

            back.Text = "BACK";
            back.Location = new Point(725, 750);
            back.AutoSize = true;
            back.Font = new Font("Verdana", 25);
            back.BackColor = Color.Black;
            back.ForeColor = Color.White;
            back.Click += Back_Click;
            this.form.Controls.Add(back);
            back.BringToFront();
            back.Visible = false;
            ctrls.Add(back);
        }

        private void MenuPic_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            g.FillRectangle(Brushes.Black, 0, 0, menuPic.Width, menuPic.Height);
        }

        private void playBtn_click(object sender, EventArgs e)
        {
            Hide();
        }

        private void ResBtn_Click(object sender, EventArgs e)
        {
            Application.Restart();
        }

        private void CtrlBtn_Click(object sender, EventArgs e)
        {
            HideMain();
            controlsH.Visible = true;
            controlsTxt.Visible = true;
        }

        private void About_Click(object sender, EventArgs e)
        {
            HideMain();
            aboutH.Visible = true;
            aboutTxt.Visible = true;
        }

        private void HideMain()
        {
            form.KeyPreview = false;
            foreach (Button btn in buttons)
                btn.Visible = false;
            back.Visible = true;
        }

        private void Back_Click(object sender, EventArgs e)
        {
            form.KeyPreview = true;
            foreach (Button btn in buttons)
                btn.Visible = true;
            foreach (Control ctrl in ctrls)
                ctrl.Visible = false;
        }

        public void Show()
        {
            foreach (Button btn in buttons)
                btn.Visible = true;
            menuPic.Visible = true;
            timer.Stop();
            visible = true;
        }

        public void Hide()
        {
            foreach (Button btn in buttons)
                btn.Visible = false;
            menuPic.Visible = false;
            timer.Start();
            visible = false;
        }
    }
}
