namespace PrimeHotel.Web.Models
{
    public class Address
    {
        public int Id { get; set; }

        public string Street { get; set; }

        public string HouseNumber { get; set; }

        public string City { get; set; }

        public string PostCode { get; set; }

        public int ProfileId { get; set; }

        public Profile Profile { get; set; }
    }
}
