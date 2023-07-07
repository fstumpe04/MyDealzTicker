using System.Data;
using System.Net;

namespace MyDealzTickerLibrary.Reader
{
  public class MyDealzWebsiteReader
  {
    private static MyDealzWebsiteReader s_instance;

    private MyDealzWebsiteReader() 
    {
      Read();
    }

    public static MyDealzWebsiteReader Instance
    {
      get
      {
        if (s_instance == null)
        {
          s_instance = new MyDealzWebsiteReader();
        }
        return s_instance;
      }
    }

    public async void Read()
    {
      using var client = new HttpClient();
      var content = await client.GetStringAsync(@"https://www.mydealz.de/");

    }
  }
}