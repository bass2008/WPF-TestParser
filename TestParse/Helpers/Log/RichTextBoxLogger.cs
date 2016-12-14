using System;
using System.Windows.Controls;

namespace TestParse.Helpers.Log
{
    public class RichTextBoxLogger : ILogger
    {
        private readonly RichTextBox _richTextBox;

        public RichTextBoxLogger(RichTextBox richTextBox)
        {
            _richTextBox = richTextBox;
        }

        public void Message(string str)
        {
            _richTextBox.AppendText("\n" + DateTime.Now.ToString("hh:mm:ss") + " " + str);
        }
    }
}
