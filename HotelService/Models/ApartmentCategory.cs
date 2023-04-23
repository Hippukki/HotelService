using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelService.Models
{
    public enum ApartmentCategory
    {
        [Display(Name = "Одноместный")] Single,
        [Display(Name = "Двухместный")] Double,
        [Display(Name = "Двухместный с двумя кроватями")] Twin,
        [Display(Name = "Трёхместный")] Triple,
        [Display(Name = "Четырёхместный")] Quadriple,
    }
}
