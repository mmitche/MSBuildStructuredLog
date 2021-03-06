﻿using System;
using System.Reflection;
using System.Windows;
using System.Windows.Threading;
using Microsoft.Build.Logging.StructuredLogger;

[assembly: AssemblyCompany("Microsoft")]
[assembly: AssemblyProduct("MSBuild Structured Log Viewer")]
[assembly: AssemblyTitle("MSBuild Structured Log Viewer")]

namespace StructuredLogViewer
{
    public class Entrypoint
    {
        [STAThread]
        public static void Main(string[] args)
        {
            ExceptionHandler.Initialize();
            DialogService.ShowMessageBoxEvent += message => MessageBox.Show(message);
            ClipboardService.Set += Clipboard.SetText;

            var app = new Application();
            app.DispatcherUnhandledException += OnDispatcherUnhandledException;
            var window = new MainWindow();
            app.Run(window);
        }

        private static void OnDispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
        {
            ErrorReporting.ReportException(e.Exception);
            DialogService.ShowMessageBox(
                    "Unexpected exception. Sorry about that.\r\nPlease Ctrl+C to copy this text and file an issue at https://github.com/KirillOsenkov/MSBuildStructuredLog/issues/new\r\n\r\n" + e.Exception.ToString());
            e.Handled = true;
        }
    }
}
