using System;
using Java.Security;
using Android.Content;
using Android.Util;
using System.Text;
using Android.Widget;

namespace XamarinCryptoSpeed
{
	public class Signature {
		// override standard common constant
		static readonly int SIG_SIZE = 1024000;
		static readonly int SIG_REPETITION = 100;

		// if elliptic curves are used keysize == ""
		// else is size of RSA key
		public static void TestSignature(Java.Security.Signature sign, IPublicKey publicKey, IPrivateKey privateKey, String keySize, Context appContext)
		{
			byte[] b1 =  new byte[SIG_SIZE];
			byte[] b2 = null;
			long startSign ,endSign, startVerify, endVerify;
			double[] signTime = new double[SIG_REPETITION];
			double[] verifyTime = new double[SIG_REPETITION];
			double sumSign = 0;
			double sumVerify = 0;
			StringBuilder buffer = new StringBuilder();
			CommonAuxiliaryCode.GenerateDummyBytes(b1);

			for(int i =0; i < SIG_REPETITION; i++)
			{
				try
				{
					sign.InitSign(privateKey);
					startSign = DateTime.Now.ToFileTime();
					sign.Update(b1);
					b2 = sign.Sign();
					endSign = DateTime.Now.ToFileTime();

					Boolean success;
					sign.InitVerify(publicKey);
					startVerify = DateTime.Now.ToFileTime();
					sign.Update(b1);
					success = sign.Verify(b2);
					endVerify = DateTime.Now.ToFileTime();
					if (success)
					{
						signTime[i] = (((double) endSign / 10000.0) - ((double) startSign)/ 10000.0);
						sumSign += signTime[i];
						verifyTime[i] =  (((double) endVerify / 10000.0) - ((double) startVerify)/ 10000.0);
						sumVerify += verifyTime[i];
						Log.Info(Constants.TAG, sign.Algorithm + " attempt : " + i + " ended successful time sign: " + signTime[i] + " verify : " + verifyTime[i]);
						buffer.Append(signTime[i] + "," + verifyTime[i] + "\n");
					}
					else
					{   // shoudn't happen, skipp test
						Log.Error(Constants.TAG, sign.Algorithm + " plain text and plain text after enc/den differs !!!");
						return;
					}
				}  
				catch (SignatureException e)
				{
					e.PrintStackTrace();
				}
				catch(InvalidKeyException e)
				{
					e.PrintStackTrace();
				}
			}
			double encR =  sumSign / ((double) SIG_REPETITION);
			double decR =  sumVerify / ((double) SIG_REPETITION);
			buffer.Append("Test " + sign.Algorithm + " by provider: " + sign.Provider + " ended succesfully");
			buffer.Append("\n Averange values: " + encR + "," + decR);
			Toast.MakeText(appContext, "SIGN time: " + encR + " VERIFY time: " + decR, ToastLength.Short ).Show();
			CommonAuxiliaryCode.WriteToFile(sign.Algorithm + "." + keySize + "." + SIG_SIZE + "x" + SIG_REPETITION  +".txt", buffer.ToString());
		}
	}
}

