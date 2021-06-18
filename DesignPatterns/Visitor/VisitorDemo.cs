using System;
using System.Collections.Generic;
using System.Linq;

namespace DesignPatterns.Visitor
{
    public static class VisitorDemo
    {
        public static void RunDemo()
        {
            var image = new Image
            {
                Texts = new List<Text>
                {
                    new Text(10,20,"Image title"),
                    new Text(110,50,"158361"),
                    new Text(210,60,"TI-8267"),
                    new Text(20,70,"273913"),
                }
            };
            var allHotspots = new AllHotspots()
                .GetFromImage(image,new HotspotVisitor());
            var numbersOnlyHotspots = new NumbersOnlyHotspots()
                .GetFromImage(image,new HotspotVisitor());
            
            Console.WriteLine("All Hotspots:  "+
                              string.Join(", ", allHotspots.Items));
            Console.WriteLine("Numbers Only Hotspots:  "+
                              string.Join(", ", numbersOnlyHotspots.Items));
        }
    }
    
    // --------------------------------------------------------------------
    // Model
    // --------------------------------------------------------------------

    public class Text
    {
        public int Left { get; }
        public int Top { get; }
        public string Content { get; }
        
        public Text(int left, int top, string content)
        {
            Left = left;
            Top = top;
            Content = content;
        }
    }
    
    public class Image
    {
        public List<Text> Texts;
    }

    public class Hotspot
    {
        private readonly int _left;
        private readonly int _top;
        private readonly string _value;
        
        public Hotspot(int left, int top, string value)
        {
            _left = left;
            _top = top;
            _value = value;
        }

        public override string ToString()
            => $"({_left},{_top},\"{_value}\")";
    }

    public abstract class Hotspots
    {
        public List<Hotspot> Items = new List<Hotspot>();
        
        /// <summary>
        /// GetFromImage = Visit method in visitor pattern
        /// </summary>
        public abstract Hotspots GetFromImage(Image image, IHotspotVisitor hotspotVisitor);
    }

    public class AllHotspots : Hotspots
    {
        public override Hotspots GetFromImage(Image image, IHotspotVisitor hotspotVisitor)
        {
            Items = hotspotVisitor.ExtractHotspots_All(image);
            return this;
        }
    }
    
    public class NumbersOnlyHotspots : Hotspots
    {
        public override Hotspots GetFromImage(Image image, IHotspotVisitor hotspotVisitor)
        {
            Items = hotspotVisitor.ExtractHotspots_WithNumbers(image);
            return this;
        }
    }

    // --------------------------------------------------------------------
    // Visitor Contract
    // --------------------------------------------------------------------

    public interface IHotspotVisitor
    {
        List<Hotspot> ExtractHotspots_All(Image image);
        List<Hotspot> ExtractHotspots_WithNumbers(Image image);
    }
    
    // --------------------------------------------------------------------
    // Visitor
    // --------------------------------------------------------------------

    public class HotspotVisitor : IHotspotVisitor
    {
        public List<Hotspot> ExtractHotspots_All(Image image)
        {
            return image.Texts
                .Select(txt => new Hotspot(txt.Left,txt.Top,txt.Content))
                .ToList();
        }

        public List<Hotspot> ExtractHotspots_WithNumbers(Image image)
        {
            return image.Texts
                .Where(txt => txt.Content.ContainsDigitsOnly())
                .Select(txt => new Hotspot(txt.Left,txt.Top,txt.Content))
                .ToList();
        }
    }

    public static class StringExtensions
    {
        public static bool ContainsDigitsOnly(this string str)
        {
            foreach (char c in str)
            {
                if (c < '0' || c > '9') return false;
            }
            return true;
        }
    }
}