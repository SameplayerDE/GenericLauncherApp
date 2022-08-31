using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;
using Microsoft.Web.WebView2.Core;
using Microsoft.Web.WebView2.Wpf;

namespace GenericLauncherApp
{
    public class Post
    {
        [JsonPropertyName("id")] public int ID { get; set; }
        [JsonPropertyName("title")] public string Title { get; set; }
        [JsonPropertyName("content")] public string Content { get; set; }
        [JsonPropertyName("author")] public int AuthorID { get; set; }
        [JsonIgnore] public User Author { get; set; }
    }
    
    public class User
    {
        [JsonPropertyName("id")] public int ID { get; set; }
        [JsonPropertyName("name")] public string Name { get; set; }
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
                var resultPosts = await GetAllPosts();
                var postCollection = JsonSerializer.Deserialize<List<Post>>(resultPosts);

                foreach (var post in postCollection)
                {
                    var responseUser = await GetUserID(post.AuthorID);
                    var userCollection = JsonSerializer.Deserialize<List<User>>(responseUser);

                    post.Author = userCollection[0];
                }
                
                ItemsControl.ItemsSource = postCollection;
                ItemsControl.Visibility = Visibility.Visible;
                Connection.Visibility = Visibility.Collapsed;
            }
            catch (Exception e)
            {
                Connection.Visibility = Visibility.Visible;
                ItemsControl.Visibility = Visibility.Collapsed;
            }
        }
        
        public static async Task<string> GetAllPosts()
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
                var response = await client.GetAsync($"blog/post/all");
                message = await response.Content.ReadAsStringAsync();
            }
            return message;
        }
        
        public static async Task<string> GetUserID(int id)
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
                var response = await client.GetAsync($"blog/user/{id}");
                message = await response.Content.ReadAsStringAsync();
            }
            return message;
        }
        
        private void CoreWebView2_NewWindowRequested(object sender, CoreWebView2NewWindowRequestedEventArgs e)
        {
            Console.WriteLine(e.Uri);
            View2.Source = new Uri(e.Uri);
            e.Handled = true;
        }
        
        private async void Window_Loaded(object sender, RoutedEventArgs e)
        {
            SomeMethod();
            await View2.EnsureCoreWebView2Async();
            View2.CoreWebView2.NewWindowRequested += CoreWebView2_NewWindowRequested;
           // MediaElement.Visibility = Visibility.Collapsed;
            MediaElement.Source =
                new Uri(
                    "https://dv.phncdn.com/videos/201805/25/167673012/720P_4000K_167673012.mp4?ttl=1661959877&ri=1228800&rs=4000&ipa=120.16.227.101&hash=57a6528da96dfade17ac5eb2f8c35dd6");
                //MediaElement.Play();
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

        protected override void OnClosed(EventArgs e)
        {
            MediaElement.Stop();
            MediaElement.Close();
            base.OnClosed(e);
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

        private void FrameworkElement_OnSourceUpdated(object? sender, DataTransferEventArgs e)
        {
            Console.WriteLine("Hallo");
        }

        private void WebView2_OnSourceChanged(object? sender, CoreWebView2SourceChangedEventArgs e)
        {
            Console.WriteLine("Hallo");
        }

        private void FrameworkElement_OnRequestBringIntoView(object sender, RequestBringIntoViewEventArgs e)
        {
            Console.WriteLine("Hallo");
        }

    }
}