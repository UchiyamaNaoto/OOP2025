﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test01 {
    public class Student {
        public required string Name { get; init; } = string.Empty;
        public required string Subject { get; init; } = string.Empty;
        public required int Score { get; init; }

    }
}
