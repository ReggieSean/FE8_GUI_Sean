using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO.Ports;
using System.Data.SqlClient;
using System.IO;

namespace SerialMonitor_Test
{
    public partial class Form1 : Form

    {
        public Form1()
        {
            InitializeComponent();
            Application.VisualStyleState = System.Windows.Forms.VisualStyles.VisualStyleState.NoneEnabled;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //Sends the message and clears the textbox
            serialPort1.Write(textBox1.Text);
            textBox1.Text = "";
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (button2.Text == "Connect" && comboBox1.Text.Length != 0 && comboBox2.Text.Length != 0)
            {
                //Tries to open port 1
                serialPort1.PortName = comboBox1.Text;
                serialPort1.BaudRate = int.Parse(comboBox2.Text);
                serialPort1.Open();


                if (serialPort1.IsOpen)
                {
                    //Handles all the enabling
                    textBox1.Enabled = true;
                    button1.Enabled = true;

                    //Handles all the disabling
                    comboBox1.Enabled = false;
                    comboBox2.Enabled = false;

                    //Prints the Connection Successful message
                    textBox2.AppendText("\nCONNECTION SUCCESS");
                    serialPort1.DiscardInBuffer();

                    //Changes button 2 to disconnect
                    button2.Text = "Disconnect";
                }
                else
                {
                    // Prints the failed to connect message
                    textBox2.AppendText("FAILED TO CONNECT");
                }
            }
            else if(button2.Text == "Disconnect")
            {
                //Closes the serial port and notifies the user
                serialPort1.Close();
                button2.Text = "Connect";
                textBox2.AppendText("\nDISCONNECTED");

                //Handles the disabling
                textBox1.Enabled = false;
                button1.Enabled = false;
                

                //Handles all the enabling
                comboBox1.Enabled = true;
                comboBox2.Enabled = true;
            }

        }

        public void serialPort1_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {

            byte i = 0;
            string buffer = "";
            
            //While there's something in the serial buffer AND you've read less than 200 bytes
            while (serialPort1.BytesToRead > 1 && i < 200)
            {
                buffer = buffer + (char) serialPort1.ReadChar();
                i++;
            }

            //We're on a different thread right now, so we need to INVOKE the UI thread to update
            textBox2.BeginInvoke((MethodInvoker) delegate ()
            {
                textBox2.AppendText(buffer);
            });
        }

        private void comboBox1_DropDown(object sender, EventArgs e)
        {
            //Handels all the com port stuff
            string[] ports = { "" };
            ports = SerialPort.GetPortNames();
            comboBox1.Items.Clear();
            comboBox1.Text = "";

            if (ports.Length > 0)
            {
                comboBox1.Items.AddRange(ports);
                comboBox1.Text = ports[0];
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            textBox2.Clear();
        }

        // H
    }
}
