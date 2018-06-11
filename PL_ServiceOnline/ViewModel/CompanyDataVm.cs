using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using SPA_Datahandler;
using SPA_Datahandler.Datamodel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace PL_ServiceOnline.ViewModel
{
    public class CompanyDataVm : ViewModelBase
    {
        #region Properties
        public bool Token { get; set; }
        private IMessenger msg = Messenger.Default;
        private string companyName { get; set; }
        public string CompanyName
        {
            get { return companyName; }
            set { companyName = value; RaisePropertyChanged(); }
        }

        private string address { get; set; }
        public string Address
        {
            get { return address; }
            set { address = value; RaisePropertyChanged(); }
        }

        private string city { get; set; }
        public string City
        {
            get { return city; }
            set { city = value; RaisePropertyChanged(); }
        }

        private string zip { get; set; }
        public string Zip
        {
            get { return zip; }
            set { zip = value; RaisePropertyChanged(); }
        }

        private string phone { get; set; }
        public string Phone
        {
            get { return phone; }
            set { phone = value; RaisePropertyChanged(); }
        }

        private int cntCompletedOrders { get; set; }
        public int CntCompletedOrders
        {
            get { return cntCompletedOrders; }
            set { cntCompletedOrders = value; RaisePropertyChanged(); }
        }


        private int cntOpenOrders { get; set; }
        public int CntOpenOrders
        {
            get { return cntOpenOrders; }
            set { cntOpenOrders = value; RaisePropertyChanged(); }
        }

        private double sumTargetedSales { get; set; }
        public double SumTargetedSales
        {
            get { return sumTargetedSales; }
            set { sumTargetedSales = value; RaisePropertyChanged(); }
        }

        private int cntUnconfirmedOrders;

        public int CntUnconfirmedOrders
        {
            get { return cntUnconfirmedOrders; }
            set { cntUnconfirmedOrders = value; RaisePropertyChanged(); }
        }

        private int cntOpenOrdersThisMonth;

        public int CntOpenOrdersThisMonth
        {
            get { return cntOpenOrdersThisMonth; }
            set { cntOpenOrdersThisMonth = value;RaisePropertyChanged(); }
        }


        private Dataprovider Datahandler;
        public RelayCommand BtnChangeClicked { get; set; }

        private int ServiceProviderId;

        #endregion
        


        public CompanyDataVm()
        {
            msg.Register<GenericMessage<string>>(this, ChangeData);

            BtnChangeClicked = new RelayCommand(() => UpdateCompanyData());
            Datahandler = new Dataprovider();
            GetCompanyData();
        }

        private void ChangeData(GenericMessage<string> obj)
        {
            
            if (obj.Content.Equals("update")){
                GetCompanyData();
            }
        }

        private void GetCompanyData()
        {
            ServiceProviderData Data = new ServiceProviderData();
            Data = Datahandler.QueryServiceProviderData();
            if (Data != null)
            {
                ServiceProviderId = Data.Id;
                CompanyName = Data.CompanyName;
                Address = Data.Address;
                Zip = Data.Zip;
                City = Data.City;
                Phone = Data.Phone;
                CntCompletedOrders = Data.CntCompletedOrders;
                CntOpenOrders = Data.CntOpenOrders;
                SumTargetedSales = Data.SumTargetedSales;
                CntOpenOrdersThisMonth = Data.CntOpenOrdersThisMonth;
                CntUnconfirmedOrders = Data.CntUnconfirmedOrders;
            }            
        }

        private void UpdateCompanyData()
        {
            ServiceProviderData Data = new ServiceProviderData();
            Data.Id = ServiceProviderId;
            Data.CompanyName = CompanyName;
            Data.Address=Address;
            Data.Zip=Zip;
            Data.City=City;
            Data.Phone=Phone;
            Data.CntCompletedOrders=CntCompletedOrders;
            Data.CntOpenOrders= CntOpenOrders;
            Data.SumTargetedSales= SumTargetedSales;

            if (Datahandler.UpdateServiceProviderData(Data))
            {
                MessageBox.Show("Update erfolgreich ausgeführt.");
            }
            else
            {
                MessageBox.Show("Update fehlgeschlagen.");
            }
        }
    }
}
