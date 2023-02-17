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


        public Menu(Form form)
        {
            playBtn.Name = "playBtn";
            playBtn.Text = "PLAY";
            playBtn.Location = new Point(300, 300);
            playBtn.AutoSize = true;
            playBtn.Font = new Font("Verdana", 25);
            playBtn.BackColor = Color.Black;
            playBtn.ForeColor = Color.White;
            playBtn.Click += playBtn_click;
            form.Controls.Add(playBtn);
            playBtn.BringToFront();
            buttons.Add(playBtn);
        }

        private void playBtn_click(object sender, EventArgs e)
        {
           
        }

        public void Show(Timer timer)
        {
            foreach (Button btn in buttons)
            {
                btn.Visible = true;
            }
            timer.Stop();
            visible = true;
        }   
        
        public void Hide(Timer timer)
        {
            foreach (Button btn in buttons)
            {
                btn.Visible = false;
            }
            timer.Start();
            visible = true;
        }
    }
}
