using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.ServiceModel;
using System.ServiceModel.Description;
using System.Windows.Input;
using UD2.Annotations;
using UD2.Services;

namespace UD2
{
    public class MainWindowViewModel : INotifyPropertyChanged
    {
        private string _customerGetResult;

        public event PropertyChangedEventHandler PropertyChanged;

        public MainWindowViewModel()
        {
            GetCustomerCommand = new BasicCommand(() =>
            {
                    using (var cf = new ChannelFactory<ICMS>(new WebHttpBinding(), "http://localhost:8341"))
                    {
                        cf.Endpoint.Behaviors.Add(new WebHttpBehavior());
                        var channel = cf.CreateChannel();
                        CustomerGetResult = channel.Customer(CustomerId);
                    }
                });
        }
        
        public ICommand GetCustomerCommand { get; private set; }
        
        public string CustomerId { get; set; }

        public string CustomerGetResult
        {
            get
            {
                return this._customerGetResult;
            }
            set
            {
                this._customerGetResult = value;
                this.OnPropertyChanged();
            }
        }

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            var handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
