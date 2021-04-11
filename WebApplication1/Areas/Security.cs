using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebApplication1.Models;

namespace WebApplication1.Areas
{
    public class Security
    {

        public static  string SecurityValidaionAddUpdate(Product Data)
        {
            string Valid = "True";
            if (string.IsNullOrEmpty(Data.Name))
            {
                Valid = "False";
            }
            else if (string.IsNullOrEmpty(Data.Description))
            {
                Valid = "False";
            }
            else if (Data.Price<=0)
            {
                Valid = "False";
            }
            if (!string.IsNullOrEmpty(Data.Action))
            {
                if (Data.Action.ToLower() == "update")
                {
                    if (Data.ItemId <= 0)
                    {
                        Valid = "False";
                    }
                }
            }
            else
            {
                Valid = "False";
            }
                return Valid;
        }
       
        public static string SecurityValidaionGetDelete(Product Data)
        {
            string Valid = "True";
            if (Data.ItemId <= 0)
            {
                Valid = "False";
            }
            
            return Valid;
        }
    }
}