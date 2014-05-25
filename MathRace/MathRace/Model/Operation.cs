namespace MathRace.Model
{
    using System;

    public class Operation
    {
        private static readonly Random Random = new Random();

        private Operation()
        {
        }

        public string Quest { get; private set; }

        public int Solution { get; private set; }

        public static Operation CreateNew()
        {
            var left = Random.Next(0, 21);
            var right = Random.Next(0, 21);
            var op = Random.Next(0, 2);

            if (op == 0)
            {
                return new Operation
                           {
                               Quest = string.Format("{0}{1}{2}", left, " + ", right),
                               Solution = left + right
                           };
            }
            else
            {
                return new Operation
                {
                    Quest = string.Format("{0}{1}{2}", left, " - ", right),
                    Solution = left - right
                };
            }
        }
    }
}