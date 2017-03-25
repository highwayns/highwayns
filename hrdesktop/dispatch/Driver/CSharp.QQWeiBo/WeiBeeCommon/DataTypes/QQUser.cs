using System;
using System.Collections.Generic;

namespace WeiBeeCommon.DataTypes
{
    public class QQUser
    {
        public string CityCode;
        public string CountryCode;
        public int Fansnum;
        public string Head;
        public int Idolnum;
        public bool IsIdol;
        public bool IsVip;
        public string Location;
        public string Name;
        public string Nick;
        public string ProvinceCode;
        public List<string> Tag;
        public QQTweet tweet;
        //To-do: Add ToString() function for test and debug
    }
    public class QQTweet
    {
        public string From;
        public string Id;
        public string Text;
        public DateTime timestamp;
        // To do: Add ToString() function for test and debug
    }
}
