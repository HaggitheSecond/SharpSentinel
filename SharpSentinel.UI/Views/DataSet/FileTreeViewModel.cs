using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using Caliburn.Micro;
using MahApps.Metro.IconPacks;
using SharpSentinel.Parser.Data;
using SharpSentinel.Parser.Helpers;
using SharpSentinel.UI.Common;

namespace SharpSentinel.UI.Views.DataSet
{
    public class FileTreeViewModel : Screen
    {
        private TreeItem _mainItem;

        public TreeItem MainItem
        {
            get { return this._mainItem; }
            set { this.Set(ref this._mainItem, value); }
        }

        public FileTreeViewModel()
        {

        }

        public void Initialize(BaseData baseData)
        {
            TreeItem item;

            if (baseData is S1Data s1Data)
                item = this.GenerateS1Tree(s1Data);
            else
                throw new NotSupportedException();

            this.MainItem = item;
        }

        public void Clear()
        {
            this.MainItem = null;
        }

        private TreeItem GenerateS1Tree(S1Data data)
        {
            var allDirectories = new DirectoryInfo(data.BaseDirectory.FullName).GetDirectories();
            
            MethodHelper.TryGetPropertyDescription(data.MeasurementDataUnits.First().GetType(), "NoiseAnnotation", out var noiseAnnotationDescription);
            MethodHelper.TryGetPropertyDescription(data.MeasurementDataUnits.First().GetType(), "ProductAnnotation", out var productAnnotationDescription);
            MethodHelper.TryGetPropertyDescription(data.MeasurementDataUnits.First().GetType(), "CalibriationAnnotation", out var calibrationAnnotationDescription);


            var measurementDirectory = new DirectoryTreeItem(allDirectories.First(f => f.Name == "measurement"));
            measurementDirectory.Children.AddRange(data.MeasurementDataUnits.Select(f => new FileTreeItem(f.File, f)));

            var previewDirectory = new DirectoryTreeItem(allDirectories.First(f => f.Name == "preview"));

            previewDirectory.Children.Add(new FileTreeItem(data.Preview.MapOverlay.File, data.Preview.MapOverlay));
            previewDirectory.Children.Add(new FileTreeItem(data.Preview.ProductPreview.File, data.Preview.ProductPreview));
            previewDirectory.Children.Add(new FileTreeItem(data.Preview.QuickLook.File, data.Preview.QuickLook));

            var supportDirectory = new DirectoryTreeItem(allDirectories.First(f => f.Name == "support"));

            var firstMeasurmenet = data.MeasurementDataUnits.First();

            supportDirectory.Children.Add(new FileTreeItem(firstMeasurmenet.Documentation.File, firstMeasurmenet.Documentation));
            supportDirectory.Children.Add(new FileTreeItem(firstMeasurmenet.ProductAnnotation.Documentation.File, firstMeasurmenet.ProductAnnotation.Documentation));
            supportDirectory.Children.Add(new FileTreeItem(firstMeasurmenet.CalibriationAnnotation.Documentation.File, firstMeasurmenet.CalibriationAnnotation.Documentation));
            supportDirectory.Children.Add(new FileTreeItem(firstMeasurmenet.NoiseAnnotation.Documentation.File, firstMeasurmenet.NoiseAnnotation.Documentation));

            supportDirectory.Children.Add(new FileTreeItem(data.Preview.MapOverlay.Documentation.File, data.Preview.MapOverlay.Documentation));
            supportDirectory.Children.Add(new FileTreeItem(data.Preview.QuickLook.Documentation.File, data.Preview.QuickLook.Documentation));
            supportDirectory.Children.Add(new FileTreeItem(data.Preview.ProductPreview.Documentation.File, data.Preview.ProductPreview.Documentation));

            var annotationDirectory = new DirectoryTreeItem(allDirectories.First(f => f.Name == "annotation"));
            var calibrationDirectory = new DirectoryTreeItem(annotationDirectory.DirectoryInfo.GetDirectories().First(f => f.Name == "calibration"));
            calibrationDirectory.Children.AddRange(data.MeasurementDataUnits.Select(f => f.CalibriationAnnotation).Select(f => new FileTreeItem(f.File, f, calibrationAnnotationDescription)));
            calibrationDirectory.Children.AddRange(data.MeasurementDataUnits.Select(f => f.NoiseAnnotation).Select(f => new FileTreeItem(f.File, f, noiseAnnotationDescription)));

            annotationDirectory.Children.Add(calibrationDirectory);
            annotationDirectory.Children.AddRange(data.MeasurementDataUnits.Select(f => f.ProductAnnotation).Select(f => new FileTreeItem(f.File, f, productAnnotationDescription)));

            var mainItem = new DirectoryTreeItem(data.BaseDirectory)
            {
                Children = new BindableCollection<TreeItem>
                {
                    annotationDirectory,
                    measurementDirectory,
                    previewDirectory,
                    supportDirectory,
                    new FileTreeItem(data.Manifest.File, data.Manifest),
                    new FileTreeItem(data.ReportFile.File, data.ReportFile)
                }
            };

            return mainItem;
        }
    }
}