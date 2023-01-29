using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TESTWebApi.Models
{
	public class Users
	{
		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public int Id { get; set; }
		public string Username { get; set; }
		public string UserEmail { get; set; }
		public string UserPassword { get; set; }

	}
}
