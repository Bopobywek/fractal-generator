using System.Windows.Controls;
using System.Windows;
using System.Windows.Shapes;
using System.Windows.Media;


namespace FractalPainter
{
    /// <summary>
    /// Класс, который содержит методы расширения для Canvas.
    /// </summary>
    public static class CanvasExtension
    {
        /// <summary>
        /// Рисует линию в контейнере Canvas.
        /// </summary>
        /// <param name="surface">Объект типа Canvas.</param>
        /// <param name="strokeColor">Кисть с цветом контура.</param>
        /// <param name="point1">Начальная точка прямой.</param>
        /// <param name="point2">Конечная точка прямой.</param>
        public static void DrawLine(this Canvas surface, Brush strokeColor, Point point1, Point point2)
        {
            var line = new Line
            {
                X1 = point1.X,
                X2 = point2.X,
                Y1 = point1.Y,
                Y2 = point2.Y,
                Stroke = strokeColor,
            };

            surface.Children.Add(line);
        }

        /// <summary>
        /// Рисует линию в контейнере Canvas.
        /// </summary>
        /// <param name="surface">Объект типа Canvas.</param>
        /// <param name="strokeColor">Кисть с цветом контура.</param>
        /// <param name="point1">Начальная точка прямой.</param>
        /// <param name="point2">Конечная точка прямой.</param>
        /// <param name="thickness">Толщина прямой.</param>
        public static void DrawLine(this Canvas surface, Brush strokeColor,
            Point point1, Point point2, double thickness)
        {
            var line = new Line
            {
                X1 = point1.X,
                X2 = point2.X,
                Y1 = point1.Y,
                Y2 = point2.Y,
                Stroke = strokeColor,
                StrokeThickness = thickness
            };

            surface.Children.Add(line);
        }

        /// <summary>
        /// Очищает все элементы в контейнере Canvas.
        /// </summary>
        /// <param name="surface">Объект типа Canvas.</param>
        public static void Clear(this Canvas surface)
        {
            surface.Children.Clear();
        }

        /// <summary>
        /// Рисует прямоугольник в контейнере Canvas.
        /// </summary>
        /// <param name="surface">Объект типа Canvas.</param>
        /// <param name="strokeColor">Кисть с цветом контура.</param>
        /// <param name="fillColor">Кисть с цветом заливки прямоугольника.</param>
        /// <param name="rectangleInfo">Информация о прямоугольнике.</param>
        public static void DrawRectangle(this Canvas surface, Brush strokeColor,
            Brush fillColor, Rect rectangleInfo)
        {
            var rectangle = new Polygon
            {
                Fill = fillColor,
                Stroke = strokeColor,
                StrokeThickness = 0
            };

            var points = new PointCollection
            {
                rectangleInfo.TopLeft,
                rectangleInfo.TopRight,
                rectangleInfo.BottomRight,
                rectangleInfo.BottomLeft
            };

            rectangle.Points = points;

            surface.Children.Add(rectangle);
        }
    }
}