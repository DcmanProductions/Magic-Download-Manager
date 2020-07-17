using com.drewchaseproject.MDM.Library.Data;
using com.drewchaseproject.MDM.Library.Data.DB;
using System;
using System.Windows;
using System.Windows.Input;

namespace Launcher
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public static MainWindow Singleton { get; private set; }

        public MainWindow()
        {
            InitializeComponent();
            Singleton = this;
            Setup();
            RegisterEvents();
        }

        private void Setup()
        {
            if (Activation.IsAuthorizedUser(Values.Singleton.Username, Values.Singleton.Password))
            {
                Main.Content = new Launching();
            }
            else
            {
                Main.Content = new Auth();
            }
        }

        private void RegisterEvents()
        {
            MouseLeftButtonDown += ( (object sender, MouseButtonEventArgs e) => DragMove() );
            ExitBtn.Click += ( (object sender, RoutedEventArgs e) => Environment.Exit(0) );
        }
    }
}
