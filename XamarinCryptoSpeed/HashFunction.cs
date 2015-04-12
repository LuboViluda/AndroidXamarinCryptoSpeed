using System;
using System.Text;
using Java.Security;
using Android.Util;

namespace XamarinCryptoSpeed
{
	public class HashFunction
	{
		public static void testHash(String algorithmName, int testFileSize, int repetions)
		{
			byte[] b1 = new byte[testFileSize];
			byte[] b2 = null;
			CommonAuxiliaryCode.GenerateDummyBytes(b1);

			long startHash ,endHash;
			double[] hashTime = new double[repetions];
			double hashSum = 0;
			StringBuilder buffer = new StringBuilder();
			MessageDigest md = null;

			Log.Info(Constants.TAG, algorithmName + " test start with file size: " + testFileSize + " bytes x times: " + repetions);
			for(int i = 0; i < repetions; i++)
			{

				try 
				{
					md = MessageDigest.GetInstance(algorithmName);
				} catch (NoSuchAlgorithmException e) 
				{
					e.PrintStackTrace();
				}
				startHash = DateTime.Now.ToFileTime();
				md.Update(b1);
				b2 = md.Digest();
				endHash = DateTime.Now.ToFileTime();
				if(b2 != null)
				{
					hashTime[i] = (((double) endHash / 10000.0) - ((double) startHash) / 10000.0);
					hashSum += hashTime[i];
					Log.Info(Constants.TAG, algorithmName + " attempt : " + i + " ended successful time : " + hashTime[i]);
					buffer.Append(hashTime[i]);
					buffer.Append (","); 	//csv
					b2 = null;
				}
			}
			double hashAvr = hashSum / ((double) repetions);
			Log.Info(Constants.TAG, "Test " + algorithmName + " finished, avrg. time : " + hashAvr);
			buffer.Append("\n" + "Test " + algorithmName + " by provider: " + md.Provider + "with file size: " +  + testFileSize + " bytes x times: " + repetions);
			buffer.Append(" ended succesfully\n Averange values: " + hashAvr);
			CommonAuxiliaryCode.WriteToFile(algorithmName + "." + testFileSize + "x" + repetions + ".txt", buffer.ToString());
		}
	}
}

