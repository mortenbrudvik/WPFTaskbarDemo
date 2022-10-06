using System.Windows.Controls;
using System.Windows.Media;
using MahApps.Metro.IconPacks;

namespace WPFTaskbarUI;

public class ImageFactory
{
    public DrawingImage Create(PackIconMaterialKind kind, SolidColorBrush? color = null)
    {
        var icon = new PackIconMaterial
        {
            Kind = kind
        };

        var geometry = Geometry.Parse(icon.Data);

        var geometryDrawing = new GeometryDrawing
        {
            Geometry = geometry,
            Brush = icon.BorderBrush,
            Pen = new Pen(color ?? Brushes.Gray, 100)
        };
        
        var drawImage = new DrawingImage(geometryDrawing);
        
        return drawImage;
    }
}

