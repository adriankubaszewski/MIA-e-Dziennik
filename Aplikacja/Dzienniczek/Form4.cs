using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using System.Security.Cryptography;

namespace Dzienniczek
{
    public partial class Form4 : Form
    {
        string login;

        string server = "localhost";
        string database = "projbddziennik";
        string uid = "projbddziennik";
        string password = "1234";

        int nrUcznia = 0;
        int nrNauczyciela = 0;

        List<string> klasy = new List<string>();

        public Form4(string kto)
        {
            InitializeComponent();

            label1.Text += kto;
            login = kto;

            wczytajKlasy();
            ostatniLoginUcznia();
            ostatniLoginNauczyciela();
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

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        void dodajUcznia()
        {
            MySqlConnection connection = new MySqlConnection("SERVER=" + server + ";Port=3306;" + "DATABASE=" + database + ";" + "UID=" + uid + ";" + "PASSWORD=" + password + ";Convert Zero Datetime=True;");

            if (comboBox1.SelectedItem == null || textBox1.Text == "" || textBox2.Text == "" || textBox3.Text == "" || textBox4.Text == "" || textBox5.Text == "" || textBox6.Text == "" || textBox7.Text == "" || textBox8.Text == "")
            {
                MessageBox.Show("Uzupełnij wymagane pola");
            }
            else
            {
                if(textBox7.Text == textBox8.Text)
                {
                    MySqlCommand cmd = new MySqlCommand("INSERT INTO uczniowie(Imie, Nazwisko, Miasto, Adres, Pesel,DataUr,password,username, id_klasy) VALUES('" + textBox1.Text + "', '" + textBox2.Text + "', '" + textBox3.Text + "','" + textBox4.Text + "','" + textBox5.Text + "','" + dateTimePicker2.Value.ToShortDateString() + "','" + MD5Hash(textBox7.Text) + "','" + textBox6.Text + "', '" + Convert.ToInt32(klasy[comboBox1.SelectedIndex]) + "')", connection);
                    connection.Open();

                    cmd.ExecuteNonQuery();

                    connection.Close();

                    MessageBox.Show("Uczeń został dodany");
                    nrUcznia++;
                }
                else
                {
                    MessageBox.Show("Sprwdź podane hasło");
                }
                
            }
        }

        void dodajNauczyciela()
        {
            MySqlConnection connection = new MySqlConnection("SERVER=" + server + ";Port=3306;" + "DATABASE=" + database + ";" + "UID=" + uid + ";" + "PASSWORD=" + password + ";Convert Zero Datetime=True;");

            if (textBox18.Text == "" || textBox17.Text == "" || textBox16.Text == "" || textBox15.Text == "" || textBox14.Text == "" || textBox13.Text == "" || textBox12.Text == "" || textBox11.Text == "")
            {
                MessageBox.Show("Uzupełnij wymagane pola");
            }
            else
            {
                if (textBox11.Text == textBox12.Text)
                {
                    MySqlCommand cmd = new MySqlCommand("INSERT INTO nauczyciele(Imie, Nazwisko, Pesel, DataUr, Adres, Miasto,password,username) VALUES('" + textBox18.Text + "', '" + textBox17.Text + "', '" + textBox14.Text + "','" + dateTimePicker1.Value.ToShortDateString() + "','" + textBox15.Text + "','" + textBox16.Text + "','" + MD5Hash(textBox12.Text) + "','"+textBox13.Text+"')", connection);
                    connection.Open();

                    cmd.ExecuteNonQuery();

                    connection.Close();

                    MessageBox.Show("Nauczyciel został dodany");
                    nrNauczyciela++;
                }
                else
                {
                    MessageBox.Show("Sprwdź podane hasło");
                }

            }
        }

        public void wczytajKlasy()
        {

            MySqlConnection connection = new MySqlConnection("SERVER=" + server + ";Port=3306;" + "DATABASE=" + database + ";" + "UID=" + uid + ";" + "PASSWORD=" + password + ";");

            MySqlCommand cmd = new MySqlCommand("SELECT * FROM klasy ORDER BY nazwa ASC", connection);
            connection.Open();

            MySqlDataReader dataReader = cmd.ExecuteReader();
            int i = 0;
            while (dataReader.Read())
            {
                comboBox1.Items.Add(dataReader["nazwa"]);
                klasy.Add(dataReader["id_klasy"] + "");
            }
            dataReader.Close();
            connection.Close();
        }

        public void ostatniLoginUcznia()
        {
            MySqlConnection connection = new MySqlConnection("SERVER=" + server + ";Port=3306;" + "DATABASE=" + database + ";" + "UID=" + uid + ";" + "PASSWORD=" + password + ";");

            MySqlCommand cmd = new MySqlCommand("SELECT * FROM uczniowie ORDER BY username ASC", connection);
            connection.Open();

            MySqlDataReader dataReader = cmd.ExecuteReader();
            string nick = "";
            while (dataReader.Read())
            {
                nick = dataReader["username"].ToString();

            }
            dataReader.Close();
            connection.Close();
            nrUcznia = Convert.ToInt32(nick.Substring(0, nick.Length - 1)) + 1;

            wypiszOstatniLoginUcznia();
            
        }

        public void ostatniLoginNauczyciela()
        {
            MySqlConnection connection = new MySqlConnection("SERVER=" + server + ";Port=3306;" + "DATABASE=" + database + ";" + "UID=" + uid + ";" + "PASSWORD=" + password + ";");

            MySqlCommand cmd = new MySqlCommand("SELECT * FROM nauczyciele ORDER BY username ASC", connection);
            connection.Open();

            MySqlDataReader dataReader = cmd.ExecuteReader();
            string nick = "";
            while (dataReader.Read())
            {
                nick = dataReader["username"].ToString();

            }
            dataReader.Close();
            connection.Close();
            nrNauczyciela = Convert.ToInt32(nick.Substring(0, nick.Length - 1)) + 1;

            wypiszOstatniLoginNauczyciela();

        }

        public void wypiszOstatniLoginUcznia()
        {
            if (nrUcznia < 10)
            {
                textBox6.Text = "0" + nrUcznia + "u";
            }
            else
            {
                textBox6.Text = nrUcznia + "u";
            }

            nrUcznia++;
        }

        public void wypiszOstatniLoginNauczyciela()
        {
            if (nrNauczyciela < 10)
            {
                textBox13.Text = "0" + nrNauczyciela + "n";
            }
            else
            {
                textBox13.Text = nrNauczyciela + "n";
            }

            
        }

        private void CzyscTextBox(Control.ControlCollection cc)
        {
         foreach (Control ctrl in cc)
         {
          TextBox tb = ctrl as TextBox;
          if (tb != null)
           tb.Text = "";
          else
           CzyscTextBox(ctrl.Controls);
         }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            dodajUcznia();

            CzyscTextBox(this.Controls);

            wypiszOstatniLoginUcznia();
            
        }

        private void button3_Click(object sender, EventArgs e)
        {
            dodajNauczyciela();

            CzyscTextBox(this.Controls);

            wypiszOstatniLoginNauczyciela();
        }
    }
}
