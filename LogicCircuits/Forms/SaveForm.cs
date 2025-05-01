using LogicCircuits.Elements;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LogicCircuits.Forms
{
    public static partial class FormsBuilder
    {
        public static Form GetSaveForm(List<IElement> circuit)
        {
            Form form = new Form
            {
                StartPosition = FormStartPosition.CenterScreen,
                Icon = Properties.Resources.diagram,
                Text = "Зберегти як",
                Width = 300,
                Height = 66,
                FormBorderStyle = FormBorderStyle.FixedSingle,
                MaximizeBox = false,
                Padding = new Padding(0, 0, 0, 0)
            };

            TextBox textBox = new TextBox
            {
                Dock = DockStyle.Fill,
                Font = new Font(FontFamily.GenericSansSerif, 10, FontStyle.Regular),
                Text = "новаСхема1",
                Multiline= true,
            };
            form.Controls.Add(textBox);

            Button button = new Button
            {
                Dock = DockStyle.Right,
                Width = 80,
                FlatStyle = FlatStyle.Flat,
                Font = new Font(FontFamily.GenericSansSerif, 10, FontStyle.Regular),
                Text = "Зберегти",
            };

            button.Click+= (s, e) => {
                Serialization.SerializeCircuit(textBox.Text, DateTime.Now, circuit);
                form.Close();
            };

            form.Controls.Add(button);

            return form;
        }
    }
}
