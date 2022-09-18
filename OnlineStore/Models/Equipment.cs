using System.ComponentModel.DataAnnotations;
using System.Drawing;
using System.Net.Mime;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace OnlineStore.Models;

public class Equipment
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public int Price { get; set; } = 0;
    public string? PathImg { get; set; }
    public string? Description { get; set; }
    public string? NumberPhone { get; set; }
    public int DiscountPercentage { get; set; } = 0;
    public byte[]? Photo { get; set; }
    public string? Category { get; set; }

    public int DiscountedPrice()
    {
        double newPrice = Price * (1 - (DiscountPercentage / 100.0));
        return (int)Math.Ceiling(newPrice / 10) * 10;
    }
}

public enum CategoryState
{
    Экскаватор,
    Грузовик,
    Прицеп,
    Бульдозер,
}

public enum SortState
{
    PriceAsc,
    PriceDesc
}