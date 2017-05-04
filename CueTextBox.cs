using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;

// a lot of help from http://social.msdn.microsoft.com/forums/en-US/csharpgeneral/thread/a07c453a-c5dd-40ed-8895-6615cc808d91/
namespace AdamOneilSoftware.Controls
{
    public class CueTextBox: TextBox
    {
        private string _cueText = null;

        // P/Invoke
        [DllImport("user32.dll", EntryPoint = "SendMessageW")]
        private static extern IntPtr SendMessage(IntPtr hWnd, int msg, IntPtr wp, IntPtr lp);

        public string CueText
        {
            get { return _cueText; }
            set
            {
                _cueText = value;
                UpdateCue();
            }
        }

        private void UpdateCue()
        {
            if (!this.IsHandleCreated || string.IsNullOrEmpty(_cueText)) return;
            IntPtr mem = Marshal.StringToHGlobalUni(_cueText);
            SendMessage(this.Handle, 0x1501, (IntPtr)1, mem);
            Marshal.FreeHGlobal(mem);
        }

        protected override void OnHandleCreated(EventArgs e)
        {
            base.OnHandleCreated(e);
            UpdateCue();
        }
    }
}
