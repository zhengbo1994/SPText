using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPText.Common.AHelper
{
    public static class StringActions
    {
        public static float ToFloat(object p_value)
        {
            return ToFloat(p_value, 0);
        }
        public static float ToFloat(object p_value, float p_def_val)
        {
            float r_v = p_def_val;
            if (p_value == null)
            {
                r_v = p_def_val;
            }
            else if (!float.TryParse(p_value.ToString(), out r_v))
            {
                r_v = p_def_val;
            }

            return r_v;
        }

        public static int ToInt(object p_value)
        {
            return ToInt(p_value, 0);
        }
        public static int ToInt(object p_value, int p_def_val)
        {
            int r_v = p_def_val;
            if (p_value == null)
            {
                r_v = p_def_val;
            }
            else if (!int.TryParse(p_value.ToString(), out r_v))
            {
                r_v = p_def_val;
            }

            return r_v;
        }

        public static bool ToBool(object p_value)
        {
            return ToBool(p_value, false);
        }
        public static bool ToBool(object p_value, bool p_def_val)
        {
            bool r_v = p_def_val;
            if (p_value == null)
            {
                r_v = p_def_val;
            }
            else if (!bool.TryParse(p_value.ToString(), out r_v))
            {
                r_v = p_def_val;
            }

            return r_v;
        }

        public static string ToString(object p_value)
        {
            return p_value.ToString();
        }
    }
}
