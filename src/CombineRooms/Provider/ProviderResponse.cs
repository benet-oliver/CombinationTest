using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace CombinationRooms
{
    //public class AuditData
    //{
    //    public string processTime { get; set; }
    //    public DateTime timestamp { get; set; }
    //    public string requestHost { get; set; }
    //    public string serverId { get; set; }
    //    public string environment { get; set; }
    //    public string release { get; set; }
    //    public string token { get; set; }
    //    public string @internal { get; set; }
    //}

    public class CancellationPolicy
    {
        public string amount { get; set; }
        public DateTime from { get; set; }
    }

    //public class Tax
    //{
    //    public bool included { get; set; }
    //    public string amount { get; set; }
    //    public string currency { get; set; }
    //    public string type { get; set; }
    //    public string clientAmount { get; set; }
    //    public string clientCurrency { get; set; }
    //    public string percent { get; set; }
    //    public List<Tax> taxes { get; set; }
    //    public bool allIncluded { get; set; }
    //}

    //public class Promotion
    //{
    //    public string code { get; set; }
    //    public string name { get; set; }
    //    public string remark { get; set; }
    //}

    public class Rate
    {
        public string rateKey { get; set; }
        public string rateClass { get; set; }
        public string rateType { get; set; }
        public decimal net { get; set; }
        public int allotment { get; set; }
        public string paymentType { get; set; }
        public bool packaging { get; set; }
        public string boardCode { get; set; }
        public string boardName { get; set; }
        public List<CancellationPolicy> cancellationPolicies { get; set; }
        //public List<Tax> taxes { get; set; }
        public int rooms { get; set; }
        public int adults { get; set; }
        public int children { get; set; }
        //public List<Promotion> promotions { get; set; }
    }

    public class Room
    {
        public string code { get; set; }
        public string name { get; set; }
        public List<Rate> rates { get; set; }
    }

    public class Hotel
    {
        public int code { get; set; }
        //public string name { get; set; }
        //public string categoryCode { get; set; }
        //public string categoryName { get; set; }
        //public string destinationCode { get; set; }
        //public string destinationName { get; set; }
        //public int zoneCode { get; set; }
        //public string zoneName { get; set; }
        //public string latitude { get; set; }
        //public string longitude { get; set; }
        public List<Room> rooms { get; set; }
        //public string minRate { get; set; }
        //public string maxRate { get; set; }
        //public string currency { get; set; }
        //public DateTime checkIn { get; set; }
        //public int total { get; set; }
        //public DateTime checkOut { get; set; }
    }

    public class ProviderResponse
    {
        //public AuditData auditData { get; set; }
        [JsonProperty("hotels")]
        public Hotels1 hotels { get; set; }
    }

    public class Hotels1
    {
        [JsonProperty("hotels")]
        public List<Hotel> hotels { get; set; }
    }
}


