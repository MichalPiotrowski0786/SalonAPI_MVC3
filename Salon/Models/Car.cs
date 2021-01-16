using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Salon.Models
{
    public class Car
    {
        public int Id { get; set; }

        [StringLength(60, MinimumLength = 2, ErrorMessage = "Nazwa marki jest za długa lub za krótka")]
        [RegularExpression(@"^[A-Z]+[a-zA-Z\- ]*$", ErrorMessage = "Podaj poprawną markę")]
        [Required(ErrorMessage = "Pole jest wymagane")]
        public string Marka { get; set; }

        [StringLength(60, MinimumLength = 1, ErrorMessage = "Nazwa modelu jest za długa lub za krótka")]
        [RegularExpression(@"^[a-zA-Z0-9 ]*$", ErrorMessage = "Podaj poprawny model")]
        [Required(ErrorMessage = "Pole jest wymagane")]
        public string Model { get; set; }

        [StringLength(50, MinimumLength = 1, ErrorMessage = "Podaj prawidłowy kolor")]
        [RegularExpression(@"^[A-Z]+[a-zA-ZĄąĆćĘęŁłŃńÓóŚśŹźŻż\- ]*$", ErrorMessage = "Podaj prawidłowy kolor")]
        public string Kolor { get; set; }

        [Range(0, 1000, ErrorMessage = "Moc musi zawierać się w przedziale od 1 do 1000")]
        [Required(ErrorMessage = "Pole jest wymagane")]
        public int Moc { get; set; }

        [Range(1900, 2021, ErrorMessage = "Rocznik musi zawierać się w przedziale od 1900 do 2050")]
        [Required(ErrorMessage = "Pole jest wymagane")]
        public int Rocznik { get; set; }

        [Range(0, 999999, ErrorMessage = "Przebieg musi zawierać się w przedziale od 0 do 999.999")]
        [Required(ErrorMessage = "Pole jest wymagane")]
        public int Przebieg { get; set; }

        [Range(1000, 1500000, ErrorMessage = "Cena musi zawierać się w przedziale od 1000 do 1.500.000")]
        [Required(ErrorMessage = "Pole jest wymagane")]
        public int Cena { get; set; }
    }
}
