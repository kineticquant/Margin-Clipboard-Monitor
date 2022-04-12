using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;

namespace Margin
{
    public partial class Form1 : Form
    {

        [DllImport("User32.dll", CharSet = CharSet.Auto)]
        public static extern IntPtr SetClipboardViewer(IntPtr hWndNewViewer);

        [DllImport("User32.dll", CharSet = CharSet.Auto)]
        public static extern bool ChangeClipboardChain(IntPtr hWndRemove, IntPtr hWndNewNext);

        private const int WM_DRAWCLIPBOARD = 0x0308;        // WM_DRAWCLIPBOARD message
        private IntPtr _clipboardViewerNext;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            _clipboardViewerNext = SetClipboardViewer(this.Handle);
            richTextBox1.Clear();
        }

        protected override void WndProc(ref Message m)
        {
            base.WndProc(ref m);    // Process the message 

            if (m.Msg == WM_DRAWCLIPBOARD)
            {
                IDataObject iData = Clipboard.GetDataObject();      // Clipboard's data

                if (iData.GetDataPresent(DataFormats.Text))
                {
                    string text = (string)iData.GetData(DataFormats.Text);      // Clipboard text
                    richTextBox1.AppendText(String.Format(Environment.NewLine + " "));
                    richTextBox1.AppendText(String.Format(Environment.NewLine + "----------"));
                    richTextBox1.AppendText(String.Format(Environment.NewLine + "Run Time:" + DateTime.Now));
                    richTextBox1.AppendText(String.Format(Environment.NewLine + " "));
                    richTextBox1.AppendText(Environment.NewLine + text);
                    richTextBox1.AppendText(String.Format(Environment.NewLine + " "));
                    richTextBox1.AppendText(String.Format(Environment.NewLine + "----------"));
                    richTextBox1.AppendText(String.Format(Environment.NewLine + " "));

                    richTextBox1.SelectionStart = richTextBox1.Text.Length;
                    richTextBox1.ScrollToCaret();
                }
                else if (iData.GetDataPresent(DataFormats.Bitmap))
                {
                    Bitmap image = (Bitmap)iData.GetData(DataFormats.Bitmap);   // Clipboard image
                                                                                // do something with it
                }
            }
        }


        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            ChangeClipboardChain(this.Handle, _clipboardViewerNext);        // Removes our from the chain of clipboard viewers when the form closes.
        }

        private void richTextBox_TextChange(object sender, EventArgs e)
        {
            //richTextBox1.SelectionStart = richTextBox1.Text.Length;
            //richTextBox1.ScrollToCaret();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            richTextBox1.Clear();
        }
    }
}
