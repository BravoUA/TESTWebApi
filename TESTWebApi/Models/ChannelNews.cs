using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TESTWebApi.Models
{
	public class ChannelNews
	{
		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public int id { get; set; }
		[Required]
		public int StatusView { get; set; }
		[Required]
		public string Title { get; set; }
		[Required]
		public string Link { get; set; }
		[Required]
		public string Description { get; set; }
		[Required]
		public string PublDate { get; set; }
		[DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
		public DateOnly DateAdd { get; set; }
		public int RSSChannelID { get; set; }
	}
}
