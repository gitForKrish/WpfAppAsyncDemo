using System;
using System.Linq;
using System.Threading.Tasks;

namespace _05_AsyncReturnTypes
{
  class Program
  {
    static Random rnd;
    static TaskCompletionSource<bool> tcs;
    static void Main()
    {
      // Async Method return Task<TResult>
      //Console.WriteLine(ShowTodaysInfo().Result); // Gets the result value.

      // Async Method return Task
      //DisplayCurrentInfo().Wait();  // Waits for the Task to complete execution.

      // Async Method return void for event handler
      // EventHandlerWithAsync().Wait();

      // Async Method with ValueTask as return type - Task and Task<TResult> are of reference type, ValueTask is of value type - Introduced in C#7.0
      //var rolledSum = GetDiceRolled().Result;
      //Console.WriteLine($"You rolled {rolledSum}");
    }
    private async static ValueTask<int> GetDiceRolled()
    {
      Console.WriteLine("...Shaking the dice...");
      int roll1 = await Roll();
      int roll2 = await Roll();
      return roll1 + roll2;
    }
    private async static ValueTask<int> Roll()
    {
      if (rnd == null)
        rnd = new Random();
      await Task.Delay(500);
      return rnd.Next(1, 7);
    }
    static async Task EventHandlerWithAsync()
    {
      tcs = new TaskCompletionSource<bool>();
      var secondHandlerFinished = tcs.Task;

      var button = new NaiveButton();
      button.Clicked += Button_Clicked_1;
      button.Clicked += Button_Clicked_2_Async;
      button.Clicked += Button_Clicked_3;

      Console.WriteLine("About to click a button...");
      button.Click();
      Console.WriteLine("Button's Click method returned.");
      await secondHandlerFinished;
    }
    private static void Button_Clicked_3(object sender, EventArgs e)
    {
      Console.WriteLine(" Handler 3 is starting...");
      Task.Delay(100).Wait();
      Console.WriteLine(" Handler 3 is done.");
    }
    // Async Method return void for event handler
    private async static void Button_Clicked_2_Async(object sender, EventArgs e)
    {
      Console.WriteLine(" Handler 2 is starting...");
      Task.Delay(100).Wait();
      Console.WriteLine(" Handler 2 is about to go async...");
      await Task.Delay(500);
      Console.WriteLine(" Handler 2 is done.");
     tcs.SetResult(true);
    }
    private static void Button_Clicked_1(object sender, EventArgs e)
    {
      Console.WriteLine(" Handler 1 is starting...");
      Task.Delay(100).Wait();
      Console.WriteLine(" Handler 1 is done.");
    }
    /// <summary>
    /// Async method which return Task<TResult>
    /// </summary>
    /// <returns></returns>
    static async Task<string> ShowTodaysInfo()
    {
      //// Version 1
      //string ret = $"Today is {DateTime.Today:D} \n" +
      //  $"Today's resting hour = {await GetRestTime()}";

      // Version 2 (Preferred)
      Task<int> hour = GetRestTime();
      string ret = $"Today is {DateTime.Today:D} \n" +
        $"Today's resting hour = {await hour}";
      return ret;
    }
    private static async Task<int> GetRestTime()
    {
      var today = await Task.FromResult<string>(DateTime.Today.DayOfWeek.ToString());
      int hours;
      if (today.First() == 'S')
        hours = 16;
      else
        hours = 5;
      return hours;
    }
    /// <summary>
    /// Async method which return no value - Task
    /// </summary>
    /// <returns></returns>
    static async Task DisplayCurrentInfo()
    {
      //// Version 1
      //await WaitAndApologize();
      //Console.WriteLine($"Today is {DateTime.Now:D}");
      //Console.WriteLine($"The current time is {DateTime.Now.TimeOfDay:t}");
      //Console.WriteLine("The current temperature is 76 degrees.");

      Task wait = WaitAndApologize();
      Console.WriteLine($"Today is {DateTime.Now:D}");
      Console.WriteLine($"The current time is {DateTime.Now.TimeOfDay:t}");
      Console.WriteLine("The current temperature is 76 degrees.");
      await wait;
    }
    static async Task WaitAndApologize()
    {      
      await Task.Delay(2000); // Task.Delay is a placeholder for actual work.      
      Console.WriteLine("\nSorry for the delay. . . .\n");
    }
  }
  public class NaiveButton
  {
    public event EventHandler Clicked;
    public void Click()
    {
      Console.WriteLine("Somebody has clicked a button. Let's raise the event...");
      Clicked?.Invoke(this, EventArgs.Empty);
      Console.WriteLine("All listeners are notified.");
    }
  }
}
