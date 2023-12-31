//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Rental_Car.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public partial class tb_cars
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public tb_cars()
        {
            this.tb_bookings = new HashSet<tb_bookings>();
        }
    
        public int car_id { get; set; }

        [Required(ErrorMessage = "Please enter car name.")]
        [StringLength(100, MinimumLength = 1, ErrorMessage = "Car name cannot be longer than 100 characters")]
        public string car_name { get; set; }


        [Required(ErrorMessage = "Please enter model name.")]
        [StringLength(100, MinimumLength = 1, ErrorMessage = "Model name cannot be longer than 100 characters")]
        public string model { get; set; }

        [Required(ErrorMessage = "Please enter make name.")]
        [StringLength(100, MinimumLength = 1, ErrorMessage = "Make name cannot be longer than 100 characters")]
        public string make { get; set; }


        public string picture { get; set; }


        [Required(ErrorMessage = "Please enter rent price.")]
        [Range(0.00, 99999.99, ErrorMessage = "Rent price can not be greater than 99999")]
        public Nullable<decimal> rent_price { get; set; }


        [StringLength(250, MinimumLength = 0, ErrorMessage = "Description name cannot be longer than 250 characters")]
        public string description { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tb_bookings> tb_bookings { get; set; }
    }
}
