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
		protected SecretKey secretKey { get; private set; }
		protected Boolean inEncryptionMode { get; private set; }
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
					encTime[i] = (((double) endEncryption / 1000000.0) - ((double) startEncryption)/ 1000000.0);
					sumEncryption += encTime[i];
					decTime[i] =  (((double) endDecryption/ 1000000.0) - ((double) startDecryption)/ 1000000.0);
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

		private static SecretKey GenerateKey(int keyLength, String algorithm)
		{
			KeyGenerator keyGenerator = KeyGenerator.GetInstance(algorithm);
			SecureRandom secureRandom = new SecureRandom();
			keyGenerator.Init(keyLength, secureRandom);
			return (SecretKey) keyGenerator.GenerateKey();
		}



		abstract public void InitEncryption();
		abstract public void InitDecryption();
	}
}

/*
 	public class SymmetricCipher
	{
		private const int SIZE = 10;
		private const String TAG = "lubo.XamarinCryptoSpeed";
		private const int REPETITION = 50;

		public SymmetricCipher ()
		{
			int a = 5;
			int b = a + 2;
		}

		public void StartTestSymmetricCipher()
		{
			byte[] b1 = new byte[SIZE];
			byte[] b2 = null;
			byte[] b3 = null;
			long startEncryption ,endEncryption, startDecryption, endDecryption;
			double[] encTime = new double[50];
			double[] decTime = new double[50];
			double sumEncryption, sumDecryption;
			sumDecryption = sumEncryption = 0;
			// buffer for log written into device sd card
			var buffer = new StringBuilder();
			//String cipherName = this.getKeyName() + "-" + this.getKeySize();
			String cipherName = "AES-128";

			this.generateDummyBytes(b1);
			Log.Info(TAG, cipherName + "test start");
			for(int i = 0; i < REPETITION; i++)
			{
				this.initEncryption();
				startEncryption = System.nanoTime();
				b2 = this.encrypt(b1);
				endEncryption = System.nanoTime();

				this.initDecryption();
				startDecryption = System.nanoTime();
				b3 = this.decrypt(b2);
				endDecryption = System.nanoTime();

				if(this.cmpByteArrayShowResult(b1, b3, cipherName + " attempt: " + i))
				{
					encTime[i] = (((double) endEncryption / 1000000.0) - ((double) startEncryption)/ 1000000.0);
					sumEncryption += encTime[i];
					decTime[i] =  (((double) endDecryption/ 1000000.0) - ((double) startDecryption)/ 1000000.0);
					sumDecryption += decTime[i];
					Log.i(TAG, cipherName + " attempt : " + i + " ended successful time enc: " + encTime[i] + " dec : " + decTime[i]);
					buffer.append(encTime[i] + "," + decTime[i] + "\n");
					b3[0] = 0;
				}
				else
				{   // shoudn't happen, skipp test
					Log.e(TAG, cipherName + " plain text and plain text after enc/den differs !!!");
					return;
				}
			}
			double encR =  sumEncryption / ((double) REPETITION);
			double decR =  sumDecryption / ((double) REPETITION);
			buffer.append("Test " + cipherName + " by provider: " + this.getCipher().getProvider() + " ended succesfully");
			buffer.append("\n Averange values: " + encR + "," + decR);
			Toast.makeText(appContex, "ENC time: " + encR + " DEC time: " + decR, Toast.LENGTH_SHORT).show();
			this.writeToFile(cipherName + "." + CommonAuxiliaryCode.SIZE + "x" + CommonAuxiliaryCode.REPETITION  +".txt", buffer.toString();
		}

		public void generateDummyBytes(byte[] array)
		{
			Random random = new Random();
			random.nextBytes(array);
		}

		public boolean cmpByteArrayShowResult(byte[] array1, byte[] array2, String comparedAlg)
		{
			if (Arrays.equals(array1, array2))
			{
				//Toast.makeText(getApplicationContext(), "Test " + comparedAlg +   " succeed", Toast.LENGTH_SHORT).show();
				return true;
			} else
			{
				//Toast.makeText(getApplicationContext(), "Test " + comparedAlg + " failed", Toast.LENGTH_SHORT).show();
				return false;
			}
		}

		public void initEncryption()
		{
			try {
				 init(Cipher.ENCRYPT_MODE, secretKey, ivSpec);
			}
				catch (InvalidKeyException e) {
				e.printStackTrace();
			}
				catch (InvalidAlgorithmParameterException e) {
				e.printStackTrace();
			}
				inEncryptionMode = true;
		}


		public void initDecryption()
		{
			try 
			{
				cipher.init(Cipher.DECRYPT_MODE, secretKey, ivSpec);
			}
			catch (InvalidKeyException e) 
			{
				e.printStackTrace();
			}
			catch (InvalidAlgorithmParameterException e) 
			{
				e.printStackTrace();
			}
				inEncryptionMode = false;
		}

		public byte[] encrypt(byte[] input)
		{
			if(!inEncryptionMode)
			{
				Toast.makeText(appContex, "No in encryption mode ! encryption skipped", Toast.LENGTH_SHORT).show();
			}
			byte[] output = null;
			try
			{
				output = cipher.doFinal(input);
			} 
					catch (IllegalBlockSizeException |Javax.Crypto.BadPaddingException ex)
			{
				ex.printStackTrace();
			}
			return output;				
		}

		public byte[] decrypt(byte[] input)
		{
			if(inEncryptionMode)
			{
				Toast.makeText(appContex, "No in decryption mode ! decryption skipped", Toast.LENGTH_SHORT).show();
			}
			byte [] output = null;
			try	
			{
				output = cipher.doFinal(input);
				} 
			catch (IllegalBlockSizeException|BadPaddingException ex)
			{
				ex.printStackTrace();
			}
			return output;	
		}

		private static SecretKey generateKey(int keyLength, String algorithm)
		{
			KeyGenerator keyGenerator = KeyGenerator.getInstance(algorithm);
			SecureRandom secureRandom = new SecureRandom();
			keyGenerator.init(keyLength, secureRandom);
			return keyGenerator.generateKey();
		}

		public static void generateDummyBytes(byte[] array)
		{					
			Random random = new Random();
			random.nextBytes(array);
		}
	}
 * */