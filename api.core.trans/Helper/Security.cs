using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace api.core.trans.Helper
{
	public class Security
	{
		public string HashSHA1Decryption(string value)
		{
			var shaSHA1 = System.Security.Cryptography.SHA1.Create();
			var inputEncodingBytes = Encoding.ASCII.GetBytes(value);
			var hashString = shaSHA1.ComputeHash(inputEncodingBytes);

			var stringBuilder = new StringBuilder();
			for (var x = 0; x < hashString.Length; x++)
			{
				stringBuilder.Append(hashString[x].ToString("X2"));
			}
			return stringBuilder.ToString();
		}
	}
}
