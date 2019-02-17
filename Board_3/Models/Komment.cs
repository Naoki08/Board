namespace Board_3.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Komment
    {
        public int KommentId { get; set; }

        [StringLength(50)]
        [Display(Name = "���e���e")]
        public string Body { get; set; }

        [Display(Name = "���e����")]
        public DateTime? Created { get; set; }

        public int AccountId { get; set; }

        public Account Account { get; set; }
    }
}
