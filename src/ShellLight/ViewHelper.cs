using System;
using System.Windows;

namespace ShellLight
{
    public class ViewHelper
    {
        public static void ShowMessage(string message, string title)
        {
            MessageBox.Show(message, title, MessageBoxButton.OK);
        }

        public static void ShowMessage(string message)
        {
            ShowMessage(message, "Hello!");
        }

        public static void ShowError(string message)
        {
             ShowError(message, "Sorry!");
        }

        public static void ShowError(string message, string title)
        {
            string supportMessage = string.Format("{0}\n\nPlease contact {1} for help", message, Config.SupportEmail);
            MessageBox.Show(supportMessage, title, MessageBoxButton.OK);
        }

        public static MessageBoxResult ShowYesNoDialog(string message, string title)
        {
            return MessageBox.Show(message, title, MessageBoxButton.OKCancel);
        }
    }
}