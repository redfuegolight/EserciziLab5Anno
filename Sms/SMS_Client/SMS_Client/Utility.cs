using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMS_Client
{
    public static class Utility
    {
        public static string NoZero(this string _input)
        {
            string appo = "";
            int i = 0;
            while (_input[i] != '\0')
            {
                appo += _input[i];
                i++;
            }
            return appo;
        }
    }
}
