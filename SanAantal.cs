﻿using System.Collections;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;

namespace MyRevitCommands
{
    [TransactionAttribute(TransactionMode.ReadOnly)]
    public class SanAantal : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            var revitSchedules = new ArrayList();
            revitSchedules.Add("SAN A");

            var fileName = GetType().Name;

            return new GenericToevoegen().GenericExecute(commandData, ref message, elements, @"c:\\temp\\Sanitair\",
                revitSchedules, fileName);
        }
    }
}