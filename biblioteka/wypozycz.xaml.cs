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
        void dostepne_egzemplarze()
        {
            SqlCommand cmd = new SqlCommand(query, con);
            con.Open();
            SqlDataAdapter adapter = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            adapter.Fill(dt);
            do_wyporzyczenia.ItemsSource = dt.DefaultView;
            cmd.Dispose();
            con.Close();
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
