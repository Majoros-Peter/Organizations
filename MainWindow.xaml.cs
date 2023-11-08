using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.IO;
using System.Globalization;

namespace OrganizationWpf
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        List<Organization> organizations;

        public MainWindow()
        {
            InitializeComponent();
            ReadCsv();

            cbOrszag.ItemsSource = organizations.Select(G => G.Country).OrderBy(G => G).Distinct().ToList();
            cbEv.ItemsSource = organizations.Select(G => G.Founded).OrderBy(G => G).Distinct().ToList();
            labTotalEmpl.Content = organizations.Sum(G => G.EmployeesNumber);
        }

        private void ReadCsv()
        {
            organizations = File.ReadAllLines("organizations-100000.csv").Skip(1).Select(G => new Organization(G)).ToList();
            dgAdatok.ItemsSource = organizations;
        }

        private void dgAdatok_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if(dgAdatok.SelectedItem is Organization valasztottObjektum)
            {
                labID.Content = valasztottObjektum.Id.ToString();
                labWEB.Content = valasztottObjektum.Website;
                labISM.Content = valasztottObjektum.Description;
            }
            else
            {
                MessageBox.Show("Hiba!");
            }
        }

        private void cbEv_SelectionChanged(object sender, SelectionChangedEventArgs e) => Filter(cbOrszag.SelectedItem, (sender as ComboBox).SelectedItem);

        private void cbOrszag_SelectionChanged(object sender, SelectionChangedEventArgs e) => Filter((sender as ComboBox).SelectedItem, cbEv.SelectedItem);

        private void Filter(object? _orszag, object? _ev)
        {
            string orszag = (_orszag ?? "").ToString();
            int ev = Convert.ToInt32(_ev);

            dgAdatok.ItemsSource = organizations
                .Where(G => ev==0 || G.Founded==ev)
                .Where(G => orszag=="" || G.Country==orszag)
                .ToList();
        }
    }
}
