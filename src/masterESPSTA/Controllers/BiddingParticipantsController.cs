using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using model;
using persistenDatabase;

namespace masterESPSTA.Controllers
{
    public class BiddingParticipantsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public BiddingParticipantsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: BiddingParticipants
        public async Task<IActionResult> Index()
        {
            return View(await _context.BiddingParticipants.ToListAsync());
        }

        // GET: BiddingParticipants/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var biddingParticipant = await _context.BiddingParticipants
                .FirstOrDefaultAsync(m => m.Id == id);
            if (biddingParticipant == null)
            {
                return NotFound();
            }

            return View(biddingParticipant);
        }

        // GET: BiddingParticipants/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: BiddingParticipants/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,NaturalPerson,Name,IdentificationOrNit,Phone,Cellular,Address,City,Email,DateCreate,Proposals")] BiddingParticipant biddingParticipant)
        {
            if (ModelState.IsValid)
            {
                _context.Add(biddingParticipant);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(biddingParticipant);
        }

        // GET: BiddingParticipants/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var biddingParticipant = await _context.BiddingParticipants.FindAsync(id);
            if (biddingParticipant == null)
            {
                return NotFound();
            }
            return View(biddingParticipant);
        }

        // POST: BiddingParticipants/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,NaturalPerson,Name,IdentificationOrNit,Phone,Cellular,Address,City,Email,DateCreate,Proposals")] BiddingParticipant biddingParticipant)
        {
            if (id != biddingParticipant.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(biddingParticipant);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BiddingParticipantExists(biddingParticipant.Id))
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
            return View(biddingParticipant);
        }

        // GET: BiddingParticipants/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var biddingParticipant = await _context.BiddingParticipants
                .FirstOrDefaultAsync(m => m.Id == id);
            if (biddingParticipant == null)
            {
                return NotFound();
            }

            return View(biddingParticipant);
        }

        // POST: BiddingParticipants/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var biddingParticipant = await _context.BiddingParticipants.FindAsync(id);
            _context.BiddingParticipants.Remove(biddingParticipant);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool BiddingParticipantExists(int id)
        {
            return _context.BiddingParticipants.Any(e => e.Id == id);
        }
    }
}
