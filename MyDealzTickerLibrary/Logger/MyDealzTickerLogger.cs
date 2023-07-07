using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyDealzTickerLibrary.Logger
{
  internal class MyDealzTickerLogger
  {
    private static MyDealzTickerLogger? s_instance = null;
    private StreamWriter _streamWriter;

    private MyDealzTickerLogger() 
    {
      var logFile = @"/MyDealzTicker.log";
      if (!File.Exists(logFile))
      {
        File.Create(logFile);
      }
      _streamWriter = new StreamWriter(logFile);
    }

    public static MyDealzTickerLogger Instance
    {
      get
      {
        if (s_instance == null)
        {
          s_instance = new MyDealzTickerLogger();
        }
        return s_instance;
      }
    }

    public void Log(string message)
    {
      using (_streamWriter)
      {
        _streamWriter.WriteLine($"{message} {GetCurrentFormattedDateTime()}");
      }
    }

    public void Log(Exception exception)
    {
      using (_streamWriter)
      {
        _streamWriter.WriteLine($"{exception.ToString()} {GetCurrentFormattedDateTime()}");
        _streamWriter.WriteLine(exception.Message);
        _streamWriter.WriteLine(exception.Source);
        _streamWriter.WriteLine(exception.StackTrace);
      }
    }

    private string GetCurrentFormattedDateTime()
    {
      var currentDateTime = DateTime.Now;
      var currentFormattedDateTime = currentDateTime.ToString("HH:mm:ss dd.MM.yyyy");
      return currentFormattedDateTime;
    }
  }
}
