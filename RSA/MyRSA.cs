using System.Numerics;
using System.Text;

namespace RSA
{
    public class MyRSA
    {
        public static List<int> GeneratePrimes(int limit)
        {
            var primes = new List<int>();
            for (int i = 2; i <= limit; i++)
                if (!primes.Any(p => i % p == 0)) primes.Add(i);
            return primes;
        }

        public static int FindE(int phi)
        {
            for (int e = 2; e < phi; e++)
                if (GCD(e, phi) == 1) return e;
            return 3;
        }

        public static int GCD(int a, int b) => b == 0 ? a : GCD(b, a % b);

        public static int ModInverse(int e, int phi)
        {
            int t = 0, newT = 1, r = phi, newR = e;
            while (newR != 0)
            {
                int quotient = r / newR;
                (t, newT) = (newT, t - quotient * newT);
                (r, newR) = (newR, r - quotient * newR);
            }
            return t < 0 ? t + phi : t;
        }

        public static (int, int) FindPrimeFactors(int n, List<int> primes)
        {
            foreach (int prime in primes)
            {
                if (n % prime == 0 && primes.Contains(n / prime))
                    return (prime, n / prime);
            }
            throw new Exception("Prime factors not found.");
        }

        public static BigInteger StringToBigInteger(string message)
        {
            byte[] byteArray = Encoding.UTF8.GetBytes(message);
            return new BigInteger(byteArray);
        }

        public static string BigIntegerToString(BigInteger bigInt)
        {
            byte[] byteArray = bigInt.ToByteArray();
            return Encoding.UTF8.GetString(byteArray);
        }
    }
}