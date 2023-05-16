using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SESSIONWPF
{
    internal class Query
    {
        public static string selectAuth = "select [Login], [Password], [Role] from [dbo].[User] where [Login] = '{0}' and [Password] = '{1}' ";
    }
}
