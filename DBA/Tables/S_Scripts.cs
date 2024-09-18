using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBA.Tables;

public class S_Scripts
{
    [Key]
    public int scriptID { get; set; }
    public string hash { get; set; }
    public string subdomain { get; set; }
    public string content { get; set; }
    public DateTime scanDate { get; set; }
    public bool isAllowed { get; set; }
}
