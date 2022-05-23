using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Module2
{
    class MainForm
    {
        static void Main(string[] args)
        {
            {
                Form main = new Form();   //Used to create a nice application view comingsoon!!

                TextBox welcomeScreen = new TextBox()
                {
                    Text = "Welcome to my first windows app! V.1"
                };
                Button startButton = new Button()
                {
                    Text = "Hello",
                    Location = new System.Drawing.Point(10, 10)
                };
                startButton.Click += (o, s) =>
                    {
                        Module2.Program.Main(Directory.GetCurrentDirectory());
                };
                main.Controls.Add(welcomeScreen);
                main.Controls.Add(startButton);
                main.ShowDialog();
            }
        }
    }
}
