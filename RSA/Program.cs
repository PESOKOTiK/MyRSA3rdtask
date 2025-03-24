using RSA;
using System.Numerics;
using System.Text;

Console.WriteLine("\n1. Pick random prime\n2. See all primes up to 1000 and select 2\n3. Type 2 prime numbers without listing them");
int choice = int.Parse(Console.ReadLine());

int p = 0, q = 0;
List<int> primes = MyRSA.GeneratePrimes(1000);

if (choice == 1)
{
    Random rand = new Random();
    p = primes[rand.Next(primes.Count)];
    q = primes[rand.Next(primes.Count)];
}
else if (choice == 2)
{
    Console.WriteLine(string.Join(", ", primes));
    Console.Write("Pick p: "); p = int.Parse(Console.ReadLine());
    Console.Write("Pick q: "); q = int.Parse(Console.ReadLine());
}
else if (choice == 3)
{
    Console.Write("Enter p: "); p = int.Parse(Console.ReadLine());
    Console.Write("Enter q: "); q = int.Parse(Console.ReadLine());
}
else
{
    Console.WriteLine("Invalid option."); return;
}

int n = p * q;
int phi = (p - 1) * (q - 1);
int e = MyRSA.FindE(phi);
int d = MyRSA.ModInverse(e, phi);

Console.Write("Enter message to encrypt (integer): ");
int x = int.Parse(Console.ReadLine());
BigInteger y = BigInteger.ModPow(x, e, n);
File.WriteAllText("ciphertext.txt", y + "," + n + "," + e);
Console.WriteLine("Encrypted and saved: " + y);

string[] data = File.ReadAllText("ciphertext.txt").Split(',');
y = BigInteger.Parse(data[0]);
n = int.Parse(data[1]);
e = int.Parse(data[2]);

(p, q) = MyRSA.FindPrimeFactors(n, primes);
phi = (p - 1) * (q - 1);
d = MyRSA.ModInverse(e, phi);
BigInteger decrypted = BigInteger.ModPow(y, d, n);
Console.WriteLine("Decrypted message: " + decrypted);