using AssemblyAnalyzer;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace WPFApp
{
    public class ViewModelBase : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;
        public void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    public class AssemblyViewModel: ViewModelBase
    {
        private AnalyzerResult _result;
        public AnalyzerResult Result
        {
            get { return _result; }
            set { _result = value; OnPropertyChanged("Result"); }
        }

        public AssemblyViewModel(AnalyzerResult result)
        {
            _result = result;
        }
    }

    public class MainWindowViewModel : ViewModelBase
    {
        private List<AssemblyViewModel> _assemblies;

        public List<AssemblyViewModel> Assemblies
        {
            get { return _assemblies; }
            set { _assemblies = value; OnPropertyChanged("AssemblyList"); }
        }

        public ICommand ButtonClickCommand => new OnClickCommand(OpenFile);

        public MainWindowViewModel()
        {
            _assemblies = new List<AssemblyViewModel>();
        }

        private void OpenFile()
        {
            var openFileDialog = new OpenFileDialog
            {
                Filter = "Assemblies|*.dll;*.exe",
                Title = "Select assembly",
                Multiselect = false
            };
            var isOpen = openFileDialog.ShowDialog();
            if (isOpen != null && isOpen.Value)
            {
                try
                {
                    Assemblies = new List<AssemblyViewModel>(Assemblies)
                    {
                        new AssemblyViewModel(Analyzer.Analyze(openFileDialog.FileName))
                    };
                }
                catch (AnalyzerException ex)
                {
                    System.Windows.MessageBox.Show(ex.Message);
                }
            }
            else
            {
                System.Windows.MessageBox.Show("Choose correct assembly file");
            }
        }
    }
}
