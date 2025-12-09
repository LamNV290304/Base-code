using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace SEP490.Domain.Logging
{
    public class LogLogin
    {
        public BigInteger Id { get; set; }
        public Guid UserId { get; set; }
        public DateTime LoginAt { get; set; } = DateTime.UtcNow;
        public DateTime LogoutAt { get; set; } = DateTime.UtcNow;
    }
}
