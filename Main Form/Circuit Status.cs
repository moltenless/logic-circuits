using LogicCircuits.Elements;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LogicCircuits
{
    public partial class MainForm : Form
    {
        bool ready = false;
        private void CheckStatus()
        {
            bool containsOutput = ContainsOutput();
            if (!containsOutput)
            {
                ready = false;
                return;
            }
        }

        private bool ContainsOutput()
        {
            for (int i = 0; i < draft.Count; i++)
            {
                if (draft[i] is Output)
                {
                    return true;
                }
            }
            return false;
        }
    }
}
