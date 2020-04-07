using System;
using MongoDB.Bson.Serialization.Attributes;

namespace AccountService.Domain
{
    public class Account
    {
        [BsonId]
        public Guid Id { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
    }
}