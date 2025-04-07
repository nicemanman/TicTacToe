using System.Text;
using Server.Services.Interfaces;

namespace Server.Services;

public class JoinCodeService : IJoinCodeService
{
	public string GetJoinCode()
	{
		var arr = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz!?/:;,.-";
		StringBuilder builder = new();
		Random random = new();
		
		for (int i = 0; i < 10; i++)
		{
			var ch = arr[random.Next(arr.Length)];
			builder.Append(ch);
		}

		return builder.ToString();
	}
}