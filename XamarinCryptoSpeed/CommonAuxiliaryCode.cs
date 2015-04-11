﻿using System;
using System.Linq;
using Java.Security;
using Android.Util;
using Java.IO;
using Java.Security.Cert;
using Javax.Crypto;

namespace XamarinCryptoSpeed
{
	public class CommonAuxiliaryCode
	{
		// generate dummy bytes
		public static void GenerateDummyBytes(byte[] array)
		{
			Random random = new Random();
			random.NextBytes (array);
		}

		public static Boolean CmpByteArrayShowResult(byte[] array1, byte[] array2, String comparedAlg)
		{
			return Enumerable.SequenceEqual(array1, array2);
		}

		public static void WriteToFile(String filename, String data) 
		{
			try 
			{
				File outputFile = new File("/sdcard/" + filename);
				FileWriter fWriter;
				fWriter = new FileWriter(outputFile, true);
				fWriter.Write(data);
				fWriter.Flush();
				fWriter.Close();
			}
			catch (IOException e) 
			{
				Log.Error("Exception", "File write failed: " + e.ToString());
			}
		}

		public static KeyStore.PrivateKeyEntry GetPrivateKeyEntry(String alias)  
		{
			try
			{
				KeyStore ks = KeyStore.GetInstance("AndroidKeyStore");
				ks.Load(null);
				return (KeyStore.PrivateKeyEntry) ks.GetEntry(alias, null);
			}
			catch(KeyStoreException e)
			{
				Log.Error ("Exception", "File write failed: " + e);
			}
			catch(IOException e)
			{
				Log.Error ("Exception", "File write failed: " + e);
			}
			catch(CertificateException e)
			{
				Log.Error ("Exception", "File write failed: " + e);
			}
			catch(NoSuchAlgorithmException e)
			{
				Log.Error ("Exception", "File write failed: " + e);
			}
			catch(UnrecoverableEntryException e)
			{
				Log.Error ("Exception", "File write failed: " + e);
			}
			return null;
		}
			
	}
}
