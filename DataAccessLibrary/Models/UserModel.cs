﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLibrary.Models
{
    public class UserModel
    {
        public int id { get; set; }
        public string username { get; set; }     
        public string password_hash { get; set; }
    }
}