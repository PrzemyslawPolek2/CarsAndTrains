﻿using CarsAndTrains.Classes;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace CarsAndTrains
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private const string NODE_POSITION_FILE_NAME = "/nodePositions.txt";
        public static MainWindow GetMain;
        public static bool CreateNode;
        
        public MainWindow()
        {
            GetMain = this;
            InitializeComponent();
            PublicAvaliableReferences.Initialize(canvas);
            clickPositionL.Content = "[" + GetMain.GetCanvasHeight() + ";" + GetMain.GetCanvasWidth() + "]";
            Thread thread =
            new Thread(
              _ => MoveFakeCar(exampleCar, new Point(1, 0))
            );
            thread.Start();
        }

        private void CanvasMouseDownEventHandler(object sender, MouseButtonEventArgs e)
        {
            Point p = Mouse.GetPosition(canvas);
            string str = p.X + " " + p.Y;

            clickPositionL.Content = str;

            if (!CreateNode)
                return;
            CreateNodePosition(str);
        }

        private static void CreateNodePosition(string str)
        {
            string path = System.Reflection.Assembly.GetEntryAssembly().Location;
            path = System.IO.Path.GetDirectoryName(path) + NODE_POSITION_FILE_NAME;

            if (!File.Exists(path))
            {
                using (StreamWriter sw = File.CreateText(path))
                {
                    sw.WriteLine(str);
                }
            }
            else
            {
                using (StreamWriter sw = File.AppendText(path))
                {
                    sw.WriteLine(str);
                }
            }
        }

        public double GetCanvasWidth()
        {
            return canvas.Width;
        }
        public double GetCanvasHeight()
        {
            return canvas.Height;
        }

        private void MoveFakeCar(UIElement movedObject, Point movementVector)
        {
            double xMovement = movementVector.X;
            double yMovement = movementVector.Y;
            double speed = 1.0f;
            for (int i = 0; i < 10000; i++)
            {
                this.Dispatcher.Invoke(() =>
                {
                    Canvas.SetLeft(movedObject, Canvas.GetLeft(movedObject) + speed * xMovement);
                    Canvas.SetTop(movedObject, Canvas.GetTop(movedObject) + speed * yMovement);
                });
                Thread.Sleep(10);
            }
        }
    }
}
