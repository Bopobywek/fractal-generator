using System.Windows.Controls;
using System.Windows;
using System.Windows.Media;

namespace FractalPainter
{   
    /// <summary>
    /// Реализация фрактала "Множество Кантора".
    /// </summary>
    public class CantorSetFractal : Fractal
    {
        /// <summary>
        /// Максимальная глубина рекурсии.
        /// </summary>
        public override int MaxDepth => 9;
        /// <summary>
        /// Расстояние между отрезками.
        /// </summary>
        public double Delta { get; set; }

        /// <summary>
        /// Конструктор фрактала.
        /// </summary>
        /// <param name="delta">Расстояние между отрезками.</param>
        public CantorSetFractal(double delta)
        {
            Delta = delta;
        }

        /// <summary>
        /// Рисует фрактал с указанной глубиной рекурсии на заданной поверхности.
        /// </summary>
        /// <param name="surface">Контейнер типа <b>Canvas</b>,
        /// на котором нужно отрисовать фрактал.</param>
        /// <param name="iterations">Количество рекурсивных вызовов.</param>
        public override void Draw(Canvas surface, int iterations)
        {
            var point1 = new Point(10, 10);
            var width = surface.ActualWidth - 40;
            DrawRecursively(surface, point1, width, iterations);
        }

        /// <summary>
        /// Рекурсивно отрисовывает фрактал.
        /// </summary>
        /// <param name="surface">Контейнер типа <b>Canvas</b>,
        /// на котором нужно отрисовать фрактал.</param>
        /// <param name="point1">Точка, из которой нужно отрисовывать текущую итерацию.</param>
        /// <param name="width">Длина отрезка.</param>
        /// <param name="iteration">Номер текущей итерации.</param>
        private void DrawRecursively(Canvas surface, Point point1, double width, int iteration)
        {
            if (iteration <= 0)
                return;
            surface.DrawLine(new SolidColorBrush(Colors.Black), point1,
                new Point(point1.X + width, point1.Y), 7);
            var point2 = new Point(point1.X, point1.Y + Delta);
            var point3 = new Point(point1.X + width * 2 / 3, point1.Y + Delta);
            DrawRecursively(surface, point2, width / 3, iteration - 1);
            DrawRecursively(surface, point3, width / 3, iteration - 1);
        }
    }
}
