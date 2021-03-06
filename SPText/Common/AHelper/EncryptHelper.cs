﻿using SPTextCommon;
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
        public static byte[] Encrypt(byte[] byteArray, int offset, int len)
        {
            byte[] bytes = new byte[byteArray.Length];
            for (int i = 0; i < len; i++)
            {
                bytes[i] = (byte)(byteArray[i + offset] ^ 0xa7);
            }
            return bytes;
        }

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

	/// <summary>
	/// 加解密扩展方法
	/// </summary>
	public static class EncryptExtension
	{
		#region Base64
		/// <summary> 
		/// 将字符串使用base64算法编码 
		/// </summary>
		/// <param name="encodingName">编码类型（编码名称）</param>
		/// <param name="inputStr">待加密的字符串</param>
		/// <returns>加密后的字符串</returns> 
		public static string EncodeBase64String(this string inputStr, string encodingName = "UTF-8")
		{
			if (inputStr.IsNull())
				return null;

			var bytes = Encoding.GetEncoding(encodingName).GetBytes(inputStr);
			return Convert.ToBase64String(bytes);
		}

		/// <summary> 
		/// 将字符串使用base64算法解码
		/// </summary> 
		/// <param name="encodingName">编码类型</param> 
		/// <param name="base64String">已用base64算法加密的字符串</param> 
		/// <returns>解密后的字符串</returns> 
		public static string DecodeBase64String(this string base64String, string encodingName = "UTF-8")
		{
			if (base64String.IsNull())
				return null;

			var bytes = Convert.FromBase64String(base64String);
			return Encoding.GetEncoding(encodingName).GetString(bytes);
		}
		#endregion

		#region Md5
		/// <summary>
		/// 将字符串使用MD5算法加密
		/// </summary>
		/// <param name="inputStr"></param>
		/// <returns></returns>
		public static string EncodeMd5String(this string inputStr)
		{
			if (inputStr.IsNullOrEmpty())
				return inputStr;

			using (var md5 = MD5.Create())
			{
				var result = md5.ComputeHash(Encoding.Default.GetBytes(inputStr));
				return BitConverter.ToString(result).Replace("-", "");
			}
		}
		#endregion

		#region SHA

		/// <summary>
		/// 将字符串使用SHA1算法加密
		/// </summary>
		/// <param name="inputStr">明文</param>
		/// <returns></returns>
		public static string EncodeSha1String(this string inputStr)
		{
			if (inputStr.IsNull())
				return null;

			using (var sha1 = SHA1.Create())
			{
				var buffer = Encoding.UTF8.GetBytes(inputStr);
				var byteArr = sha1.ComputeHash(buffer);
				return BitConverter.ToString(byteArr).Replace("-", "").ToLower();
			}
		}

		/// <summary>
		/// 将字符串使用SHA256算法加密
		/// </summary>
		/// <param name="inputStr">明文</param>
		/// <returns></returns>
		public static string EncodeSha256String(this string inputStr)
		{
			if (inputStr.IsNull())
				return null;

			using (var sha256 = SHA256.Create())
			{
				var buffer = Encoding.UTF8.GetBytes(inputStr);
				var byteArr = sha256.ComputeHash(buffer);
				return BitConverter.ToString(byteArr).Replace("-", "").ToLower();
			}
		}

		/// <summary>
		/// 将字符串使用SHA384算法加密
		/// </summary>
		/// <param name="inputStr">明文</param>
		/// <returns></returns>
		public static string EncodeSha384String(this string inputStr)
		{
			if (inputStr.IsNull())
				return null;

			using (var sha384 = SHA384.Create())
			{
				var buffer = Encoding.UTF8.GetBytes(inputStr);
				var byteArr = sha384.ComputeHash(buffer);
				return BitConverter.ToString(byteArr).Replace("-", "").ToLower();
			}
		}

		/// <summary>
		/// 将字符串使用SHA512算法加密
		/// </summary>
		/// <param name="inputStr">明文</param>
		/// <returns></returns>
		public static string EncodeSha512String(this string inputStr)
		{
			if (inputStr.IsNull())
				return null;

			using (var sha512 = SHA512.Create())
			{
				var buffer = Encoding.UTF8.GetBytes(inputStr);
				var byteArr = sha512.ComputeHash(buffer);
				return BitConverter.ToString(byteArr).Replace("-", "").ToLower();
			}
		}
		#endregion
	}
}
