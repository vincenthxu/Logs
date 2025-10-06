namespace Logs.Api.Models
{
    public class User
    {
        #region Constructors
        public User(string name, DateOnly dateOfBirth)
        {
            Id = Guid.NewGuid();
            Name = name;
            DateOfBirth = dateOfBirth;
        }
        #endregion

        #region Properties
        public Guid Id { get; private set; }
        public string? Name { get; set; }
        public DateOnly DateOfBirth { get; set; }
        public int Age
        {
            get
            {
                int years = DateTime.Today.Year - DateOfBirth.Year;
                return DateOnly.FromDateTime(DateTime.Today) > DateOfBirth.AddYears(years) ? years : --years;
            }
        }
        #endregion

        #region Methods
        public override string ToString() => System.Text.Json.JsonSerializer.Serialize(this);
        #endregion
    }
}