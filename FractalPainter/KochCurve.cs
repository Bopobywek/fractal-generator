using System;
using System.Windows.Controls;
using System.Windows;
using System.Windows.Media;

namespace FractalPainter
{
    /// <summary>
    /// Реализация фрактала "Кривая Коха."
    /// </summary>
    public class KochCurve : Fractal
    {
        /// <summary>
        /// Максимальная глубина рекурсии.
        /// </summary>
        public override int MaxDepth => 5;

        /// <summary>
        /// Рисует фрактал с указанной глубиной рекурсии на заданной поверхности.
        /// </summary>
        /// <param name="surface">Контейнер типа <b>Canvas</b>,
        /// на котором нужно отрисовать фрактал.</param>
        /// <param name="iterations">Количество рекурсивных вызовов.</param>
        public override void Draw(Canvas surface, int iterations)
        {
            var point1 = new Point(20, surface.ActualHeight / 2 + 70);
            var point2 = new Point(surface.ActualWidth - 20, surface.ActualHeight / 2 + 70);
            DrawRecursively(surface, point1, point2, iterations);
        }

        /// <summary>
        /// Рекурсивно отрисовывает фрактал.
        /// </summary>
        /// <param name="surface">Контейнер типа <b>Canvas</b>,
        /// на котором нужно отрисовать фрактал.</param>
        /// <param name="point1">Начальная точка отрезка.</param>
        /// <param name="point2">Конечная точка отрезка.</param>
        /// <param name="iteration">Номер текущей итерации.</param>
        private void DrawRecursively(Canvas surface, Point point1, Point point2, int iteration)
        {
            if (iteration > 0)
            {
                var point3 = new Point((point2.X + 2 * point1.X) / 3, (point2.Y + 2 * point1.Y) / 3);
                var point4 = new Point((2 * point2.X + point1.X) / 3, (point1.Y + 2 * point2.Y) / 3);
                var point5 = new Point((point1.X + point2.X) / 2 + (point2.Y - point1.Y) / (Math.Sqrt(3) * 2),
                    (point1.Y + point2.Y) / 2 + (point1.X - point2.X) / (Math.Sqrt(3) * 2));

                DrawRecursively(surface, point1, point3, iteration - 1);
                DrawRecursively(surface, point3, point5, iteration - 1);
                DrawRecursively(surface, point5, point4, iteration - 1);
                DrawRecursively(surface, point4, point2, iteration - 1);
            }
            else
            {
                surface.DrawLine(new SolidColorBrush(Colors.Black), point1, point2);
            }
        }
    }
}
