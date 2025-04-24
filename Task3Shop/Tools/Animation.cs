using Avalonia.Controls;
using Avalonia.Media;
using Avalonia;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Task3Shop.Models;
using Avalonia.Controls.Shapes;
using System.Collections.Concurrent;

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

        private IEnumerable<Shop> _shops;
        private IEnumerable<Customer> _customers;
        private IEnumerable<DeliveryService> _deliveryServices;
        internal class EntityPosition
        { 
            internal double Top { get; set; }
            internal double Left { get; set; }
        };

        internal Dictionary<Shop, EntityPosition> ShopsPositions;
        internal Dictionary<Customer, EntityPosition> CustomersPositions;
        internal Dictionary<DeliveryService, EntityPosition> DeliveryServicesPositions;
        public Animation(IEnumerable<Shop> globalShops, IEnumerable<Customer> globalCustomers, IEnumerable<DeliveryService> globalDeliveryServices)
        {
            _shops = globalShops;
            _customers = globalCustomers;
            _deliveryServices = globalDeliveryServices;
        }

        public void DrawAllCustomers(Canvas canvas, double scalefactor)
        {
            CustomersPositions = [];

            var path = Geometry.Parse(ModelsAnimationHelper.CUSTOMERIMAGE);

            var bounds = path.Bounds;
            double itemWidth = bounds.Right * scalefactor;
            double itemHeight = bounds.Bottom * scalefactor;

            double marginLR = itemWidth;
            double marginTB = itemHeight / 4;

            if (_customers.Count() == 1)
            {
                (Path customerImagePath, TextBlock text) = _customers.First().CreateImage(COLORS[1], scalefactor);
                canvas.Children.Add(customerImagePath);
                canvas.Children.Add(text);

                text.Measure(Size.Infinity);
                text.Arrange(new Rect(text.DesiredSize));

                double leftX = (canvas.Bounds.Width - itemWidth) / 2 ;
                double topY = marginTB;
                CustomersPositions.Add(_customers.First(), new EntityPosition { Left = leftX, Top = topY });

                double leftXCorrected = leftX -bounds.Right / 2 + bounds.Right * scalefactor / 2;
                double topYCorrected = topY - bounds.Bottom / 2 + bounds.Bottom * scalefactor / 2;
                Canvas.SetLeft(customerImagePath, leftXCorrected);            
                Canvas.SetTop(customerImagePath, topYCorrected);

                double pictureCenterX = canvas.Bounds.Width / 2;

                Canvas.SetLeft(text, pictureCenterX - text.Bounds.Right / 2);
                Canvas.SetTop(text, marginTB + itemHeight + marginTB);
                return;
            }

            double interlace = (canvas.Bounds.Width - marginLR * 2) / (_customers.Count() + 1);
            int counter = 1;
            foreach (Customer customer in _customers)
            {
                (Path customerImagePath, TextBlock text) = customer.CreateImage(COLORS[counter % COLORS.Length], scalefactor);
                canvas.Children.Add(customerImagePath);
                canvas.Children.Add(text);

                text.Measure(new Size(itemWidth * 2, double.PositiveInfinity));
                text.MaxWidth = itemWidth * 2;
                text.Arrange(new Rect(text.DesiredSize));

                double leftX = marginLR + interlace * counter - itemWidth / 2;
                double topY = marginTB;
                CustomersPositions.Add(customer, new EntityPosition { Left = leftX, Top = topY });

                double leftXCorrected = leftX - bounds.Right / 2 + bounds.Right * scalefactor / 2;
                double topYCorrected = topY - bounds.Bottom / 2 + bounds.Bottom * scalefactor / 2;
                Canvas.SetLeft(customerImagePath, leftXCorrected);              
                Canvas.SetTop(customerImagePath, topYCorrected);

                double textCenterX = marginLR + interlace * counter;

                Canvas.SetLeft(text, textCenterX - text.Bounds.Right / 2);
                Canvas.SetTop(text, marginTB + itemHeight + marginTB);

                counter++;
            }
        }
        public void DrawAllDeliveryServices(Canvas canvas, double scalefactor)
        {
            DeliveryServicesPositions = [];
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

            if (_deliveryServices.Count() == 1)
            {
                (Path deliveryServiceImagePath, TextBlock text) = _deliveryServices.First().CreateImage(COLORS[1], scalefactor);
                canvas.Children.Add(deliveryServiceImagePath);
                canvas.Children.Add(text);

                text.Measure(Size.Infinity);
                text.Arrange(new Rect(text.DesiredSize));

                double pictureTopY = (canvas.Bounds.Height - itemHeight - marginTB - tmpText.Bounds.Height) / 2;
                double leftX = marginLR - itemWidth / 2;
                double topY = pictureTopY;
                DeliveryServicesPositions.Add(_deliveryServices.First(), new EntityPosition { Left = leftX, Top = topY });

                double leftXCorrected = leftX - bounds.Right / 2 + bounds.Right * scalefactor / 2;
                double topYCorrected = topY - bounds.Height / 2 + bounds.Height * scalefactor / 2;
                Canvas.SetLeft(deliveryServiceImagePath, leftXCorrected);
                Canvas.SetTop(deliveryServiceImagePath, topYCorrected);

                double pictureCenterX = canvas.Bounds.Width / 2;

                Canvas.SetLeft(text, marginLR - text.Bounds.Right / 2);
                Canvas.SetTop(text, pictureTopY + itemHeight + marginTB);
                return;
            }

            double interlace = (canvas.Bounds.Height - marginTB * 2 - marginTop - marginBottom) / (_deliveryServices.Count());
            int counter = 0;
            foreach (DeliveryService service in _deliveryServices)
            {
                (Path serviceImagePath, TextBlock text) = service.CreateImage(COLORS[(counter + 1) % COLORS.Length], scalefactor);
                canvas.Children.Add(serviceImagePath);
                canvas.Children.Add(text);

                text.Measure(new Size(itemWidth * 2, double.PositiveInfinity));
                text.MaxWidth = itemWidth * 2;
                text.Arrange(new Rect(text.DesiredSize));

                double leftX = marginLR - itemWidth / 2;
                double topY = marginTop + marginTB + interlace / 2 + interlace * counter - (itemHeight + marginTB + text.Bounds.Height) / (_deliveryServices.Count()) * counter;
                DeliveryServicesPositions.Add(service, new EntityPosition { Left = leftX, Top = topY });

                double leftXCorrected = leftX - bounds.Right / 2 + bounds.Right * scalefactor / 2;
                double topYCorrected = topY - bounds.Height / 2 + bounds.Height * scalefactor / 2;
                Canvas.SetLeft(serviceImagePath, leftXCorrected);
                Canvas.SetTop(serviceImagePath, topYCorrected);

                Canvas.SetLeft(text, marginLR - text.Bounds.Right / 2);
                Canvas.SetTop(text, marginTop + marginTB + interlace / 2 + interlace * counter - (itemHeight + marginTB + text.Bounds.Height) / (_deliveryServices.Count()) * counter + itemHeight + marginTB);

                counter++;
            }
        }

        public void DrawAllShops(Canvas canvas, double scalefactor)
        {
            ShopsPositions = [];
            var path = Geometry.Parse(ModelsAnimationHelper.SHOPIMAGE);

            var bounds = path.Bounds;
            double itemWidth = bounds.Right * scalefactor;
            double marginLR = itemWidth;
            double itemHeight = bounds.Bottom * scalefactor;

            double marginTB = itemHeight / 4;


            if (_shops.Count() == 1)
            {
                (Path shopImagePath, TextBlock text) = _shops.First().CreateImage(COLORS[1], scalefactor);
                canvas.Children.Add(shopImagePath);
                canvas.Children.Add(text);

                text.Measure(Size.Infinity);
                text.Arrange(new Rect(text.DesiredSize));

                double leftX = (canvas.Bounds.Width - itemWidth) / 2;
                double topY = (canvas.Bounds.Height - marginTB - itemHeight - text.Bounds.Height);
                ShopsPositions.Add(_shops.First(), new EntityPosition { Left = leftX, Top = topY });

                double leftXCorrected = leftX - bounds.Right / 2 + bounds.Right * scalefactor / 2;
                double topYCorrected = topY - bounds.Bottom / 2 + bounds.Bottom * scalefactor / 2;
                Canvas.SetLeft(shopImagePath, leftXCorrected);
                Canvas.SetTop(shopImagePath, topYCorrected);

                double pictureCenterX = canvas.Bounds.Width / 2;
                double pictureTopY = (canvas.Bounds.Height - itemHeight - marginTB / 2) - bounds.Bottom / 2 + bounds.Bottom * scalefactor / 2;

                Canvas.SetLeft(text, pictureCenterX - text.Bounds.Right / 2);
                Canvas.SetTop(text, canvas.Bounds.Height - text.Bounds.Bottom - marginTB / 2);
                return;
            }

            double interlace = (canvas.Bounds.Width - marginLR * 2) / (_shops.Count() + 1);
            int counter = 1;
            foreach (Shop shop in _shops)
            {
                (Path shopImagePath, TextBlock text) = shop.CreateImage(COLORS[counter % COLORS.Length], scalefactor);
                canvas.Children.Add(shopImagePath);
                canvas.Children.Add(text);

                text.Measure(new Size(itemWidth * 2, double.PositiveInfinity));
                text.MaxWidth = itemWidth * 2;
                text.Arrange(new Rect(text.DesiredSize));

                double leftX = marginLR + interlace * counter - itemWidth / 2;
                double topY = (canvas.Bounds.Height - itemHeight - marginTB - text.Bounds.Height);
                ShopsPositions.Add(shop, new EntityPosition { Left = leftX, Top = topY });

                double leftXCorrected = leftX - bounds.Right / 2 + bounds.Right * scalefactor / 2;
                double topYCorrected = topY - bounds.Bottom / 2 + bounds.Bottom * scalefactor / 2;
                Canvas.SetLeft(shopImagePath, leftXCorrected);
                Canvas.SetTop(shopImagePath, topYCorrected);

                double textCenterX = marginLR + interlace * counter;

                Canvas.SetLeft(text, textCenterX - text.Bounds.Right / 2);
                Canvas.SetTop(text, canvas.Bounds.Height - text.Bounds.Bottom - marginTB / 2);

                counter++;
            }
        }
        public void DrawAllVehicles(Canvas canvas, double scalefactor)
        {
            var path = Geometry.Parse(ModelsAnimationHelper.VEHICLEIMAGE);

            var bounds = path.Bounds;
            double itemWidth = bounds.Right * scalefactor;
            double marginLR = itemWidth;
            double itemHeight = bounds.Bottom * scalefactor;

            int counter = 1;
            foreach (DeliveryService service in _deliveryServices)
            {

                lock (service.Vehicles)
                {
                    foreach (DeliveryServiceVehicle vehicle in service.Vehicles.Where(v => v.Order != null))
                    {
                        Path vehicleImagePath = vehicle.CreateImage(COLORS[counter % COLORS.Length], scalefactor);
                        canvas.Children.Add(vehicleImagePath);

                        EntityPosition position = GetRawPosition(vehicle, service);

                        double leftX = position.Left - bounds.Right / 2 + bounds.Right * scalefactor / 2;
                        double topY = position.Top - bounds.Bottom / 2 + bounds.Bottom * scalefactor / 2;

                        Canvas.SetLeft(vehicleImagePath, leftX);
                        Canvas.SetTop(vehicleImagePath, topY);
                    }
                }
                counter++;
            }
        }

        private EntityPosition GetRawPosition(DeliveryServiceVehicle vehicle, DeliveryService deliveryService)
        {
            EntityPosition start = vehicle.CurrentDirection switch
            {
                DeliveryServiceVehicle.Direction.ToShop =>  DeliveryServicesPositions[deliveryService],
                DeliveryServiceVehicle.Direction.ToClient => ShopsPositions[vehicle.Order.Shop],
                DeliveryServiceVehicle.Direction.ToBase => CustomersPositions[vehicle.Order.Customer],
            };

            EntityPosition finish = vehicle.CurrentDirection switch
            {
                DeliveryServiceVehicle.Direction.ToShop => ShopsPositions[vehicle.Order.Shop],
                DeliveryServiceVehicle.Direction.ToClient => CustomersPositions[vehicle.Order.Customer],
                DeliveryServiceVehicle.Direction.ToBase => DeliveryServicesPositions[deliveryService],
            };
            double realDistance = vehicle.CurrentDirection switch
            {
                DeliveryServiceVehicle.Direction.ToShop => deliveryService.GetDistanceToShop(vehicle),
                DeliveryServiceVehicle.Direction.ToClient => deliveryService.GetDistanceToClient(vehicle),
                DeliveryServiceVehicle.Direction.ToBase =>  deliveryService.GetDistanceToBase(vehicle),
            };

            double dX = start.Left - finish.Left;
            double dY = start.Top - finish.Top;

            double fullScreenDistance = Math.Sqrt(Math.Pow(dX, 2) + Math.Pow(dY, 2));
            double vehicleLeftScreenDistance = fullScreenDistance / realDistance * vehicle.Distance;

            double relation = fullScreenDistance / vehicleLeftScreenDistance;

            return new EntityPosition
            {
                Left = finish.Left + dX / relation,
                Top = finish.Top + dY / relation,
            };
        }
    }
}
