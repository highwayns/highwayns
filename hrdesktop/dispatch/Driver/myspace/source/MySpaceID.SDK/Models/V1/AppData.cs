using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MySpaceID.SDK.Models.V1
{
    /// <summary>
    /// Application Data for a particular application.
    /// </summary>
    public class AppData
    {
        /// <summary>
        /// The Collection of application data pairs.
        /// </summary>
        public AppDataPair[] KeyValueCollection { get; set; }
    }

    /// <summary>
    /// A key-value pair representing Application Data.
    /// </summary>
    public class AppDataPair
    {
        /// <summary>
        /// The key of the AppData pair.
        /// </summary>
        public string Key { get; set; }
        /// <summary>
        /// The value of the AppData pair.
        /// </summary>
        public string Value { get; set; }
    }

    /// <summary>
    /// User Application Data class.
    /// </summary>
    public class UserAppData : AppData
    {
        /// <summary>
        /// The userId corresponding to this application data.
        /// </summary>
        public int UserId { get; set; }
    }
}
