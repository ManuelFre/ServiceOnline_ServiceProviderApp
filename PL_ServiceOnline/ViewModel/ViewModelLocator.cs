/*
  In App.xaml:
  <Application.Resources>
      <vm:ViewModelLocator xmlns:vm="clr-namespace:PL_ServiceOnline"
                           x:Key="Locator" />
  </Application.Resources>
  
  In the View:
  DataContext="{Binding Source={StaticResource Locator}, Path=ViewModelName}"

  You can also use Blend to do all this with the tool's support.
  See http://www.galasoft.ch/mvvm
*/

using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Ioc;
using Microsoft.Practices.ServiceLocation;

namespace PL_ServiceOnline.ViewModel
{
    /// <summary>
    /// This class contains static references to all the view models in the
    /// application and provides an entry point for the bindings.
    /// </summary>
    public class ViewModelLocator
    {
        /// <summary>
        /// Initializes a new instance of the ViewModelLocator class.
        /// </summary>
        public ViewModelLocator()
        {
            ServiceLocator.SetLocatorProvider(() => SimpleIoc.Default);

            ////if (ViewModelBase.IsInDesignModeStatic)
            ////{
            ////    // Create design time view services and models
            ////    SimpleIoc.Default.Register<IDataService, DesignDataService>();
            ////}
            ////else
            ////{
            ////    // Create run time view services and models
            ////    SimpleIoc.Default.Register<IDataService, DataService>();
            ////}
            SimpleIoc.Default.Register<LoginVm>(true);
            //SimpleIoc.Default.Register<UpcomingJobsVm>(true); not used anymore!
            SimpleIoc.Default.Register<JobsVm>(true);
            SimpleIoc.Default.Register<CompanyDataVm>(true);
            SimpleIoc.Default.Register<DetailVm>();
            SimpleIoc.Default.Register<MainViewModel>();
        }

        public MainViewModel Main
        {
            get
            {
                return ServiceLocator.Current.GetInstance<MainViewModel>();
            }
        }
        public LoginVm LoginVm
        {
            get
            {
                return ServiceLocator.Current.GetInstance<LoginVm>();
            }
        }
        //public UpcomingJobsVm UpcomingJobsVm
        //{
        //    get
        //    {
        //        return ServiceLocator.Current.GetInstance<UpcomingJobsVm>();
        //    }
        //}
        public JobsVm JobsVm
        {
            get
            {
                return ServiceLocator.Current.GetInstance<JobsVm>();
            }
        }
        public DetailVm DetailVm
        {
            get
            {
                return ServiceLocator.Current.GetInstance<DetailVm>();
            }
        }
        public CompanyDataVm CompanyData
        {
            get
            {
                return ServiceLocator.Current.GetInstance<CompanyDataVm>();
            }
        }


        public static void Cleanup()
        {
            // TODO Clear the ViewModels
        }
    }
}