﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PallyWad.Domain.Entities
{
    public class MailRequest
    {
        public string? ToEmail { get; set; }
        public string? Subject { get; set; }
        public string? Body { get; set; }
    }

    public class MailSettings
    {
        public string? Host { get; set; }
        public int Port { get; set; }
        public string? DisplayName { get; set; }
        public string? Mail { get; set; }
        public string? Username { get; set; }
        public string? Password { get; set; }
    }
}
