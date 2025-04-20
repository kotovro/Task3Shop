using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Controls;
using ReactiveUI;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive;
using System.Threading.Tasks;
using Task3Shop.Models;
using Task3Shop.Views;
using System.Windows.Input;
using Avalonia.Media.Imaging;
using SkiaSharp;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using Avalonia.Controls.Shapes;
using Avalonia.Media;
using Avalonia.Media.Immutable;
using Avalonia;
using System.Xml.Linq;

namespace Task3Shop.ViewModels
{
    public partial class MainWindowViewModel : ViewModelBase
    {

        public const string SHOPIMAGE = "M36.8 192l566.3 0c20.3 0 36.8-16.5 36.8-36.8c0-7.3-2.2-14.4-6.2-20.4L558.2 21.4C549.3 8 534.4 0 518.3 0L121.7 0c-16 0-31 8-39.9 21.4L6.2 134.7c-4 6.1-6.2 13.2-6.2 20.4C0 175.5 16.5 192 36.8 192zM64 224l0 160 0 80c0 26.5 21.5 48 48 48l224 0c26.5 0 48-21.5 48-48l0-80 0-160-64 0 0 160-192 0 0-160-64 0zm448 0l0 256c0 17.7 14.3 32 32 32s32-14.3 32-32l0-256-64 0z";
        public const string CUSTOMERIMAGE = "M224 256A128 128 0 1 0 224 0a128 128 0 1 0 0 256zm-45.7 48C79.8 304 0 383.8 0 482.3C0 498.7 13.3 512 29.7 512l388.6 0c16.4 0 29.7-13.3 29.7-29.7C448 383.8 368.2 304 269.7 304l-91.4 0z";
        public const string DELIVERYSERVICEIMAGE = "M0 488L0 171.3c0-26.2 15.9-49.7 40.2-59.4L308.1 4.8c7.6-3.1 16.1-3.1 23.8 0L599.8 111.9c24.3 9.7 40.2 33.3 40.2 59.4L640 488c0 13.3-10.7 24-24 24l-48 0c-13.3 0-24-10.7-24-24l0-264c0-17.7-14.3-32-32-32l-384 0c-17.7 0-32 14.3-32 32l0 264c0 13.3-10.7 24-24 24l-48 0c-13.3 0-24-10.7-24-24zm488 24l-336 0c-13.3 0-24-10.7-24-24l0-56 384 0 0 56c0 13.3-10.7 24-24 24zM128 400l0-64 384 0 0 64-384 0zm0-96l0-80 384 0 0 80-384 0z";

        public readonly IBrush[] COLORS = new IBrush[]
        { 
            Brushes.Magenta,
            Brushes.Orange,
            Brushes.Sienna,
            Brushes.Cyan,
            Brushes.Red,
            Brushes.Green
        };

        public List<IOutOfStockClientStrategy> OutOfStockClientStrategies = [
            new DontBuyAnythingStrategy(),
            new MakeNewOrderAnywhereStrategy(),
            new MakeNewOrderHereStrategy()
        ]; 
        private Image _image;
        private double _x = 0;
        public SynchronizedCollection<Good>? GlobalGoods { get; } = new();
        public SynchronizedCollection<Shop>? GlobalShops { get; } = new();
        public SynchronizedCollection<Customer>? GlobalCustomers { get; } = new();
        public SynchronizedCollection<DeliveryService>? GlobalDeliveryServices { get; } = new();

        private readonly Window _mainWindow;
        public  async void AddGood()
        {
            var addGoodWindow = new AddGoodWindow();
            var goodFormVM = new AddGoodViewModel(_mainWindow, addGoodWindow);
            addGoodWindow.DataContext = goodFormVM;

            await addGoodWindow.ShowDialog(_mainWindow);
        }
        public async void AddShop()
        {
            var addShopWindow = new AddShopWindow();
            var shopFormVM = new AddShopViewModel(_mainWindow, addShopWindow);
            addShopWindow.DataContext = shopFormVM;

            await addShopWindow.ShowDialog(_mainWindow);
        }

