using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
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

namespace WpfAppAsynchronous
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
      // Make a list of web addresses.
      List<string> urlList = SetUpURLList();
      var total = 0;
      foreach (var url in urlList)
      {
        // GetURLContents returns the contents of url as a byte array.
        byte[] urlContents = await GetURLContents(url);
        DisplayResults(url, urlContents);
        // Update the total.
        total += urlContents.Length;
      }
      // Display the total count for all of the web addresses.
      resultsTextBox.Text += $"\r\n\r\nTotal bytes returned: {total}\r\n";
    }    

    private async Task<byte[]> GetURLContents(string url)
    {
      var contents = new MemoryStream();
      var webRequest = (HttpWebRequest)WebRequest.Create(url);
      using (WebResponse response = await webRequest.GetResponseAsync())
      using (Stream responseStream = response.GetResponseStream())
        await responseStream.CopyToAsync(contents);
      return contents.ToArray();
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
