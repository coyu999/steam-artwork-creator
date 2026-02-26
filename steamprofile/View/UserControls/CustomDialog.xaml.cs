using Microsoft.VisualBasic;
using System.Windows;
namespace steamprofile.View
{
    
    public partial class CustomDialog : Window
    {

        public Action? BtnOneHandler
        {
            get; set;
        }
        public Action? BtnTwoHandler
        {
            get; set;
        }

        public Action? BtnDoneHandler 
        { 
            get; set; 
        }

        public CustomDialog(string question, string answer1, string answer2, string title, string iconName, Action? btnOneAction = null, Action? btnTwoAction = null)
        {
            InitializeComponent();
            tbQuestion.Text = question;
            btnOne.Content = answer1;
            btnTwo.Content = answer2;
            Title = title;
            BtnOneHandler = btnOneAction;
            BtnTwoHandler = btnTwoAction;
            switch(iconName.ToLower())
            {
                case "question":
                    this.Icon = MBIcons.Question;
                    boxIcon.Source = MBIcons.Question;
                    break;
                case "warning":
                    this.Icon = MBIcons.Warning;
                    boxIcon.Source = MBIcons.Warning;
                    break;
                case "error":
                    this.Icon = MBIcons.Error;
                    boxIcon.Source = MBIcons.Error;
                    break;
                case "information":
                    this.Icon = MBIcons.Information;
                    boxIcon.Source = MBIcons.Information;
                    break;
            }
        }

        private void btnOne_Click(object sender, RoutedEventArgs e)
        {
            BtnOneHandler?.Invoke();
        }
        private void btnTwo_Click(object sender, RoutedEventArgs e)
        {
            BtnTwoHandler?.Invoke();
        }

        private void btnDone_Click(object sender, RoutedEventArgs e)
        {
            BtnDoneHandler?.Invoke();
            this.Close();
        }
    }
}
