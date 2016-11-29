using System;
using System.Collections.Generic;
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
using System.Windows.Threading;

namespace MatchingGameWPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        protected Random rand;
        protected DispatcherTimer matchFailTimer;
        protected DispatcherTimer choiceDelayTimer;
        protected DispatcherTimer countTimer;

        protected int count;
        protected List<string> symbols;

        protected Brush showSymbol;
        protected Label firstClicked;
        protected Label secondClicked;

        public MainWindow()
        {
            InitializeComponent();

            rand = new Random();
            matchFailTimer = new DispatcherTimer();
            matchFailTimer.Tick += new EventHandler(matchFailTimer_Tick);
            matchFailTimer.Interval = new TimeSpan(0, 0, 0, 0, 750);

            choiceDelayTimer = new DispatcherTimer();
            choiceDelayTimer.Tick += new EventHandler(choiceDelayTimer_Tick);
            choiceDelayTimer.Interval = new TimeSpan(0, 0, 0, 2, 0);

            countTimer = new DispatcherTimer();
            countTimer.Tick += new EventHandler(countTimer_Tick);
            countTimer.Interval = new TimeSpan(0, 0, 1);

            showSymbol = new SolidColorBrush(Colors.Black);

            AssignIconsToSquares();
        }

        /// <summary>
        /// Assign each icon from the list of icons to a random square
        /// </summary>
        private void AssignIconsToSquares()
        {
            count = 0;
            symbols = new List<string>()
            {
                "!", "!", "N", "N", ",", ",", "k", "k",
                "b", "b", "v", "v", "w", "w", "z", "z"
            };
            firstClicked = null;
            secondClicked = null;

            // The TableLayoutPanel has 16 labels,
            // and the icon list has 16 icons,
            // so an icon is pulled at random from the list
            // and added to each label
            foreach (UIElement element in matchGrid.Children)
            {
                Label iconLabel = element as Label;
                if (iconLabel != null)
                {
                    int randomNumber = rand.Next(symbols.Count);
                    iconLabel.Content = symbols[randomNumber];
                    iconLabel.Foreground = iconLabel.Background;
                    symbols.RemoveAt(randomNumber);
                }
            }

            countTimer.Start();
        }

        /// <summary>
        /// Every label's Click event is handled by this event handler
        /// </summary>
        /// <param name="sender">The label that was clicked</param>
        /// <param name="e"></param>
        private void label_MouseDown(object sender, MouseButtonEventArgs e)
        {
            // The timer is only on after two non-matching 
            // icons have been shown to the player, 
            // so ignore any clicks if the timer is running
            if (matchFailTimer.IsEnabled == true)
                return;

            Label clickedLabel = sender as Label;

            if (clickedLabel != null)
            {
                // If the clicked label is black, the player clicked
                // an icon that's already been revealed --
                // ignore the click
                if (clickedLabel.Foreground == showSymbol)
                    return;

                // If firstClicked is null, this is the first icon 
                // in the pair that the player clicked,
                // so set firstClicked to the label that the player 
                // clicked, change its color to black, and return
                if (firstClicked == null)
                {
                    firstClicked = clickedLabel;
                    firstClicked.Foreground = showSymbol;
                    choiceDelayTimer.Start();

                    return;
                }

                // If the player gets this far, the timer isn't
                // running and firstClicked isn't null,
                // so this must be the second icon the player clicked
                // Set its color to black
                secondClicked = clickedLabel;
                secondClicked.Foreground = showSymbol;
                choiceDelayTimer.Stop();

                // Check to see if the player won
                CheckForWinner();

                // If the player clicked two matching icons, keep them 
                // black and reset firstClicked and secondClicked 
                // so the player can click another icon
                if (firstClicked.Content == secondClicked.Content)
                {
                    firstClicked = null;
                    secondClicked = null;
                    return;
                }

                // If the player gets this far, the player 
                // clicked two different icons, so start the 
                // timer (which will wait three quarters of 
                // a second, and then hide the icons)
                matchFailTimer.Start();
            }
        }

        /// <summary>
        /// Check every icon to see if it is matched, by 
        /// comparing its foreground color to its background color. 
        /// If all of the icons are matched, the player wins
        /// </summary>
        private void CheckForWinner()
        {
            // Go through all of the labels in the TableLayoutPanel, 
            // checking each one to see if its icon is matched
            foreach (UIElement element in matchGrid.Children)
            {
                Label iconLabel = element as Label;
                if (iconLabel != null)
                {
                    if (iconLabel.Foreground == iconLabel.Background)
                        return;
                }
            }

            // If the loop didn’t return, it didn't find
            // any unmatched icons
            // That means the user won. Show a message and close the form
            MessageBox.Show("You matched all the icons in " + count + " seconds!", "Congratulations");
            Close();
        }

        /// <summary>
        /// This timer is started when the player clicks 
        /// two icons that don't match,
        /// so it counts three quarters of a second 
        /// and then turns itself off and hides both icons
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void matchFailTimer_Tick(object sender, EventArgs e)
        {
            // Stop the timer
            matchFailTimer.Stop();

            // Hide both icons
            firstClicked.Foreground = firstClicked.Background;
            secondClicked.Foreground = secondClicked.Background;

            // Reset firstClicked and secondClicked 
            // so the next time a label is
            // clicked, the program knows it's the first click
            firstClicked = null;
            secondClicked = null;
        }

        private void choiceDelayTimer_Tick(object sender, EventArgs e)
        {
            choiceDelayTimer.Start();
            firstClicked.Foreground = firstClicked.Background;
            firstClicked = null;
        }

        private void countTimer_Tick(object sender, EventArgs e)
        {
            count += 1;
        }
    }
}