﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace ColorChecker {
    public struct MyColor {
        public Color Color { get; set; }
        public string Name { get; set; }
        public override string ToString() {
            return base.ToString(); //←後で使いやすいように書き換える
        }
    }
}
