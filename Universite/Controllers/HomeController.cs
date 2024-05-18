using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Universite.Models;

namespace Universite.Controllers;

public class HomeController : Controller
{
    private readonly UniversiteContext _context = new UniversiteContext();
    public IActionResult Index(int? id)
    {
        var fakulteler = _context.Fakultelers.ToList();
        var bolumler = _context.Bolumlers.Include(x => x.Fakulte).ToList();

        var allData = (fakulteler: fakulteler, bolumler: bolumler);

        if (!id.HasValue || id == 0)
        {
            return View(allData);
        }
        else
        {
            var filtreBolumler = _context.Bolumlers.Include(x => x.Fakulte).Where(x => x.FakulteId == id).ToList();
            allData.bolumler = filtreBolumler; // Tuple içerisindeki bolumler özelliğine filtreBolumler'i atadık.
            return View(allData);
        }
    }


    public IActionResult FakulteEkle()
    {
        return View();
    }

    [HttpPost]
    public IActionResult FakulteEkle(Fakulteler fakulte)
    {
        _context.Fakultelers.Add(fakulte);
        _context.SaveChanges();
        return RedirectToAction("FakulteEkle");
    }

    public IActionResult BolumEkle()
    {
        var data = _context.Fakultelers.ToList();
        ViewBag.Fakulteler = new SelectList(data, "FakulteId", "FakulteAd");
        return View();
    }

    [HttpPost]
    public IActionResult BolumEkle(Bolumler bolum)
    {
        _context.Bolumlers.Add(bolum);
        _context.SaveChanges();
        return RedirectToAction("BolumEkle");
    }

    public IActionResult DeleteBolum(int id)
    {
        var data = _context.Bolumlers.FirstOrDefault(x => x.BolumId == id);
        _context.Bolumlers.Remove(data!);
        _context.SaveChanges();
        return RedirectToAction("Index");
    }
}
