using Avalonia.Controls;
using Avalonia.Media.Imaging;
using NoteTree.ViewModels;
using NoteTree.Views;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;

namespace NoteTree.Services
{
    public class MessageBox
    {
        public async static Task<MessageBoxResult> Show(string message, string caption = "", MessageBoxButton buttons = MessageBoxButton.OK, MessageBoxImage icon = MessageBoxImage.Information, Window parentWindow = null)
        {
            if (parentWindow == null) parentWindow = App.MainWindow;
            MessageBoxView view = null;
            if (Avalonia.Threading.Dispatcher.UIThread.CheckAccess())
            {
                try
                {
                    view = new MessageBoxView();
                    var viewModel = new MessageBoxViewModel
                    {
                        Caption = caption,
                        Message = message,
                        Icon = GetIcon(icon),
                        UserControl = view
                    };
                    viewModel.SetButtons(buttons);
                    view.Owner = parentWindow;
                    view.DataContext = viewModel;
                    view.Result = MessageBoxResult.Cancel;
                    await view.ShowDialog(parentWindow);
                    if (parentWindow != null) parentWindow.Focus();
                }
                catch (Exception e)
                {
                   Debug.Print(e.Message);
                }
            }
            else
            {
                try
                {
                    Avalonia.Threading.Dispatcher.UIThread.Post(() =>
                    {
                        view = new MessageBoxView();
                        var viewModel = new MessageBoxViewModel
                        {
                            Caption = caption,
                            Message = message,
                            Icon = GetIcon(icon),
                            UserControl = view
                        };
                        viewModel.SetButtons(buttons);
                        view.Owner = parentWindow;
                        view.DataContext = viewModel;
                        view.Result = MessageBoxResult.Cancel;
                        view.ShowDialog(parentWindow);
                        if (parentWindow != null) parentWindow.Focus();
                    });
                }
                catch (Exception e)
                {
                    Debug.Print(e.Message);
                }
            }
            return view.Result;
        }

        public static void ShowAsync(string message, string caption = "", MessageBoxButton buttons = MessageBoxButton.OK, MessageBoxImage icon = MessageBoxImage.Information)
        {
            var view = new MessageBoxView();
            Avalonia.Threading.Dispatcher.UIThread.InvokeAsync(() =>
            {
                var viewModel = new MessageBoxViewModel
                {
                    Caption = caption,
                    Message = message,
                    Icon = GetIcon(icon),
                    UserControl = view
                };
                viewModel.SetButtons(buttons);
                view.Owner = App.MainWindow;
                view.DataContext = viewModel;
                view.Result = MessageBoxResult.Cancel;
                view.ShowDialog(App.MainWindow);
                if (App.MainWindow != null) App.MainWindow.Focus();
            });

        }

        private static Bitmap GetIcon(MessageBoxImage icon)
        {
            switch (icon)
            {
                case MessageBoxImage.Warning:
                    return new Bitmap("Assets/images/mb_warning.png");
                case MessageBoxImage.Error:
                    return new Bitmap("Assets/images/mb_error.png");
                case MessageBoxImage.Information:
                    return new Bitmap("Assets/images/mb_information.png");
                case MessageBoxImage.Question:
                    return new Bitmap("Assets/images/mb_question.png");
                case MessageBoxImage.Exclamation:
                    return new Bitmap("Assets/images/mb_exclamation.png");
                default:
                    throw new ArgumentOutOfRangeException(nameof(icon), icon, null);
            }
        }
    }

    public enum MessageBoxImage
    {
        Warning, Error, Information, Question, Exclamation
    }

    public enum MessageBoxButton
    {
        OK, YesNo, OKCancel, YesNoCancel
    }

    public enum MessageBoxResult
    {
        Cancel, OK, Yes, No
    }
}
