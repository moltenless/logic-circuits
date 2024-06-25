using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace LogicCircuits
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {

        }

        private void MenuButtonsMouseEnter(object sender, EventArgs e)
        {
            (sender as Control).Parent.BackColor = Color.LightGray;
            object tag = (sender as Control).Tag;
            if (tag != null && int.TryParse(tag.ToString(), out int gate))
            {
                string[] names = new string[9]{
                    "Buffer", "NOT or Inverter", "AND", "OR", "NAND (NOT-AND)", "NOR (NOT-OR)", "XOR (Exclusive OR)", "XNOR (Exclusive NOR)", "IMPLY or Material implication"                };
                Image[] formulas = new Image[9] {Properties.Resources.formula1, Properties.Resources.formula2, Properties.Resources.formula3,
                Properties.Resources.formula4, Properties.Resources.formula5, Properties.Resources.formula6,
                Properties.Resources.formula7, Properties.Resources.formula8, Properties.Resources.formula9};
                Image[] diagrams = new Image[9] { Properties.Resources.gate1, Properties.Resources.gate2, Properties.Resources.gate3,
                Properties.Resources.gate4, Properties.Resources.gate5, Properties.Resources.gate6,
                Properties.Resources.gate7, Properties.Resources.gate8, Properties.Resources.gate9 };
                Image[] tables = new Image[9] { Properties.Resources.table1, Properties.Resources.table2, Properties.Resources.table3,
                Properties.Resources.table4, Properties.Resources.table5, Properties.Resources.table6,
                Properties.Resources.table7, Properties.Resources.table8, Properties.Resources.table9};

                labelGateName.Text = names[gate - 1];
                pictureBoxFormula.Image = formulas[gate - 1];
                pictureBoxDiagram.Image = diagrams[gate - 1];
                pictureBoxGateTable.Image = tables[gate - 1];
            }
        }

        private void MenuButtonsMouseLeave(object sender, EventArgs e)
        {
            (sender as Control).Parent.BackColor = SystemColors.Control;

            labelGateName.Text = "<Назва вентиля>";
            if (pictureBoxFormula.Image != null) pictureBoxFormula.Image = null;
            if (pictureBoxDiagram != null) pictureBoxDiagram.Image = null;
            if (pictureBoxGateTable != null) pictureBoxGateTable.Image = null;
        }
    }
}
