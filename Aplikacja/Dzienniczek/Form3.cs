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

namespace Dzienniczek
{
    public partial class Form3 : Form
    {
        string login;

        string server = "localhost";
        string database = "projbddziennik";
        string uid = "projbddziennik";
        string password = "1234";

        List<string> uczniowie = new List<string>();
        List<string> przedmioty = new List<string>();

        int ID_Naucz;

        public Form3(string kto)
        {
            InitializeComponent();

            label1.Text += kto;
            login = kto;

            idNauczyciela();
            wczytajPrzedmioty();
            wczytajUczniow();
            SQLInformacje();
        }

        public void idNauczyciela()
        {
            MySqlConnection connection = new MySqlConnection("SERVER=" + server + ";Port=3306;" + "DATABASE=" + database + ";" + "UID=" + uid + ";" + "PASSWORD=" + password + ";");

            MySqlCommand cmd = new MySqlCommand("SELECT * FROM nauczyciele WHERE username='" + login + "'", connection);
            connection.Open();

            MySqlDataReader dataReader = cmd.ExecuteReader();
            while (dataReader.Read())
            {
                ID_Naucz = Convert.ToInt32(dataReader["ID_Nauczyciela"]);  
            }
            dataReader.Close();
            connection.Close();
        }

        public void wczytajPrzedmioty()
        {

            MySqlConnection connection = new MySqlConnection("SERVER=" + server + ";Port=3306;" + "DATABASE=" + database + ";" + "UID=" + uid + ";" + "PASSWORD=" + password + ";");

            MySqlCommand cmd = new MySqlCommand("SELECT * FROM przedmioty ORDER BY NazwaPrzedmiotu ASC", connection);
            connection.Open();

            MySqlDataReader dataReader = cmd.ExecuteReader();
            int i = 0;
            while (dataReader.Read())
            {
                comboBox1.Items.Add(dataReader["NazwaPrzedmiotu"]);
                przedmioty.Add(dataReader["ID_Przedmiotu"] + "");
            }
            dataReader.Close();
            connection.Close();
        }

        public void wczytajUczniow()
        {
            MySqlConnection connection = new MySqlConnection("SERVER=" + server + ";Port=3306;" + "DATABASE=" + database + ";" + "UID=" + uid + ";" + "PASSWORD=" + password + ";");

            MySqlCommand cmd = new MySqlCommand("SELECT u.ID_Ucznia, u.Imie, u.Nazwisko, k.nazwa FROM uczniowie u JOIN klasy k ON(u.id_klasy=k.id_klasy) ORDER BY k.nazwa ASC", connection);
            connection.Open();

            MySqlDataReader dataReader = cmd.ExecuteReader();
            int i = 0;
            while (dataReader.Read())
            {
                comboBox3.Items.Add(dataReader["nazwa"] + " | " + dataReader["Imie"] + " "+ dataReader["Nazwisko"]);
                uczniowie.Add(dataReader["ID_Ucznia"] + "");
               
            }
            dataReader.Close();
            connection.Close();
        }

        public void dodajOcene()
        {
            MySqlConnection connection = new MySqlConnection("SERVER=" + server + ";Port=3306;" + "DATABASE=" + database + ";" + "UID=" + uid + ";" + "PASSWORD=" + password + ";");

            

            if(comboBox1.SelectedItem == null || comboBox2.SelectedItem == null || comboBox3.SelectedItem == null)
            {
                MessageBox.Show("Uzupełnij wymagane pola");
            }
            else
            {
                string opis;
                if (textBox1.Text == "")
                {
                    opis = " - ";
                }
                else
                {
                    opis = textBox1.Text;
                }
                int ocena = Convert.ToInt32(comboBox2.Text);
                int ID_Przedmiotu = Convert.ToInt32(przedmioty[comboBox1.SelectedIndex]);
                int ID_Ucznia = Convert.ToInt32(uczniowie[comboBox3.SelectedIndex]);
                string data = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

                MySqlCommand cmd = new MySqlCommand("INSERT INTO oceny(Ocena, ID_Przedmiotu, ID_Nauczyciela, ID_Ucznia, Opis, DataGodzina) VALUES('"+ocena+"', '"+ID_Przedmiotu+"', '"+ID_Naucz+"','"+ID_Ucznia+"','"+opis+"','"+data+"')", connection);
                connection.Open();


                cmd.ExecuteNonQuery();

                connection.Close();

                MessageBox.Show("Ocena została dodana");
            }
           
        }

        private void button2_Click(object sender, EventArgs e)
        {
            dodajOcene();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        public void SQLInformacje()
        {

            MySqlConnection connection = new MySqlConnection("SERVER=" + server + ";Port=3306;" + "DATABASE=" + database + ";" + "UID=" + uid + ";" + "PASSWORD=" + password + ";Convert Zero Datetime=True;");

            MySqlCommand cmd = new MySqlCommand("SELECT * FROM nauczyciele WHERE username='" + login + "'", connection);

            connection.Open();

            MySqlDataReader dataReader = cmd.ExecuteReader();

            dataReader.Read();

            label14.Text += "  " + dataReader["Imie"] + "";
            label13.Text += "  " + dataReader["Nazwisko"] + "";
            label12.Text += "  " + dataReader["Miasto"] + "";
            label11.Text += "  " + dataReader["Adres"] + "";
            label10.Text += "  " + dataReader["Pesel"] + "";
            label8.Text += "  " + dataReader["DataUr"] + "";
            label9.Text += "  " + dataReader["ID_Nauczyciela"] + "";

            dataReader.Close();

            connection.Close();

        }
    }
}
