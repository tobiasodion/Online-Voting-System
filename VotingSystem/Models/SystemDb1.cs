using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace VotingSystem.Models
{
    public class SystemDb1 : DbContext
    {
        public SystemDb1() : base("name=DefaultConnection")
        {

        }
        public DbSet<Institution> Institutions { get; set; }
        public DbSet<Administrator> Administrators { get; set; }
        public DbSet<Observer> Observers { get; set; }
        public DbSet<Member> Members { get; set; }
        public DbSet<Voter> Voters { get; set; }
        public DbSet<Candidate> Candidates { get; set; }
        public DbSet<Election> Elections{ get; set; }
        public DbSet<Position> Positions { get; set; }
        public DbSet<Ballot> Ballots { get; set; }
    }
}