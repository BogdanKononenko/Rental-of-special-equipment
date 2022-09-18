using Microsoft.AspNetCore.Mvc.Rendering;

namespace OnlineStore.Models;

public class ReviewsEquipmentModel
{
    public Equipment NeedEquipment { get; set; } = null!;
    public List<Review> NeedReviews { get; set; } = null!;
}