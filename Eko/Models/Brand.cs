﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Eko.Models
{
    public class Brand
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string URLName
        {
            get { return Name.ToLower().Replace(' ', '-'); }
        }

        public IList<Model> Models { get; set; }
    }
}
