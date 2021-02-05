using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace ColossusFileManager.Shared.Models
{
    [Table("cb_files")]
    public class CbFile
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public int Id { get; set; }

        public int ParentFolderId { get; set; }

        public CbFolder ParentFolder { get; set; }

        [Required]
        public string FileName { get; set; }
        
        public string FileExtension { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime DateCreated { get; set; }

        public DateTime? Dateupdated { get; set; }


    }
}
