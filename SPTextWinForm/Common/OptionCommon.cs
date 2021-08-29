using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPTextWinForm.Common
{
	public static class OptionCommon
	{
		public static string GetPrismR(double prism1_r, int prism_axis1_r, double prism2_r, int prism_axis2_r)
		{
			string text = string.Empty;
			if (prism1_r != 0.0)
			{
				double num = Math.Cos((double)prism_axis1_r * Math.PI / 180.0) * prism1_r;
				string str = (num > 0.0) ? "I" : "O";
				double value = Math.Abs(num);
				string text2 = Convert.ToDouble(Math.Round(value, 2, MidpointRounding.AwayFromZero)).ToString("0.00");
				if (text2 != "0.00")
				{
					text = $"{text}{str + text2}";
				}
			}
			if (prism2_r != 0.0)
			{
				double num2 = Math.Sin((double)prism_axis2_r * Math.PI / 180.0) * prism2_r;
				string str2 = (num2 > 0.0) ? "U" : "D";
				double value2 = Math.Abs(num2);
				string text3 = Convert.ToDouble(Math.Round(value2, 2, MidpointRounding.AwayFromZero)).ToString("0.00");
				if (text3 != "0.00")
				{
					text = $"{text}{str2 + text3}";
				}
			}
			return text;
		}

		public static string GetPrismL(double prism1_l, int prism_axis1_l, double prism2_l, int prism_axis2_l)
		{
			string text = string.Empty;
			if (prism1_l != 0.0)
			{
				double num = Math.Cos((double)prism_axis1_l * Math.PI / 180.0) * prism1_l;
				string str = (num > 0.0) ? "O" : "I";
				double value = Math.Abs(num);
				string text2 = Convert.ToDouble(Math.Round(value, 2, MidpointRounding.AwayFromZero)).ToString("0.00");
				if (text2 != "0.00")
				{
					text = $"{text}{str + text2}";
				}
			}
			if (prism2_l != 0.0)
			{
				double num2 = Math.Sin((double)prism_axis2_l * Math.PI / 180.0) * prism2_l;
				string str2 = (num2 > 0.0) ? "U" : "D";
				double value2 = Math.Abs(num2);
				string text3 = Convert.ToDouble(Math.Round(value2, 2, MidpointRounding.AwayFromZero)).ToString("0.00");
				if (text3 != "0.00")
				{
					text = $"{text}{str2 + text3}";
				}
			}
			return text;
		}
	}
}
