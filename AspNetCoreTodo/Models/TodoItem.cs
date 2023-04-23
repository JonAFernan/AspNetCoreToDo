using System;
using System.ComponentModel.DataAnnotations;

namespace AspNetCoreTodo.Models
{
    public class TodoItem
    {
        public Guid ID{get; set;}
        public bool IsDone{get; set;}
        [Required]
        public string Title {get; set;}
        public DateTimeOffset DueAt{get; set;}
        [Required]
        public int AddDays{get; set;}
        [Required]
        public string UserId {get ; set;}
    }
}