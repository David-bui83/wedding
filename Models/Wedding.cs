using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace wedding.Models
{
    public class Wedding
    {
        [Key]
        public int WeddingId {get;set;}

        public int UserId {get;set;}

        [Required(ErrorMessage="Wedder one cannot be empty")]
        [MinLength(2,ErrorMessage="Wedder one must be 2 characters or more")]    
        public string WedderOne {get;set;}

        [Required(ErrorMessage="Wedder two cannot be empty")]
        [MinLength(2,ErrorMessage="Wedder two must be 2 characters or more")]  
        public string WedderTwo {get;set;}
        [Required(ErrorMessage="Wedding date cannot be empty")]
        // [FutureDate]
        public DateTime? WeddingDate {get;set;}
        
        [Required(ErrorMessage="Wedding address is connot be empty")]
        [MinLength(2,ErrorMessage="Wedding address must be 2 characters or more")]
        public string Address {get;set;}

        public DateTime CreatedAt {get;set;} = DateTime.Now;
        public DateTime UpdatedAt {get;set;} = DateTime.Now;

        public User creator {get;set;}

        public List<RSVP> UserRsvp {get;set;} 
    }

    //  public class FutureDateAttribute : ValidationAttribute
    // {
    //     protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    //     {
    //         if (value == null || Convert.ToDateTime(value)<DateTime.Now)
    //             return new ValidationResult("Wedding date must be in the future!");
    //         return ValidationResult.Success;
    //     }
    // }
}