using Avalonia.Controls;
using Avalonia.Media;
using Avalonia;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Task3Shop.Models;
using Avalonia.Controls.Shapes;

namespace Task3Shop.Tools
{
    public class Animation
    {
         public static readonly IBrush[] COLORS = new IBrush[]
        {
            Brushes.Magenta,
            Brushes.Orange,
            Brushes.Sienna,
            Brushes.Cyan,
            Brushes.Red,
            Brushes.Green
        };


       


        public static void DrawAllCustomers(Canvas canvas, double scalefactor, IEnumerable<Customer> customers)
        {
            var path = Geometry.Parse(ModelsAnimationHelper.CUSTOMERIMAGE);

            var bounds = path.Bounds;
            double itemWidth = bounds.Right * scalefactor;
            double itemHeight = bounds.Bottom * scalefactor;

            double marginLR = itemWidth;
            double marginTB = itemHeight / 4;

            if (customers.Count() == 1)
            {
                (Path customerImagePath, TextBlock text) = customers.First().CreateImage(COLORS[1], scalefactor);
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

            double interlace = (canvas.Bounds.Width - marginLR * 2) / (customers.Count() + 1);
            int counter = 1;
            foreach (Customer customer in customers)
            {
                (Path customerImagePath, TextBlock text) = customer.CreateImage(COLORS[counter % COLORS.Length], scalefactor);
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



        public static void DrawAllDeliveryServices(Canvas canvas, double scalefactor, IEnumerable<DeliveryService> deliveryServices)
        {
            var path = Geometry.Parse(ModelsAnimationHelper.DELIVERYSERVICEIMAGE);

            var pathForCustomer = Geometry.Parse(ModelsAnimationHelper.CUSTOMERIMAGE);
            var tmpText = new TextBlock
            {
                Text = "name",
                FontSize = 18,
            };
            tmpText.Measure(Size.Infinity);
            tmpText.Arrange(new Rect(tmpText.DesiredSize));

            var pathForShop = Geometry.Parse(ModelsAnimationHelper.SHOPIMAGE);


            double marginTop = (pathForCustomer.Bounds.Height * scalefactor + pathForCustomer.Bounds.Height * scalefactor / 2 + tmpText.Bounds.Height) / 2;
            double marginBottom = (pathForShop.Bounds.Height * scalefactor + pathForShop.Bounds.Height * scalefactor / 2 + tmpText.Bounds.Height) / 2;


            var bounds = path.Bounds;
            double itemWidth = bounds.Right * scalefactor;
            double itemHeight = bounds.Bottom * scalefactor;


            double marginLR = itemWidth;
            double marginTB = itemHeight / 4;

            if (deliveryServices.Count() == 1)
            {
                (Path deliveryServiceImagePath, TextBlock text) = deliveryServices.First().CreateImage(COLORS[1], scalefactor);
                canvas.Children.Add(deliveryServiceImagePath);
                canvas.Children.Add(text);

                text.Measure(Size.Infinity);
                text.Arrange(new Rect(text.DesiredSize));


                Canvas.SetLeft(deliveryServiceImagePath, marginLR - itemWidth / 2 - bounds.Right / 2 + bounds.Right * scalefactor / 2);


                double pictureTopY = (canvas.Bounds.Height - itemHeight - marginTB - tmpText.Bounds.Height) / 2;
                Canvas.SetTop(deliveryServiceImagePath, pictureTopY - bounds.Height / 2 + bounds.Height * scalefactor / 2);

                double pictureCenterX = canvas.Bounds.Width / 2;

                Canvas.SetLeft(text, marginLR - text.Bounds.Right / 2);
                Canvas.SetTop(text, pictureTopY + itemHeight + marginTB);
                return;
            }

            double interlace = (canvas.Bounds.Height - marginTB * 2 - marginTop - marginBottom) / (deliveryServices.Count());
            int counter = 0;
            foreach (DeliveryService service in deliveryServices)
            {
                (Path serviceImagePath, TextBlock text) = service.CreateImage(COLORS[counter % COLORS.Length], scalefactor);
                canvas.Children.Add(serviceImagePath);
                canvas.Children.Add(text);

                text.Measure(new Size(itemWidth * 2, double.PositiveInfinity));
                text.MaxWidth = itemWidth * 2;
                text.Arrange(new Rect(text.DesiredSize));

                double pictureTopY = marginTop + marginTB + interlace / 2 + interlace * counter - (itemHeight + marginTB + text.Bounds.Height) / (deliveryServices.Count()) * counter - bounds.Height / 2 + bounds.Height * scalefactor / 2;

                Canvas.SetTop(serviceImagePath, pictureTopY);
                Canvas.SetLeft(serviceImagePath, marginLR - itemWidth / 2 - bounds.Right / 2 + bounds.Right * scalefactor / 2);

                Canvas.SetLeft(text, marginLR - text.Bounds.Right / 2);
                Canvas.SetTop(text, marginTop + marginTB + interlace / 2 + interlace * counter - (itemHeight + marginTB + text.Bounds.Height) / (deliveryServices.Count()) * counter + itemHeight + marginTB);

                counter++;
            }
        }

        public static void DrawAllShops(Canvas canvas, double scalefactor, IEnumerable<Shop> shops)
        {
            var path = Geometry.Parse(ModelsAnimationHelper.SHOPIMAGE);

            var bounds = path.Bounds;
            double itemWidth = bounds.Right * scalefactor;
            double marginLR = itemWidth;
            double itemHeight = bounds.Bottom * scalefactor;

            double marginTB = itemHeight / 4;


            if (shops.Count() == 1)
            {
                (Path shopImagePath, TextBlock text) = shops.First().CreateImage(COLORS[1], scalefactor);
                canvas.Children.Add(shopImagePath);
                canvas.Children.Add(text);

                text.Measure(Size.Infinity);
                text.Arrange(new Rect(text.DesiredSize));

                Canvas.SetLeft(shopImagePath, (canvas.Bounds.Width - itemWidth) / 2 - bounds.Right / 2 + bounds.Right * scalefactor / 2);
                Canvas.SetTop(shopImagePath, (canvas.Bounds.Height - marginTB - itemHeight - text.Bounds.Height) - bounds.Bottom / 2 + bounds.Bottom * scalefactor / 2);

                double pictureCenterX = canvas.Bounds.Width / 2;
                double pictureTopY = (canvas.Bounds.Height - itemHeight - marginTB / 2) - bounds.Bottom / 2 + bounds.Bottom * scalefactor / 2;

                Canvas.SetLeft(text, pictureCenterX - text.Bounds.Right / 2);
                Canvas.SetTop(text, canvas.Bounds.Height - text.Bounds.Bottom - marginTB / 2);
                return;
            }

            double interlace = (canvas.Bounds.Width - marginLR * 2) / (shops.Count() + 1);
            int counter = 1;
            foreach (Shop shop in shops)
            {
                (Path shopImagePath, TextBlock text) = shop.CreateImage(COLORS[counter % COLORS.Length], scalefactor);
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
    }
}
