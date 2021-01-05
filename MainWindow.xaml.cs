using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
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

namespace AsyncAwait
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }
        string readFileAsync(string fileName)
        {
            StreamReader reader = new StreamReader(fileName);
            string line = null;
            string totalString = null;
            while ((line = reader.ReadLine()) != null)
            {
                totalString = totalString + line;
            }
            return totalString;
        }

        private async void ButtonAsync_Click(object sender, RoutedEventArgs e)
        {
            var watch = System.Diagnostics.Stopwatch.StartNew();
            await AddingTaskAsync(); 
            watch.Stop();
            var secs = watch.ElapsedMilliseconds;
            ElapsedTime.Text += "Total Elapsed Time in Asynchronous format  :   " + secs + "\n";
        }

        private async Task AddingTaskAsync()
        {
            DataRepository dataRepository = new DataRepository();
            List<Task<string>> taskList = new List<Task<string>>();
            foreach (string link in dataRepository.dataRepositoryList)
            {
                taskList.Add((Task.Run(() => readFileAsync(link))));
            }
            var entireData = await Task.WhenAll(taskList);   
            foreach(string eachPersonBiography in entireData)
            {
                getDataOnWindowForm(eachPersonBiography);
            }
        }

        private void getDataOnWindowForm(string data)
        {
           NumberOfWords.Text += "Number of words  :  " + data.Length + Environment.NewLine;
        }

        private async void ButtonSync_Click(object sender, RoutedEventArgs e)
        {
            var watch = System.Diagnostics.Stopwatch.StartNew();
            await AddingTaskSync();
            watch.Stop();
            var secs = watch.ElapsedMilliseconds;
            ElapsedTime.Text += "Total Elapsed Time in Synchronous format    :   " + secs + "\n";
        }
        private async Task AddingTaskSync()
        {
            DataRepository dataRepo = new DataRepository();
            foreach (string str in dataRepo.dataRepositoryList)
            {
               var biography =  await  Task.Run(() => readFileAsync(str));
                getDataOnWindowForm(biography);
            }
        }

    }
}
