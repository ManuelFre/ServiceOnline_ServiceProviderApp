using System;
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
        
        public RelayCommand Btn_UpcomingJobs { get; set; }
        public RelayCommand Btn_PastJobs { get;  set; }
        public RelayCommand Btn_Logout { get; set; }
        public RelayCommand Btn_Detail { get; set; }

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
            if (!IsInDesignMode)
            {
                //CurrentDetailView = SimpleIoc.Default.GetInstance<LoginVm>();
                CurrentDetailView = SimpleIoc.Default.GetInstance<DetailVm>();

                Btn_UpcomingJobs = new RelayCommand(() =>
                {
                    
                    CurrentDetailView = SimpleIoc.Default.GetInstance<JobsVm>();
                    msg.Send<GenericMessage<string>>(new GenericMessage<string>("future"));
                });

                Btn_PastJobs = new RelayCommand(() =>
                {
                    
                    CurrentDetailView = SimpleIoc.Default.GetInstance<JobsVm>();
                    msg.Send<GenericMessage<string>>(new GenericMessage<string>("past"));

                });

                Btn_Logout = new RelayCommand(() =>
                {
                    CurrentDetailView = SimpleIoc.Default.GetInstance<LoginVm>();
                });

                Btn_Detail = new RelayCommand(execute: ChangeDetail, canExecute: () => 
                {
                    return (SelectedJob != null);
                }
                );
                
                msg.Register<GenericMessage<OrderSummary>>(this, ChangeSelected);
            }

        }

        private void ChangeDetail()
        {
            
            //msg.Send<>
            msg.Send<GenericMessage<OrderSummary>>(new GenericMessage<OrderSummary>(SelectedJob));

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