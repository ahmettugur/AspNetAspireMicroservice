using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.RabbitMQ;

public class RabbitMqSettings
{
    public string Hostname { get; set; } = null!;
    public string VHostname { get; set; } = null!;
    public string Username { get; set; } = null!;
    public string Password { get; set; } = null!;
    public int Port { get; set; }
}
