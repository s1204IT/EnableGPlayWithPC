using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Security.Permissions;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EnableGPlayWithPC
{
    public partial class Progress : Form
    {
        private ProgressBar progressBar1;
        private Label label2;
        private Label label1;

        public Progress()
        {
            InitializeComponent();
        }

        public string Title
        {
            get { return this.Text; }
            set
            {
                if (InvokeRequired)
                {
                    this.Invoke((MethodInvoker)delegate () { this.Text = value; });
                }
                else
                {
                    this.Text = value;
                }
            }
        }
        public string Message
        {
            get { return label1.Text; }
            set
            {
                if (InvokeRequired)
                {
                    this.Invoke((MethodInvoker)delegate () { label1.Text = value; });
                }
                else
                {
                    label1.Text = value;
                }
            }
        }
        public int Value
        {
            get { return progressBar1.Value; }
            set
            {
                double percent = (double)(this.Value - 0)
                    / (double)(100 - 0);
                string displayText = string.Format("{0}%", (int)(percent * 100.0));
                label2.Text = displayText;

                if (InvokeRequired)
                {
                    this.Invoke((MethodInvoker)delegate () { progressBar1.Value = value; });
                }
                else
                {
                    progressBar1.Value = value;
                }
            }
        }
        public BackgroundWorker Worker
        {
            get;
            set;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Worker.CancelAsync();
        }

        private void InitializeComponent()
        {
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // progressBar1
            // 
            this.progressBar1.Location = new System.Drawing.Point(12, 82);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(260, 31);
            this.progressBar1.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.Font = new System.Drawing.Font("MS UI Gothic", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(260, 39);
            this.label1.TabIndex = 1;
            this.label1.Text = "label1";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label2
            // 
            this.label2.Font = new System.Drawing.Font("MS UI Gothic", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.label2.Location = new System.Drawing.Point(12, 48);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(260, 31);
            this.label2.TabIndex = 2;
            this.label2.Text = "label2";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // Progress
            // 
            this.ClientSize = new System.Drawing.Size(284, 125);
            this.ControlBox = false;
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.progressBar1);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Progress";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.TopMost = true;
            this.ResumeLayout(false);

        }
    }
}