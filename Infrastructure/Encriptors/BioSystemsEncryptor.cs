using System.Security.Cryptography;
using System.Text;

namespace Infrastructure.Encriptors;

public class BioSystemsEncryptor : IBioSystemsEncryptor
{

    #region "Declarations"

    //Variables required to generate the Symmetric Key needed to encrypt/decrypt Users' Passwords 
    private const int MaxValue = 8;
    private readonly byte[ ] _keyArray;
    private readonly byte[ ] _initVectorArray;
    private const string CipherTextConst = "cipherText";
    private const string PlainTextConst = "plainText";
    private const string KeyConst = "Key";
    private const string IVConst = "IV";

    #endregion

    #region "Ctor"

    /// <summary>
    /// Initializes a new instance of the <see cref="BioSystemsEncryptor"/> class.
    /// </summary>
    public BioSystemsEncryptor ( )
    {
        //Initialization of variables required to generate the Symmetric Key needed to 
        //encrypt/decrypt Users' Passwords
        string passPhrase = "AX00UserSoftware";
        string saltValue = "AX00UserSoftware";
        string initVector = "@8A7b6C5d4E3f2G1";
        int keySize = 128;

        Encoding encoding = Activator.CreateInstance<ASCIIEncoding>( );

        Rfc2898DeriveBytes key = new( passPhrase, encoding.GetBytes( saltValue ) );
        _keyArray = key.GetBytes( keySize / MaxValue );
        _initVectorArray = encoding.GetBytes( initVector );

    }
    #endregion

    #region "Interface"

    /// <summary>
    /// Function used to decrypt a cipher Text
    /// </summary>
    /// <param name="encryptedText"></param>
    /// <returns></returns>
    public string Decrypt ( string encryptedText )
    {
        string plainText = string.Empty;
        try
        {
            if ( encryptedText != string.Empty )
            {
                plainText = DecryptStringFromBytesAes( encryptedText );
            }

            return plainText;
        }
        catch ( Exception e )
        {
            Console.WriteLine( e );
            throw;
        }
    }

    /// <summary>
    /// Function used to encrypt a plain text
    /// </summary>
    /// <param name="plainText"></param>
    /// <returns></returns>
    public string Encrypt ( string plainText )
    {
        string encryptedText = string.Empty;
        try
        {
            if ( plainText != string.Empty )
            {
                encryptedText = EncryptStringToBytesAes( plainText );
            }

            return encryptedText;
        }
        catch ( Exception e )
        {
            Console.WriteLine( e );
            throw;
        }
    }
    #endregion

    #region "Private methods"

    /// <summary>
    /// Encrypts the string to bytes aes.
    /// </summary>
    /// <param name="plainText">The plain text.</param>
    /// <returns></returns>
    private string EncryptStringToBytesAes ( string plainText )
    {
        // Check arguments.
        if ( plainText == null || plainText.Length <= 0 )
            throw new ArgumentNullException( PlainTextConst );
        if ( _keyArray == null || _keyArray.Length <= 0 )
            throw new ArgumentNullException( KeyConst );
        if ( _initVectorArray == null || _initVectorArray.Length <= 0 )
            throw new ArgumentNullException( IVConst );
        byte[ ] encrypted;

        // Create an Aes object
        // with the specified key and IV.
        using ( Aes aesAlg = Aes.Create( ) )
        {
            aesAlg.Key = _keyArray;
            aesAlg.IV = _initVectorArray;

            // Create an encryptor to perform the stream transform.
            ICryptoTransform encryptor = aesAlg.CreateEncryptor( aesAlg.Key, aesAlg.IV );

            // Create the streams used for encryption.
            using ( MemoryStream msEncrypt = new( ) )
            {
                using ( CryptoStream csEncrypt = new( msEncrypt, encryptor, CryptoStreamMode.Write ) )
                {
                    using ( StreamWriter swEncrypt = new( csEncrypt ) )
                    {
                        //Write all data to the stream.
                        swEncrypt.Write( plainText );
                    }
                    encrypted = msEncrypt.ToArray( );
                }
            }
        }

        // Return the encrypted bytes from the memory stream.
        return Convert.ToBase64String( encrypted );
    }

    /// <summary>
    /// Decrypts the string from bytes aes.
    /// </summary>
    /// <param name="cipherText">The cipher text.</param>
    /// <returns></returns>
    private string DecryptStringFromBytesAes ( string cipherText )
    {
        // Check arguments.
        if ( cipherText is not { Length: > 0 } )
            throw new ArgumentNullException( CipherTextConst );
        if ( _keyArray is not { Length: > 0 } )
            throw new ArgumentNullException( KeyConst );
        if ( _initVectorArray is not { Length: > 0 } )
            throw new ArgumentNullException( IVConst );

        // Declare the string used to hold
        // the decrypted text.
        string plaintext;

        // Create an Aes object
        // with the specified key and IV.
        using Aes aesAlg = Aes.Create( );
        aesAlg.Key = _keyArray;
        aesAlg.IV = _initVectorArray;

        // Create a decryptor to perform the stream transform.
        ICryptoTransform decryptor = aesAlg.CreateDecryptor( aesAlg.Key, aesAlg.IV );

        // Create the streams used for decryption.
        using ( MemoryStream msDecrypt = new( Convert.FromBase64String( cipherText ) ) )
        {
            using ( CryptoStream csDecrypt = new( msDecrypt, decryptor, CryptoStreamMode.Read ) )
            {
                using ( StreamReader srDecrypt = new( csDecrypt ) )
                {

                    // Read the decrypted bytes from the decrypting stream
                    // and place them in a string.
                    plaintext = srDecrypt.ReadToEnd( );
                }
            }
        }

        return plaintext;
    }
    #endregion


}