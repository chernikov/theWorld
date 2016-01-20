using Microsoft.Data.Entity;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TheWorld.Models
{
    public class WorldRepository : IWorldRepository
    {
        private WorldContext _context;
        private ILogger<WorldRepository> _logger;

        public WorldRepository(WorldContext context, ILogger<WorldRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public void AddStop(string tripName, Stop newStop)
        {
            var theTrip = GetTripByName(tripName);
            newStop.Order = theTrip.Stops.Max(s => s.Order) + 1;
            theTrip.Stops.Add(newStop);
            _context.Stops.Add(newStop);

        }

        public void AddTrim(Trip newTrip)
        {
            _context.Trips.Add(newTrip);
        }

        public IEnumerable<Trip> GetAllTrips()
        {
            try {
                return _context.Trips.OrderBy(t => t.Name).ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError("Could not get trips from database", ex);
                return null;
            }
        }

        public IEnumerable<Trip> GetAllTripsWithStops()
        {
            try
            {
                return _context.Trips
                .Include(p => p.Stops)
                .OrderBy(p => p.Name)
                .ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError("Could not get trips from database", ex);
                throw;
            }
        }

        public Trip GetTripByName(string tripName)
        {
            return _context.Trips.Include(p => p.Stops)
                .FirstOrDefault(t => t.Name == tripName);
        }

        public bool SaveAll()
        {
            return _context.SaveChanges() > 0;
        }
    }
}
