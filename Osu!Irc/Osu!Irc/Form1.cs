using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;
using Meebey.SmartIrc4net;

namespace Osu_Irc
{
    public partial class Form1 : Form
    {
       
        int ErrorCode;
        public static IrcClient IRC = new IrcClient();
        private static Thread _ListenThread;
        string msg;
        public void set()
        {
            IRC.OnChannelMessage += IRC_OnChannelMessage;
            IRC.OnQueryAction += IRC_OnQueryAction;
            IRC.OnQueryMessage += IRC_OnQueryMessage;
            IRC.Encoding = Encoding.UTF8;
            
        }
        public void Start()
        {
            _ListenThread = new Thread(new ThreadStart(IRCThread));
            _ListenThread.Start();
        }
        private void IRC_OnQueryMessage(object sender, IrcEventArgs e)
        {
            Console.WriteLine("[" + System.DateTime.Now + "私聊" + "]" + e.Data.Nick + ":" + e.Data.Message);
            msg = msg + "[" + System.DateTime.Now + "私聊" + "]" + e.Data.Nick + ":" + e.Data.Message+"\n";
            textBox3.Text = msg;
        }
        private void IRC_OnQueryAction(object sender, ActionEventArgs e)
        {
            throw new NotImplementedException();
        }
        private void IRC_OnChannelMessage(object sender, IrcEventArgs e)
        {
            Console.WriteLine("[" + System.DateTime.Now + "来自" + e.Data.Channel + "]" + e.Data.Nick + ":" + e.Data.Message);
            msg = msg + "[" + System.DateTime.Now + "来自" + e.Data.Channel + "]" + e.Data.Nick + ":" + e.Data.Message+"\n";
            textBox3.Text = msg;
        }

        public static void send(string message, string Chanl)
        {

            IRC.SendMessage(SendType.Message, Chanl, message);
        }
        public void IRCsend(string Msg, string id)
        {
            IRC.SendMessage(SendType.Message, id, Msg);
        }
        public void IRCThread()
        {
            try

            {
                IRC.Listen();
            }
            catch { }
        }
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string id = textBox1.Text;
            string passoword = textBox2.Text;         
                try
                {
                    IRC.Connect("irc.ppy.sh", 6667);
                   
                ErrorCode = 0;
                }
                catch
                {
                    MessageBox.Show("服务器连接失败");
                ErrorCode = 1;
                }
            switch(ErrorCode)
            {
                case 1:
                    break;
                case 0:
                    this.set();
                    label10.Text = "Yes";
                    IRC.Login(id, id, 0, id, passoword);
                    IRC.RfcJoin("#lobby");
                    this.Start();
                    break;
                default:
                    this.set();
                    label10.Text = "Yes";
                    IRC.Login(id, id, 0, id, passoword);
                    IRC.RfcJoin("#lobby");
                    this.Start();
                    break;
            }                 
        }

        private void textBox4_TextChanged(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {
            IRC.RfcQuit();
            IRC.RfcJoin("#"+textBox4.Text);
            label6.Text = "#" + textBox4.Text;
            this.Start();
        }
    }
}
