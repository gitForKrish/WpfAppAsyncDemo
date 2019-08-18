using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Windows;

namespace WpfAppSynchronous
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

    private void StartButton_Click(object sender, RoutedEventArgs e)
    {
      resultsTextBox.Clear();
      SumPageSizes();
      resultsTextBox.Text += "\r\nControl returned to startButton_Click.";
    }

    private void SumPageSizes()
    {
      // Make a list of web addresses.
      List<string> urlList = SetUpURLList();
      var total = 0;
      foreach (var url in urlList)
      {
        // GetURLContents returns the contents of url as a byte array.
        byte[] urlContents = GetURLContents(url);
        DisplayResults(url, urlContents);
        // Update the total.
        total += urlContents.Length;
      }
      // Display the total count for all of the web addresses.
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

    private byte[] GetURLContents(string url)
    {
      var contents = new MemoryStream();
      var webRequest = (HttpWebRequest)WebRequest.Create(url);
      using (WebResponse response = webRequest.GetResponse())
      using (Stream responseStream = response.GetResponseStream())
        responseStream.CopyTo(contents);
      return contents.ToArray();
    }
    private void DisplayResults(string url, byte[] content)
    {
      var bytes = content.Length;
      var displayURL = url.Replace("https://", "");
      resultsTextBox.Text += $"\n{displayURL} {bytes}";
    }
  }
}
