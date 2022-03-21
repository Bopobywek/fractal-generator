using System.Windows.Controls;

namespace FractalPainter
{
    /// <summary>
    /// Абстрактный класс для реализации фракталов.
    /// </summary>
    public abstract class Fractal
    {
        /// <summary>
        /// Максимальная глубина рекурсии.
        /// </summary>
        public abstract int MaxDepth { get; }

        /// <summary>
        /// Рисует фрактал с указанной глубиной рекурсии на заданной поверхности.
        /// </summary>
        /// <param name="surface">Контейнер типа <b>Canvas</b>,
        /// на котором нужно отрисовать фрактал.</param>
        /// <param name="iterations">Количество рекурсивных вызовов.</param>
        public abstract void Draw(Canvas surface, int iterations);
    }
}
