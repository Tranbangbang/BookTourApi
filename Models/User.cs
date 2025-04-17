using System.ComponentModel.DataAnnotations;

namespace BookTour.Models
{
    public class User
    {
        [Key]
        public int UserId { get; set; }
        [StringLength(50)]
        public string Username { get; set; }

        [StringLength(255)]
        public string Password { get; set; }

        [StringLength(100)]
        public string FullName { get; set; }

        [StringLength(100)]
        [EmailAddress]
        public string Email { get; set; }

        [StringLength(20)]
        public string Phone { get; set; }

        [StringLength(255)]
        public string Address { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public DateTime? LastLogin { get; set; }

        public virtual ICollection<Booking> Bookings { get; set; }
        public virtual ICollection<SavedTour> SavedTours { get; set; }
        public virtual ICollection<Review> Reviews { get; set; }
        public virtual ICollection<CustomTour> CustomTours { get; set; }
    }
}