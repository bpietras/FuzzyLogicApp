using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using System.ComponentModel.DataAnnotations.Schema;

namespace FuzzyLogicWebService.FISFiles.DBModel
{
    [Table("Models")]
    public class FuzzyModel
    {
        [Key]
        [HiddenInput(DisplayValue = false)]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ModelID { get; set; }

        [Required(ErrorMessage = "Podaj nazwę modelu")]
        [Display(Name = "Nazwa systemu:")]
        public string Name { get; set; }

        public string Description { get; set; }

        [Required(ErrorMessage = "Podaj liczbe wejść")]
        [Range(1, 10, ErrorMessage = "Liczba wejść musi mieścić się w przedziale 1-10")]
        [Display(Name = "Liczba wejść:")]
        public int InputsNumber { get; set; }

        [Required(ErrorMessage = "Podaj liczbe wyjść")]
        [Range(1, 10, ErrorMessage = "Liczba wyjść musi mieścić się w przedziale 1-10")]
        [Display(Name = "Liczba wyjść:")]
        public int OutputsNumber { get; set; }

        [Required(ErrorMessage = "Podaj liczbę reguł")]
        [Range(1, 100, ErrorMessage = "Liczba reguł musi mieścić się w przedziale 1-100")]
        [Display(Name = "Liczba reguł:")]
        public int RulesNumber { get; set; }

        [HiddenInput(DisplayValue = false)]
        public int UserID { get; set; }

        [ForeignKey("UserID")]
        public virtual User User { get; set; }

        public virtual IEnumerable<InVariable> InputVariables { get; set; }
        public virtual ICollection<OVariable> OutputVariables { get; set; }
        public virtual ICollection<Rule> Rules { get; set; }


    }
}