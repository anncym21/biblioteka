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
        
        public wypozycz()
        {
            InitializeComponent();
            con = new SqlConnection(baza);
            wczytaj_kategorie();
            dostepne_egzemplarze();
        }
        void wczytaj_kategorie()
        {
            kategoria.Items.Add("lektura");
            kategoria.Items.Add("literatura");
            kategoria.Items.Add("dla dzieci");
            kategoria.Items.Add("horror");
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
            string id_k = rowView.Row[0].ToString();
            
            cmd = new SqlCommand("Select count(id) from dbo.egzemplarze where id = "+id_k, con);
            con.Open();
            ilosc_egzemplarzy.Content = "Ile dostępnych egzemplarzy.:" + cmd.ExecuteScalar().ToString();
            con.Close();
        }
    }
}
