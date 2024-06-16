
using LMS.Attributes;
using System.ComponentModel.DataAnnotations;
using System.Security.Cryptography;
using System.Text;

namespace LMS.Models
{
    public class User
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; } = string.Empty;

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; } = string.Empty;

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Confirm Password")]
        [Compare("Password", ErrorMessage = "Password and Confirmation Password do not match.")]
        public string ConfirmPassword { get; set; } = string.Empty;

        [Required]
        [Display(Name = "First Name")]
        public string FirstName { get; set; } = string.Empty;

        [Required]
        [Display(Name = "Last Name")]
        public string LastName { get; set; } = string.Empty;

        [Required]
        [Display(Name = "Birth Date")]
        [DataType(DataType.Date)]
        [MinimumAge(16)]
        public DateTime BirthDate { get; set; }

        [Required]
        [Display(Name = "Role")]
        public string Role { get; set; } = string.Empty;

        //public virtual ICollection<Event> Events { get; set;}

        [Display(Name = "Address 1")]
        public string? Address1 { get; set; } = string.Empty;

        [Display(Name = "Address 2")]
        public string? Address2 { get; set; } = string.Empty;

        [Display(Name = "City")]
        public string? City { get; set; } = string.Empty;

        [Display(Name = "State")]
        public string? State { get; set; } = string.Empty;

        [Display(Name = "Zip")]
        public string? ZipCode { get; set; } = string.Empty;

        [Display(Name = "Phone Number")]
        public string? Phone { get; set; } = string.Empty;

        [Url]
        [Display(Name = "Link 1")]
        public string? Link1 { get; set; }

        [Url]
        [Display(Name = "Link 2")]
        public string? Link2 { get; set; } 

        [Url]
        [Display(Name = "Link 3")]
        public string? Link3 { get; set; }

        public double? UserPayment { get; set; } = null;

        public bool CheckPassword(string password, string hash, byte[] salt)
        {
            var hashToCompare = Rfc2898DeriveBytes.Pbkdf2(password, salt, 350000, HashAlgorithmName.SHA512, 64);
            return CryptographicOperations.FixedTimeEquals(hashToCompare, Convert.FromHexString(hash));

            //string passwordhash = HashPassword(password, out salt);
            //return (passwordhash.Equals(hash));
        }

        //private string _password = string.Empty;

        //private string HashPassword(string password, out byte[] salt) // function influenced by "https://code-maze.com/csharp-hashing-salting-passwords-best-practices/"
        //{
        //    salt = new byte[64] { 1,0,1,1,0,1,1,0,1,1,1,0,0,0,1,0,0,0,0,0,0,0,1,1,0,0,1,1,0,1,0,1,1,1,0,1,0,0,1,1,1,0,1,1,0,0,1,0,1,0,0,0,1,0,0,1,1,1,0,0,0,0,0,1 }; //temp fixed salt //TODO: remove //RandomNumberGenerator.GetBytes(64); //64 = keysize
        //    var hash = Rfc2898DeriveBytes.Pbkdf2(
        //        Encoding.UTF8.GetBytes(password),
        //        salt,
        //        350000, //iterations
        //        HashAlgorithmName.SHA512,
        //        64);//key size
        //    return Convert.ToHexString(hash);
        //}

    }
}
