using System;

using Android.Content;
using Java.Security.Spec;
using Java.Security;

namespace XamarinCryptoSpeed
{
	// no used yet
	// problems with starter licencion and EC
	public class SignatureEllipticCurves
	{
		public static void testEC(String ecName, Context appContex)
		{
			ECGenParameterSpec ecGenSpec = new ECGenParameterSpec(ecName); 
			KeyPairGenerator kpg = null;
			try {
				kpg = KeyPairGenerator.GetInstance("ECDSA");
				kpg.Initialize(ecGenSpec, new SecureRandom());
			} 
			catch (NoSuchAlgorithmException e) 
			{
				e.PrintStackTrace();
			}
			catch(InvalidAlgorithmParameterException e)
			{
				e.PrintStackTrace();
			}
			KeyPair pair = kpg.GenerateKeyPair();

			IPrivateKey privateKey = pair.Private;
			IPublicKey publicKey = pair.Public;

			Java.Security.Signature ec = null;
			try 
			{
				ec = Java.Security.Signature.GetInstance("ECDSA");

			} catch (NoSuchAlgorithmException e) 
			{
				e.PrintStackTrace();
			}
			XamarinCryptoSpeed.Signature.TestSignature(ec, publicKey, privateKey, ecName, appContex);
		}
	}
}

