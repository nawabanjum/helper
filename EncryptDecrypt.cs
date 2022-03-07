using System;
using System.Configuration;
using System.IO;
using System.Security.Cryptography;
using System.Text;

public class EncryptDecrypt
{
    private static readonly byte[] IV =
      new byte[8] { 124, 113, 155, 129, 120, 176, 133, 159 };

    private static string Password = "Password";
    static string Salt = "Salt";
    static string HashAlgorithm = "MD5";
    static int PasswordIterations = 5;
    static string InitialVector = "InitialVector";
    static int KeySize = 256;

    public static string Encrypt(string encodedData)
    {

        byte[] InitialVectorBytes = Encoding.ASCII.GetBytes(InitialVector);
        byte[] SaltValueBytes = Encoding.ASCII.GetBytes(Salt);
        byte[] PlainTextBytes = Encoding.UTF8.GetBytes(encodedData);
        PasswordDeriveBytes DerivedPassword = new PasswordDeriveBytes(Password, SaltValueBytes, HashAlgorithm, PasswordIterations);
        byte[] KeyBytes = DerivedPassword.GetBytes(KeySize / 8);
        RijndaelManaged SymmetricKey = new RijndaelManaged();
        SymmetricKey.Mode = CipherMode.CBC;
        ICryptoTransform Encryptor = SymmetricKey.CreateEncryptor(KeyBytes, InitialVectorBytes);
        MemoryStream MemStream = new MemoryStream();
        CryptoStream cryptoStream = new CryptoStream(MemStream, Encryptor, CryptoStreamMode.Write);
        cryptoStream.Write(PlainTextBytes, 0, PlainTextBytes.Length);
        cryptoStream.FlushFinalBlock();
        byte[] CipherTextBytes = MemStream.ToArray();
        MemStream.Close();
        cryptoStream.Close();
        MemStream.Dispose();
        cryptoStream.Dispose();
        Encryptor.Dispose();
        string varEncode = Convert.ToBase64String(CipherTextBytes);
        varEncode = varEncode.Replace('+', '@').Replace("/","$$$");
        return varEncode;

    }

    public static string Decrypt(string dataToEncode)
    {
        dataToEncode = dataToEncode.Replace('@', '+').Replace("$$$", "/");

        byte[] InitialVectorBytes = Encoding.ASCII.GetBytes(InitialVector);
        byte[] SaltValueBytes = Encoding.ASCII.GetBytes(Salt);
        byte[] CipherTextBytes = Convert.FromBase64String(dataToEncode);
        PasswordDeriveBytes DerivedPassword = new PasswordDeriveBytes(Password, SaltValueBytes, HashAlgorithm, PasswordIterations);
        byte[] KeyBytes = DerivedPassword.GetBytes(KeySize / 8);
        RijndaelManaged SymmetricKey = new RijndaelManaged();
        SymmetricKey.Mode = CipherMode.CBC;
        ICryptoTransform Decryptor = SymmetricKey.CreateDecryptor(KeyBytes, InitialVectorBytes);
        MemoryStream MemStream = new MemoryStream(CipherTextBytes);
        CryptoStream cryptoStream = new CryptoStream(MemStream, Decryptor, CryptoStreamMode.Read);
        byte[] PlainTextBytes = new byte[CipherTextBytes.Length];
        int ByteCount = cryptoStream.Read(PlainTextBytes, 0, PlainTextBytes.Length);
        MemStream.Close();
        cryptoStream.Close();
        MemStream.Dispose();
        cryptoStream.Dispose();
        Decryptor.Dispose();
        return Encoding.UTF8.GetString(PlainTextBytes, 0, ByteCount);

    }
}
