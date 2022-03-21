using System;
using System.Windows.Controls;
using System.Windows;
using System.Windows.Media;

namespace FractalPainter
{
    /// <summary>
    /// Реализация фрактала "Ковер Серпинского"
    /// </summary>
    public class Carpet : Fractal
    {
        /// <summary>
        /// Максимальная глубина рекурсии.
        /// </summary>
        public override int MaxDepth => 4;

        /// <summary>
        /// Рисует фрактал с указанной глубиной рекурсии на заданной поверхности.
        /// </summary>
        /// <param name="surface">Контейнер типа <b>Canvas</b>,
        /// на котором нужно отрисовать фрактал.</param>
        /// <param name="iterations">Количество рекурсивных вызовов.</param>
        public override void Draw(Canvas surface, int iterations)
        {
            var sideLength = Math.Min(surface.ActualHeight, surface.ActualWidth) / 1.2;
            var rect = new Rect(40, 40,
                sideLength, sideLength);
            surface.DrawRectangle(new SolidColorBrush(Colors.Transparent),
                new SolidColorBrush(Colors.Blue), rect);
            DrawRecursively(surface, rect, iterations);
        }

        /// <summary>
        /// Рекурсивно отрисовывает фрактал.
        /// </summary>
        /// <param name="surface">Контейнер типа <b>Canvas</b>,
        /// на котором нужно отрисовать фрактал.</param>
        /// <param name="rect">Квадрат, для которого нужно выполнить отрисовку.</param>
        /// <param name="iteration">Номер текущей итерации.</param>
        private void DrawRecursively(Canvas surface, Rect rect, int iteration)
        {
            if (iteration <= 0)
                return;

            var newWidth = rect.Width / 3;
            var newHeight = rect.Height / 3;

            var x1 = rect.Left;
            var x2 = x1 + newWidth;
            var x3 = x2 + newWidth;

            var y1 = rect.Top;
            var y2 = y1 + newHeight;
            var y3 = y2 + newHeight;

            var brush = new SolidColorBrush(Colors.White);
            surface.DrawRectangle(brush, brush, new Rect(x2, y2, newWidth, newHeight));

            DrawRecursively(surface, new Rect(x1, y1, newWidth, newHeight), iteration - 1);
            DrawRecursively(surface, new Rect(x1, y2, newWidth, newHeight), iteration - 1);
            DrawRecursively(surface, new Rect(x1, y3, newWidth, newHeight), iteration - 1);
            DrawRecursively(surface, new Rect(x2, y1, newWidth, newHeight), iteration - 1);
            DrawRecursively(surface, new Rect(x2, y3, newWidth, newHeight), iteration - 1);
            DrawRecursively(surface, new Rect(x3, y1, newWidth, newHeight), iteration - 1);
            DrawRecursively(surface, new Rect(x3, y2, newWidth, newHeight), iteration - 1);
            DrawRecursively(surface, new Rect(x3, y3, newWidth, newHeight), iteration - 1);
        }
    }
}
