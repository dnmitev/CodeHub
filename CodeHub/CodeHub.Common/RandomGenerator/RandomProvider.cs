namespace CodeHub.Common.RandomGenerator
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using CodeHub.Common.RandomGenerator.Contracts;

    public sealed class RandomProvider : IRandomProvider
    {
        private static Random randomGenerator;
        private static IRandomProvider randomProvider;

        private const int DefaultMinimaLStringLength = 3;
        private const int DefaultMaximalStringLenght = 10;
        private const string CharacterSet = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz ";
        private const string DigitsSet = "0123456789";

        private RandomProvider()
        {
            randomGenerator = new Random();
        }

        public static IRandomProvider Instance
        {
            get
            {
                if (randomProvider == null)
                {
                    randomProvider = new RandomProvider();
                }

                return randomProvider;
            }
        }

        /// <summary>
        /// Provides random integer value in given or default range
        /// </summary>
        /// <param name="minValue">the min value of the range</param>
        /// <param name="maxValue">the max value of the range, inclusive</param>
        /// <returns>random integer number in the given or default range</returns>
        public int GetRandomInt(int minValue = 0, int maxValue = int.MaxValue)
        {
            int number = randomGenerator.Next(minValue, maxValue + 1);

            return number;
        }

        /// <summary>
        /// Provides random double value in given or default range
        /// </summary>
        /// <param name="minValue">the min value of the range</param>></param>
        /// <param name="maxValue">the max value of the range, inclusive</param>></param>
        /// <returns>random double number in the given or default range</returns>
        public double GetRandomDouble(double minValue = 0.00, double maxValue = double.MaxValue)
        {
            var randomDoubleValue = randomGenerator.NextDouble();
            var generatedNumber = minValue + (randomDoubleValue * (maxValue - minValue));

            return generatedNumber;
        }

        /// <summary>
        /// Provides a randomly generated string of letters with given of default lenght
        /// </summary>
        /// <param name="length">the length of the retrieved string or default</param>
        /// <returns>string with given or default length</returns>
        public string GetRandomString(int length = DefaultMaximalStringLenght)
        {
            char[] result = new char[length];

            for (int i = 0; i < length; i++)
            {
                result[i] = CharacterSet[randomGenerator.Next(CharacterSet.Length)];
            }

            return new string(result);
        }

        /// <summary>
        /// Provides randomly generated string with random length
        /// </summary>
        /// <returns>randomly generated string with random length</returns>
        public string GetRandomLengthString(int min = DefaultMinimaLStringLength, int max = DefaultMaximalStringLenght)
        {
            int randomLength = this.GetRandomInt(min, max);
            string randomGeneratedString = this.GetRandomString(randomLength);

            return randomGeneratedString;
        }

        /// <summary>
        /// Provides a randomly generated string consisting just digits
        /// </summary>
        /// <param name="length">the length of the retrieved string</param>
        /// <returns>string that contains only digits</returns>
        public string GetRandomStringOfNumbers(int length = DefaultMaximalStringLenght)
        {
            char[] digits = new char[length];

            for (int i = 0; i < length; i++)
            {
                digits[i] = DigitsSet[randomGenerator.Next(DigitsSet.Length)];
            }

            return new string(digits);
        }

        /// <summary>
        /// Provides a randomly generated string consisting of just digits with random length
        /// </summary>
        /// <returns>string that contains only digits with random length</returns>
        public string GetRandomLengthStringOfNumbers()
        {
            int randomLength = this.GetRandomInt(DefaultMinimaLStringLength, DefaultMaximalStringLenght);
            string randomGeneratedString = this.GetRandomStringOfNumbers(randomLength);

            return randomGeneratedString;
        }

        /// <summary>
        /// Provides a collection of unique randomly generated strings with random lengths
        /// </summary>
        /// <param name="listLength">the total count of unique elements to retrieve</param>
        /// <returns>HashSet of strings</returns>
        public ISet<string> GetUniqueRandomStringsSet(int listLength, int minLegth, int maxLength)
        {
            var generatedStrings = new HashSet<string>();

            while (generatedStrings.Count < listLength)
            {
                generatedStrings.Add(this.GetRandomLengthString(minLegth,maxLength));
            }

            return generatedStrings;
        }

        /// <summary>
        /// Provides a collection of unique randomly generated integers
        /// </summary>
        /// <param name="listLength">the total count of unique elements to retrieve</param>
        /// <returns>HashSet of integers</returns>
        public ISet<int> GetUniqueRandomIntegersSet(int listLength, int minValue = 0, int maxValue = int.MaxValue)
        {
            var generatedIntegers = new HashSet<int>();

            while (generatedIntegers.Count < listLength)
            {
                int currentInteger = this.GetRandomInt(minValue, maxValue);
                generatedIntegers.Add(currentInteger);
            }

            return generatedIntegers;
        }

        /// <summary>
        /// Provides randomly generated Date object
        /// </summary>
        /// <param name="minimalYear">The least year in the period</param>
        /// <returns>DateTime object in the range from the minimalYear to today</returns>
        public DateTime GetRandomDate(int minimalYear = 1990)
        {
            DateTime startDate = new DateTime(minimalYear, 1, 1);
            int range = (DateTime.Today - startDate).Days;
            var generatedDate = startDate.AddDays(randomGenerator.Next(range));

            return generatedDate;
        }

        public DateTime GetRandomDate(DateTime startData)
        {
            int range = (DateTime.Today - startData).Days;
            var generatedDate = startData.AddDays(randomGenerator.Next(range));

            return generatedDate;
        }

        /// <summary>
        /// Provides randomly generated price in specific range
        /// </summary>
        /// <param name="minPrice">lower bound of the range</param>
        /// <param name="maxPrice">upper bound of the range</param>
        /// <returns>decimal as a price object</returns>
        public decimal GetRandomPrice(double minPrice, double maxPrice)
        {
            return (decimal)this.GetRandomDouble(minPrice, maxPrice);
        }

        /// <summary>
        /// Provides random int representing percent value
        /// </summary>
        /// <returns>random percent</returns>
        public int GetRandomPercent()
        {
            return this.GetRandomInt(0, 100);
        }
    }
}