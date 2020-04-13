using CFS.Domain.Aggregates.FacilityAggregate;
using CFS.Domain.SeedWork;
using System;
using System.Text;

namespace CFS.Infrastructure.Repositories
{
    public class FacilityRepository : IFacilityRepository
    {
        private readonly DataContext _context;
        public IUnitOfWork UnitOfWork => _context;

        public FacilityRepository(DataContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public void Add(Facility facility)
        {
            StringBuilder sql = new StringBuilder();
            sql.AppendLine("INSERT INTO CFS.Facilities (street, city, state, country, zipCode)");
            sql.AppendLine(string.Format("VALUES ('{0}', '{1}', '{2}', '{3}', '{4}')", 
                facility.Address.Street,
                facility.Address.City,
                facility.Address.State,
                facility.Address.Country,
                facility.Address.ZipCode));

            _context.Execute(sql.ToString());
        }

        public void Update(Facility facility)
        {
            StringBuilder sql = new StringBuilder();
            sql.AppendLine("UPDATE CFS.Facilities SET");
            sql.AppendLine($"street = '{facility.Address.Street}'");
            sql.AppendLine($"city = '{facility.Address.City}'");
            sql.AppendLine($"state = '{facility.Address.State}'");
            sql.AppendLine($"country = '{facility.Address.Country}'");
            sql.AppendLine($"zipCode = '{facility.Address.ZipCode}'");

            sql.AppendLine($"WHERE facilityId = {facility.Id}");

            _context.Execute(sql.ToString());
        }

        public Facility GetFacility(int facilityId)
        {
            var sql = $"SELECT * FROM CFS.Facilities WHERE facilityId = {facilityId}";
            return _context.Query<Facility>(sql);
        }
    }
}
