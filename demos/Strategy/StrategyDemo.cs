using System;

namespace DesignPatterns.Strategy
{
    public static class StrategyDemo
    {
        public static void RunDemo()
        {
            var resolution = new Resolution(400, 200);
            var resizer1 = new PictureResizer(resolution, new MaxRealCropCalculator());
            var resizer2 = new PictureResizer(resolution, new NoCropCalculator());

            var image200x120 = new Image
            {
                Width = 200, Height = 120,
                ContentRect = new Rect(50, 20, 150, 90)
            };
            resizer1.Resize(image200x120);
            resizer2.Resize(image200x120);
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

        public override string ToString() 
            => $"Rect(({Left},{Top}),{Width}x{Height})";
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
            Console.WriteLine($"image: {image.Width}x{image.Height}"+
                              $" - Crop{rect} => {_resolution.Width}x{_resolution.Height}"+
                              $" - {_cropRectCalculator.GetStrategyName()}");
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
        string GetStrategyName();
    }
    
    // --------------------------------------------------------------------
    // Strategy pattern:    Strategies
    // --------------------------------------------------------------------

    public class NoCropCalculator : ICropRectCalculator
    {
        public Rect Calculate(Image image, Resolution resolution)
            => new Rect(0, 0, image.Width, image.Height);
        public string GetStrategyName() => "NoCrop Calculator";
    }

    public class MaxRealCropCalculator : ICropRectCalculator
    {
        public Rect Calculate(Image image, Resolution resolution)
        {
            var multi = (float)resolution.Width / resolution.Height;
            if (image.Width < image.Height * multi)
            {
                var hg = (int) (image.Width / multi);
                var top = (image.Height - hg) / 2;
                return new Rect(0, top, image.Width, hg);
            }
            var wd = (int)(image.Height*multi);
            var left = (image.Width - wd) / 2;
            return new Rect(left, 0, wd, image.Height);
        }
        public string GetStrategyName() => "MaxRealisticCrop Calculator";
    }
}