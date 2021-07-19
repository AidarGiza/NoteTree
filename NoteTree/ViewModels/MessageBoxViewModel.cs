using Avalonia.Media.Imaging;
using NoteTree.Services;
using NoteTree.Views;
using ReactiveUI;

namespace NoteTree.ViewModels
{
    public class MessageBoxViewModel : ViewModelBase
    {
        public bool IsFirstButtonVisible
        {
            get => isFirstButtonVisible;
            set => this.RaiseAndSetIfChanged(ref isFirstButtonVisible, value, nameof(IsFirstButtonVisible));
        }
        private bool isFirstButtonVisible;

        public bool IsSecondButtonVisible
        {
            get => isSecondButtonVisible;
            set => this.RaiseAndSetIfChanged(ref isSecondButtonVisible, value, nameof(IsSecondButtonVisible));
        }
        private bool isSecondButtonVisible;

        public bool IsThirdButtonVisible
        {
            get => isThirdButtonVisible;
            set => this.RaiseAndSetIfChanged(ref isThirdButtonVisible, value, nameof(IsThirdButtonVisible));
        }
        private bool isThirdButtonVisible;


        public string FirstButtonText
        {
            get => firstButtonText;
            set => this.RaiseAndSetIfChanged(ref firstButtonText, value, nameof(FirstButtonText));
        }
        private string firstButtonText;

        public string SecondButtonText
        {
            get => secondButtonText;
            set => this.RaiseAndSetIfChanged(ref secondButtonText, value, nameof(SecondButtonText));
        }
        private string secondButtonText;

        public string ThirdButtonText
        {
            get => thirdButtonText;
            set => this.RaiseAndSetIfChanged(ref thirdButtonText, value, nameof(ThirdButtonText));
        }
        private string thirdButtonText;


        public string Caption
        {
            get => caption;
            set => this.RaiseAndSetIfChanged(ref caption, value, nameof(Caption));
        }
        private string caption;

        public string Message
        {
            get => message;
            set => this.RaiseAndSetIfChanged(ref message, value, nameof(Message));
        }
        private string message;

        public Bitmap Icon
        {
            get => icon;
            set => this.RaiseAndSetIfChanged(ref icon, value, nameof(Icon));
        }
        private Bitmap icon;

        public MessageBoxButton BoxButtons
        {
            get => boxButtons;
            set => this.RaiseAndSetIfChanged(ref boxButtons, value, nameof(BoxButtons));
        }
        private MessageBoxButton boxButtons;

        private void FirstButtonClick()
        {
            switch (boxButtons)
            {
                case MessageBoxButton.OK:
                    (UserControl as MessageBoxView).Result = MessageBoxResult.OK;
                    break;
                case MessageBoxButton.OKCancel:
                    (UserControl as MessageBoxView).Result = MessageBoxResult.OK;
                    break;
                case MessageBoxButton.YesNoCancel:
                case MessageBoxButton.YesNo:
                    (UserControl as MessageBoxView).Result = MessageBoxResult.Yes;
                    break;
            }
            (UserControl as MessageBoxView).Close();
        }

        private void SecondButtonClick()
        {
            switch (boxButtons)
            {
                case MessageBoxButton.OKCancel:
                    (UserControl as MessageBoxView).Result = MessageBoxResult.Cancel;
                    break;
                case MessageBoxButton.YesNoCancel:
                case MessageBoxButton.YesNo:
                    (UserControl as MessageBoxView).Result = MessageBoxResult.No;
                    break;

            }
            (UserControl as MessageBoxView).Close();
        }

        private void ThirdButtonClick()
        {
            switch (boxButtons)
            {
                case MessageBoxButton.YesNoCancel:
                    (UserControl as MessageBoxView).Result = MessageBoxResult.Cancel;
                    break;
            }
            (UserControl as MessageBoxView).Close();
        }

        public void SetButtons(MessageBoxButton buttons)
        {
            switch (buttons)
            {
                case MessageBoxButton.OK:
                    IsFirstButtonVisible = true;
                    IsSecondButtonVisible = false;
                    IsThirdButtonVisible = false;
                    FirstButtonText = "Принять";
                    break;
                case MessageBoxButton.OKCancel:
                    IsFirstButtonVisible = true;
                    IsSecondButtonVisible = true;
                    IsThirdButtonVisible = false;
                    FirstButtonText = "Принять";
                    SecondButtonText = "Отмена";
                    break;
                case MessageBoxButton.YesNo:
                    IsFirstButtonVisible = true;
                    IsSecondButtonVisible = true;
                    IsThirdButtonVisible = false;
                    FirstButtonText = "Да";
                    SecondButtonText = "Нет";
                    break;
                case MessageBoxButton.YesNoCancel:
                    IsFirstButtonVisible = true;
                    IsSecondButtonVisible = true;
                    IsThirdButtonVisible = true;
                    FirstButtonText = "Да";
                    SecondButtonText = "Нет";
                    ThirdButtonText = "Отмена";
                    break;
            }
            boxButtons = buttons;
        }
    }
}
