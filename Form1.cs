using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Rebar;

namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        Random rand = new Random();
        private void btnCalculateOrder_Click(object sender, EventArgs e)
        {
            string customerName = txtCustomerName.Text;

            if (customerName == "")
            {
                MessageBox.Show("Please enter the customer name.");
                return;
            }
            for (int i = 0; i < customerName.Length; i++)
            {
                if (customerName[i] >= '0' && customerName[i] <= '9')
                {
                    MessageBox.Show("Name cannot contain numbers!");
                    return;
                }
            }
            int quantity;

            if (!int.TryParse(txtQuantity.Text, out  quantity))
            {
                MessageBox.Show("Please enter a valid quantity.");
                return;
            }

            if (quantity < 1)
            {
                MessageBox.Show("Quantity must be at least 1 or greater 1.");
                return;
            }

            decimal drinkPrice = 0;
            string drinkName = "";

            if (rdoTea.Checked)
            {
                drinkPrice = 1;
                drinkName = "Tea";
            }
            else if (rdoCoffee.Checked)
            {
                drinkPrice = 2;
                drinkName = "Coffee";
            }
            else if (rdoJuice.Checked)
            {
                drinkPrice = 3;
                drinkName = "Juice";
            }
            else
            {
                MessageBox.Show("Please select a drink type.");
                return;
            }

            lstOrderList.Items.Clear();

            decimal totalPrice = drinkPrice * quantity;

            if (chkStudentMember.Checked)
            {
                totalPrice = totalPrice * 0.90m;
            }

            bool gotFreeCookie = false;

            for (int i = 1; i <= quantity; i++)
            {
                int luckyNumber = rand.Next(1, 51);

                if (luckyNumber == 7 || luckyNumber == 17)
                {
                    gotFreeCookie = true;
                }

                string orderLine = i + ") " + customerName +
                                   " - " + drinkName +
                                   " - Lucky: " + luckyNumber +
                                   " - $" + drinkPrice.ToString("F2");

                lstOrderList.Items.Add(orderLine);
            }

            lblTotalPrice.Text = "Total Price: " + totalPrice.ToString("C");

            if (gotFreeCookie)
            {
                lblLuckyMessage.Text = "Lucky message: You got a Free Cookie!";
            }
            else
            {
                lblLuckyMessage.Text = "Lucky message: No prize this time.";
            }

        }

        private void btnSaveToFile_Click(object sender, EventArgs e)
        {
            if (lstOrderList.Items.Count == 0)
            {
                MessageBox.Show("No order to save.");
                return;
            }

            StreamWriter outputFile = null;

            try
            {
                outputFile = File.AppendText("orders.txt");

                for (int i = 0; i < lstOrderList.Items.Count; i++)
                {
                    outputFile.WriteLine(lstOrderList.Items[i].ToString());
                }

                outputFile.WriteLine(lblTotalPrice.Text);
                outputFile.WriteLine(lblLuckyMessage.Text);

                outputFile.WriteLine("------------------------------");

                outputFile.Close();
                outputFile = null;

                MessageBox.Show("Order saved.");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error saving file: " + ex.Message);
            }
            finally
            {
                if (outputFile != null)
                {
                    outputFile.Close();
                }
            }
        }

        private void btnLoadFromFile_Click(object sender, EventArgs e)
        {
            if (!File.Exists("orders.txt"))
            {
                MessageBox.Show("No file found.");
                return;
            }

            StreamReader inputFile = null;

            try
            {
                lstOrderList.Items.Clear();

                inputFile = File.OpenText("orders.txt");

                while (!inputFile.EndOfStream)
                {
                    string line = inputFile.ReadLine();

                    lstOrderList.Items.Add(line);
                }

                inputFile.Close();
                inputFile = null;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading file: " + ex.Message);
            }
            finally
            {
                if (inputFile != null)
                {
                    inputFile.Close();
                }
            }
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            txtCustomerName.Clear();
            txtQuantity.Clear();

            lstOrderList.Items.Clear();

            lblTotalPrice.Text = "Total Price: $0.00";
            lblLuckyMessage.Text = "Lucky message:";

            chkStudentMember.Checked = false;

            rdoTea.Checked = false;
            rdoCoffee.Checked = false;
            rdoJuice.Checked = false;

           // File.Delete("orders.txt");

            txtCustomerName.Focus();
        }
    }
}
