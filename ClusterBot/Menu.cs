using System;
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
        Button playBtn = new Button();
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

            playBtn.Text = "PLAY";
            playBtn.Location = new Point(150, 450);
            playBtn.AutoSize = true;
            playBtn.Font = new Font("Verdana", 25);
            playBtn.BackColor = Color.Black;
            playBtn.ForeColor = Color.White;
            playBtn.Click += playBtn_click;
            this.form.Controls.Add(playBtn);
            this.form.Controls.Add(menuPic);
            menuPic.BringToFront();
            playBtn.BringToFront();
            buttons.Add(playBtn);
        }

        private void MenuPic_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            g.FillRectangle(Brushes.Black, 0, 0, 1000, 1000);
        }

        private void playBtn_click(object sender, EventArgs e)
        {
            Hide();
        }

        public void Show()
        {
            foreach (Button btn in buttons)
            {
                btn.Visible = true;
            }
            menuPic.Visible = true;
            timer.Stop();
            visible = true;
        }

        public void Hide()
        {
            foreach (Button btn in buttons)
            {
                btn.Visible = false;
            }
            menuPic.Visible = false;
            timer.Start();
            visible = false;
        }
    }
}
