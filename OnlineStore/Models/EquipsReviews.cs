using Microsoft.AspNetCore.Mvc.Rendering;

namespace OnlineStore.Models;

public class EquipsReviews
{
    public IEnumerable<Equipment> Equipments = null!;
    public IEnumerable<Review> Reviews = null!;
    public List<SelectListItem> SelectListItem = null!;
}