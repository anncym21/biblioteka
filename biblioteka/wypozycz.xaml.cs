using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Ribbon.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.Data;

namespace biblioteka
{
    /// <summary>
    /// Logika interakcji dla klasy wypozycz.xaml
    /// </summary>
    public partial class wypozycz : Window
    {
        string query = "select id, tytul, kategoria, id_autor as id__autor from dbo.ksiazka";
        string baza = "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=baza;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
        SqlConnection con;
        SqlCommand cmd;
        string id_k;

        public wypozycz()
        {
            InitializeComponent();
            con = new SqlConnection(baza);
            wczytaj_kategorie();
            dostepne_egzemplarze();
            wczytaj_wyporzyczone();
            wczytaj_historie();
        }
        void wczytaj_kategorie()
        {
            kategoria.Items.Add("lektura");
            kategoria.Items.Add("literatura");
            kategoria.Items.Add("dla dzieci");
            kategoria.Items.Add("horror");
        }
        void wczytaj_wyporzyczone()
        {
            string wyporzyczone = "SELECT ksiazka.id, ksiazka.tytul, ksiazka.kategoria, wyporzyczenia.data_wyp, wyporzyczenia.data_zwrot FROM ksiazka INNER JOIN egzemplarze ON ksiazka.id = egzemplarze.id_ksiazki INNER JOIN wyporzyczenia ON wyporzyczenia.id_egzemplarz = egzemplarze.id WHERE egzemplarze.do_wyporzyczenia like 'tak' and data_zwrot is null";
            cmd = new SqlCommand(wyporzyczone, con);
            con.Open();
            SqlDataAdapter adapter = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            adapter.Fill(dt);
            aktualnie_wyporzyczone.ItemsSource = dt.DefaultView;
            cmd.Dispose();
            con.Close();
        }
        void dostepne_egzemplarze(string wybrana_kategoria = "")
        {   if (wybrana_kategoria == ""){
               
                cmd = new SqlCommand(query, con);
            }
            else
            { 
                cmd = new SqlCommand("select id, tytul, kategoria, id_autor as id__autor from dbo.ksiazka where kategoria like '"+wybrana_kategoria+ "'", con);
            }
                con.Open();
                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                adapter.Fill(dt);
                do_wyporzyczenia.ItemsSource = dt.DefaultView;
                cmd.Dispose();
                con.Close();
           
        }


        private void kategoria_SelectionChanged_1(object sender, SelectionChangedEventArgs e)
        {
            string wybrana_kategoria = kategoria.Items.GetItemAt(kategoria.SelectedIndex).ToString();
            dostepne_egzemplarze(wybrana_kategoria);
           
        }

        private void do_wyporzyczenia_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            DataGrid dataGrid = sender as DataGrid;
            DataRowView rowView = dataGrid.SelectedItem as DataRowView;
            if(con.State != ConnectionState.Open)
            {
                con.Open();
            }
            int ile = 0;
            if(rowView != null)
            {
                
                id_k = rowView.Row[0].ToString();
                cmd = new SqlCommand("Select * from dbo.egzemplarze where id_ksiazki = " + id_k + "and do_wyporzyczenia like 'tak'", con);
                SqlDataReader czyt = cmd.ExecuteReader();
                while (czyt.Read())
                {
                    ile += 1;
                }

                ilosc_egzemplarzy.Content = "Ile dostępnych egzemplarzy.: " + ile.ToString();
            }
            
            
            con.Close();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
           
            string update_egzeplarz = "UPDATE TOP(1) dbo.egzemplarze SET do_wyporzyczenia = 'nie' WHERE id_ksiazki =" + id_k ;
            con.Open();
            cmd = new SqlCommand(update_egzeplarz, con);
            cmd.ExecuteScalar();
            con.Close();
            wczytaj_wyporzyczone();
            wczytaj_historie();
            MessageBox.Show("Książka wyporzyczona");
        }

        private void aktualnie_wyporzyczone_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            DataRowView wiersz = aktualnie_wyporzyczone.SelectedItem as DataRowView;
            
            do_wyporzyczenia1.Content = "Data wyporzyczenia.: " + wiersz.Row.ItemArray[3].ToString();
        }

        private void historia_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            DataGrid dataGrid = sender as DataGrid;
            DataRowView rowView = dataGrid.SelectedItem as DataRowView;
            if (con.State != ConnectionState.Open)
            {
                con.Open();
            }
            if (rowView != null)
            {
                cmd = new SqlCommand("Select * from dbo.wyporzyczenia where data_zwrot is not null");
                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                adapter.Fill(dt);
                historia.ItemsSource = dt.DefaultView;
                cmd.Dispose();

            }

            con.Close();
        }
        private void wczytaj_historie()
        {
            
            string historia_wyporzyczen = "Select * from dbo.wyporzyczenia where data_zwrot is not null";
            cmd = new SqlCommand(historia_wyporzyczen, con);
            con.Open();
            SqlDataAdapter adapter = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            adapter.Fill(dt);
            historia.ItemsSource = dt.DefaultView;
            cmd.Dispose();
            con.Close();
        }
    }
}
