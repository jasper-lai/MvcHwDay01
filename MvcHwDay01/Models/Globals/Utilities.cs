using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MvcHwDay01.Models
{
    public static class Utilities
    {
        public static void MakeBillingHistoryData()
        {
            //初始化靜態類別 BillingHistory
            if (null == BillingHistory.Data)
            {
                BillingHistory.Data = new Dictionary<string, IEnumerable<BillingItemViewModel>>()
                {
                    {"Jasper", new List<BillingItemViewModel>()
                        {
                            new BillingItemViewModel() { BillType=1, Aoumnt=100, BillDate=new DateTime(2016,04,09), Memo="支出100元" },
                            new BillingItemViewModel() { BillType=2, Aoumnt=200, BillDate=new DateTime(2016,04,10), Memo="收入200元" },
                        }
                    },
                    {"Judy", new List<BillingItemViewModel>()
                        {
                            new BillingItemViewModel() { BillType=2, Aoumnt=200, BillDate=new DateTime(2016,04,09), Memo="收入200元" },
                            new BillingItemViewModel() { BillType=1, Aoumnt=100, BillDate=new DateTime(2016,04,10), Memo="支出100元" },
                        }
                    }
                };
            }
        }

        public static IEnumerable<BillingItemViewModel> GetBillingDataByUser(string userName)
        {
            if (BillingHistory.Data.ContainsKey(userName))
            {
                return BillingHistory.Data[userName];
            }
            else
            {
                return new List<BillingItemViewModel>();
            }
        }

        public static void AddBillingDataByUser(string userName, BillingItemViewModel item)
        {
            if (BillingHistory.Data.ContainsKey(userName))
            {
                (BillingHistory.Data[userName] as List<BillingItemViewModel>).Add(item);
            }
            else
            {
                BillingHistory.Data.Add(userName, new List<BillingItemViewModel>() { item });
            }
        }

        //public static IEnumerable<BillingItemViewModel> AddBillingDataByUser(string userName, BillingItemViewModel item)
        //{
        //    if (BillingHistory.Data.ContainsKey(userName))
        //    {
        //        //GetBillingDataByUser(userName).ToList().Add(item);
        //        (BillingHistory.Data[userName] as List<BillingItemViewModel>).Add(item);
        //    }
        //    else
        //    {
        //        BillingHistory.Data.Add(userName, new List<BillingItemViewModel>() { item });
        //    }
        //    return GetBillingDataByUser(userName);
        //}

    }
}