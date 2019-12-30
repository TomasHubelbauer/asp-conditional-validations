using System;
using System.ComponentModel.DataAnnotations;

namespace asp_conditional_validations.Models
{
  public class DemoModel
  {
    [Required]
    public string FirstName { get; set; }

    [Required]
    public string LastName { get; set; }

    [Required]
    public bool IsEmployed { get; set; }

    [Required]
    public string JobTitle { get; set; }
  }
}
