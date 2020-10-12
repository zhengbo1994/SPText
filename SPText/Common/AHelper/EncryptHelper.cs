using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Web.Security;

namespace System
{
	/// <summary>
	/// 加解密帮助类
	/// </summary>
	public class EncryptHelper
	{
		private static string encryptKey = "Oyea";    //定义密钥  

		//private static string encryptKey =  ConfigurationHelper.GetAppSettingStr("EncryptKey");    //定义密钥  

		private static int encryptKey2 = 0xa7;

		/// <summary> 
		/// 加密字符串   
		/// </summary>  
		/// <param name="str">要加密的字符串</param>  
		/// <returns>加密后的字符串</returns>  
		public static string Encrypt(string str)
		{
			DESCryptoServiceProvider descsp = new DESCryptoServiceProvider();   //实例化加/解密类对象   

			byte[] key = Encoding.Unicode.GetBytes(encryptKey); //定义字节数组，用来存储密钥    

			byte[] data = Encoding.Unicode.GetBytes(str);//定义字节数组，用来存储要加密的字符串  

			MemoryStream MStream = new MemoryStream(); //实例化内存流对象      

			//使用内存流实例化加密流对象   
			CryptoStream CStream = new CryptoStream(MStream, descsp.CreateEncryptor(key, key), CryptoStreamMode.Write);

			CStream.Write(data, 0, data.Length);  //向加密流中写入数据      

			CStream.FlushFinalBlock();              //释放加密流      

			return Convert.ToBase64String(MStream.ToArray());//返回加密后的字符串  
		}

		/// <summary>  
		/// 解密字符串   
		/// </summary>  
		/// <param name="str">要解密的字符串</param>  
		/// <returns>解密后的字符串</returns>  
		public static string Decrypt(string str, string from)
		{
			DESCryptoServiceProvider descsp = new DESCryptoServiceProvider();   //实例化加/解密类对象    

			byte[] key = Encoding.Unicode.GetBytes(encryptKey); //定义字节数组，用来存储密钥    

			byte[] data = Convert.FromBase64String(str);//定义字节数组，用来存储要解密的字符串  

			MemoryStream MStream = new MemoryStream(); //实例化内存流对象      

			//使用内存流实例化解密流对象       
			CryptoStream CStream = new CryptoStream(MStream, descsp.CreateDecryptor(key, key), CryptoStreamMode.Write);

			CStream.Write(data, 0, data.Length);       //向解密流中写入数据     

			CStream.FlushFinalBlock();               //释放解密流      

			return Encoding.Unicode.GetString(MStream.ToArray());       //返回解密后的字符串  
		}

		//加密
		//public static byte[] Encrypt(byte[] byteArray, int offset, int len)
		//{
		//	byte[] bytes = new byte[byteArray.Length];
		//	for (int i = 0; i < len; i++)
		//	{
		//		bytes[i] = (byte)(byteArray[i + offset] ^ 0xa7);
		//	}
		//	return bytes;
		//}

		/// <summary>
		/// 加密
		/// </summary>
		/// <param name="byteArray"></param>
		/// <returns></returns>
		public static byte[] Encrypt(byte[] byteArray)
		{
			byte[] bytes = new byte[byteArray.Length];
			for (int i = 0; i < byteArray.Length; i++)
			{
				bytes[i] = (byte)(byteArray[i] ^ encryptKey2);
			}
			return bytes;
		}
		//解密
		//public static void Decrypt(byte[] byteArray, int offset, int len)
		//{
		//	for (int i = 0; i < len; i++)
		//		byteArray[i + offset] ^= 0xa7;

		//}

		/// <summary>
		/// 解密
		/// </summary>
		/// <param name="byteArray"></param>
		/// <returns></returns>
		public static byte[] Decrypt(byte[] byteArray)
		{
			byte[] bytes = new byte[byteArray.Length];
			for (int i = 0; i < byteArray.Length; i++)
			{
				bytes[i] = (byte)(byteArray[i] ^ encryptKey2);
			}
			return bytes;
		}

		/// <summary>
		/// DES数据加密
		/// </summary>
		/// <param name="targetValue">目标值</param>
		/// <param name="key">密钥</param>
		/// <returns>加密值</returns>
		public static string Encrypt1(string targetValue, string key)
		{
			if (string.IsNullOrEmpty(targetValue))
			{
				return string.Empty;
			}

			var returnValue = new StringBuilder();
			var des = new DESCryptoServiceProvider();
			byte[] inputByteArray = Encoding.Default.GetBytes(targetValue);
			// 通过两次哈希密码设置对称算法的初始化向量   
			des.Key = Encoding.ASCII.GetBytes(FormsAuthentication.HashPasswordForStoringInConfigFile
													(FormsAuthentication.HashPasswordForStoringInConfigFile(key, "md5").
														Substring(0, 8), "sha1").Substring(0, 8));
			// 通过两次哈希密码设置算法的机密密钥   
			des.IV = Encoding.ASCII.GetBytes(FormsAuthentication.HashPasswordForStoringInConfigFile
													(FormsAuthentication.HashPasswordForStoringInConfigFile(key, "md5")
														.Substring(0, 8), "md5").Substring(0, 8));
			var ms = new MemoryStream();
			var cs = new CryptoStream(ms, des.CreateEncryptor(), CryptoStreamMode.Write);
			cs.Write(inputByteArray, 0, inputByteArray.Length);
			cs.FlushFinalBlock();
			foreach (byte b in ms.ToArray())
			{
				returnValue.AppendFormat("{0:X2}", b);
			}
			return returnValue.ToString();
		}

		/// <summary>
		/// DES数据解密
		/// </summary>
		/// <param name="targetValue"></param>
		/// <param name="key"></param>
		/// <returns></returns>
		public static string Decrypt1(string targetValue, string key)
		{
			if (string.IsNullOrEmpty(targetValue))
			{
				return string.Empty;
			}
			// 定义DES加密对象
			var des = new DESCryptoServiceProvider();
			int len = targetValue.Length / 2;
			var inputByteArray = new byte[len];
			int x, i;
			for (x = 0; x < len; x++)
			{
				i = Convert.ToInt32(targetValue.Substring(x * 2, 2), 16);
				inputByteArray[x] = (byte)i;
			}
			// 通过两次哈希密码设置对称算法的初始化向量   
			des.Key = Encoding.ASCII.GetBytes(FormsAuthentication.HashPasswordForStoringInConfigFile
													(FormsAuthentication.HashPasswordForStoringInConfigFile(key, "md5").
														Substring(0, 8), "sha1").Substring(0, 8));
			// 通过两次哈希密码设置算法的机密密钥   
			des.IV = Encoding.ASCII.GetBytes(FormsAuthentication.HashPasswordForStoringInConfigFile
													(FormsAuthentication.HashPasswordForStoringInConfigFile(key, "md5")
														.Substring(0, 8), "md5").Substring(0, 8));
			// 定义内存流
			var ms = new MemoryStream();
			// 定义加密流
			var cs = new CryptoStream(ms, des.CreateDecryptor(), CryptoStreamMode.Write);
			cs.Write(inputByteArray, 0, inputByteArray.Length);
			cs.FlushFinalBlock();
			return Encoding.Default.GetString(ms.ToArray());
		}
	}
}
