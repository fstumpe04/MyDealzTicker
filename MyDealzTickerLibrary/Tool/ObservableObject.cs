using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace MyDealzTickerLibrary.Tool
{
  internal class ObservableObject : INotifyPropertyChanged
  {
    public event PropertyChangedEventHandler? PropertyChanged;

    public void SetProperty<T>(ref T field, T value, [CallerMemberName] string propertyName = "")
    {
      if (!String.IsNullOrEmpty(propertyName))
      {
        field = value;
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
      }
    }
  }
}
