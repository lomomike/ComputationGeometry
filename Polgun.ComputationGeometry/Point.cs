using System;
using System.Diagnostics;

namespace Polgun.ComputationGeometry
{

    /// <summary>
    /// Представляет упорядоченную пару x- и y-координат  типа double, определяющих точку в декартовой системе координат
    /// </summary>
    public struct Point
    {
        private double m_x;
        private double m_y;

        /// <summary>
        /// Инициализирует новый экземпляр структуры Point с указанными координатами.
        /// </summary>
        /// <param name="x">Горизонтальная позиция точки</param>
        /// <param name="y">Вертикальная позиция точки</param>
        [DebuggerStepThrough]
        public Point(double x, double y)
        {
            m_x = x;
            m_y = y;
        }

        /// <summary>
        /// Получает или задает координату Х точки Point
        /// </summary>
        public double X
        {
            get { return m_x; }
            set { m_x = value; }
        }

        /// <summary>
        /// Получает или задает координату Y точки Point
        /// </summary>
        public double Y
        {
            get { return m_y; }
            set { m_y = value; }
        }

        /// <summary>
        /// Получает значение, определяющее, пуста ли структура Point.
        /// </summary>
        public bool IsEmpty
        {
            get { return m_x == 0.0 && m_y == 0.0; }
        }

        /// <summary>
        /// Преобразует объект Point в строку, доступную для чтения.
        /// </summary>
        /// <returns>Строка, представляющая структуру Point.</returns>
        public override string ToString()
        {
            return string.Format("{{X={0}, Y = {1}}}", m_x, m_y);
        }

        /// <summary>
        /// Возвращает хэш-код для этой структуры Point.
        /// </summary>
        /// <returns>Целое значение, указывающее значение хэша для этой структуры Point.</returns>
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        /// <summary>
        /// Определяет, содержит или нет объект Point те же координаты, что и указанный объект Object.
        /// </summary>
        /// <param name="obj">Объект Object для проверки.</param>
        /// <returns>Метод возвращает значение true, если obj является Point и имеет такие же координаты как и данный Point</returns>
        public override bool Equals(Object obj)
        {
            if (obj == null) return false;
            if (this.GetType() != obj.GetType()) return false;

            Point gpTmp = (Point)obj;
            return this.m_x == gpTmp.m_x && this.m_y == gpTmp.m_y;
        }


        #region Static Members
        /// <summary>
        /// Представляет новый экземпляр класса Point с неинициализированными данными членов.
        /// </summary>
        public static readonly Point Empty = new Point();

        /// <summary>
        /// Смещает указанную точку Point на заданное значение Size.
        /// </summary>
        /// <param name="pt">Класс Point для преобразования.</param>
        /// <param name="sz">Объект Size, определяющий числа, которые нужно добавить к значениям координат pt.</param>
        /// <returns>Смещенная точка Point.</returns>
        public static Point Add(Point pt, Size sz)
        {
            return new Point(pt.X + sz.Width, pt.Y + sz.Height);
        }

        /// <summary>
        /// Смещает указанную точку Point на заданное значение Size.
        /// </summary>
        /// <param name="pt">Класс Point для преобразования.</param>
        /// <param name="sz">Объект Size, определяющий числа, которые нужно добавить к значениям координат pt.</param>
        /// <returns>Смещенная точка Point.</returns>
        public static Point operator +(Point pt, Size sz)
        {
            return Add(pt, sz);
        }

       /// <summary>
        /// Сравнивает две структуры Point. Результат определяет, равны или нет значения свойств X и Y двух структур point.
        /// </summary>
        /// <param name="left">Объект Point для сравнения.</param>
        /// <param name="right">Объект Point для сравнения.</param>
        /// <returns>Значение true, если значения X и Y левой и правой структур Point равны; в противном случае — false.</returns>
        public static bool operator ==(Point left, Point right)
        {
            return ((left.X == right.X) && (left.Y == right.Y));

        }

        /// <summary>
        /// Определяет, равны или нет координаты указанных точек.
        /// </summary>
        /// <param name="left">Объект Point для сравнения.</param>
        /// <param name="right">Объект Point для сравнения.</param>
        /// <returns>Значение true, чтобы указать, что значения X и Y параметров left и right не равны; в противном случае — false. </returns>
        public static bool operator !=(Point left, Point right)
        {
            return !(left == right);
        }

        /// <summary>
        /// Смещает указанную точку Point на отрицательную величину заданного размера.
        /// </summary>
        /// <param name="pt">Класс Point для смещения.</param>
        /// <param name="sz">Размер Size, указывающий числа для вычитания из координат pt.</param>
        /// <returns>Смещенная точка Point.</returns>
        public static Point Substract(Point pt, Size sz)
        {
            return new Point(pt.X - sz.Width, pt.Y - sz.Height);
        }

        /// <summary>
        /// Смещает указанную точку Point на отрицательную величину заданного размера.
        /// </summary>
        /// <param name="pt">Класс Point для смещения.</param>
        /// <param name="sz">Размер Size, указывающий числа для вычитания из координат pt.</param>
        /// <returns>Смещенная точка Point.</returns>
        public static Point operator -(Point pt, Size sz)
        {
            return Substract(pt, sz);
        }

       
        #endregion
    }

}