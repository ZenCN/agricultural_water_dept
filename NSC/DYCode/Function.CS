using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Text;
using System.Security.Cryptography;
using NSC;

namespace Dynashety
{
    /// <summary>
    /// 常量
    /// </summary>
    public class ConstInfo
    {
        /// <summary>
        /// 会话ID
        /// </summary>
        public const string sys_session_info = "sys_session_info";
        public enum AssignSession { ActionResult, JsonResult }
    }

    /// <summary>
    /// 会话
    /// </summary>
    [Serializable]
    public class SessionInfo : SX02_USER
    {
    }
}