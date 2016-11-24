using System;
using System.Collections.Generic;
using System.Linq;
using System.Media;
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
using System.Windows.Threading;

namespace MathQuizWPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Random rand;
        DispatcherTimer dispatcherTimer;

        int timeLeft;

        int plusLeft;
        int plusRight;
        int minusLeft;
        int minusRight;
        int multiplyLeft;
        int multiplyRight;
        int divideLeft;
        int divideRight;

        public MainWindow()
        {
            InitializeComponent();

            rand = new Random();
            dispatcherTimer = new DispatcherTimer();
            dispatcherTimer.Tick += new EventHandler(dispatcherTimer_Tick);
            dispatcherTimer.Interval = new TimeSpan(0, 0, 1);
        }

        public void StartQuiz()
        {
            plusLeft = rand.Next(51);
            plusRight = rand.Next(51);
            minusLeft = rand.Next(1, 101);
            minusRight = rand.Next(1, minusLeft);
            multiplyLeft = rand.Next(2, 11);
            multiplyRight = rand.Next(2, 11);

            int tempQuotient = rand.Next(2, 11);
            divideRight = rand.Next(2, 11);
            divideLeft = divideRight * tempQuotient;

            plusLeftLabel.Content = plusLeft.ToString();
            plusRightLabel.Content = plusRight.ToString();
            minusLeftLabel.Content = minusLeft.ToString();
            minusRightLabel.Content = minusRight.ToString();
            multiplyLeftLabel.Content = multiplyLeft.ToString();
            multiplyRightLabel.Content = multiplyRight.ToString();
            divideLeftLabel.Content = divideLeft.ToString();
            divideRightLabel.Content = divideRight.ToString();

            timeLeft = 30;
            countdownLabel.Content = "30 seconds";
            plusAnswer.Value = null;
            minusAnswer.Value = null;
            multiplyAnswer.Value = null;
            divideAnswer.Value = null;
            dispatcherTimer.Start();
        }

        public bool CheckAnswers()
        {
            if ((plusLeft + plusRight == plusAnswer.Value) && 
                (minusLeft - minusRight == minusAnswer.Value) &&
                (multiplyLeft * multiplyRight == multiplyAnswer.Value) && 
                (divideLeft / divideRight == divideAnswer.Value))
                return true;
            else
                return false;
        }

        private void startButton_Click(object sender, RoutedEventArgs e)
        {
            StartQuiz();
            startButton.IsEnabled = false;
            countdownLabel.Background = new SolidColorBrush(Colors.White);
        }

        private void dispatcherTimer_Tick(object sender, EventArgs e)
        {
            if (CheckAnswers())
            {
                dispatcherTimer.Stop();
                MessageBox.Show("You got all the answers right!", "Congratulations!");
                startButton.IsEnabled = true;
            }
            else if (timeLeft > 0)
            {
                // Display the new time left
                // by updating the Time Left label.
                timeLeft = timeLeft - 1;
                countdownLabel.Content = timeLeft + " seconds";
            }
            else
            {
                // If the user ran out of time, stop the timer, show
                // a MessageBox, and fill in the answers.
                dispatcherTimer.Stop();
                timeLabel.Content = "Time's up!";
                MessageBox.Show("You didn't finish in time.", "Sorry!");
                plusAnswer.Value = plusLeft + plusRight;
                minusAnswer.Value = minusLeft - minusRight;
                multiplyAnswer.Value = multiplyLeft * multiplyRight;
                divideAnswer.Value = divideLeft / divideRight;
                startButton.IsEnabled = true;
            }

            if (timeLeft <= 5)
            {
                countdownLabel.Background = new SolidColorBrush(Colors.Red);
            }
        }

        private void plusAnswer_ValueChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            if (plusLeft + plusRight == plusAnswer.Value)
                SystemSounds.Beep.Play();
        }

        private void minusAnswer_ValueChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            if (minusLeft - minusRight == minusAnswer.Value)
                SystemSounds.Beep.Play();
        }

        private void multiplyAnswer_ValueChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            if (multiplyLeft * multiplyRight == multiplyAnswer.Value)
                SystemSounds.Beep.Play();
        }

        private void divideAnswer_ValueChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            if (divideLeft / divideRight == divideAnswer.Value)
                SystemSounds.Beep.Play();
        }
    }
}
