namespace Logs.Api.Models
{
    public class User
    {
        #region Fields
        private DateOnly dateOfBirth;
        #endregion

        #region Constructors
        public User(string name, DateOnly dateOfBirth)
        {
            Id = Guid.NewGuid();
            Name = name;
            DateOfBirth = dateOfBirth;
        }
        #endregion

        #region Properties
        internal bool HasValidDateOfBirth { get; set; } = true;
        internal bool HasInvalidDateOfBirth => !HasValidDateOfBirth;
        public Guid Id { get; internal set; }
        public string? Name { get; set; }
        public DateOnly DateOfBirth
        {
            get => dateOfBirth;
            set
            {
                // if birthday has not yet occurred, indicate invalid date of birth
                // else indicate valid and allow set

                if (DateOnly.FromDateTime(DateTime.Today) < value)
                {
                    HasValidDateOfBirth = false;
                }
                else
                {
                    dateOfBirth = value;
                }
            }
        }
        public int Age
        {
            get
            {
                int years = DateTime.Today.Year - DateOfBirth.Year;
                return DateOnly.FromDateTime(DateTime.Today) >= DateOfBirth.AddYears(years) ? years : --years;
            }
        }
        #endregion

        #region Methods
        public override string ToString() => System.Text.Json.JsonSerializer.Serialize(this);
        #endregion
    }
}