using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Практическая_работа_4_Козырь_Бредихин;
namespace UnitTestProject_PR4
{
    /// <summary>
    /// Набор автоматизированных модульных тестов для математических функций
    /// из Практической работы №4 (метод «белого ящика»).
    /// </summary>
    [TestClass]
    public class UnitTest1
    {
        // ═══════════════════════════════════════════════════════════════════
        // Учебный тест — TestMethod1 (для тренировки работы с Assert)
        // ═══════════════════════════════════════════════════════════════════

        /// <summary>
        /// Тренировочный тест, демонстрирующий основные методы объекта <see cref="Assert"/>:
        ///   Assert.AreEqual — проверяет равенство двух значений
        ///   Assert.AreNotEqual — проверяет неравенство двух значений
        ///   Assert.IsTrue — проверяет, что условие истинно
        ///   Assert.IsFalse — проверяет, что условие ложно
        ///   Assert.IsNull — проверяет, что объект равен null
        ///   Assert.IsNotNull — проверяет, что объект не равен null.
        /// </summary>
        [TestMethod]
        public void TestMethod1()
        {
            Assert.AreEqual(4, 2 + 2);
            Assert.AreEqual(0.3, 0.1 + 0.2, 1e-10);   

            Assert.AreNotEqual(5, 2 + 2);

            Assert.IsTrue(10 > 5);
            Assert.IsFalse(double.IsNaN(1.0));

            string s = null;
            Assert.IsNull(s);
            Assert.IsNotNull("hello");
        }

        // Тесты для MathFunctions.Page1  (формула страницы 1)

        /// <summary>
        /// Проверяет корректный результат при типичных допустимых входных данных
        /// для функции страницы 1 (x=2, y=1, z=1).
        /// </summary>
        [TestMethod]
        public void TestPage1_ValidInput_ReturnsCorrectResult()
        {
            double x = 2.0, y = 1.0, z = 1.0;
            double result = MathFunctions.ComputePage1(x, y, z);
            Assert.IsFalse(double.IsNaN(result), "Результат не должен быть NaN");
            Assert.IsFalse(double.IsInfinity(result), "Результат не должен быть бесконечностью");
            double inner = x - (2.0 * y) / (1.0 + x * x * y * y);
            double expected = (1.0 + Math.Pow(Math.Sin(x + y), 2)) / Math.Abs(inner)
                                * Math.Pow(x, Math.Abs(y))
                                + Math.Pow(Math.Cos(Math.Atan(1.0 / z)), 2);

            Assert.AreEqual(expected, result, 1e-10);
        }

        /// <summary>
        /// Проверяет, что при z = 0 выбрасывается исключение <see cref="ArgumentException"/>.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void TestPage1_ZeroZ_ThrowsArgumentException()
        {
            MathFunctions.ComputePage1(1.0, 1.0, 0.0);
        }

        /// <summary>
        /// Проверяет, что при x &lt; 0 и нецелом |y| выбрасывается исключение <see cref="ArgumentException"/>,
        /// так как x^|y| не определён в вещественных числах.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void TestPage1_NegativeXNonIntegerY_ThrowsArgumentException()
        {
            MathFunctions.ComputePage1(-2.0, 1.5, 1.0);
        }

        // Тесты для MathFunctions.Page2  (кусочная функция, страница 2)

        /// <summary>
        /// Проверяет ветку x == y: c = f²(x) + y² + sin(y)
        /// при режиме "sinh" и x = y = 1.
        /// </summary>
        [TestMethod]
        public void TestPage2_XEqualsY_CorrectBranch()
        {
            double x = 1.0, y = 1.0;
            double result = MathFunctions.ComputePage2("sinh", x, y);
            double fx = Math.Sinh(x);
            double expected = fx * fx + y * y + Math.Sin(y);
            Assert.AreEqual(expected, result, 1e-10);
        }

        /// <summary>
        /// Проверяет ветку x &gt; y: c = (f(x) − y)² + cos(y)
        /// при режиме "x2" и x=3, y=1.
        /// </summary>
        [TestMethod]
        public void TestPage2_XGreaterThanY_CorrectBranch()
        {
            double x = 3.0, y = 1.0;
            double result = MathFunctions.ComputePage2("x2", x, y);
            double fx = x * x;                
            double expected = Math.Pow(fx - y, 2) + Math.Cos(y);
            Assert.AreEqual(expected, result, 1e-10);
        }

        /// <summary>
        /// Проверяет ветку x &lt; y: c = (y − f(x))² + tan(y)
        /// при режиме "exp" и x=0, y=1.
        /// </summary>
        [TestMethod]
        public void TestPage2_XLessThanY_CorrectBranch()
        {
            double x = 0.0, y = 1.0;
            double result = MathFunctions.ComputePage2("exp", x, y);
            double fx = Math.Exp(x);         
            double expected = Math.Pow(y - fx, 2) + Math.Tan(y);
            Assert.AreEqual(expected, result, 1e-10);
        }

        /// <summary>
        /// Проверяет, что при x &lt; y и cos(y) ≈ 0 (tan не определён)
        /// выбрасывается исключение <see cref="ArgumentException"/>.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void TestPage2_TanUndefined_ThrowsArgumentException()
        {
            MathFunctions.ComputePage2("exp", -1.0, Math.PI / 2.0);
        }

        // Тесты для MathFunctions.Page3  (формула страницы 3)

        /// <summary>
        /// Проверяет корректный результат при допустимых входных данных
        /// для функции страницы 3 (x=1, a=1, b=0).
        /// </summary>
        [TestMethod]
        public void TestPage3_ValidInput_ReturnsCorrectResult()
        {
            double x = 1.0, a = 1.0, b = 0.0;
            double result = MathFunctions.ComputePage3(x, a, b);
            double expected = 0.1 * a * Math.Pow(x, 3) * Math.Tan(a - b * x);
            Assert.AreEqual(expected, result, 1e-10);
        }

        /// <summary>
        /// Проверяет, что при cos(a − b·x) ≈ 0 (тангенс не определён)
        /// функция возвращает <see cref="double.NaN"/>.
        /// </summary>
        [TestMethod]
        public void TestPage3_TanUndefined_ReturnsNaN()
        {
            double a = Math.PI / 2.0, b = 0.0, x = 1.0;
            double result = MathFunctions.ComputePage3(x, a, b);
            Assert.IsTrue(double.IsNaN(result), "Ожидается NaN при cos(arg) ≈ 0");
        }

        /// <summary>
        /// Проверяет, что при x = 0 результат равен нулю (x³ = 0),
        /// независимо от значений a и b.
        /// </summary>
        [TestMethod]
        public void TestPage3_XIsZero_ReturnsZero()
        {
            double x = 0.0, a = 5.0, b = 2.0;
            double result = MathFunctions.ComputePage3(x, a, b);
            Assert.AreEqual(0.0, result, 1e-10,
                "При x=0 результат должен быть равен 0, так как x³=0");
        }
    }
}
