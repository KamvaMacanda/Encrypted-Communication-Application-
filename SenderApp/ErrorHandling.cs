using System;
using System.Collections.Generic;
using System.Text;

namespace SenderApp
{
    public class ErrorHandling
    {

        public  static string CheckValidInput(string Input )
        {
            while (true)
            {
                if (string.IsNullOrWhiteSpace(Input))
                {
                    Console.WriteLine("Message cant be empty, try again  ");
                    return null;

                }
            ;
                return Input;
            }
        }
    }
}
