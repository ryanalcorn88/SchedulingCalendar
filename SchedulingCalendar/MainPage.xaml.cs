using System;
using System.Collections.Generic;
using System.Linq;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace SchedulingCalendar
{
    public sealed partial class MainPage : Page
    {
        private readonly Dictionary<string, string> times = new Dictionary<string, string>()
        {
            { "EightAM","8:00 A.M." },
            { "NineAM", "9:00 A.M." },
            { "TenAM", "10:00 A.M." },
            { "ElevenAM", "11:00 A.M." },
            { "TwelvePM", "12:00 P.M." },
            { "OnePM", "1:00 P.M." },
            { "TwoPM", "2:00 P.M." },
            { "ThreePM", "3:00 P.M." },
            { "FourPM", "4:00 P.M." }
        };

        private readonly Dictionary<int, string> months = new Dictionary<int, string>()
        {
            { 1, "January" },
            { 2, "February"},
            { 3, "March" },
            { 4, "April" },
            { 5, "May" },
            { 6, "June" },
            { 7, "July" },
            { 8, "August" },
            { 9, "September" },
            { 10, "October" },
            { 11, "November" },
            { 12, "December" }
        };

        public MainPage()
        {
            InitializeComponent();
            Calendar.SelectedDates.Add(DateTime.Now);
        }

        /// <summary>
        /// Changes the text displayed at the top of the screen based on what date was selected.
        /// </summary>
        private void Calendar_SelectedDatesChanged(CalendarView sender, CalendarViewSelectedDatesChangedEventArgs args)
        {
            var month = months[Calendar.SelectedDates[0].Month];

            if (Calendar.SelectedDates.Count > 0)
            {
                DisplayBox.Text = $"{month} {Calendar.SelectedDates[0].Day}, {Calendar.SelectedDates[0].Year}";
            }
        }

        /// <summary>
        /// Calls functions to change the text box information based on what row the update button was in.
        /// </summary>
        private void Update_Click(object sender, RoutedEventArgs e)
        {
            string textBlockName = FindTextBlockName(sender);

            UpdateTask(times[textBlockName]);
        }

        /// <summary>
        /// Calls functions to delete the text box information(if information is present) based on what row the update button was in.
        /// </summary>
        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            string textBlockName = FindTextBlockName(sender);
            TextBlock textBlockInRow = (TextBlock)FindName(textBlockName + "_Task");

            if(!textBlockInRow.Text.Equals(""))
            {
                DeleteTask(times[textBlockName], textBlockInRow.Text);
            }
            else
            {
                Flyout f = new Flyout();
                TextBlock t = new TextBlock
                {
                    Text = "No task scheduled at this time"
                };
                f.Content = t;
                f.ShowAt((FrameworkElement)sender);
            }
        }

        /// <summary>
        /// Finds the name of the text box at a time in order to change information in the box.
        /// </summary>
        /// <returns>The name of the text box in the same row as the Update or Delete button that was clicked.</returns>
        private string FindTextBlockName(object sender)
        {
            string buttonName = sender.GetType().GetProperty("Name").GetValue(sender).ToString();
            string textBlockName = buttonName.Split('_')[0];

            return textBlockName;
        }

        /// <summary>
        /// Creates a dialog box that allows the user to change appointment information.
        /// </summary>
        private async void UpdateTask(string timeToChange)
        {
            TextBox userEnteredInfo = new TextBox();
            ContentDialog updateDialog = new ContentDialog
            {
                Title = $"Update {timeToChange} task in the schedule?",
                Content = userEnteredInfo,
                PrimaryButtonText = "Submit",
                CloseButtonText = "Cancel"
            };

            ContentDialogResult result = await updateDialog.ShowAsync();

            if (result == ContentDialogResult.Primary)
            {
                ChangeTextBlockText(timeToChange, userEnteredInfo.Text);
            }
        }

        /// <summary>
        /// Creates a dialog box that allows the user to delete appointment information.
        /// </summary>
        private async void DeleteTask(string timeToChange, string currentInformation)
        {
            ContentDialog deleteDialog = new ContentDialog
            {
                Title = $"Delete {timeToChange} task in the schedule?",
                Content = $"Currently \"{currentInformation}\"",
                PrimaryButtonText = "Yes",
                CloseButtonText = "No"
            };

            ContentDialogResult result = await deleteDialog.ShowAsync();

            if(result == ContentDialogResult.Primary)
            {
                ChangeTextBlockText(timeToChange, "");
            }
        }

        /// <summary>
        /// Changes the Textblock information based on which button was clicked and text entered by user.
        /// </summary>
        private void ChangeTextBlockText(string timeToChange, string newInformation)
        {
            string convertTime = times.FirstOrDefault(x => x.Value == timeToChange).Key;
            TextBlock textBlockInRow = (TextBlock)FindName(convertTime + "_Task");
            textBlockInRow.Text = newInformation;
        }
    }
}
