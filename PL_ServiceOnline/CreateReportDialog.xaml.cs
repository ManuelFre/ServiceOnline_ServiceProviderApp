using Microsoft.Win32;
using PL_ServiceOnline.Converter;
using SPA_Datahandler.Datamodel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace PL_ServiceOnline
{
    /// <summary>
    /// Interaction logic for CreateReportDialog.xaml
    /// </summary>
    public partial class CreateReportDialog : Window
    {
        private bool pictureAdded = false;
        private bool changesMade = false;
        private int CurrentMaxOrderItemReportId;
        private BitmapImage appendixImage;

        public List<OrderItemReportAppendix> AppendixImageList{ get; set; }

        //private BitmapImage emptyImage;

        //public ObservableCollection<BitmapImage> ImageList { get; set; }

        private int OrderItemId;

        public CreateReportDialog(int CurrentMaxOrderItemReportId, int orderItemId)
        {
            InitializeComponent();
            btnDialogOk.IsEnabled = false;
            lblDate.Content = DateTime.Now.ToString("dddd, dd. MMMM yyyy");
            this.CurrentMaxOrderItemReportId = CurrentMaxOrderItemReportId;
            this.OrderItemId = orderItemId;

            AppendixImageList = new List<OrderItemReportAppendix>();

            //emptyImage = new BitmapImage(new Uri(@"/Images/KeinBild.jpg", UriKind.Relative));

            //ImageList = new ObservableCollection<BitmapImage>();
            //ImageList.Add(emptyImage);

            //ImageBox.ItemsSource = ImageList;

        }
        public OrderItemReport_ Answer
        {
            

            get
            {
                return new OrderItemReport_()
                {
                    //Don't set ID so DB can AutoSet its AutoIncrement...
                    Comment = txtComment.Text,
                    OrderItemId = OrderItemId,                                      //CurrentMaxOrderItemReportId,
                    ReportDate = DateTime.Now,
                    Appendix = AppendixImageList,
                    //Appendix = new List<OrderItemReportAppendix>()
                    //{
                    //   new OrderItemReportAppendix()
                    //   {
                    //       OrderItemReportId = CurrentMaxOrderItemReportId,
                    //       Picture = ImageConverter.ImageToByteArray(appendixImage)
                    //   }
                    //},
                    Visibility = "Visible", //(optional)to expand this OrderItemReport after Dialog is closed... (doesnt work yet properly) -> DetailVm /xaml problem

                };
            }
        }

        private void btnDialogOk_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
        }

        private void Window_ContentRendered(object sender, EventArgs e)
        {
            txtComment.Focus();
        }

        private void btnAddPicture_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Title = "Select a picture",
                Filter = "Image files (*.jpg, *.jpeg, *.jpe, *.jfif, *.png) | *.jpg; *.jpeg; *.jpe; *.jfif; *.png"
            };
            if ((bool)openFileDialog.ShowDialog())
            {
                try
                {
                    appendixImage = new BitmapImage(new Uri(openFileDialog.FileName, UriKind.Absolute));
                    AppendixImageList.Add(new OrderItemReportAppendix()
                    {
                        Picture = ImageConverter.ImageToByteArray(appendixImage)
                    });
                    imgImage.Source = appendixImage;
                    pictureAdded = true;
                    btnDialogOk.IsEnabled = true;
                }
                catch (Exception ex)
                {
                    if (!pictureAdded)
                    {
                        pictureAdded = false;
                        if (!changesMade)
                        {
                            btnDialogOk.IsEnabled = false;
                        }
                        MessageBox.Show("Laden des Bildes fehlgeschlagen!\n" + ex.Message, "Fehler", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
            else
            {
                if (!pictureAdded)
                {
                    MessageBox.Show("Laden des Bildes fehlgeschlagen", "Fehler", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void txtComment_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (txtComment.Text.Equals(String.Empty) && !pictureAdded)
            {
                btnDialogOk.IsEnabled = false;
                changesMade = false;
            }
            else
            {
                changesMade = true;
                btnDialogOk.IsEnabled = true;
            }
        }
    }
}
