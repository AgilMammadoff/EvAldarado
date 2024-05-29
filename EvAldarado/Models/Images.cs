using System.ComponentModel.DataAnnotations.Schema;

namespace EvAldarado.Models
{
    public class Images

    {
        public int Id {  get; set; }    
        public string ImgUrlbase { get; set; }
        public int UserProductId { get; set; }
        [ForeignKey("UserProductId")]
        public UserProducts userProducts { get; set; }

    }
}
