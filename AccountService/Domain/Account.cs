using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json.Converters;

namespace AccountService.Domain
{
    public class Account
    {
        [BsonId]
        public Guid Id { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public bool isDAppOwner { get; set; }
        public bool isDelegate { get; set; }
    }
}