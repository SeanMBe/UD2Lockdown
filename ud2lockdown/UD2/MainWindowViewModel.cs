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
                var customerId = CustomerId;
                var customerResult = "";
                customerResult = Services.Cms.GetCustomer(customerId);
                CustomerGetResult = customerResult;
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
