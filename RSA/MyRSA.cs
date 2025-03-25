using System.Numerics;
using System.Text;

namespace RSA
{
    public static class Constants
    {
        public static int e = 0x10001;
    }

    public enum KeyType
    {
        PUBLIC,
        PRIVATE
    }

    public class Key
    {
        public BigInteger n { get; set; }
        public int e = Constants.e;
        public BigInteger d;
        public KeyType type { get; set; }

        public Key(BigInteger n_, KeyType type_, BigInteger d_ = default(BigInteger))
        {
            n = n_;
            type = type_;
            d = d_;
        }
    }

    public class KeyPair
    {
        public Key PublicKey { get; set; }
        public Key PrivateKey { get; set; }

        public KeyPair(Key publicKey, Key privateKey)
        {
            PublicKey = publicKey;
            PrivateKey = privateKey;
        }
    }

    public static class MyRSA
    {
        public static KeyPair GenerateKeyPair(int p, int q)
        {
            BigInteger n = p * q;

            BigInteger phi_n = (p - 1) * (q - 1);

            BigInteger d = ModularInverse(Constants.e, phi_n);

            Key publicKey = new Key(n, KeyType.PUBLIC);

            Key privateKey = new Key(n, KeyType.PRIVATE, d);

            return new KeyPair(publicKey, privateKey);
        }

        public static string Encrypt(string plaintext, Key publicKey)
        {
            StringBuilder encryptedText = new StringBuilder();

            foreach (char c in plaintext)
            {
                BigInteger m = new BigInteger(c);
                BigInteger cEncrypted = BigInteger.ModPow(m, publicKey.e, publicKey.n);
                encryptedText.Append(cEncrypted + " ");
            }

            return encryptedText.ToString().Trim();
        }

        public static string Decrypt(string ciphertext, Key privateKey)
        {
            StringBuilder decryptedText = new StringBuilder();
            string[] encryptedChars = ciphertext.Split(' ');

            foreach (string encryptedChar in encryptedChars)
            {
                if (BigInteger.TryParse(encryptedChar, out BigInteger c))
                {
                    BigInteger m = BigInteger.ModPow(c, privateKey.d, privateKey.n);
                    decryptedText.Append((char)(int)m);
                }
            }

            return decryptedText.ToString();
        }


        public static BigInteger GCD(BigInteger a, BigInteger b)
        {
            while (b != 0)
            {
                BigInteger temp = b;
                b = a % b;
                a = temp;
            }
            return a;
        }

        public static BigInteger ModularInverse(BigInteger e, BigInteger phi_n)
        {
            BigInteger t = 0;
            BigInteger newT = 1;
            BigInteger r = phi_n;
            BigInteger newR = e;

            while (newR != 0)
            {
                BigInteger quotient = r / newR;

                BigInteger tempT = t;
                t = newT;
                newT = tempT - quotient * newT;

                BigInteger tempR = r;
                r = newR;
                newR = tempR - quotient * newR;
            }

            if (r > 1)
                throw new Exception("e and phi(n) are not coprime");

            if (t < 0)
                t = t + phi_n;

            return t;
        }
        public static int GetRandomPrime()
        {
            Random rand = new Random();
            var primes = GetPrimesUpTo1000();
            return primes[rand.Next(primes.Length)];
        }

        public static int[] GetPrimesUpTo1000()
        {
            return Enumerable.Range(2, 999).Where(x => IsPrime(x)).ToArray();
        }

        public static bool IsPrime(int number)
        {
            if (number <= 1) return false;
            for (int i = 2; i <= Math.Sqrt(number); i++)
            {
                if (number % i == 0) return false;
            }
            return true;
        }
    }

}