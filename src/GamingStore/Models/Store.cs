﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Security.Policy;
using System.Threading;
using GamingStore.Contracts;
using GamingStore.Models.Relationships;

namespace GamingStore.Models
{
    public class Store
    {
        public static int StoreCounter;

        public Store()
        {
            Orders = new List<Order>();
            StoreItems = new List<StoreItem>();
            Id = StoreCounter;
            Interlocked.Increment(ref StoreCounter);
        }

        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }

        [Required, DataType(DataType.Text), StringLength(50), RegularExpression(@"[a-zA-Z]{2,}$")]
        public string Name { get; set; }

        [Required] public Address Address { get; set; }
        [DisplayName("Phone Number")]
        [Required, DataType(DataType.PhoneNumber), StringLength(50), Phone]
        public string PhoneNumber { get; set; }

        [DataType(DataType.EmailAddress), EmailAddress]
        public string Email { get; set; }
        [DisplayName("Opening Hours")]
        public OpeningHours[] OpeningHours { get; set; }

        //public Dictionary<Item,uint> Stock { get; set; } // determines how many items there are in the store. example: {{fridge, 5},{mouse,6}}
        public ICollection<Order> Orders { get; set; }

        public ICollection<StoreItem> StoreItems { get; set; } // many to many relationship

        public bool IsOpen()
        {
            var currentDateTime = DateTime.Now;
            var curDay = currentDateTime.Day - 1;
            var curTime = currentDateTime.TimeOfDay;

            return OpeningHours[curDay].OpeningTime <= curTime && curTime <= OpeningHours[curDay].ClosingTime;
        }
    }
}