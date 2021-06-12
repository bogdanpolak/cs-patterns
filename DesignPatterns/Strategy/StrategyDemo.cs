using System;

namespace DesignPatterns.Strategy
{
    public static class StrategyDemo
    {
        public static void RunDemo()
        {
            var resolution = new Resolution(400, 200);
            var pictureResizer = new PictureResizer(resolution, new FakeCropCalculator());

            var _ = pictureResizer.Resize(new Image
            {
                Width = 200, Height = 120,
                ContentRect = new Rect(50, 20, 150, 90)
            });
        }
    }
    
    // --------------------------------------------------------------------
    // Model
    // --------------------------------------------------------------------

    public class Rect
    {
        public int Left { get; }
        public int Top { get; }
        public int Width { get; }
        public int Height { get; }
        public Rect(int left, int top, int width, int height)
        {
            Left = left;
            Top = top;
            Width = width;
            Height = height;
        }
    }
    
    public class Image
    {
        public int Width;
        public int Height;
        public Rect ContentRect;
    }

    public class Resolution
    {
        public int Width { get; }
        public int Height { get; }
        public Resolution(int width, int height)
        {
            Width = width;
            Height = height;
        }
    }
    
    // --------------------------------------------------------------------
    // Strategy context:    PictureResizer
    // --------------------------------------------------------------------

    public class PictureResizer
    {
        private readonly Resolution _resolution;
        private readonly ICropRectCalculator _cropRectCalculator;

        public PictureResizer(Resolution resolution, ICropRectCalculator cropRectCalculator)
        {
            _resolution = resolution;
            _cropRectCalculator = cropRectCalculator;
        }

        public Image Resize(Image image)
        {
            var rect = _cropRectCalculator.Calculate(image, _resolution);
            var croppedImage = DoCrop(image, rect);
            var resizedImage = DoResize(croppedImage, _resolution);
            return resizedImage;
        }

        private static Image DoCrop(Image image, Rect rect)
        {
            var contentRect = image.ContentRect;
            return new Image
            {
                Width = rect.Width,
                Height = rect.Height,
                ContentRect = new Rect(
                    contentRect.Left - rect.Left, 
                    contentRect.Top - rect.Top, 
                    contentRect.Width, 
                    contentRect.Height)
            };
        }
        
        private static Image DoResize(Image image, Resolution resolution)
        {
            var rect = image.ContentRect;
            var xMulti = (float)resolution.Width / image.Width;
            var yMulti = (float)resolution.Height / image.Height;
            return new Image
            {
                Width = resolution.Width,
                Height = resolution.Height,
                ContentRect = new Rect(
                    (int)Math.Floor(rect.Left * xMulti) , 
                    (int)Math.Round(rect.Top * yMulti), 
                    (int)Math.Round(rect.Width * xMulti, MidpointRounding.AwayFromZero), 
                    (int)Math.Round(rect.Height * yMulti, MidpointRounding.AwayFromZero)
                )
            };
        }
    }

    // --------------------------------------------------------------------
    // Strategy pattern:    Contract
    // --------------------------------------------------------------------

    public interface ICropRectCalculator
    {
        Rect Calculate(Image image, Resolution resolution);
    }
    
    // --------------------------------------------------------------------
    // Strategy pattern:    Strategies
    // --------------------------------------------------------------------

    public class FakeCropCalculator : ICropRectCalculator
    {
        public Rect Calculate(Image image, Resolution resolution)
        {
            return new Rect(0, 0, 200, 100);
        }
    }
}