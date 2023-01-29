using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace TESTWebApi.Models
{
	public class RSSChannel
	{
		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public int id { get; set; }
		public int State { get; set; }
		[Required]
        public string NameChannel { get; set; }
		[DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MM/dd/yyyy}")]
		public DateOnly DateSubscribe { get; set; }
		[Required]
		public string Link { get; set; }
		[Required]
		public string LinkforUpdate { get; set; }




	}
}
