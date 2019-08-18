using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows;

namespace WpfAppAsynchronousImproved
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
      var urlTasks = new List<Task<byte[]>>();

      foreach (var url in urlList)
      {
        var client = new HttpClient { MaxResponseContentBufferSize = 1000000 };        
        urlTasks.Add(client.GetByteArrayAsync(url));
      }

      watch.Start();
      var taskFinished = await Task.WhenAll(urlTasks);
      for (int i = 0; i < urlList.Count; i++)
      {
        DisplayResults(urlList[i], taskFinished[i]);
        total += taskFinished[i].Length;
      }
      watch.Stop();
      lblTime.Content = watch.ElapsedMilliseconds.ToString() + "Milliseconds";
      resultsTextBox.Text += $"\r\n\r\nTotal bytes returned: {total}\r\n";
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
