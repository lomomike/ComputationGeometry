using System.Globalization;

namespace Polgun.ComputationGeometry
{
    /// <summary>
    /// Сохраняет упорядоченную пару целых чисел, обычно ширину и высоту прямоугольника
    /// </summary>
    public struct Size
    {
        public static readonly Size Empty;
        private double width;
        private double height;

        /// <summary>
        /// Инициализирует новый экземпляр класса Size из указанного объекта Size
        /// </summary>
        /// <param name="size">Объект Size, используемый для инициализации данного размера Size</param>
        public Size(Size size)
        {
            this.width = size.width;
            this.height = size.height;
        }

        /// <summary>
        /// Инициализирует новый экземпляр класса Size из указанного объекта Point
        /// </summary>
        /// <param name="pt">Объект Point, используемый для инициализации данного размера Size</param>
        public Size(Point pt)
        {
            this.width = pt.X;
            this.height = pt.Y;
        }

        /// <summary>
        /// Инициализирует новый экземпляр класса Size из указанных размеров
        /// </summary>
        /// <param name="width">Компонент ширины нового размера Size</param>
        /// <param name="height">Компонент высоты нового размера Size</param>
        public Size(double width, double height)
        {
            this.width = width;
            this.height = height;
        }

        /// <summary>
        /// Прибавляет ширину и высоту одной структуры Size к ширине и высоте другой структуры Size
        /// </summary>
        /// <param name="sz1">Первый прибавляемый размер Size</param>
        /// <param name="sz2">Второй прибавляемый размер Size</param>
        /// <returns>Структура Size, получаемая в результате операции сложения</returns>
        public static Size operator +(Size sz1, Size sz2)
        {
            return Add(sz1, sz2);
        }

        /// <summary>
        /// Вычитает ширину и высоту одной структуры Size из ширины и высоты другой структуры Size
        /// </summary>
        /// <param name="sz1">Структура Size, которая находится слева от оператора вычитания</param>
        /// <param name="sz2">Структура Size, которая находится справа от оператора вычитания</param>
        /// <returns>Структура Size, полученная в результате операции вычитания</returns>
        public static Size operator -(Size sz1, Size sz2)
        {
            return Subtract(sz1, sz2);
        }

        /// <summary>
        /// Проверяет равенство двух структур Size
        /// </summary>
        /// <param name="sz1">Структура Size, которая находится слева от оператора равенства</param>
        /// <param name="sz2">Структура Size, которая находится справа от оператора равенства</param>
        /// <returns>Значение true, если у sz1 и sz2 равны значения ширины и высоты, в противном случае значение false</returns>
        public static bool operator ==(Size sz1, Size sz2)
        {
            return ((sz1.Width == sz2.Width) && (sz1.Height == sz2.Height));
        }

        /// <summary>
        /// Проверяет, различны ли две структуры Size
        /// </summary>
        /// <param name="sz1">Структура Size, которая находится слева от оператора неравенства</param>
        /// <param name="sz2">Структура Size, которая находится справа от оператора равенства</param>
        /// <returns>Значение true, если параметры sz1 и sz2 отличаются по ширине или высоте, и значение false, если параметры sz1 и sz2 равны</returns>
        public static bool operator !=(Size sz1, Size sz2)
        {
            return !(sz1 == sz2);
        }

        /// <summary>
        /// Преобразует указанный размер Size в точку Point
        /// </summary>
        /// <param name="size">Преобразуемый объект Size</param>
        /// <returns>Структура Point, которая является результатом преобразования, выполненного с помощью этого оператора</returns>
        public static explicit operator Point(Size size)
        {
            return new Point(size.Width, size.Height);
        }

        /// <summary>
        /// Проверяет, равны ли 0 ширина и высота размера Size
        /// </summary>
        public bool IsEmpty
        {
            get
            {
                return ((this.width == 0f) && (this.height == 0f));
            }
        }

        /// <summary>
        /// Получает или задает горизонтальный компонент размера Size
        /// </summary>
        public double Width
        {
            get
            {
                return this.width;
            }
            set
            {
                this.width = value;
            }
        }

        /// <summary>
        /// Получает или задает вертикальный компонент размера Size
        /// </summary>
        public double Height
        {
            get
            {
                return this.height;
            }
            set
            {
                this.height = value;
            }
        }

        /// <summary>
        /// Прибавляет ширину и высоту одной структуры Size к ширине и высоте другой структуры Size
        /// </summary>
        /// <param name="sz1">Первый прибавляемый размер Size</param>
        /// <param name="sz2">Второй прибавляемый размер Size</param>
        /// <returns>Структура Size, получаемая в результате операции сложения</returns>
        public static Size Add(Size sz1, Size sz2)
        {
            return new Size(sz1.Width + sz2.Width, sz1.Height + sz2.Height);
        }

        /// <summary>
        /// Вычитает ширину и высоту одной структуры Size из ширины и высоты другой структуры Size
        /// </summary>
        /// <param name="sz1">Структура Size, которая находится слева от оператора вычитания</param>
        /// <param name="sz2">Структура Size, которая находится справа от оператора вычитания</param>
        /// <returns>Размер Size, полученный в результате операции вычитания</returns>
        public static Size Subtract(Size sz1, Size sz2)
        {
            return new Size(sz1.Width - sz2.Width, sz1.Height - sz2.Height);
        }

        /// <summary>
        /// Проверяет, является совпадают ли размеры указанного объекта Size с размерами объекта Size
        /// </summary>
        /// <param name="obj">Объект Object для проверки</param>
        /// <returns>Значение true, если параметр obj является размером Size и 
        /// его значения ширины и высоты совпадают с соответствующими значениями размера Size, в противном случае значение false</returns>
        public override bool Equals(object obj)
        {
            if (!(obj is Size))
            {
                return false;
            }
            Size ef = (Size)obj;
            return (((ef.Width == this.Width) && (ef.Height == this.Height)) && ef.GetType().Equals(base.GetType()));
        }

        /// <summary>
        /// Возвращает хэш-код для этой структуры Size
        /// </summary>
        /// <returns>Целое значение, указывающее значение хэша для этой структуры Size</returns>
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        /// <summary>
        /// Преобразует указанный размер Size в точку Point
        /// </summary>
        /// <returns>Структура Point, которая является результатом преобразования</returns>
        public Point ToPoint()
        {
            return (Point)this;
        }

        /// <summary>
        /// Создает удобную для восприятия строку, представляющую размер GisSize
        /// </summary>
        /// <returns>Строка, представляющая размер GisSize</returns>
        public override string ToString()
        {
            return ("{Width=" + this.width.ToString(CultureInfo.CurrentCulture) +
                    ", Height=" + this.height.ToString(CultureInfo.CurrentCulture) + "}");
        }

        static Size()
        {
            Empty = new Size();
        }
    }
}