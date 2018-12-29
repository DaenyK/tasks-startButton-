using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Text.RegularExpressions;
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

namespace tasks_startButton_
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            Task t = new Task(WriteToFileTask2);
            t.Start();
            Task t2 = new Task(WriteToFileTask3);
            t2.Start();
        }

        private void WriteToFileTask3()
        {
            string path = @"task3.txt";
            string text = "07:05:45PM 04:45:00AM 10:14:12PM 03:11:22PM 04:07:25AM";

            try
            {
                using (StreamWriter sw = new StreamWriter(path, false, System.Text.Encoding.Default))
                {
                    sw.WriteLine(text);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        private void WriteToFileTask2()
        {
            string path = @"task2.txt";
            string text = "6 85 19 44 78";

            try
            {
                using (StreamWriter sw = new StreamWriter(path, false, System.Text.Encoding.Default))
                {
                    sw.WriteLine(text);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        private void BtnStart_Click(object sender, RoutedEventArgs e)
        {
            Thread t1 = new Thread(() => { GetFactorial(); });
            t1.IsBackground = true;
            Thread t2 = new Thread(() => { ConvertTime(); });
            t2.IsBackground = true;

            t1.Start();
            t2.Start();
        }

        AutoResetEvent m1 = new AutoResetEvent(false);
        AutoResetEvent m2 = new AutoResetEvent(false);


        private void GetFactorial()
        {
            string path = @"task2.txt";
            string text = "";
            try
            {
                using (StreamReader sr = new StreamReader(path))
                {
                    text = sr.ReadToEnd();
                }
                int[] nums = text.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries).Select(int.Parse).ToArray();

                m1.WaitOne();
                Dispatcher.Invoke(() =>
                {
                    lblTask2.Content = text;
                });

                using (StreamWriter stream = new StreamWriter(path, true, System.Text.Encoding.Default))
                {
                    BigInteger result;
                    for (int i = 0; i < nums.Length; i++)
                    {
                        if (nums[i] > 0 && nums[i] < 101)
                        {
                            result = FactNaive(nums[i]);
                            stream.WriteLine();
                            stream.Write(string.Format("{0}-->{1}", (nums[i]), FactNaive(nums[i])));
                        }
                        else
                            MessageBox.Show(nums[i].ToString() + " doesn't fit the task");
                    }
                }

                m2.WaitOne();
                Dispatcher.Invoke(() =>
                {
                    lblResultTask2.Content = "recording completed";
                });
            }
            catch (Exception e)
            {
                MessageBox.Show("error in task 2: " + e.Message);
            }
            finally
            {
                Process.Start("notepad.exe", path);
            }
        }
        private void ConvertTime()
        {
            string path = @"task3.txt";
            string text = "";
            try
            {
                using (StreamReader sr = new StreamReader(path))
                {
                    text = sr.ReadToEnd();
                }
                string[] arr = Regex.Split(text, " ");

                Dispatcher.Invoke(() =>
                {
                    lblTask3.Content = text;

                });
                m1.Set();

                using (StreamWriter stream = new StreamWriter(path, true, System.Text.Encoding.Default))
                {
                    DateTime result;
                    for (int i = 0; i < arr.Length; i++)
                    {
                        result = Convert.ToDateTime(arr[i]);
                        stream.WriteLine();
                        stream.WriteLine(result.ToString("HH:mm:ss"));
                    }
                }

                Dispatcher.Invoke(() =>
                {
                    lblResultTask3.Content = "recording completed";

                });
                m2.Set();
            }
            catch (Exception e)
            {
                MessageBox.Show("error in task 2: " + e.Message);
            }
            finally
            {
                Process.Start("notepad.exe", path);
            }
        }
        static BigInteger FactNaive(int n)
        {
            BigInteger r = 1;
            for (int i = 2; i <= n; ++i)
                r *= i;
            return r;
        }
    }
}
