using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelService.Models
{
    public enum ApartmentLevel
    {
        [Display(Name = "Стандартный")] Standard,
        [Display(Name = "Улучшенный")] Superior,
        [Display(Name = "Со спальной комнатой")] Bedroom,
        [Display(Name = "Апартаменты/квартира")] Apartment,
        [Display(Name = "Студия")] Studio,
        [Display(Name = "Люкс")] Suite,
        [Display(Name = "Двухэтажный")] Duplex
    }
}
