namespace Board_3.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Account
    {
        public int AccountId { get; set; }

        [StringLength(20)]
        [Display(Name = "–¼‘O")]
        public string Name { get; set; }

        [StringLength(30)]
        public string PassWord { get; set; }

        public List<Komment> Komments { get; set; }
    }
}
