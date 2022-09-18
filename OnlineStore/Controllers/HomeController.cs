using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Drawing;
using System.Net.Mime;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations.Operations;
using OnlineStore.Models;

namespace OnlineStore.Controllers;

public class HomeController : Controller
{
    ApplicationContext db;

    public HomeController(ApplicationContext context)
    {
        db = context;
    }

    public async Task<IActionResult> Index(string category = "Все", SortState sortState = SortState.PriceAsc)
    {
        ViewBag.PriceSort = sortState == SortState.PriceAsc ? SortState.PriceDesc : SortState.PriceAsc;

        var dbEquipmentsSort = sortState switch
        {
            SortState.PriceDesc => await db.Equipments.OrderByDescending(equip => equip.Price).ToListAsync(),
            _ => await db.Equipments.OrderBy(equip => equip.Price).ToListAsync()
        };

        var selectListItemFromEnum = Enum.GetValues(typeof(CategoryState))
            .Cast<CategoryState>()
            .Select(v => new SelectListItem
            {
                Text = v.ToString()
            }).ToList();

        selectListItemFromEnum.Insert(0, new SelectListItem() { Text = "Все" });
        selectListItemFromEnum.RemoveAll(item => item.Text == category);
        selectListItemFromEnum.Insert(0, new SelectListItem() { Text = category });

        var equipsReviews = new EquipsReviews()
        {
            SelectListItem = selectListItemFromEnum
        };

        if (category == "Все")
        {
            equipsReviews.Equipments = dbEquipmentsSort;
            equipsReviews.Reviews = await db.Reviews.ToListAsync();

            return View(equipsReviews);
        }

        equipsReviews.Equipments = dbEquipmentsSort.Where(equip => equip.Category == category);
        var listId = equipsReviews.Equipments.Select(v => v.Id).ToList();
        equipsReviews.Reviews = await db.Reviews.Where(review => listId.Contains(review.EquipmentId)).ToListAsync();

        return View(equipsReviews);
    }

    public IActionResult PlaceAd()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> PlaceAd(Equipment equipment, IFormFile image)
    {
        using (var ms = new MemoryStream())
        {
            await image.CopyToAsync(ms);
            equipment.Photo = ms.ToArray();

            db.Equipments.Add(equipment);
            await db.SaveChangesAsync();
        }

        return RedirectToAction("Index");
    }

    public async Task<IActionResult> CommentsAd(int id)
    {
        var fitEquipment = db.Equipments.FirstOrDefault(equipment => equipment.Id == id);
        var fitReviews = await db.Reviews.Where(rev => rev.EquipmentId == id).ToListAsync();

        var fitModel = new ReviewsEquipmentModel()
        {
            NeedEquipment = fitEquipment,
            NeedReviews = fitReviews
        };
        return View(fitModel);
    }

    public async Task<IActionResult> SendComment(int equipmentId, string nameText, string reviewsText)
    {
        db.Reviews.Add(new Review()
        {
            EquipmentId = equipmentId,
            ReviewString = reviewsText,
            ReviewersName = nameText
        });
        await db.SaveChangesAsync();

        return RedirectToAction("CommentsAd", new { id = equipmentId });
    }

    [Authorize]
    public IActionResult CheckingForAuthorization() // передавать токен в запрос без fetch и делать переход по ссылке
    {
        return Ok();
    }
    
    [Authorize(Roles = "admin")]
    public IActionResult CheckingForAdmin()
    {
        return Ok();
    }

    public IActionResult Login()
    {
        return View();
    }

    public async Task<IActionResult> DeleteEquipment(int id)
    {
        var deleteEquipment = db.Equipments.FirstOrDefault(equipment => equipment.Id == id);
        db.Equipments.Remove(deleteEquipment);
        await db.SaveChangesAsync();

        return RedirectToAction("Index");
    }
}