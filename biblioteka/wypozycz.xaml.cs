﻿using System;
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
        string baza = "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog = biblioteka; Integrated Security = True; Connect Timeout = 30; Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
        SqlConnection con;
        
        public wypozycz()
        {
            InitializeComponent();
            wczytaj_kategorie();

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
            SqlCommand cmd = new SqlCommand("select do_wyporzyczenia from dbo.egzemplarze", con);
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