        public async void AddCustomer()
        {
            var addCustomerWindow = new AddCustomerWindow();
            var customerFormVM = new AddCustomerViewModel(_mainWindow, addCustomerWindow);
            addCustomerWindow.DataContext = customerFormVM;

            await addCustomerWindow.ShowDialog(_mainWindow);
        }

        public async void AddDeliveryService()
        {
            var addDeliveryServiceWindow = new AddDeliveryServiceWindow();
            var deliveryFormVM = new AddDeliveryServiceViewModel(_mainWindow, addDeliveryServiceWindow);
            addDeliveryServiceWindow.DataContext = deliveryFormVM;

            await addDeliveryServiceWindow.ShowDialog(_mainWindow);
        }

        private (Path, TextBlock) CreateShopImage(string name, IBrush color, double scaleFactor)
        {
            var text = new TextBlock
            {
                Text = name,
                FontSize = 18,
                Foreground = color,
            };

            var path =  new Path()
            {
                Data = Geometry.Parse(SHOPIMAGE),
                Stroke = color,
                Fill = color,
                StrokeThickness = 2,
                RenderTransform = new ScaleTransform(scaleFactor,  scaleFactor),
            };
            return (path, text);
        }

        private (Path, TextBlock) CreateCustomerImage(string name, IBrush color, double scaleFactor)
        {
            var text = new TextBlock
            {
                Text = name,
                FontSize = 18,
                Foreground = color,
            };

            var path = new Path()
            {
                Data = Geometry.Parse(CUSTOMERIMAGE),
                Stroke = color,
                Fill = color,
                StrokeThickness = 2,
                RenderTransform = new ScaleTransform(scaleFactor, scaleFactor),
            };
            return (path, text);
        }

        private (Path, TextBlock) CreateDeliveryServiceImage(string name, IBrush color, double scaleFactor)
        {
            var text = new TextBlock
            {
                Text = name,
                FontSize = 18,
                Foreground = color,
            };

            var path = new Path()
            {
                Data = Geometry.Parse(DELIVERYSERVICEIMAGE),
                Stroke = color,
                Fill = color,
                StrokeThickness = 2,
                RenderTransform = new ScaleTransform(scaleFactor, scaleFactor),
            };
            return (path, text);
        }

        private void DrawAllCustomers(Canvas canvas, double scalefactor)
        {
            var path = Geometry.Parse(CUSTOMERIMAGE);

            var bounds = path.Bounds;
            double itemWidth = bounds.Right * scalefactor;
            double itemHeight = bounds.Bottom * scalefactor;
            
            double marginLR = itemWidth;
            double marginTB = itemHeight / 4;

            if (GlobalCustomers.Count == 1)
            {
                (Path customerImagePath, TextBlock text) = CreateCustomerImage(GlobalCustomers.First().Name, COLORS[0], scalefactor);
                canvas.Children.Add(customerImagePath);
                canvas.Children.Add(text);

                text.Measure(Size.Infinity);
                text.Arrange(new Rect(text.DesiredSize));

                Canvas.SetLeft(customerImagePath, (canvas.Bounds.Width - itemWidth) / 2 - bounds.Right / 2 + bounds.Right * scalefactor / 2);

                double pictureTopY = marginTB - bounds.Bottom / 2 + bounds.Bottom * scalefactor / 2;
                Canvas.SetTop(customerImagePath, pictureTopY);

                double pictureCenterX = canvas.Bounds.Width / 2;
                
                Canvas.SetLeft(text, pictureCenterX - text.Bounds.Right / 2);
                Canvas.SetTop(text, marginTB + itemHeight + marginTB);
                return;
            }

            double interlace = (canvas.Bounds.Width - marginLR * 2) / (GlobalCustomers.Count - 1);
            int counter = 0;
            foreach (Customer customer in GlobalCustomers)
            {
                (Path customerImagePath, TextBlock text) = CreateCustomerImage(customer.Name, COLORS[counter % COLORS.Length], scalefactor);
                canvas.Children.Add(customerImagePath);
                canvas.Children.Add(text);

                text.Measure(new Size(itemWidth * 2, double.PositiveInfinity));
                text.MaxWidth = itemWidth * 2;
                text.Arrange(new Rect(text.DesiredSize));

                double pictureCenterX = marginLR + interlace * counter - bounds.Right / 2 + bounds.Right * scalefactor / 2;
                
                Canvas.SetLeft(customerImagePath, pictureCenterX - itemWidth / 2);
                
                double pictureTopY = marginTB - bounds.Bottom / 2 + bounds.Bottom * scalefactor / 2;
                Canvas.SetTop(customerImagePath, pictureTopY);
                
                double textCenterX = marginLR + interlace * counter;

                Canvas.SetLeft(text, textCenterX - text.Bounds.Right / 2);
                Canvas.SetTop(text, marginTB + itemHeight + marginTB);

                counter++;
            }
        }

        

