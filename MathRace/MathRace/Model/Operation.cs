using System;

namespace MathRace.Model
{
    public class Operation
    {
        private static Random random = new Random();

        private Operation()
        {
        }

        public string Quest { get; private set; }
        public int Solution { get; private set; }

        public static Operation Create()
        {
            var left = random.Next(0, 21);
            var right = random.Next(0, 21);
            var op = random.Next(0, 2);

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