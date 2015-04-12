using System;
using Javax.Crypto.Spec;
using Java.Security;
using Android.Content;
using Javax.Crypto;

namespace XamarinCryptoSpeed
{
	public class SymmetricBlockCipher : SymmetricCipher
	{
		protected int ivLength { get; private set; }
		protected IvParameterSpec ivSpec { get; private set; }

		public SymmetricBlockCipher(String parCipherName, String parKeyName, int parKeySize, int parIvLength, Context parAppContex) 
		: base(parCipherName, parKeyName, parKeySize, parAppContex)
		{
			ivLength = parIvLength;
			ivSpec = new IvParameterSpec(SecureRandom.GetSeed(ivLength));	
		}

		public override void InitEncryption()
		{
			try 
			{
				// 1 encyption mode
				cipher.Init(CipherMode.EncryptMode, ((IKey) secretKey));
			} 
			catch (InvalidKeyException e) 
			{
				e.PrintStackTrace();
			} 
			catch (InvalidAlgorithmParameterException e) 
			{
				e.PrintStackTrace();
			}
			inEncryptionMode = true;
		}
		public override void InitDecryption()
		{
			try 
			{
				// 2 decyption mode
				cipher.Init(CipherMode.DecryptMode, ((IKey) this.secretKey));
			} 
			catch (InvalidKeyException e) 
			{
				e.PrintStackTrace();
			} 
			catch (InvalidAlgorithmParameterException e) 
			{
				e.PrintStackTrace();
			}
			inEncryptionMode = false;
		}
	}
}

