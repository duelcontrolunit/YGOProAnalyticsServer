﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace YGOProAnalyticsServer.DTOs
{
    public class CardDTO
    {
        public int PassCode { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Archetype { get; set; }
        public string Type { get; set; }
        public string Race { get; set; }
        public string ImageUrl { get; set; }
        public string SmallImageUrl { get; set; }
    }
}
