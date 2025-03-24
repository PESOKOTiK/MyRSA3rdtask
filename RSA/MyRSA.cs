using System.Numerics;
using System.Text;

namespace RSA
{
    public static class Constants
    {
        public static int e = 0x10001; // Common public exponent value for RSA.
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
        // Generate a key pair for RSA encryption
        public static KeyPair GenerateKeyPair(int p, int q)
        {
            // Calculate n = p * q
            BigInteger n = p * q;

            // Compute phi(n) = (p - 1) * (q - 1)
            BigInteger phi_n = (p - 1) * (q - 1);

            // Compute d such that (d * e) % phi(n) = 1 (modular inverse of e)
            BigInteger d = ModularInverse(Constants.e, phi_n);

            // Public key (e, n)
            Key publicKey = new Key(n, KeyType.PUBLIC);

            // Private key (d, n)
            Key privateKey = new Key(n, KeyType.PRIVATE, d);

            return new KeyPair(publicKey, privateKey);
        }

        // Encrypt message using RSA
        public static string Encrypt(string plaintext, Key publicKey)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(plaintext);
            BigInteger m = new BigInteger(bytes);
            BigInteger c = BigInteger.ModPow(m, publicKey.e, publicKey.n);
            return Convert.ToBase64String(c.ToByteArray());
        }

        // Decrypt message using RSA
        public static string Decrypt(string ciphertext, Key privateKey)
        {
            byte[] bytes = Convert.FromBase64String(ciphertext);
            BigInteger c = new BigInteger(bytes);
            BigInteger m = BigInteger.ModPow(c, privateKey.d, privateKey.n);
            byte[] decryptedBytes = m.ToByteArray();
            return Encoding.UTF8.GetString(decryptedBytes).TrimEnd('\0');
        }

        // Euclidean algorithm to compute gcd(a, b)
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

        // Extended Euclidean algorithm to compute modular inverse
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

        // Function to generate random prime numbers up to 1000
        public static int GetRandomPrime()
        {
            Random rand = new Random();
            var primes = GetPrimesUpTo1000();
            return primes[rand.Next(primes.Length)];
        }

        // Function to get all prime numbers up to 1000
        public static int[] GetPrimesUpTo1000()
        {
            return Enumerable.Range(2, 999).Where(x => IsPrime(x)).ToArray();
        }

        // Function to check if a number is prime
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