using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace TESTWebApi.Models
{
	public class UsersSubscribes
	{
		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public int Id { get; set; }
		public RSSChannel RSSChannel { get; set; }
        public int UsersID { get; set; }
    }
}
