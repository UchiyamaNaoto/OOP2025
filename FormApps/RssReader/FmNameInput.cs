using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RssReader {
    public partial class FmNameInput : Form {
        Form1 parent;
        public FmNameInput(Form1 parent) {
            InitializeComponent();
            this.parent = parent;
        }

        private void btResist_Click(object sender, EventArgs e) {
            parent.inputName = tbNameText.Text;
            this.Close();
        }

        private void FmNameInput_Load(object sender, EventArgs e) {

        }

        private void btCancel_Click(object sender, EventArgs e) {
            this.Close();
        }
    }
}
