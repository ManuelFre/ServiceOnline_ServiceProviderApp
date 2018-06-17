using Microsoft.Win32;
using PL_ServiceOnline.Converter;
using SPA_Datahandler.Datamodel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace PL_ServiceOnline
{
    /// <summary>
    /// CreateReportDialog is a Custom Dialog for adding multiple Pictures to the OrderItemReport by creating multiple OrderItemReportAppendices
    /// Use Answer Property to get a full OrderItemReport_
    /// </summary>
    public partial class CreateReportDialog : Window
    {
        #region Fields
        private bool pictureAdded = false;
        private bool changesMade = false;
        private int CurrentMaxOrderItemReportId;
        private int imageCount = 0;
        private Timer timer;

        private BitmapImage appendixImage;

        private int OrderItemId;
        #endregion

        #region Properties
        public List<OrderItemReportAppendix> AppendixImageList { get; set; }

        public ObservableCollection<BitmapImage> ImageList { get; set; }



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
                    Visibility = "Visible", //(optional)to expand this OrderItemReport after Dialog is closed... 

                };
            }
        }
        #endregion

        #region Constructor, Destructor
        public CreateReportDialog(int CurrentMaxOrderItemReportId, int orderItemId)
        {
            InitializeComponent();
            btnDialogOk.IsEnabled = false;

            lblImgCount.Content = imageCount.ToString();
            lblDate.Content = DateTime.Now.ToString("dd. MMMM yyyy HH:mm");
            this.CurrentMaxOrderItemReportId = CurrentMaxOrderItemReportId;
            OrderItemId = orderItemId;

            AppendixImageList = new List<OrderItemReportAppendix>();
            ImageList = new ObservableCollection<BitmapImage>();
            ItmCtrl.ItemsSource = ImageList;


            timer = new Timer((s) =>
            {
                Dispatcher.Invoke(new Action(() =>
                                {
                                    lblDate.Content = DateTime.Now.ToString("dd. MMMM yyyy HH:mm");
                                }));
            }, null, 0, 1000);


            //timer = new Timer(1000);

            //timer.Elapsed += new ElapsedEventHandler((o, s) =>
            //{
            //    Dispatcher.Invoke(new Action(() =>
            //    {
            //        lblDate.Content = DateTime.Now.ToString("dd. MMMM yyyy HH:mm");
            //    }));
            //});
            //timer.Start();


        }

        ~CreateReportDialog(){
            timer.Dispose();
        }
        #endregion

        #region Gui Events
    private void BtnDialogOk_Click(object sender, RoutedEventArgs e)
    {
        DialogResult = true;
    }

    private void Window_ContentRendered(object sender, EventArgs e)
    {
        txtComment.Focus();
    }

    private void BtnAddPicture_Click(object sender, RoutedEventArgs e)
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
                ImageList.Add(appendixImage);
                pictureAdded = true;
                btnDialogOk.IsEnabled = true;
                imageCount++;
                lblImgCount.Content = imageCount.ToString();

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

    private void TxtComment_TextChanged(object sender, TextChangedEventArgs e)
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
    #endregion
}
}
