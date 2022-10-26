using System.Windows.Media;
using System.Windows.Media.Imaging;
using MahApps.Metro.IconPacks;

namespace WPFTaskbarUI;

public static class ImageFactory
{
    public static BitmapSource Create(PackIconMaterialKind kind, Brush? color = null, int targetSize = 64)
    {
        var icon = new PackIconMaterial
        {
            Kind = kind
        };

        var geometry = Geometry.Parse(icon.Data);

        return Convert(geometry, color ??  Brushes.Black, targetSize);
    }
    
    private static BitmapSource Convert(Geometry geometry, Brush color,  int targetSize)
    {
        var rect = geometry.GetRenderBounds(new Pen(color, 0));

        var bigger = rect.Width > rect.Height ? rect.Width : rect.Height;
        var scale = targetSize / bigger;

        Geometry scaledGeometry = Geometry.Combine(geometry, geometry, GeometryCombineMode.Intersect, new ScaleTransform(scale, scale));
        rect = scaledGeometry.GetRenderBounds(new Pen(color, 0));

        Geometry transformedGeometry = Geometry.Combine(scaledGeometry, scaledGeometry, 
            GeometryCombineMode.Intersect, 
            new TranslateTransform((targetSize - rect.Width) / 2 - rect.Left, (targetSize - rect.Height) / 2 - rect.Top));

        var bmp = new RenderTargetBitmap(targetSize, targetSize, // Size
            96, 96, // DPI 
            PixelFormats.Pbgra32);
        
        var viz = new DrawingVisual();
        using (var dc = viz.RenderOpen())
        {
            dc.DrawGeometry(color, null, transformedGeometry);
        }

        bmp.Render(viz);
        
        return bmp;
    }
}

