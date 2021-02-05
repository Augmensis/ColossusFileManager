using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace ColossusFileManager.Shared.Models
{
    [Table("cb_folders")]
    public class CbFolder
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public int Id { get; set; }
        
        public int ParentFolderId { get; set; }
        public CbFolder ParentFolder { get; set; }

        public List<CbFolder> ChildFolders { get; set; } = new List<CbFolder>();

        public List<CbFile> Files { get; set; } = new List<CbFile>();


        [Required]
        public string FolderName { get; set; }

        public string FolderPath { get; set; }


        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime DateCreated { get; set; }

        public DateTime? Dateupdated { get; set; }
    }
}
