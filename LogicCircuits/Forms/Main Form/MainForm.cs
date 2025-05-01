using LogicCircuits.Elements;
using LogicCircuits.Elements.Gates;
using LogicCircuits.Elements.Interfaces;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;


namespace LogicCircuits
{
    public partial class MainForm : Form
    {
        private GateInfo[] gateInfos;
        public static List<IElement> draft = new List<IElement>();

        public MainForm()
        {
            InitializeComponent();
            gateInfos = new GateInfo[9] { Elements.Gates.Buffer.GetInfo(), NOT.GetInfo(), AND.GetInfo(),
                OR.GetInfo(), NAND.GetInfo(), NOR.GetInfo(), XOR.GetInfo(), XNOR.GetInfo(), IMPLY.GetInfo()};
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            RenderCompletely();
            SetStatusLabel(ready);
        }

        private void AddElement(int tag)
        {
            IElement element = null;
            switch (tag)
            {
                case 1:
                    element = new Elements.Gates.Buffer();
                    break;
                case 2:
                    element = new NOT();
                    break;
                case 3:
                    element = new AND();
                    break;
                case 4:
                    element = new OR();
                    break;
                case 5:
                    element = new NAND();
                    break;
                case 6:
                    element = new NOR();
                    break;
                case 7:
                    element = new XOR();
                    break;
                case 8:
                    element = new XNOR();
                    break;
                case 9:
                    element = new IMPLY();
                    break;
                case 10:
                    string prefix = null;
                    if (radioButtonInputAuto.Checked)
                        prefix = "X1";
                    if (radioButtonInputCustom.Checked)
                        prefix = textBoxInput.Text;

                    int indexer = 1;
                    string name = prefix;
                    bool unique = NameUnique(name);
                    while (!unique)
                    {
                        if (radioButtonInputAuto.Checked)
                            name = "X" + indexer;
                        else
                            name = prefix + indexer;

                        indexer++;
                        unique = NameUnique(name);
                    }
                    element = new Input(name);
                    break;
                case 11:
                    string prefix2 = null;
                    if (radioButtonOutputAuto.Checked)
                        prefix2 = "Y";
                    if (radioButtonOutputCustom.Checked)
                        prefix2 = textBoxOutput.Text;

                    int indexer2 = 1;
                    string name2 = prefix2;
                    bool unique2 = NameUnique(name2);
                    while (!unique2)
                    {
                        name2 = prefix2 + indexer2;
                        indexer2++;
                        unique2 = NameUnique(name2);
                    }
                    element = new Output(name2);
                    break;
            }

            Point newLoc = panelCanvas.PointToClient(Cursor.Position);
            if (element is IGate)
                newLoc.X += 15;
            element.Location = newLoc.X < 37 && newLoc.Y < 33 ? new Point(37, 33) : newLoc.X < 37 ? new Point(37, newLoc.Y) : newLoc.Y < 33 ? new Point(newLoc.X, 33) : newLoc;

            draft.Add(element);
            if (element is Output)
                panelParams.Controls[0].Enabled = false;
            RenderNewElement(element);
        }

        private bool NameUnique(string name)
        {
            for (int i = 0; i < draft.Count; i++)
                if (draft[i] is Input input)
                    if (input.Name == name)
                        return false;
                else if (draft[i] is Output output)
                    if (output.Name == name)
                        return false;
            return true;
        }

        protected override void WndProc(ref Message m)
        {
            if (m.Msg == 0x128) return;
            base.WndProc(ref m);
        }

        private void MainForm_SizeChanged(object sender, EventArgs e)
        {
            RenderCompletely();
        }
    }
}
