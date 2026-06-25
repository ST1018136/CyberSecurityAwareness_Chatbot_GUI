using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace CyberSecurityAwareness_Chatbot_GUI
{
    // the balloon represents a single balloon for the simulato 
   
    public class Balloon
    {

        public Ellipse Body { get; set; } // the balloon body
        public Polygon String { get; set; }
        public double Speed { get; set; } // balloon speed
        public double XPosition { get; set; } // Horizontal

        public Balloon(double x,  double y, Color color,
            double size)

        {
            // creates the balloon

            Body = new Ellipse
            {
                Width = size,
                Height = size * 1.2,
                Fill = new SolidColorBrush(color), Opacity = 0.9 // transparent

            };

            Canvas.SetLeft(Body, x);
            Canvas.SetTop(Body, y);

            // create the balloon string
            String = new Polygon
            {
                Points = new PointCollection
                {
                    new Point(x + size/2, y + size * 1.2),
                    new Point(x + size/2 - 2, y + size * 1.5),
                    new Point(x + size/2 + 2, y + size * 1.5)
                },
                Fill = new SolidColorBrush(Colors.YellowGreen),
                Opacity = 0.6
            };

            // random speed
            Speed = 0.5 + new Random().NextDouble() * 1.5;
            XPosition = x;

        }
    }
}
