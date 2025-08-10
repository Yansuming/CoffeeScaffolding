using System;
using CoffeeScaffolding.CoffeeScaffoldingData.EntityDomain;

namespace CoffeeScaffoldingData.Models.RABC
{
    public class CoffeeUser : BaseWithConcurrencyTable
    {
        public long Id { get; set; }
        public string? Account { get; set; }
        public string? UserName { get; set; }
        public string? PasswordHash { get; set; }
        public string? PhoneNumber { get; set; }
        public bool PhoneNumberConfirmed { get; set; }
        public string? Email { get; set; }
        public bool EmailConfirmed { get; set; }
        public string? OpenId { get; set; }
        public string? UnionId { get; set; }
        public string? AppId { get; set; }
        public bool IsActive { get; set; }
        public int? AccessFailedCount { get; set; }
    }
}