using System;
using System.Globalization;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using GalaSoft.MvvmLight.Ioc;
using GalaSoft.MvvmLight.Messaging;
using SPA_Datahandler;
using SPA_Datahandler.Datamodel;

namespace PL_ServiceOnline.ViewModel
{
    /// <summary>
    /// This class contains properties that the main View can data bind to.
    /// <para>
    /// Use the <strong>mvvminpc</strong> snippet to add bindable properties to this ViewModel.
    /// </para>
    /// <para>
    /// You can also use Blend to data bind with the tool's support.
    /// </para>
    /// <para>
    /// See http://www.galasoft.ch/mvvm
    /// </para>
    /// </summary>
    public class MainViewModel : ViewModelBase
    {
        private OrderSummary selectedJob;

        public OrderSummary SelectedJob
        {
            get { return selectedJob; }
            set
            {
                selectedJob = value;
                RaisePropertyChanged();
            }
        }

        private IMessenger msg = Messenger.Default;
        private ViewModelBase currentDetailView;
        public bool Token { get; set; }
        public string Username { get; set; }

        public RelayCommand Btn_UpcomingJobs { get; set; }
        public RelayCommand Btn_PastJobs { get;  set; }
        public RelayCommand Btn_Logout { get; set; }
        public RelayCommand Btn_Detail { get; set; }
        public RelayCommand Btn_CompanyData { get; set; }
        public RelayCommand Btn_DeniedJobs { get; set; }



        public ViewModelBase CurrentDetailView
        {
            get { return currentDetailView; }
            set
            {
                currentDetailView = value;
                RaisePropertyChanged();
            }
        }
        /// <summary>
        /// Initializes a new instance of the MainViewModel class.
        /// </summary>
        public MainViewModel()
        {
            CultureInfo.DefaultThreadCurrentCulture = new CultureInfo("de-DE");
            CultureInfo.DefaultThreadCurrentUICulture = new CultureInfo("de-DE");


            if (!IsInDesignMode)
            {
                Token = false;
                Username = "Nicht eingeloggt.";
                msg.Register<GenericMessage<bool>>(this, ChangeToken);
                msg.Register<GenericMessage<string>>(this, "userToken", ChangeUserLabel);

                CurrentDetailView = SimpleIoc.Default.GetInstance<LoginVm>();
                //CurrentDetailView = SimpleIoc.Default.GetInstance<DetailVm>();

                Btn_UpcomingJobs = new RelayCommand(() =>
                {
                    ShowUpcoming();
                    
                }, () =>
                {
                    return Token;
                });

                Btn_PastJobs = new RelayCommand(() =>
                {
                    
                    CurrentDetailView = SimpleIoc.Default.GetInstance<JobsVm>();
                    msg.Send<GenericMessage<string>>(new GenericMessage<string>("past"));

                }, () =>
                {
                    return Token;
                });

                Btn_DeniedJobs = new RelayCommand(() =>
                {

                    CurrentDetailView = SimpleIoc.Default.GetInstance<JobsVm>();
                    msg.Send<GenericMessage<string>>(new GenericMessage<string>("denied"));

                }, () =>
                {
                    return Token;
                });


                Btn_Detail = new RelayCommand(execute: ChangeDetail, canExecute: () => 
                {
                    return (SelectedJob != null) && Token;
                }
                );

                Btn_CompanyData = new RelayCommand(() =>
                {

                    CurrentDetailView = SimpleIoc.Default.GetInstance<CompanyDataVm>();

                }, () =>
                {
                    return Token;
                });


                Btn_Logout = new RelayCommand(() =>
                {
                    Token = false;
                    ChangeUser("Nicht eingeloggt.");
                    CurrentDetailView = SimpleIoc.Default.GetInstance<LoginVm>();
                });

                msg.Register<GenericMessage<OrderSummary>>(this, ChangeSelected);
                //msg.Register<GenericMessage<LoginData>>(this, ChangeLoginData);
            }

        }

        private void ChangeUserLabel(GenericMessage<string> obj)
        {
            ChangeUser(obj.Content);
        }

        private void ChangeUser(string content)
        {
            Username = content;
            RaisePropertyChanged("Username");
        }

        private void ChangeToken(GenericMessage<bool> obj)
        {
            if (obj.Content)
            {
                Token = true;
                ShowUpcoming();
            }
            else
            {
                Token = false;
            }
        }

        private void ChangeDetail()
        {
            msg.Send<GenericMessage<OrderSummary>>(new GenericMessage<OrderSummary>(SelectedJob));
        }
        private void ShowUpcoming()
        {
            CurrentDetailView = SimpleIoc.Default.GetInstance<JobsVm>();
            msg.Send<GenericMessage<string>>(new GenericMessage<string>("future"));
        }

        private void ChangeSelected(GenericMessage<OrderSummary> obj)
        {
            if(selectedJob != obj.Content)
            {
                SelectedJob = obj.Content;
                
                ChangeDetail();
            }
            CurrentDetailView = SimpleIoc.Default.GetInstance<DetailVm>();

        }
    }
}