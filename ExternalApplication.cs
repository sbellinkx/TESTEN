﻿using Autodesk.Revit.UI;
using System;
using System.Reflection;
using System.Windows.Media.Imaging;

namespace MyRevitCommands
{
    internal class ExternalApplication : IExternalApplication
    {
        public Result OnShutdown(UIControlledApplication application)
        {
            return Result.Succeeded;
        }

        public Result OnStartup(UIControlledApplication application)
        {
            string path = Assembly.GetExecutingAssembly().Location;
            string strImageFolder = System.IO.Path.GetDirectoryName(path) + @"\Resources\";

            // Create a custom ribbon tab
            String tabName = "AE Plus";
            application.CreateRibbonTab(tabName);

            // Create a ribbon panel
            RibbonPanel panelTot = application.CreateRibbonPanel(tabName, "Totaal");
            RibbonPanel panelHvac = application.CreateRibbonPanel(tabName, "Export HVAC");
            RibbonPanel panelSan = application.CreateRibbonPanel(tabName, "Export SAN");
            RibbonPanel panelElek = application.CreateRibbonPanel(tabName, "Export elektriciteit");

            // Create button
            PushButtonData buttonTot = new PushButtonData("Export schedules",
                                               "Export schedules",
                                               path,
                                               "MyRevitCommands.CSVPerTabToevoegen");
            PushButtonData button2 = new PushButtonData("(vs.Name.Contains(AE_E60) (AE_M52) (AE_M57_ Ventilatieroosters)",
                                               "Export met aantallen",
                                               path,
                                               "MyRevitCommands.CSV_E60Toevoegen");

            PushButtonData button3 = new PushButtonData("Export leidingen",
                                                        "Export leidingen lengte",
                                                        path,
                                                        "MyRevitCommands.ExportLeidingen");

            BitmapImage button1image = new BitmapImage(new Uri("pack://application:,,,/MyRevitCommands;component/Resources/favicon.ico"));
            buttonTot.LargeImage = button1image;

            BitmapImage button2image = new BitmapImage(new Uri("pack://application:,,,/MyRevitCommands;component/Resources/numbers.ico"));
            button2.LargeImage = button2image;

            BitmapImage button3image = new BitmapImage(new Uri("pack://application:,,,/MyRevitCommands;component/Resources/pipe.ico"));
            button3.LargeImage = button3image;

            panelTot.AddItem(buttonTot);
            panelSan.AddItem(button2);
            panelElek.AddItem(button3);
            return Result.Succeeded;
        }
    }
}