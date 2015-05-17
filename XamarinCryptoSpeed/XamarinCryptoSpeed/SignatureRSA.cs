using System;

using Java.Security;
using Java.Util;

using Android.Security;
using Android.Util;

using Android.App;
using Android.Content;

using Android.Widget;
using Android.Provider;
using Java.Math;


namespace XamarinCryptoSpeed
{
	public class SignatureRSA 
	{
		public static void generateRSAkey(String alias, int keySize, Context appContext)
		{
			Calendar start = Calendar.GetInstance(Java.Util.TimeZone.Default);
			Calendar end = Calendar.GetInstance(Java.Util.TimeZone.Default);
			end.Add(CalendarField.Year, 1);

			KeyPairGeneratorSpec.Builder builder = new KeyPairGeneratorSpec.Builder(appContext);
			builder.SetAlias(Constants.ALIAS);                        // set key alias
			try {
				builder.SetKeyType("RSA");                          // set key type
			} catch (NoSuchAlgorithmException e) {
				e.PrintStackTrace();
			}
			builder.SetKeySize(keySize);                        // set key size
			builder.SetSubject(new Javax.Security.Auth.X500.X500Principal("CN=test"));   // set subject used for self-signed certificate of the generated key pair
			builder.SetSerialNumber(BigInteger.One);            // set serial number used for the self-signed certificate of the generated key pair
			builder.SetStartDate(start.Time);              // set start of validity for the self-signed certificate of the generated key pair
			builder.SetEndDate(end.Time);                  // set end of validity used for the self-signed certificate of the generated key pair
			KeyPairGeneratorSpec spec = builder.Build();
			// next two rows can throws exception NoSuchAlgorithmException|NoSuchProviderException|InvalidAlgorithmParameterException
			KeyPairGenerator kpGenerator = null;
			try {
				kpGenerator = KeyPairGenerator.GetInstance("RSA", "AndroidKeyStore");
				kpGenerator.Initialize(spec);
			} 
			catch (NoSuchProviderException e) {
				e.PrintStackTrace();
			}
			catch(InvalidAlgorithmParameterException  e)
			{
				e.PrintStackTrace();
			}
			catch(NoSuchAlgorithmException e)
			{
				e.PrintStackTrace();
			}
			KeyPair kp = kpGenerator.GenerateKeyPair();
			//
			Toast.MakeText(appContext, "Key pair installed into app private keyStore successfully under alias: " + alias, ToastLength.Short).Show();
			Log.Info(Constants.TAG, "Public key and private key loaded with alias: " + Constants.ALIAS);
		}

		public static void test(String RSAName, int keySize, Context appContex)
		{
			Java.Security.IPublicKey publicKey = null;
			Java.Security.IPrivateKey privateKey = null;

			Java.Security.KeyStore.PrivateKeyEntry privateKeyEntry = CommonAuxiliaryCode.GetPrivateKeyEntry(Constants.ALIAS);
			if(null == privateKeyEntry)
			{
				generateRSAkey(Constants.ALIAS, keySize, appContex);
				privateKeyEntry = CommonAuxiliaryCode.GetPrivateKeyEntry(Constants.ALIAS);
			}
			// WTF .Certificate.PublicKey, .Private aren't documentent in API documentation
			if(null == privateKeyEntry)
			{   // key generation error
				Log.Error(Constants.TAG, "Error loading and generation key pair failed");
			} else 
			{
				publicKey = privateKeyEntry.Certificate.PublicKey;
				privateKey = privateKeyEntry.PrivateKey;
			}
				
			Java.Security.Signature rsa = null;

			try 
			{
				rsa = Java.Security.Signature.GetInstance(RSAName);
			} catch (NoSuchAlgorithmException e) {
				e.PrintStackTrace();
			}
			XamarinCryptoSpeed.Signature.TestSignature(rsa, publicKey, privateKey, keySize.ToString(), appContex);
		}
	}
}

