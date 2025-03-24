using RSA;

while (true)
{
    Console.WriteLine("\nRSA Menu:");
    Console.WriteLine("1. Pick random prime");
    Console.WriteLine("2. See all primes up to 1000 and select 2");
    Console.WriteLine("3. Type 2 prime numbers without listing them");
    Console.WriteLine("0. Exit");
    Console.Write("Choose an option: ");
    string choice = Console.ReadLine();

    switch (choice)
    {
        case "1":
            // Option 1: Pick random prime
            int prime1 = MyRSA.GetRandomPrime();
            int prime2 = MyRSA.GetRandomPrime();
            Console.WriteLine($"Random Primes Selected: p = {prime1}, q = {prime2}");
            GenerateAndDisplayKeys(prime1, prime2);
            break;

        case "2":
            // Option 2: See all primes up to 1000 and select 2
            var primesUpTo1000 = MyRSA.GetPrimesUpTo1000();
            Console.WriteLine("Prime numbers up to 1000:");
            for (int i = 0; i < primesUpTo1000.Length; i++)
            {
                Console.WriteLine($"{i + 1}. {primesUpTo1000[i]}");
            }
            Console.Write("Select two primes by entering their numbers separated by space: ");
            string[] selectedIndices = Console.ReadLine().Split(' ');
            int p2 = primesUpTo1000[int.Parse(selectedIndices[0]) - 1];
            int q2 = primesUpTo1000[int.Parse(selectedIndices[1]) - 1];
            Console.WriteLine($"You selected: p = {p2}, q = {q2}");
            GenerateAndDisplayKeys(p2, q2);
            break;

        case "3":
            // Option 3: Type 2 prime numbers directly
            Console.Write("Enter first prime number: ");
            int p3 = int.Parse(Console.ReadLine());
            Console.Write("Enter second prime number: ");
            int q3 = int.Parse(Console.ReadLine());
            Console.WriteLine($"You entered: p = {p3}, q = {q3}");
            GenerateAndDisplayKeys(p3, q3);
            break;

        case "0":
            // Exit the program
            Console.WriteLine("Exiting program...");
            return;

        default:
            Console.WriteLine("Invalid option, please try again.");
            break;
    }
}

static void GenerateAndDisplayKeys(int p, int q)
{
    // Generate the key pair using p and q
    KeyPair keyPair = MyRSA.GenerateKeyPair(p, q);

    // Print public and private keys
    Console.WriteLine($"Public Key: (e={keyPair.PublicKey.e}, n={keyPair.PublicKey.n})");
    Console.WriteLine($"Private Key: (d={keyPair.PrivateKey.d}, n={keyPair.PrivateKey.n})");

    // Input plaintext message to encrypt
    Console.Write("Enter message to encrypt: ");
    string message = Console.ReadLine();

    // Encrypt the message
    string encryptedMessage = MyRSA.Encrypt(message, keyPair.PublicKey);
    Console.WriteLine($"Encrypted Message: {encryptedMessage}");

    // Save the encrypted message and public key to a file
    File.WriteAllText("ciphertext.txt", encryptedMessage);
    File.WriteAllText("publicKey.txt", $"{keyPair.PublicKey.e},{keyPair.PublicKey.n}");

    // Read the encrypted message from the file
    string ciphertext = File.ReadAllText("ciphertext.txt");

    // Decrypt the message
    string decryptedMessage = MyRSA.Decrypt(ciphertext, keyPair.PrivateKey);
    Console.WriteLine($"Decrypted Message: {decryptedMessage}");
}