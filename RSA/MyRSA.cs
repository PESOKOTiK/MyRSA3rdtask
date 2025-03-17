

namespace RSA
{
    public class MyRSA
    {
        public static bool IsPrime(int n)
        {
            if (n <= 1) return false;
            for (int i = 2; i <= Math.Sqrt(n); i++)
            {
                if (n % i == 0) return false;
            }
            return true;
        }

        public static int GCD(int a, int b)
        {
            while (b != 0)
            {
                int temp = b;
                b = a % b;
                a = temp;
            }
            return a;
        }

        public static int ModInverse(int a, int m)
        {
            int m0 = m, x0 = 0, x1 = 1;
            if (m == 1) return 0;

            while (a > 1)
            {
                int q = a / m;
                int t = m;

                m = a % m;
                a = t;
                t = x0;

                x0 = x1 - q * x0;
                x1 = t;
            }

            if (x1 < 0) x1 += m0;

            return x1;
        }

        public static (int n, int e, int d) GenerateKeys(int p, int q)
        {
            int n = p * q;
            int phi = (p - 1) * (q - 1);

            int e = 2;
            while (GCD(e, phi) != 1)
            {
                e++;
            }

            int d = ModInverse(e, phi);

            return (n, e, d);
        }

        public static List<int> Encrypt(string plaintext, int n, int e)
        {
            List<int> ciphertext = new List<int>();

            foreach (char c in plaintext)
            {
                int m = (int)c;  // Convert the character to its ASCII integer representation
                int cipherChar = (int)Math.Pow(m, e) % n; // RSA encryption step
                ciphertext.Add(cipherChar);
            }

            return ciphertext;
        }

        public static string Decrypt(List<int> ciphertext, int n, int d)
        {
            string decryptedText = string.Empty;

            foreach (int cipherChar in ciphertext)
            {
                // Decrypt the ciphertext using RSA formula: m = (ciphertext^d) % n
                int m = (int)Math.Pow(cipherChar, d) % n;

                // Ensure the result is positive (modular arithmetic fix)
                m = (m + n) % n;

                // Convert the decrypted number back to a character
                // Only add valid ASCII characters (range: 0-127 for basic ASCII)
                if (m >= 0 && m <= 127)
                {
                    decryptedText += (char)m; // Convert back to character
                }
                else
                {
                    decryptedText += "?"; // If it's not a valid ASCII value, add a "?"
                }
            }

            return decryptedText;
        }

        public static List<int> FindPrimes(int max)
        {
            List<int> primes = new List<int>();
            for (int i = 2; i <= max; i++)
            {
                if (IsPrime(i))
                {
                    primes.Add(i);
                }
            }
            return primes;
        }
    }
}
