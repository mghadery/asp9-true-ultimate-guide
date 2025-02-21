namespace People.Entities
{
    /// <summary>
    /// Domain Entity
    /// </summary>
    public class Country
    {
        public Guid CountryId { get; set; }
        public string CountryName { get; set; } = string.Empty;

        public virtual ICollection<Person> Persons { get; set; } = new List<Person>();
    }
}
