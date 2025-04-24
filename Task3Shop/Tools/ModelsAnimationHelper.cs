using Avalonia.Controls;
using Avalonia.Controls.Shapes;
using Avalonia.Media;
using Task3Shop.Models;

namespace Task3Shop.Tools
{
    public static class ModelsAnimationHelper
    {
        public const string SHOPIMAGE = "M36.8 192l566.3 0c20.3 0 36.8-16.5 36.8-36.8c0-7.3-2.2-14.4-6.2-20.4L558.2 21.4C549.3 8 534.4 0 518.3 0L121.7 0c-16 0-31 8-39.9 21.4L6.2 134.7c-4 6.1-6.2 13.2-6.2 20.4C0 175.5 16.5 192 36.8 192zM64 224l0 160 0 80c0 26.5 21.5 48 48 48l224 0c26.5 0 48-21.5 48-48l0-80 0-160-64 0 0 160-192 0 0-160-64 0zm448 0l0 256c0 17.7 14.3 32 32 32s32-14.3 32-32l0-256-64 0z";
        public const string CUSTOMERIMAGE = "M224 256A128 128 0 1 0 224 0a128 128 0 1 0 0 256zm-45.7 48C79.8 304 0 383.8 0 482.3C0 498.7 13.3 512 29.7 512l388.6 0c16.4 0 29.7-13.3 29.7-29.7C448 383.8 368.2 304 269.7 304l-91.4 0z";
        public const string DELIVERYSERVICEIMAGE = "M0 488L0 171.3c0-26.2 15.9-49.7 40.2-59.4L308.1 4.8c7.6-3.1 16.1-3.1 23.8 0L599.8 111.9c24.3 9.7 40.2 33.3 40.2 59.4L640 488c0 13.3-10.7 24-24 24l-48 0c-13.3 0-24-10.7-24-24l0-264c0-17.7-14.3-32-32-32l-384 0c-17.7 0-32 14.3-32 32l0 264c0 13.3-10.7 24-24 24l-48 0c-13.3 0-24-10.7-24-24zm488 24l-336 0c-13.3 0-24-10.7-24-24l0-56 384 0 0 56c0 13.3-10.7 24-24 24zM128 400l0-64 384 0 0 64-384 0zm0-96l0-80 384 0 0 80-384 0z";

        public static (Path, TextBlock) CreateImage(this Shop shop, IBrush color, double scaleFactor)
        {
            var text = new TextBlock
            {
                Text = shop.Name,
                FontSize = 18,
                Foreground = color,
            };

            var path = new Path()
            {
                Data = Geometry.Parse(SHOPIMAGE),
                Stroke = color,
                Fill = color,
                StrokeThickness = 2,
                RenderTransform = new ScaleTransform(scaleFactor, scaleFactor),
            };
            return (path, text);
        }
        public static (Path, TextBlock) CreateImage(this Customer customer, IBrush color, double scaleFactor)
        {
            var text = new TextBlock
            {
                Text = customer.Name,
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
        public static (Path, TextBlock) CreateImage(this DeliveryService deliveryService, IBrush color, double scaleFactor)
        {
            var text = new TextBlock
            {
                Text = deliveryService.ServiceName,
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
    }
}
