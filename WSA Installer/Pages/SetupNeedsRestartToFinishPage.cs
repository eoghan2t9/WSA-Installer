using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WSA_Installer.Pages
{
    public partial class SetupNeedsRestartToFinishPage : Page
    {
        public SetupNeedsRestartToFinishPage()
        {
            InitializeComponent();
            this.Tag = "SetupNeedsRestartToFinishPage";
            this.NoBanner = true;
            this.Size = new Size(541, 330);
        }
    }
}
