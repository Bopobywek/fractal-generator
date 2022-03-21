using System;
using System.Windows.Controls;
using System.Windows;
using System.Windows.Media;

namespace FractalPainter
{
    public class Triangle : Fractal
    {
        /// <summary>
        /// Максимальная глубина рекурсии.
        /// </summary>
        public override int MaxDepth => 6;

        /// <summary>
        /// Рисует фрактал с указанной глубиной рекурсии на заданной поверхности.
        /// </summary>
        /// <param name="surface">Контейнер типа <b>Canvas</b>,
        /// на котором нужно отрисовать фрактал.</param>
        /// <param name="iterations">Количество рекурсивных вызовов.</param>
        public override void Draw(Canvas surface, int iterations)
        {
            var p1 = new Point(surface.ActualWidth / 9, surface.ActualHeight - 20);
            var p2 = new Point(surface.ActualWidth - surface.ActualWidth / 9, surface.ActualHeight - 20);
            var p3 = new Point((p1.X + p2.X) / 2 + Math.Sqrt(3) * (p2.Y - p1.Y) / 2,
                ((p1.Y + p2.Y) / 2 + Math.Sqrt(3) * (p1.X - p2.X) / 2));
            surface.DrawLine(new SolidColorBrush(Colors.Black), p1, p2);
            surface.DrawLine(new SolidColorBrush(Colors.Black), p2, p3);
            surface.DrawLine(new SolidColorBrush(Colors.Black), p1, p3);

            DrawRecursively(surface, p1, p2, p3, iterations);
        }

        /// <summary>
        /// Рекурсивно отрисовывает фрактал.
        /// </summary>
        /// <param name="surface">Контейнер типа <b>Canvas</b>,
        /// на котором нужно отрисовать фрактал.</param>
        /// <param name="p1">Первая вершина треугольника.</param>
        /// <param name="p2">Вторая вершина треугольника.</param>
        /// <param name="p3">Третья вершина треугольника.</param>
        /// <param name="iteration">Номер текущей итерации.</param>
        private void DrawRecursively(Canvas surface, Point p1, Point p2, Point p3, int iteration)
        {
            if (iteration <= 0)
                return;
            var p4 = new Point((p1.X + p2.X) / 2, (p1.Y + p2.Y) / 2);
            var p5 = new Point((p2.X + p3.X) / 2, (p2.Y + p3.Y) / 2);
            var p6 = new Point((p3.X + p1.X) / 2, (p3.Y + p1.Y) / 2);

            surface.DrawLine(new SolidColorBrush(Colors.Black), p4, p5);
            surface.DrawLine(new SolidColorBrush(Colors.Black), p5, p6);
            surface.DrawLine(new SolidColorBrush(Colors.Black), p6, p4);
            DrawRecursively(surface, p1, p4, p6, iteration - 1);
            DrawRecursively(surface, p6, p5, p3, iteration - 1);
            DrawRecursively(surface, p4, p2, p5, iteration - 1);
        }
    }
}
