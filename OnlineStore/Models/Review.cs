using System.ComponentModel.DataAnnotations.Schema;

namespace OnlineStore.Models;

public class Review
{
    public int Id { get; set; }
    public string? ReviewString { get; set; }
    public int EquipmentId { get; set; }
    public string? ReviewersName { get; set; }
}