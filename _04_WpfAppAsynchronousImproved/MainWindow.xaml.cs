using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows;

namespace _04_WpfAppAsynchronousImproved
{
  /// <summary>
  /// Interaction logic for MainWindow.xaml
  /// </summary>
  public partial class MainWindow : Window
  {
    public MainWindow()
    {
      InitializeComponent();
    }
    private async void StartButton_Click(object sender, RoutedEventArgs e)
    {
      startButton.IsEnabled = false;
      resultsTextBox.Clear();
      await SumPageSizesAsync();
      resultsTextBox.Text += "\r\nControl returned to startButton_Click.";
      startButton.IsEnabled = true;
    }

    private async Task SumPageSizesAsync()
    {
      var total = 0;
      var watch = new Stopwatch();
      var urlList = SetUpURLList();

      watch.Start();
      // Defer behavior of LINQ
      IEnumerable<Task<int>> downloadTaskQuery = urlList.Select(x => ProcessUrlAsync(x));

      // Convert to Array for execution
      Task<int>[] downloadTasks = downloadTaskQuery.ToArray();

      // Call WhenAll for all the task
      int[] lengths = await Task.WhenAll(downloadTasks);
      watch.Stop();
      total = lengths.Sum();

      watch.Stop();
      lblTime.Content = watch.ElapsedMilliseconds.ToString() + "Milliseconds";
      resultsTextBox.Text += $"\r\n\r\nTotal bytes returned: {total}\r\n";
    }

    private async Task<int> ProcessUrlAsync(string url)
    {
      var client = new HttpClient { MaxResponseContentBufferSize = 1000000 };
      var urlContents = await client.GetByteArrayAsync(url);

      DisplayResults(url, urlContents);
      return urlContents.Length;
    }

    private List<string> SetUpURLList()
    {
      var urls = new List<string>
        {
        "https://msdn.microsoft.com/library/windows/apps/br211380.aspx",
        "https://msdn.microsoft.com",
        "https://msdn.microsoft.com/library/hh290136.aspx",
        "https://msdn.microsoft.com/library/ee256749.aspx",
        "https://msdn.microsoft.com/library/hh290138.aspx",
        "https://msdn.microsoft.com/library/hh290140.aspx",
        "https://msdn.microsoft.com/library/dd470362.aspx",
        "https://msdn.microsoft.com/library/aa578028.aspx",
        "https://msdn.microsoft.com/library/ms404677.aspx",
        "https://msdn.microsoft.com/library/ff730837.aspx"
        };
      return urls;
    }
    private void DisplayResults(string url, byte[] content)
    {
      var bytes = content.Length;
      var displayURL = url.Replace("https://", "");
      resultsTextBox.Text += $"\n{displayURL} {bytes}";
    }
  }
}
