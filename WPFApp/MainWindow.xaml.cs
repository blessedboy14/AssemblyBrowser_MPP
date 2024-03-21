using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WPFApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            DataContext = new MainWindowViewModel();
        }
        private TreeViewItem CreateTreeViewItem(string imagePath, string text)
        {
            StackPanel stackPanel = new() { Orientation = Orientation.Horizontal };

            stackPanel.Children.Add(new Image()
            {
                Width = 16,
                Height = 16,
                Margin = new Thickness(0, 0, 5, 0),
                Source = new BitmapImage(new Uri(imagePath))
            });

            stackPanel.Children.Add(new TextBlock() { Text = text });

            return new TreeViewItem()
            {
                Header = stackPanel,
                Margin = new Thickness(0, 5, 0, 0),
                Focusable = false
            };
        }

    }
}