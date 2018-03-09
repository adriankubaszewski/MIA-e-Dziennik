using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
//using System.Data.SqlClient;
using System.Security.Cryptography;
using MySql.Data.MySqlClient;


namespace Dzienniczek
{
    public partial class Form1 : Form
    {
        string server = "localhost";
        string database = "projbddziennik";
        string uid = "projbddziennik";
        string password = "1234";

        public Form1()
        {
            InitializeComponent();
        }

        public static string MD5Hash(string input)
        {
            StringBuilder hash = new StringBuilder();
            MD5CryptoServiceProvider md5provider = new MD5CryptoServiceProvider();
            byte[] bytes = md5provider.ComputeHash(new UTF8Encoding().GetBytes(input));

            for (int i = 0; i < bytes.Length; i++)
            {
                hash.Append(bytes[i].ToString("x2"));
            }
            return hash.ToString();
        }


        void sprawdzanieLogowania()
        {
            string login = "";
            string haslo = "";

            login = textBox1.Text;
            haslo = textBox2.Text;

            if (String.IsNullOrEmpty(textBox1.Text) || String.IsNullOrEmpty(textBox2.Text))
            {
                labelBlad.Text = "Nie wpisano danych logowania";
            }
            else
            {           
                MySqlConnection connection = new MySqlConnection("SERVER=" + server + ";Port=3306;" + "DATABASE=" + database + ";" + "UID=" + uid + ";" + "PASSWORD=" + password + ";");

                MySqlCommand cmdUczen = new MySqlCommand("SELECT * FROM uczniowie WHERE username='" + login + "' AND password='" + MD5Hash(haslo) + "'", connection);
                
                MySqlCommand cmdNauczyciel = new MySqlCommand("SELECT * FROM nauczyciele WHERE username='" + login + "' AND password='" + MD5Hash(haslo) + "'", connection);

                MySqlCommand cmdAdmin = new MySqlCommand("SELECT * FROM administrator WHERE username='" + login + "' AND password='" + MD5Hash(haslo) + "'", connection);

                connection.Open();

                char znakUser = login[login.Length - 1];

                if(znakUser == 'u')
                {
                    MySqlDataReader dataRead = cmdUczen.ExecuteReader();

                    if (dataRead.HasRows == true)
                    {
                        Form2 form2 = new Form2(login);
                        dataRead.Close();
                        connection.Close();
                        form2.Show();

                    }
                    else
                    {
                        MessageBox.Show("Blad");
                    }
                }
                else if (znakUser == 'n')
                {
                    MySqlDataReader dataRead = cmdNauczyciel.ExecuteReader();

                    if (dataRead.HasRows == true)
                    {
                        Form3 form3 = new Form3(login);
                        dataRead.Close();
                        connection.Close();
                        form3.Show();

                    }
                    else
                    {
                        MessageBox.Show("Blad");
                    }
                }
                else if (login[0] == 'a' && login[1] == 'd')
                {
                    MySqlDataReader dataRead = cmdAdmin.ExecuteReader();

                    if (dataRead.HasRows == true)
                    {
                        Form4 form4 = new Form4(login);
                        dataRead.Close();
                        connection.Close();
                        form4.Show();

                    }
                    else
                    {
                        MessageBox.Show("Blad");
                    }
                }
                

                if(login=="uczen")
                {
                    Form2 form2 = new Form2(login);

                    form2.Show();
                 
                }
                
            }

        }

        private void button1_Click(object sender, EventArgs e)
        {
            sprawdzanieLogowania();
        }

        private void button2_Click(object sender, EventArgs e)
        {

        }
    }
}