        private void DrawAllDeliveryServices(Canvas canvas, double scalefactor)
        {
            var path = Geometry.Parse(DELIVERYSERVICEIMAGE);

            var pathForCustomer = Geometry.Parse(CUSTOMERIMAGE);
            var tmpText = new TextBlock
            {
                Text = "name",
                FontSize = 18,
            };
            tmpText.Measure(Size.Infinity);
            tmpText.Arrange(new Rect(tmpText.DesiredSize));

            var pathForShop = Geometry.Parse(SHOPIMAGE);


            double marginTop =  pathForCustomer.Bounds.Height * scalefactor + pathForCustomer.Bounds.Height * scalefactor / 2 + tmpText.Bounds.Height;
            double marginBottom = pathForShop.Bounds.Height * scalefactor + pathForShop.Bounds.Height * scalefactor / 2 + tmpText.Bounds.Height;


            var bounds = path.Bounds;
            double itemWidth = bounds.Right * scalefactor;
            double itemHeight = bounds.Bottom * scalefactor;

            
            double marginLR = itemWidth;
            double marginTB = itemHeight / 4;

            if (GlobalDeliveryServices.Count == 1)
            {
                (Path deliveryServiceImagePath, TextBlock text) = CreateDeliveryServiceImage(GlobalDeliveryServices.First().ServiceName, COLORS[0], scalefactor);
                canvas.Children.Add(deliveryServiceImagePath);
                canvas.Children.Add(text);

                text.Measure(Size.Infinity);
                text.Arrange(new Rect(text.DesiredSize));


                Canvas.SetLeft(deliveryServiceImagePath, marginLR - itemWidth / 2 - bounds.Right / 2 + bounds.Right * scalefactor / 2);
               

                double pictureTopY = (canvas.Bounds.Height - itemHeight - marginTB - tmpText.Bounds.Height)/ 2;
                Canvas.SetTop(deliveryServiceImagePath, pictureTopY - bounds.Height / 2 + bounds.Height * scalefactor / 2);

                double pictureCenterX = canvas.Bounds.Width / 2;
                
                Canvas.SetLeft(text, marginLR - text.Bounds.Right / 2);
                Canvas.SetTop(text, pictureTopY + itemHeight + marginTB);
                return;
            }

            double interlace = (canvas.Bounds.Height - marginTB * 2 - marginTop - marginBottom) / (GlobalDeliveryServices.Count - 1);
            int counter = 0;
            foreach (DeliveryService service in GlobalDeliveryServices)
            {
                (Path serviceImagePath, TextBlock text) = CreateDeliveryServiceImage(service.ServiceName, COLORS[counter % COLORS.Length], scalefactor);
                canvas.Children.Add(serviceImagePath);
                canvas.Children.Add(text);

                text.Measure(new Size(itemWidth * 2, double.PositiveInfinity));
                text.MaxWidth = itemWidth * 2;
                text.Arrange(new Rect(text.DesiredSize));

                double pictureTopY = marginTop + marginTB + interlace * counter - (itemHeight + marginTB + text.Bounds.Height) / (GlobalDeliveryServices.Count - 1) * counter - bounds.Height / 2 + bounds.Height * scalefactor / 2;
                
                Canvas.SetTop(serviceImagePath, pictureTopY);
                Canvas.SetLeft(serviceImagePath, marginLR - itemWidth / 2 - bounds.Right / 2 + bounds.Right * scalefactor / 2);

                Canvas.SetLeft(text, marginLR - text.Bounds.Right / 2);
                Canvas.SetTop(text, marginTop + marginTB + interlace * counter - (itemHeight + marginTB + text.Bounds.Height) / (GlobalDeliveryServices.Count - 1) * counter + itemHeight + marginTB);

                counter++;
            }
        }

