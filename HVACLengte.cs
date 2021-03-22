﻿using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using OfficeOpenXml;
using System;
using System.Collections;
using System.Globalization;
using System.IO;

namespace MyRevitCommands
{
    [TransactionAttribute(TransactionMode.ReadOnly)]
    public class HVACLengte : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            ArrayList revitSchedules = new ArrayList();
            revitSchedules.Add("AE_M56");
            revitSchedules.Add("ventilatierooster");

            return new GenericToevoegen().GenericExecute(commandData, ref message, elements, @"c:\\temp\\HVAC\\Lengte\", revitSchedules);
        }
    }
}