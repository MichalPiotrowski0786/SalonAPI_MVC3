using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Salon.Data;
using Salon.Models;

namespace Salon.Controllers
{
    public class CarsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CarsController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index(string sortOrder, string searchString)
        {
            ViewData["MarkaSort"] = sortOrder == "Marka" ? "marka_desc" : "Marka";
            ViewData["PriceSort"] = sortOrder == "Cena" ? "cena_desc" : "Cena";
            ViewData["RocznikSort"] = sortOrder == "Rocznik" ? "rocznik_desc" : "Rocznik";
            ViewData["ModelSort"] = sortOrder == "Model" ? "model_desc" : "Model";
            ViewData["KolorSort"] = sortOrder == "Kolor" ? "kolor_desc" : "Kolor";
            ViewData["MocSort"] = sortOrder == "Moc" ? "moc_desc" : "Moc";
            ViewData["CurrentFilter"] = searchString;

            var cars = from s in _context.Car
                       select s;
            if (!String.IsNullOrEmpty(searchString))
            {
                cars = cars.Where(s => s.Marka.Contains(searchString)
                                       || s.Model.Contains(searchString));
            }
            switch (sortOrder)
            {
                case "Marka":
                    cars = cars.OrderBy(s => s.Marka);
                    break;
                case "marka_desc":
                    cars = cars.OrderByDescending(s => s.Marka);
                    break;
                case "Cena":
                    cars = cars.OrderBy(s => s.Cena);
                    break;
                case "cena_desc":
                    cars = cars.OrderByDescending(s => s.Cena);
                    break;
                case "Rocznik":
                    cars = cars.OrderBy(s => s.Rocznik);
                    break;
                case "rocznik_desc":
                    cars = cars.OrderByDescending(s => s.Rocznik);
                    break;
                case "Model":
                    cars = cars.OrderBy(s => s.Model);
                    break;
                case "model_desc":
                    cars = cars.OrderByDescending(s => s.Model);
                    break;
                case "Kolor":
                    cars = cars.OrderBy(s => s.Kolor);
                    break;
                case "kolor_desc":
                    cars = cars.OrderByDescending(s => s.Kolor);
                    break;
                case "Moc":
                    cars = cars.OrderBy(s => s.Moc);
                    break;
                case "moc_desc":
                    cars = cars.OrderByDescending(s => s.Moc);
                    break;
                default:
                    cars = cars.OrderBy(s => s.Marka);
                    break;
            }
            return View(await cars.AsNoTracking().ToListAsync());
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var car = await _context.Car
                .FirstOrDefaultAsync(m => m.Id == id);
            if (car == null)
            {
                return NotFound();
            }

            return View(car);
        }

        /*[Authorize(Roles = "admins")]*/
        [Authorize(Policy = "writepolicy")]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Marka,Model,Kolor,Moc,Rocznik,Cena")] Car car)
        {

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Add(car);
                    TempData["add"] = "Dodano pojazd: " + car.Marka + " " + car.Model;
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));

                } catch (DbUpdateException ex) {
                    return RedirectToAction("DBException", "Exceptions", "Błąd");
                }        
                
            }
            return View(car);
        }

        /*[Authorize(Roles = "admins")]*/
        [Authorize(Policy = "writepolicy")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var car = await _context.Car.FindAsync(id);
            if (car == null)
            {
                return NotFound();
            }
            return View(car);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Marka,Model,Kolor,Moc,Rocznik,Cena")] Car car)
        {
            if (id != car.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(car);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CarExists(car.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(car);
        }

        /*[Authorize(Roles = "admins")]*/
        [Authorize(Policy = "writepolicy")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var car = await _context.Car
                .FirstOrDefaultAsync(m => m.Id == id);
            if (car == null)
            {
                return NotFound();
            }

            return View(car);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var car = await _context.Car.FindAsync(id);
            _context.Car.Remove(car);
            TempData["del"] = "Usunięto pojazd: " + car.Marka + " " + car.Model;
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CarExists(int id)
        {
            return _context.Car.Any(e => e.Id == id);
        }
    }
}
