using System;
using System.Text;
using Android.Util;
using Java.Security;
using Javax.Crypto;
using Android.Content;
using Android.Widget;


namespace XamarinCryptoSpeed
{
	public abstract class SymmetricCipher
	{
		protected String cipherName { get; private set; }
		protected String keyName { get; private set; }
		protected int keySize { get; private set; }
		protected Cipher cipher { get; private set; }
		protected ISecretKey secretKey { get; private set; }
		public Boolean inEncryptionMode { get; protected set; }
		protected Context appContex { get; private set; }

		public SymmetricCipher(String parCipherName, String parKeyName, int parKeySize, Context parAppConntext)
		{
			cipherName = parCipherName;
			keyName = parKeyName;
			keySize = parKeySize;
			appContex = parAppConntext;

			try
			{
				cipher = Cipher.GetInstance(cipherName);
				secretKey = GenerateKey(parKeySize, keyName);
			}
			catch (NoSuchAlgorithmException ex) 
			{
				ex.PrintStackTrace ();
			}
			catch (NoSuchPaddingException ex)
			{
				ex.PrintStackTrace();
			}
		}

		public void startSymmetricCipherTest()
		{
			// b1 plain text, b2 cipher text, b3 plain text to compare
			byte[] b1 = new byte[Constants.SIZE];
			byte[] b2 = null;
			byte[] b3 = null;
			long startEncryption ,endEncryption, startDecryption, endDecryption;
			double[] encTime = new double[50];
			double[] decTime = new double[50];
			double sumEncryption, sumDecryption;
			sumDecryption = sumEncryption = 0;
			// buffer for log written into device sd card
			StringBuilder buffer = new StringBuilder();
			String cipherName = keyName + "-" + keySize;

			CommonAuxiliaryCode.GenerateDummyBytes(b1);
			Log.Info(Constants.TAG, cipherName + "test start");
			for(int i = 0; i < Constants.REPETITION; i++)
			{
				Log.Info(Constants.TAG , "Start attemp : " + i + " of " + cipher.Algorithm);
				this.InitEncryption();
				startEncryption = DateTime.Now.ToFileTime();
				b2 = this.Encrypt(b1);
				endEncryption = DateTime.Now.ToFileTime();

				this.InitDecryption();
				startDecryption = DateTime.Now.ToFileTime();
				b3 = this.Decrypt(b2);
				endDecryption = DateTime.Now.ToFileTime();

				if(CommonAuxiliaryCode.CmpByteArrayShowResult(b1, b3, cipherName + " attempt: " + i))
				{
					encTime[i] = (((double) endEncryption / 10000.0) - ((double) startEncryption)/ 10000.0);
					sumEncryption += encTime[i];
					decTime[i] =  (((double) endDecryption/ 10000.0) - ((double) startDecryption)/ 10000.0);
					sumDecryption += decTime[i];
					Log.Info(Constants.TAG, cipherName + " attempt : " + i + " ended successful time enc: " + encTime[i] + " dec : " + decTime[i]);
					buffer.Append(encTime[i] + "," + decTime[i] + "\n");
					b3[0] = 0;
				}
				else
				{   // shoudn't happen, skipp test
					Log.Error(Constants.TAG, cipherName + " plain text and plain text after enc/den differs !!!");
					return;
				}
			}
			double encR =  sumEncryption / ((double) Constants.REPETITION);
			double decR =  sumDecryption / ((double) Constants.REPETITION);
			buffer.Append("Test " + cipherName + " by provider: " + cipher.Provider + " ended succesfully");
			buffer.Append("\n Averange values: " + encR + "," + decR);
			Toast.MakeText(appContex, "ENC time: " + encR + " DEC time: " + decR, ToastLength.Short).Show();
			CommonAuxiliaryCode.WriteToFile(cipherName + "." + Constants.SIZE + "x" + Constants.REPETITION  +".txt", buffer.ToString());
		}

		public byte[] Encrypt(byte[] input)
		{
			if(!inEncryptionMode)
			{
				Toast.MakeText(appContex, "No in encryption mode ! encryption skipped", ToastLength.Short).Show();
			}
			byte[] output = null;
			try
			{
				output = cipher.DoFinal(input);
			} 
			catch(IllegalBlockSizeException ex)
			{
				ex.PrintStackTrace();
			}
			catch(BadPaddingException ex)
			{
				ex.PrintStackTrace();
			}
			return output;
		}

		public byte[] Decrypt(byte[] input)
		{
			if(inEncryptionMode)
			{
				Toast.MakeText(appContex, "No in decryption mode ! decryption skipped", ToastLength.Short).Show();
			}
			byte [] output = null;
			try
			{
				output = cipher.DoFinal(input);
			} 
			catch (IllegalBlockSizeException ex)
			{
				ex.PrintStackTrace();
			}
			catch(BadPaddingException ex)
			{
				ex.PrintStackTrace();
			}

			return output;
		}

		private static ISecretKey GenerateKey(int keyLength, String algorithm)
		{
			KeyGenerator keyGenerator = KeyGenerator.GetInstance(algorithm);
			SecureRandom secureRandom = new SecureRandom();
			keyGenerator.Init(keyLength, secureRandom);
			// difference against JAVA API, Xamarin can't cast from IKeySpec
			// to Secret key, must be casted through SecretKeyFactory
			// var secretKeyFactory = Javax.Crypto.SecretKeyFactory.GetInstance(algorithm);
			ISecretKey sc = keyGenerator.GenerateKey();
			return sc;
		}
			
		abstract public void InitEncryption();
		abstract public void InitDecryption();
	}
}
