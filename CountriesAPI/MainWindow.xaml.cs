using CountriesAPI.Models;
using CountriesAPI.Services;
using Newtonsoft.Json;
using SharpVectors.Converters;
using SharpVectors.Renderers.Wpf;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace CountriesAPI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        #region Atributes
        private List<Country> Countries;

        private NetworkService networkService;

        private ApiService apiService;

        private DataService dataService;

        private DialogService dialogService;
        #endregion

        public MainWindow()
        {
            InitializeComponent();
            OpenStartup();
            networkService = new NetworkService();
            apiService = new ApiService();
            dataService = new DataService();
            dialogService = new DialogService();
            LoadAPI();
        }

        private void OpenStartup()
        {
            StartMenu startMenu = new StartMenu(this);
            start.Children.Clear();
            start.Children.Add(startMenu);
        }

        private async void LoadAPI()
        {
            bool load;

            lb_result.Content = "Loading Countries...";

            var connection = networkService.CheckConnection();

            if (!connection.IsSuccess)
            {
                LoadLocalCountries();
                load = false;
            }
            else
            {
                await LoadApiCountries();
                load = true;
            }

            if (Countries.Count == 0)
            {
                lb_result.Content = "There is no internet connection." + Environment.NewLine +
                                    "Countries haven't been loaded previously." + Environment.NewLine +
                                    "Please try again later...";

                lb_status.Content = "First Time using the program has a required internet connection";
                return;
            }

            lb_result.Content = "Countries have been loaded";

            cb_country.ItemsSource = Countries;


            if (load)
            {
                lb_status.Content = string.Format("Coutries loaded from the internet in {0:F}", DateTime.Now);
            }
            else
            {
                lb_status.Content = string.Format("Countries loaded from Data Base");
            }

        }

        private void LoadLocalCountries()
        {
            Countries = dataService.GetData();
        }

        private async Task LoadApiCountries()
        {
            var response = await apiService.GetCountries("http://restcountries.eu", "/rest/v2/all");

            Countries = (List<Country>) response.Result;

            dataService.SaveData(Countries);

            await dataService.DownloadImages(Countries);
        }

        private void cb_country_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {

            var country = (Country)cb_country.SelectedItem;

            lb_name.Content = country.name;

            if(string.IsNullOrEmpty(country.capital))
            {
                lb_capital.Content = "No data available";
            }
            else
            {
                lb_capital.Content = country.capital;
            }

            lb_region.Content = country.region;

            if (string.IsNullOrEmpty(country.subregion))
            {
                lb_subregion.Content = "No data available";
            }
            else
            {
                lb_subregion.Content = country.capital;
            }

            lb_population.Content = country.population;

            if (country.gini == null)
            {
                lb_gini.Content = "No data available";
            }
            else
            {
                lb_gini.Content = country.gini;
            }
            ConvertionImage(cb_country.SelectedItem.ToString());
        }
        private void ConvertionImage(string country)
        {
            WpfDrawingSettings settings = new WpfDrawingSettings();
            settings.IncludeRuntime = true;
            settings.TextAsGeometry = false;

            foreach (var item in Countries)
            {
                string svgTestFile = Environment.CurrentDirectory + "/Flags/" + item.alpha3code + ".svg";
                if (country == item.name)
                {
                    FileSvgReader converter = new FileSvgReader(settings);
                    try
                    {
                        DrawingGroup drawing = converter.Read(svgTestFile);

                        if (drawing != null)
                        {
                            image_source.Source = new DrawingImage(drawing);
                        }
                    }
                    catch (Exception e)
                    {
                        dialogService.ShowMessage("Error", e.Message);
                        return;
                    }
                }
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Are you sure you want to close the app?", "Exit", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                Environment.Exit(0);
            }
        }
    }
}
