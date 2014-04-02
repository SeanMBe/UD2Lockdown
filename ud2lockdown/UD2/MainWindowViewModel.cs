﻿using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;
using System.Windows.Input;
using CMS.Client;
using UD2.Annotations;

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
                customerResult = CmsClient.GetCustomer(customerId, new X509Certificate2(@"C:\Program Files (x86)\Microsoft SDKs\Windows\v7.0A\Bin\cmsDevClient.cer", ""));
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
