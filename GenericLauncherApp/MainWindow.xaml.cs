﻿using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace GenericLauncherApp
{
    public class ToDo
    {
        [JsonPropertyName("id")] public int Id { get; set; }

        [JsonPropertyName("name")] public string Name { get; set; }

        [JsonPropertyName("done")] public int Done { get; set; }
    }
    
    public class Comment
    {
        //https://jsonplaceholder.typicode.com/comments
        [JsonPropertyName("postId")] public int PostId { get; set; }
        [JsonPropertyName("id")] public int Id { get; set; }
        [JsonPropertyName("name")] public string Name { get; set; }
        [JsonPropertyName("email")] public string Email { get; set; }
        [JsonPropertyName("body")] public string Body { get; set; }
    }

    public class Employee
    {
        //https://dummy.restapiexample.com/api/v1/employees
        [JsonPropertyName("id")] public int Id { get; set; }
        [JsonPropertyName("employee_name")] public string Name { get; set; }
        [JsonPropertyName("profile_image")] public string Path { get; set; }
        [JsonPropertyName("employee_salary")] public int Salary { get; set; }
        [JsonPropertyName("employee_age")] public int Age { get; set; }
    }
    
    public class WeatherForecast
    {
        [JsonPropertyName("date")]
        public DateTime Date { get; set; }
        [JsonPropertyName("temperatureC")]
        public int TemperatureC { get; set; }
        [JsonPropertyName("temperatureF")]
        public int TemperatureF { get; set; }
        [JsonPropertyName("summary")]
        public string Summary { get; set; }
    }
    
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }
        
        public async void SomeMethod()
        {
            try
            {
                var result = await Get();
                //Console.WriteLine(result);
                //var document = JsonDocument.Parse(result);
                //var data = document.RootElement.GetProperty("data");
                var collection = JsonSerializer.Deserialize<List<WeatherForecast>>(result);
                DataGrid.ItemsSource = collection;
                DataGrid.Visibility = Visibility.Visible;
                Connection.Visibility = Visibility.Collapsed;
            }
            catch (Exception e)
            {
                Connection.Visibility = Visibility.Visible;
                DataGrid.Visibility = Visibility.Collapsed;
            }
        }
        
        public static async Task<string> Get()
        {
            string message = string.Empty;
            using (var client = new HttpClient())
            {
                //client.BaseAddress = new Uri("http://localhost/Rest/ToDo/todo/");
                //client.BaseAddress = new Uri("https://dummy.restapiexample.com/api/v1/");
                //client.BaseAddress = new Uri("https://jsonplaceholder.typicode.com/");
                client.BaseAddress = new Uri("http://212.227.209.159/");

                //var response = await client.GetAsync($"read");
                //var response = await client.GetAsync($"comments");
                var response = await client.GetAsync($"weatherforecast");
                message = await response.Content.ReadAsStringAsync();
            }
            return message;
        }
        
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            SomeMethod();
            /*try
            {
                Utils.FetchPosts();
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message);
            }*/
        }

        private void TopBar_Down(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                DragMove();
            }
        }

        private void Close_OnClick(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void Maximise_OnClick(object sender, RoutedEventArgs e)
        {
            if (Application.Current.MainWindow!.WindowState != WindowState.Maximized)
            {
                Application.Current.MainWindow!.WindowState = WindowState.Maximized;
            }
            else
            {
                Application.Current.MainWindow!.WindowState = WindowState.Normal;
            }
        }

        private void Minimise_OnClick(object sender, RoutedEventArgs e)
        {
            Application.Current.MainWindow!.WindowState = WindowState.Minimized;
        }

        private void Update_OnClick(object sender, RoutedEventArgs e)
        {
            SomeMethod();
        }
    }
}