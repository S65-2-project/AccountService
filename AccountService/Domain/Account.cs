using System;
using MongoDB.Bson.Serialization.Attributes;

namespace AccountService.Domain
{
    public class Account
    {
        [BsonId] 
        public Guid Id { get; set; }
        public string Email { get; set; }
        public byte[] Password { get; set; }
        public byte[] Salt { get; set; }
        public bool isDAppOwner { get; set; }
        public bool isDelegate { get; set; }
        public string Token { get; set; }
    }
}