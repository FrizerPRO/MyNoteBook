using System;

namespace Matrix
{
    /// <summary>
    /// Сам класс
    /// </summary>
    class Program
    {
        /// <summary>
        /// Метод Main.
        /// Тут почти ничего не происходит,все передается через остальные методы.
        /// </summary>
        static void Main()
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            ChooseOperation(Greetings());
            //Принимается значение из метода Greetings и передается в метод ChooseOperation для дальнейших расчетов.
            RestartProgram();
            //Узнает у пользователя о желании воспользоваться калькулятором еще раз. 
        }
        /// <summary>
        /// Этот метод приветствует пользователя и принимает от него значение,чтобы дальше перейти к функции для работы с матрицей.
        /// </summary>
        /// <returns>Возвращает ввод пользователя в метод ChooseOperation для перехода к дальнейшей операции с матрицей</returns>
        static uint Greetings()
        {
            uint count;
            Console.WriteLine("Привет,дорогой пользователь.");
            Console.WriteLine("Чтобы продолжить,выбери операцию,которую хочешь совершить с матрицей.");
            Console.WriteLine("После чего нажми на соответствующую цифру:\n");
            Console.WriteLine("\'1\' - найти след матрицы");
            Console.WriteLine("\'2\' - транспонировать матрицу");
            Console.WriteLine("\'3\' - найти сумму двух матриц");
            Console.WriteLine("\'4\' - найти разность двух матриц");
            Console.WriteLine("\'5\' - найти произведение двух матриц");
            Console.WriteLine("\'6\' - умножить матрицу на число");
            Console.WriteLine("\'7\' - найти определитель матрицы");
            Console.WriteLine("\'8\' - выйти из программы");
            do
            {
                Console.Write("Выберите операцию: ");
            }
            while (!uint.TryParse(Console.ReadLine(), out count) || count == 0 || count >= 9);
            return count;
        }
        /// <summary>
        /// Это один из самых важных методов в программе,при его вызове генерируется матрица,и принимаются матрицы,либо генерируются
        /// случайно в зависимости от ввода пользователя.
        /// </summary>
        /// <returns>Возвращает саму матрицу,которую сгенерировал пользователь для методов калькулятора </returns>
        static double[,] Matrix_Generation()
        {
            int i, j;
            uint isRandom;
            Console.WriteLine("Хотите ли вы сгенерировать элементы матрицы случайным образом?");
            Console.WriteLine("Для экономии ресурсов генерация производится на диапазоне от [-100;100]");
            Console.WriteLine("\'1\' - да\n\'0\' - нет");
            do
            {
                Console.Write("Выберите один из предложенных вариантов: ");
            }
            while (!uint.TryParse(Console.ReadLine(), out isRandom) || (isRandom != 1 && isRandom != 0));
            var Rand = new Random();
            Console.WriteLine("Задайте размер матрицы. ");
            do
            {
                Console.Write("Введите количество строк: ");
            }
            while (!int.TryParse(Console.ReadLine(), out i) || i <= 0);
            do
            {
                Console.Write("Введите количество столбцов: ");
            }
            while (!int.TryParse(Console.ReadLine(), out j) || j <= 0);
            double[,] Matrix = new double[i, j];
            for (int i1 = 0; i1 < i; i1++)
            {
                for (int j1 = 0; j1 < j; j1++)
                {
                    Matrix[i1, j1] = isRandom == 1 ? Rand.Next(-100, 101) : CheckCorrectionOfVariable(i1, j1);
                }
            }
            return Matrix;
        }
        /// <summary>
        /// Проверяет корректность ввода пользователя,используется при генерации матриц,если пользователь сам захочет задать элементы матрицы.
        /// </summary>
        /// <param name="i">Принимает из метода Matrix_Generation i-ую строку матрицы</param>
        /// <param name="j">Принимает из метода Matrix_Generation j-ую строку матрицы</param>
        /// <returns>Возвращает корректный ввод пользователя обратно в метод Matrix_Generation</returns>
        static double CheckCorrectionOfVariable(int i, int j)
        {
            double element;
            do
            {
                Console.Write($"Введите элемент {i + 1} строки и {j + 1} столбца: ");
            }
            while (!double.TryParse(Console.ReadLine(), out element));
            return element;
        }
        /// <summary>
        /// Выводит на консоль пользователя сгенерированную им матрицу,либо уже результат работы калькулятора.
        /// Используется часто,поэтому имеет смысл на выделение отдельного метода для этой функции.
        /// </summary>
        /// <param name="matrix">Принимает матрицу,которую нужно вывести на консоль</param>
        static void PrintMatrix(double[,] matrix)
        {
            for (int i = 0; i < matrix.GetLength(0); i++)
            {
                for (int j = 0; j < matrix.GetLength(1); j++)
                {
                    Console.Write("  " + Math.Round(matrix[i, j], 2) + "  ");
                }
                Console.WriteLine();
            }
        }
        /// <summary>
        /// Метод для перезапуска программы.
        /// </summary>
        static void RestartProgram()
        {
            uint restartprogram;
            do
            {
                Console.WriteLine("\nХотите ли воспользоваться калькулятором снова?");
                Console.WriteLine("\'1\' - да\n\'0\' - нет");

            }
            while (!uint.TryParse(Console.ReadLine(), out restartprogram) || restartprogram >= 2);
            if (restartprogram == 0)
            {
                Environment.Exit(0);
            }
            else
            {
                Main();
            }
        }
        /// <summary>
        /// Из метода Greetings приходит значение введенного пользователем и после этого запускается метод калькулятора,выбранный при запуске.
        /// </summary>
        /// <param name="count">Значение введенное пользователем из метода Greetings</param>
        static void ChooseOperation(uint count)
        {
            switch (count)
            {
                case 1:
                    MatrixTrace();
                    break;
                case 2:
                    TransposeMatrix();
                    break;
                case 3:
                    SumMatrix();
                    break;
                case 4:
                    DifferenceMatrix();
                    break;
                case 5:
                    MultiplicationMatrix();
                    break;
                case 6:
                    MultiplicationCounterMatrix();
                    break;
                case 7:
                    DeterminantMatrix();
                    break;
                case 8:
                    Environment.Exit(0);
                    break;
            }
        }
        /// <summary>
        /// Нахождения следа матрицы.
        /// </summary>
        static void MatrixTrace()
        {
            double[,] matrix;
            do
            {
                Console.WriteLine("\nДля нахождения следа количество строк и столбцов должно быть одинаковым.");
                matrix = Matrix_Generation();
            }
            while (matrix.GetLength(0) != matrix.GetLength(1));
            Console.WriteLine("Ваша матрица: ");
            PrintMatrix(matrix);
            double sum = 0;
            for (int i = 0; i < matrix.GetLength(0); i++)
            {
                for (int j = 0; j < matrix.GetLength(1); j++)
                {
                    if (i == j)
                    {
                        sum += matrix[i, j];
                    }
                }
            }
            Console.WriteLine($"След Вашей матрицы равен: {sum}");
        }
        /// <summary>
        /// Транспонирование матрицы.
        /// </summary>
        static void TransposeMatrix()
        {
            Console.WriteLine();
            double[,] matrix = Matrix_Generation();
            double[,] transposematrix = new double[matrix.GetLength(1), matrix.GetLength(0)];
            Console.WriteLine("Ваша матрица: ");
            PrintMatrix(matrix);
            for (int i = 0; i < matrix.GetLength(0); i++)
            {
                for (int j = 0; j < matrix.GetLength(1); j++)
                {
                    transposematrix[j, i] = matrix[i, j];
                    //Меняет строки и столбцы местами.
                }
            }
            Console.WriteLine("Ваша транспонированная матрица: ");
            PrintMatrix(transposematrix);
        }
        /// <summary>
        /// Нахождение суммы матриц.
        /// </summary>
        static void SumMatrix()
        {
            double[,] firstmatrix;
            double[,] secondmatrix;
            do
            {
                Console.WriteLine("\nДля успешного сложения матриц количество строк и столбцов у обоих матриц должно быть одинаковым. ");
                Console.WriteLine("Задайте первую матрицу. ");
                firstmatrix = Matrix_Generation();
                Console.WriteLine("\nЗадайте вторую матрицу. ");
                secondmatrix = Matrix_Generation();
            }
            while ((firstmatrix.GetLength(0) != secondmatrix.GetLength(0)) || (firstmatrix.GetLength(1) != secondmatrix.GetLength(1)));
            //Создаются две матрицы с помощью метода Matrix_Generation() и проверяются на корректность.
            Console.WriteLine("\nВаша первая матрица: ");
            PrintMatrix(firstmatrix);
            Console.WriteLine("\nВаша вторая матрица: ");
            PrintMatrix(secondmatrix);
            double[,] summatrix = new double[firstmatrix.GetLength(0), firstmatrix.GetLength(1)];
            for (int i = 0; i < firstmatrix.GetLength(0); i++)
            {
                for (int j = 0; j < firstmatrix.GetLength(1); j++)
                {
                    summatrix[i, j] = firstmatrix[i, j] + secondmatrix[i, j];
                    //Сумма элементов матрицы.
                }
            }
            Console.WriteLine("\nВаша просумированная матрица");
            PrintMatrix(summatrix);
        }
        /// <summary>
        /// Нахождение разности матриц.
        /// </summary>
        static void DifferenceMatrix()
        {
            double[,] firstmatrix;
            double[,] secondmatrix;
            do
            {
                Console.WriteLine("Для успешного сложения матриц количество строк и столбцов у обоих матриц должно быть одинаковым. ");
                Console.WriteLine("\nЗадайте первую матрицу. ");
                firstmatrix = Matrix_Generation();
                Console.WriteLine("\nЗадайте вторую матрицу. ");
                secondmatrix = Matrix_Generation();
            }
            while ((firstmatrix.GetLength(0) != secondmatrix.GetLength(0)) || (firstmatrix.GetLength(1) != secondmatrix.GetLength(1)));
            //Создаются две матрицы с помощью метода Matrix_Generation() и проверяются на корректность.
            Console.WriteLine("\nВаша первая матрица: ");
            PrintMatrix(firstmatrix);
            Console.WriteLine("\nВаша вторая матрица: ");
            PrintMatrix(secondmatrix);
            double[,] differencematrix = new double[firstmatrix.GetLength(0), firstmatrix.GetLength(1)];
            for (int i = 0; i < firstmatrix.GetLength(0); i++)
            {
                for (int j = 0; j < firstmatrix.GetLength(1); j++)
                {
                    differencematrix[i, j] = firstmatrix[i, j] - secondmatrix[i, j];
                    //Разность элементов матрицы.
                    
                }
            }
            Console.WriteLine("\nВаша просумированная матрица");
            PrintMatrix(differencematrix);
        }
        /// <summary>
        /// Нахождение произведения матриц.
        /// </summary>
        static void MultiplicationMatrix()
        {
            double[,] firstmatrix;
            double[,] secondmatrix;
            do
            {
                Console.WriteLine("Для успешного умножения матриц количество столбцов первой матрицы и строк второй матрицы должно быть одинаковым. ");
                Console.WriteLine("\nЗадайте первую матрицу. ");
                firstmatrix = Matrix_Generation();
                Console.WriteLine("\nЗадайте вторую матрицу. ");
                secondmatrix = Matrix_Generation();
            }
            while ((firstmatrix.GetLength(1) != secondmatrix.GetLength(0)));
            //Создаются две матрицы с помощью метода Matrix_Generation() и проверяются на корректность.
            Console.WriteLine("\nВаша первая матрица: ");
            PrintMatrix(firstmatrix);
            Console.WriteLine("\nВаша вторая матрица: ");
            PrintMatrix(secondmatrix);
            double[,] multiplicationmatrix = new double[firstmatrix.GetLength(0), secondmatrix.GetLength(1)];
            for (int i = 0; i < firstmatrix.GetLength(0); i++)
            {
                for (int j = 0; j < secondmatrix.GetLength(1); j++)
                {
                    multiplicationmatrix[i, j] = 0;

                    for (int k = 0; k < firstmatrix.GetLength(1); k++)
                    {
                        multiplicationmatrix[i, j] += firstmatrix[i, k] * secondmatrix[k, j];
                        //Выполняет произведение матриц по определению,суммируются элементы и записываются в новый массив.
                    }
                }
            }
            Console.WriteLine("\nВаша умноженная матрица");
            PrintMatrix(multiplicationmatrix);
        }
        /// <summary>
        /// Нахождение произведения матрицы с числом.
        /// </summary>
        static void MultiplicationCounterMatrix()
        {
            double[,] matrix = Matrix_Generation();
            double counter;
            do
            {
                Console.Write("Задайте коэффицент,на который нужно умножить матрицу: ");
            }
            while (!double.TryParse(Console.ReadLine(), out counter));
            double[,] multiplicationcountermatrix = new double[matrix.GetLength(0), matrix.GetLength(1)];
            for (int i = 0; i < matrix.GetLength(0); i++)
            {
                for (int j = 0; j < matrix.GetLength(1); j++)
                {
                    multiplicationcountermatrix[i, j] = matrix[i, j] * counter;
                }
            }
            Console.WriteLine("\nВаша умноженная матрица");
            PrintMatrix(multiplicationcountermatrix);
        }
        /// <summary>
        /// Нахождение определителя матрицы.
        /// </summary>
        static void DeterminantMatrix()
        {
            double[,] matrix;
            do
            {
                Console.WriteLine("\nДля нахождения определителя количество строк и столбцов должно быть одинаковым.");
                matrix = Matrix_Generation();
            }
            while (matrix.GetLength(0) != matrix.GetLength(1));
            Console.WriteLine("\nВаша первая матрица: ");
            PrintMatrix(matrix);
            double[][] toothedmatrix = new double[matrix.GetLength(0)][];
            for (int i = 0; i < matrix.GetLength(0); i++)
            {
                toothedmatrix[i] = new double[matrix.GetLength(0)];
            }
            for (int i = 0; i < matrix.GetLength(0); i++)
            {
                for (int j = 0; j < matrix.GetLength(0); j++)
                {
                    toothedmatrix[i][j] = matrix[i, j];
                }
            }
            //Перевод двумерного массива в зубчатый для использования вспомогательного метода ищущего детерминант матрицы.
            Console.WriteLine("Определитель вашей матрицы: ");
            Console.WriteLine(GetMinor(toothedmatrix));
        }
        /// <summary>
        /// Вспомогательный рекурсивный метод для нахождения минора матрицы.
        /// </summary>
        /// <param name="matrix">Принимает матрицу зубчатого вида и находит от нее определитель</param>
        /// <returns>Возвращает определитель переданной по атрибуту матрицы</returns>
        static double GetMinor(double[][] matrix)
        {
            double returning;
            if (matrix.Length == 2)
            {
                returning = matrix[0][0] * matrix[1][1] - matrix[0][1] * matrix[1][0];
                //Выход из рекурсии.
            }
            else
            {
                double[][] minor = new double[matrix.Length - 1][]; //Минор, но возможно, что это матрица n-ого порядка.
                int i, j, k;
                short minus = 1;
                double temp;
                returning = 0;
                //Разложение матрицы n на n с помощью минора и рекурсивной функции.
                //После дохождения всех матриц к виду 2 на 2 считается значение returning и возвращается в метод.
                for (i = 0; i < matrix.Length; i++)
                {
                    for (j = 1; j < matrix.Length; j++)
                    {
                        //Сохраняю значения для "возможного" минора.
                        minor[j - 1] = new double[matrix.Length - 1];
                        for (k = 0; k < i; k++) 
                            //Значения до диагонали.
                            minor[j - 1][k] = matrix[j][k];

                        for (k++; k < matrix.Length; k++)   
                            //Значения после диагонали.
                            minor[j - 1][k - 1] = matrix[j][k];
                    }

                    temp = GetMinor(minor);
                    temp = matrix[0][i] * minus * temp;
                    returning += temp;

                    if (minus > 0)  
                        //Меняем знак, согласно правилам.
                        minus = -1;
                    else
                        minus = 1;
                }
            }
            return returning;
        }
    }
}
