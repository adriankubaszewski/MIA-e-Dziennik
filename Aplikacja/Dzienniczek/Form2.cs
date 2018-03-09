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
    public partial class Form2 : Form
    {
        string login;

        string server = "localhost";
        string database = "projbddziennik";
        string uid = "projbddziennik";
        string password = "1234";

        int liczbaprzedmiotow = 0;

        List<string> przedmioty = new List<string>();

        public Form2(string kto)
        {
            InitializeComponent();
           

            label1.Text += kto;
            login = kto;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            //this.dataGridView1.Rows.Add("Matematyka", "3, 5, 6", "4,5");
            SQLInformacje();
            SQLPrzedmioty();
            SQLOceny(liczbaprzedmiotow);
            SQLWiadomosci();
            
        }

        
        public void SQLPrzedmioty()
        {
            MySqlConnection connection = new MySqlConnection("SERVER=" + server + ";Port=3306;" + "DATABASE=" + database + ";" + "UID=" + uid + ";" + "PASSWORD=" + password + ";");

            MySqlCommand cmd = new MySqlCommand("SELECT * FROM przedmioty", connection);
            connection.Open();

            MySqlDataReader dataReader = cmd.ExecuteReader();
            int i = 0;
            while (dataReader.Read())
            {
                przedmioty.Add(dataReader["NazwaPrzedmiotu"] + "");
                i++;

            }
            liczbaprzedmiotow = i;
            dataReader.Close();
            connection.Close();

        }

        public void SQLOceny(int liczbaprzedmiotow)
        {
            MySqlConnection connection = new MySqlConnection("SERVER=" + server + ";Port=3306;" + "DATABASE=" + database + ";" + "UID=" + uid + ";" + "PASSWORD=" + password + ";");

            connection.Open();

            List<string> oceny = new List<string>();
           
            for (int i = 0; i < liczbaprzedmiotow; i++)
            {
                MySqlCommand cmd = new MySqlCommand("SELECT o.ocena, o.Opis, o.DataGodzina FROM oceny o WHERE EXISTS ( SELECT * FROM uczniowie u WHERE u.username ='" + login + "' AND u.ID_Ucznia = o.ID_Ucznia AND EXISTS (SELECT * FROM przedmioty p WHERE p.NazwaPrzedmiotu = '" + przedmioty[i] + "' AND p.ID_Przedmiotu=o.ID_Przedmiotu))", connection);

                MySqlDataReader dataReader = cmd.ExecuteReader();

                string ocenyy = "";
                double srednia = 0;
                int liczbaocen = 0;

                while (dataReader.Read())
                {
                    oceny.Add(dataReader["Ocena"] + "");
                    //oceny.Add(dataReader["DataGodzina"] + "");
                    //oceny.Add(dataReader["Opis"] + "");
                    srednia += Convert.ToInt32(dataReader["Ocena"]);
                    ocenyy += dataReader["Ocena"]+", ";
                    liczbaocen++;
                }
                dataGridView1.Rows.Add(przedmioty[i] + "", ocenyy, srednia/liczbaocen + "");
                liczbaocen = 0;
                dataReader.Close();
            }
            
            connection.Close();
        }

        public void SQLInformacje()
        {

            MySqlConnection connection = new MySqlConnection("SERVER=" + server + ";Port=3306;" + "DATABASE=" + database + ";" + "UID=" + uid + ";" + "PASSWORD=" + password + ";Convert Zero Datetime=True;");

            MySqlCommand cmd = new MySqlCommand("SELECT * FROM uczniowie WHERE username='" + login + "'", connection);

            connection.Open();

            MySqlDataReader dataReader = cmd.ExecuteReader();

            dataReader.Read();

            label3.Text += "  "+ dataReader["Imie"] + "";
            label4.Text += "  " + dataReader["Nazwisko"] + "";
            label5.Text += "  " + dataReader["Miasto"] + "";
            label6.Text += "  " + dataReader["Adres"] + "";
            label7.Text += "  " + dataReader["Pesel"] + "";
            label8.Text += "  " + dataReader["DataUr"] + "";
            label9.Text += "  " + dataReader["ID_Ucznia"] + "";

            dataReader.Close();

            connection.Close();
           
        }

        public void SQLWiadomosci()
        {
            MySqlConnection connection = new MySqlConnection("SERVER=" + server + ";Port=3306;" + "DATABASE=" + database + ";" + "UID=" + uid + ";" + "PASSWORD=" + password + ";Convert Zero Datetime=True;");

            connection.Open();

            List<string> wiadomosci = new List<string>();
            
            MySqlCommand cmd = new MySqlCommand("SELECT n.Imie, n.Nazwisko, w.tresc, w.id_nadawcy, w.data, w.temat, w.id_wiadomosci FROM wiadomosci w JOIN nauczyciele n on (n.id_nauczyciela=w.id_nadawcy) WHERE EXISTS ( SELECT * FROM uczniowie u WHERE u.username ='" + login + "' AND u.ID_Ucznia = w.id_odbiorcy)", connection);

            MySqlDataReader dataReader = cmd.ExecuteReader();

            while (dataReader.Read())
            {
                wiadomosci.Add(dataReader["id_wiadomosci"] + "");

                string tresc = dataReader["tresc"]+"";
                string data = dataReader["data"]+"";
                string imie = dataReader["Imie"]+"";
                string nazwisko = dataReader["Nazwisko"]+"";
                string temat = dataReader["temat"]+"";

                dataGridView2.Rows.Add(imie + nazwisko + "", temat + "",tresc + "",data + "");
                    
            }
                
            dataReader.Close();
            connection.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
