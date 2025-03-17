// See https://aka.ms/new-console-template for more information
using RSA;

List<int> primes = MyRSA.FindPrimes(1000);
Console.WriteLine("\n1.Pick random prime\n2.See all primes up to 1000 and select 2\n3.Type 2 prime numbers without listing them\n");
int choise, p, q;
try {
    choise = Convert.ToInt32(Console.ReadLine());
}
catch
{
    Console.WriteLine("Not a number");
    return;
}
Start:
switch (choise)
{
    case 1:
        Random random = new Random();

        p = primes[random.Next(primes.Count)];
        q = primes[random.Next(primes.Count)];
        Console.WriteLine($"p = {p} and q = {q}");
        break;
    case 2:
        foreach (int i in primes)
        {
            int j = 0;
            Console.Write($"{i}   ");
            j++;
            if (j == 4)
            {
                j = 0;
                Console.Write("\n");
            }
        }
        Console.WriteLine("Enter first prime p\n");
        p = Convert.ToInt32(Console.ReadLine());
        Console.WriteLine("Enter second prime q\n");
        q = Convert.ToInt32(Console.ReadLine());
        if (!MyRSA.IsPrime(p) || !MyRSA.IsPrime(q))
        {
            Console.WriteLine("Not a prime");
            goto Start;
        }

        break;
    case 3:
        Console.WriteLine("Enter first prime p\n");
        p = Convert.ToInt32(Console.ReadLine());
        Console.WriteLine("Enter second prime q\n");
        q = Convert.ToInt32(Console.ReadLine());
        if (!MyRSA.IsPrime(p) || !MyRSA.IsPrime(q))
        {
            Console.WriteLine("Not a prime");
            goto Start;
        }
        break;
    default:
        Console.WriteLine("Wrong choice!\n");
        goto Start;
        break;
}
Console.WriteLine("Enter plaintext:\n");
string x = Console.ReadLine();


var (n, e, d) = MyRSA.GenerateKeys(p,q);
Console.WriteLine($"Public key is n={n}, e = {e}");
Console.WriteLine($"Private key is n={n}, d = {d}");

List<int> ciphertext = MyRSA.Encrypt(x, n, e);
Console.WriteLine($"Ciphertext: {string.Join(", ", ciphertext)}");

File.WriteAllText("ciphertext.txt", $"{string.Join(", ", ciphertext)}\n{n},{e}");
Console.WriteLine("Ciphertext and public key saved to 'ciphertext.txt'.");

string[] fileData = File.ReadAllLines("ciphertext.txt");
string[] cipherTextStr = fileData[0].Split(new string[] { ", " }, StringSplitOptions.None);
List<int> readCiphertext = new List<int>();
foreach (string val in cipherTextStr)
{
    readCiphertext.Add(int.Parse(val));
}

string[] publicKey = fileData[1].Split(',');
int readN = int.Parse(publicKey[0]);
int readE = int.Parse(publicKey[1]);

string decryptedText = MyRSA.Decrypt(readCiphertext, readN, d);
Console.WriteLine($"Decrypted Text: {decryptedText}");
File.WriteAllText("decrypted.txt", decryptedText);

