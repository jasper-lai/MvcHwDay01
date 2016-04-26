using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MvcHwDay01.Models.Extensions
{
    public static class StringExtension
    {
        /// <summary>
        /// 取子字串
        /// </summary>
        /// <param name="str"></param>
        /// <param name="startIndex">起始index</param>
        /// <param name="maxLength">欲取得的最大字串長度</param>
        /// <param name="postStr">原字串長度 大於 欲取得的最大字串長度 時, 應附加的結尾字串, 預設為 ...</param>
        /// <returns></returns>
        public static string SubstringEx(this String str, int startIndex, int maxLength, string postStr = "...")
        {
            string result = String.Empty;

            //如果傳入的是 null, 或空字串, 就回傳空字串
            //[註]: 原有的 Substring 會發生例外 (並未將物件參考設定為物件的執行個體。), 所以需要這個判斷
            if (String.IsNullOrEmpty(str))
            {
                return result;
            }

            //如果傳入的起始位置大於 (字串長度 - 1), 就回傳空字串
            //[註]: 原有的 Substring 會發生例外 (startIndex 不可以大於字串的長度), 所以需要這個判斷
            if (startIndex > str.Length -1 )
            {
                return result;
            }

            int endIndex = startIndex + maxLength - 1;
            //如果要取的子字串, 超過原有字串的內容, 就取到最後一個字元
            if (endIndex > str.Length - 1)
            {
                //abcdefg
                //0123456
                //--> Substring(5, 3)
                //--> endIndex = 5 + 3 - 1 = 7, 此時, 傳入參數計算的結尾index > 字串結束的 index
                //--> 原本是 Substring(5,3), 會改成 Substring(5,2)
                result = str.Substring(startIndex, str.Length - startIndex);
            }
            else
            {
                //abcdefg
                //0123456
                //--> Substring(5, 2)
                //--> endIndex = 5 + 2 - 1 = 6, 此時, 傳入參數計算的結尾index == 字串結束的 index
                result = str.Substring(startIndex, maxLength);

                //如果原始字串, 在截取資料後, 仍有殘餘字串, 則加上 postStr
                //abcdefg
                //0123456
                //--> Substring(3, 2)
                //--> 預期: "de" + "..."
                if (startIndex + maxLength < str.Length )
                {
                    result = result + postStr;
                }
            }

            return result;
        }
    }
}