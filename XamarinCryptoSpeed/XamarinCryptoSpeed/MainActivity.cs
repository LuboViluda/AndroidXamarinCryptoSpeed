using System;

using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;

namespace XamarinCryptoSpeed
{
	[Activity (Label = "XamarinCryptoSpeed", MainLauncher = true, Icon = "@drawable/icon")]
	public class MainActivity : Activity
	{

		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);
			// Set our view from the "main" layout resource
			SetContentView (Resource.Layout.Main);
			// Get our button from the layout resource,
			// and attach an event to it
			Button button = FindViewById<Button>(Resource.Id.AES128Button);
			button.Click += delegate {
				var AES128 = new SymmetricBlockCipher("AES/CBC/NoPadding", "AES", 128, 16, Application.Context);
				AES128.startSymmetricCipherTest();
			};

			button = FindViewById<Button>(Resource.Id.AES256Button);
			button.Click += delegate {
				var AES128 = new SymmetricBlockCipher("AES/CBC/NoPadding", "AES", 256, 16, Application.Context);
				AES128.startSymmetricCipherTest();
			};

			button = FindViewById<Button>(Resource.Id.AES256Button);
			button.Click += delegate {
				var AES128 = new SymmetricBlockCipher("AES/CBC/NoPadding", "AES", 256, 16, Application.Context);
				AES128.startSymmetricCipherTest();
			};

			button = FindViewById<Button>(Resource.Id.ARC4Button);
			button.Click += delegate {
				var RC4 = new SymmetricStreamCipher("ARC4", "ARC4", 128, Application.Context);
				RC4.startSymmetricCipherTest();
			};

			button = FindViewById<Button>(Resource.Id.MD5Button);
			button.Click += delegate {
				HashFunction.testHash("MD5", Constants.SIZE, Constants.REPETITION);
			};

			button = FindViewById<Button>(Resource.Id.SHA1Button);
			button.Click += delegate {
				HashFunction.testHash("SHA-1", 20, Constants.REPETITION);
			};

			button = FindViewById<Button>(Resource.Id.SHA1withRSA1024Button);
			button.Click += delegate {
				SignatureRSA.test("SHA1withRSA", 1024, Application.Context);
			};

			button = FindViewById<Button>(Resource.Id.SHA1withRSA2048Button);
			button.Click += delegate {
				SignatureRSA.test("SHA1withRSA", 2048, Application.Context);
			};

			button = FindViewById<Button>(Resource.Id.TripleDESButton);
			button.Click += delegate {
				var td = new SymmetricBlockCipher("DESede/CBC/NoPadding", "DESede", 168, 8, Application.Context);
				td.startSymmetricCipherTest();
			};

			button = FindViewById<Button>(Resource.Id.ECtestButton);
			button.Click += delegate {
				// do nothing, no added support library for elliptic curves :{
				// KEyPairGenerator throws NoSuchAlgorithmException in getInstanceMethod("ECDSA");
				// SignatureEllipticCurves.testEC("prime256v1", Application.Context);
			};
		}
			
	}


}


