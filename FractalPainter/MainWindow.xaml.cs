using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Microsoft.Win32;

namespace FractalPainter
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        /// <summary>
        /// Поле для хранения текущего фрактала.
        /// </summary>
        private Fractal currentFractal;
        /// <summary>
        /// Обработчики инициализации фракталов.
        /// </summary>
        private readonly Dictionary<string, Action> handlers;
        /// <summary>
        /// Временная точка для хранения позиции мыши.
        /// </summary>
        private Point tempPoint;
        /// <summary>
        /// Масштаб.
        /// </summary>
        private double scaleFactor = 1;
        
        /// <summary>
        /// Конструктор окна.
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();
            MinWidth = SystemParameters.PrimaryScreenWidth / 2;
            MinHeight = SystemParameters.PrimaryScreenHeight / 2;
            Height = MinHeight;
            Width = MinWidth;
            handlers = new Dictionary<string, Action>()
            {
                {"treeFractal", SetTreeFractal},
                {"curveFractal", SetKochCurveFractal},
                {"carpetFractal", SetCarpetFractal},
                {"triangleFractal", SetTriangleFractal},
                {"cantorSetFractal", SetCantorSetFractal}
            };
        }

        /// <summary>
        /// Обработчик изменения размеров окна.
        /// </summary>
        /// <param name="sender">Инициатор события.</param>
        /// <param name="e">Аргументы события.</param>
        private void Window_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            RedrawCurrentFractal(sender, EventArgs.Empty);
        }

        /// <summary>
        /// Скрывает все элементы управления,
        /// которые нужны для настройки только некоторых фракталов.
        /// </summary>
        private void HideAllUncommonElements()
        {
            firstSegmentAngle.Visibility = Visibility.Hidden;
            secondSegmentAngle.Visibility = Visibility.Hidden;
            lengthProportion.Visibility = Visibility.Hidden;
            cantorSetDelta.Visibility = Visibility.Hidden;
        }

        /// <summary>
        /// Устанавливает Множество Кантора в качестве текущего фрактала.
        /// </summary>
        private void SetCantorSetFractal()
        {
            cantorSetDelta.Visibility = Visibility.Visible;
            currentFractal = new CantorSetFractal(delta.Value);
            iterations.Maximum = currentFractal.MaxDepth;
        }

        /// <summary>
        /// Устанавливает Фрактальное дерево в качестве текущего фрактала.
        /// </summary>
        private void SetTreeFractal()
        {
            firstSegmentAngle.Visibility = Visibility.Visible;
            secondSegmentAngle.Visibility = Visibility.Visible;
            lengthProportion.Visibility = Visibility.Visible;
            currentFractal = new Tree(angle1.Value, angle2.Value, ratio.Value);
            iterations.Maximum = currentFractal.MaxDepth;
        }

        /// <summary>
        /// Устанавливает Кривую Коха в качестве текущего фрактала.
        /// </summary>
        private void SetKochCurveFractal()
        {
            currentFractal = new KochCurve();
            iterations.Maximum = currentFractal.MaxDepth;
        }

        /// <summary>
        /// Устанавливает Треугольник Серпинского в качестве текущего фрактала. 
        /// </summary>
        private void SetTriangleFractal()
        {
            currentFractal = new Triangle();
            iterations.Maximum = currentFractal.MaxDepth;
        }

        /// <summary>
        /// Устанавливает Ковер Серпинского в качестве текущего фрактала.
        /// </summary>
        private void SetCarpetFractal()
        {
            currentFractal = new Carpet();
            iterations.Maximum = currentFractal.MaxDepth;
        }

        /// <summary>
        /// Перерисовывает текущий фрактал.
        /// </summary>
        /// <param name="sender">Инициатор события.</param>
        /// <param name="e">Аргументы события.</param>
        private void RedrawCurrentFractal(object sender, EventArgs e)
        {
            switch (currentFractal)
            {
                case Tree tree:
                    tree.Angle1 = angle1.Value;
                    tree.Angle2 = angle2.Value;
                    tree.LengthRatio = ratio.Value;
                    break;
                case CantorSetFractal set:
                    set.Delta = delta.Value;
                    break;
                case null:
                    return;
            }

            scaleFactor = 1;
            drawSurface.Clear();
            currentFractal.Draw(drawSurface, (int) iterations.Value);
        }

        /// <summary>
        /// Обработчик выбора фрактала.
        /// </summary>
        /// <param name="sender">Инициатор события.</param>
        /// <param name="e">Аргументы события.</param>
        private void fractalsComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (sender is not ComboBox comboBox)
            {
                return;
            }

            HideAllUncommonElements();

            if (comboBox.SelectedItem is not TextBlock selectedFractal)
            {
                return;
            }

            iterations.ValueChanged -= RedrawCurrentFractal;
            iterations.Value = 0;
            iterations.ValueChanged += RedrawCurrentFractal;


            if (!handlers.TryGetValue(selectedFractal.Name, out Action handler))
            {
                return;
            }

            handler();
            RedrawCurrentFractal(sender, EventArgs.Empty);
        }

        /// <summary>
        /// Возвращает Bitmap с отрисованным drawSurface.
        /// </summary>
        /// <returns>Bitmap с отрисованным drawSurface.</returns>
        private RenderTargetBitmap GetCanvasBitmap()
        {
            var bounds = new Size(drawSurface.ActualWidth, drawSurface.ActualHeight);
            const double dpi = 96;

            var renderTargetBitmap = new RenderTargetBitmap((int)bounds.Width, (int)bounds.Height,
                dpi, dpi, PixelFormats.Default);
            
            var outSurface = new DrawingVisual();
            using (var context = outSurface.RenderOpen())
            {
                var vb = new VisualBrush(drawSurface);
                context.DrawRectangle(vb, null, new Rect(new Point(), bounds));
            }

            renderTargetBitmap.Render(outSurface);

            return renderTargetBitmap;
        }

        /// <summary>
        /// Обработчик нажатия на кнопку сохранения изображения.
        /// </summary>
        /// <param name="sender">Инициатор события.</param>
        /// <param name="e">Аргументы события.</param>
        private void saveImageButton_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new SaveFileDialog
            {
                OverwritePrompt = true,
                Title = "Сохранить изображение...",
                FileName = "Untitled",
                DefaultExt = ".png",
                Filter = "Изображение PNG (.png)|*.png"
            };

            if (dialog.ShowDialog() != true)
            {
                return;
            }

            string filename = dialog.FileName;

            BitmapEncoder pngEncoder = new PngBitmapEncoder();
            pngEncoder.Frames.Add(BitmapFrame.Create(GetCanvasBitmap()));

            try
            {
                using var stream = new MemoryStream();
                pngEncoder.Save(stream);
                File.WriteAllBytes(filename, stream.ToArray());
            }
            catch (Exception error)
            {
                MessageBox.Show($"Возникла ошибка при попытке сохранить изображение: {error}",
                    "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        /// <summary>
        /// Обработчик нажатия на левую кнопку мыши.
        /// </summary>
        /// <param name="sender">Инициатор события.</param>
        /// <param name="e">Аргументы события.</param>
        private void drawSurface_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            drawSurface.Cursor = Cursors.ScrollAll;
            tempPoint = e.GetPosition(drawSurface);
        }

        /// <summary>
        /// Обработчик ситуации, когда пользователь отпускает левую клавишу мыши.
        /// </summary>
        /// <param name="sender">Инициатор события.</param>
        /// <param name="e">Аргументы события.</param>
        private void drawSurface_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            drawSurface.Cursor = Cursors.Arrow;
        }

        /// <summary>
        /// Обработчик движения мыши над drawSurface.
        /// </summary>
        /// <param name="sender">Инициатор события.</param>
        /// <param name="e">Аргументы события.</param>
        private void drawSurface_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.LeftButton != MouseButtonState.Pressed)
            {
                return;
            }

            const double sensitivity = 1.05;
            var cursorPoint = e.GetPosition(drawSurface);
            var displacementVector = new Vector(cursorPoint.X - tempPoint.X, cursorPoint.Y - tempPoint.Y);
            tempPoint = cursorPoint;

            foreach (var child in drawSurface.Children)
            {
                double displacementSensitivity = sensitivity * scaleFactor;
                switch (child)
                {
                    case Line line:
                        line.X1 += displacementVector.X / displacementSensitivity;
                        line.X2 += displacementVector.X / displacementSensitivity;
                        line.Y1 += displacementVector.Y / displacementSensitivity;
                        line.Y2 += displacementVector.Y / displacementSensitivity;
                        break;
                    case Polygon polygon:
                        for (var index = 0; index < polygon.Points.Count; index++)
                        {
                            var newPoint = polygon.Points[index];
                            newPoint.X += displacementVector.X / displacementSensitivity;
                            newPoint.Y += displacementVector.Y / displacementSensitivity;

                            polygon.Points[index] = newPoint;
                        }

                        break;
                }
            }
        }

        /// <summary>
        /// Изменяет масштаб элементов.
        /// </summary>
        /// <param name="scale">Масштаб.</param>
        private void ScaleElements(double scale)
        {
            scaleFactor = scale;
            foreach (UIElement child in drawSurface.Children)
            {
                var transform = new ScaleTransform();

                var centerX = drawSurface.ActualWidth / 2;
                var centerY = drawSurface.ActualHeight / 2;
                transform.CenterX = centerX;
                transform.CenterY = centerY;
                transform.ScaleX = scale;
                transform.ScaleY = scale;
                child.RenderTransform = transform;
            }
        }

        /// <summary>
        /// Обработчик события нажатия на кнопку увеличения масштаба в два раза.
        /// </summary>
        /// <param name="sender">Инициатор события.</param>
        /// <param name="e">Аргументы события.</param>
        private void zoomIn2_Click(object sender, RoutedEventArgs e)
        {
            ScaleElements(2);
        }

        /// <summary>
        /// Обработчик события нажатия на кнопку увеличения масштаба в три раза.
        /// </summary>
        /// <param name="sender">Инициатор события.</param>
        /// <param name="e">Аргументы события.</param>
        private void zoomIn3_Click(object sender, RoutedEventArgs e)
        {
            ScaleElements(3);
        }

        /// <summary>
        /// Обработчик события нажатия на кнопку увеличения масштаба в пять раз.
        /// </summary>
        /// <param name="sender">Инициатор события.</param>
        /// <param name="e">Аргументы события.</param>
        private void zoomIn5_Click(object sender, RoutedEventArgs e)
        {
            ScaleElements(5);
        }

        /// <summary>
        /// Обработчик события нажатия на кнопку уменьшения масштаба в два раза.
        /// </summary>
        /// <param name="sender">Инициатор события.</param>
        /// <param name="e">Аргументы события.</param>
        private void zoomOut2_Click(object sender, RoutedEventArgs e)
        {
            ScaleElements(1.0 / 2.0);
        }

        /// <summary>
        /// Обработчик события нажатия на кнопку уменьшения масштаба в три раза.
        /// </summary>
        /// <param name="sender">Инициатор события.</param>
        /// <param name="e">Аргументы события.</param>
        private void zoomOut3_Click(object sender, RoutedEventArgs e)
        {
            ScaleElements(1.0 / 3.0);
        }

        /// <summary>
        /// Обработчик события нажатия на кнопку уменьшения масштаба в пять раз.
        /// </summary>
        /// <param name="sender">Инициатор события.</param>
        /// <param name="e">Аргументы события.</param>
        private void zoomOut5_Click(object sender, RoutedEventArgs e)
        {
            ScaleElements(1.0 / 5.0);
        }

        /// <summary>
        /// Обработчик события нажатия на кнопку сброса масштаба.
        /// </summary>
        /// <param name="sender">Инициатор события.</param>
        /// <param name="e">Аргументы события.</param>
        private void resetZoomOut_Click(object sender, RoutedEventArgs e)
        {
            RedrawCurrentFractal(sender, e);
        }

        /// <summary>
        /// Обработчик события нажатия на кнопку сброса масштаба.
        /// </summary>
        /// <param name="sender">Инициатор события.</param>
        /// <param name="e">Аргументы события.</param>
        private void resetZoomIn_Click(object sender, RoutedEventArgs e)
        {
            RedrawCurrentFractal(sender, e);
        }
    }
}
