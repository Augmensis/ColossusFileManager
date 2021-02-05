using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.IO;
using System.Linq;
using System.Text;

namespace ColossusFileManager.Shared.Models
{
    [Table("cb_files")]
    public class CbFile
    {

        public CbFile() { }
        public CbFile(string fileName) 
        {
            FileName = fileName;

            // Pseudo extension parsing - Could use FileInfo or Path
            FileExtension = fileName.Split(".").Last();

            DateCreated = DateTime.UtcNow;
        }


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
