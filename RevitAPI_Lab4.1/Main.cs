using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RevitAPI_Lab4._1
{
    [Transaction(TransactionMode.Manual)]
    public class Main : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            UIApplication uiapp = commandData.Application;
            UIDocument uidoc = uiapp.ActiveUIDocument;
            Document doc = uidoc.Document;

            string wallinfo = String.Empty;

            var walls = new FilteredElementCollector(doc)
                .OfCategory(BuiltInCategory.OST_Walls)
                .Cast<Wall>()
                .ToList();

            foreach (Wall wall in walls)
            {
                string wallname = wall.get_Parameter(BuiltInParameter.CURTAIN_WALL_PANELS_CONSTRUCTION_TYPE).AsString();
                wallinfo += $"{wallname};{(wall.GetMaterialVolume(wall.Id)).ToString()}{Environment.NewLine}";

            }

            string desktoppath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            string csvPath = Path.Combine(desktoppath, "wallinfo.csv");

            File.WriteAllText(csvPath, wallinfo);

            return Result.Succeeded;
        }
    }
}
