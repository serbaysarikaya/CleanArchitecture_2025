﻿using System;
using System.Collections.Generic;
using System.Text;

namespace CleanArchitecture_2025.Infrastructure.Options
{
    public sealed class JwtOptions
    {
        public string Issuer { get; set; } = default!;
        public string Audience { get; set; } = default!;
        public string SecretKey { get; set; } = default!;
    }
}
