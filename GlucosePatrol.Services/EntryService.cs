﻿using GlucosePatrol.Data;
using GlucosePatrol.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GlucosePatrol.Services
{
    public class EntryService
    {
        private readonly Guid _userId;
        public EntryService(Guid userId)
        {
            _userId = userId;
        }

        public bool CreateEntry(EntryCreate model)  //Create method
        {
            var entity =
                new Entry()
                {
                    OwnerId = _userId,
                    BloodSugarReading = model.BloodSugarReading,
                    CreatedUtc = DateTimeOffset.Now
                };
            using (var ctx = new ApplicationDbContext())
            {
                ctx.Entries.Add(entity);
                return ctx.SaveChanges() == 1;
            }
        }

        public IEnumerable<EntryListItem> GetEntries()
        {
            using (var ctx = new ApplicationDbContext())
            {
                var query =
                    ctx
                    .Entries
                    .Where(e => e.OwnerId == _userId)
                    .Select(
                        e =>
                        new EntryListItem
                        {
                            EntryId = e.EntryId,
                            BloodSugarReading = e.BloodSugarReading,
                            CreatedUtc = e.CreatedUtc
                        }
                     );
                return query.ToArray();
            }
        }
        public EntryDetail GetEntryById(int id)
        {
            using (var ctx = new ApplicationDbContext())
            {
                var entity =
                    ctx
                    .Entries
                    .Single(e => e.EntryId == id && e.OwnerId == _userId);
                return
                    new EntryDetail
                    {
                        EntryId = entity.EntryId,
                        BloodSugarReading = entity.BloodSugarReading,
                        CreatedUtc = entity.CreatedUtc,
                        ModifiedUtc = entity.ModifiedUtc
                    };
            }
        }
        public bool UpdateEntry(EntryEdit model)
        {
            using(var ctx = new ApplicationDbContext())
            {
                var entity =
                    ctx
                    .Entries
                    .Single(e => e.EntryId == model.EntryId && e.OwnerId == _userId);
                entity.BloodSugarReading = model.BloodSugarReading;
                entity.ModifiedUtc = DateTimeOffset.UtcNow;

                return ctx.SaveChanges() == 1;
            }
        }
        public bool DeleteEntry(int entryId)
        {
            using(var ctx = new ApplicationDbContext())
            {
                var entity =
                    ctx
                    .Entries
                    .Single(e => e.EntryId == entryId && e.OwnerId == _userId);
                ctx.Entries.Remove(entity);
                return ctx.SaveChanges() == 1;
            }
        }
    }
}
