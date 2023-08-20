using TechBlogApp.Domain.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace TechBlogApp.Persistence
{
    public class ApplicationDbContext : IdentityDbContext<AppUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        //public DbSet<RunWay> RunWays { get; set; }
        //public DbSet<Luggage> Luggages { get; set; }
        //public DbSet<Airline> Airlines { get; set; }
        //public DbSet<Flight> Flights { get; set; }
        //public DbSet<Gate> Gates { get; set; }
        public DbSet<AppUser> AppUsers { get; set; }
        public DbSet<Article> Articles { get; set; }
        //public DbSet<Tag> Tags { get; set; }
        public DbSet<Comment> Comments { get; set; }

        //public DbSet<FlightTicket> FlightTickets { get; set; }
        //public DbSet<Aircraft> Aircrafts { get; set; }
        //public DbSet<Passenger> Passengers { get; set; }    
        //public DbSet<Order> Orders { get; set; }
        //public DbSet<OrderItem> OrderItems { get; set; }
        //public DbSet<ShoppingCartItem> ShoppingCartItems { get; set; }
        //public DbSet<Airport> Airports { get; set; }
        //public DbSet<ConversationData> ConversationData { get; set; }
        //public DbSet<Contact> Contacts { get; set; }

        //public DbSet<BusTicket> BusTickets { get; set; }

    }
}