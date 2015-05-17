using System;
using Javax.Crypto;
using Android.Content;
using Java.Security;

namespace XamarinCryptoSpeed
{
	public class SymmetricStreamCipher : SymmetricCipher
	{
		public SymmetricStreamCipher(String parCipherName, String parKeyName, int parKeySize, Context parAppContex)
			: base(parCipherName, parKeyName, parKeySize, parAppContex)
		{
		}

		public override void InitEncryption()
		{
			try {
				cipher.Init(CipherMode.EncryptMode, secretKey);
			} catch (InvalidKeyException e) {
				e.PrintStackTrace();
			}
			inEncryptionMode = true;
		}

		public override void InitDecryption()
		{
			try {
				cipher.Init(CipherMode.DecryptMode, secretKey);
			} catch (InvalidKeyException e) {
				e.PrintStackTrace();
			}
			inEncryptionMode = false;
		}
	}
}

