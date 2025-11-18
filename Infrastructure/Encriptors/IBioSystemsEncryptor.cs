namespace Infrastructure.Encriptors;

public interface IBioSystemsEncryptor
{
    /// <summary>
    /// Function used to encrypt a plain text
    /// </summary>
    /// <param name="plainText"></param>
    /// <returns></returns>
    string Encrypt ( string plainText );

    /// <summary>
    /// Function used to decrypt a cipher Text
    /// </summary>
    /// <param name="encryptedText"></param>
    /// <returns></returns>
    string Decrypt ( string encryptedText );

}