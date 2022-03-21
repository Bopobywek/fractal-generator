using System;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows;

namespace FractalPainter
{
    /// <summary>
    /// Реализация фрактала "Обдуваемое ветром фрактальное дерево".
    /// </summary>
    public class Tree : Fractal
    {
        private double angle1;
        private double angle2;
        private double lengthRatio;
        /// <summary>
        /// Маскимальная глубина рекурсии.
        /// </summary>
        public override int MaxDepth => 9;
        /// <summary>
        /// Угол наклона первого отрезка.
        /// </summary>
        public double Angle1
        {
            get => angle1;
            set => angle1 = value * Math.PI / 180;
        }
        /// <summary>
        /// Угол наклона второго отрезка.
        /// </summary>
        public double Angle2 {
            get => angle2;
            set => angle2 = value * Math.PI / 180;
        }
        /// <summary>
        /// Отношение длин отрезков.
        /// </summary>
        public double LengthRatio { 
            get => lengthRatio;
            set => lengthRatio = value / 100;
        }

        /// <summary>
        /// Конструктор фрактала.
        /// </summary>
        /// <param name="angle1">Угол наклона первого отрезка.</param>
        /// <param name="angle2">Угол наклона второго отрезка.</param>
        /// <param name="lengthRatio">Отношение длин отрезков.</param>
        public Tree(double angle1, double angle2, double lengthRatio)
        {
            Angle1 = angle1;
            Angle2 = angle2;
            LengthRatio = lengthRatio;
        }

        /// <summary>
        /// Рисует фрактал с указанной глубиной рекурсии на заданной поверхности.
        /// </summary>
        /// <param name="surface">Контейнер типа <b>Canvas</b>,
        /// на котором нужно отрисовать фрактал.</param>
        /// <param name="iterations">Количество рекурсивных вызовов.</param>
        public override void Draw(Canvas surface, int iterations)
        {
            var p1 = new Point(surface.ActualWidth / 2, surface.ActualHeight - 40);
            const double startAngle = Math.PI / 2;
            var segmentLength = 2 * surface.ActualHeight / 7;
            DrawRecursively(surface, p1, startAngle, iterations, segmentLength);
        }

        /// <summary>
        /// Рекурсивно отрисовывает фрактал.
        /// </summary>
        /// <param name="surface">Контейнер типа <b>Canvas</b>,
        /// на котором нужно отрисовать фрактал.</param>
        /// <param name="p1">Точка, из которой нужно отрисовывать текущую итерацию.</param>
        /// <param name="angle">Угол наклона отрезка.</param>
        /// <param name="iteration">Номер текущей итерации.</param>
        /// <param name="segmentLength">Длина отрезка.</param>
        private void DrawRecursively(Canvas surface, Point p1, double angle, int iteration, double segmentLength)
        {
            var newPoint = new Point(p1.X + segmentLength * Math.Cos(angle),
                p1.Y - segmentLength * Math.Sin(angle));

            if (iteration == 0)
            {
                surface.DrawLine(new SolidColorBrush(Colors.Black), p1, newPoint, 2);
                return;
            }

            segmentLength *= LengthRatio;
            surface.DrawLine(new SolidColorBrush(Colors.Black), p1, newPoint);
            DrawRecursively(surface, newPoint, angle - Angle1, iteration - 1, segmentLength);
            DrawRecursively(surface, newPoint, angle + Angle2, iteration - 1, segmentLength);
        }
    }
}