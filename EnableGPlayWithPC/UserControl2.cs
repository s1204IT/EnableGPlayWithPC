using System;
using System.Runtime.Remoting.Contexts;
using System.Windows.Forms;
using SharpAdbClient;

namespace EnableGPlayWithPC {
    public partial class UserControl2 : UserControl {
        static UserControl2 instance;

        public static UserControl2 getInstance() {
            return instance;
        }

        public UserControl2() {
            InitializeComponent();
            instance = this;
            progressBar1.Style = ProgressBarStyle.Marquee;
            textBox1.ReadOnly = true;
            button1.Text = "OK";
            button1.Enabled = false;
        }

        public void SetMessage(string str) {
            this.Invoke(new Action<string>(this.LabelText), str);
        }

        public void StopProgress() {
            AdbClient.Instance.KillAdb();
            this.Invoke(new Action(this.ProgressBar));
        }
        public void WriteLog(string logText) {
            this.Invoke(new Action<string>(this.WriteMessage), logText);
        }


        public void Button_Click(object sender, EventArgs e) {
            Main.getInstance().Cancel();
        }

        private void WriteMessage(string logText) {
            textBox1.SelectionStart = textBox1.Text.Length;
            textBox1.SelectionLength = 0;
            textBox1.SelectedText = "[" + DateTime.Now.ToString("HH:mm:ss") + "]  " + logText + "\r\n";
        }
        public void WriteLine() {
            textBox1.SelectionStart = textBox1.Text.Length;
            textBox1.SelectionLength = 0;
            textBox1.SelectedText = "\r\n";
        }

        private void ProgressBar() {
            progressBar1.Style = ProgressBarStyle.Blocks;
            progressBar1.Value = 0;
            button1.Enabled = true;
        }

        private void LabelText(string str) {
            label1.Text = str;
        }
    }
}