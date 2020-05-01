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
        private readonly int _userId;
        public EntryService(int patientId)
        {
            _userId = patientId;
        }

        public bool CreateEntry(EntryCreate model)  //Create method
        {
            var entity =
                new Entry()
                { 
                    PatientId = _userId,
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
                    .Where(e => e.PatientId == _userId)
                    .Select(
                        e =>
                        new EntryListItem
                        {
                            PatientId = e.PatientId,
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
                    .Single(e => e.EntryId == id);
                return
                    new EntryDetail
                    {
                        EntryId = entity.EntryId,
                        PatientId = entity.PatientId,
                        BloodSugarReading = entity.BloodSugarReading,
                        CreatedUtc = entity.CreatedUtc,
                        FirstName = entity.Patient.FirstName,
                        LastName= entity.Patient.LastName,
                        ModifiedUtc = entity.ModifiedUtc
                    };
            }
        }
        public bool UpdateEntry(EntryEdit model)
        {
            using (var ctx = new ApplicationDbContext())
            {
                var entity =
                    ctx
                    .Entries
                    .Single(e => e.EntryId == model.EntryId);
                entity.BloodSugarReading = model.BloodSugarReading;
                entity.ModifiedUtc = DateTimeOffset.UtcNow;

                return ctx.SaveChanges() == 1;
            }
        }
        public bool DeleteEntry(int entryId)
        {
            using (var ctx = new ApplicationDbContext())
            {
                var entity =
                    ctx
                    .Entries
                    .Single(e => e.EntryId == entryId);
                ctx.Entries.Remove(entity);
                return ctx.SaveChanges() == 1;
            }
        }
        public List<int> GetListOfBloodSugarLevels()
        {
            List<int> ListOfBloodSugarLevels = new List<int>();

            IEnumerable<EntryListItem> ListOfEntries = GetEntries();

            foreach (EntryListItem reading in ListOfEntries)
            {
                ListOfBloodSugarLevels.Add(reading.BloodSugarReading);
            }
            return ListOfBloodSugarLevels;

        }
        public EntryStatistics GetMinMaxAvg(List<int> bSReadings)
        {
            
            EntryStatistics Model = new EntryStatistics();
            string[,] MinMaxAvg = new string[3, 2] { { "Max", " " }, { "Min", " " }, { "Average", "" } };
            string max = bSReadings.Max().ToString();
            MinMaxAvg[0, 1] = max;
            string min = bSReadings.Min().ToString();
            MinMaxAvg[1, 1] = min;
            string avg = bSReadings.Average().ToString();
            MinMaxAvg[2, 1] = avg;
            Model.MinMaxAvg = MinMaxAvg;
            return Model;

            //string count = bSReadings.Count().ToString();
        }
        public List<int> GetListOfBloodSugarByDate(DateTime Start, DateTime End)
        {
            List<int> ListOfBloodSugarLevels = new List<int>();

            IEnumerable<EntryListItem> ListOfEntries = GetEntries();

            foreach(EntryListItem reading in ListOfEntries)
            {
                if(reading.CreatedUtc.Date >= Start && reading.CreatedUtc.Date <= End)
                {
                    ListOfBloodSugarLevels.Add(reading.BloodSugarReading);
                }
            }
            return ListOfBloodSugarLevels;
        }
    }
}