        private void DrawAllShops(Canvas canvas, double scalefactor)
        {
        
            
            var path = Geometry.Parse(SHOPIMAGE);

            var bounds = path.Bounds;
            double itemWidth = bounds.Right * scalefactor;
            double marginLR = itemWidth;
            double itemHeight = bounds.Bottom * scalefactor;

            double marginTB = itemHeight / 4;


            if (GlobalShops.Count == 1)
            {
                (Path shopImagePath, TextBlock text) = CreateShopImage(GlobalShops.First().Name, COLORS[0], scalefactor);
                canvas.Children.Add(shopImagePath);
                canvas.Children.Add(text);

                text.Measure(Size.Infinity);
                text.Arrange(new Rect(text.DesiredSize));

                Canvas.SetLeft(shopImagePath, (canvas.Bounds.Width - itemWidth) / 2 - bounds.Right / 2 + bounds.Right * scalefactor / 2);
                Canvas.SetTop(shopImagePath, (canvas.Bounds.Height - marginTB - itemHeight - text.Bounds.Height) - bounds.Bottom / 2 + bounds.Bottom * scalefactor / 2);

                double pictureCenterX = canvas.Bounds.Width / 2 ;
                double pictureTopY = (canvas.Bounds.Height - itemHeight - marginTB / 2) - bounds.Bottom / 2 + bounds.Bottom * scalefactor / 2;

                Canvas.SetLeft(text, pictureCenterX - text.Bounds.Right / 2);
                Canvas.SetTop(text, canvas.Bounds.Height - text.Bounds.Bottom - marginTB / 2);
                return;
            }

            double interlace = (canvas.Bounds.Width - marginLR * 2) / (GlobalShops.Count - 1);
            int counter = 0;
            foreach (Shop shop in GlobalShops)
            {
                (Path shopImagePath, TextBlock text) = CreateShopImage(shop.Name, COLORS[counter % COLORS.Length], scalefactor);
                canvas.Children.Add(shopImagePath);
                canvas.Children.Add(text);

                text.Measure(new Size(itemWidth * 2, double.PositiveInfinity));
                text.MaxWidth = itemWidth * 2;
                text.Arrange(new Rect(text.DesiredSize));

                double pictureCenterX = marginLR + interlace * counter - bounds.Right / 2 + bounds.Right * scalefactor / 2;
                double pictureTopY = (canvas.Bounds.Height - itemHeight - marginTB - text.Bounds.Height) - bounds.Bottom / 2 + bounds.Bottom * scalefactor / 2;

                Canvas.SetLeft(shopImagePath, pictureCenterX - itemWidth / 2);
                Canvas.SetTop(shopImagePath, pictureTopY);

                double textCenterX = marginLR + interlace * counter;

                Canvas.SetLeft(text, textCenterX - text.Bounds.Right / 2);
                Canvas.SetTop(text, canvas.Bounds.Height - text.Bounds.Bottom - marginTB / 2);

                counter++;
            }
        }

        public void RedrawCanvas()
        {
            var scaleFactor = 0.05;
            Canvas canvas = _mainWindow.FindControl<Canvas>("Canvas");
            canvas.Children.Clear();
            if (GlobalShops.Count > 0)
            {
                DrawAllShops(canvas, scaleFactor);
            }
            if (GlobalCustomers.Count > 0)
            {
                DrawAllCustomers(canvas, scaleFactor);
            }
            if (GlobalDeliveryServices.Count > 0)
            {
                DrawAllDeliveryServices(canvas, scaleFactor);
            }
        }


        public MainWindowViewModel(Window mainWindow)
        {
            _mainWindow = mainWindow;
        }

        private void OnFrame(object sender, EventArgs e)
        {
            //_x += 2; // Move to the right
            //Canvas.SetLeft(_image, _x);

            //// Stop after moving off-screen
            //if (_x > Bounds.Width)
            //{
            //    CompositionTarget.Rendering -= OnFrame;
            //}
        }

        public string Greeting { get; } = "Welcome to Avalonia!";
    }


}
